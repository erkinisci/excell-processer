using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using Excell.Processor.Core;
using Matriks.Oms.EnterpriseLibrary;
using Matriks.Oms.EnterpriseLibrary.Common;
using Matriks.Oms.EnterpriseLibrary.Windows;

namespace Excell.Processor.Models
{
  public class ExeFileCreator
  {
    public const string ResourceString = "Matriks.ClientAPI.Setup.ExeFiles";
    public MatriksClientApiSetupModel MatriksClientApiSetup { get; set; }

    public ISetupLogger SetupLogger { get; set; }


    public ExeFileCreator()
    {
      MatriksClientApiSetup =
        DependencyContainer.Resolver.GetService<MatriksClientApiSetupModel>("MatriksClientApiSetupModel");
      SetupLogger = DependencyContainer.Resolver.GetService<ISetupLogger>("SetupLogger");
    }

    private Stream GetFileStream(string zipName)
    {
      return Assembly.GetEntryAssembly().GetManifestResourceStream($"{ResourceString}.{zipName}");
    }

    public bool ExtractFilesFromEmbeddedZip()
    {
      SetupLogger.WriteInfoLog("Zip extract islemlerine baslaniyor.");

      var error = false;
      try
      {
        SetupLogger.WriteInfoLog(MatriksClientApiSetup.MainFolderPath + " klasor konumu kontrol ediliyor.");

        var isExist = CheckDirectory(MatriksClientApiSetup.MainFolderPath);
        if (!isExist)
        {
          SetupLogger.WriteInfoLog(MatriksClientApiSetup.MainFolderPath + " klasor bulunamadi. Olusturuluyor...");

          error = !CreateDirectory(MatriksClientApiSetup.MainFolderPath);
        }

        if (error)
        {
          SetupLogger.WriteInfoLog(MatriksClientApiSetup.MainFolderPath + " klasoru olusturulamadi.");
          return error;
        }
        SetupLogger.WriteInfoLog(MatriksClientApiSetup.MainFolderPath + " klasor bulunamadi. Olusturuldu.");

        UninstallServices();
        error = !CleanFiles();
        if (error)
        {
          SetupLogger.WriteErrorLog("Yukleme islemi iptal edildi!");
          return error;
        }

        foreach (var appInfo in from a in MatriksClientApiSetupModel.Apps where a.IsSetup select a)
        {
          var subFolderName = Path.Combine(MatriksClientApiSetup.MainFolderPath);

          var filePath = Path.Combine(MatriksClientApiSetup.MainFolderPath, appInfo.ZipName);
          if (File.Exists(filePath))
            DeleteFiles(subFolderName);
          isExist = CheckDirectory(Path.Combine(MatriksClientApiSetup.MainFolderPath, appInfo.FolderName));
          if (isExist)
            Directory.Delete(Path.Combine(MatriksClientApiSetup.MainFolderPath, appInfo.FolderName), true);
          error = !ZipToFile(appInfo, filePath);
          if (error)
          {
            SetupLogger.WriteInfoLog(appInfo.ZipName +
                                     " isimli zip dosyasi extract islemi esnasinda hata olustu. Yukleme Iptal ediliyor..");
            break;
          }

          ZipFile.ExtractToDirectory(filePath, subFolderName);

          try
          {
            File.Delete(filePath);
          }
          catch (Exception)
          {
            SetupLogger.WriteErrorLog(filePath + " dosyasi silme hatasi. Yukleme duzgun calismayabilir.");
            error = true;
          }
        }
      }
      catch (Exception ex)
      {
        SetupLogger.WriteErrorLog("Yukeleme tamamlanamadi. Hata:" + ex.StackTrace);

        error = true;
      }

      return error;
    }

    private bool ZipToFile(AppInfo appInfo, string filePath)
    {
      var zipStream = GetFileStream(appInfo.ZipName);
      if (zipStream == null)
        return false;

      using (Stream outStream = File.Create(filePath))
      {
        try
        {
          CopyStream(zipStream, outStream);
        }
        catch (Exception)
        {
          // logla
          return false;
        }
      }

      return true;
    }

    private bool CleanFiles()
    {
      try
      {
        foreach (var appInfo in MatriksClientApiSetupModel.Apps)
        {
          var subFolderPath = Path.Combine(MatriksClientApiSetup.MainFolderPath, appInfo.FolderName);

          SetupLogger.WriteInfoLog(subFolderPath + " klasoru konumu kontrol ediliyor...");
          if (!CheckDirectory(subFolderPath)) continue;

          DeleteFiles(subFolderPath);
          SetupLogger.WriteInfoLog(subFolderPath + " klasor icerikleri siliniyor...");
        }

        return true;
      }
      catch (Exception ex)
      {
        SetupLogger.WriteErrorLog("ExeFileCreator icerisinde CleanFiles islemleri sirasinda hata olustu! hata:" +
                                  ex.StackTrace);
        return false;
      }
    }

    private static void DeleteFiles(string subFolderPath)
    {
      var files = Directory.GetFiles(subFolderPath);
      foreach (var filePath in files)
        File.Delete(filePath);
    }

    private bool CreateDirectory(string directoryName)
    {
      try
      {
        Directory.CreateDirectory(directoryName);
        return true;
      }
      catch (Exception ex)
      {
        // logla          
        return false;
      }
    }

    private bool CheckDirectory(string directoryName)
    {
      return Directory.Exists(directoryName);
    }

    public void RunApplication()
    {
      try
      {
        foreach (var appInfo in from a in MatriksClientApiSetupModel.Apps where a.IsSetup && !a.IsWindowsService select a)
        {
          var exeFilePath = Path.Combine(MatriksClientApiSetup.MainFolderPath, appInfo.FolderName, appInfo.ExeName);
          var result = appInfo.Arguments != null
            ? Process.Start(exeFilePath, appInfo.Arguments)
            : Process.Start(exeFilePath);

          SetupLogger.WriteInfoLog(appInfo.ExeName + " uygulamasi baslatildi.");
        }
      }
      catch (Exception ex)
      {
        SetupLogger.WriteErrorLog("hata :" + ex.StackTrace);
      }
    }

    public void RunServices()
    {
      try
      {
        foreach (var appInfo in from a in MatriksClientApiSetupModel.Apps where a.IsSetup && a.IsWindowsService select a)
        {
          var exeFilePath = Path.Combine(MatriksClientApiSetup.MainFolderPath, appInfo.FolderName, appInfo.ExeName);

          var result = appInfo.Arguments != null
            ? Process.Start(exeFilePath, appInfo.Arguments)
            : Process.Start(exeFilePath);

          if (appInfo.IsWindowsService)
            result?.WaitForExit();

          SetupLogger.WriteInfoLog(appInfo.ExeName + " servisi install edildi.");
        }

      }
      catch (Exception ex)
      {
        SetupLogger.WriteErrorLog("Hata : " + ex.StackTrace);
      }
    }

    public void UninstallServices()
    {
      foreach (var appInfo in from a in MatriksClientApiSetupModel.Apps where a.IsSetup && a.IsWindowsService select a)
      {
        var exeFilePath = Path.Combine(MatriksClientApiSetup.MainFolderPath, appInfo.FolderName, appInfo.ExeName);

        var isRunning = WindowsServiceIsRunning(appInfo.ServiceName);

        if (isRunning)
        {
          var result = Process.Start(exeFilePath, " /uninstall /nomsg");

          if (appInfo.IsWindowsService)
            result?.WaitForExit();

          SetupLogger.WriteInfoLog(appInfo.ExeName + " servisi uninstall edildi.");
        }
      }
    }

    public bool GetProcessPath(string name)
    {
      Process[] processes = Process.GetProcessesByName(name);

      if (processes.Length > 0)
      {
        return true;
      }
      else
      {
        return false;
      }
    }

    public bool WindowsServiceIsRunning(string serviceName)
    {
      try
      {
        var sc = new ExtendedService(serviceName);

        sc.Stop();
      }
      catch (Exception)
      {
        return false;
      }

      return true;
    }

    public static void CopyStream(Stream input, Stream output)
    {
      byte[] buffer = new byte[8 * 1024];
      int len;
      while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
      {
        output.Write(buffer, 0, len);
      }
    }


  }
}

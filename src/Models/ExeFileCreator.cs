using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Matriks.Oms.EnterpriseLibrary;
using Matriks.Oms.EnterpriseLibrary.Common;
using Matriks.Oms.EnterpriseLibrary.Configuration;

namespace Matriks.ClientAPI.Setup.Models
{
  public class ExeFileCreator
  {
    public const string ResourceString = "Matriks.ClientAPI.Setup.ExeFiles";
    public MatriksClientApiSetupModel MatriksClientApiSetup { get; set; }

    public ExeFileCreator()
    {
      MatriksClientApiSetup =
        DependencyContainer.Resolver.GetService<MatriksClientApiSetupModel>("MatriksClientApiSetupModel");
    }

    private Stream GetFileStream(string zipName)
    {
      return Assembly.GetEntryAssembly().GetManifestResourceStream($"{ResourceString}.{zipName}");
    }

    public bool ExtractFilesFromEmbeddedZip()
    {
      var error = false;
      try
      {
        bool isExist = CheckDirectory(MatriksClientApiSetup.MainFolderPath);
        if (!isExist)
          error = !CreateDirectory(MatriksClientApiSetup.MainFolderPath);

        if (error)
        {
          // logla
          return error;
        }

        error = !CleanFiles();
        if (error)
        {
          // logla
          return error;
        }

        foreach (var appInfo in from a in MatriksClientApiSetup.Apps where a.IsSetup select a)
        {
          var subFolderName = Path.Combine(MatriksClientApiSetup.MainFolderPath);

          //isExist = CheckDirectory(subFolderName);
          //if (!isExist)
          //  error = !CreateDirectory(subFolderName);

          //if (error)
          //{
          //  //logla 
          //  break;
          //}

          var filePath = Path.Combine(MatriksClientApiSetup.MainFolderPath, appInfo.ZipName);
          if (File.Exists(filePath))
            DeleteFiles(subFolderName);
          isExist = CheckDirectory(Path.Combine(MatriksClientApiSetup.MainFolderPath, appInfo.FolderName));
          if (isExist)
            Directory.Delete(Path.Combine(MatriksClientApiSetup.MainFolderPath, appInfo.FolderName), true);
          error = !ZipToFile(appInfo, filePath);
          if (error)
          {
            //logla 
            break;
          }

          ZipFile.ExtractToDirectory(filePath, subFolderName);
          //using (var archive = ZipFile.OpenRead(filePath))
          //{
          //  foreach (var entry in archive.Entries)
          //  {
          //   // Thread.Sleep(200);
          //    if (entry.Length > 0)
          //    {
          //      var fileName = Path.Combine(subFolderName, entry.Name);
          //      if (File.Exists(fileName) || (entry.FullName.EndsWith(".pdb", StringComparison.OrdinalIgnoreCase)))
          //        continue;

          //      entry.ExtractToFile(fileName);
          //    }
          //  }
          //}

          try
          {
            File.Delete(filePath);
            //Directory.Delete(Path.Combine(subFolderName, appInfo.FolderName), true);
          }
          catch (Exception)
          {
            // logla
            error = false;
          }
        }
      }
      catch (Exception ex)
      {
        // logla
        error = false;
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
        foreach (var appInfo in MatriksClientApiSetup.Apps)
        {
          var subFolderPath = Path.Combine(MatriksClientApiSetup.MainFolderPath, appInfo.FolderName);
          if (!CheckDirectory(subFolderPath)) continue;

          DeleteFiles(subFolderPath);
        }

        return true;
      }
      catch (Exception)
      {
        //logla
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
      Process process = new Process();
      foreach (var appInfo in from a in MatriksClientApiSetup.Apps where a.IsSetup select a)
      {
        var exeFilePath = Path.Combine(MatriksClientApiSetup.MainFolderPath, appInfo.FolderName, appInfo.ExeName);


        process.StartInfo.FileName = exeFilePath;
        if (appInfo.Arguments != null)
          process.StartInfo.Arguments = appInfo.Arguments;
        process.Start();
      }
      //_finalfilePath = Path.Combine(_destinationFolder, _clientApiExeFileName);
      //var result = System.Diagnostics.Process.Start(_finalfilePath);
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

using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Matriks.Oms.EnterpriseLibrary;
using Matriks.Oms.EnterpriseLibrary.Common;
using Matriks.Oms.EnterpriseLibrary.Configuration;

namespace Matriks.ClientAPI.Setup.Models
{
  public class ExeFileCreator
  {
    private static IAppSettings _appSettings;
    private string _zipfileName;
    private string _destinationFolder;
    private string _clientApiExeFileName;

    private string _finalfilePath;

    public ExeFileCreator()
    {
      _appSettings = DependencyContainer.Resolver.GetService<IAppSettings>("UISettings");
      _zipfileName = _appSettings.GetString("ClientApiZipFileName");

      _clientApiExeFileName = _appSettings.GetString("ClientApiExeFileName");
      _destinationFolder = App.FileOutputPath;
    }

    public void ExtractFilesFromEmbeddedZip()
    {
      try
      {
        Stream zipStream = Assembly.GetEntryAssembly().GetManifestResourceStream("Matriks.ClientAPI.Setup.ExeFiles." + _zipfileName);

        if (!Directory.Exists(_destinationFolder))
        {
          Directory.CreateDirectory(_destinationFolder);
        }
        else
        {
          var filePaths = Directory.GetFiles(_destinationFolder);
          foreach (var filePath in filePaths)
            File.Delete(filePath);
        }

        if (zipStream != null)
        {
          _finalfilePath = Path.Combine(_destinationFolder, _zipfileName);
          using (Stream outStream = File.Create(_finalfilePath))
          {
            CopyStream(zipStream, outStream);
          }

          ZipFile.ExtractToDirectory(_finalfilePath, _destinationFolder);

          using (var archive = ZipFile.OpenRead(_finalfilePath))
          {
            foreach (var entry in archive.Entries)
            {
              Thread.Sleep(1000);
              if (entry.Length > 0)
              {
                var fileName = Path.Combine(_destinationFolder, entry.Name);
                if (File.Exists(fileName) || (entry.FullName.EndsWith(".pdb", StringComparison.OrdinalIgnoreCase)))
                  continue;

                entry.ExtractToFile(fileName);
              }
            }
          }

          File.Delete(Path.Combine(_destinationFolder, _zipfileName));
        }
      }
      catch (Exception exception)
      {

      }
    }

    public void RunApplication()
    {
      _finalfilePath = Path.Combine(_destinationFolder, _clientApiExeFileName);
      var result = System.Diagnostics.Process.Start(_finalfilePath);
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

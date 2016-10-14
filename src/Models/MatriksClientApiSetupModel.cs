using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Matriks.ClientAPI.Setup.Annotations;

namespace Matriks.ClientAPI.Setup.Models
{
  public class MatriksClientApiSetupModel : INotifyPropertyChanged
  {
    private string _mainFolderPath = @"C:\MATRIKS_OMS";

    public string MainFolderPath
    {
      get { return _mainFolderPath; }
      set { _mainFolderPath = value; OnPropertyChanged(nameof(MainFolderPath)); }
    }

    public List<AppInfo> Apps { get; set; }

    public MatriksClientApiSetupModel()
    {
      Apps = new List<AppInfo>()
      {
          new AppInfo() { ZipName = "ClientAPIServer.zip", ExeName = "clientserver.exe", FolderName = "ClientAPI Server", Args = new []{ "install", "nomessage"}, IsWindowsService = true}
          , new AppInfo() { ZipName = "ClientAPIPriceUpdater.zip", ExeName = "priceUpdater.exe", FolderName = "ClientAPI Price Updater", Args = new []{ "install", "nomessage"}, IsWindowsService = true}
          , new AppInfo() { ZipName = "ClientAPIPriceServer.zip", ExeName = "priceServer.exe", FolderName = "ClientAPI Price Server", Args = new []{ "install", "nomessage"}, IsWindowsService = true}
          , new AppInfo() { ZipName = "ClientAPIMonitor.zip", ExeName = "clientapimanager.exe", FolderName = "ClientAPI Monitor", IsWindowsService = false}
      };
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}

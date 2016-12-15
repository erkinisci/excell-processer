using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Excell.Processor.Properties;

namespace Excell.Processor.Models
{
  public class ExcellProcessorSetupModel : INotifyPropertyChanged
  {
    private string _mainFolderPath = @"C:\MATRIKS_OMS";

    public string MainFolderPath
    {
      get { return _mainFolderPath; }
      set { _mainFolderPath = value; OnPropertyChanged(nameof(MainFolderPath)); }
    }

    public static List<AppInfo> Apps { get; set; }

    public ExcellProcessorSetupModel()
    {
      Apps = new List<AppInfo>()
      {
          new AppInfo() { ZipName = "ClientAPI Server.zip", ExeName = "clientserver.exe", ServiceName = "ClientApiService", FolderName = "ClientAPI Server", Args = new []{ "/install /nomsg"}, IsWindowsService = true, IsSetup = true}
          , new AppInfo() { ZipName = "ClientAPI PriceUpdater.zip", ExeName = "priceUpdater.exe", ServiceName  = "PriceDbUpdater",FolderName = "ClientAPI PriceUpdater", Args = new []{ "/install /nomsg"}, IsWindowsService = true, IsSetup = true}
          , new AppInfo() { ZipName = "ClientAPI PriceServer.zip", ExeName = "priceServer.exe", ServiceName = "PriceApiServer", FolderName = "ClientAPI PriceServer", Args = new []{ "/install /nomsg"}, IsWindowsService = true, IsSetup = true}
          ,new AppInfo() { ZipName = "ClientAPI Monitor.zip", ShortCutName = "ClientAPI Monitor" ,ExeName = "clientapimanager.exe", FolderName = "ClientAPI Monitor", IsWindowsService = false, IsSetup = true}
      };
    }

    public static AppInfo FindApp(string exeName)
    {
      var appInfo = from app in Apps where app.ExeName == exeName || app.ExeName.Contains(exeName) select app;
      return appInfo.FirstOrDefault();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}

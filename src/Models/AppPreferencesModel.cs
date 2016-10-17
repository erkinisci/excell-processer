using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IWshRuntimeLibrary;

namespace Matriks.ClientAPI.Setup.Models
{
  public class AppPreferencesModel
  {
    public static  void CreateShortcut()
    {
      var app = MatriksClientApiSetupModel.FindApp("clientapimanager");
      if(app == null)
        return;
      var mts = new MatriksClientApiSetupModel();
      var shDesktop = (object)"Desktop";
      var shell = new WshShell();
      string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + @"\ClientAPI Manager.lnk";
      IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
      shortcut.Description = "ClientAPI Manager Uygulaması";

      shortcut.TargetPath = Path.Combine(mts.MainFolderPath, app.FolderName,app.ExeName);
      shortcut.Save();
    }
  }
}

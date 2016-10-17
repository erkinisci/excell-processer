using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
//using IWshRuntimeLibrary;

namespace Matriks.ClientAPI.Setup.Models
{
  public class AppPreferencesModel
  {
    public static  void CreateShortcut()
    {
      var appInfo = MatriksClientApiSetupModel.FindApp("clientapimanager");
      if (appInfo == null)
        return;
      var mts = new MatriksClientApiSetupModel();

      Type t = Type.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")); //Windows Script Host Shell Object
      dynamic shell = Activator.CreateInstance(t);
      try
      {
        var shDesktop = (object)"Desktop";
        string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + @"\" + appInfo.ShortCutName + ".lnk";
        var lnk = shell.CreateShortcut(shortcutAddress);
        try
        {

          lnk.TargetPath = Path.Combine(mts.MainFolderPath, appInfo.FolderName, appInfo.ExeName);
          lnk.IconLocation = Path.Combine(lnk.TargetPath+ ", 0");
          lnk.Save();
        }
        finally
        {
          Marshal.FinalReleaseComObject(lnk);
        }
      }
      finally
      {
        Marshal.FinalReleaseComObject(shell);
      }    
    }
  }
}

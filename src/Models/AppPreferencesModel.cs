using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Matriks.ClientAPI.Setup.Core;
using Matriks.Oms.EnterpriseLibrary;
using Matriks.Oms.EnterpriseLibrary.Common;

//using IWshRuntimeLibrary;

namespace Matriks.ClientAPI.Setup.Models
{
  public class AppPreferencesModel
  {
    public static ISetupLogger SetupLogger { get; set; }
    public static  void CreateShortcut()
    {

      SetupLogger = DependencyContainer.Resolver.GetService<ISetupLogger>("SetupLogger");

      var appInfo = MatriksClientApiSetupModel.FindApp("clientapimanager");
      if (appInfo == null)
        return;

      var mts = new MatriksClientApiSetupModel();

      SetupLogger.WriteInfoLog("Windows Script Host Shell Object reflection islemleri basliyor.");
      var t = Type.GetTypeFromCLSID(new Guid("72C24DD5-D70A-438B-8A42-98424B88AFB8")); //Windows Script Host Shell Object
      dynamic shell = Activator.CreateInstance(t);
      try
      {
        var shDesktop = (object)"Desktop";
        var shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + @"\" + appInfo.ShortCutName + ".lnk";
        var lnk = shell.CreateShortcut(shortcutAddress);
        try
        {

          lnk.TargetPath = Path.Combine(mts.MainFolderPath, appInfo.FolderName, appInfo.ExeName);
          lnk.IconLocation = Path.Combine(lnk.TargetPath + ", 0");
          lnk.Save();

          SetupLogger.WriteErrorLog("Windows Script Host Shell Object reflection islemlerinde hata olustu. Kisa yol hazirlandi.");
        }
        catch (Exception ex)
        {
          SetupLogger.WriteErrorLog("Windows Script Host Shell Object reflection islemlerinde hata olustu. Kisa yol hazirlanamadi. Hata:" + ex.StackTrace);
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

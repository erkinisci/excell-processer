using System.Windows;
using Matriks.Oms.EnterpriseLibrary;
using Matriks.Oms.EnterpriseLibrary.Common;
using Matriks.Oms.EnterpriseLibrary.Configuration;
using Matriks.Wpf;
using Matriks.Wpf.Framework.Commands;

namespace Matriks.ClientAPI.Setup.ViewModels
{
  public class LoadingPageModel : NavigateObject
  {
    private IAppSettings _appSettings;
    public DelegateCommand LoadCommand { get; set; }

    public DelegateCommand ReturnCommand { get; set; }


    public override void OnLoaded(FrameworkElement view)
    {
      base.OnLoaded(view);

      _appSettings = DependencyContainer.Resolver.GetService<IAppSettings>("UISettings");

      LoadCommand = new DelegateCommand(OnLoadCommand);
      ReturnCommand = new DelegateCommand(OnReturnCommand);
    }

    private void OnReturnCommand()
    {
      App.MenuListBoxSelection(-1);
    }

    private void OnLoadCommand()
    {
      
      var clientApiFileName = _appSettings.GetString("ClientApiExeFileName");


      App.MenuListBoxSelection(1);
    }
  }
}
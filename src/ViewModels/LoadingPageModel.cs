using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Forms;
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

      LoadCommand = new DelegateCommand(OnLoadCommand);
      ReturnCommand = new DelegateCommand(OnReturnCommand);
    }

    private void OnReturnCommand()
    {
      App.LayoutRoot.GoBack();
    }

    private void OnLoadCommand()
    {
      App.Navigate("/views/complatedpage.xaml");
    }
  }
}
using System.Windows;
using Matriks.Wpf;
using Matriks.Wpf.Framework.Commands;

namespace Matriks.ClientAPI.Setup.ViewModels
{
  public class LoadingPageModel : NavigateObject
  {
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
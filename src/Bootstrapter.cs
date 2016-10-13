using Matriks.ClientAPI.Setup.ViewModels;
using Matriks.ClientAPI.Setup.Views;
using Matriks.Wpf;
using Matriks.Wpf.Framework;

namespace Matriks.ClientAPI.Setup
{
  public class Bootstrapter : MonitorBootstrapter
  {
    public void Initialize()
    {
      NavigationService.RegisterViewModel("/Views/FirstPage.xaml", typeof(FirstPageModel));
      NavigationService.RegisterViewModel("/Views/LoadingPage.xaml", typeof(LoadingPageModel));
      NavigationService.RegisterViewModel("/Views/ComplatedPage.xaml", typeof(LoadingPageModel));
    }
  }
}
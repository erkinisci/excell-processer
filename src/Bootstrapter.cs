using Excell.Processor.ViewModels;
using Matriks.Wpf;
using Matriks.Wpf.Framework;

namespace Excell.Processor
{
  public class Bootstrapter : MonitorBootstrapter
  {
    public void Initialize()
    {
      NavigationService.RegisterViewModel("/Views/FirstPage.xaml", typeof(FirstPageModel));
      NavigationService.RegisterViewModel("/Views/ApplicationChoosePage.xaml", typeof(ApplicationChoosePageModel));
      NavigationService.RegisterViewModel("/Views/ColumnChoosePage.xaml", typeof(ColumnChoosePageModel));
      NavigationService.RegisterViewModel("/Views/LoadingPage.xaml", typeof(LoadingPageModel));
      NavigationService.RegisterViewModel("/Views/ComplatedPage.xaml", typeof(ComplatedPageModel));
      NavigationService.RegisterViewModel("/Views/ErrorPage.xaml", typeof(ErrorPageModel));

      
    }
  }
}
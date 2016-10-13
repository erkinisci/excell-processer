using Matriks.ClientAPI.UI.ViewModels;
using Matriks.Wpf;
using Matriks.Wpf.Framework;

namespace Matriks.ClientAPI.UI
{
  public class Bootstrapter : MonitorBootstrapter
  {
    public void Initialize()
    {
      NavigationService.RegisterViewModel("/Views/ClientServerView.xaml", typeof(ClientServerViewModel));
    }
  }
}
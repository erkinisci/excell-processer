using Matriks.ClientAPI.Setup.ViewModels;
using Matriks.Wpf;
using Matriks.Wpf.Framework;

namespace Matriks.ClientAPI.Setup
{
  public class Bootstrapter : MonitorBootstrapter
  {
    public void Initialize()
    {
      NavigationService.RegisterViewModel("/Views/ClientServerView.xaml", typeof(ClientServerViewModel));
    }
  }
}
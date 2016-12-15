using System.Linq;
using System.Windows;
using Excell.Processor.Models;
using Matriks.Oms.EnterpriseLibrary;
using Matriks.Oms.EnterpriseLibrary.Common;
using Matriks.Wpf.Framework.Commands;

namespace Excell.Processor.ViewModels
{
  public class ComplatedPageModel : SetupMainPageModel
  {
    public DelegateCommand LastCommand { get; set; }

    public FileSingletonModel FileSingletonModel { get; set; }

    public override void OnLoaded(FrameworkElement view)
    {
      base.OnLoaded(view);

      LastCommand = new DelegateCommand(OnLastCommand);
      FileSingletonModel = DependencyContainer.Resolver.GetService<FileSingletonModel>();
    }

    private void OnLastCommand()
    {
      if (IsApplicationStart)
      {
      }

      Application.Current.Shutdown();
    }

    public bool IsApplicationStart
    {
      get { return (bool)GetValue(IsApplicationStartProperty); }
      set { SetValue(IsApplicationStartProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IsApplicationStartProperty =
      DependencyProperty.Register("IsApplicationStart", typeof(bool), typeof(ComplatedPageModel));
  }
}
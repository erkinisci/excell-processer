using System.Windows;
using Matriks.ClientAPI.Setup.Models;
using Matriks.Oms.EnterpriseLibrary;
using Matriks.Oms.EnterpriseLibrary.Common;
using Matriks.Wpf;
using Matriks.Wpf.Framework.Commands;

namespace Matriks.ClientAPI.Setup.ViewModels
{
  public class SetupMainPageModel : NavigateObject
  {
    public DelegateCommand ReturnCommand { get; set; }

    public DelegateCommand NextPageCommand { get; set; }

    public DelegateCommand CancelCommand { get; set; }

    public new DelegateCommand CloseCommand { get; set; }

    
    public override void OnLoaded(FrameworkElement view)
    {
      base.OnLoaded(view);

      MatriksClientApiSetup = DependencyContainer.Resolver.GetService<MatriksClientApiSetupModel>("MatriksClientApiSetupModel");

      ReturnCommand = new DelegateCommand(OnReturnCommand);
      NextPageCommand = new DelegateCommand(OnNextPageCommand);
      CancelCommand = new DelegateCommand(App.GlobalCancelCommand);
      CloseCommand = new DelegateCommand(OnCloseCommand);
    }

    private void OnCloseCommand()
    {
      Application.Current.Shutdown();
    }

    public virtual void OnNextPageCommand()
    {
      App.MenuListBoxSelection(1);
    }

    public virtual void OnReturnCommand()
    {
      App.MenuListBoxSelection(-1);
    }

    public MatriksClientApiSetupModel MatriksClientApiSetup
    {
      get { return (MatriksClientApiSetupModel)GetValue(MatriksClientApiSetupProperty); }
      set { SetValue(MatriksClientApiSetupProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Menus.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty MatriksClientApiSetupProperty = DependencyProperty.Register("MatriksClientApiSetup", typeof(MatriksClientApiSetupModel), typeof(SetupMainPageModel));
  }
}
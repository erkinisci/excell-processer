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

    public DelegateCommand CancelCommand { get; set; }

    public bool IsLoading { get; private set; }

    public override void OnLoaded(FrameworkElement view)
    {
      base.OnLoaded(view);

      _appSettings = DependencyContainer.Resolver.GetService<IAppSettings>("UISettings");
      ProgressBarVisibility=Visibility.Collapsed;

      LoadCommand = new DelegateCommand(OnLoadCommand);
      ReturnCommand = new DelegateCommand(OnReturnCommand);
      CancelCommand = new DelegateCommand(App.GlobalCancelCommand);

      IsLoading = false;
    }

    private void OnReturnCommand()
    {
      App.MenuListBoxSelection(-1);
    }

    private void OnLoadCommand()
    {
      if(IsLoading)
        return;

      ProgressBarVisibility = Visibility.Visible;
      var clientApiFileName = _appSettings.GetString("ClientApiExeFileName");

      IsLoading = true;
      //App.MenuListBoxSelection(1);
    }

    public Visibility ProgressBarVisibility
    {
      get { return (Visibility)GetValue(ProgressBarVisibilityProperty); }
      set { SetValue(ProgressBarVisibilityProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ProgressBarVisibilityProperty = DependencyProperty.Register("ProgressBarVisibility", typeof(Visibility), typeof(LoadingPageModel));


  }
}
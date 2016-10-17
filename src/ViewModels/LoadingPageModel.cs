using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using Matriks.ClientAPI.Setup.Models;
using Matriks.Wpf.Framework;
using Matriks.Wpf.Framework.Commands;

namespace Matriks.ClientAPI.Setup.ViewModels
{
  public class LoadingPageModel : SetupMainPageModel
  {
    public DelegateCommand LoadCommand { get; set; }

    public DelegateCommand LastCommand { get; set; }

    public bool IsLoading { get; private set; }

    private ExeFileCreator ExeFileCreator { get; set; }

    public override void OnLoaded(FrameworkElement view)
    {
      base.OnLoaded(view);

      ProgressBarVisibility = Visibility.Collapsed;

      LoadCommand = new DelegateCommand(OnLoadCommand);
      LastCommand = new DelegateCommand(OnLastCommand);

      ExeFileCreator = new ExeFileCreator();

      IsLoading = false;
    }
    
    private void OnLastCommand()
    {
      if (IsApplicationStart)
        ExeFileCreator.RunApplication();

      Application.Current.Shutdown();
    }

    private void OnLoadCommand()
    {
      if (IsLoading)
        return;

      ProgressBarVisibility = Visibility.Visible;

      BackgroundWorker bw = new BackgroundWorker();
      bw.DoWork += Bw_DoWork;
      bw.RunWorkerCompleted += Bw_RunWorkerCompleted;

      IsLoading = true;
      bw.RunWorkerAsync();
    }

    private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      var result = e.Result as bool? ?? false;
      var bw = sender as BackgroundWorker;
      bw.DoWork -= Bw_DoWork;
      bw.RunWorkerCompleted -= Bw_RunWorkerCompleted;
      bw?.Dispose();

      IsLoading = false;
      if (!result)
      {
        AppPreferencesModel.CreateShortcut();
        SetupLogger.WriteInfoLog("Servislerin calisma durumu kontrol ediliyor...");
        ExeFileCreator.UninstallServices();
        ExeFileCreator.RunServices();
        SetupLogger.WriteInfoLog("Servislerin calisma durumu kontrolu tamamlandi.");

        Dispatcher.DoInvoke(() => { App.MenuListBoxSelection(1); }, DispatcherPriority.Send);
      }
      else
      {
        Dispatcher.DoInvoke(() => { App.Navigate("/views/errorpage.xaml"); }, DispatcherPriority.Send);
      }
    }

    private void Bw_DoWork(object sender, DoWorkEventArgs e)
    {
      e.Result = ExeFileCreator.ExtractFilesFromEmbeddedZip();
    }

    public bool IsApplicationStart
    {
      get { return (bool)GetValue(IsApplicationStartProperty); }
      set { SetValue(IsApplicationStartProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IsApplicationStartProperty = DependencyProperty.Register("IsApplicationStart", typeof(bool), typeof(LoadingPageModel));

    public Visibility ProgressBarVisibility
    {
      get { return (Visibility)GetValue(ProgressBarVisibilityProperty); }
      set { SetValue(ProgressBarVisibilityProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ProgressBarVisibilityProperty = DependencyProperty.Register("ProgressBarVisibility", typeof(Visibility), typeof(LoadingPageModel));
  }
}
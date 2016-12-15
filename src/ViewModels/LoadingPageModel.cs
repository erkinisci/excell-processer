using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Excell.Processor.Models;
using Matriks.Oms.EnterpriseLibrary;
using Matriks.Oms.EnterpriseLibrary.Common;
using Matriks.Wpf.Framework;
using Matriks.Wpf.Framework.Commands;

namespace Excell.Processor.ViewModels
{
  public class LoadingPageModel : SetupMainPageModel
  {
    public DelegateCommand LoadCommand { get; set; }

    public DelegateCommand LastCommand { get; set; }

    public bool IsLoading { get; private set; }

    public FileSingletonModel FileSingletonModel { get; set; }

    public override void OnLoaded(FrameworkElement view)
    {
      base.OnLoaded(view);

      LoadCommand = new DelegateCommand(OnLoadCommand);
      LastCommand = new DelegateCommand(OnLastCommand);

      FileSingletonModel = DependencyContainer.Resolver.GetService<FileSingletonModel>();
      var fileList = FileSingletonModel.FileCollection.ToList().Where(x => x.IsSelected);
      var columnList = FileSingletonModel.ColumnCollection.ToList().Where(x => x.IsSelected);

      foreach (var fileItem in fileList)
        FileSingletonModel.ProcessingFileCollection.Add(new FileProcessingItem() { File = fileItem, Columns = columnList, IsSelected = true, ProgressBarVisibility = Visibility.Collapsed });
      //ExcellFileProcess.Instance.GetConentAsTable(FilesingletonModel.FileCollection.First(), FilesingletonModel.ColumnCollection.ToList());

      IsLoading = false;
    }

    public override void OnReturnCommand()
    {
      if (!IsLoading)
        base.OnReturnCommand();
    }

    private void OnLastCommand()
    {
      if (IsApplicationStart)
      {
      }

      Application.Current.Shutdown();
    }

    private void OnLoadCommand()
    {
      if (IsLoading)
        return;

      foreach (var fileProcessingItem in FileSingletonModel.ProcessingFileCollection)
      {
        fileProcessingItem.ProgressBarVisibility = Visibility.Visible;
      }
      return;
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
        SetupLogger.WriteInfoLog("Servislerin calisma durumu kontrol ediliyor...");

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
      e.Result = null;
    }

    public bool IsApplicationStart
    {
      get { return (bool)GetValue(IsApplicationStartProperty); }
      set { SetValue(IsApplicationStartProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IsApplicationStartProperty = DependencyProperty.Register("IsApplicationStart", typeof(bool), typeof(LoadingPageModel));
  }
}
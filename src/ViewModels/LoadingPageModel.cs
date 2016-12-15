using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Excell.Processor.Interfaces;
using Excell.Processor.Models;
using Matriks.Oms.EnterpriseLibrary;
using Matriks.Oms.EnterpriseLibrary.Common;
using Matriks.Oms.EnterpriseLibrary.Resolvers;
using Matriks.Wpf.Framework;
using Matriks.Wpf.Framework.Commands;

namespace Excell.Processor.ViewModels
{
  public class LoadingPageModel : SetupMainPageModel
  {
    public DelegateCommand LoadCommand { get; set; }

    public bool IsLoading { get; private set; }

    public FileSingletonModel FileSingletonModel { get; set; }

    public override void OnLoaded(FrameworkElement view)
    {
      base.OnLoaded(view);

      LoadCommand = new DelegateCommand(OnLoadCommand);

      FileSingletonModel = DependencyContainer.Resolver.GetService<FileSingletonModel>();
      var fileList = FileSingletonModel.FileCollection.ToList().Where(x => x.IsSelected);
      var columnList = FileSingletonModel.ColumnCollection.ToList().Where(x => x.IsSelected);

      foreach (var fileItem in fileList)
        FileSingletonModel.ProcessingFileCollection.Add(new FileProcessingItem()
        {
          File = fileItem,
          Columns = columnList,
          IsSelected = true,
          ProgressBarVisibility = Visibility.Collapsed,
          DonePathVisibility = Visibility.Collapsed
        });

      IsLoading = false;
    }

    public override void OnReturnCommand()
    {
      if (!IsLoading)
        base.OnReturnCommand();
    }

    private void OnLoadCommand()
    {
      if (IsLoading)
        return;

      IsLoading = true;

      foreach (var fileProcessingItem in FileSingletonModel.ProcessingFileCollection)
        fileProcessingItem.ProgressBarVisibility = Visibility.Visible;

      var bw = new BackgroundWorker();
      bw.DoWork += Bw_DoWork;
      bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
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
      if (result)
      {
        SetupLogger.WriteInfoLog("Excell olusturma islemi tamamlandi...");

        Dispatcher.DoInvoke(() => { App.MenuListBoxSelection(1); }, DispatcherPriority.Send);
      }
      else
      {
        SetupLogger.WriteInfoLog("Excell olusturma islemi tamamlanamadi...");

        Dispatcher.DoInvoke(() => { App.Navigate("/views/errorpage.xaml"); }, DispatcherPriority.Send);
      }
    }

    private void Bw_DoWork(object sender, DoWorkEventArgs e)
    {
     var officeDocumentManager  = DependencyContainer.Resolver.GetService<IOfficeeDocumentManager>();

      foreach (var fileProcessingItem in FileSingletonModel.ProcessingFileCollection)
      {
        var excelTable = ExcellFileProcess.Instance.GetConentAsTable(fileProcessingItem.File, fileProcessingItem.Columns);
        if (excelTable != null)
        {
          var result = officeDocumentManager.CreateExcelDocument(fileProcessingItem.File, excelTable);
          if (result)
          {
            fileProcessingItem.ProgressBarVisibility = Visibility.Collapsed;
            fileProcessingItem.DonePathVisibility = Visibility.Visible;
          }
        }
      }
      e.Result = true;
    }
  }
}
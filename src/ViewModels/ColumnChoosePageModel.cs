using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Excell.Processor.Models;
using Matriks.Oms.EnterpriseLibrary;
using Matriks.Oms.EnterpriseLibrary.Common;
using Matriks.Wpf.Framework;

namespace Excell.Processor.ViewModels
{
  public class ColumnChoosePageModel : SetupMainPageModel
  {
    private FileSingletonModel _filesingletonModel;
    public FileSingletonModel FilesingletonModel
    {
      get { return _filesingletonModel; }
      set
      {
        _filesingletonModel = value;
        RaisePropertyChanged(nameof(FilesingletonModel));
      }
    }

    public override void OnLoaded(FrameworkElement view)
    {
      base.OnLoaded(view);

      ProgressBarVisibility = Visibility.Visible;

      FilesingletonModel = DependencyContainer.Resolver.GetService<FileSingletonModel>();

      if (!FilesingletonModel.FileCollection.Any())
        return;

      var firstFile = FilesingletonModel.FileCollection.First();
      
      Task.Factory.StartNew(() =>
      {

        foreach (var s in ExcellFileProcess.Instance.GetColumnCollection(firstFile))
        {
          FilesingletonModel.ColumnCollection.Add(new ColumnItem() { ColumName = s.ColumName, Index = s.Index, IsSelected = true });
        }
        Dispatcher.DoInvoke(() => { ProgressBarVisibility = Visibility.Collapsed; }, DispatcherPriority.Send);
      });
    }

    public Visibility ProgressBarVisibility
    {
      get { return (Visibility)GetValue(ProgressBarVisibilityProperty); }
      set { SetValue(ProgressBarVisibilityProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ProgressBarVisibilityProperty = DependencyProperty.Register("ProgressBarVisibility", typeof(Visibility), typeof(ColumnChoosePageModel));
  }
}
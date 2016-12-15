using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Excell.Processor.Annotations;
using Excell.Processor.Models;
using Matriks.Wpf.Framework;

namespace Excell.Processor.ViewModels
{
  public class FileSingletonModel : INotifyPropertyChanged
  {
    public FileSingletonModel()
    {
      FileCollection = new DispatchedObservableCollection<FileItem>();
      ColumnCollection = new DispatchedObservableCollection<ColumnItem>();
      ProcessingFileCollection = new DispatchedObservableCollection<FileProcessingItem>();
    }


    private DispatchedObservableCollection<FileProcessingItem> _processingFileCollection;
    public DispatchedObservableCollection<FileProcessingItem> ProcessingFileCollection
    {
      get { return _processingFileCollection; }
      set
      {
        _processingFileCollection = value;
        OnPropertyChanged(nameof(ProcessingFileCollection));
      }
    }

    private DispatchedObservableCollection<FileItem> _fileCollection;
    public DispatchedObservableCollection<FileItem> FileCollection
    {
      get { return _fileCollection; }
      set
      {
        _fileCollection = value;
        OnPropertyChanged(nameof(FileCollection));
      }
    }

    private DispatchedObservableCollection<ColumnItem> _columnChoosePage;
    public DispatchedObservableCollection<ColumnItem> ColumnCollection
    {
      get { return _columnChoosePage; }
      set
      {
        _columnChoosePage = value;
        OnPropertyChanged(nameof(ColumnCollection));
      }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
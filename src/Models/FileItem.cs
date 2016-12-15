using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Excell.Processor.Annotations;

namespace Excell.Processor.Models
{
  public class FileItem
  {
    public string FileName { get; set; }
    public string Path { get; set; }
    public bool IsSelected { get; set; }
  }

  public class ColumnItem
  {
    public string ColumName { get; set; }
    public int Index { get; set; }
    public bool IsSelected { get; set; }

    public ColumnItem()
    {
      IsSelected = false;
    }
  }

  public class FileProcessingItem : INotifyPropertyChanged
  {
    private Visibility _progressBarVisibility;
    public FileItem File { get; set; }
    public IEnumerable<ColumnItem> Columns { get; set; }
    public bool IsSelected { get; set; }
    public bool IsDone { get; set; }

    public Visibility ProgressBarVisibility
    {
      get { return _progressBarVisibility; }
      set { _progressBarVisibility = value;
        OnPropertyChanged(nameof(ProgressBarVisibility));
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

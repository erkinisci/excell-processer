using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Excell.Processor.Models;
using Matriks.Wpf.Framework.Commands;

namespace Excell.Processor.ViewModels
{
  public class ApplicationChoosePageModel : SetupMainPageModel
  {

    public override void OnLoaded(FrameworkElement view)
    {
      base.OnLoaded(view);

      FileCollection = new ObservableCollection<FileItem>();
      var extensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".xls", ".xlsx" };
      
      foreach (var s in Directory.EnumerateFiles(ExcellProcessorSetup.MainFolderPath).Where(filename => extensions.Contains(Path.GetExtension(filename))))
      {
        FileCollection.Add(new FileItem() { FileName = Path.GetFileName(s), Path = s, IsSelected = true });
      }
    }
  }
}
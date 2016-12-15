using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Excell.Processor.Models;
using Matriks.Oms.EnterpriseLibrary;
using Matriks.Oms.EnterpriseLibrary.Common;
using Matriks.Oms.EnterpriseLibrary.Resolvers;
using Matriks.Wpf.Framework.Commands;

namespace Excell.Processor.ViewModels
{
  public class ApplicationChoosePageModel : SetupMainPageModel
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

      FilesingletonModel = DependencyContainer.Resolver.GetService<FileSingletonModel>();

      var extensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { ".xls", ".xlsx" };
      foreach (var s in Directory.EnumerateFiles(ExcellProcessorSetup.MainFolderPath).Where(filename => extensions.Contains(Path.GetExtension(filename))))
      {
        FilesingletonModel.FileCollection.Add(new FileItem() { FileName = Path.GetFileName(s), Path = s, IsSelected = true });
      }
    }
  }
}
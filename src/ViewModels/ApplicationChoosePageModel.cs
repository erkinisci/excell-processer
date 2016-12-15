using System.Windows;
using System.Windows.Forms;
using Matriks.Wpf.Framework.Commands;

namespace Excell.Processor.ViewModels
{
  public class ApplicationChoosePageModel : SetupMainPageModel
  {
    public DelegateCommand FilePathDialogCommand { get; set; }

    public override void OnLoaded(FrameworkElement view)
    {
      base.OnLoaded(view);

      FilePathDialogCommand = new DelegateCommand(OnFilePathDialogCommand);
    }

    private void OnFilePathDialogCommand()
    {
      using (var dlg = new FolderBrowserDialog())
      {
        dlg.SelectedPath = ExcellProcessorSetup.MainFolderPath;
        var result = dlg.ShowDialog();
        if (result == DialogResult.OK)
        {
          ExcellProcessorSetup.MainFolderPath = dlg.SelectedPath;
        }
      }
    }
  }
}
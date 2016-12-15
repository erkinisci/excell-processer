using System.Windows;
using System.Windows.Forms;
using Excell.Processor.Models;
using Matriks.Oms.EnterpriseLibrary;
using Matriks.Oms.EnterpriseLibrary.Common;
using Matriks.Wpf.Framework.Commands;

namespace Excell.Processor.ViewModels
{
  public class FirstPageModel : SetupMainPageModel
  {
    public DelegateCommand FilePathDialogCommand { get; set; }

    public override void OnLoaded(FrameworkElement view)
    {
      base.OnLoaded(view);

      MatriksClientApiSetup = DependencyContainer.Resolver.GetService<MatriksClientApiSetupModel>("MatriksClientApiSetupModel");

      FilePathDialogCommand = new DelegateCommand(OnFilePathDialogCommand);
    }

    private void OnFilePathDialogCommand()
    {
      using (var dlg = new FolderBrowserDialog())
      {
        dlg.SelectedPath = MatriksClientApiSetup.MainFolderPath;
        var result = dlg.ShowDialog();
        if (result == DialogResult.OK)
        {
          MatriksClientApiSetup.MainFolderPath = dlg.SelectedPath;
        }
      }
    }
  }
}
using System.Diagnostics;
using System.Windows;
using Excell.Processor.Models;
using Matriks.Oms.EnterpriseLibrary;
using Matriks.Oms.EnterpriseLibrary.Common;
using Matriks.Wpf.Framework.Commands;

namespace Excell.Processor.ViewModels
{
  public class ErrorPageModel : SetupMainPageModel
  {
    public DelegateCommand OpenErrorFileCommand { get; set; }

    public override void OnLoaded(FrameworkElement view)
    {
      base.OnLoaded(view);

      ExcellProcessorSetup = DependencyContainer.Resolver.GetService<ExcellProcessorSetupModel>("ExcellProcessorSetupModel");

      OpenErrorFileCommand = new DelegateCommand(OnOpenErrorFileCommand);
    }

    private void OnOpenErrorFileCommand()
    {
      Process.Start("notepad.exe", SetupLogger.GetLogFilePath());
    }
  }
}
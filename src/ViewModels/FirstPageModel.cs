using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Forms;
using Matriks.ClientAPI.Setup.Models;
using Matriks.Oms.EnterpriseLibrary;
using Matriks.Oms.EnterpriseLibrary.Common;
using Matriks.Oms.EnterpriseLibrary.Configuration;
using Matriks.Wpf;
using Matriks.Wpf.Framework.Commands;

namespace Matriks.ClientAPI.Setup.ViewModels
{
  public class FirstPageModel : NavigateObject
  {
    public DelegateCommand FilePathDialogCommand { get; set; }
    public DelegateCommand NextPageCommand { get; set; }

    public DelegateCommand CancelCommand { get; set; }

    public override void OnLoaded(FrameworkElement view)
    {
      base.OnLoaded(view);

      MatriksClientApiSetup = DependencyContainer.Resolver.GetService<MatriksClientApiSetupModel>("MatriksClientApiSetupModel");

      FilePathDialogCommand = new DelegateCommand(OnFilePathDialogCommand);
      NextPageCommand = new DelegateCommand(OnNextPageCommand);
      CancelCommand = new DelegateCommand(App.GlobalCancelCommand);
    }

    private void OnNextPageCommand()
    {
      App.MenuListBoxSelection(1);
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

    public MatriksClientApiSetupModel MatriksClientApiSetup
    {
      get { return (MatriksClientApiSetupModel)GetValue(MatriksClientApiSetupProperty); }
      set { SetValue(MatriksClientApiSetupProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Menus.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty MatriksClientApiSetupProperty = DependencyProperty.Register("MatriksClientApiSetup", typeof(MatriksClientApiSetupModel), typeof(FirstPageModel));
  }
}
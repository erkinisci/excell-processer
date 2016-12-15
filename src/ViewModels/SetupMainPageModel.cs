using System.Collections.ObjectModel;
using System.Windows;
using Excell.Processor.Core;
using Excell.Processor.Models;
using Matriks.Oms.EnterpriseLibrary;
using Matriks.Oms.EnterpriseLibrary.Common;
using Matriks.Wpf;
using Matriks.Wpf.Framework.Commands;

namespace Excell.Processor.ViewModels
{
  public class SetupMainPageModel : NavigateObject
  {
    public ISetupLogger SetupLogger { get; set; }

    public DelegateCommand ReturnCommand { get; set; }

    public DelegateCommand NextPageCommand { get; set; }

    public DelegateCommand CancelCommand { get; set; }

    public new DelegateCommand CloseCommand { get; set; }

    private ObservableCollection<FileItem> _fileCollection;
    public ObservableCollection<FileItem> FileCollection
    {
      get { return _fileCollection; }
      set
      {
        _fileCollection = value;
        RaisePropertyChanged(nameof(FileCollection));
      }
    }

    public override void OnLoaded(FrameworkElement view)
    {
      base.OnLoaded(view);

      ExcellProcessorSetup = DependencyContainer.Resolver.GetService<ExcellProcessorSetupModel>("ExcellProcessorSetupModel");
      SetupLogger = DependencyContainer.Resolver.GetService<ISetupLogger>("SetupLogger");

      ReturnCommand = new DelegateCommand(OnReturnCommand);
      NextPageCommand = new DelegateCommand(OnNextPageCommand);
      CancelCommand = new DelegateCommand(App.GlobalCancelCommand);
      CloseCommand = new DelegateCommand(OnCloseCommand);
    }

    private void OnCloseCommand()
    {
      Application.Current.Shutdown();
    }

    public virtual void OnNextPageCommand()
    {
      App.MenuListBoxSelection(1);
    }

    public virtual void OnReturnCommand()
    {
      App.MenuListBoxSelection(-1);
    }

    public ExcellProcessorSetupModel ExcellProcessorSetup
    {
      get { return (ExcellProcessorSetupModel)GetValue(ExcellProcessorSetupProperty); }
      set { SetValue(ExcellProcessorSetupProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Menus.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ExcellProcessorSetupProperty = DependencyProperty.Register("ExcellProcessorSetup", typeof(ExcellProcessorSetupModel), typeof(SetupMainPageModel));
  }
}
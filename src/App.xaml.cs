using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using Matriks.ClientAPI.Setup.Core;
using Matriks.Oms.EnterpriseLibrary.Common;
using Matriks.Oms.EnterpriseLibrary.Configuration;
using Matriks.Oms.EnterpriseLibrary.Resolvers;
using Matriks.Wpf;
using Matriks.Wpf.Framework;

namespace Matriks.ClientAPI.Setup
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    public new static Dispatcher Dispatcher { get; private set; }

    public static Frame LayoutRoot { get; set; }

    public static ListBox MenuListBox { get; set; }

    public static void MenuListBoxSelection(int index)
    {
      if (index < 0 && MenuListBox.SelectedIndex > -1)
        MenuListBox.SelectedIndex = MenuListBox.SelectedIndex - 1;
      else
      {
        MenuListBox.SelectedIndex = MenuListBox.SelectedIndex + 1;
      }
    }
    
    public static void Navigate(string viewName)
    {
      var n = NavigationService.Navigate(viewName, true);
      LayoutRoot.Navigate(n.View);
    }

    [STAThread]
    protected override void OnStartup(StartupEventArgs e)
    {
      Timeline.DesiredFrameRateProperty.OverrideMetadata(typeof(Timeline), new FrameworkPropertyMetadata { DefaultValue = 10 });

      var module = new Bootstrapter();
      module.Initialize();

      DependencyContainer.AddResolver(new SingletonDependencyResolver<IAppSettings>(new UIAppSettings(), "UISettings"));

      var mainWindow = new MainWindow();
      Current.MainWindow = mainWindow;

      if (e.Args.Length > 0)
      {
        try
        {
          Dispatcher = Dispatcher.CurrentDispatcher;

          Task.Factory.StartNew(WaitExitMainProcess);

        }
        catch (Exception ex)
        {
          Console.Error.WriteLine(ex);
        }
      }
      else
      {
        mainWindow.Show();
      }

      base.OnStartup(e);

      Current.DispatcherUnhandledException += Current_DispatcherUnhandledException;
    }

    private void Current_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
      e.Handled = true;
    }

    private static void WaitExitMainProcess()
    {
      var mainProcess = Process.GetProcessesByName(Utils.ManagementAppName).FirstOrDefault();
      if (mainProcess == null)
        return;
      mainProcess.WaitForExit();
      Dispatcher.DoInvoke(Application.Current.Shutdown);
    }
  }
}

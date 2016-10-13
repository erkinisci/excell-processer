using System;
using System.Windows;
using System.Windows.Input;

namespace Matriks.ClientAPI.Setup.Core.Windows
{
  public class WindowMaximizeCommand : ICommand
  {
     
    public bool CanExecute(object parameter)
    {
      Window prmWindow = parameter as Window;

      return prmWindow?.ResizeMode != ResizeMode.CanMinimize;
    }

    public event EventHandler CanExecuteChanged
    {
      add { }
      remove { }
    }

    public void Execute(object parameter)
    {
      var window = parameter as Window;

      if (window != null)
      {
        if (window.WindowState == WindowState.Maximized)
        {
          window.WindowState = WindowState.Normal;
        }
        else
        {
          window.WindowState = WindowState.Maximized;
        }
      }
    }
  }
}

using System;
using System.Windows;
using System.Windows.Input;

namespace Matriks.ClientAPI.UI.Core.Windows
{
  public class WindowCloseCommand : ICommand
  {

    public bool CanExecute(object parameter)
    {
      return true;
    }
     
    public event EventHandler CanExecuteChanged
    {
      add {  } 
      remove { }
    }

    public void Execute(object parameter)
    {
      var window = parameter as Window;

      window?.Close();
    }
  }
}

using System.Collections.ObjectModel;
using System.Windows;
using Matriks.Wpf;

namespace Matriks.ClientAPI.Setup.ViewModels
{
  public class FirstPageModel : NavigateObject
  {
    public override void OnNavigate(UriQuery query)
    {
      base.OnNavigate(query);


    }

    public override void OnLoaded(FrameworkElement view)
    {
      base.OnLoaded(view);

      FilePath = "C:ProgramFiles";
    }

    public string FilePath
    {
      get { return (string)GetValue(FilePathProperty); }
      set { SetValue(FilePathProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Menus.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FilePathProperty = DependencyProperty.Register("FilePath", typeof(string), typeof(FirstPageModel));

  }
}
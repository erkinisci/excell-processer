using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Matriks.Wpf.Framework;
using Matriks.Wpf.Models;

namespace Matriks.ClientAPI.Setup
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();

      DataContext = this;

      Menus = new ObservableCollection<MenuModel>();

      Loaded += MainWindow_Loaded;

      AddMenu("collecting", "views.firstpage");
      AddMenu("FirstPage", "views.firstpage");
      AddMenu("LoadingPage", "views.loadingpage");
      AddMenu("ComplatedPage", "views.complatedpage");

      MenuListBox.SelectionChanged += MenuListBox_SelectionChanged;

      App.LayoutRoot = LayoutRoot;
    }

    private void MenuListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      foreach (var menuModel in Menus)
        menuModel.IsActive = false;

      Menus[MenuListBox.SelectedIndex].IsActive = true;
      Navigate(Menus[MenuListBox.SelectedIndex].Root);
    }

    protected void AddMenu(string name, string nameSpace)
    {
      Menus.Add(new MenuModel { Name = name, ViewNamespace = nameSpace });
    }

    void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
      Navigate("/views/firstpage.xaml");
    }

    private void Navigate(string viewName)
    {
      App.Navigate(viewName);
    }

    public ObservableCollection<MenuModel> Menus
    {
      get { return (ObservableCollection<MenuModel>)GetValue(MenusProperty); }
      set { SetValue(MenusProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Menus.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty MenusProperty = DependencyProperty.Register("Menus", typeof(ObservableCollection<MenuModel>), typeof(MainWindow));

  }
}

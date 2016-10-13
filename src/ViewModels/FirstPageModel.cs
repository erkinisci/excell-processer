﻿using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Forms;
using Matriks.Oms.EnterpriseLibrary;
using Matriks.Oms.EnterpriseLibrary.Common;
using Matriks.Oms.EnterpriseLibrary.Configuration;
using Matriks.Wpf;
using Matriks.Wpf.Framework.Commands;

namespace Matriks.ClientAPI.Setup.ViewModels
{
  public class FirstPageModel : NavigateObject
  {
    private IAppSettings _appSettings;
    public DelegateCommand FilePathDialogCommand { get; set; }

    public override void OnNavigate(UriQuery query)
    {
      base.OnNavigate(query);
    }

    public override void OnLoaded(FrameworkElement view)
    {
      base.OnLoaded(view);

      _appSettings = DependencyContainer.Resolver.GetService<IAppSettings>("UISettings");

      FilePath = _appSettings.GetString("FilePath");
      FilePathDialogCommand = new DelegateCommand(OnFilePathDialogCommand);
    }

    private void OnFilePathDialogCommand()
    {
      using (var dlg = new FolderBrowserDialog())
      {
        dlg.SelectedPath = FilePath;
        var result = dlg.ShowDialog();
        if (result == DialogResult.OK)
        {
          FilePath = dlg.SelectedPath;
        }
      }
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
﻿using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms.VisualStyles;
using System.Windows.Threading;
using Matriks.ClientAPI.Setup.Models;
using Matriks.Oms.EnterpriseLibrary;
using Matriks.Oms.EnterpriseLibrary.Common;
using Matriks.Oms.EnterpriseLibrary.Configuration;
using Matriks.Wpf;
using Matriks.Wpf.Framework;
using Matriks.Wpf.Framework.Commands;

namespace Matriks.ClientAPI.Setup.ViewModels
{
  public class LoadingPageModel : NavigateObject
  {
    private IAppSettings _appSettings;
    public DelegateCommand LoadCommand { get; set; }

    public DelegateCommand ReturnCommand { get; set; }

    public DelegateCommand CancelCommand { get; set; }

    public DelegateCommand LastCommand { get; set; }


    public bool IsLoading { get; private set; }

    private ExeFileCreator ExeFileCreator { get; set; }

    public override void OnLoaded(FrameworkElement view)
    {
      base.OnLoaded(view);

      _appSettings = DependencyContainer.Resolver.GetService<IAppSettings>("UISettings");
      ProgressBarVisibility = Visibility.Collapsed;

      LoadCommand = new DelegateCommand(OnLoadCommand);
      ReturnCommand = new DelegateCommand(OnReturnCommand);
      CancelCommand = new DelegateCommand(App.GlobalCancelCommand);
      LastCommand = new DelegateCommand(OnLastCommand);
      ExeFileCreator = new ExeFileCreator();

      IsLoading = false;

     
    }
    
    private void OnLastCommand()
    {
      if (IsApplicationStart)
        ExeFileCreator.RunApplication();

      Application.Current.Shutdown();
    }

    private void OnReturnCommand()
    {
      App.MenuListBoxSelection(-1);
    }

    private void OnLoadCommand()
    {
      if (IsLoading)
        return;

      ProgressBarVisibility = Visibility.Visible;

      BackgroundWorker bw = new BackgroundWorker();
      bw.DoWork += Bw_DoWork;
      bw.RunWorkerCompleted += Bw_RunWorkerCompleted;

      IsLoading = true;
      bw.RunWorkerAsync();
    }

    private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      IsLoading = true;

      Dispatcher.DoInvoke(() => { App.MenuListBoxSelection(1); }, DispatcherPriority.Send);

    }

    private void Bw_DoWork(object sender, DoWorkEventArgs e)
    {
      ExeFileCreator.ExtractFilesFromEmbeddedZip();
    }

    public bool IsApplicationStart
    {
      get { return (bool)GetValue(IsApplicationStartProperty); }
      set { SetValue(IsApplicationStartProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IsApplicationStartProperty = DependencyProperty.Register("IsApplicationStart", typeof(bool), typeof(LoadingPageModel));


    public Visibility ProgressBarVisibility
    {
      get { return (Visibility)GetValue(ProgressBarVisibilityProperty); }
      set { SetValue(ProgressBarVisibilityProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ProgressBarVisibilityProperty = DependencyProperty.Register("ProgressBarVisibility", typeof(Visibility), typeof(LoadingPageModel));
  }
}
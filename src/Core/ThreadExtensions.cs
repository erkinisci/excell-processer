﻿using System;
using System.Windows.Threading;

namespace Matriks.ClientAPI.UI.Core
{
  public static class ThreadExtensions
  {

    public static void Do(this Dispatcher dispatcher, Action action)
    {
      dispatcher.Invoke(action);
    }
  }
}
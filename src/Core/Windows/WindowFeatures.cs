using System.Windows;

namespace Excell.Processor.Core.Windows
{
  public static class WindowFeatures
  { 

    public static int GetStatu(DependencyObject obj)
    {
      return (int)obj.GetValue(StatuProperty);
    }

    public static void SetStatu(DependencyObject obj, int value)
    {
      obj.SetValue(StatuProperty, value);
    }

    // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty StatuProperty =
        DependencyProperty.RegisterAttached("Statu", typeof(int), typeof(WindowFeatures), new PropertyMetadata(-1));
     
  }
}

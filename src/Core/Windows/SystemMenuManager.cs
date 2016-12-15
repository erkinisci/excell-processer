using System;
using System.Windows;
using System.Windows.Interop;

namespace Excell.Processor.Core.Windows
{
    public static class SystemMenuManager
    {        
        public static void ShowMenu(Window targetWindow, Point menuLocation)
        {
            if (targetWindow == null)
                throw new ArgumentNullException("TargetWindow is null.");

            int x, y;

            try
            {
                x = Convert.ToInt32(menuLocation.X);
                y = Convert.ToInt32(menuLocation.Y);
            }
            catch (OverflowException)
            {
                x = 0;
                y = 0;
            }

            uint WM_SYSCOMMAND = 0x112, TPM_LEFTALIGN = 0x0000, TPM_RETURNCMD = 0x0100;           

            IntPtr window = new WindowInteropHelper(targetWindow).Handle;

            IntPtr wMenu = NativeMethods.GetSystemMenu(window, false);

            int command = NativeMethods.TrackPopupMenuEx(wMenu, TPM_LEFTALIGN | TPM_RETURNCMD, x, y, window, IntPtr.Zero);

            if (command == 0)
                return;

            NativeMethods.PostMessage(window, WM_SYSCOMMAND, new IntPtr(command), IntPtr.Zero);
        }
    }
}

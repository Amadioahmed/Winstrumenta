﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace Windows.Win32
{
    internal static partial class PInvoke
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        // This helper static method is required because the 32-bit version of user32.dll does not contain this API
        // (on any versions of Windows), so linking the method will fail at run-time. The bridge dispatches the request
        // to the correct function (GetWindowLong in 32-bit mode and GetWindowLongPtr in 64-bit mode)
        public static IntPtr SetWindowLongPtr(
            Win32.Foundation.HWND hWnd,
            Win32.UI.WindowsAndMessaging.WINDOW_LONG_PTR_INDEX nIndex,
            IntPtr dwNewLong
        )
        {
            if (Environment.Is64BitProcess)
            {
                return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
            }

            return new IntPtr(SetWindowLong32(hWnd, nIndex, dwNewLong.ToInt32()));
        }

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern int SetWindowLong32(
            Win32.Foundation.HWND hWnd,
            Win32.UI.WindowsAndMessaging.WINDOW_LONG_PTR_INDEX nIndex,
            int dwNewLong
        );

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLongPtr64(
            Win32.Foundation.HWND hWnd,
            Win32.UI.WindowsAndMessaging.WINDOW_LONG_PTR_INDEX nIndex,
            IntPtr dwNewLong
        );

        [DllImport("user32.dll")]
        internal static extern IntPtr CallWindowProc(
            IntPtr lpPrevWndFunc,
            IntPtr hWnd,
            WindowMessage Msg,
            IntPtr wParam,
            IntPtr lParam
        );

        public delegate IntPtr WinProc(
            IntPtr hWnd,
            WindowMessage Msg,
            IntPtr wParam,
            IntPtr lParam
        );
    }
}

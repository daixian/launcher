using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class SetScreenReslution : MonoBehaviour
{
    [DllImport("user32.dll")]
    public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, int wndProc);

    [DllImport("user32.dll")]
    public static extern IntPtr CallWindowProc(IntPtr wndProc, IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();

    private const uint SWP_SHOWWINDOW = 0x0040;
    private const int GWL_STYLE = -16;
    private const int WS_BORDER = 1;
    private const int WS_POPUP = 0x800000;

    private void Start()
    {
        WithOutWindow();
    }

    private void WithOutWindow()
    {
        SetWindowLong(GetForegroundWindow(), GWL_STYLE, WS_POPUP);//将网上的WS_BORDER替换成WS_POPUP
        SetWindowPos(GetForegroundWindow(), 0, 0, 0, 1920, 1080, SWP_SHOWWINDOW);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace xuexue
{
    class WinApi
    {

        [DllImport("User32.dll")]
        //返回值：如果窗口原来可见，返回值为非零；如果函数原来被隐藏，返回值为零。
        public static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);

        //API 常数定义
        public const int SW_HIDE = 0;
        public const int SW_NORMAL = 1;    //正常弹出窗体
        public const int SW_MAXIMIZE = 3;    //最大化弹出窗体
        public const int SW_SHOWNOACTIVATE = 4;
        public const int SW_SHOW = 5;
        public const int SW_MINIMIZE = 6;
        public const int SW_RESTORE = 9;
        public const int SW_SHOWDEFAULT = 10;
        //ShowWindowAsync(instance.MainWindowHandle, SW_RESTORE);
        //SetForegroundWindow(instance.MainWindowHandle);


        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int Width, int Height, int flags);
        /// <summary> 
        /// 得到当前活动的窗口 
        /// </summary> 
        /// <returns></returns> 
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern System.IntPtr GetForegroundWindow();


    }
}

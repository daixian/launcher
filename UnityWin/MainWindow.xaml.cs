using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using xuexue;

namespace UnityWin
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //这里最好不要使用这些小白设置，在xaml里面有一个WindowState="Maximized" WindowStartupLocation="CenterOwner"
            //this.WindowState = System.Windows.WindowState.Normal;
            //this.WindowStyle = System.Windows.WindowStyle.None;
            //this.ResizeMode = System.Windows.ResizeMode.NoResize;
            //this.Left = 200;
            //this.Top = 20;
            //this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            //this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;

            //this.Width = 1920;
            //this.Height = 1080;

            this.Topmost = true;
            this.ShowInTaskbar = false;
        }

        public static Process process;

        /// <summary>
        /// 自己窗口的句柄
        /// </summary>
        public IntPtr handle;

        private Timer _timer;


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            handle = new WindowInteropHelper(this).Handle;
            loadUnity();
            //this.Hide();
            //_timer = new Timer(OnTimer);
            //_timer.Change(1000, 1000);//由于调试的时候程序名不一样，所以不用了

        }

        /// <summary>
        /// 载入untiy窗口
        /// </summary>
        public void loadUnity()
        {
            try
            {
                string startConfig = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "launcher.dat");
                if (!System.IO.File.Exists(startConfig))
                {
                    System.IO.File.Create(startConfig).Dispose();//创建一个空文件

                    Process.GetCurrentProcess().Kill();//关闭自己
                }
                string[] lines = System.IO.File.ReadAllLines(startConfig);
                if (lines.Length < 1)
                {
                    Process.GetCurrentProcess().Kill();//关闭自己
                    return;
                }
                string exeName = lines[0];

                process = new Process();
                process.StartInfo.FileName = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, exeName);
                process.StartInfo.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;

                var cache = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), System.IO.Path.Combine("F3DTeaching", "u3d.log"));
                string args = "-stackTraceLogType Full  -silent-crashes -screen-width 1280 -screen-height 720";
                process.StartInfo.Arguments = args + " -parentHWND " + handle.ToInt64();
                // process.StartInfo.Arguments = " -parentHWND " + this.u3dPanel.u3dPanel.Handle.ToInt64(); //MainWindow

                process.StartInfo.UseShellExecute = true;
                process.StartInfo.CreateNoWindow = true;

                process.Start();

                process.WaitForInputIdle();//去掉了这个以免发生问题

                process.EnableRaisingEvents = true;
                process.Exited += (o1,o2) =>
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        if (Process.GetCurrentProcess() != null)
                            Process.GetCurrentProcess().Kill();//关闭自己
                    });
                };

                //过2秒之后才去寻找u3d句柄
                Task.Run(new Action(FindUnityHWND));


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// unity句柄
        /// </summary>
        static IntPtr unityHWND = IntPtr.Zero;

     
        private void FindUnityHWND()
        {
            while (unityHWND == IntPtr.Zero)
            {
                Thread.Sleep(500);
                App.Current.Dispatcher.Invoke(() =>
                {
                    EnumChildWindows(handle, WindowEnum, IntPtr.Zero);
                });
            }

            App.Current.Dispatcher.Invoke(() =>
            {

                DxDebug.LogConsole("FindUnityHWND()：找到了Unity窗口");
                this.Topmost = false;
            });           
        }

        private void OnTimer(object state)
        {
            try
            {
                Process[] pro = Process.GetProcesses();//获取已开启的所有进程

                //遍历所有查找到的进程
                for (int i = 0; i < pro.Length; i++)
                {
                    //判断此进程是否是要查找的进程
                    if (pro[i].ProcessName.ToString().ToLower() == "WoKeBrowserWpf".ToLower())
                    {
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                DxDebug.LogConsole("CheckNeedClose.CheckClose():异常：" + e.Message);
                return;
            }

            App.Current.Dispatcher.Invoke(() =>
            {
                try
                {
                    DxDebug.LogConsole("CheckNeedClose.CheckClose():不存在主进程了，关闭自己");
                    try { _timer.Dispose(); } catch (Exception) { }//关闭timer
                    try { process.Kill(); } catch (Exception) { }//关闭u3d
                    Process.GetCurrentProcess().Kill();
                }
                catch (Exception e)
                {
                    Process.GetCurrentProcess().Kill();
                }
            });
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            { 
                try { _timer.Dispose(); } catch (Exception) { }//关闭timer
                try { process.Kill(); } catch (Exception) { }//关闭u3d
       
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 枚举子窗口得到u3d句柄，枚举函数的回调
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lparam"></param>
        /// <returns></returns>
        static int WindowEnum(IntPtr hwnd, IntPtr lparam)
        {
            unityHWND = hwnd;
            SendMessage(unityHWND, WM_ACTIVATE, WA_ACTIVE, IntPtr.Zero);
            return 0;
        }


        [DllImport("User32.dll")]
        static extern bool MoveWindow(IntPtr handle, int x, int y, int width, int height, bool redraw);


        [DllImport("user32.dll", SetLastError = true)]
        private static extern long SetParent(IntPtr hWndChild, IntPtr hWndNewParent);


        const int WM_ACTIVATE = 0x0006;
        static readonly IntPtr WA_ACTIVE = new IntPtr(1);
        static readonly IntPtr WA_INACTIVE = new IntPtr(0);

        internal delegate int WindowEnumProc(IntPtr hwnd, IntPtr lparam);
        [DllImport("user32.dll")]
        internal static extern bool EnumChildWindows(IntPtr hwnd, WindowEnumProc func, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);


    }
}

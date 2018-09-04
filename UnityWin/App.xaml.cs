using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using xuexue;

namespace UnityWin
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();
        /// <summary>
        /// 释放控制台
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();

        private void Application_Startup(object sender, StartupEventArgs e)
        {

#if DEBUG
            //如果定义了DEBUG才打开控制台
            AllocConsole();
#endif
            var logDir = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), System.IO.Path.Combine("UnityLauncher", "log"));
            LogFile.GetInst().ClearLogFileInFolder(logDir);//清理日志
            LogFile.GetInst().CreatLogFile(logDir);//创建日志文件
            DxDebug.IsLogFile = true;
            DxDebug.IsConsole = true;

        }
    }
}

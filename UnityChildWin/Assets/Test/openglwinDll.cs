using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

public class openglwinDll : MonoBehaviour
{
    [DllImport("oglwin")]
    public static extern bool InitDll(IntPtr hwnd, bool isLogFile, string logFileName);

    [DllImport("oglwin")]
    public static extern void draw();

    [DllImport("oglwin")]
    public static extern void drawTexture(IntPtr texture, int width, int height);

    [DllImport("oglwin")]
    public static extern IntPtr GetRenderEventFunc();

    [DllImport("oglwin")]
    public static extern void SetParam(int w, int h, int startx, int starty, int[] startxArr, int length);

    [DllImport("oglwin")]
    public static extern void setTextureParam(IntPtr texture, int width, int height);

    [DllImport("oglwin")]
    public static extern void setHeaderParam(byte[] header, int size, int width, int height);

    [DllImport("oglwin")]
    public static extern void setIsLog(bool isLog);

    [DllImport("oglwin")]
    public static extern bool getIsLog();

    [DllImport("oglwin")]
    public static extern void setIsDrawHarder(bool IsDraw);

    [DllImport("oglwin")]
    public static extern bool getIsDrawHarder();

    [DllImport("oglwin")]
    public static extern IntPtr gethwnd();

    [DllImport("oglwin")]
    public static extern int getWinWidth();

    [DllImport("oglwin")]
    public static extern int getWinHeight();

    [DllImport("oglwin")]
    public static extern uint getDpiX();

    [DllImport("oglwin")]
    public static extern uint getDpiY();

    /// <summary>
    /// 得到屏幕个数，要注意只有在扩展模式屏幕个数才是2，复制模式屏幕个数仍是1.
    /// </summary>
    /// <returns></returns>
    [DllImport("oglwin")]
    public static extern uint getMonitorCount();

    [DllImport("oglwin")]
    public static extern void drawTexture2();

    [DllImport("oglwin")]
    public static extern void drawTextureLR(IntPtr ltexture, int lwidth, int lheight, IntPtr rtexture, int rwidth, int rheight);

    [DllImport("oglwin")]
    public static extern void drawTexture2LR();

    [DllImport("oglwin")]
    public static extern void setTextureParamLR(IntPtr texture_l, int width_l, int height_l, IntPtr texture_r, int width_r, int height_r);

    [DllImport("oglwin")]
    public static extern void drawTextureLR_depth(IntPtr ztexture, int zwidth, int zheight, bool isReadScreen);

    [DllImport("oglwin")]
    public static extern void ReadScreen(bool isForceU3D);

    [DllImport("oglwin")]
    public static extern void SetU3DWindowInfo(int w, int h);

    ///----------------------------------   获得窗口句柄  ----------------------------------------------
    public delegate bool WNDENUMPROC(IntPtr hwnd, uint lParam);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, uint lParam);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr GetParent(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, ref uint lpdwProcessId);

    [DllImport("kernel32.dll")]
    public static extern void SetLastError(uint dwErrCode);

    public static IntPtr GetProcessWnd()
    {
        IntPtr ptrWnd = IntPtr.Zero;
        uint pid = (uint)Process.GetCurrentProcess().Id;  // 当前进程 ID

        bool bResult = EnumWindows(new WNDENUMPROC(delegate (IntPtr hwnd, uint lParam)
        {
            uint id = 0;

            if (GetParent(hwnd) == IntPtr.Zero)
            {
                GetWindowThreadProcessId(hwnd, ref id);
                if (id == lParam)    // 找到进程对应的主窗口句柄
                {
                    ptrWnd = hwnd;   // 把句柄缓存起来
                    SetLastError(0);    // 设置无错误
                    return false;   // 返回 false 以终止枚举窗口
                }
            }

            return true;
        }), pid);

        return (!bResult && Marshal.GetLastWin32Error() == 0) ? ptrWnd : IntPtr.Zero;
    }

    ///-------------------------------------------------------------------------------------------------

    ///-------------------------------------------------------------------------------------------------
    /// <summary> 按当前窗口大小创建渲染贴图. </summary>
    ///
    /// <remarks> Xian Dai, 2017/5/28. </remarks>
    ///
    /// <returns> The new render texture l. </returns>
    ///-------------------------------------------------------------------------------------------------
    public static RenderTexture CreateRenderTexture_LR()
    {
        RenderTexture rt = new RenderTexture(getWinWidth() / 2, getWinHeight(), 24, RenderTextureFormat.ARGB32);
        return rt;
    }
}
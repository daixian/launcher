using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;
using CETextBoxControl;

public class Test : MonoBehaviour
{

    public void SetCandidateWindow(IntPtr handle)
    {
        if (handle == null)
        {
            return;
        }

        IntPtr hImc = CEWin32Api.ImmGetContext(handle);
        CEWin32Api.POINT point = new CEWin32Api.POINT(0, 0);
        CEWin32Api.GetCaretPos(out point);
        Debug.Log($"GetCaretPos执行结果 x={point.X} y={point.Y}");
        CEWin32Api.CANDIDATEFORM cndFrm = new CEWin32Api.CANDIDATEFORM();
        //cndFrm.ptCurrentPos.X = point.X;
        //cndFrm.ptCurrentPos.Y = point.Y;
        cndFrm.ptCurrentPos.X = 800;
        cndFrm.ptCurrentPos.Y = 600;
        cndFrm.dwStyle = CEWin32Api.CFS_CANDIDATEPOS;
        if (hImc != null)
        {
            bool res = CEWin32Api.ImmSetCandidateWindow(hImc, ref cndFrm);
            Debug.Log("ImmSetCandidateWindow执行结果 " + res);
        }
        CEWin32Api.ImmReleaseContext(handle, hImc);
    }

    /// <summary>
    /// 現在のキャレット位置にコンポジションウィンドウを表示する 
    /// （コンポジションウィンドウ：入力中の未確定文字列）
    /// </summary>
    /// <param name="handle"></param>
    public void SetCompositionWindow(IntPtr handle, Font font)
    {
        if (handle == null)
        {
            return;
        }

        IntPtr hImc = CEWin32Api.ImmGetContext(handle);
        CEWin32Api.POINT point = new CEWin32Api.POINT(0, 0);
        CEWin32Api.GetCaretPos(out point);
        CEWin32Api.COMPOSITIONFORM compform = new CEWin32Api.COMPOSITIONFORM();
        compform.ptCurrentPos.X = point.X;
        compform.ptCurrentPos.Y = point.Y;
        compform.dwStyle = CEWin32Api.CFS_POINT;
        if (hImc != null)
        {
            CEWin32Api.ImmSetCompositionWindow(hImc, ref compform);
        }

        IntPtr hHGlobalLOGFONT = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(CEWin32Api.LOGFONT)));
        IntPtr pLogFont = CEWin32Api.GlobalLock(hHGlobalLOGFONT);
        CEWin32Api.LOGFONT logFont = new CEWin32Api.LOGFONT();
 
        logFont.lfFaceName/*Name*/ = font.name; //追加
        Marshal.StructureToPtr(logFont, pLogFont, false);
        CEWin32Api.GlobalUnlock(hHGlobalLOGFONT);
        CEWin32Api.ImmSetCompositionFont(hImc, hHGlobalLOGFONT);
        Marshal.FreeHGlobal(hHGlobalLOGFONT);

        CEWin32Api.ImmReleaseContext(handle, hImc);
    }

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        setIMETest();
    }


    public void setIMETest()
    {
        SetCompositionWindow(openglwinDll.GetProcessWnd(), new Font());
        SetCandidateWindow(openglwinDll.GetProcessWnd());
    }


    public void OnGUI()
    {

    }
}

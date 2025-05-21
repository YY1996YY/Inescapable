using UnityEngine;
using System.Runtime.InteropServices;

public class Title : MonoBehaviour
{
    public string txt;
    // 条件付きコンパイル（Windowsスタンドアロンのみ有効）
#if UNITY_STANDALONE_WIN
    [DllImport("user32.dll", EntryPoint = "FindWindow", CharSet = CharSet.Unicode)]
    static extern System.IntPtr FindWindow(string className, string windowName);
    [DllImport("user32.dll", EntryPoint = "SetWindowText", CharSet = CharSet.Unicode)]
    static extern bool SetWindowText(System.IntPtr hwnd, string txt);
    void Start()
    {
        System.IntPtr hwnd = FindWindow(null, Application.productName);
        SetWindowText(hwnd, txt);
    }
#endif
}
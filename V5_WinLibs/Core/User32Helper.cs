using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace V5_WinLibs.Core {
    public class User32Helper {
        /// <summary>
        /// ��ȡ���ڱ���
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lpString"></param>
        /// <param name="nMaxCount"></param>
        /// <returns></returns>
        [DllImport("user32", SetLastError = true)]
        public static extern int GetWindowText(
        IntPtr hWnd, 
        StringBuilder lpString, 
        int nMaxCount  
        );
        /// <summary>
        /// ��ȡ�������
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="lpString"></param>
        /// <param name="nMaxCount"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern int GetClassName(
            IntPtr hWnd, 
            StringBuilder lpString, 
            int nMaxCount 
        );
        /// <summary>
        /// ���������ȡ���ھ��
        /// </summary>
        /// <param name="Point"></param>
        /// <returns></returns>
        [DllImport("user32")]
        private static extern IntPtr WindowFromPoint(
        Point Point  
        );
        /// <summary>
        /// ��ȡ���ھ��
        /// </summary>
        /// <param name="lpClassName">����</param>
        /// <param name="lpWindowName">��������</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        /// <summary>
        /// ��ȡ�ؼ����
        /// </summary>
        /// <param name="hwndParent"></param>
        /// <param name="hwndChildAfter"></param>
        /// <param name="lpszClass"></param>
        /// <param name="lpszWindow"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="wMsg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern IntPtr PostMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);
    }
}

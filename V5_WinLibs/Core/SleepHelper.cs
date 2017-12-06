using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace V5_WinLibs.Core {
    /// <summary>
    /// C#�ӳٺ��� ��Ҫʹ��sleep
    /// </summary>
    public class SleepHelper {
        [DllImport("kernel32.dll")]

        static extern uint GetTickCount();
        /// <summary>
        /// �ӳٺ���
        /// </summary>
        /// <param name="ms">����</param>
       public static void Delay(uint ms) {
            uint start = GetTickCount();
            while (GetTickCount() - start < ms) {
                Application.DoEvents();
            }
        }
    }
}

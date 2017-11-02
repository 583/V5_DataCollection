using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using V5_Utility.Core;
using V5_Utility;

namespace V5_DataPublish {
    static class Program {
        /// <summary>
        /// Ӧ�ó��������ڵ㡣
        /// </summary>
        [STAThread]
        static void Main() {
            {
                bool isAppRunning = false;
                System.Threading.Mutex mutex = new System.Threading.Mutex(
                    true,
                    System.Diagnostics.Process.GetCurrentProcess().ProcessName,
                    out isAppRunning);
                if (!isAppRunning) {
                    MessageBox.Show("�������Ѿ��������ˣ��벻Ҫ�ظ����У�");
                    Environment.Exit(1);
                }
                else {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    frmSplash fromSplash = new frmSplash();
                    Application.Run(fromSplash);
                    if (fromSplash.IsShow) {
                        Application.Run(new frmMain());
                    }
                }
            }
        }
    }
}

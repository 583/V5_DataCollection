using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace V5_DataPublishModuleEdit {
    static class Program {
        /// <summary>
        /// Ӧ�ó��������ڵ㡣
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace V5.AutoUpdate {
    static class Program {
        /// <summary>
        /// Ӧ�ó��������ڵ㡣
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            //// �ϲ������в���
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmUpdate());
        }
    }
}

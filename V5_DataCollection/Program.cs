using System;
using System.Threading;
using System.Windows.Forms;
using V5_DataCollection._Class.Plan;
using V5_WinLibs.DBUtility;

namespace V5_DataCollection {
    static class Program {

        /// <summary>
        /// Ӧ�ó��������ڵ㡣
        /// </summary>
        [STAThread]
        static void Main() {
            DbHelper.dbType = DataBaseType.SQLite;

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
                try {
                    Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    frmSplashForm fromSplash = new frmSplashForm();
                    Application.Run(fromSplash);
                    if (fromSplash.IsShow) {

                        #region ִ�мƻ�����
                        PlanTaskHelper.InitScheduler();
                        PlanTaskHelper.LoadAllJobs();
                        #endregion

                        Application.Run(new frmMain());
                    }
                }
                catch (Exception ex) {
                    MessageBox.Show(ex.Message + "::" + ex.InnerException + "::" + ex.StackTrace + "::" + ex.Source + "::" + ex.HelpLink);
                }

            }
        }


        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e) {
            try {
                string errorMsg = "�������й����з�������,������Ϣ����:\n";
                errorMsg += e.Exception.Message;
                errorMsg += "\n��������ĳ���Ϊ:";
                errorMsg += e.Exception.Source;
                errorMsg += "\n��������ľ���λ��Ϊ:\n";
                errorMsg += e.Exception.StackTrace;
                errorMsg += "\n\n ��ץȡ�˴�����Ļ,����V5�����ϵ!";
                MessageBox.Show(errorMsg, "����ʱ����--V5���", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch {
                MessageBox.Show("ϵͳ����ʱ������������!\n�뱣����������,����ϵͳ��", "��������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using System.Diagnostics;

namespace V5_WinLibs.Core {
    public class AppRunHelper {
        public delegate void OutPutTextHandler(object sender, AppRunEventArgs e);
        public static event OutPutTextHandler OutPutText;
        public static OutPutTextHandler OutPutMessage;
        AppRunHelper() {
            OutPutText += new OutPutTextHandler(AppRunHelper_OutPutText);
        }

        void AppRunHelper_OutPutText(object sender, AppRunHelper.AppRunEventArgs e) {
            if (OutPutMessage != null) {
                OutPutMessage(sender, e);
            }
        }
        /// <summary>
        /// ����С�������
        /// </summary>
        public static Form AppRunWebBrowserByAssembly(Form mainForm) {
            try {
                string path = AppDomain.CurrentDomain.BaseDirectory + "V5.DataWebBrowser.exe";
                Assembly assem = Assembly.LoadFile(path);
                Type tExForm = assem.GetType("V5.DataWebBrowser.frmMainBrowser");
                Object exFormAsObj = Activator.CreateInstance(tExForm);
                #region �ֶ�
                FieldInfo fi = tExForm.GetField("IsInvoke", BindingFlags.NonPublic | BindingFlags.Instance);
                fi.SetValue(exFormAsObj, true);
                #endregion
                #region �¼�
                EventInfo evClick = tExForm.GetEvent("OutPutText");
                Type tDelegate = evClick.EventHandlerType;
                MethodInfo miHandler =
                    typeof(AppRunHelper).GetMethod("AppRunHelper_OutPutText",
                        BindingFlags.NonPublic | BindingFlags.Instance);
                Delegate d = Delegate.CreateDelegate(tDelegate, null, miHandler);
                MethodInfo addHandler = evClick.GetAddMethod();
                Object[] addHandlerArgs = { d };
                addHandler.Invoke(exFormAsObj, addHandlerArgs);
                #endregion
                Form f = (Form)exFormAsObj;
                f.Show(mainForm);
                return f;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message + "==" + ex.Source + "==" + ex.InnerException);
            }
            return null;
        }
        /// <summary>
        /// �����¼�
        /// </summary>
        public class AppRunEventArgs : EventArgs {
            public string Msg1 { get; set; }
            public string Msg2 { get; set; }
            public string Msg3 { get; set; }
            public string Msg4 { get; set; }
        }
        /// <summary>
        /// �رմ���
        /// </summary>
        /// <param name="fr"></param>
        /// <param name="isInvoke"></param>
        public static void CloseWindow(Form fr, bool isInvoke) {
            GC.Collect();
            if (isInvoke) {
                fr.Close();
            }
            else {

                System.Environment.Exit(0);
            }
        }


        /// <summary>
        /// ��Ӧ�ó���
        /// </summary>
        /// <param name="cmdPath"></param>
        public static void RunAppCmd(string cmdPath, string cmdArguments) {
            try {
                Process cmd = new Process();
                cmd.StartInfo.FileName = cmdPath;
                cmd.StartInfo.Arguments = cmdArguments;
                cmd.StartInfo.CreateNoWindow = true;
                cmd.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                cmd.Start(); 
            }
            catch (Exception ex) {
                MessageBox.Show(cmdPath + ":::" + ex.Message + "==" + ex.Source + "==" + ex.InnerException);
            }

        }
    }
}

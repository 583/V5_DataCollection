using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace V5_WinLibs.Utility {
    /// <summary>
    /// ��־������
    /// </summary>
    public class LogHelper {
        private bool m_isWriteLog = true;  //�Ƿ�д��־
        private String m_logFilePath = String.Empty;  //��־·��
        private String m_time;  //ʱ���ַ���
        /// <summary>
        /// ��ʼ����־�ļ���
        /// </summary>
        /// <param name="layerIndex"></param>
        /// <param name="logPath"></param>
        public void InitLog(String layerIndex, String logPath) {
            //��־д��·��
            if (!String.IsNullOrEmpty(logPath)) {
                //��ֵʱ���ִ�
                m_time = GetTimeStr();
                if (!String.IsNullOrEmpty(logPath)) {
                    //·���������򴴽�һ��
                    if (!Directory.Exists(logPath)) {
                        Directory.CreateDirectory(logPath);
                    }
                    //������ʵʱ�Ļ�����ʷ��
                    String logName = "_log_";

                    //�������һ���ļ�·��
                    String filePath = logPath + "\\" + layerIndex + logName + m_time + ".txt";
                    //�ļ�·���ĵ�ַ
                    m_logFilePath = filePath;

                }
            }
        }
        /// <summary>
        /// д����־
        /// </summary>
        /// <param name="msg"></param>
        public void WriteLog(String msg) {
            try {
                lock (this) {
                    if (m_isWriteLog) {
                        if (!File.Exists(m_logFilePath)) {
                            using (FileStream fs = File.Create(m_logFilePath)) {
                                fs.Close();
                                fs.Dispose();
                            }
                        }
                        using (StreamWriter sw = File.AppendText(m_logFilePath)) {
                            sw.WriteLine("��" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "�� " + msg);
                            sw.Flush();
                            sw.Close();
                        }
                    }
                }
            }
            catch { }
        }
        /// <summary>
        /// ��ȡʱ���ʽ
        /// </summary>
        /// <returns></returns>
        private String GetTimeStr() {
            return DateTime.Now.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day;
        }

        static LogHelper log = new LogHelper();
        /// <summary>
        /// �����־�ļ�
        /// </summary>
        /// <param name="logPath"></param>
        /// <param name="logContent"></param>
        public static void LogWrite(string logPath, string flag, string logContent) {
            log.InitLog(flag, logPath);
            log.WriteLog(logContent);
        }

        public static void LogWrite(string flag, string logContent) {
            string logPath = AppDomain.CurrentDomain.BaseDirectory + "logs";
            LogWrite(logPath, flag, logContent);
        }

        public static void LogWrite2(string logPath, string flag, string logContent) {
            string logPath2 = AppDomain.CurrentDomain.BaseDirectory + logPath;
            LogWrite(logPath2, flag, logContent);
        }
    }
}

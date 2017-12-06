using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Serialization;

namespace V5_WinLibs.Utility {
    /// <summary>
    /// ��������Helper
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TaskHelper<T> {
        public delegate void OutTaskRunHandler(TaskHelper<T> obj, T model);
        public delegate void OutDataBaseRunHandler(TaskHelper<T> queue);
        /// <summary>
        /// ί�� ��������
        /// </summary>
        public event OutTaskRunHandler OutTaskRunDelegate;
        /// <summary>
        /// ί�� ��������
        /// </summary>
        public event OutDataBaseRunHandler OutDataBaseRunDelegate;


        public int BaseStepTime = 10;
        public int BaseBusyStepTime = 100;
        public int DbStepTime = 5;
        public int DbBusyStepTime = 300;
        /// <summary>
        /// ���б����ļ�
        /// </summary>
        public string fileToXml = @"config\xml.config";
        //�߳��Ƿ�����
        private bool isStop = true;
        //���̸߳���
        public int ThreadBaseCount = 1;
        //
        public int ThreadDataBaseCount = 1;
        //����������
        public int MaxQueueNum = 500;

        private Thread[] ths;//���߳�
        private Thread[] thsDb;//���߳�
        private Queue<T> queue = new Queue<T>();

        //������
        private static object lockObj = new object();

        //��������
        public void Start() {
            if (this.isStop) {
                this.isStop = false;
                //����δ��ɵ����ݶ���
                this.XmlToList(queue, fileToXml);

                ThreadPool.QueueUserWorkItem(StartBase);
                ThreadPool.QueueUserWorkItem(StartDataBase);
            }
        }

        public void StartBase(object oo) {
            ths = new Thread[ThreadBaseCount];
            for (int i = 0; i < ThreadBaseCount; i++) {
                ths[i] = new Thread(new ParameterizedThreadStart(RunBase));
                ths[i].Start(i);//����object��������
            }
        }

        public void StartDataBase(object oo) {
            thsDb = new Thread[ThreadDataBaseCount];
            for (int i = 0; i < ThreadDataBaseCount; i++) {
                thsDb[i] = new Thread(new ParameterizedThreadStart(RunDataBase));
                thsDb[i].Start(i);//����object��������
            }
        }

        //����ִ�� ���߳�  ʵ�ʴ�������
        private void RunBase(object args) {
            //do something
            int stepTime = BaseStepTime;
            while (true) {
                //while something
                if (this.isStop) {
                    break;
                }
                T model = default(T);
                lock (lockObj) {
                    if (queue.Count > 0) {
                        model = queue.Dequeue();
                        stepTime = BaseStepTime;
                    }
                    else {
                        stepTime = BaseBusyStepTime;
                    }
                }

                if (model != null) {
                    if (OutTaskRunDelegate != null) {
                        OutTaskRunDelegate(this, model);
                    }
                }

                Thread.Sleep(stepTime);
            }
        }
        //ֹͣ����
        public void Stop() {
            this.isStop = true;
            this.UnDoneToXml(queue, fileToXml);
            return;
            if (ths != null) {
                for (int i = 0; i < ThreadBaseCount; i++) {
                    ths[i].Abort();
                }
            }

            if (thsDb != null) {
                for (int i = 0; i < ThreadDataBaseCount; i++) {
                    thsDb[i].Abort();
                }
            }
        }
        //���߳� ����ȡ���ݿ�����
        private void RunDataBase(object args) {
            int stepTime = DbStepTime;
            while (true) {
                if (this.isStop) {
                    break;
                }
                if (queue.Count > MaxQueueNum) {
                    stepTime = DbBusyStepTime;
                }
                else {
                    //�������  �Ƚ��ȳ�
                    if (OutDataBaseRunDelegate != null) {
                        OutDataBaseRunDelegate(this);
                    }
                    stepTime = DbStepTime;
                }
                Thread.Sleep(stepTime);
            }
        }

        #region
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="t"></param>
        public void InsertQueue(T t) {
            lock (lockObj) 
            {
                queue.Enqueue(t);
            }
        }

        /// <summary>
        /// ��ȡ�ڴ����
        /// </summary>
        /// <returns></returns>
        public int GetMemoryCount() {
            lock (lockObj) {
                return queue.Count;
            }
        }

        /// <summary>
        /// ���ش������ݶ���
        /// </summary>
        /// <param name="queue"></param>
        /// <param name="xmlPath"></param>
        private void XmlToList(Queue<T> queue, string xmlPath) {
            List<T> listBuf = new List<T>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
            try {
                string apppath = AppDomain.CurrentDomain.BaseDirectory;
                string fileName = apppath + xmlPath;

                if (!File.Exists(fileName)) {
                    return;
                }

                FileStream fs = new FileStream(fileName, FileMode.Open);
                listBuf = (List<T>)serializer.Deserialize(fs);
                fs.Close();
                foreach (T mt in listBuf) {
                    lock (lockObj) {
                        queue.Enqueue(mt);
                    }
                }
                listBuf.Clear();
                //�������
                Queue<T> queueNull = new Queue<T>();
                UnDoneToXml(queueNull, xmlPath);
            }
            catch {

            }
        }
        /// <summary>
        /// ����δ��ɵ����ݶ���
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ListBuf"></param>
        /// <param name="xmlPath"></param>
        /// <returns></returns>
        private bool UnDoneToXml(Queue<T> queue, string xmlPath) {
            try {
                List<T> ListBuf = queue.ToList();
                string apppath = AppDomain.CurrentDomain.BaseDirectory;
                string fileName = apppath + xmlPath;

                var fileInfo = new FileInfo(fileName);

                if (!Directory.Exists(fileInfo.DirectoryName)) {
                    Directory.CreateDirectory(fileInfo.DirectoryName);

                }

                if (!File.Exists(fileName)) {
                    using (var sw = new StreamWriter(fileName, false, Encoding.UTF8)) {
                        sw.Write("");
                        sw.Flush();
                        sw.Close();
                    }
                }

                lock (lockObj) {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
                    FileStream fs = new FileStream(fileName, FileMode.Create);
                    serializer.Serialize(fs, ListBuf);
                    fs.Close();
                }
                return true;
            }
            catch {
            }
            return false;
        }
        #endregion
    }
}

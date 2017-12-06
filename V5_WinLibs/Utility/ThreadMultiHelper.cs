using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace V5_WinLibs.Utility {
    /// <summary>
    /// ���̷߳�װ
    /// </summary>
    public class ThreadMultiHelper {
        #region ����

        public delegate void DelegateComplete();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskindex">��������</param>
        /// <param name="threadindex">�߳�����</param>
        public delegate void DelegateWork(int taskindex, int threadindex);
        /// <summary>
        /// �������ί��
        /// </summary>
        public event DelegateComplete CompleteEvent;
        /// <summary>
        /// ����������ί��
        /// </summary>
        public event DelegateWork WorkMethod;

        private Thread[] _threads;
        private bool[] _threadState;
        private int _taskCount = 0;//�������
        private int _taskindex = 0;//��������
        private int _threadCount = 5;//Ĭ���߳���

        #endregion

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="taskcount">�������</param>
        public ThreadMultiHelper(int taskcount) {
            _taskCount = taskcount;
        }
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="taskcount">�������</param>
        /// <param name="threadCount">�̸߳���</param>
        public ThreadMultiHelper(int taskcount, int threadCount) {
            _taskCount = taskcount;
            _threadCount = threadCount;
        }

        #region ��ȡ����
        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <returns></returns>
        private int GetTask() {
            lock (this) {
                if (_taskindex < _taskCount) {
                    _taskindex++;
                    return _taskindex;
                }
                else {
                    return 0;
                }
            }
        }
        #endregion

        #region Start

        /// <summary>
        /// ��������
        /// </summary>
        public void Start() {
            _taskindex = 0;
            int num = _taskCount < _threadCount ? _taskCount : _threadCount;
            _threadState = new bool[num];
            _threads = new Thread[num];

            for (int n = 0; n < num; n++) {
                _threadState[n] = false;
                _threads[n] = new Thread(new ParameterizedThreadStart(Work));
                _threads[n].Start(n);
            }
        }

        /// <summary>
        /// �����߳�
        /// </summary>
        public void Stop() {
            for (int i = 0; i < _threads.Length; i++) {
                _threads[i].Abort();
            }
        }
        #endregion

        #region Work
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="arg"></param>
        private void Work(object arg) {
            //��ȡ����ִ��
            int threadindex = int.Parse(arg.ToString());
            int taskindex = GetTask();

            while (taskindex != 0 && WorkMethod != null) {
                WorkMethod(taskindex, threadindex + 1);
                taskindex = GetTask();
            }
            //���е�����ִ�����
            _threadState[threadindex] = true;

            //������ ����������߳�ͬʱ���ֻ����һ������complete�¼�
            lock (this) {
                for (int i = 0; i < _threadState.Length; i++) {
                    if (_threadState[i] == false) {
                        return;
                    }
                }
                //���ȫ�����
                if (CompleteEvent != null) {
                    CompleteEvent();
                }

                //����complete�¼��� �����߳�״̬
                //Ϊ���¸�ͬʱ��ɵ��̲߳���ͨ��������ж�
                for (int j = 0; j < _threadState.Length; j++) {
                    _threadState[j] = false;
                }
            }

        }

        #endregion
    }
}

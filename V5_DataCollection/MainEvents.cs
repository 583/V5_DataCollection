using System;

namespace V5_DataCollection {
    /// <summary>
    /// 
    /// </summary>
    public class MainEvents {
        /// <summary>
        /// ���
        /// </summary>
        public class OutPutWindowEventArgs : EventArgs {
            public int TaskId { get; set; }
            public string Message { set; get; }
        }

        /// <summary>
        /// ������
        /// </summary>
        public class OutPutTaskProgressBarEventArgs : EventArgs {
            /// <summary>
            /// ��ǰ���ȸ���
            /// </summary>
            public int ProgressNum { get; set; }
            /// <summary>
            /// ��¼�ܸ���
            /// </summary>
            public int RecordNum { get; set; }
            /// <summary>
            /// ��������
            /// </summary>
            public int TaskIndex { get; set; }
        }

        /// <summary>
        /// ����������
        /// </summary>
        public class TreeViewEventArgs : EventArgs {
            /// <summary>
            /// �������
            /// </summary>
            public string Result { get; set; }
            /// <summary>
            /// ��Ϣ���
            /// </summary>
            public string Message { get; set; }
            /// <summary>
            /// ���ؽ������(any object)
            /// </summary>
            public object ReturnObj { get; set; }
        }


        /// <summary>
        /// ����ί�ж���
        /// </summary>
        public class CommonEventArgs : EventArgs {
            /// <summary>
            /// �������
            /// </summary>
            public string Result { get; set; }
            /// <summary>
            /// ��Ϣ���
            /// </summary>
            public string Message { get; set; }
            /// <summary>
            /// ���ؽ������(any object)
            /// </summary>
            public object ReturnObj { get; set; }
        }

        public enum OperationEnum {
            Select = 0,
            Add = 1,
            Edit = 2,
            Delete = 3,
            Error = 4
        }

        public class DataOperationArgs : EventArgs {
            /// <summary>
            ///������ʽ
            /// </summary>
            public OperationEnum Operation { get; set; }
            /// <summary>
            /// ��Ϣ���
            /// </summary>
            public string Message { get; set; }
            /// <summary>
            /// ���ؽ������(any object)
            /// </summary>
            public object ReturnObj { get; set; }
        }
    }
}

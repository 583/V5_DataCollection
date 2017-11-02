using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace V5_DataPublish {
    /// <summary>
    /// ϵͳ�¼�
    /// </summary>
    public class MainEvents {
        /// <summary>
        /// ί�в�������
        /// </summary>
        public enum OutPutWindowType {
            Option,
            Collecton,
            Publish
        }
        /// <summary>
        /// �����ί�в���
        /// </summary>
        public class OutPutWindowEventArgs : EventArgs {
            private string _Message = string.Empty;
            private OutPutWindowType _OutPutWindowType;
            private object oData;
            #region Model
            /// <summary>
            /// ��Ϣ
            /// </summary>
            public string Message {
                get { return _Message; }
                set {
                    if (!string.IsNullOrEmpty(value))
                        _Message = "��" + DateTime.Now + "�� " + value;
                }
            }
            /// <summary>
            /// ί������
            /// </summary>
            public OutPutWindowType OutPutWindowType {
                get { return _OutPutWindowType; }
                set { _OutPutWindowType = value; }
            }
            /// <summary>
            /// ��������
            /// </summary>
            public object OData {
                get { return oData; }
                set { oData = value; }
            }
            #endregion
        }
    }
}

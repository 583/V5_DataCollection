using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using V5_Model;

namespace V5_DataCollection.Forms.Task {
    public class TaskEvents {
        /// <summary>
        /// ���ListUrl
        /// </summary>
        public class AddLinkUrlEvents : EventArgs {
            /// <summary>
            /// ��������
            /// </summary>
            public int LinkType { set; get; }
            /// <summary>
            /// �����б�
            /// </summary>
            public ListBox.ObjectCollection LinkObject { set; get; }
        }
        /// <summary>
        /// ���LabelName
        /// </summary>
        public class AddViewLabelEvents : EventArgs {
            /// <summary>
            /// ��������
            /// </summary>
            public string OPType { set; get; }
            /// <summary>
            /// ��ǩ����
            /// </summary>
            public int LabelIndex { set; get; }
            private ModelTaskLabel _LabelModel = new ModelTaskLabel();
            /// <summary>
            /// ��ǩģ��
            /// </summary>
            public ModelTaskLabel LabelModel {
                get { return _LabelModel; }
                set { _LabelModel = value; }
            }
            private ModelTaskLabel _LabelModelOld = new ModelTaskLabel();
            /// <summary>
            /// ��ǩOldģ��
            /// </summary>
            public ModelTaskLabel LabelModelOld {
                get { return _LabelModelOld; }
                set { _LabelModelOld = value; }
            }
        }
        /// <summary>
        /// �������
        /// </summary>
        public class TaskOpEvents : EventArgs {
            /// <summary>
            /// ��������
            /// </summary>
            public int TaskIndex { get; set; }

            private int _OpType = 0;
            /// <summary>
            /// �������� 0 ��� 1Ϊ�޸�
            /// </summary>
            public int OpType {
                get { return _OpType; }
                set { _OpType = value; }
            }

        }
    }
}

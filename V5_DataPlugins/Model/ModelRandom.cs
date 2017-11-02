using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace V5_DataPlugins.Model {
    /// <summary>
    /// ���ֵ
    /// </summary>
    [Serializable]
    public class ModelRandom {
        string _LabelName = string.Empty;
        /// <summary>
        /// ��ǩ����
        /// </summary>
        public string LabelName {
            get { return _LabelName; }
            set { _LabelName = value; }
        }
        string _RandomLabelType = string.Empty;
        /// <summary>
        /// �Ƿ��¼ 0 Ϊδ��¼ 1Ϊ��¼
        /// </summary>
        public string RandomLabelType {
            get { return _RandomLabelType; }
            set { _RandomLabelType = value; }
        }
        string _RandomUrl = string.Empty;
        /// <summary>
        /// ���ʵ�ַ
        /// </summary>
        public string RandomUrl {
            get { return _RandomUrl; }
            set { _RandomUrl = value; }
        }
        string _RandomRefUrl = string.Empty;
        /// <summary>
        /// ������Դ
        /// </summary>
        public string RandomRefUrl {
            get { return _RandomRefUrl; }
            set { _RandomRefUrl = value; }
        }
        string _RandomPostData = string.Empty;
        /// <summary>
        /// ����Post����
        /// </summary>
        public string RandomPostData {
            get { return _RandomPostData; }
            set { _RandomPostData = value; }
        }

        string _RandomCutRegex = string.Empty;
        /// <summary>
        /// ��ȡ���ʽ
        /// </summary>
        public string RandomCutRegex {
            get { return _RandomCutRegex; }
            set { _RandomCutRegex = value; }
        }
    }
}

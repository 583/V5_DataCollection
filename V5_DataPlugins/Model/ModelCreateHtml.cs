using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace V5_DataPlugins.Model {
    /// <summary>
    /// ���ɾ�̬
    /// </summary>
    [Serializable]
    public class ModelCreateHtml {
        string _CreateName = string.Empty;
        /// <summary>
        /// ����
        /// </summary>
        public string CreateName {
            get { return _CreateName; }
            set { _CreateName = value; }
        }
        string _CreateHtmlUrl = string.Empty;
        /// <summary>
        /// ���ɵ�ַ
        /// </summary>
        public string CreateHtmlUrl {
            get { return _CreateHtmlUrl; }
            set { _CreateHtmlUrl = value; }
        }
        string _CreateHtmlRefUrl = string.Empty;
        /// <summary>
        /// ������Դ
        /// </summary>
        public string CreateHtmlRefUrl {
            get { return _CreateHtmlRefUrl; }
            set { _CreateHtmlRefUrl = value; }
        }
        string _CreateHtmlPostData = string.Empty;
        /// <summary>
        /// ��Դ��ַ
        /// </summary>
        public string CreateHtmlPostData {
            get { return _CreateHtmlPostData; }
            set { _CreateHtmlPostData = value; }
        }
    }
}

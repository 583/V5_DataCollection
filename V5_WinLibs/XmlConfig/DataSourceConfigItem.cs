using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace V5_WinLibs.XmlConfig {
    [Serializable]
    public class DataSourceConfigItem {
        int _DataSourceType = 0;
        /// <summary>
        /// ����Դ����
        /// </summary>
        public int DataSourceType {
            get { return _DataSourceType; }
            set { _DataSourceType = value; }
        }

        int _DataBaseType = 0;
        /// <summary>
        /// �Զ������ݱ�������
        /// </summary>
        public int DataBaseType {
            get { return _DataBaseType; }
            set { _DataBaseType = value; }
        }

        string _DataBaseUrl = string.Empty;
        /// <summary>
        /// �Զ������ݿ����ӵ�ַ
        /// </summary>
        public string DataBaseUrl {
            get { return _DataBaseUrl; }
            set { _DataBaseUrl = value; }
        }
        string _SelectSql = string.Empty;
        /// <summary>
        /// ��ѯ���
        /// </summary>
        public string SelectSql {
            get { return _SelectSql; }
            set { _SelectSql = value; }
        }

        string _IndexDataDir = string.Empty;
        /// <summary>
        /// ������Ŀ¼
        /// </summary>
        public string IndexDataDir {
            get { return _IndexDataDir; }
            set { _IndexDataDir = value; }
        }

        string _RemoteDataUrl = string.Empty;
        /// <summary>
        /// �������ݿ��ַ
        /// </summary>
        public string RemoteDataUrl {
            get { return _RemoteDataUrl; }
            set { _RemoteDataUrl = value; }
        }

    }
}

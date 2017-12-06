using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace V5_DataCollection._Class.WebPublishModule {
    /// <summary>
    /// ��վ����ģ��
    /// </summary>
    public class cWebPublishModule {
        private ModelWebPublishModule model = new ModelWebPublishModule();
        private string _LoginCookies = string.Empty;
        /// <summary>
        /// Cookies
        /// </summary>
        public string LoginCookies {
            get { return _LoginCookies; }
            set { _LoginCookies = value; }
        }

        public cWebPublishModule(ModelWebPublishModule _model) {
            model = _model;
        }

        ~cWebPublishModule() {
            model = null;
        }
        /// <summary>
        /// ��ȡ�������Cookies
        /// </summary>
        public void GetBaseCookie(string url) {
            try {
                Uri myUri = new Uri(url);
                WebRequest webRequest = WebRequest.Create(myUri);
                WebResponse webResponse = webRequest.GetResponse();
                LoginCookies = webResponse.Headers["Set-Cookie"];
            }
            catch (Exception) {

            }
        }
        /// <summary>
        /// ��֤�봰��
        /// </summary>
        public void LoginVerCodeWindow() {

        }
        /// <summary>
        /// ��¼��վ
        /// </summary>
        public void LoginCMS() {

        }
        /// <summary>
        /// �����б�
        /// </summary>
        public void GetClassList() {
        }
        /// <summary>
        /// ��������
        /// </summary>
        public void GetView() {

        }
    }
}

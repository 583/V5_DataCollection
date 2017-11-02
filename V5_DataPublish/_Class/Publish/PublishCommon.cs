using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using V5_DataPlugins;
using System.Windows.Forms;
using System.Net;
using System.Collections.Specialized;
using V5_DataPublish._Class.Publish;
using V5_Utility.Core;
using System.Text.RegularExpressions;
using System.Web;
using V5_DataPlugins.Model;
using V5_Utility.Utility;
using V5_WinLibs.Core;

namespace V5_DataPlugins {
    /// <summary>
    /// ʵ��pmod���ط�ʽ��������
    /// </summary>
    public class PublishCommon : IPublish {
        #region ����
        private string _Encode = string.Empty;
        private PluginEventHandler.OutPutResult Out;
        private ModelPublishModuleItem _model;
        private string strLoginDir;
        private string _WebSiteUrl;
        /// <summary>
        /// ��վ����
        /// </summary>
        public string Publish_Encode {
            get {
                return _Encode;
            }
            set {
                _Encode = value;
            }
        }
        /// <summary>
        /// ��������
        /// </summary>
        public PublishType Publish_Type { set; get; }
        /// <summary>
        /// �ط�����
        /// </summary>
        public PluginEventHandler.OutPutResult Publish_OutResult {
            set { Out = value; }
        }
        /// <summary>
        /// WebBrowser����
        /// </summary>
        public WebBrowser Publish_WebBrowser {
            set { throw new NotImplementedException(); }
        }
        /// <summary>
        /// �ļ�����
        /// </summary>
        public string Publish_Name { set; get; }
        /// <summary>
        /// ������վ����
        /// </summary>
        public ModelPublishModuleItem Publish_Model {
            get { return _model; }
            set { _model = value; }
        }
        /// <summary>
        /// ��վ��ַ
        /// </summary>
        public string WebSiteUrl {
            get {
                if (_WebSiteUrl.LastIndexOf("/") == -1) {
                    _WebSiteUrl += "/";
                }
                return _WebSiteUrl;
            }
            set { _WebSiteUrl = value; }
        }
        /// <summary>
        /// �����½Ŀ¼
        /// </summary>
        public string StrLoginDir {
            get {
                if (strLoginDir.LastIndexOf("/") == -1) {
                    strLoginDir += "/";
                }
                return strLoginDir;
            }
            set { strLoginDir = value; }
        }
        #endregion

        #region ˽�б���
        private string cookieHeader = string.Empty;
        private Dictionary<string, string> dic = new Dictionary<string, string>();
        private string strUserName = string.Empty, strUserPwd = string.Empty;
        private string tempLinkUrl = string.Empty, tempLinkContent = string.Empty;
        #endregion

        #region
        /// <summary>
        /// ��ʼ����½��Ϣ
        /// </summary>
        public void Publish_Init(string strSiteUrl, string strLoginDir, string strUserName, string strUserPwd, int isCookie, string strCookie) {
            this.WebSiteUrl = strSiteUrl;
            if (strLoginDir.ToLower().IndexOf("http://") > -1) {
                this.StrLoginDir = strLoginDir;
            }
            else {
                this.StrLoginDir = strSiteUrl + "/" + strLoginDir;
            }
            this.StrLoginDir += "/";
            this.strUserName = strUserName;
            this.strUserPwd = strUserPwd;
        }

        #region ʵ�ַ���

        /// <summary>
        /// ���������
        /// </summary>
        /// <param name="IsLogin">0δ��½ 1��½</param>
        private void Load_RandomList(string RandomLabelType) {
            #region ���������
            Encoding ec = Encoding.GetEncoding("utf-8");
            foreach (ModelRandom m in Publish_Model.ListRandomModel.Where(p => p.RandomLabelType == RandomLabelType)) {
                string randContent = string.Empty;
                if (tempLinkUrl.ToLower() == (StrLoginDir + m.RandomUrl).ToLower()) {
                    randContent = tempLinkContent;
                }
                else {
                    randContent = SimulationHelper.PostPage(StrLoginDir + m.RandomUrl, m.RandomPostData, string.Empty, Publish_Model.PageEncode, ref cookieHeader);
                    tempLinkUrl = StrLoginDir + m.RandomUrl;
                    tempLinkContent = randContent;
                }
                string RandomCutRegex = m.RandomCutRegex;
                RandomCutRegex = RandomCutRegex.Replace("[����]", "([\\S\\s]*?)");
                string CutStrContent = SimulationHelper.CutStr(randContent, RandomCutRegex)[0];
                if (!dic.ContainsKey(m.LabelName)) {
                    dic.Add(m.LabelName, HttpUtility.UrlEncode(CutStrContent, ec));
                }
            }
            #endregion
        }
        /// <summary>
        /// ��̨��½��ַ
        /// </summary>
        public string Publish_GetLoginAdminUrl(string strSiteUrl, string strLoginDir) {
            if (strSiteUrl.LastIndexOf("/") == -1) {
                strSiteUrl += "/";
            }
            if (strLoginDir.LastIndexOf("/") == -1) {
                strLoginDir += "/";
            }
            return strSiteUrl + strLoginDir + Publish_Model.LoginUrl;
        }
        /// <summary>
        /// �滻Post����
        /// </summary>
        /// <returns></returns>
        private string ReplacePostData(string postData) {
            postData = postData.Replace("[�û���]", strUserName);
            postData = postData.Replace("[����]", strUserPwd);
            postData = postData.Replace("[��֤��]", strUserPwd);
            foreach (KeyValuePair<string, string> item in dic) {
                postData = postData.Replace("[" + item.Key + "]", item.Value);
            }
            return postData;
        }
        /// <summary>
        /// �û���½
        /// </summary>
        public void Publish_Login() {
            CacheManageHelper cache = CacheManageHelper.Instance;
            if (cache.Contains(this.WebSiteUrl)) {
                cookieHeader = cache.Get(this.WebSiteUrl) as string;
                Load_RandomList("��½");
                if (Publish_Type == PublishType.Login) {
                    if (Out != null) {
                        Out(this, PublishType.LoginOver, true, "��½�ɹ�!", null);
                    }
                }
            }
            else {
                string postData = Publish_Model.LoginPostData;
                Load_RandomList("��½");
                postData = ReplacePostData(postData);
                string htmlContent = SimulationHelper.PostLogin(this.StrLoginDir + Publish_Model.LoginUrl, postData, string.Empty, Publish_Model.PageEncode, ref cookieHeader);
                if (htmlContent.IndexOf(Publish_Model.LoginSuccessResult) > -1) {
                    cache.Add(this.WebSiteUrl, cookieHeader);
                    if (Publish_Type == PublishType.Login) {
                        if (Out != null) {
                            Out(this, PublishType.LoginOver, true, "��½�ɹ�!", null);
                        }
                    }
                }
                else {
                    if (Publish_Type == PublishType.Login) {
                        if (Out != null) {
                            Out(this, PublishType.LoginOver, false, "��½ʧ��!", null);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="strClassName"></param>
        public void Publish_CreateClass(string strClassName) {
            //��½
            if (string.IsNullOrEmpty(cookieHeader)) {
                Publish_Login();
            }
            Load_RandomList("�б�");
            string postData = Publish_Model.ListCreatePostData;
            postData = ReplacePostData(postData);
            string htmlContent = SimulationHelper.PostPage(StrLoginDir + Publish_Model.ListCreateUrl,
                Publish_Model.ListCreatePostData,
                StrLoginDir + Publish_Model.ListCreateRefUrl,
                Publish_Model.PageEncode,
                ref cookieHeader);
            string[] errorResult = Publish_Model.ListCreateError.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string str in errorResult) {
                if (htmlContent.IndexOf(str) > -1) {
                    if (Out != null) {
                        Out(this, PublishType.CreateClassOver, false, "���ഴ��ʧ��!", strClassName);
                    }
                    break;
                }
            }
            if (!string.IsNullOrEmpty(Publish_Model.ListCreateSuccess)) {
                if (htmlContent.IndexOf(Publish_Model.ContentSuccessResult) > -1) {
                    if (Out != null) {
                        Out(this, PublishType.CreateClassOver, true, "���ഴ���ɹ�!", strClassName);
                    }
                }
                else {
                    if (Out != null) {
                        Out(this, PublishType.CreateClassOver, false, "���ഴ��ʧ��!", strClassName);
                    }
                }
            }
            else {
                if (Out != null) {
                    Out(this, PublishType.CreateClassOver, true, "���ഴ���ɹ�!", strClassName);
                }
            }
        }
        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <returns></returns>
        public void Publish_GetClassList() {
            //��½
            if (string.IsNullOrEmpty(cookieHeader)) {
                Publish_Login();
            }
            string postData = string.Empty;
            string htmlContent = SimulationHelper.PostPage(StrLoginDir + Publish_Model.ListUrl,
                postData,
                StrLoginDir + Publish_Model.ListUrl,
                Publish_Model.PageEncode,
                ref cookieHeader);

            string regexClassID = HtmlHelper.Instance.ParseCollectionStrings(Publish_Model.ListClassIDNameRegex);
            regexClassID = regexClassID.Replace("\\[����:����ID]", "([\\S\\s]*?)");
            regexClassID = regexClassID.Replace("\\[����:��������]", ".+?");
            string[] ArrayClassID = CollectionHelper.Instance.CutStr(htmlContent, regexClassID);

            string regexClassName = HtmlHelper.Instance.ParseCollectionStrings(Publish_Model.ListClassIDNameRegex);
            regexClassName = regexClassName.Replace("\\[����:����ID]", ".+?");
            regexClassName = regexClassName.Replace("\\[����:��������]", "([\\S\\s]*?)");
            string[] ArrayClassName = CollectionHelper.Instance.CutStr(htmlContent, regexClassName);

            List<ModelClassItem> dicClassList = new List<ModelClassItem>();
            for (int i = 0; i < ArrayClassID.Length; i++) {
                string ClassID = ArrayClassID[i];
                string ClassName = ArrayClassName[i];
                ModelClassItem m = new ModelClassItem();
                m.ClassID = ClassID;
                m.ClassName = ClassName;
                dicClassList.Add(m);
            }

            if (Out != null) {
                Out(this, PublishType.GetClassListOver, true, "��ȡ�����б�ɹ�!", dicClassList);
            }
        }

        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="mlistPost"></param>
        /// <param name="mClassList"></param>
        /// <returns></returns>
        public void Publish_PostData(ModelGatherItem mlistPost, ModelClassItem mClassList) {
            //��½
            if (string.IsNullOrEmpty(cookieHeader)) {
                Publish_Login();
            }
            Load_RandomList("����");
            string postData = Publish_Model.ContentPostData;
            postData = ReplacePostData(postData);
            postData = postData.Replace("[����]", mlistPost.Title);
            postData = postData.Replace("[����]", System.Web.HttpUtility.UrlEncode(mlistPost.Content));
            //�滻����
            postData = postData.Replace("[����ID]", mClassList.ClassID);
            postData = postData.Replace("[��������]", mClassList.ClassName);
            string htmlContent = SimulationHelper.PostPage(StrLoginDir + Publish_Model.ContentUrl,
                postData,
                StrLoginDir + Publish_Model.ContentRefUrl,
                Publish_Model.PageEncode,
                ref cookieHeader);
            string[] errorResult = Publish_Model.ContentErrorResult.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string str in errorResult) {
                if (htmlContent.IndexOf(str) > -1) {
                    if (Out != null) {
                        Out(this, PublishType.PostDataOver, false, "���·���ʧ��!", mlistPost);
                    }
                    break;
                }
            }
            if (!string.IsNullOrEmpty(Publish_Model.ContentSuccessResult)) {
                if (htmlContent.IndexOf(Publish_Model.ContentSuccessResult) > -1) {
                    if (Out != null) {
                        Out(this, PublishType.PostDataOver, true, "���·����ɹ�!", mlistPost);
                    }
                }
                else {
                    if (Out != null) {
                        Out(this, PublishType.PostDataOver, false, "���·���ʧ��!", mlistPost);
                    }
                }
            }
            else {
                if (Out != null) {
                    Out(this, PublishType.PostDataOver, true, "���·����ɹ�!", mlistPost);
                }
            }
        }
        #endregion

        #endregion
    }
}

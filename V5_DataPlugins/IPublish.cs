using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using V5_DataPlugins.Model;

namespace V5_DataPlugins {
    /// <summary>
    /// ��������
    /// </summary>
    public enum PublishType {
        Login,
        GetClassList,
        CreateClass,
        PostData,
        UploadFile,
        CreateHtml,
        LoginOver,
        GetClassListOver,
        CreateClassOver,
        PostDataOver,
        UploadFileOver,
        CreateHtmlOver
    }
    /// <summary>
    /// ���ɾ�̬����
    /// </summary>
    public enum CreateHtmlType {
        Index,
        ClassList,
        View
    }
    /// <summary>
    /// �����ӿ�
    /// </summary>
    public interface IPublish {
        /// <summary>
        /// ��������
        /// </summary>
        PublishType Publish_Type { set; get; }
        /// <summary>
        /// ��վ����
        /// </summary>
        string Publish_Encode { set; get; }
        /// <summary>
        /// �ط�����
        /// </summary>
        PluginEventHandler.OutPutResult Publish_OutResult { set; }
        /// <summary>
        /// WebBrowser����
        /// </summary>
        WebBrowser Publish_WebBrowser { set; }
        /// <summary>
        /// �������˵��
        /// </summary>
        string Publish_Name { set; get; }
        /// <summary>
        /// ��վ����
        /// </summary>
        ModelPublishModuleItem Publish_Model { get; set; }
        /// <summary>
        /// ��ʼ����Ϣ
        /// </summary>
        void Publish_Init(string strSiteUrl, string strLoginDir, string strUserName, string strUserPwd, int isCookie, string strCookie);
        /// <summary>
        /// ��̨��½��ַ
        /// </summary>
        string Publish_GetLoginAdminUrl(string strSiteUrl, string strLoginDir);
        /// <summary>
        /// ��վ��̨��¼
        /// </summary>
        void Publish_Login();
        /// <summary>
        /// ��վ��̨��������
        /// </summary>
        void Publish_CreateClass(string strClassName);
        /// <summary>
        /// ��վ��̨��ȡ�����б�
        /// </summary>
        void Publish_GetClassList();
        /// <summary>
        /// ��վ��̨��������
        /// </summary>
        void Publish_PostData(ModelGatherItem mlistPost, ModelClassItem mClassList);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace V5_DataCollection._Class.WebPublishModule
{
    /// <summary>
    /// ����ģ��
    /// </summary>
    [Serializable]
    public class ModelWebPublishModule
    {
        string _ModuleName = string.Empty;
        string _DemoSiteUrl = string.Empty;
        string _PageEncode = string.Empty;
        //
        string _LoginUrl = string.Empty;
        string _LoginRefUrl = string.Empty;
        string _LoginVerCodeUrl = string.Empty;
        string _LoginPostData = string.Empty;
        string _LoginErrorResult = string.Empty;
        string _LoginSuccessResult = string.Empty;
        //
        string _ListUrl = string.Empty;
        string _ListRefUrl = string.Empty;
        string _ListStartCut = string.Empty;
        string _ListEndCut = string.Empty;
        string _ListClassIDRegex = string.Empty;
        //
        string _ViewUrl = string.Empty;
        string _ViewRefUrl = string.Empty;
        string _ViewPostData = string.Empty;
        string _ViewErrorResult = string.Empty;
        string _ViewSuccessResult = string.Empty;
        //
        string _ModuleReadMe = string.Empty;
        //
        /// <summary>
        /// ģ������
        /// </summary>
        public string ModuleName
        {
            get { return _ModuleName; }
            set { _ModuleName = value; }
        }
        /// <summary>
        /// ����վ��
        /// </summary>
        public string DemoSiteUrl
        {
            get { return _DemoSiteUrl; }
            set { _DemoSiteUrl = value; }
        }
        /// <summary>
        /// ҳ�����
        /// </summary>
        public string PageEncode
        {
            get { return _PageEncode; }
            set { _PageEncode = value; }
        }
        /// <summary>
        /// ��¼��ַ
        /// </summary>
        public string LoginUrl
        {
            get { return _LoginUrl; }
            set { _LoginUrl = value; }
        }
        /// <summary>
        /// ��¼��Դ��ַ
        /// </summary>
        public string LoginRefUrl
        {
            get { return _LoginRefUrl; }
            set { _LoginRefUrl = value; }
        }
        /// <summary>
        /// ��֤���ַ
        /// </summary>
        public string LoginVerCodeUrl
        {
            get { return _LoginVerCodeUrl; }
            set { _LoginVerCodeUrl = value; }
        }
        /// <summary>
        /// ��¼Post����
        /// </summary>
        public string LoginPostData
        {
            get { return _LoginPostData; }
            set { _LoginPostData = value; }
        }
        /// <summary>
        /// ��¼����
        /// </summary>
        public string LoginErrorResult
        {
            get { return _LoginErrorResult; }
            set { _LoginErrorResult = value; }
        }
        /// <summary>
        /// ��¼�ɹ�
        /// </summary>
        public string LoginSuccessResult
        {
            get { return _LoginSuccessResult; }
            set { _LoginSuccessResult = value; }
        }
        /// <summary>
        /// �б��ַ
        /// </summary>
        public string ListUrl
        {
            get { return _ListUrl; }
            set { _ListUrl = value; }
        }
        /// <summary>
        /// �б���Դ
        /// </summary>
        public string ListRefUrl
        {
            get { return _ListRefUrl; }
            set { _ListRefUrl = value; }
        }
        /// <summary>
        /// �б�ʼ��ȡ�ַ�
        /// </summary>
        public string ListStartCut
        {
            get { return _ListStartCut; }
            set { _ListStartCut = value; }
        }
        /// <summary>
        /// �б������ȡ�ַ�
        /// </summary>
        public string ListEndCut
        {
            get { return _ListEndCut; }
            set { _ListEndCut = value; }
        }
        /// <summary>
        /// ����ID�ͷ�������
        /// </summary>
        public string ListClassIDRegex
        {
            get { return _ListClassIDRegex; }
            set { _ListClassIDRegex = value; }
        }
        /// <summary>
        /// ������ַ
        /// </summary>
        public string ViewUrl
        {
            get { return _ViewUrl; }
            set { _ViewUrl = value; }
        }
        /// <summary>
        /// ������Դ
        /// </summary>
        public string ViewRefUrl
        {
            get { return _ViewRefUrl; }
            set { _ViewRefUrl = value; }
        }
        /// <summary>
        /// ����Post����
        /// </summary>
        public string ViewPostData
        {
            get { return _ViewPostData; }
            set { _ViewPostData = value; }
        }
        /// <summary>
        /// ��������
        /// </summary>
        public string ViewErrorResult
        {
            get { return _ViewErrorResult; }
            set { _ViewErrorResult = value; }
        }
        /// <summary>
        /// �����ɹ�
        /// </summary>
        public string ViewSuccessResult
        {
            get { return _ViewSuccessResult; }
            set { _ViewSuccessResult = value; }
        }
        /// <summary>
        /// ģ��˵��
        /// </summary>
        public string ModuleReadMe
        {
            get { return _ModuleReadMe; }
            set { _ModuleReadMe = value; }
        }
    }
}

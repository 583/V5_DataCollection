using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;
using V5_DataPlugins.Model;
using V5_DataPlugins;
using V5_Utility.Core;
using V5_WinUtility.Expand;

namespace V5_PublishModule {
    public partial class frmTestModule : Form {
        private string LoginedCookies = string.Empty;
        private string tempLinkUrl = string.Empty, tempLinkContent = string.Empty;
        private Dictionary<string, string> dic = new Dictionary<string, string>();
        private ModelPublishModuleItem model;
        private string modulePath = AppDomain.CurrentDomain.BaseDirectory + "/Modules/";
        public frmTestModule() {
            InitializeComponent();
        }
        private string _ModuleName = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string ModuleName {
            get { return _ModuleName; }
            set { _ModuleName = value; }
        }
        #region ��������
        /// <summary>
        /// �滻���ֵ
        /// </summary>
        /// <param name="labelType"></param>
        /// <returns></returns>
        private void Load_RandomLabel(string labelType) {
            #region ���������
            Encoding ec = Encoding.GetEncoding("utf-8");
            foreach (ModelRandom m in model.ListRandomModel.Where(p => p.RandomLabelType == labelType)) {
                string randContent = string.Empty;
                if (tempLinkUrl.ToLower() == (this.txtTestSiteAdminUrl.Text + m.RandomUrl).ToLower()) {
                    tempLinkUrl = this.txtTestSiteAdminUrl.Text + m.RandomUrl;
                    randContent = tempLinkContent;
                }
                else {
                    randContent = SimulationHelper.PostPage(this.txtTestSiteAdminUrl.Text + m.RandomUrl, m.RandomPostData, string.Empty, model.PageEncode, ref this.LoginedCookies);
                }
                string RandomCutRegex = m.RandomCutRegex;
                RandomCutRegex = RandomCutRegex.Replace("[����]", "([\\S\\s]*?)");
                string CutStrContent = SimulationHelper.CutStr(randContent, RandomCutRegex)[0];
                if (!dic.ContainsKey(m.LabelName)) {
                    dic.Add(m.LabelName, HttpUtility.UrlEncode(CutStrContent, ec));
                    //dic.Add(m.LabelName, CutStrContent);
                }
            }
            #endregion
        }
        /// <summary>
        /// ���ص���ģ������
        /// </summary>
        /// <param name="pathName"></param>
        /// <returns></returns>
        public ModelPublishModuleItem GetModelXml(string pathName) {
            ModelPublishModuleItem model = new ModelPublishModuleItem();
            try {
                string fileName = modulePath + pathName;
                model = (ModelPublishModuleItem)ObjFileStoreHelper.Deserialize(fileName);
            }
            catch {
                model = null;
            }
            return model;
        }
        /// <summary>
        /// ��ģ���б�
        /// </summary>
        private void Bind_ModuleList() {
            try {
                DirectoryInfo di = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "/Modules/");
                FileInfo[] files = di.GetFiles("*.pmod");
                foreach (FileInfo fi in files) {
                    this.cmbModuleList.Items.Add(fi.Name);
                }
            }
            catch {
            }
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTestModule_Load(object sender, EventArgs e) {

            if (!string.IsNullOrEmpty(this.ModuleName)) {
                this.cmbModuleList.Text = this.ModuleName;
            }
            Bind_ModuleList();

            IniHelper.FilePath = AppDomain.CurrentDomain.BaseDirectory + "V5.DataPublishModuleTest.ini";
            this.cmbModuleList.Text = IniHelper.GetIniKeyValue("Config", "ModuleList", "��ѡ��");
            this.txtTestSiteUrl.Text = IniHelper.GetIniKeyValue("Config", "TestSiteUrl", "http://www.v5soft.com");
            this.txtTestSiteAdminUrl.Text = IniHelper.GetIniKeyValue("Config", "TestSiteAdminUrl", "http://www.v5soft.com");
            this.txtTestUserName.Text = IniHelper.GetIniKeyValue("Config", "TestUserName", "abc");
            this.txtTestUserPwd.Text = IniHelper.GetIniKeyValue("Config", "TestUserPwd", "123");
        }
        /// <summary>
        ///  ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveTestConfig_Click(object sender, EventArgs e) {
            IniHelper.FilePath = AppDomain.CurrentDomain.BaseDirectory + "V5.DataPublishModuleTest.ini";
            IniHelper.WriteIniKey("Config", "ModuleList", this.cmbModuleList.Text);
            IniHelper.WriteIniKey("Config", "TestSiteUrl", this.txtTestSiteUrl.Text);
            IniHelper.WriteIniKey("Config", "TestSiteAdminUrl", this.txtTestSiteAdminUrl.Text);
            IniHelper.WriteIniKey("Config", "TestUserName", this.txtTestUserName.Text);
            IniHelper.WriteIniKey("Config", "TestUserPwd", this.txtTestUserPwd.Text);

        }
        #endregion
        private bool IsLogin = false;
        #region ���Ե�½
        /// <summary>
        /// ���Ե�½����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTestLogin_Click(object sender, EventArgs e) {
            this.txtHtmlView.Clear();
            model = this.GetModelXml(this.cmbModuleList.Text);
            Encoding ed = Encoding.GetEncoding(model.PageEncode);
            string PostData = model.LoginPostData;
            PostData = PostData.Replace("[�û���]", this.txtTestUserName.Text);
            PostData = PostData.Replace("[����]", this.txtTestUserPwd.Text);
            //�������ֵ
            this.Load_RandomLabel("��½");
            //�滻���ֵ
            foreach (KeyValuePair<string, string> item in dic) {
                PostData = PostData.Replace("[" + item.Key + "]", item.Value);
            }
            string result = SimulationHelper.PostLogin(this.txtTestSiteAdminUrl.Text + model.LoginChkrl,
                PostData,
                model.LoginRefUrl,
                model.PageEncode,
                ref LoginedCookies);
            this.txtHtmlView.Text = result;
            foreach (string str in model.LoginErrorResult.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)) {
                if (result.IndexOf(str) > -1) {
                    this.txtResultView.Clear();
                    this.txtResultView.AppendText("��¼ʧ��!ֵΪ:" + str);
                    return;
                }
            }
            if (result.IndexOf(model.LoginSuccessResult) > -1) {
                IsLogin = true;
                this.txtResultView.Clear();
                this.txtResultView.AppendText("��¼�ɹ�!CookiesֵΪ:" + LoginedCookies);
            }
            else {
                this.txtResultView.Clear();
                this.txtResultView.Text = result;
            }
        }

        #endregion

        #region �����б�
        /// <summary>
        /// ��ȡ�����б�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTestClassList_Click(object sender, EventArgs e) {
            this.txtHtmlView.Clear();
            this.txtResultView.Clear();
            model = this.GetModelXml(this.cmbModuleList.Text);
            Encoding ed = Encoding.GetEncoding(model.PageEncode);
            if (!IsLogin) {
                btnTestLogin_Click(this, null);
            }
            string result = SimulationHelper.PostPage(
                this.txtTestSiteAdminUrl.Text + model.ListUrl,
                "",
                this.txtTestSiteAdminUrl.Text + model.ListRefUrl,
                model.PageEncode,
                ref this.LoginedCookies);
            this.txtHtmlView.Text = result;
            if (!string.IsNullOrEmpty(model.ListClassIDNameRegex)) {
                string CutClassRegex = model.ListClassIDNameRegex;
                CutClassRegex = HtmlHelper.Instance.ParseCollectionStrings(CutClassRegex);
                CutClassRegex = CutClassRegex.Replace("\\[����:����ID]", "([\\S\\s].*?)");
                CutClassRegex = CutClassRegex.Replace("\\[����:��������]", "([\\S\\s].*?)");
                CutClassRegex = CutClassRegex.Replace("(*)", "[\\S\\s].*?");
                Match mch = null;
                Regex reg = new Regex(CutClassRegex, RegexOptions.IgnoreCase | RegexOptions.Compiled);
                this.cmbClassList.Items.Clear();
                this.cmbViewClassList.Items.Clear();
                for (mch = reg.Match(result); mch.Success; mch = mch.NextMatch()) {
                    string classid = mch.Groups[1].Value;
                    string classname = mch.Groups[2].Value;
                    this.txtResultView.AppendText(classid + "===" + classname + "\r\n");
                    this.cmbClassList.Items.Add(new ListItem(classid, classname));
                    this.cmbViewClassList.Items.Add(new ListItem(classid, classname));
                }
                this.cmbClassList.Items.Insert(0, new ListItem("0", "��ѡ��"));
                this.cmbClassList.SelectedIndex = 0;
                this.cmbViewClassList.Items.Insert(0, new ListItem("0", "��ѡ��"));
                this.cmbViewClassList.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTestClassListCreate_Click(object sender, EventArgs e) {
            this.txtHtmlView.Clear();
            this.txtResultView.Clear();
            model = this.GetModelXml(this.cmbModuleList.Text);
            Encoding ed = Encoding.GetEncoding(model.PageEncode);
            if (!IsLogin) {
                btnTestLogin_Click(null, null);
            }
            string PostData = model.ListCreatePostData;
            PostData = PostData.Replace("[��������]", this.txtClassName.Text);
            //�������ֵ
            this.Load_RandomLabel("�б�");
            //�滻���ֵ
            foreach (KeyValuePair<string, string> item in dic) {
                PostData = PostData.Replace("[" + item.Key + "]", item.Value);
            }
            string result = string.Empty;
            try {
                result = SimulationHelper.PostPage(
                   this.txtTestSiteAdminUrl.Text + model.ListCreateUrl,
                   PostData,
                   this.txtTestSiteAdminUrl.Text + model.ListCreateRefUrl,
                   model.PageEncode,
                   ref this.LoginedCookies);
                this.txtHtmlView.Clear();
                this.txtHtmlView.Text = result;
                this.txtResultView.Clear();
                this.txtResultView.Text = "��������ɹ�!";
            }
            catch (Exception ex) {
                this.txtHtmlView.Clear();
                this.txtHtmlView.Text = result;
                this.txtResultView.Clear();
                this.txtResultView.Text = "��������ʧ��!" + ex.Message + "==" + ex.StackTrace;
            }
        }
        #endregion

        #region ��������
        /// <summary>
        /// ���Է�������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTestContent_Click(object sender, EventArgs e) {
            this.txtHtmlView.Clear();
            this.txtResultView.Clear();
            model = this.GetModelXml(this.cmbModuleList.Text);
            Encoding ed = Encoding.GetEncoding(model.PageEncode);
            if (!IsLogin) {
                btnTestLogin_Click(null, null);
            }
            string PostData = model.ContentPostData;
            PostData = PostData.Replace("[����]", this.txtTitle.Text);
            PostData = PostData.Replace("[����]", this.txtContent.Text);
            ListItem li = (ListItem)this.cmbViewClassList.SelectedItem;
            PostData = PostData.Replace("[����ID]", li.Value);
            PostData = PostData.Replace("[��������]", li.Text);
            //�������ֵ
            this.Load_RandomLabel("����");
            //�滻���ֵ
            foreach (KeyValuePair<string, string> item in dic) {
                PostData = PostData.Replace("[" + item.Key + "]", item.Value);
            }
            string result = string.Empty;
            try {
                result = SimulationHelper.PostPage(
                   this.txtTestSiteAdminUrl.Text + model.ContentUrl,
                   PostData,
                   this.txtTestSiteAdminUrl.Text + model.ContentRefUrl,
                   model.PageEncode,
                   ref this.LoginedCookies);
                this.txtHtmlView.Clear();
                this.txtHtmlView.Text = result;
                this.txtResultView.Clear();
                this.txtResultView.Text = "���ݷ����ɹ�!";
            }
            catch (Exception ex) {
                this.txtHtmlView.Clear();
                this.txtHtmlView.Text = result;
                this.txtResultView.Clear();
                this.txtResultView.Text = "���ݷ���ʧ��!" + ex.Message + "==" + ex.StackTrace;
            }

        }
        #endregion
    }
}

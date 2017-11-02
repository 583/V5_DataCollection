using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using V5_DAL;
using V5_DataPlugins;
using V5_Utility;
using V5_DataPublish._Class;
using V5_Utility.Utility;
using V5_DataPublish._Class.BLL;
using V5_DataPublish._Class.Model;

namespace V5_DataPublish.Forms.Desk {
    public partial class frmHandInsert : Form {
        /// <summary>
        /// ����
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public string Content { get; set; }

        public frmHandInsert() {
            InitializeComponent();
        }

        /// <summary>
        /// ��������
        /// </summary>
        private void btnSubmit_Click(object sender, EventArgs e) {
            this.Invoke(new MethodInvoker(delegate() {
                this.lblProcess.Text = "���Ժ�...�������ڷ�����...";

                this.Save_CheckBoxList();

                if (string.IsNullOrEmpty(this.txtTitle.Text)) {
                    MessageBox.Show(this, "���±��ⲻ��Ϊ��!", "����", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!this.backgroundWorker.IsBusy) {
                    this.backgroundWorker.RunWorkerAsync();
                }

            }));
        }
        /// <summary>
        /// ��������
        /// </summary>
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e) {
            this.Invoke(new MethodInvoker(delegate() {
                try {
                    Title = this.txtTitle.Text;
                    ModelGatherItem m_GatherItem = new ModelGatherItem();
                    m_GatherItem.Title = Title;
                    m_GatherItem.Content = Content;
                    m_GatherItem.CreateTime = DateTime.Now.ToString();
                }
                catch (Exception ex) {
                    MessageBox.Show(this, "���·�������!" + ex.Message + ex.InnerException + ex.StackTrace + ex.Source,
                        "����", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Log4Helper.Write(LogLevel.Error, ex);
                }
            }));
        }
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            this.lblProcess.Text = "�������!";
        }
        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmHandInsert_Load(object sender, EventArgs e) {

            this.txtTitle.Text = this.Title;
            BLLDeskTopPublish bll = new BLLDeskTopPublish();
            List<ModelWebSiteChecked> list = new List<ModelWebSiteChecked>();
            list = bll.GetXmlConfig();
            Bind_WebSiteList(list);
        }
        /// <summary>
        /// ȡ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e) {
            this.Close();
            this.Dispose();
        }
        /// <summary>
        /// �Զ���������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAutoCreateArticle_Click(object sender, EventArgs e) {
            frmAutoCreateArticle FormAutoCreateArticle = new frmAutoCreateArticle();
            FormAutoCreateArticle.ShowDialog(this);
        }
        /// <summary>
        /// ѡ��һ��վ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectWebSite_Click(object sender, EventArgs e) {
            frmSelectGroupSite formSelectGroupSite = new frmSelectGroupSite();
            formSelectGroupSite.OutModel = OutModel;
            formSelectGroupSite.Show(this);
        }
        /// <summary>
        /// ѡ��վ�����ί��
        /// </summary>
        /// <param name="strMsg"></param>
        private void OutModel(string strMsg, string Text, string Value) {

            List<ModelWebSiteChecked> list = new List<ModelWebSiteChecked>();
            list.Add(new ModelWebSiteChecked() {
                Name = Text,
                IsChecked = true,
                Value = Value
            });
            Bind_WebSiteList(list);
        }
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="MeOutPut"></param>
        private void PublishOP(object sender, MainEvents.OutPutWindowEventArgs MeOutPut) {
            MessageBox.Show(MeOutPut.Message);
        }

        /// <summary>
        /// ����Ҫ������վ�����
        /// </summary>
        /// <param name="list"></param>
        private void Bind_WebSiteList(List<ModelWebSiteChecked> list) {

        }

        private void Save_CheckBoxList() {

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using V5_DAL;
using V5_Utility;
using V5_Utility.Core;
using V5_DataPublish._Class;
using System.Web.UI.WebControls;

namespace V5_DataPublish.Forms.Desk {
    public partial class frmSelectGroupSite : Form {
        /// <summary>
        /// վ����Ϣί��
        /// </summary>
        /// <param name="strMsg"></param>
        public delegate void OutWebSiteModelHandler(string strMsg, string Text, string Value);
        public OutWebSiteModelHandler OutModel;
        public frmSelectGroupSite() {
            InitializeComponent();
        }
        #region
        /// <summary>
        /// ��վ���б�
        /// </summary>
        private void Bind_WebSiteList(string GroupClassID) {
            this.cmbWebSiteList.Items.Clear();
            var list = Common.GetList<WebSiteHelper>(p => p.ClassID == GroupClassID);
            this.cmbWebSiteList.Items.Add(new ListItem("0", "��ѡ��һ��վ��"));
            foreach (var l in list) {
                ListItem li = new ListItem(l.Uuid, l.WebSiteName);
                this.cmbWebSiteList.Items.Add(li);
            }
            this.cmbWebSiteList.SelectedIndex = 0;
        }
        /// <summary>
        /// վȺ����ѡ��ı�
        /// </summary>
        private void cmbSiteClassList_SelectedIndexChanged(object sender, EventArgs e) {
            string GroupClassID = ((ListItem)this.cmbSiteClassList.SelectedItem).Value;
            Bind_WebSiteList(GroupClassID);
        }
        #endregion
        /// <summary>
        /// �����ʼ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmSelectGroupSite_Load(object sender, EventArgs e) {
            this.lblResult.Text = string.Empty;
            var list = Common.GetList<ModelTreeClass>(p => p.Uuid != string.Empty);
            this.cmbSiteClassList.Items.Add(new ListItem("0", "��ѡ��һ��վȺ����"));
            foreach (var l in list) {
                ListItem li = new ListItem(l.Uuid, l.ClassName);
                this.cmbSiteClassList.Items.Add(li);
            }
            this.cmbSiteClassList.SelectedIndex = 0;
            string GroupClassID = ((ListItem)this.cmbSiteClassList.SelectedItem).Value;
            Bind_WebSiteList(GroupClassID);
            int WebSiteID = int.Parse(((ListItem)this.cmbWebSiteList.SelectedItem).Value);
        }
        /// <summary>
        /// ȷ��վ����Ϣ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmitSite_Click(object sender, EventArgs e) {
            ListItem GroupSiteClass = (ListItem)(this.cmbSiteClassList.SelectedItem);
            int GroupClassID = int.Parse(GroupSiteClass.Value);
            ListItem WebSiteList = (ListItem)(this.cmbWebSiteList.SelectedItem);
            int WebSiteID = int.Parse(WebSiteList.Value);
            ListItem WebSiteClassList = (ListItem)(this.cmbClassList.SelectedItem);
            if (GroupClassID == 0
                || WebSiteID == 0) {
                this.lblResult.Text = "��ѡ��������վ��!";
                return;
            }
            string TempString = WebSiteID.ToString();
            if (!string.IsNullOrEmpty(TempString)) {
                if (OutModel != null) {
                    OutModel(TempString,
                        GroupSiteClass.Text + "��" + WebSiteList.Text + "��" + WebSiteClassList.Text,
                        GroupSiteClass.Value + "��" + WebSiteList.Value + "��" + WebSiteClassList.Value);
                }
            }
            this.Close();
            this.Dispose();
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

        private void cmbWebSiteList_SelectedIndexChanged(object sender, EventArgs e) {
            int WebSiteID = int.Parse(((ListItem)this.cmbWebSiteList.SelectedItem).Value);
            Bind_WebSiteClassList(WebSiteID);
        }

        /// <summary>
        /// ���ط���
        /// </summary>
        private void Bind_WebSiteClassList(int WebSiteID) {
            this.cmbClassList.Items.Clear();
            this.cmbClassList.Items.Add(new ListItem("0", "��ѡ��"));
            DALWebSiteClassList dal = new DALWebSiteClassList();
            DataSet ds = dal.GetClassList(WebSiteID.ToString());
            if (ds != null && ds.Tables[0].Rows.Count > 0) {
                foreach (DataRow dr in ds.Tables[0].Rows) {
                    this.cmbClassList.Items.Add(new ListItem(dr["ClassID"].ToString(), dr["ClassName"].ToString()));
                }
            }
            this.cmbClassList.SelectedIndex = 0;
        }
    }
}

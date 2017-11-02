using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace V5_DataPublish.Forms.Desk {
    public partial class frmPageContentEdit : Form {
        /// <summary>
        /// ���ݱ༭ί��
        /// </summary>
        /// <param name="strTitle"></param>
        /// <param name="strContent"></param>
        public delegate void ReturnContentEventHandler(string strTitle, string strContent);
        public ReturnContentEventHandler rcEH;
        /// <summary>
        /// ����
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public string Content { get; set; }
        public frmPageContentEdit() {
            InitializeComponent();
        }
        /// <summary>
        /// �����˳�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e) {
            this.Close();
            this.Dispose();
        }
        /// <summary>
        /// �޸�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmit_Click(object sender, EventArgs e) {
            if (string.IsNullOrEmpty(this.txtTitle.Text)) {
                MessageBox.Show("���±��ⲻ��Ϊ��!", "����", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Close();
            this.Dispose();
        }
        /// <summary>
        /// �����ʼ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmPageContentEdit_Load(object sender, EventArgs e) {
            this.txtTitle.Text = this.Title;
        }
        /// <summary>
        /// �༭����ʼ������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void txtFckEditorContent_OnEditorInitialized(object sender, EventArgs e) {
            
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using V5_DataCollection.Forms.LeftTree;
using V5_DataCollection.Forms.Task;
using V5_DataCollection._Class.DAL;
using V5_WinLibs.Core;

namespace V5_DataCollection.Forms.Docking {
    /// <summary>
    /// ����������
    /// </summary>
    public partial class frmTreeBox : BaseContent
    {
        /// <summary>
        /// �����������¼�
        /// </summary>
        public event MainEventHandler.TreeViewEventHandler OpOver;

        public frmTreeBox() {
            InitializeComponent();
        }

        /// <summary>
        /// ��ӷ���ڵ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton_AddTaskTreeClass_Click(object sender, EventArgs e) {
            frmLeftTreeClass formLeftTreeClass = new frmLeftTreeClass();
            formLeftTreeClass.OutOpMsg = OutOpMsg;
            formLeftTreeClass.ShowDialog();
        }
        /// <summary>
        /// �༭����ڵ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton_EditTaskTreeClass_Click(object sender, EventArgs e) {
            TreeNode node = this.treeView_TaskList.SelectedNode;
            if (node != this.treeView_TaskList.Nodes[0]
                && node != this.treeView_TaskList.Nodes[1]) {
                frmLeftTreeClass formLeftTreeClass = new frmLeftTreeClass();
                formLeftTreeClass.EditValue = node.Tag.ToString();
                formLeftTreeClass.OutOpMsg = OutOpMsg;
                formLeftTreeClass.ShowDialog();
            }
        }
        /// <summary>
        /// ������ί�з���
        /// </summary>
        /// <param name="opType"></param>
        /// <param name="Message"></param>
        private void OutOpMsg(string opType, string Message) {
            if (opType == "add"
                || opType == "edit") {
                this.Bind_TreeDataList();
            }
        }
        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmTreeBox_Load(object sender, EventArgs e) {
            this.Bind_TreeDataList();

            if (this.treeView_TaskList.Nodes[1].Nodes.Count > 0) {
                this.treeView_TaskList.Nodes[1].Nodes[0].Checked = true;
            }
        }
        /// <summary>
        /// �����������ڵ�
        /// </summary>
        private void Bind_TreeDataList() {
            DALTaskClass dal = new DALTaskClass();
            DataTable dt = dal.GetList(string.Empty).Tables[0];
            TreeNode rootNode = this.treeView_TaskList.Nodes[1];
            rootNode.Nodes.Clear();
            foreach (DataRow dr in dt.Rows) {
                TreeNode tn = new TreeNode();
                tn.Name = dr["ClassID"].ToString();
                tn.Text = dr["TreeClassName"].ToString();
                tn.Tag = dr["ClassID"].ToString();
                rootNode.Nodes.Add(tn);
            }
            rootNode.ExpandAll();
        }
        /// <summary>
        /// ��ӷ���ڵ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_AddTaskTreeClass_Click(object sender, EventArgs e) {
            frmLeftTreeClass formLeftTreeClass = new frmLeftTreeClass();
            formLeftTreeClass.OutOpMsg = OutOpMsg;
            formLeftTreeClass.ShowDialog();
        }
        /// <summary>
        /// �༭����ڵ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_EditTaskTreeClass_Click(object sender, EventArgs e) {
            TreeNode node = this.treeView_TaskList.SelectedNode;
            if (node != this.treeView_TaskList.Nodes[0]
                || node != this.treeView_TaskList.Nodes[1]) {
                frmLeftTreeClass formLeftTreeClass = new frmLeftTreeClass();
                formLeftTreeClass.EditValue = node.Tag.ToString();
                formLeftTreeClass.OutOpMsg = OutOpMsg;
                formLeftTreeClass.ShowDialog();
            }
        }
        /// <summary>
        /// ɾ������ڵ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripMenuItem_DelTaskTreeClass_Click(object sender, EventArgs e) {
            TreeNode node = this.treeView_TaskList.SelectedNode;
            if (node != this.treeView_TaskList.Nodes[0]
                || node != this.treeView_TaskList.Nodes[1]) {
                if (MessageBox.Show("��ȷ��Ҫɾ����?ɾ����������ɾ��!", "����!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK) {
                    int ID = StringHelper.Instance.SetNumber(node.Tag.ToString());
                    DALTaskClass dal = new DALTaskClass();
                    dal.Delete(ID);
                    this.Bind_TreeDataList();
                }
            }
        }
        /// <summary>
        /// ɾ������ڵ�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButton_DelTaskTreeClass_Click(object sender, EventArgs e) {
            TreeNode node = this.treeView_TaskList.SelectedNode;
            if (node != this.treeView_TaskList.Nodes[0]
                || node != this.treeView_TaskList.Nodes[1]) {
                if (MessageBox.Show("��ȷ��Ҫɾ����?ɾ����������ɾ��!", "����!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK) {
                    int ID = StringHelper.Instance.SetNumber(node.Tag.ToString());
                    DALTaskClass dal = new DALTaskClass();
                    dal.Delete(ID);
                    this.Bind_TreeDataList();
                }
            }
        }

        /// <summary>
        /// �������������¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_TaskList_AfterSelect(object sender, TreeViewEventArgs e) {
            TreeNode SelectedNode = this.treeView_TaskList.SelectedNode;
            if (SelectedNode.Parent == this.treeView_TaskList.Nodes[0]) {
                MessageBox.Show("��ѡ��һ����ȷ�ڵ��ڽ��в���!");
                return;
            }
            else if (SelectedNode.Parent == this.treeView_TaskList.Nodes[1]) {
                if (SelectedNode != this.treeView_TaskList.Nodes[1]) {
                    if (OpOver != null) {
                        OpOver(this, new MainEvents.TreeViewEventArgs() {
                            Result = "selectednode",
                            Message = "ѡ��ڵ�ɹ�!",
                            ReturnObj = SelectedNode.Tag
                        });
                    }
                }
            }
            else if (SelectedNode == this.treeView_TaskList.Nodes[0]) {
                if (OpOver != null) {
                    OpOver(this, new MainEvents.TreeViewEventArgs() {
                        Result = "selectednodetask",
                        Message = "ѡ��ڵ�ɹ�!",
                        ReturnObj = SelectedNode.Tag
                    });
                }
            }
        }
    }
}

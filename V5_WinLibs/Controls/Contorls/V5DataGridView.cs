using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace V5_WinControls {
    public partial class V5DataGridView : DataGridView {
        public V5DataGridView() {
            InitializeComponent();
        }

        public V5DataGridView(IContainer container) {
            container.Add(this);

            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            ////�½����и�
            ////�ܸ߶� �����и�
            ////����ܿ�
            //// �洢��������λ�ü��߶�
            //// �洢��������λ�ü��߶�
            //// �洢��������λ�ü��߶�
            ////��
            ////���ƿ�
            ////�����ɫ
            //// ��װͼ.FillRectangle(New SolidBrush(Me.RowHeadersDefaultCellStyle.BackColor), rowHeader
        }
    }
}

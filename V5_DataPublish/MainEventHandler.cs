using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace V5_DataPublish {
    public class MainEventHandler {
        /// <summary>
        /// �������
        /// </summary>
        public delegate void OutPutWindowHandler(object sender, MainEvents.OutPutWindowEventArgs e);
        /// <summary>
        /// ��־ί��
        /// </summary>
        public delegate void PublishOutPutWindowHandler(object sender, MainEvents.OutPutWindowEventArgs e);
    }
}

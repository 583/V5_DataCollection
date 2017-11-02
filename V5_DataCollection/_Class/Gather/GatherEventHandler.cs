using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using V5_Model;

namespace V5_DataCollection._Class.Gather {
    public class GatherEventHandler {
        /// <summary>
        /// �ɼ�������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void GatherWorkHandler(object sender, GatherEvents.GatherLinkEvents e);
        /// <summary>
        /// �ɼ����
        /// </summary>
        /// <param name="model"></param>
        public delegate void GatherComplateHandler(ModelTask model);
    }
}

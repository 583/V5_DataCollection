using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace V5_DataPlugins {
    public class PluginEventHandler {
       /// <summary>
        /// �ص����ͽ��
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="pt"></param>
       /// <param name="isLogin"></param>
       /// <param name="Msg"></param>
       /// <param name="Result"></param>
        public delegate void OutPutResult(object sender, PublishType pt, bool isLogin, string Msg, object Result);
    }
}

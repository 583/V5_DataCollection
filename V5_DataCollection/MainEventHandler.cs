
namespace V5_DataCollection {
    /// <summary>
    /// 
    /// </summary>
    public class MainEventHandler {
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void OutPutWindowHandler(object sender, MainEvents.OutPutWindowEventArgs e);

        /// <summary>
        /// ���������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void OutPutTaskProgressBarHandler(object sender, MainEvents.OutPutTaskProgressBarEventArgs e);

        /// <summary>
        ///������ί��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void TreeViewEventHandler(object sender, MainEvents.TreeViewEventArgs e);

        /// <summary>
        /// ����ί��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void CommonEventHandler(object sender, MainEvents.CommonEventArgs e);

        /// <summary>
        /// ���ݲ���ί��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void DataOperationHandler(object sender, MainEvents.DataOperationArgs e);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using V5_DataPlugins;
using V5_DataPlugins.Model;
using V5_WinLibs.DBUtility;

namespace V5_DataPublish._Class.DataSource {
    //ʹ��SqlServer����Դ��������
    public class PublishTaskSqlServerHelper : IPublishTaskHelper {

        public List<ModelPublishItem> GetDataList(string keyword) {
            return GetDataList(keyword, 0);
        }

        public List<ModelPublishItem> GetDataList(string keyword, int topNum) {
            List<ModelPublishItem> LItem = new List<ModelPublishItem>();
            string SQLKeyword = string.Empty;
            //ƴ��SQL���
            SqlParameter[] parameter = { 
                                       new SqlParameter("@SQLKeyword",SqlDbType.VarChar,500),
                                       new SqlParameter("@TopNum",SqlDbType.Int,4),
                                       };
            parameter[0].Value = SQLKeyword;
            parameter[1].Value = topNum;
            DataSet ds = DbHelperSQL.RunProcedure("[dbo].[Pro_GetArtileList]", parameter, "ds");
            if (ds != null && ds.Tables[0].Rows.Count > 0) {
                foreach (DataRow dr in ds.Tables[0].Rows) {
                    ModelPublishItem model = new ModelPublishItem();
                    model.Title = dr["Title"].ToString();
                    model.Content = dr["Content"].ToString();
                    model.Abstract = dr["Abstract"].ToString();
                    model.Url = dr["Url"].ToString();
                    model.Time = Convert.ToDateTime(dr["AddDateTime"].ToString());
                    LItem.Add(model);
                }
            }
            return LItem;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using V5_DAL;
using System.Data;
using System.Threading;
using System.IO;
using V5_Utility;
using V5_DataPlugins;
using V5_Utility.Core;
using V5_DataPublish._Class.BaiduHelper;
using System.Windows.Forms;
using V5_DataPlugins.Model;
using V5_DataPublish._Class;
using V5_Utility.Utility;
using V5_WinLibs.Core;
using V5_WinLibs.DBUtility;

namespace V5_DataPublish._Class.Publish {
    public class PublishTask {
        #region ����
        public MainEventHandler.PublishOutPutWindowHandler PublishOP;
        MainEvents.OutPutWindowEventArgs MeOutPut = new MainEvents.OutPutWindowEventArgs();
        public delegate void OverOpHandler(WebSiteHelper model);
        public OverOpHandler OverOP;
        #endregion

        #region ˽��
        private WebSiteHelper ModelWebSite = new WebSiteHelper();
        private string ClassID = string.Empty, ClassName = string.Empty, Keywords = string.Empty;
        private IPublish iPublish;
        private ModelGatherItem mGatherItem;
        private ModelClassItem mClassList;
        private string Title = string.Empty, Content = string.Empty;
        public string modulePath = AppDomain.CurrentDomain.BaseDirectory + "/Modules/";
        #endregion

        public PublishTask() {
        }

        #region   �����������
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="model"></param>
        public void ThreadSendContent(WebSiteHelper model) {
            ModelWebSite = model;
            int DataSourceType = ModelWebSite.DataSourceType.Value;
            if (DataSourceType == 2) {
                MeOutPut.Message = string.Format("��վ����:{0} ��վ��ַ:{1} ��Ϣ:{2} ����ʧ��!",
                            ModelWebSite.WebSiteName, ModelWebSite.WebSiteUrl, "������ȡ���ݽӿ����ڿ�����..,");
                PublishOP(this, MeOutPut);
                return;
            }
            SendArticleContentClassList(DataSourceType);
        }
        /// <summary>
        /// ���������ⲿ���
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Title"></param>
        /// <param name="Content"></param>
        /// <param name="ClassName"></param>
        /// <param name="ClassID"></param>
        public void CommonSendContent(WebSiteHelper model,
            string Title, string Content, string ClassName, string ClassID) {
            this.SendArticleContent(model, Title, Content, ClassName, ClassID);
        }
        #endregion
        /// <summary>
        /// ���෢������
        /// </summary>
        private void SendArticleContentClassList(int DataSourceType) {
            DataSet dsClassList = new DALWebSiteClassList().GetClassList(ModelWebSite.ID.ToString());
            if (dsClassList != null && dsClassList.Tables[0].Rows.Count > 0) {
                if (DataSourceType == 4) {
                    this.SendArticleContentByNetWork(dsClassList.Tables[0]);
                    return;
                }
                if (DataSourceType == 3) {
                    this.SendArticleContentByFileDir();
                }
            }

            foreach (DataRow drClass in dsClassList.Tables[0].Rows) {
                ClassID = drClass["ClassID"].ToString();
                ClassName = drClass["ClassName"].ToString();
                Keywords = drClass["KeywordList"].ToString();
                switch (DataSourceType) {
                    case 1:
                        this.SendArticleContentByDataBase();
                        break;
                    case 2:
                        break;
                    case 3:
                        this.SendArticleContentByFileDir();
                        break;
                    case 4:
                        break;
                }
            }
        }



        private void SendArticleContent(WebSiteHelper model, string Title, string Content, string ClassName, string ClassID) {
            ModelWebSite = model;
            try {
                LoadPublishModule(ModelWebSite.PublishName);
                if (iPublish != null) {
                    Title = Title.Replace("'", "''");
                    Content = Content.Replace("'", "''");

                    mGatherItem = new ModelGatherItem();
                    mGatherItem.Title = Title;
                    mGatherItem.Content = Content;

                    mClassList = new ModelClassItem();
                    mClassList.ClassID = ClassID;
                    mClassList.ClassName = ClassName;

                    iPublish.Publish_OutResult = OPR_SendData;
                    iPublish.Publish_Init(ModelWebSite.WebSiteUrl,
                        ModelWebSite.WebSiteLoginUrl,
                        ModelWebSite.LoginUserName,
                        ModelWebSite.LoginUserPwd,
                        0,
                        string.Empty);
                    iPublish.Publish_Type = PublishType.PostData;
                    iPublish.Publish_PostData(mGatherItem, mClassList);

                }
                else {
                    MeOutPut.Message = string.Format("���������!���߲������!");
                    if (PublishOP != null) {
                        PublishOP(this, MeOutPut);
                    }
                }
            }
            catch (Exception ex) {
                MeOutPut.Message = string.Format("��վ����:{0} ��վ��ַ:{1} ��Ϣ:{2} ����ʧ��!",
                               ModelWebSite.WebSiteName, ModelWebSite.WebSiteUrl, ex.Message);
                if (PublishOP != null) {
                    PublishOP(this, MeOutPut);
                }
            }
        }

        private void OPR_SendData(object sender, PublishType pt, bool isLogin, string Msg, object oResult) {
            if (pt == PublishType.Login) {
                MessageBox.Show(Msg.ToString());
            }
            else if (pt == PublishType.LoginOver) {
                MessageBox.Show(Msg.ToString());
            }
            else if (pt == PublishType.PostData) {
                iPublish.Publish_PostData(mGatherItem, mClassList);
            }
            else if (pt == PublishType.PostDataOver) {
                this.BakDataBase(Title, Content);
            }
        }


        #region ����Դ�������ݿ�
        private void SendArticleContentByDataBase() {
            try {
                if (ModelWebSite.DataSourceType == 1) {
                    string strWhere = string.Empty;
                    if (!string.IsNullOrEmpty(Keywords)) {
                        string[] KeywordsArr = Keywords.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string key in KeywordsArr) {
                            strWhere += " And (Title like '%" + key + "%' And Content like '%" + key + "%') ";
                        }
                    }
                    DbHelperSQL.connectionString = ModelWebSite.DataLinkUrl;
                    DataTable dt = DbHelperSQL.Query(ModelWebSite.DataQuerySQL.Replace("[����]", strWhere)).Tables[0];
                    foreach (DataRow dr in dt.Rows) {
                        Title = dr["Title"].ToString();
                        Content = dr["Content"].ToString();
                        if (!CheckArticleContentIsSend(Title, Content)) {
                            this.SendArticleContent(ModelWebSite, Title, Content, ClassName, ClassID);
                        }
                        Thread.Sleep(0);
                    }
                }
            }
            catch (Exception ex) {
                MeOutPut.Message = string.Format("��վ����:{0} ��վ��ַ:{1} ��Ϣ:{2} ����ʧ��!",
                                ModelWebSite.WebSiteName, ModelWebSite.WebSiteUrl, ex.Message);
                PublishOP(this, MeOutPut);
            }

        }
        #endregion

        #region ����Դ����������

        #endregion

        #region ����Դ�����ļ���
        private void SendArticleContentByFileDir() {

            string Title = string.Empty, Content = string.Empty;
            if (ModelWebSite.DataSourceType == 3) {
                string[] files = Directory.GetFiles(ModelWebSite.FileSourcePath, "*.html");
                int lLen = files.Length;
                for (int i = 0; i < lLen; i++) {
                    try {
                        string file = files[i];
                        FileInfo fiFile = new FileInfo(file);
                        Title = fiFile.Name.Replace(fiFile.Extension, "");
                        int l = fiFile.Name.LastIndexOf("_");
                        if (l > -1) {
                            Title = Title.Substring(0, l);
                        }
                        StreamReader sr = new StreamReader(file, Encoding.Default);
                        Content = sr.ReadToEnd();
                        sr.Close();
                        if (!CheckArticleContentIsSend(Title, Content)) {
                            this.SendArticleContent(ModelWebSite, Title, Content, ClassName, "1");
                        }
                        else {
                            MeOutPut.Message = string.Format("��ַ:{0} ����:{1} ����:{2} ���´���!",
                             ModelWebSite.WebSiteUrl, ClassName, Title);
                            if (PublishOP != null) {
                                PublishOP(this, MeOutPut);
                            }
                        }
                        File.Delete(file);
                        Thread.Sleep(0);
                    }
                    catch (Exception ex) {
                        MeOutPut.Message = string.Format("��վ����:{0} ��վ��ַ:{1} ��Ϣ:{2} ����ʧ��!",
                                       ModelWebSite.WebSiteName, ModelWebSite.WebSiteUrl, ex.Message);
                        if (PublishOP != null) {
                            PublishOP(this, MeOutPut);
                        }
                    }
                }
                if (OverOP != null) {
                    OverOP(ModelWebSite);
                }
            }
        }
        #endregion

        #region ����Դ��������
        private void SendArticleContentByNetWork(DataTable dtClassList) {
            try {
                string Title = string.Empty, Content = string.Empty;
                if (ModelWebSite.DataSourceType == 4) {

                    while (true) {
                        foreach (DataRow dr in dtClassList.Rows) {
                            ClassID = dr["ClassID"].ToString();
                            ClassName = dr["ClassName"].ToString();
                            string dataDir = ModelWebSite.ID + "-" + ClassID;
                            string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "\\Data\\NetWork\\" + dataDir + "\\", "*.html");
                            int lLen = files.Length;
                            for (int i = 0; i < lLen; i++) {
                                try {
                                    string file = files[i];
                                    FileInfo fiFile = new FileInfo(file);
                                    Title = fiFile.Name.Replace(fiFile.Extension, "");
                                    int l = fiFile.Name.LastIndexOf("_");
                                    if (l > -1) {
                                        Title = Title.Substring(0, l);
                                    }
                                    StreamReader sr = new StreamReader(file, Encoding.Default);
                                    Content = sr.ReadToEnd();
                                    sr.Close();
                                    if (!CheckArticleContentIsSend(Title, Content)) {
                                        this.SendArticleContent(ModelWebSite, Title, Content, ClassName, ClassID);
                                    }
                                    else {
                                        MeOutPut.Message = string.Format("��ַ:{0} ����:{1} ����:{2} ���´���!",
                                         ModelWebSite.WebSiteUrl, ClassName, Title);
                                        if (PublishOP != null) {
                                            PublishOP(this, MeOutPut);
                                        }
                                    }
                                    File.Delete(file);
                                    Thread.Sleep(1);
                                }
                                catch (Exception ex) {
                                    MeOutPut.Message = string.Format("��վ����:{0} ��վ��ַ:{1} ��Ϣ:{2} ����ʧ��!",
                                                   ModelWebSite.WebSiteName, ModelWebSite.WebSiteUrl, ex.Message);
                                    if (PublishOP != null) {
                                        PublishOP(this, MeOutPut);
                                    }
                                }
                            }
                            if (OverOP != null) {
                                OverOP(ModelWebSite);
                            }
                            Thread.Sleep(6000);
                        }
                    }
                }
            }
            catch (Exception ex) {
                Log4Helper.Write(LogLevel.Error, ex);
            }
        }
        #endregion

        #region αԭ��  pdf pic word
        /// <summary>
        /// αԭ��  pdf pic word
        /// </summary>
        private void OutFalseOriginal(ref string Title, ref string Content) {
            #region ���±���αԭ��
            Dictionary<string, string> retDict = null;
            //�����Զ�αԭ��
            if (ModelWebSite.IsTitleFalse.Value == 1) {
                if (retDict == null)
                    retDict = (Dictionary<string, string>)cFalseOriginalHelper.FalseOriginalWords();
                foreach (var item in retDict) {
                    Title = cFalseOriginalHelper.FalseOriginalData(Title, item.Key, item.Value, 4);
                }
            }
            //�����Զ�αԭ��
            if (ModelWebSite.IsContentFalse.Value == 1) {
                if (retDict == null)
                    retDict = (Dictionary<string, string>)cFalseOriginalHelper.FalseOriginalWords();
                foreach (var item in retDict) {
                    Content = cFalseOriginalHelper.FalseOriginalData(Content, item.Key, item.Value, 4);
                }
            }
            retDict = null;
            #endregion
            #region ���������ͼƬ ����ͼƬ�б� ͼƬ��СΪ200X300 �첽����

            #endregion
            #region �Զ�����  pdf word <200 ���ַ�  ���Prֵ �첽����

            #endregion
        }
        #endregion

        #region SQLite����
        /// <summary>
        /// �����ļ�
        /// </summary>
        /// <param name="sWebSiteID"></param>
        public void CreateDataFile(string sWebSiteID) {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\Spider\\";
            string SQLiteName = baseDir + sWebSiteID + "\\SpiderResult.db";
            string LocalSQLiteName = "Data\\Spider\\" + sWebSiteID + "\\SpiderResult.db";

            string SQL = string.Empty;
            if (!Directory.Exists(baseDir + sWebSiteID + "\\")) {
                Directory.CreateDirectory(baseDir + sWebSiteID + "\\");
            }

            string PdfDir = baseDir + sWebSiteID + "\\Pdf\\";
            string ImagesDir = baseDir + sWebSiteID + "\\Images\\";
            string WordDir = baseDir + sWebSiteID + "\\Word\\";
            string VideoDir = baseDir + sWebSiteID + "\\Video\\";
            if (!Directory.Exists(PdfDir)) {
                Directory.CreateDirectory(PdfDir);
            }
            if (!Directory.Exists(ImagesDir)) {
                Directory.CreateDirectory(ImagesDir);
            }
            if (!Directory.Exists(WordDir)) {
                Directory.CreateDirectory(WordDir);
            }
            if (!Directory.Exists(VideoDir)) {
                Directory.CreateDirectory(VideoDir);
            }
            if (!File.Exists(SQLiteName)) {
                DbHelper.CreateDataBase(SQLiteName);
                string createSQL = string.Empty;
                SQL = @"
                create table Content(
                    ID integer primary key autoincrement,
                    Title varchar,
                    Content varchar,
                    AddDateTime varchar
                );
            ";
                DbHelper.Execute(LocalSQLiteName, SQL);
            }
        }
        /// <summary>
        /// ��������Ƿ񷢲�����
        /// </summary>
        private bool CheckArticleContentIsSend(string Title, string Content) {
            int sWebSiteID = ModelWebSite.ID;
            string baseDir = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\Spider\\";
            string SQLiteName = baseDir + sWebSiteID + "\\SpiderResult.db";
            string LocalSQLiteName = "Data\\Spider\\" + sWebSiteID + "\\SpiderResult.db";
            if (File.Exists(SQLiteName)) {
                string SQL = string.Empty;
                SQL = string.Format(@"Select Count(1) From Content Where Title='{0}' ", Title, Content.Replace("'", "''"), DateTime.Now.ToString());
                int result = StringHelper.Instance.SetNumber(DbHelper.Execute(LocalSQLiteName, SQL));
                if (result > 0) {
                    return true;
                }
                return false;
            }
            return false;
        }

        /// <summary>
        /// ��������
        /// </summary>
        private void BakDataBase(string Title, string Content) {
            int sWebSiteID = ModelWebSite.ID;
            string baseDir = AppDomain.CurrentDomain.BaseDirectory + "\\Data\\Spider\\";
            string SQLiteName = baseDir + sWebSiteID + "\\SpiderResult.db";
            string LocalSQLiteName = "Data\\Spider\\" + sWebSiteID + "\\SpiderResult.db";
            if (!File.Exists(SQLiteName)) {
                CreateDataFile(sWebSiteID.ToString());
            }
            else {
                string SQL = string.Empty;
                SQL = string.Format(@"Insert Into  Content(Title,Content,AddDateTime)values
                        ('{0}','{1}','{2}')
                        ", Title, Content, DateTime.Now.ToString());
                DbHelper.Execute(LocalSQLiteName, SQL);
            }
        }
        #endregion

        #region
        /// <summary>
        /// ���ط���ģ��
        /// </summary>
        /// <param name="publishName"></param>
        public void LoadPublishModule(string publishName) {
            if (publishName.IndexOf(".pmod") > -1) {
                iPublish = new PublishCommon();
                iPublish.Publish_OutResult = OPR_SendData;
                iPublish.Publish_Name = publishName;
                iPublish.Publish_Model = Load_PublishItem(publishName);
            }
            else {
                iPublish = Utility.ListIPublish.Where(p => p.Publish_Name == ModelWebSite.PublishName).FirstOrDefault();
                iPublish.Publish_OutResult = OPR_SendData;
                iPublish.Publish_Name = ModelWebSite.PublishName;
            }
        }
        /// <summary>
        /// ���ص���ģ������
        /// </summary>
        /// <param name="pathName"></param>
        private ModelPublishModuleItem Load_PublishItem(string pathName) {
            ModelPublishModuleItem model = new ModelPublishModuleItem();
            try {
                string fileName = modulePath + pathName;
                model = (ModelPublishModuleItem)ObjFileStoreHelper.Deserialize(fileName);
            }
            catch {
                model = null;
            }
            return model;
        }
        #endregion

    }
}

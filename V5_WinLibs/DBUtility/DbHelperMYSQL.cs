using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace V5_WinLibs.DBUtility
{
    public abstract class DbHelperMySQL
    {
         public static string connectionString = PubConstant.ConnectionString;
         public DbHelperMySQL()
         {
         }

         #region  ִ�м�SQL���

         public static int GetMaxID(string FieldName, string TableName)
         {
             string strsql = "select max(" + FieldName + ")+1 from " + TableName;
             object obj = DbHelperMySQL.GetSingle(strsql);
             if (obj == null)
             {
                 return 1;
             }
             else
             {
                 return int.Parse(obj.ToString());
             }
         }
         /// <summary>
         /// ִ��SQL��䣬����Ӱ��ļ�¼��
         /// </summary>
         /// <param name="SQLString">SQL���</param>
         /// <returns>Ӱ��ļ�¼��</returns>
         public static int ExecuteSql(string SQLString)
         {
             using (MySqlConnection connection = new MySqlConnection(connectionString))
             {
                 using (MySqlCommand cmd = new MySqlCommand(SQLString, connection))
                 {
                     try
                     {
                         connection.Open();
                         int rows = cmd.ExecuteNonQuery();
                         return rows;
                     }
                     catch (MySqlException E)
                     {
                         connection.Close();
                         throw new Exception(E.Message);
                     }
                 }
             }
         }

         /// <summary>
         /// ִ�ж���SQL��䣬ʵ�����ݿ�����
         /// </summary>
         /// <param name="SQLStringList">����SQL���</param>		
         public static void ExecuteSqlTran(ArrayList SQLStringList)
         {
             using (MySqlConnection conn = new MySqlConnection(connectionString))
             {
                 conn.Open();
                 MySqlCommand cmd = new MySqlCommand();
                 cmd.Connection = conn;
                 MySqlTransaction tx = conn.BeginTransaction();
                 cmd.Transaction = tx;
                 try
                 {
                     for (int n = 0; n < SQLStringList.Count; n++)
                     {
                         string strsql = SQLStringList[n].ToString();
                         if (strsql.Trim().Length > 1)
                         {
                             cmd.CommandText = strsql;
                             cmd.ExecuteNonQuery();
                         }
                     }
                     tx.Commit();
                 }
                 catch (MySqlException E)
                 {
                     tx.Rollback();
                     throw new Exception(E.Message);
                 }
             }
         }
         /// <summary>
         /// ִ�ж���SQL��䣬ʵ�����ݿ�����
         /// </summary>
         /// <param name="SQLStringList">����SQL���</param>	
         public static int ExecuteSqlTran(List<String> SQLStringList)
         {
             using (MySqlConnection connection = new MySqlConnection(connectionString))
             {
                 connection.Open();
                 MySqlCommand cmd = new MySqlCommand();
                 cmd.Connection = connection;
                 MySqlTransaction tx = connection.BeginTransaction();
                 cmd.Transaction = tx;
                 try
                 {
                     int count = 0;
                     for (int n = 0; n < SQLStringList.Count; n++)
                     {
                         string strsql = SQLStringList[n];
                         if (strsql.Trim().Length > 1)
                         {
                             cmd.CommandText = strsql;
                             count += cmd.ExecuteNonQuery();
                         }
                     }
                     tx.Commit();
                     return count;
                 }
                 catch
                 {
                     tx.Rollback();
                     return 0;
                 }
                 finally
                 {
                     cmd.Dispose();
                     connection.Close();
                 }
             }
         }
         /// <summary>
         /// ִ�д�һ���洢���̲����ĵ�SQL��䡣
         /// </summary>
         /// <param name="SQLString">SQL���</param>
         /// <param name="content">��������,����һ���ֶ��Ǹ�ʽ���ӵ����£���������ţ�����ͨ�������ʽ���</param>
         /// <returns>Ӱ��ļ�¼��</returns>
         public static int ExecuteSql(string SQLString, string content)
         {
             using (MySqlConnection connection = new MySqlConnection(connectionString))
             {
                 MySqlCommand cmd = new MySqlCommand(SQLString, connection);
                 MySqlParameter myParameter = new MySqlParameter("@content", MySqlDbType.VarChar);
                 myParameter.Value = content;
                 cmd.Parameters.Add(myParameter);
                 try
                 {
                     connection.Open();
                     int rows = cmd.ExecuteNonQuery();
                     return rows;
                 }
                 catch (MySqlException E)
                 {
                     throw new Exception(E.Message);
                 }
                 finally
                 {
                     cmd.Dispose();
                     connection.Close();
                 }
             }
         }
         /// <summary>
         /// �����ݿ������ͼ���ʽ���ֶ�(������������Ƶ���һ��ʵ��)
         /// </summary>
         /// <param name="strSQL">SQL���</param>
         /// <param name="fs">ͼ���ֽ�,���ݿ���ֶ�����Ϊimage�����</param>
         /// <returns>Ӱ��ļ�¼��</returns>
         public static int ExecuteSqlInsertImg(string strSQL, byte[] fs)
         {
             using (MySqlConnection connection = new MySqlConnection(connectionString))
             {
                 MySqlCommand cmd = new MySqlCommand(strSQL, connection);
                 MySqlParameter myParameter = new MySqlParameter("@fs", MySqlDbType.Binary);
                 myParameter.Value = fs;
                 cmd.Parameters.Add(myParameter);
                 try
                 {
                     connection.Open();
                     int rows = cmd.ExecuteNonQuery();
                     return rows;
                 }
                 catch (MySqlException E)
                 {
                     throw new Exception(E.Message);
                 }
                 finally
                 {
                     cmd.Dispose();
                     connection.Close();
                 }
             }
         }

         /// <summary>
         /// ִ��һ�������ѯ�����䣬���ز�ѯ�����object����
         /// </summary>
         /// <param name="SQLString">�����ѯ������</param>
         /// <returns>��ѯ�����object��</returns>
         public static object GetSingle(string SQLString)
         {
             using (MySqlConnection connection = new MySqlConnection(connectionString))
             {
                 using (MySqlCommand cmd = new MySqlCommand(SQLString, connection))
                 {
                     try
                     {
                         connection.Open();
                         object obj = cmd.ExecuteScalar();
                         if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                         {
                             return null;
                         }
                         else
                         {
                             return obj;
                         }
                     }
                     catch (MySqlException e)
                     {
                         connection.Close();
                         throw new Exception(e.Message);
                     }
                 }
             }
         }
         /// <summary>
         /// ִ�в�ѯ��䣬����MySqlDataReader
         /// </summary>
         /// <param name="strSQL">��ѯ���</param>
         /// <returns>MySqlDataReader</returns>
         public static MySqlDataReader ExecuteReader(string strSQL)
         {
             MySqlConnection connection = new MySqlConnection(connectionString);
             MySqlCommand cmd = new MySqlCommand(strSQL, connection);
             try
             {
                 connection.Open();
                 MySqlDataReader myReader = cmd.ExecuteReader();
                 return myReader;
             }
             catch (MySqlException e)
             {
                 throw new Exception(e.Message);
             }

         }
         /// <summary>
         /// ִ�в�ѯ��䣬����DataSet
         /// </summary>
         /// <param name="SQLString">��ѯ���</param>
         /// <returns>DataSet</returns>
         public static DataSet Query(string SQLString)
         {
             using (MySqlConnection connection = new MySqlConnection(connectionString))
             {
                 DataSet ds = new DataSet();
                 try
                 {
                     connection.Open();
                     MySqlDataAdapter command = new MySqlDataAdapter(SQLString, connection);
                     command.Fill(ds, "ds");
                 }
                 catch (MySqlException ex)
                 {
                     throw new Exception(ex.Message);
                 }
                 return ds;
             }
         }


         #endregion
    }
}

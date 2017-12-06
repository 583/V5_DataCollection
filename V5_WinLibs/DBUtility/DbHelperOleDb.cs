using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Collections.Generic;
using System.Web;

namespace V5_WinLibs.DBUtility
{
	/// <summary>
	/// 数据访问基础类(基于OleDb)
	/// 可以用户可以修改满足自己项目的需要。
	/// </summary>
	public abstract class DbHelperOleDb
	{
        public static string connectionString = PubConstant.ConnectionString;   
		public DbHelperOleDb()
		{			
		}

        private static string ConnectionConn(string connectionString)
        {
            return "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + HttpContext.Current.Server.MapPath(connectionString)+";";
        }
		#region  执行简单SQL语句

        public static bool Exists(string strSql)
        {
            object obj = DbHelperOleDb.GetSingle(strSql);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static int GetMaxID(string FieldName, string TableName)
        {
            string strsql = "select max(" + FieldName + ")+1 from " + TableName;
            object obj = DbHelperOleDb.GetSingle(strsql);
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
		/// 执行SQL语句，返回影响的记录数
		/// </summary>
		/// <param name="SQLString">SQL语句</param>
		/// <returns>影响的记录数</returns>
		public static int ExecuteSql(string SQLString)
		{
            using (OleDbConnection connection = new OleDbConnection(ConnectionConn(connectionString)))
			{				
				using (OleDbCommand cmd = new OleDbCommand(SQLString,connection))
				{
					try
					{		
						connection.Open();
						int rows=cmd.ExecuteNonQuery();
						return rows;
					}
					catch(System.Data.OleDb.OleDbException E)
					{					
						connection.Close();
						throw new Exception(E.Message);
					}
				}				
			}
		}
		
		/// <summary>
		/// 执行多条SQL语句，实现数据库事务。
		/// </summary>
		/// <param name="SQLStringList">多条SQL语句</param>		
		public static void ExecuteSqlTran(ArrayList SQLStringList)
		{
            using (OleDbConnection conn = new OleDbConnection(ConnectionConn(connectionString)))
			{
				conn.Open();
				OleDbCommand cmd = new OleDbCommand();
				cmd.Connection=conn;				
				OleDbTransaction tx=conn.BeginTransaction();			
				cmd.Transaction=tx;				
				try
				{   		
					for(int n=0;n<SQLStringList.Count;n++)
					{
						string strsql=SQLStringList[n].ToString();
						if (strsql.Trim().Length>1)
						{
							cmd.CommandText=strsql;
							cmd.ExecuteNonQuery();
						}
					}										
					tx.Commit();					
				}
				catch(System.Data.OleDb.OleDbException E)
				{		
					tx.Rollback();
					throw new Exception(E.Message);
				}
			}
		}
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>	
        public static int ExecuteSqlTran(List<String> SQLStringList)
        {
            using (OleDbConnection connection = new OleDbConnection(ConnectionConn(connectionString)))
            {
                connection.Open();
                OleDbCommand cmd = new OleDbCommand();
                cmd.Connection = connection;
                OleDbTransaction tx = connection.BeginTransaction();
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
		/// 执行带一个存储过程参数的的SQL语句。
		/// </summary>
		/// <param name="SQLString">SQL语句</param>
		/// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
		/// <returns>影响的记录数</returns>
		public static int ExecuteSql(string SQLString,string content)
		{
            using (OleDbConnection connection = new OleDbConnection(ConnectionConn(connectionString)))
			{
				OleDbCommand cmd = new OleDbCommand(SQLString,connection);		
				System.Data.OleDb.OleDbParameter  myParameter = new System.Data.OleDb.OleDbParameter ( "@content", OleDbType.VarChar);
				myParameter.Value = content ;
				cmd.Parameters.Add(myParameter);
				try
				{
					connection.Open();
					int rows=cmd.ExecuteNonQuery();
					return rows;
				}
				catch(System.Data.OleDb.OleDbException E)
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
		/// 向数据库里插入图像格式的字段(和上面情况类似的另一种实例)
		/// </summary>
		/// <param name="strSQL">SQL语句</param>
		/// <param name="fs">图像字节,数据库的字段类型为image的情况</param>
		/// <returns>影响的记录数</returns>
		public static int ExecuteSqlInsertImg(string strSQL,byte[] fs)
		{
            using (OleDbConnection connection = new OleDbConnection(ConnectionConn(connectionString)))
			{
				OleDbCommand cmd = new OleDbCommand(strSQL,connection);	
				System.Data.OleDb.OleDbParameter  myParameter = new System.Data.OleDb.OleDbParameter ( "@fs", OleDbType.Binary);
				myParameter.Value = fs ;
				cmd.Parameters.Add(myParameter);
				try
				{
					connection.Open();
					int rows=cmd.ExecuteNonQuery();
					return rows;
				}
				catch(System.Data.OleDb.OleDbException E)
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
		/// 执行一条计算查询结果语句，返回查询结果（object）。
		/// </summary>
		/// <param name="SQLString">计算查询结果语句</param>
		/// <returns>查询结果（object）</returns>
		public static object GetSingle(string SQLString)
		{
            using (OleDbConnection connection = new OleDbConnection(ConnectionConn(connectionString)))
			{
				using(OleDbCommand cmd = new OleDbCommand(SQLString,connection))
				{
					try
					{
						connection.Open();
						object obj = cmd.ExecuteScalar();
						if((Object.Equals(obj,null))||(Object.Equals(obj,System.DBNull.Value)))
						{					
							return null;
						}
						else
						{
							return obj;
						}				
					}
					catch(System.Data.OleDb.OleDbException e)
					{						
						connection.Close();
						throw new Exception(e.Message);
					}	
				}
			}
		}
		/// <summary>
		/// 执行查询语句，返回OleDbDataReader
		/// </summary>
		/// <param name="strSQL">查询语句</param>
		/// <returns>OleDbDataReader</returns>
		public static OleDbDataReader ExecuteReader(string strSQL)
		{
            OleDbConnection connection = new OleDbConnection(ConnectionConn(connectionString));			
			OleDbCommand cmd = new OleDbCommand(strSQL,connection);				
			try
			{
				connection.Open();	
				OleDbDataReader myReader = cmd.ExecuteReader();
				return myReader;
			}
			catch(System.Data.OleDb.OleDbException e)
			{								
				throw new Exception(e.Message);
			}			
			
		}		
		/// <summary>
		/// 执行查询语句，返回DataSet
		/// </summary>
		/// <param name="SQLString">查询语句</param>
		/// <returns>DataSet</returns>
		public static DataSet Query(string SQLString)
		{
            using (OleDbConnection connection = new OleDbConnection(ConnectionConn(connectionString)))
			{
				DataSet ds = new DataSet();
				try
				{
					connection.Open();
					OleDbDataAdapter command = new OleDbDataAdapter(SQLString,connection);				
					command.Fill(ds,"ds");
				}
				catch(System.Data.OleDb.OleDbException ex)
				{				
					throw new Exception(ex.Message);
				}			
				return ds;
			}			
		}


		#endregion

		#region 执行带参数的SQL语句

		/// <summary>
		/// 执行SQL语句，返回影响的记录数
		/// </summary>
		/// <param name="SQLString">SQL语句</param>
		/// <returns>影响的记录数</returns>
		public static int ExecuteSql(string SQLString,params OleDbParameter[] cmdParms)
		{
			using (OleDbConnection connection = new OleDbConnection(connectionString))
			{				
				using (OleDbCommand cmd = new OleDbCommand())
				{
					try
					{		
						PrepareCommand(cmd, connection, null,SQLString, cmdParms);
						int rows=cmd.ExecuteNonQuery();
						cmd.Parameters.Clear();
						return rows;
					}
					catch(System.Data.OleDb.OleDbException E)
					{				
						throw new Exception(E.Message);
					}
				}				
			}
		}
		
			
		/// <summary>
		/// 执行多条SQL语句，实现数据库事务。
		/// </summary>
		/// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的OleDbParameter[]）</param>
		public static void ExecuteSqlTran(Hashtable SQLStringList)
		{			
			using (OleDbConnection conn = new OleDbConnection(connectionString))
			{
				conn.Open();
				using (OleDbTransaction trans = conn.BeginTransaction()) 
				{
					OleDbCommand cmd = new OleDbCommand();
					try 
					{
						foreach (DictionaryEntry myDE in SQLStringList)
						{	
							string 	cmdText=myDE.Key.ToString();
							OleDbParameter[] cmdParms=(OleDbParameter[])myDE.Value;
							PrepareCommand(cmd,conn,trans,cmdText, cmdParms);
							int val = cmd.ExecuteNonQuery();
							cmd.Parameters.Clear();

							trans.Commit();
						}					
					}
					catch 
					{
						trans.Rollback();
						throw;
					}
				}				
			}
		}
	
				
		/// <summary>
		/// 执行一条计算查询结果语句，返回查询结果（object）。
		/// </summary>
		/// <param name="SQLString">计算查询结果语句</param>
		/// <returns>查询结果（object）</returns>
		public static object GetSingle(string SQLString,params OleDbParameter[] cmdParms)
		{
			using (OleDbConnection connection = new OleDbConnection(connectionString))
			{
				using (OleDbCommand cmd = new OleDbCommand())
				{
					try
					{
						PrepareCommand(cmd, connection, null,SQLString, cmdParms);
						object obj = cmd.ExecuteScalar();
						cmd.Parameters.Clear();
						if((Object.Equals(obj,null))||(Object.Equals(obj,System.DBNull.Value)))
						{					
							return null;
						}
						else
						{
							return obj;
						}				
					}
					catch(System.Data.OleDb.OleDbException e)
					{				
						throw new Exception(e.Message);
					}					
				}
			}
		}
		
		/// <summary>
		/// 执行查询语句，返回OleDbDataReader
		/// </summary>
		/// <param name="strSQL">查询语句</param>
		/// <returns>OleDbDataReader</returns>
		public static OleDbDataReader ExecuteReader(string SQLString,params OleDbParameter[] cmdParms)
		{		
			OleDbConnection connection = new OleDbConnection(connectionString);
			OleDbCommand cmd = new OleDbCommand();				
			try
			{
				PrepareCommand(cmd, connection, null,SQLString, cmdParms);
				OleDbDataReader myReader = cmd.ExecuteReader();
				cmd.Parameters.Clear();
				return myReader;
			}
			catch(System.Data.OleDb.OleDbException e)
			{								
				throw new Exception(e.Message);
			}					
			
		}		
		
		/// <summary>
		/// 执行查询语句，返回DataSet
		/// </summary>
		/// <param name="SQLString">查询语句</param>
		/// <returns>DataSet</returns>
		public static DataSet Query(string SQLString,params OleDbParameter[] cmdParms)
		{
			using (OleDbConnection connection = new OleDbConnection(connectionString))
			{
				OleDbCommand cmd = new OleDbCommand();
				PrepareCommand(cmd, connection, null,SQLString, cmdParms);
				using( OleDbDataAdapter da = new OleDbDataAdapter(cmd) )
				{
					DataSet ds = new DataSet();	
					try
					{												
						da.Fill(ds,"ds");
						cmd.Parameters.Clear();
					}
					catch(System.Data.OleDb.OleDbException ex)
					{				
						throw new Exception(ex.Message);
					}			
					return ds;
				}				
			}			
		}


		private static void PrepareCommand(OleDbCommand cmd,OleDbConnection conn,OleDbTransaction trans, string cmdText, OleDbParameter[] cmdParms) 
		{
			if (conn.State != ConnectionState.Open)
				conn.Open();
			cmd.Connection = conn;
			cmd.CommandText = cmdText;
			if (trans != null)
				cmd.Transaction = trans;
			cmd.CommandType = CommandType.Text;
			if (cmdParms != null) 
			{
				foreach (OleDbParameter parm in cmdParms)
					cmd.Parameters.Add(parm);
			}
		}

		#endregion

		#region 存储过程操作

		/// <summary>
		/// 执行存储过程
		/// </summary>
		/// <param name="storedProcName">存储过程名</param>
		/// <param name="parameters">存储过程参数</param>
		/// <returns>OleDbDataReader</returns>
		public static OleDbDataReader RunProcedure(string storedProcName, IDataParameter[] parameters )
		{
			OleDbConnection connection = new OleDbConnection(connectionString);
			OleDbDataReader returnReader;
			connection.Open();
			OleDbCommand command = BuildQueryCommand( connection,storedProcName, parameters );
			command.CommandType = CommandType.StoredProcedure;
			returnReader = command.ExecuteReader();				
			return returnReader;			
		}
		
		
		/// <summary>
		/// 执行存储过程
		/// </summary>
		/// <param name="storedProcName">存储过程名</param>
		/// <param name="parameters">存储过程参数</param>
		/// <param name="tableName">DataSet结果中的表名</param>
		/// <returns>DataSet</returns>
		public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName )
		{
			using (OleDbConnection connection = new OleDbConnection(connectionString))
			{
				DataSet dataSet = new DataSet();
				connection.Open();
				OleDbDataAdapter sqlDA = new OleDbDataAdapter();
				sqlDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters );
				sqlDA.Fill( dataSet, tableName );
				connection.Close();
				return dataSet;
			}
		}

		
		/// <summary>
		/// 构建 OleDbCommand 对象(用来返回一个结果集，而不是一个整数值)
		/// </summary>
		/// <param name="connection">数据库连接</param>
		/// <param name="storedProcName">存储过程名</param>
		/// <param name="parameters">存储过程参数</param>
		/// <returns>OleDbCommand</returns>
		private static OleDbCommand BuildQueryCommand(OleDbConnection connection,string storedProcName, IDataParameter[] parameters)
		{			
			OleDbCommand command = new OleDbCommand( storedProcName, connection );
			command.CommandType = CommandType.StoredProcedure;
			foreach (OleDbParameter parameter in parameters)
			{
				command.Parameters.Add( parameter );
			}
			return command;			
		}
		
		/// <summary>
		/// 执行存储过程，返回影响的行数		
		/// </summary>
		/// <param name="storedProcName">存储过程名</param>
		/// <param name="parameters">存储过程参数</param>
		/// <param name="rowsAffected">影响的行数</param>
		/// <returns></returns>
		public static int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected )
		{
			using (OleDbConnection connection = new OleDbConnection(connectionString))
			{
				int result;
				connection.Open();
				OleDbCommand command = BuildIntCommand(connection,storedProcName, parameters );
				rowsAffected = command.ExecuteNonQuery();
				result = (int)command.Parameters["ReturnValue"].Value;
				return result;
			}
		}
		
		/// <summary>
		/// 创建 OleDbCommand 对象实例(用来返回一个整数值)	
		/// </summary>
		/// <param name="storedProcName">存储过程名</param>
		/// <param name="parameters">存储过程参数</param>
		/// <returns>OleDbCommand 对象实例</returns>
		private static OleDbCommand BuildIntCommand(OleDbConnection connection,string storedProcName, IDataParameter[] parameters)
		{
			OleDbCommand command = BuildQueryCommand(connection,storedProcName, parameters );
			command.Parameters.Add( new OleDbParameter ( "ReturnValue",
				OleDbType.Integer,4,ParameterDirection.ReturnValue,
				false,0,0,string.Empty,DataRowVersion.Default,null ));
			return command;
		}
		#endregion	

	}
}

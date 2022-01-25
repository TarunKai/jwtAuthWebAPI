using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;



namespace jwtAuthWebAPI.Helper
{
	public class SqlHelper
	{
		private string strConnectionString = "";



		public SqlHelper()
		{
			strConnectionString = ConfigurationManager.ConnectionStrings["iAutomateDocsConnection"].ConnectionString;
		}
		public SqlHelper(string cnnStr)
		{
			strConnectionString = ConfigurationManager.ConnectionStrings[cnnStr].ConnectionString;
		}



		public int ExecuteNonQuery(string query)
		{
			int retval = 0;
			using (SqlConnection cnnStr = new SqlConnection(strConnectionString))
			{
				SqlCommand command = new SqlCommand(query, cnnStr);
				command.CommandTimeout = 600;
				if (query.StartsWith("INSERT") | query.StartsWith("insert") | query.StartsWith("UPDATE") | query.StartsWith("update") | query.StartsWith("DELETE") | query.StartsWith("delete") | query.StartsWith("truncate"))
				{
					command.CommandType = CommandType.Text;
				}
				else
				{
					command.CommandType = CommandType.StoredProcedure;
				}

				try
				{
					cnnStr.Open();
					retval = command.ExecuteNonQuery();



				}
				catch (Exception exp)
				{
				}
				finally
				{
					if (cnnStr.State == ConnectionState.Open)
					{
						cnnStr.Close();
						cnnStr.Dispose();
					}
				}
			}
			return retval;
		}



		public int ExecuteNonQuery(string query, params SqlParameter[] parameters)
		{
			int retval = 0;
			using (SqlConnection cnnStr = new SqlConnection(strConnectionString))
			{
				SqlCommand command = new SqlCommand(query, cnnStr);
				command.CommandTimeout = 600;
				try
				{
					if (query.StartsWith("SET") | query.StartsWith("INSERT") | query.StartsWith("insert") | query.StartsWith("UPDATE") | query.StartsWith("update") | query.StartsWith("DELETE") | query.StartsWith("delete"))
					{
						command.CommandType = CommandType.Text;
					}
					else
					{
						command.CommandType = CommandType.StoredProcedure;
					}
					for (int i = 0; i <= parameters.Length - 1; i++)
					{
						command.Parameters.Add(parameters[i]);
					}
					cnnStr.Open();
					retval = command.ExecuteNonQuery();
					command.Parameters.Clear();
					cnnStr.Close();
				}
				catch (Exception ex)
				{



				}
				finally
				{
					cnnStr.Close();
					cnnStr.Dispose();
				}
			}
			return retval;
		}
		public object ExecuteScalarSalary(string query)
		{
			object retval = null;
			using (SqlConnection cnnStr = new SqlConnection(strConnectionString))
			{
				SqlCommand command = new SqlCommand(query, cnnStr);
				command.CommandTimeout = 600;
				try
				{
					if (query.StartsWith("SELECT") | query.StartsWith("select") | query.StartsWith("if"))
					{
						command.CommandType = CommandType.Text;
					}
					else
					{
						command.CommandType = CommandType.StoredProcedure;
					}
					cnnStr.Open();
					retval = command.ExecuteScalar();
					cnnStr.Close();
				}
				catch (Exception ex)
				{
				}
				finally
				{
					cnnStr.Close();
					cnnStr.Dispose();
				}
			}



			return retval;
		}



		public object ExecuteScalar(string query)
		{
			object retval = null;
			using (SqlConnection cnnStr = new SqlConnection(strConnectionString))
			{
				SqlCommand command = new SqlCommand(query, cnnStr);
				command.CommandTimeout = 600;
				try
				{
					if (query.StartsWith("SELECT") | query.StartsWith("select"))
					{
						command.CommandType = CommandType.Text;
					}
					else
					{
						command.CommandType = CommandType.StoredProcedure;
					}
					cnnStr.Open();
					retval = command.ExecuteScalar();
					cnnStr.Close();
				}
				catch (Exception ex)
				{
				}
				finally
				{
					cnnStr.Close();
					cnnStr.Dispose();
				}
			}
			return retval;
		}



		public object ExecuteScalar(string query, params SqlParameter[] parameters)
		{
			object retval = null;
			using (SqlConnection cnnStr = new SqlConnection(strConnectionString))
			{
				SqlCommand command = new SqlCommand(query, cnnStr);
				command.CommandTimeout = 600;
				try
				{
					if (query.StartsWith("SELECT") | query.StartsWith("select"))
					{
						command.CommandType = CommandType.Text;
					}
					else
					{
						command.CommandType = CommandType.StoredProcedure;
					}
					for (int i = 0; i <= parameters.Length - 1; i++)
					{
						command.Parameters.Add(parameters[i]);
					}
					cnnStr.Open();
					retval = command.ExecuteScalar();
					cnnStr.Close();
				}
				catch (Exception ex)
				{



				}
				finally
				{
					cnnStr.Close();
					cnnStr.Dispose();
				}
			}
			return retval;
		}



		public SqlDataReader ExecuteReader(string query)
		{
			SqlConnection cnnStr = new SqlConnection(strConnectionString);
			SqlCommand command = new SqlCommand(query, cnnStr);
			command.CommandTimeout = 600;



			if (query.StartsWith("SELECT") | query.StartsWith("select"))
			{
				command.CommandType = CommandType.Text;
			}
			else
			{
				command.CommandType = CommandType.StoredProcedure;



			}
			cnnStr.Open();



			//finally
			//{
			// cnnStr.Close();
			// cnnStr.Dispose();
			//}
			SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);



			return reader;



		}



		public SqlDataReader ExecuteReader(string query, params SqlParameter[] parameters)
		{
			SqlConnection cnnStr = new SqlConnection(strConnectionString);
			SqlCommand command = new SqlCommand(query, cnnStr);
			command.CommandTimeout = 600;
			if (query.StartsWith("SELECT") | query.StartsWith("select"))
			{
				command.CommandType = CommandType.Text;
			}
			else
			{
				command.CommandType = CommandType.StoredProcedure;
			}
			for (int i = 0; i <= parameters.Length - 1; i++)
			{
				command.Parameters.Add(parameters[i]);
			}
			cnnStr.Open();




			return command.ExecuteReader(CommandBehavior.CloseConnection);
		}



		public DataSet ExecuteDataSet(string query)
		{
			DataSet ds = new DataSet();



			SqlConnection cnnStr = new SqlConnection(strConnectionString);
			SqlCommand command = new SqlCommand(query, cnnStr);
			command.CommandTimeout = 600;
			try
			{
				if (query.StartsWith("SELECT") | query.StartsWith("select") | query.StartsWith("DECLARE") | query.StartsWith(";with"))
				{
					command.CommandType = CommandType.Text;
				}
				else
				{
					command.CommandType = CommandType.StoredProcedure;
				}
				cnnStr.Open();
				SqlDataAdapter adapter = new SqlDataAdapter();
				adapter.SelectCommand = command;
				command.CommandTimeout = 180;
				adapter.Fill(ds);
				cnnStr.Close();
				cnnStr.Dispose();
			}
			catch (Exception ex)
			{
			}
			finally
			{
				cnnStr.Close();
				cnnStr.Dispose();
			}
			return ds;
		}



		public DataSet ExecuteDataSet(string query, params SqlParameter[] parameters)
		{

			DataSet ds = new DataSet();



			SqlConnection cnnStr = new SqlConnection(strConnectionString);
			SqlCommand command = new SqlCommand(query, cnnStr);
			command.CommandTimeout = 600;
			try
			{
				if (query.StartsWith("SELECT") | query.StartsWith("select"))
				{
					command.CommandType = CommandType.Text;
				}
				else
				{
					command.CommandType = CommandType.StoredProcedure;
				}
				for (int i = 0; i <= parameters.Length - 1; i++)
				{
					command.Parameters.Add(parameters[i]);
				}
				cnnStr.Open();
				SqlDataAdapter adapter = new SqlDataAdapter();
				command.CommandTimeout = 5000;
				adapter.SelectCommand = command;
				adapter.Fill(ds);
				command.Parameters.Clear();
				cnnStr.Close();
			}
			catch (Exception ex)
			{
			}
			finally
			{
				cnnStr.Close();
				cnnStr.Dispose();
			}
			return ds;
		}
		public DataSet ExecuteDataSetP(string query, params SqlParameter[] parameters)
		{
			DataSet ds = new DataSet();
			SqlConnection cnnStr = new SqlConnection(strConnectionString);
			SqlCommand command = new SqlCommand(query, cnnStr);
			command.CommandTimeout = 300;
			try
			{
				if (query.StartsWith("SELECT") | query.StartsWith("select"))
				{
					command.CommandType = CommandType.Text;
				}
				else
				{
					command.CommandType = CommandType.StoredProcedure;
				}
				for (int i = 0; i <= parameters.Length - 1; i++)
				{
					command.Parameters.Add(parameters[i]);
				}
				cnnStr.Open();
				SqlDataAdapter adapter = new SqlDataAdapter();
				command.CommandTimeout = 300;
				adapter.SelectCommand = command;



				adapter.Fill(ds);
				command.Parameters.Clear();
				cnnStr.Close();
			}
			catch (Exception ex)
			{
			}
			finally
			{
				cnnStr.Close();
				cnnStr.Dispose();
			}
			return ds;
		}
		public DataTable ExecuteDataTable(string sQuery, string sDaFill = "N", params SqlParameter[] sp)
		{
			DataTable dt = new DataTable();
			SqlConnection cnnStr = new SqlConnection(strConnectionString);
			try
			{
				SqlDataAdapter da = new SqlDataAdapter(sQuery, cnnStr);
				if (sp != null)
				{
					da.SelectCommand.Parameters.AddRange(sp);
					da.SelectCommand.CommandTimeout = 600;
					if (sDaFill == "Y")
					{
						da.SelectCommand.CommandType = CommandType.StoredProcedure;



					}



				}




				da.Fill(dt);
				cnnStr.Close();
				cnnStr.Dispose();
			}
			catch (Exception ex)
			{
			}
			finally
			{
				cnnStr.Close();
				cnnStr.Dispose();
			}
			return dt;
		}



		public DataTable ExecuteDataTable(string sQuery, string sDaFill = "N")
		{
			DataTable dt = new DataTable();
			SqlConnection cnnStr = new SqlConnection(strConnectionString);
			try
			{
				SqlDataAdapter da = new SqlDataAdapter(sQuery, cnnStr);



				if (sDaFill == "Y")
				{
					da.SelectCommand.CommandType = CommandType.StoredProcedure;
				}





				da.Fill(dt);
				cnnStr.Close();
				cnnStr.Dispose();
			}
			catch (Exception ex)
			{
			}
			finally
			{
				cnnStr.Close();
				cnnStr.Dispose();
			}
			return dt;
		}
		public string ManageEncryption(string Conn)
		{
			string returnValue = "";
			string IsPwdEncrypted = System.Configuration.ConfigurationManager.AppSettings["DBPwdEncrypted"];
			if (IsPwdEncrypted.ToUpper() == "TRUE")
			{



			}
			else
			{
				returnValue = Conn;
			}
			return returnValue;
		}



		public void PerformBulkCopy(string destinationTable, DataTable sourceTable, string[,] colMapping)
		{
			SqlConnection cnnStr = new SqlConnection(strConnectionString);
			try
			{
				using (SqlBulkCopy bulkCopy = new SqlBulkCopy(cnnStr))
				{
					bulkCopy.DestinationTableName = destinationTable;



					cnnStr.Open();



					for (int i = 0; i <= colMapping.Length / 2 - 1; i++)
					{
						int j = 0;
						bulkCopy.ColumnMappings.Add(colMapping[i, j], colMapping[i, j + 1]);



					}
					bulkCopy.WriteToServer(sourceTable);
					cnnStr.Close();



				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				cnnStr.Close();
				cnnStr.Dispose();
			}
		}



		public void UpdateData(DataTable sourceTable)
		{
			SqlConnection cnnStr = new SqlConnection(strConnectionString);
			DataTable dt = new DataTable();
			SqlDataAdapter da = new SqlDataAdapter();
			try
			{



				dt = sourceTable;
				da.Update(dt);
				//using (SqlBulkCopy bulkCopy = new SqlBulkCopy(cnnStr))
				//{
				// bulkCopy.DestinationTableName = destinationTable;



				// cnnStr.Open();



				// for (int i = 0; i <= colMapping.Length / 2 - 1; i++)
				// {
				// int j = 0;
				// bulkCopy.ColumnMappings.Add(colMapping[i, j], colMapping[i, j + 1]);



				// }
				// bulkCopy.WriteToServer(sourceTable);
				cnnStr.Close();



				//}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
				cnnStr.Close();
				cnnStr.Dispose();
			}
		}



	}
}

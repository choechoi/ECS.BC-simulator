using System;
using System.Data;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using LGCNS.ezControl.Diagnostics;
using ECS.Common;
using System.Data.SqlClient;

namespace Database.IF
{
    public class DBInterface_MSSQL
    {
        private string connectionString = @"Persist Security Info=False;Integrated Security=false;database=WCS.MonitoringSystem;server=10.212.124.100;User ID=sa;Password=sysadmin1!";

        private static object _updateLockObject = new object();

        public DBInterface_MSSQL()
        {
            connectionString = ConfigurationManager.ConnectionStrings["DB_CONNECTION"].ConnectionString;
        }

        public DBInterface_MSSQL(string CenterCd)
        {
            switch (CenterCd)
            {
                case CConstant.CenterCode.Dongtan:
                    connectionString = @"Persist Security Info=False;Integrated Security=false;database=WCS.MonitoringSystem;server=10.211.174.71;User ID=sa;Password=sysadmin1!";
                    break;                
                default:
                    break;
            }
        }

        public int ExecuteNonQuerywithParam(string sp_procedure, List<SqlParameter> listParam)
        {
            int result = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = sp_procedure;

                        foreach (SqlParameter item in listParam)
                        {
                            command.Parameters.Add(item);
                        }

                        command.Parameters.Add("@ErrorMessage", SqlDbType.VarChar, 200);
                        command.Parameters["@ErrorMessage"].Direction = ParameterDirection.Output;

                        connection.Open();
                        result = command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, ex.Message);
                return result;
            }
        }

    }

}


using System;
using System.Data;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;
using LGCNS.ezControl.Diagnostics;
using ECS.Common;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Database.IF
{
    public class DBInterface
    {
        private string Username = "SMARTWCS";
        private string Password = "smartwcs1234";

        private string connectionValue = string.Empty;
        public string connectionString = string.Empty;

        public DBInterface()
        {
            connectionValue = "Data Source = (DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=oracle11g.ctfe9ihnllwc.ap-northeast-2.rds.amazonaws.com)(PORT = 1521))(CONNECT_DATA=(SERVICE_NAME=DEDICATED)(SERVICE_NAME = ORCL))); Max Pool Size=999;";
            connectionString = connectionValue + "User ID = " + Username + "; Password = " + Password + ";";
        }
        

        public DBInterface(CEnum2.EnumECSType type, string CenterCd)
        {
            switch (CenterCd)
            {
                case CConstant.CenterCode.Dongtan:                  
                    connectionValue = "Data Source = (DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=oracle11g.ctfe9ihnllwc.ap-northeast-2.rds.amazonaws.com)(PORT = 1521))(CONNECT_DATA=(SERVICE_NAME=DEDICATED)(SERVICE_NAME = ORCL))); Max Pool Size=999;"; 
                    break;
                default:
                    break;
            }

            switch (type)
            {
                case CEnum2.EnumECSType.Sorter:
                    connectionString = connectionValue + "User ID = " + Username + "; Password = " + Password + ";";
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Oracle 프로시져 호출
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="listParams"></param>
        /// <returns></returns>
        public void WcsProcedureVoidCall(string procedureName, List<OracleParameter> listParams)
        {
            string retVal = string.Empty;
            try
            {
                using (OracleConnection oracleConnection = new OracleConnection(connectionString))
                {
                    DateTime fromdt = DateTime.Now;
                    oracleConnection.Open();
                    OracleCommand oracleCommand = new OracleCommand(procedureName, oracleConnection);
                    oracleCommand.CommandType = CommandType.StoredProcedure;
                    oracleCommand.Parameters.Clear();
                    oracleCommand.CommandTimeout = 60;
                    string parameterLog = string.Empty;

                    foreach (OracleParameter para in listParams)
                    {
                        oracleCommand.Parameters.Add(para);
                        parameterLog += " /" + para.Value;
                    }
                    oracleCommand.ExecuteNonQuery();
                    oracleConnection.Close();

                    SystemLogger.Log(Level.Info, string.Format("[{0}] Parameter Values {1}", procedureName, parameterLog), "DB");

                    DateTime todt = DateTime.Now;
                    TimeSpan ddd = todt - fromdt;
                    if (ddd.Seconds > 1)
                    {
                        SystemLogger.Log(Level.Verbose, string.Format("{0} - Dely Time : {1}", procedureName, ddd.TotalMilliseconds), "DB_Warning");
                    }
                }
            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, ex.Message, "DB_Exception");
            }
        }

        /// <summary>
        /// Oracle 프로시져 호출
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="listParams"></param>
        /// <returns></returns>
        public string WcsProcedureCall(string procedureName, List<OracleParameter> listParams)
        {
            string retVal = string.Empty;
            try
            {
                using (OracleConnection oracleConnection = new OracleConnection(connectionString))
                {
                    DateTime fromdt = DateTime.Now;
                    oracleConnection.Open();
                    OracleCommand oracleCommand = new OracleCommand(procedureName, oracleConnection);
                    oracleCommand.CommandType = CommandType.StoredProcedure;
                    oracleCommand.Parameters.Clear();
                    oracleCommand.CommandTimeout = 60;
                    string parameterLog = string.Empty;

                    foreach (OracleParameter para in listParams)
                    {
                        oracleCommand.Parameters.Add(para);
                        parameterLog += " /" + para.Value;
                    }
                    oracleCommand.ExecuteNonQuery();
                    oracleConnection.Close();

                    SystemLogger.Log(Level.Info, string.Format("[{0}] Parameter Values {1}", procedureName, parameterLog), "DB");
                    SystemLogger.Log(Level.Info, string.Format("[{0}] Result : {1}", procedureName,
                        oracleCommand.Parameters[CConstant.ProcPnm.SP_RESULT_MSG].Value.ToString()), "DB");

                    DateTime todt = DateTime.Now;
                    TimeSpan ddd = todt - fromdt;
                    if (ddd.Seconds > 1)
                    {
                        SystemLogger.Log(Level.Verbose, string.Format("{0} - Dely Time : {1}", procedureName, ddd.TotalMilliseconds), "DB_Warning");
                    }

                    return oracleCommand.Parameters[CConstant.ProcPnm.SP_RESULT_MSG].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, ex.Message, "DB_Exception");
                return ex.Message;
            }
        }

        public string[] WcsProcedureCallGetValue(string procedureName, List<OracleParameter> listParams, string[] args)
        {
            string[] retVal = new string[args.Length];
            try
            {
                using (OracleConnection oracleConnection = new OracleConnection(connectionString))
                {
                    DateTime fromdt = DateTime.Now;
                    oracleConnection.Open();
                    OracleCommand oracleCommand = new OracleCommand(procedureName, oracleConnection);
                    oracleCommand.CommandType = CommandType.StoredProcedure;
                    oracleCommand.Parameters.Clear();
                    oracleCommand.CommandTimeout = 60;

                    string parameterLog = string.Empty;

                    foreach (OracleParameter para in listParams)
                    {
                        oracleCommand.Parameters.Add(para);
                        parameterLog += " /" + para.Value;
                    }
                    SystemLogger.Log(Level.Info, string.Format("[{0}] Parameter Values {1}", procedureName, parameterLog), "DB");

                    oracleCommand.ExecuteNonQuery();
                    oracleConnection.Close();

                    for (int i = 0; i < retVal.Length; i++)
                    {
                        retVal[i] = oracleCommand.Parameters[args[i]].Value.ToString();
                    }
                    SystemLogger.Log(Level.Info, string.Format("[{0}] Result : {1}", procedureName, string.Join(",", retVal)), "DB");

                    DateTime todt = DateTime.Now;
                    TimeSpan ddd = todt - fromdt;
                    if (ddd.Seconds > 1)
                    {
                        SystemLogger.Log(Level.Verbose, string.Format("{0} - Dely Time : {1}", procedureName, ddd.TotalMilliseconds), "DB_Warning");
                    }
                    return retVal;
                }
            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, ex.Message, "DB_Exception");
                return null;
            }
        }

        public DataSet WcsProcedureCallGetDataSet(string procedureName, List<OracleParameter> listParams)
        {
            DataSet dsResult = null;
            try
            {
                using (OracleConnection oracleConnection = new OracleConnection(connectionString))
                {
                    DateTime fromdt = DateTime.Now;
                    oracleConnection.Open();

                    OracleDataAdapter oracleDataAdapter = new OracleDataAdapter();
                    OracleCommand oracleCommand = new OracleCommand(procedureName, oracleConnection);
                    oracleCommand.CommandType = CommandType.StoredProcedure;
                    oracleCommand.Parameters.Clear();
                    oracleCommand.CommandTimeout = 60;

                    string parameterLog = string.Empty;

                    foreach (OracleParameter para in listParams)
                    {
                        oracleCommand.Parameters.Add(para);
                        parameterLog += " /" + para.Value;
                    }
                    SystemLogger.Log(Level.Info, string.Format("[{0}] Parameter Values {1}", procedureName, parameterLog), "DB");

                    oracleDataAdapter.SelectCommand = oracleCommand;
                    dsResult = new DataSet();
                    oracleDataAdapter.Fill(dsResult);

                    oracleConnection.Close();

                    DateTime todt = DateTime.Now;
                    TimeSpan ddd = todt - fromdt;
                    if (ddd.Seconds > 1)
                    {
                        SystemLogger.Log(Level.Verbose, string.Format("{0} - Dely Time : {1}", procedureName, ddd.TotalMilliseconds), "DB_Warning");
                    }
                }
                return dsResult;

            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, ex.Message, "DB_Exception");
                return null;
            }
        }

        public DataSet WcsProcedureCallGetDataSet(string procedureName, List<OracleParameter> listParams, string[] args)
        {
            DataSet dsResult = null;
            try
            {
                using (OracleConnection oracleConnection = new OracleConnection(connectionString))
                {
                    DateTime fromdt = DateTime.Now;
                    oracleConnection.Open();

                    OracleDataAdapter oracleDataAdapter = new OracleDataAdapter();
                    OracleCommand oracleCommand = new OracleCommand(procedureName, oracleConnection);
                    oracleCommand.CommandType = CommandType.StoredProcedure;
                    oracleCommand.Parameters.Clear();
                    oracleCommand.CommandTimeout = 60;

                    string parameterLog = string.Empty;

                    foreach (OracleParameter para in listParams)
                    {
                        oracleCommand.Parameters.Add(para);
                        parameterLog += " /" + para.Value;
                    }
                    SystemLogger.Log(Level.Info, string.Format("[{0}] Parameter Values {1}", procedureName, parameterLog), "DB");

                    oracleDataAdapter.SelectCommand = oracleCommand;
                    dsResult = new DataSet();
                    oracleDataAdapter.Fill(dsResult);

                    DataTable dt = new DataTable("table");
                    for (int i = 0; i < args.Length; i++)
                    {
                        dt.Columns.Add(args[i]);
                    }

                    // 라벨러 용
                    dt.Rows.Add(oracleCommand.Parameters[args[0]].Value.ToString()
                       , oracleCommand.Parameters[args[1]].Value.ToString()
                       , oracleCommand.Parameters[args[2]].Value.ToString());

                    dsResult.Tables.Add(dt);

                    oracleConnection.Close();

                    DateTime todt = DateTime.Now;
                    TimeSpan ddd = todt - fromdt;
                    if (ddd.Seconds > 1)
                    {
                        SystemLogger.Log(Level.Verbose, string.Format("{0} - Dely Time : {1}", procedureName, ddd.TotalMilliseconds), "DB_Warning");
                    }

                }
                return dsResult;

            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, ex.Message, "DB_Exception");
                return null;
            }
        }

        // ExecuteQuery_Select
        public DataSet ExecuteQuery(string dbQuery, string tableName)
        {
            try
            {
                DataSet dsResult = null;
                using (OracleConnection oracleConnection = new OracleConnection(connectionString))
                {
                    DateTime fromdt = DateTime.Now;
                    // DB연결
                    oracleConnection.Open();

                    // 트랜잭션 속설 설정
                    OracleCommand oracleCommand = new OracleCommand(dbQuery, oracleConnection);
                    OracleDataAdapter oracleDataadapter = new OracleDataAdapter(oracleCommand);

                    oracleCommand.Parameters.Clear();
                    oracleCommand.CommandTimeout = 120;

                    dsResult = new DataSet();
                    if (dsResult != null)
                    {
                        oracleDataadapter.Fill(dsResult, tableName);
                    }

                    oracleConnection.Close();

                    DateTime todt = DateTime.Now;
                    TimeSpan ddd = todt - fromdt;
                    if (ddd.Seconds > 10)
                    {
                        SystemLogger.Log(Level.Verbose, string.Format("{0} - Dely Time : {1}", dbQuery, ddd.TotalMilliseconds), "DB_Warning");
                    }
                }
                return dsResult;
            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, ex.Message);
                return null;
            }
        }

        // Execute Procedure return result as int
        public int ExecuteProcedure(string procedureName, params OracleParameter[] parameters)
        {
            int nEffected = 0;

            using (OracleConnection oracleConnection = new OracleConnection(connectionString))
            {
                // DB연결
                oracleConnection.Open();

                // 트랜잭션 시작
                OracleTransaction oracleTransaction = oracleConnection.BeginTransaction();

                // 트랜잭션 속설 설정
                OracleCommand oracleCommand = new OracleCommand(procedureName, oracleConnection);

                oracleCommand.Parameters.Clear();
                oracleCommand.CommandType = CommandType.StoredProcedure;

                foreach (OracleParameter p in parameters)
                {
                    oracleCommand.Parameters.Add(p);
                }

                try
                {
                    nEffected = oracleCommand.ExecuteNonQuery();
                    oracleTransaction.Commit();
                }
                catch
                {
                    oracleTransaction.Rollback();
                }
                finally
                {
                    oracleConnection.Close();
                }

                return nEffected;
            }
        }

        // Insert/Update/Delete
        public int ExecuteNonQuery(string dbQuery, params OracleParameter[] parameters)
        {
            int nEffected = 0;

            // Connection 생성
            using (OracleConnection oracleConnection = new OracleConnection(connectionString))
            {
                // Connection 연결
                oracleConnection.Open();

                // Command 생성
                OracleCommand oracleCommand = new OracleCommand(dbQuery, oracleConnection);

                // 트랜잭션 시작
                OracleTransaction oracleTransaction = oracleConnection.BeginTransaction();
                oracleCommand.Transaction = oracleTransaction;

                oracleCommand.Parameters.Clear();
                foreach (OracleParameter p in parameters)
                {
                    oracleCommand.Parameters.Add(p);
                }

                // SQL 수행 -> Commit 또는 Rollback
                try
                {
                    nEffected = oracleCommand.ExecuteNonQuery();
                    oracleTransaction.Commit();
                }
                catch (Exception ex)
                {
                    oracleTransaction.Rollback();
                    Console.Write(ex.ToString());
                }
                finally
                {
                    oracleConnection.Close();
                }

                return nEffected;
            }
        }

        public DataSet ExecuteQuery(string dbQuery, string tableName, params OracleParameter[] parameters)
        {
            // Connection 생성
            OracleConnection oracleConnection = new OracleConnection(connectionString);

            DataSet dsResult = null;

            try
            {
                // Connection 연결
                oracleConnection.Open();

                // Command 생성
                OracleCommand oracleCommand = new OracleCommand(dbQuery, oracleConnection);

                // DataAdapter 생성 및 초기화
                OracleDataAdapter oracleDataAdapter = new OracleDataAdapter(oracleCommand);

                oracleCommand.Parameters.Clear();
                foreach (OracleParameter oracleParameter in parameters)
                {
                    oracleCommand.Parameters.Add(oracleParameter);
                }

                // DataTable 생성
                dsResult = new DataSet();

                // Adapter를 이용하여 DataTable 채우기
                oracleDataAdapter.Fill(dsResult, tableName);

                // DataSet 전달
                return dsResult;
            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, ex.Message);
                return null;
            }
            finally
            {
                // Connection 닫기
                oracleConnection.Close();
            }
        }

        public string GetDbDatetime()
        {
            // Connection 생성
            OracleConnection oracleConnection = new OracleConnection(connectionString);

            DataSet dsResult = null;

            try
            {
                string dbQuery = "select to_char(sysdate, 'YYYYMMDDHH24MISS') SYS_DATE24 from dual";

                // Connection 연결
                oracleConnection.Open();

                // Command 생성
                OracleCommand oracleCommand = new OracleCommand(dbQuery, oracleConnection);

                // DataAdapter 생성 및 초기화
                OracleDataAdapter oracleDataAdapter = new OracleDataAdapter(oracleCommand);

                // DataTable 생성
                dsResult = new DataSet();

                // Adapter를 이용하여 DataTable 채우기
                oracleDataAdapter.Fill(dsResult, "DbDateTime");

                return dsResult.Tables[0].Rows[0][0].ToString();
            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, ex.Message, "DB_Exception");
                return null;
            }
            finally
            {
                // Connection 닫기
                oracleConnection.Close();
            }
        }

        // 시뮬레이션 테이블을 이용하지 않고 tb_srt_box_rslt 테이블을 이용한다.      CP, BC 같이 사용한다면, 같은 바코드가 동시에 들어온다면 틀릴 가능성이 존재, 프로시져로 작성해서  회사, 센터 코드 를 이용할 필요 있음  (리팩토링 필요)
        public DataSet GetRecentDestination(string bcd_info)
        {
            DataSet dsResult;
            string dbQuery =
                "SELECT pid, plan_cd, box_bcd, rgn_bcd, bcd_info, plan_chute_id1, plan_chute_id2, plan_chute_id3, scan_dt, rslt_chute_id, srt_wrk_stat_cd, srt_wrk_cmpt_dt, srt_err_cd, srt_rsn_cd" +
                "  FROM   (SELECT * " +
                "            FROM tb_srt_box_rslt " +
                "           WHERE bcd_info = : P_BCD_INFO " +
                "           ORDER BY upd_dt desc ) a" +
                " WHERE rownum = 1";
            try
            {
                OracleParameter[] oracleParameters = new OracleParameter[1];
                oracleParameters[0] = new OracleParameter("P_BCD_INFO", OracleDbType.Varchar2, bcd_info, ParameterDirection.Input);

                dsResult = ExecuteQuery(dbQuery, "tb_srt_box_rslt", oracleParameters);
            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, ex.Message, "GetRecentDestination");
                MessageBox.Show("조회하는데 Error가 생겼습니다.");
                // getLog를 실행시키기 위함
                throw ex;
            }
            return dsResult;
        }

        public DataSet GetSimulCases()
        {
            DataSet dsResult;
            string dbQuery =
                "SELECT bcd_info, sid "+
                "  FROM tb_test_simul_data" +
                " ORDER BY sid "
                ;
            try
            {
                dsResult = ExecuteQuery(dbQuery, "tb_test_simul_data");
            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, ex.Message, "GetSimulCases");
                MessageBox.Show("조회하는데 Error가 생겼습니다.");
                // getLog를 실행시키기 위함
                throw ex;
            }
        
            return dsResult;
        }

        public DataSet GetSimulCaseStatus(string curTimeStr)
        {
                
            DataSet dsResult;
            string dbQuery =
                "SELECT  pid, plan_cd, box_bcd, rgn_bcd, bcd_info, plan_chute_id1, plan_chute_id2, plan_chute_id3, scan_dt, rslt_chute_id, srt_wrk_stat_cd, srt_wrk_cmpt_dt, srt_err_cd, srt_rsn_cd" +
                "  FROM tb_srt_box_rslt" +
                " WHERE TO_CHAR(SCAN_DT,'YYYYMMDDHH24MISS') >= '" + curTimeStr +"'"                // 바인드 변수로 대체 하는게 좋을 듯  리팩토링 필요
                ;
            try
            {

                dsResult = ExecuteQuery(dbQuery, "tb_srt_box_rslt");
            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, ex.Message, "GetSimulCaseStatus");
                MessageBox.Show("조회하는데 Error가 생겼습니다.");
                // getLog를 실행시키기 위함
                throw ex;
            }
            return dsResult;
        }
        public void UpdateSimulCases(int? sid , string bcd_info)
        {
            string dbQuery = string.Empty;
            if (sid == 0)
            {
                dbQuery =
                          "INSERT INTO tb_test_simul_data" +
                          "            (bcd_info,  sid)" +
                          "     VALUES (:P_BCD_INFO, SQ_SIMUL_CASE.NEXTVAL)";
                OracleParameter[] oracleParameters = new OracleParameter[1];
                oracleParameters[0] = new OracleParameter("P_BCD_INFO", OracleDbType.Varchar2, bcd_info, ParameterDirection.Input);
                ExecuteNonQuery(dbQuery, oracleParameters);
            }
            
            else
            {
                dbQuery =
                                "UPDATE tb_test_simul_data" +
                                "   SET bcd_info = :P_BCD_INFO" +
                                " WHERE sid      = :P_SID";
                OracleParameter[] oracleParameters = new OracleParameter[2];
                oracleParameters[0] = new OracleParameter("P_BCD_INFO", OracleDbType.Varchar2, bcd_info, ParameterDirection.Input);
                oracleParameters[1] = new OracleParameter("P_SID", OracleDbType.Int32, sid, ParameterDirection.Input);
                ExecuteNonQuery(dbQuery, oracleParameters);
            }
          
        }
        
        // 사용안함  삭제 가능
        public bool resetSimulCase()
        {
            bool isRtnValue = true;
            // 트랜잭션
            OracleTransaction transaction = null;
            // Connection 생성
            OracleConnection oracleConnection = new OracleConnection(connectionString);
            try
            {
                string dbQuery =
                    "UPDATE tb_test_simul_status" +
                    "   SET srt_wrk_stat_cd = 20" +
                    "      ,plan_chute_id1 = null" +
                    "      ,plan_chute_id2 = null" +
                    "      ,plan_chute_id3 = null" +
                    "      ,srt_err_cd = null" +
                    "      ,rgn_bcd = null"    +
                    "      ,upd_dt = sysdate";
                    ;
                // Connection 연결
                oracleConnection.Open();

                // Command 생성
                OracleCommand oracleCommand = new OracleCommand(dbQuery, oracleConnection);

                // 트랜잭션 시작 오라클이므로 ReadCommitted
                transaction = oracleConnection.BeginTransaction(IsolationLevel.ReadCommitted);
                oracleCommand.Transaction = transaction;

                oracleCommand.ExecuteNonQuery();
                transaction.Commit();

                if (isRtnValue)
                {
                    MessageBox.Show("성공적으로 리셋을 하였습니다.");
                }
                return isRtnValue;
            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, ex.Message, "ResetSimulCase");
                MessageBox.Show("리셋하는데 Error가 생겼습니다.");
                // 트랜젝션이 시작도 안된경우
                if (transaction != null)
                {
                    transaction.Rollback();
                }
                isRtnValue = false;
                // getLog를 실행
                throw ex;
            }
            finally
            {
                // Connection 닫기
                oracleConnection.Close();
            }
        }
    }
}


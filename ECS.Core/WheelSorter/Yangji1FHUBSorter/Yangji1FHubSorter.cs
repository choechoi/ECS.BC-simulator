/// <summary>
/// ECS.Core.CrossBeltSorterServer의 요약
/// </summary>
/// <filename>CrossBeltSorterServer.cs</filename>
/// <version>1.0.0.0</version>
/// <authors></authors>
/// <modifications>
/// 	1.	 ver1.0.0.0		2018.04.18 00:00:00		Devloper Name	- PARK DAE KYUNG : Created.	
/// </modifications>
/// <copyright>Copyright (c) 2007~. EzControl, LG CNS All rights reserved.</copyright>

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Data;
using LGCNS.ezControl.Core;
using LGCNS.ezControl.Diagnostics;

using ECS.Common;

using Oracle.ManagedDataAccess.Client;
using Database.IF;

using ECS.Driver;


namespace ECS.Core
{
    /// <summary>
    /// 사용 : ezControl Modeler 의 CrossBeltSorterServer Logic
    /// IOMudule에서 완료된 항목을 DBMS에 처리 
    /// </summary>
    public partial class Yangji1FHubSorter : WheelSorterServer
    {       
        #region system override
        protected override void OnInstancingCompleted()
        {
            base.OnInstancingCompleted();

            inductionBcr = false;

            SystemLogger.Log(Level.Info, this.Name + " OK!!");
        }

        protected override void OnUnloaded()
        {
            base.OnUnloaded();            
        }

        #endregion

        #region [WCS IF override]                
        protected override string WcsOnItemInducted(CSorterItemData itemData)
        {                 
            try
            {
                List<OracleParameter> listPara = new List<OracleParameter>
                {
                    new OracleParameter("P_CENTER_CD", OracleDbType.Varchar2, CENTER_CD, ParameterDirection.Input),
                    new OracleParameter("P_EQP_ID", OracleDbType.Varchar2, MODULEID, ParameterDirection.Input),
                    new OracleParameter("P_DATE", OracleDbType.Varchar2, itemData.INDUCTEDTIME_UK, ParameterDirection.Input),
                    new OracleParameter("P_PID", OracleDbType.Varchar2, itemData.PID, ParameterDirection.Input),
                    new OracleParameter("P_INDT_ID", OracleDbType.Varchar2, itemData.InductionNo, ParameterDirection.Input),
                    new OracleParameter("P_CART_NO", OracleDbType.Varchar2, itemData.CartNo, ParameterDirection.Input),
                    new OracleParameter("P_INPUT_MODE", OracleDbType.Varchar2, itemData.Mode, ParameterDirection.Input),
                    new OracleParameter("P_USER_ID", OracleDbType.Varchar2, string.Empty, ParameterDirection.Input),
                    new OracleParameter("O_RTN_CD", OracleDbType.Varchar2, 32767, string.Empty, ParameterDirection.Output),
                    new OracleParameter("O_RTN_MSG", OracleDbType.Varchar2, 32767, string.Empty, ParameterDirection.Output),
                };

                string[] outpara = new string[2] { "O_RTN_CD", "O_RTN_MSG" };                

                string[] retVal = dbinterface.WcsProcedureCallGetValue(SP_INDUCTED, listPara, outpara);               

                return retVal[0];              
            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, string.Format("[{0}]{1} : {2}", this.Name, ex.StackTrace, ex.Message));
                return null;
            }
        }

        protected override string WcsOnItemDischarged(CSorterItemData itemData)
        {                    

            try
            {
                List<OracleParameter> listPara = new List<OracleParameter>
                {
                    new OracleParameter("P_CENTER_CD", OracleDbType.Varchar2, CENTER_CD, ParameterDirection.Input),
                    new OracleParameter("P_EQP_ID", OracleDbType.Varchar2, MODULEID, ParameterDirection.Input),
                    new OracleParameter("P_DATE", OracleDbType.Varchar2, itemData.INDUCTEDTIME_UK, ParameterDirection.Input),
                    new OracleParameter("P_PID", OracleDbType.Varchar2, itemData.PID, ParameterDirection.Input),
                    new OracleParameter("P_INDT_ID", OracleDbType.Varchar2, itemData.InductionNo, ParameterDirection.Input),
                    new OracleParameter("P_CART_NO", OracleDbType.Varchar2, itemData.CartNo, ParameterDirection.Input),
                    new OracleParameter("P_CHUTE_ID", OracleDbType.Varchar2, itemData.Discharge_ChuteNumber, ParameterDirection.Input),
                    new OracleParameter("P_INPUT_MODE", OracleDbType.Varchar2, itemData.Mode, ParameterDirection.Input),
                    new OracleParameter("P_CIRCU_CNT", OracleDbType.Varchar2, itemData.RecirculationCount, ParameterDirection.Input),
                    new OracleParameter("P_USER_ID", OracleDbType.Varchar2, string.Empty, ParameterDirection.Input),
                    new OracleParameter("O_RTN_CD", OracleDbType.Varchar2, 32767, string.Empty, ParameterDirection.Output),
                    new OracleParameter("O_RTN_MSG", OracleDbType.Varchar2, 32767, string.Empty, ParameterDirection.Output),
                };

                string[] outpara = new string[2] { "O_RTN_CD", "O_RTN_MSG" };

                string[] retVal = dbinterface.WcsProcedureCallGetValue(SP_DISCHARED, listPara, outpara);
                
                return retVal[0];
            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, string.Format("[{0}]{1} : {2}", this.Name, ex.StackTrace, ex.Message));
                return null;
            }           
        }

        protected override string WcsOnItemSortedConfirm(CSorterItemData itemData)
        {
            try
            {
                List<OracleParameter> listPara = new List<OracleParameter>
                {
                    new OracleParameter("P_CENTER_CD", OracleDbType.Varchar2, CENTER_CD, ParameterDirection.Input),
                    new OracleParameter("P_EQP_ID", OracleDbType.Varchar2, MODULEID, ParameterDirection.Input),
                    new OracleParameter("P_DATE", OracleDbType.Varchar2, itemData.INDUCTEDTIME_UK, ParameterDirection.Input),
                    new OracleParameter("P_PID", OracleDbType.Varchar2, itemData.PID, ParameterDirection.Input),
                    new OracleParameter("P_INDT_ID", OracleDbType.Varchar2, itemData.InductionNo, ParameterDirection.Input),
                    new OracleParameter("P_CART_NO", OracleDbType.Varchar2, itemData.CartNo, ParameterDirection.Input),
                    new OracleParameter("P_CHUTE_ID", OracleDbType.Varchar2, itemData.SortedConfirm_ChuteNumber, ParameterDirection.Input),
                    new OracleParameter("P_INPUT_MODE", OracleDbType.Varchar2, itemData.Mode, ParameterDirection.Input),
                    new OracleParameter("P_CIRCU_CNT", OracleDbType.Varchar2, itemData.RecirculationCount, ParameterDirection.Input),
                    new OracleParameter("P_SPS_LOC_CD", OracleDbType.Varchar2, itemData.SpsPos, ParameterDirection.Input),
                    new OracleParameter("P_OUT_REAN_CD", OracleDbType.Varchar2, itemData.ReasonCode, ParameterDirection.Input),
                    new OracleParameter("P_USER_ID", OracleDbType.Varchar2, string.Empty, ParameterDirection.Input),
                    
                    new OracleParameter("O_RTN_CD", OracleDbType.Varchar2, 32767, string.Empty, ParameterDirection.Output),
                    new OracleParameter("O_RTN_MSG", OracleDbType.Varchar2, 32767, string.Empty, ParameterDirection.Output),
                };

                string[] outpara = new string[2] { "O_RTN_CD", "O_RTN_MSG" };

                string[] retVal = dbinterface.WcsProcedureCallGetValue(SP_SORTED_CFM, listPara, outpara);                

                return retVal[0];
            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, string.Format("[{0}]{1} : {2}", this.Name, ex.StackTrace, ex.Message));
                return null;
            }
        }

        protected override string[] WcsIPS_OnIPSResult(CSorterItemData itemData)
        {
            try
            {
                List<OracleParameter> listPara = new List<OracleParameter>
                {
                    new OracleParameter("P_CENTER_CD", OracleDbType.Varchar2, CENTER_CD, ParameterDirection.Input),
                    new OracleParameter("P_EQP_ID", OracleDbType.Varchar2, MODULEID, ParameterDirection.Input),
                    new OracleParameter("P_DATE", OracleDbType.Varchar2, itemData.INDUCTEDTIME_UK, ParameterDirection.Input),
                    new OracleParameter("P_PID", OracleDbType.Varchar2, itemData.PID, ParameterDirection.Input),
                    new OracleParameter("P_INDT_ID", OracleDbType.Varchar2, itemData.InductionNo, ParameterDirection.Input),
                    new OracleParameter("P_CART_NO", OracleDbType.Varchar2, itemData.CartNo, ParameterDirection.Input),
                    new OracleParameter("P_BCR_NO", OracleDbType.Varchar2, itemData.IPSData, ParameterDirection.Input),                
                    new OracleParameter("P_USER_ID", OracleDbType.Varchar2, string.Empty, ParameterDirection.Input),
                    new OracleParameter("P_IPS_NO", OracleDbType.Varchar2, itemData.IPSID, ParameterDirection.Input),
                    new OracleParameter("O_DEST_CHUTE_NO", OracleDbType.Varchar2, 32767, string.Empty, ParameterDirection.Output),
                    new OracleParameter("O_RTN_CD", OracleDbType.Varchar2, 32767, string.Empty, ParameterDirection.Output),
                    new OracleParameter("O_RTN_MSG", OracleDbType.Varchar2, 32767, string.Empty, ParameterDirection.Output),
                };                

                string[] outpara = new string[3] {"O_DEST_CHUTE_NO", "O_RTN_CD", "O_RTN_MSG" };                

                return dbinterface.WcsProcedureCallGetValue(SP_IPS_SCAN, listPara, outpara);
            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, string.Format("[{0}]{1} : {2}", this.Name, ex.StackTrace, ex.Message));
                return null;
            }
        }

        private void SetSorterConfig(string reject1, string reject2, string recCnt)
        {
            try
            {
                SetOverflowConfiguration__ErrorChute1 = Convert.ToUInt16(reject1);
                SetOverflowConfiguration__ErrorChute2 = Convert.ToUInt16(reject1);
                SetOverflowConfiguration__Recirculation = Convert.ToUInt16(recCnt);

                SendOverflowConfiguration();
            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, string.Format("[{0}]{1} : {2}", this.Name, ex.StackTrace, ex.Message));
            }
        }        

        #endregion

        #region
        #region UI Control
        private void FireToUIEvent(CEnum2.EnumToUIEvent eventName, params object[] args)
        {
            try
            {
                RaiseEvent((int)eventName, args);
            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, string.Format("[{0}]{1} : {2}", this.Name, ex.StackTrace, ex.Message));
            }
        }
        #endregion

        #region From UI Event
        public int UIEventReceiver(int iEventID, params object[] args)
        {
            try
            {
                switch (iEventID)
                {
                    //case (int)UIConstant.enumRemoteControl.Start:
                    //    SystemLogger.Log(Level.Warning, "Sorter Start");
                        
                    //    break;
                    //case (int)UIConstant.enumRemoteControl.Stop:
                    //    SystemLogger.Log(Level.Warning, "Sorter Stop");                        
                    //    break;
                    case (int)CEnum2.EnumToCoreEvent.CommandData:
                        SetCommand(Convert.ToUInt16(args[0].ToString()));
                        break;
                    case (int)CEnum2.EnumToCoreEvent.SetSorterConfig:
                        SetSorterConfig(args[0].ToString(), args[1].ToString(), args[2].ToString());
                        break;

                    default:
                        break;

                }

                return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        #endregion    

        #region Method
        //void SetWHInform(params object[] args)
        //{           
        //    // FireToUIEvent(CEnum2.EnumToUIEvent.TestValueRcv, BARCODE, DUE_DATE, USER_ID);
        //}6
        #endregion

        public void SetCommand(ushort Command)
        {
            Logger.Log(Level.Info, "[" + this.Name + "] Command Type:" + Command.ToString(), this.Name);

            COMMAND__TYPE = Command;
            COMMAND__UNIT = 0;

            if (CVariableAction.TimeOut(__COMMAND__REASON_CODE, (ushort)0, 100, 1, true))
            {
                Logger.Log(Level.Warning, "Command Timeout", this.Name);
                CommandClear();
                return;
            }

            COMMAND__REPLY = COMMAND__REASON_CODE;
            SystemLogger.Log(Level.Info, string.Format("COMMAND : {0} / REASON_CODE : {1} ", COMMAND__TYPE, COMMAND__REASON_CODE), this.Name);

            //for (int i = 0; i < 10; i++)
            //{
            //    if (COMMAND__REASON_CODE == 0)
            //    {
            //        CommandClear();
            //        return;
            //    }

            //    System.Threading.Thread.Sleep(100);
            //}

            Logger.Log(Level.Warning, "Reply Timeout", this.Name);
            CommandClear();
        }

        void CommandClear()
        {
            COMMAND__REPLY = 0;
            COMMAND__TYPE = 0;
            COMMAND__UNIT = 0;
        }
        #endregion

    }
}

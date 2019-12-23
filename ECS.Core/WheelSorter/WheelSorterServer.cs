/// <summary>
/// ECS.Core.WheelSorterServer의 요약
/// </summary>
/// <filename>WheelSorterServer.cs</filename>
/// <version>1.0.0.0</version>
/// <authors></authors>
/// <modifications>
/// 	1.	 ver1.0.0.0		2018.04.18 00:00:00		Devloper Name	- LIM HYUNG TAEK : Created.	
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
using Oracle.ManagedDataAccess.Client;
using Database.IF;
using ECS.Common;
using ECS.Driver;
using System.Text.RegularExpressions;

namespace ECS.Core
{
    /// <summary>
    /// 사용 : ezControl Modeler 의 WheelSorterServer Logic
    /// IOMudule에서 PLC I/F | DBMS I/F | 데이터 처리
    /// </summary>
    public partial class WheelSorterServer : CElement
    {
        #region variable
        public CSorterItemDataManager itemDataMgr;

        public SocketDriver drvSocket;        

        protected DBInterface dbinterface;

        System.Timers.Timer heartbeatTimer;
        public bool bHeartBeat = true;
        private int heartbeatCounter = 0;

        System.Timers.Timer systemDateSyncTimer;

        string dbDateTime = string.Empty;

        object lockObj = new object();

        int threadPool_available;
        int threadPool_maxLimit;

        public int sorterMainSendPort;
        public int sorterSortedConfirmRecvPort;
        public int sorterDestinationSendPort;

        private const string systemLogNm = "Sorter Log";

        #endregion

        #region system override
        protected override void OnInstancingCompleted()
        {
            base.OnInstancingCompleted();

            dbinterface = new DBInterface();

            sorterMainSendPort = PORT;
            sorterSortedConfirmRecvPort = PORT + 1;
            sorterDestinationSendPort = PORT + 2;

            itemDataMgr = new CSorterItemDataManager();

            drvSocket = new SocketDriver();
            drvSocket.OnRecievePacket += drvSocket_OnRecievePacket;
            drvSocket.OnConnect += Drv_OnConnect;
            drvSocket.SocketConnected(EQP_ID, 3, IP, PORT, true);
            
            drvSocket.EnableLog(true, true);

            foreach (CElement element in this.Elements.Values)
            {
                 if (element is IpsModule)
                {
                    IpsModule bcrmodule = (IpsModule)element;
                    bcrmodule.OnIPSResult += IPS_OnIPSResult;
                    bcrmodule.InitDriver();
                }
                
                System.Threading.Thread.Sleep(1);
            }

            __DEBUG.OnBooleanChanged += __DEBUG_OnBooleanChanged;
            __SET_CONFIG_SEND.OnBooleanChanged += __SET_CONFIG_SEND_OnBooleanChanged; 

            heartbeatTimer = new System.Timers.Timer()
            {
                Interval = 5 * 1000
            };
            heartbeatTimer.Elapsed += new System.Timers.ElapsedEventHandler(HeartbeatTimer_Elapsed);
            //heartbeatTimer.Start();
            
        }   
        private void HeartbeatTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {                        
            if (bHeartBeat)
            {
                HeartBeat();
                if (heartbeatCounter > WheelSorterTelegram.HEARTBEAT_TIMEOUT_COUNT)
                {
                    drvSocket.TryReconnection();
                    heartbeatCounter = 4; //접속 후 Heartbeat를 바로 못 받아서 Count를 내려서 여유를 줌
                }
                heartbeatCounter++;
            }

            System.Threading.ThreadPool.GetAvailableThreads(out threadPool_available, out threadPool_maxLimit);
            SystemLogger.Log(Level.Debug, string.Format("[{0}] ABLE:{1} / MAX:{2}", this.Name, threadPool_available, threadPool_maxLimit));

        }

        private void HeartBeat()
        {
            WheelSorterTelegram.Heartbeat send = new WheelSorterTelegram.Heartbeat();
            drvSocket.SendData(send.MakePacket(EQP_ID), sorterMainSendPort);
        }

        protected override void OnUnloaded()
        {
            base.OnUnloaded();

            drvSocket.Close();            
            SystemLogger.Log(String.Format("Disconnection : {0}", "drvSocket"), "CLIENT_INFO");

        }        

        private void __DEBUG_OnBooleanChanged(CVariable sender, bool value)
        {            
            if (drvSocket != null)
            {
                drvSocket.EnableLog(value, value);
            }
        }

        private void __SET_CONFIG_SEND_OnBooleanChanged(CVariable sender, bool value)
        {
            SendConfiguration();
        }

        private void Drv_OnConnect(string key, bool connected)
        {
            SystemLogger.Log(String.Format("Connection {0} {1}", key, connected), "CLIENT_INFO");
        }

        private void TimeAlignmentRequest(DateTime idate)
        {
            try
            {
                WheelSorterTelegram.TimeAlignmentRequest send = new WheelSorterTelegram.TimeAlignmentRequest()
                {
                    Year = Convert.ToInt16(idate.Year.ToString().Substring(2)),
                    Month = Convert.ToInt16(idate.Month),
                    Day = Convert.ToInt16(idate.Day),
                    Hour = Convert.ToInt16(idate.Hour),
                    Minute = Convert.ToInt16(idate.Minute),
                    Second = Convert.ToInt16(idate.Second)
                };
                drvSocket.SendData(send.MakePacket(EQP_ID), sorterMainSendPort);
                SystemLogger.Log(Level.Info, String.Format("<TimeAlignmentRequest> [Datetime={0}{1}{2}{3}{4}{5}]"
                    , send.Year, send.Month, send.Day, send.Hour, send.Minute, send.Second));
            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, string.Format("[{0}]{1} : {2}", this.Name, ex.StackTrace, ex.Message));
            }
        }

        #endregion

        #region [Track Socket와 통신 메시지]

        #region [drvSocket_OnRecievePacket(WheelSorterTelegram sender, List<short> listdata)]
        void drvSocket_OnRecievePacket(CommonHeader sender, List<short> listdata)
        {
            switch ((WheelSorterTelegram.TELEGRAM)sender.TelegramNo)
            {
                case WheelSorterTelegram.TELEGRAM.HEARTBEAT:
                    heartbeatCounter = 0;
                    break;                
                case WheelSorterTelegram.TELEGRAM.ITEM_SORTEDCONFIRM:
                    OnItemSortedConfirm(listdata);
                    break;     
                case WheelSorterTelegram.TELEGRAM.SET_CONFIG_ACK:
                    OnConfigurationAck(listdata);
                    break;                
                default:
                    break;
            }
        }
        #endregion

        #region [IPS과 통신 메시지 ]      

        #region IPS_ONIPSResult 
        void IPS_OnIPSResult(IpsTelegram.IPSResult Result)
        {
            try
            {
                CSorterItemData itemData;

                lock (lockObj)
                {
                    itemData = new CSorterItemData()  // 최초 Pid 및 데이터를 생성하여  Dictionary에 담는다. 
                    {
                        inductedTime = DateTime.Now.ToString(CConstant.DATEFORMAT + CConstant.TIMEFORMAT),
                        eqpId = __EQP_ID.AsString,
                        pid = CUtil.GetShortNumber(Result.parcelID),
                    };

                    itemData.ipsData = Result.IPSData;

                    itemDataMgr.OldDataProcess(itemData.pid);
                    itemDataMgr.Add(itemData, itemData.pid);
                }

                SystemLogger.Log(Level.Info, String.Format("<IPS_READ> INPUT VALUES [UK={0}] [PID={1}][IPS={2}]",
                    itemData.inductedTime, itemData.pid, itemData.ipsData), systemLogNm);

                System.Threading.ThreadPool.QueueUserWorkItem(delegate
                {
                    if (itemData.pid != 0)
                    {
                        string[] scanResult = new string[5];

                        scanResult = WcsIPS_OnIPSResult(itemData);  // Procedure를 호출하여 목적지를 받는다. 

                        if (scanResult != null && scanResult[0] != null && scanResult[0] != "null")
                        { 
                            itemData.Ldestination[0] = CUtil.GetShortNumber(scanResult[0]);
                            itemData.Ldestination[1] = CUtil.GetShortNumber(scanResult[1]);
                            itemData.Ldestination[2] = CUtil.GetShortNumber(scanResult[2]);
                        }

                        SystemLogger.Log(Level.Debug, String.Format("<IPS_PROC_READ> [UK={0}] [PID={1}][IPS={2}]  PROC_CALL= [CHUTE={3}][RTN={4}] [ERR={5}]",
                            itemData.inductedTime, itemData.pid, itemData.ipsData, itemData.Ldestination[0] + ";" + itemData.Ldestination[1] + ";" + itemData.Ldestination[2], scanResult == null ? "null" : scanResult[3], scanResult == null ? "null" : scanResult[4]),
                            systemLogNm);                        
                    }
                    else
                    {
                        SystemLogger.Log(Level.Info, String.Format("<PID is Zero. Destination Don't Send>"));
                        itemData.destination1 = CUtil.GetShortNumber(CConstant.PROCEDURE_RESULT.REWORK);
                    }

                    WheelSorterTelegram.DestinationSend send = new WheelSorterTelegram.DestinationSend()  //Telegram에 담는다.
                    {
                        ParcelID = itemData.pid,
                        Destination = itemData.Ldestination,
                    };
                    drvSocket.SendData(send.MakePacket(EQP_ID), sorterDestinationSendPort);  //Socket 전송
                    SystemLogger.Log(Level.Debug, String.Format("<Destination Send OK> [PID={0}][CHUTE={1}]",
                        send.ParcelID, send.Destination[0] + ";" + send.Destination[1] + ";" + send.Destination[2]));

                });
            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, string.Format("[{0}]{1} : {2}", this.Name, ex.StackTrace, ex.Message));
            }
        }
        #endregion

        protected virtual string[] WcsIPS_OnIPSResult(CSorterItemData itemData)
        {
            try
            {
                DataSet dsRegEx = GetRegEx(); // 쿠팡의 정규식을 테이블에서 가져온다. 

                Regex regInvNo = new Regex(string.Format(@"{0}", dsRegEx.Tables[0].Rows[0]["INV_NO_REGEX"]));
                Regex regBoxCd = new Regex(string.Format(@"{0}", dsRegEx.Tables[0].Rows[0]["BOX_BCD_REGEX"]));
                Regex regRgnCd = new Regex(string.Format(@"{0}", dsRegEx.Tables[0].Rows[0]["RGN_BCD_REGEX"]));

                char[] charSeparators = { ';' };
                string[] arrBCRData = itemData.ipsData.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries);        // 바코드 개수만큼 string 배열에 담는다.        

                string sInvNo = string.Empty;
                string sBoxCd = string.Empty;
                string sRgnCd = string.Empty;

                foreach (string strBcrdata in arrBCRData) // 바코드 개수만큼 돌면서 정규식 체크하여 바코드를 추출한다. 
                {
                    foreach (Match match in regInvNo.Matches(strBcrdata))
                    {
                        sInvNo += match.Value + ";";
                    }
                    foreach (Match match in regBoxCd.Matches(strBcrdata))
                    {
                        sBoxCd += match.Value + ";";
                    }
                    foreach (Match match in regRgnCd.Matches(strBcrdata))
                    {
                        sRgnCd += match.Value + ";";
                    }
                }

                sInvNo = sInvNo.Length > 0 ? sInvNo.Remove(sInvNo.Length - 1) : sInvNo;  //마지막의 ";"은 제거 한다. 
                sBoxCd = sBoxCd.Length > 0 ? sBoxCd.Remove(sBoxCd.Length - 1) : sBoxCd;
                sRgnCd = sRgnCd.Length > 0 ? sRgnCd.Remove(sRgnCd.Length - 1) : sRgnCd;

                List<OracleParameter> listPara = new List<OracleParameter>
                {
                    new OracleParameter(CConstant.ProcPnm.COMPANY_CD, OracleDbType.Varchar2,COMPANY_CD,ParameterDirection.Input),
                    new OracleParameter(CConstant.ProcPnm.CENTER_CD, OracleDbType.Varchar2,CENTER_CD,ParameterDirection.Input),
                    new OracleParameter(CConstant.ProcPnm.EQUIPMENT_ID, OracleDbType.Varchar2,EQP_ID,ParameterDirection.Input),
                    new OracleParameter(CConstant.ProcPnm.INPUT_TIME, OracleDbType.Varchar2,itemData.inductedTime,ParameterDirection.Input),
                    new OracleParameter(CConstant.ProcPnm.PARCEL_ID, OracleDbType.Varchar2,itemData.pid,ParameterDirection.Input),
                    new OracleParameter(CConstant.ProcPnm.BARCODE, OracleDbType.Varchar2,itemData.ipsData.ToString(),ParameterDirection.Input),
                     new OracleParameter(CConstant.ProcPnm.INV_BCD,OracleDbType.Varchar2,        sInvNo,                     ParameterDirection.Input),
                    new OracleParameter(CConstant.ProcPnm.BOX_BCD,  OracleDbType.Varchar2,        sBoxCd,                     ParameterDirection.Input),
                    new OracleParameter(CConstant.ProcPnm.RGN_BCD,  OracleDbType.Varchar2,        sRgnCd,                     ParameterDirection.Input),
                    new OracleParameter(CConstant.ProcPnm.USER_ID, OracleDbType.Varchar2, 32767, string.Empty, ParameterDirection.Output),
                    new OracleParameter(CConstant.ProcPnm.O_CHUTE_NO1, OracleDbType.Varchar2, 32767, string.Empty, ParameterDirection.Output),
                    new OracleParameter(CConstant.ProcPnm.O_CHUTE_NO2, OracleDbType.Varchar2, 32767, string.Empty, ParameterDirection.Output),
                    new OracleParameter(CConstant.ProcPnm.O_CHUTE_NO3, OracleDbType.Varchar2, 32767, string.Empty, ParameterDirection.Output),
                    new OracleParameter(CConstant.ProcPnm.SP_RESULT_CD, OracleDbType.Varchar2, 32767, string.Empty, ParameterDirection.Output),
                    new OracleParameter(CConstant.ProcPnm.SP_RESULT_MSG, OracleDbType.Varchar2, 32767, string.Empty, ParameterDirection.Output),
                };

                string[] outpara = new string[5] { CConstant.ProcPnm.O_CHUTE_NO1, CConstant.ProcPnm.O_CHUTE_NO2, CConstant.ProcPnm.O_CHUTE_NO3,
                    CConstant.ProcPnm.SP_RESULT_CD, CConstant.ProcPnm.SP_RESULT_MSG };

                return dbinterface.WcsProcedureCallGetValue(SP_IPS_SCAN, listPara, outpara);
            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, string.Format("[{0}]{1} : {2}", this.Name, ex.StackTrace, ex.Message));
                return null;
            }
        }

        private DataSet GetRegEx()
        {
            try
            {
                List<OracleParameter> listPara = new List<OracleParameter>
                {
                    new OracleParameter(CConstant.ProcPnm.COMPANY_CD,    OracleDbType.Varchar2, COMPANY_CD,                 ParameterDirection.Input),
                    new OracleParameter(CConstant.ProcPnm.CENTER_CD,     OracleDbType.Varchar2, CENTER_CD,                  ParameterDirection.Input),
                    new OracleParameter(CConstant.ProcPnm.EQUIPMENT_ID,  OracleDbType.Varchar2, EQP_ID,                     ParameterDirection.Input),
                    new OracleParameter(CConstant.ProcPnm.O_RSLT_LIST,   OracleDbType.RefCursor,                            ParameterDirection.Output),
                    new OracleParameter(CConstant.ProcPnm.O_RSLT,        OracleDbType.RefCursor,                            ParameterDirection.Output),
            };

                return dbinterface.WcsProcedureCallGetDataSet("PK_C1024_SRT.SP_BCD_REGEX_INQ", listPara);

            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, string.Format("[{0}]{1} : {2}", this.Name, ex.StackTrace, ex.Message));
                return null;
            }
        }
        #endregion

        #region [OnItemSortedConfirm(List<short> Message)]
        private void OnItemSortedConfirm(List<short> Message)
        {
            try
            {
                WheelSorterTelegram.ItemSortedConfirm item = new WheelSorterTelegram.ItemSortedConfirm(Message);
                CSorterItemData itemData = itemDataMgr.GetItem(item.ParcelID);
                if (itemData == null) //ItemData가 없으면 Dictionary에 새로 생성해준다. 
                {
                    itemData = new CSorterItemData()
                    {
                        inductedTime = DateTime.Now.ToString(CConstant.DATEFORMAT + CConstant.TIMEFORMAT),
                        eqpId = __EQP_ID.AsString,
                        pid = item.ParcelID,
                        sortedConfirmedChuteNumber = item.ChuteNumber,
                        reasonCode = item.ReasCode,
                        sensorYN = item.SensorYN
                        
                    };
                    itemDataMgr.Add(itemData, itemData.pid); 
                    SystemLogger.Log(Level.Exception, string.Format("[OnItemSortedConfirm 새로 생성] UK={0}  parcelID = {1}", itemData.inductedTime, itemData.pid), systemLogNm);
                   
                }
                else  
                {                    
                    itemData.sortedConfirmedChuteNumber = item.ChuteNumber;
                    itemData.reasonCode = item.ReasCode;
                    itemData.sensorYN = item.SensorYN;
                }

                string result = WcsOnItemSortedConfirm(itemData);

                itemDataMgr.Remove(itemData, itemData.pid); //Sorted Confirm 후 Dictionary에서 제거              

                SystemLogger.Log(Level.Debug, String.Format("<OnItemSortedConfirm> [UK={0}] [PID={1}][CHUTE={2}][REASON={3}][SENSOR={4}] -> [RESULT={5}]",
                    itemData.inductedTime, itemData.pid, itemData.sortedConfirmedChuteNumber, itemData.reasonCode, itemData.sensorYN, result == null ? "null" : result), systemLogNm);
            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, string.Format("[{0}]{1} : {2}", this.Name, ex.StackTrace, ex.Message));
            }
        }

        protected virtual string WcsOnItemSortedConfirm(CSorterItemData itemData)
        {
            try
            {
                List<OracleParameter> listPara = new List<OracleParameter>
                {
                    new OracleParameter(CConstant.ProcPnm.COMPANY_CD, OracleDbType.Varchar2,COMPANY_CD,ParameterDirection.Input),
                    new OracleParameter(CConstant.ProcPnm.CENTER_CD, OracleDbType.Varchar2,CENTER_CD,ParameterDirection.Input),
                    new OracleParameter(CConstant.ProcPnm.EQUIPMENT_ID, OracleDbType.Varchar2,EQP_ID,ParameterDirection.Input),
                    new OracleParameter(CConstant.ProcPnm.INPUT_TIME, OracleDbType.Varchar2,itemData.inductedTime,ParameterDirection.Input),
                    new OracleParameter(CConstant.ProcPnm.PARCEL_ID, OracleDbType.Varchar2,itemData.pid,ParameterDirection.Input),
                    new OracleParameter(CConstant.ProcPnm.CHUTE_NO, OracleDbType.Varchar2,itemData.sortedConfirmedChuteNumber,ParameterDirection.Input),
                    new OracleParameter(CConstant.ProcPnm.REASON_CD, OracleDbType.Varchar2,itemData.reasonCode,ParameterDirection.Input),
                    new OracleParameter(CConstant.ProcPnm.SENSOR_YN, OracleDbType.Varchar2,itemData.sensorYN,ParameterDirection.Input),
                    new OracleParameter(CConstant.ProcPnm.USER_ID, OracleDbType.Varchar2, 32767, string.Empty, ParameterDirection.Output),
                    new OracleParameter(CConstant.ProcPnm.SP_RESULT_CD, OracleDbType.Varchar2, 32767, string.Empty, ParameterDirection.Output),
                    new OracleParameter(CConstant.ProcPnm.SP_RESULT_MSG, OracleDbType.Varchar2, 32767, string.Empty, ParameterDirection.Output),
                };
                return dbinterface.WcsProcedureCall(SP_SORTED_CFM, listPara);
            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, string.Format("[{0}]{1} : {2}", this.Name, ex.StackTrace, ex.Message));
                return null;
            }
        }

        #endregion        

        private void OnCodeRequest(List<short> Message)
        {
             SystemLogger.Log(Level.Info, string.Format("OnCodeRequest(List<short> Message)"));
            //WheelSorterTelegram.CodeRequest requestInfo = new WheelSorterTelegram.CodeRequest(Message);

            //dicInduction[requestInfo.InductionNo].SetCodeReqeust(requestInfo);
        }

        protected virtual void SendConfiguration()
        {
            try
            {
                WheelSorterTelegram.SetConfiguration send = new WheelSorterTelegram.SetConfiguration()
                {
                    ErrorChute1 = Convert.ToInt16(__SET_CONFIGURATION__ERROR_CHUTE1.AsShort),
                    ErrorChute2 = Convert.ToInt16(__SET_CONFIGURATION__ERROR_CHUTE2.AsShort),
                };
                drvSocket.SendData(send.MakePacket(EQP_ID), sorterMainSendPort);

                SystemLogger.Log(Level.Info, String.Format("<SetConfiguration Send OK> [ErrorChute1={0}][ErrorChute2={1}]",
                    send.ErrorChute1, send.ErrorChute2), systemLogNm);

                __SET_CONFIGURATION__ERROR_CHUTE1.Save();
                __SET_CONFIGURATION__ERROR_CHUTE2.Save();
            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, string.Format("[{0}]{1} : {2}", this.Name, ex.StackTrace, ex.Message));
            }
        }

        protected virtual void OnConfigurationAck(List<short> Message)
        {
            try
            {
                WheelSorterTelegram.SetConfigurationAck send = new WheelSorterTelegram.SetConfigurationAck(Message);
 
                SystemLogger.Log(Level.Info, String.Format("<SetConfiguration Ack OK> [ErrorChute1={0}][ErrorChute2={1}][Reason={2}]",
                    send.ErrorChute1, send.ErrorChute2, send.Reason), systemLogNm);
            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, string.Format("[{0}]{1} : {2}", this.Name, ex.StackTrace, ex.Message));
            }
        }        

        #endregion

        
        #region public method       

        public object GetValue(string name)
        {
            object val = null;

            if (this.Variables.ContainsKey(name))
            {
                return this.Variables[name];
            }

            return val;
        }

        public object GetChildValue(String Path, String Name)
        {
            if (this.Elements.ContainsKey(Path))
            {
                if (this.Elements[Path].Variables.ContainsKey(Name))
                {
                    return this.Elements[Path].Variables[Name].Value;
                }
            }

            return null;
        }

        [ManagedFunction]
        protected void TestFunction(int Interval)
        {
            int i = 0;

            if (Interval < 1000)
                Interval = 1000;

            while (!IsFunctionAborted())
            {
                //RaiseEvent(9999, ChuteTest1);
                if (i > 1000) i = 0;
                Wait(Interval);
            }
        }
        #endregion

        [DllImport("kernel32.dll")]
        static extern bool SetSystemTime(ref SYSTEMTIME time);

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEMTIME
        {
            public ushort Year;
            public ushort Month;
            public ushort DayOfWeek;
            public ushort Day;
            public ushort Hour;
            public ushort Minute;
            public ushort Second;
            public ushort Milliseconds;

            public SYSTEMTIME(DateTime dt)
            {
                Year = (ushort)dt.Year;
                Month = (ushort)dt.Month;
                DayOfWeek = (ushort)dt.DayOfWeek;
                Day = (ushort)dt.Day;
                Hour = (ushort)dt.Hour;
                Minute = (ushort)dt.Minute;
                Second = (ushort)dt.Second;
                Milliseconds = (ushort)dt.Millisecond;
            }
        }
    }
}

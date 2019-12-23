/// <summary>
/// ECS.Core.WheelSorterServer의 요약
/// </summary>
/// <filename>WheelSorterSimulator.cs</filename>
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

using ECS.Common;
using ECS.Driver;
using Database.IF;
using System.Collections.Concurrent;
using System.Threading;

using Oracle.ManagedDataAccess.Client;

namespace ECS.Simulator
{
    /// <summary>
    /// 사용 : ezControl Modeler 의 WheelSorterSimulator Logic
    /// IOMudule에서 완료된 항목을 DBMS에 처리 
    /// </summary>
    public partial class WheelSorterSimulator : CElement
    {
        #region variable
        CSorterItemDataManager _itemDataMgr;

        protected DBInterface dbinterface;

        SocketDriver _drvSocket;

        System.Timers.Timer heartbeatTimer;
        private bool _bHeartBeat = true;
        private int _heartbeatCounter = 0;

        object _lock = new object();

        public int sorterMainSendPort;
        public int sorterSortedConfirmRecvPort;
        public int sorterDestinationSendPort;

        private const string simulLogNm = "Simulator Log";

        Random rnd = new Random();

        #endregion

        #region system override
        protected override void OnInstancingCompleted()
        {
            base.OnInstancingCompleted();

            dbinterface = new DBInterface();

            sorterMainSendPort = PORT;
            sorterSortedConfirmRecvPort = PORT + 1;
            sorterDestinationSendPort = PORT + 2;

            _itemDataMgr = new CSorterItemDataManager();

            _drvSocket = new SocketDriver(EQP_ID, 3, IP, PORT, false);
            _drvSocket.OnRecievePacket += _drvSocket_OnRecievePacket;
            _drvSocket.OnConnect += drv_OnConnect;
            _drvSocket.EnableLog(true, true);

            foreach (CElement element in this.Elements.Values)
            {
                System.Threading.Thread.Sleep(1);
            }            

            heartbeatTimer = new System.Timers.Timer()
            {
                Interval = 5 * 1000
            };
            heartbeatTimer.Elapsed += new System.Timers.ElapsedEventHandler(heartbeatTimer_Elapsed);
            //heartbeatTimer.Start();

        }

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

        private void heartbeatTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            HeartBeat();

            if (_bHeartBeat)
            {
                if (_heartbeatCounter > WheelSorterTelegram.HEARTBEAT_TIMEOUT_COUNT)
                {
                    _drvSocket.TryPLCReconnection();
                }
                _heartbeatCounter++;
            }
        }

        private void HeartBeat()
        {
            WheelSorterTelegram.Heartbeat send = new WheelSorterTelegram.Heartbeat();
            _drvSocket.SendData(send.MakePacket(EQP_ID), sorterMainSendPort);
        }

        private void ItemSortedConfirm(List<short> message)
        {
            WheelSorterTelegram.ItemSortedConfirm send = new WheelSorterTelegram.ItemSortedConfirm(message);
            _drvSocket.SendData(send.MakePacket(EQP_ID), sorterSortedConfirmRecvPort);
        }

        protected override void OnUnloaded()
        {
            base.OnUnloaded();
        }

        void __DEBUG_OnBooleanChanged(CVariable sender, bool value)
        {
            if (_drvSocket != null)
            {
                _drvSocket.EnableLog(value, value);
            }
        }

        void drv_OnConnect(string key, bool connected)
        {
            SystemLogger.Log(String.Format("Connection {0} {1}", key, connected), "CLIENT_INFO");
        }


        #endregion

        #region [Track Socket와 통신 메시지]

        #region [_drvSocket_OnRecievePacket(WheelSorterTelegram sender, List<short> listdata)]
        void _drvSocket_OnRecievePacket(CommonHeader sender, List<short> listdata)
        {
            switch ((WheelSorterTelegram.TELEGRAM)sender.TelegramNo)
            {
                case WheelSorterTelegram.TELEGRAM.HEARTBEAT:
                    _heartbeatCounter = 0;
                    break;
                case WheelSorterTelegram.TELEGRAM.DESTINATION_SEND:   //여기서 부터 Simulator
                    DestinationRecv(listdata);
                    break; 
                default:
                    break;
            }
        }
        #endregion

        #endregion

        #region [Test]

        #region [Test Sorted Confirm]
        public void TestSortedConfirm()
        {
            try
            {
                List<Byte> packetData = new List<byte>();
                packetData.Add((byte)CConstant._stx);
            }
            //try
            //{
            //    List<OracleParameter> listPara = new List<OracleParameter>();                
            //    listPara.Add(new OracleParameter("P_PID", OracleDbType.Varchar2, _pid.ToString(), ParameterDirection.Input));
            //    listPara.Add(new OracleParameter("T_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));

            //    DataSet ds50 = dbinterface.WcsProcedureCallGetDataSet("SP_TEST_PROGRESS", listPara);
            //    DataTable dt50 = ds50.Tables[0];

            //}
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, string.Format("[{0}]{1} : {2}", this.Name, ex.StackTrace, ex.Message));
            }

        }
        public void TestHubFlowMode(int btch_seq)
        {
            try
            {

                System.Threading.ThreadPool.QueueUserWorkItem(delegate
                {
                    TestHubMode(btch_seq, 40);
                    System.Threading.ThreadPool.QueueUserWorkItem(delegate
                    {
                        TestHubMode(btch_seq, 50);
                    });

                });      

            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, string.Format("[{0}]{1} : {2}", this.Name, ex.StackTrace, ex.Message));
            }
        }

        public void TestHubMode(int btch_seq, int stat_cd)
        {
            try
            {
                List<OracleParameter> listPara = new List<OracleParameter>();
                listPara.Add(new OracleParameter("P_BTCH_SEQ", OracleDbType.Int32, btch_seq, ParameterDirection.Input));
                listPara.Add(new OracleParameter("P_EQP_ID", OracleDbType.Varchar2, EQP_ID, ParameterDirection.Input));

                switch (stat_cd)
                {                   
                    
                    case 50: //sortedconfirm
                        listPara.Add(new OracleParameter("P_STAT_CD", OracleDbType.Varchar2, (stat_cd - 10).ToString(), ParameterDirection.Input));
                        listPara.Add(new OracleParameter("T_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));

                        DataSet ds50 = dbinterface.WcsProcedureCallGetDataSet("SP_TEST_PROGRESS", listPara);
                        DataTable dt50 = ds50.Tables[0];
                        foreach (DataRow dr in dt50.Rows)
                        {
                            //TestSortedConfirm(CUtil.GetNumber(dr[0].ToString()), CUtil.GetShortNumber(dr[2].ToString()), 0, CUtil.GetShortNumber(dr[1].ToString()));
                            Thread.Sleep(400);
                        }
                        break;
                    default:
                        break;

                }

            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, string.Format("[{0}]{1} : {2}", this.Name, ex.StackTrace, ex.Message));
            }

        }
        #endregion      

        private void DestinationRecv(List<short> Message)
        {
            WheelSorterTelegram.DestinationSend recv = new WheelSorterTelegram.DestinationSend(Message);

            FireToUIEvent(CEnum2.EnumToUIEvent.IPSRead, null);

            SystemLogger.Log(Level.Info, String.Format("< Destination Receive OK > [PID ={0}][CHUTE={1}]", recv.ParcelID, recv.Destination[0]), simulLogNm);

            ItemSortedConfirm(Message);
            /*
            WheelSorterTelegram.ItemSortedConfirm itemSortedcfm = new WheelSorterTelegram.ItemSortedConfirm(Message);
            SystemLogger.Log(Level.Info, String.Format("< ItemSortedConfirm = ? > [PID ={0}][CHUTE={1}][REASCODE={2}][SENSORYN={3}]", itemSortedcfm.ParcelID, itemSortedcfm.ChuteNumber, itemSortedcfm.ReasCode,itemSortedcfm.SensorYN), simulLogNm);
                     */                                                                               

        }


        #endregion

        #region public method


        #region From UI Event
        public int UIEventReceiver(int iEventID, params object[] args)
        {
            try
            {
                switch (iEventID)
                {                    
                    case (int)CEnum2.EnumToCoreEventForSimulator.TestSorter:
                        TestHubFlowMode(CUtil.GetNumber(args[0].ToString()));
                        break;  
                    case (int)CEnum2.EnumToCoreEventForSimulator.TestSortedConfirm:
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
    }
}

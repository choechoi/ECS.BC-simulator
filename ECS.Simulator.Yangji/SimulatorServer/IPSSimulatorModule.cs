/// <summary>
///  IpsModule의 요약
/// </summary>
/// <filename>IpsModule.cs</filename>
/// <version>1.0.0.0</version>
/// <authors></authors>
/// <modifications>
/// 	1.	 ver1.0.0.0		2018.04.18 00:00:00		Devloper Name	- LIM HYUNG TAEK : Created.	
/// </modifications>
/// <copyright>Copyright (c) 2007~. EzControl, LG CNS All rights reserved.</copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using System.Threading;

using LGCNS.ezControl.Diagnostics;
using LGCNS.ezControl.Common;
using LGCNS.ezControl.Core;

using ECS.Driver;
using ECS.Common;


namespace ECS.Simulator
{
    /// <summary>
    /// IPSManager의 요약
    /// </summary>
    public partial class IpsSimulatorModule : CElement
    {
        #region delegate
        public delegate void delegateConnectionState(string key, int connected);
        public delegate void delegateUpdateLogText(String sockName, String log);
        public delegate void delegateUpdateMakeLogText(String sockName, int direction, byte[] packet);
        public delegate void delegateIPSResult(IpsTelegram.IPSResult Result);
        #endregion

        #region event
        public event delegateIPSResult OnIPSResult;

        public event delegateConnectionState OnConnect;
        public event delegateUpdateLogText OnUpdateLog;
        #endregion

        #region variable
        SocketDriverIPS _drvIPS = null;

        public int IPS_SendPort_2;

        private const string simulIpsLogNm = "IPS Log";

        #endregion

        #region system override
        /// <summary>
        /// Component의 모든 구성요소가 Instancing완료 되었을 때 호출
        /// </summary>
        protected override void OnInstancingCompleted()
        {
            base.OnInstancingCompleted();

            InitDriver();

            IPS_SendPort_2 = PORT;
        }
        #endregion

        #region public method
        public void InitDriver()
        {
            _drvIPS = new SocketDriverIPS(EQP_ID, IP, PORT);

            _drvIPS.OnRecieveMessage += _drvIPS_OnRecieveMessage;
            _drvIPS.OnConnect += Drv_OnConnect;
            _drvIPS.InitServer();

            __DEBUG.OnBooleanChanged += __DEBUG_OnBooleanChanged;
            DEBUG = true;
        }

        public void SendData(string str)
        {
            _drvIPS.SendMessage(str);
        }

        void __DEBUG_OnBooleanChanged(CVariable sender, bool value)
        {
            _drvIPS.EnableLog(value);
        }

        void Drv_OnConnect(string key, bool connected)
        {
            SystemLogger.Log(Level.Info, String.Format("Connection {0} {1}", key, connected));
            __IS_CONNECT.AsBoolean = connected;
        }

        void _drvIPS_OnRecieveMessage(String message)
        {
            if (String.IsNullOrEmpty(message) == false)
            {
                IpsTelegram.IPSResult result = new IpsTelegram.IPSResult(message);

                if (OnIPSResult != null && result.DataType == IpsTelegram.IPSDataType.Data)
                {
                    OnIPSResult(result);
                }
            }
        }

        #endregion

        #region [IPS DataSend Test]
        public void Test_IPS_Data_Send(string barcode)
        //string dataType, string iTSDateTime, string iTSID, string ParcelID, string iTSData1, string iTSData2, string width, string length, string height)
        {    
            byte[] strMessage = Encoding.UTF8.GetBytes(string.Format("D,{0},{1}", GetparcelID(), barcode));

            List<Byte> packetData = new List<byte>();
            packetData.Add((byte)CConstant._stx);
            foreach (byte element in strMessage)
            {
                packetData.Add(element);
            }
            packetData.Add((byte)CConstant._etx);

            //CUtil.ByteToString();

            _drvIPS.SendData(packetData.ToArray());
            SystemLogger.Log(Level.Info, String.Format("<Test_IPS_Data_Send> [Code={0}] [Status={1}]", "OK", "Good"), simulIpsLogNm);

            Thread.Sleep(400);

            //}
        }

        public int GetparcelID()
        {
            int nHeader = CUtil.GetNumber(Name.Substring(Name.Length - 1)) * 10000;

            if (nHeader < 0)
                nHeader = 0;

            this.parcelID++;
            __parcelID.Save();

            if (this.parcelID >= nHeader || this.parcelID <= 0)
                this.parcelID = 1;


            return nHeader + this.parcelID;
        }

        #endregion

        // 시뮬 UI에서 받는다
        #region From UI Event
        public int UIEventReceiver(int iEventID, params object[] args)
        {
            try
            {
                switch (iEventID)
                {
                    case (int)CEnum2.EnumToCoreEventForSimulator.TestIPSData:
                        Test_IPS_Data_Send(args[0].ToString());
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
    }
}

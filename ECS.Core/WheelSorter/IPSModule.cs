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

using LGCNS.ezControl.Diagnostics;
using LGCNS.ezControl.Common;
using LGCNS.ezControl.Core;

using ECS.Driver;
using ECS.Common;


namespace ECS.Core
{
    /// <summary>
    /// IPSManager의 요약
    /// </summary>
    public partial class IpsModule : CElement
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

        #endregion

        #region system override
        /// <summary>
        /// Component의 모든 구성요소가 Instancing완료 되었을 때 호출
        /// </summary>
        protected override void OnInstancingCompleted()
        {
            base.OnInstancingCompleted();
        }
        #endregion

        #region public method
        public void InitDriver()
        {
            _drvIPS = new SocketDriverIPS(EQP_ID, IP, PORT);

            _drvIPS.OnRecieveMessage += _drvIPS_OnRecieveMessage;
            _drvIPS.OnConnect += Drv_OnConnect;
            _drvIPS.Init();

            __DEBUG.OnBooleanChanged += __DEBUG_OnBooleanChanged;
            DEBUG = true;
            _drvIPS.bHeartBeat = true;
        }

        protected override void OnUnloaded()
        {
            base.OnUnloaded();

            _drvIPS.Close();
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
    }
}

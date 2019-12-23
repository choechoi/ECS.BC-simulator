using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECS.Common 
{
    public class ClientConstant
    {
        public const string MasterID = "MASTERID";
        public const string ClearKey = "DATA CLEAR";

        public const string UIEventReceiver = "UIEventReceiver";

        public const string ServerConnected = "SERVER CONNECTED";
        public const string ServerDisconnected = "SERVER DISCONNECTED";

        public const int BarcodeInputMin = 3;

        /* Enum.CS로 변경
        public enum EnumToUIEvent
        {
            // connect
            Connected,
            Disconnected,
            CheckReply,
            SendResult,      // int result  0보다 작으면 error 
            KioskSetInductionMode,
            SendResultClear,

            kioskRead = 1000,
            Inducted,
            IPSRead,
            Discharged,
            SortedConfirm,

            TestValueRcv = 2000,
            TestRetrieve,
            TestItemStart,

        }

        public enum EnumToCoreEvent
        {
            //UIStarted,
            //BarcodeRequest = 100,
            //KioskSetInductionMode,
            //CommandData,
            //SendBarcode,
            //SendPCSCnt,

            //Start,
            //Stop,

            //QpsOutboundRequest,

            UIStarted,
            BarcodeRequest = 100,
            KioskSetInductionMode,
            CommandData,
            SendBarcode,
            SendPCSCnt,
            ChangeWHInform,
            DasReleaseStart,

            Start,
            Stop,

            QpsOutboundRequest,

        }

        public enum EnumToCoreEventForSimulator
        {
            TestWareHousing = 100,
            TestBarcode,
            TestWeightBarcode,
            TestPreSorting,

            TestInducted = 200,
            TestIPSData,
            TestPCSSorter,
            TestHUBSorter,
            TestKioskInput,

            TestIPSData = 300,
        }

        public enum EnumCommandParcelData
        {
            DataClear,
            ParcelPass,
        }
        */

        public class AppConfig
        {
            public const string ClientID = "CLIENT_ID";
        }

    }
}


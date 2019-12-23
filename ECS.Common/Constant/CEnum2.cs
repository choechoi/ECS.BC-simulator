using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECS.Common
{
    public class CEnum2
    {  
        public enum EnumRESULT
        {
            OK = 0,
            HEADER_INVAILD,
            WRONG_ITEM,
            FILE_BROKEN,
        }

        public enum EnumECSType
        {
            Sorter = 0,
            Conveyor,
            Qps,
            Minlload,
            IPS,
            ECS,
            MPS,
            DAS,
            IFD,
            OPENDB
        }

        public enum EnumConveyorType
        {
            NORMAL = 0,
            BCRRead,
            BCRWeightRead,
            BCRWeightVolumRead,
            BCRPresorting,
            AUTOLABELER,
            ITS,

        }

        public enum EnumToCoreEvent
        {
            UIStarted,
            BarcodeRequest = 100,
            KioskSetInductionMode,
            CommandData,
            SendBarcode,
            SendPCSCnt,
            ItemStart,
            KioskRepYN,

            Start,
            Stop,            

            QpsOutboundRequest,

            SetSorterConfig,

            TestDataMemoryUpdate = 1000,
            TestDataMemoryClear,


            QpsCellAssign,
            QpsInspectionResult,
            DpsStockInspection,
            DpsStockRelease,


            StockTakingRequest,
            InventoryDischargedRequest,
            Picking,     

            InspConvStatus,

        }
        //public enum EnumToCoreEvent
        //{
        //    //UIStarted,
        //    //BarcodeRequest = 100,
        //    //ChcekConnection,
        //    //KioskSetInductionMode,
        //    //CommandData,
        //    //ChangeWHInform,
        //    //DasReleaseStart,

        //    //TestDataMemoryUpdate = 1000,
        //    //TestDataMemoryClear
        //}        

        public enum EnumToCoreEventForSimulator
        {
            TestInducted = 1000,
            TestIPSData,
            TestSorter,            
            TestSortedConfirm,
            TestBarcode,
        }       

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
            LineStop,

            ValueRcv = 2000,
            Retrieve,
            ItemStart,

        }

        public enum EnumModeType
        {
            Empty = -1, //Overflow
            IPS=0,
            Kiosk =1,
            UnKnown,
        }

        public enum EnumChuteStatus
        {
            Normal = 0,
            Full,
            Block,
        }  

        

        public enum EnumCommandParcelData
        {
            DataClear,
            ParcelPass,
        }
    }
}


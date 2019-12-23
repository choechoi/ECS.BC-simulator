using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECS.Common
{
    public class CConstant
    {
        public static string NoRead = "?";

        public class SortedConfirmReasonCode
        {
            public const short Normal = 1;  // 정상 배출
            public const short ChuteFull = 2; // 슈트 만재 : 리젝트 슈트로 배출 
            public const short ChuteBlocked = 3;  // 슈트 블럭 : 리젝트 슈트로 배출
            public const short CarrierDeactiovation = 4;  // 없음(사용안함)
            public const short DischargeError = 5; // 없음(사용안함)
            public const short TrackingFailure =6; // 없음(사용안함)
            public const short NoDestination = 7; // 목적지 없음 : 리젝트 슈트로 배출
            public const short DestinationError = 8; // 없음(사용안함)
            public const short MultiCarrierDestinations = 9; // 없음(사용안함)
            public const short NoDischargedCarrierDetected = 10; // 배출이 안되었는데, SPS에서 화물 감지 못함.
            public const short DischargedCarrierNoDetected = 11; // 배출되었는데, SPS에서 화물 감지함.
            public const short AutoBlock = 12; // 목적지 슈트로 배출하기전에 BLDC모터 이상이 발견된 경우 : 리젝트 슈트로 배출
            public const short ParcelPositionAbnormal = 13; // 화물이 카트사이에 로딩된 경우 : 리젝트 슈트로 배출
            public const short BadLoardingFromInduction = 14; // 인덕션투입시 정상적으로 카트에 로딩되지 않은 경우 : 스캔하지 않고 리젝트 슈트로 배출

        }

        public class ProcPnm
        {
            public const string COMPANY_CD = "P_CO_CD";
            public const string CENTER_CD = "P_CNTR_CD";
            public const string EQUIPMENT_ID = "P_EQP_ID";
            public const string USER_ID = "P_USER_ID";
            public const string INPUT_TIME = "P_INPUT_DT";
            public const string PARCEL_ID = "P_PID";
            public const string INDUCTION_NO = "P_INDT_NO";
            public const string BARCODE = "P_BCR_NO";
            public const string BOX_TYPE_CD = "P_BOX_TYPE_CD";
            public const string OUT_BCR_NO = "P_OUT_BCR_NO";
            public const string BOX_WEIGHT = "P_BOX_WGT";
            public const string CART_NO = "P_CART_NO";
            public const string CART_CNT = "P_CART_CNT";
            public const string MODE = "P_INPUT_MODE";
            public const string CHUTE_NO = "P_CHUTE_NO";
            public const string RECIRCULATION_CNT = "P_RECIRC_CNT";
            public const string SPS_POSITION = "P_SPS_LOC_CD";
            public const string REASON_CD = "P_SRT_RSN_CD";

            public const string SENSOR_YN = "P_CHUTE_SENS_RCG_YN";

            public const string INV_BCD = "P_INV_BCD";
            public const string BOX_BCD = "P_BOX_BCD";
            public const string RGN_BCD = "P_RGN_BCD";

            public const string O_CHUTE_NO1 = "O_DEST_CHUTE_ID_01";
            public const string O_CHUTE_NO2 = "O_DEST_CHUTE_ID_02";
            public const string O_CHUTE_NO3 = "O_DEST_CHUTE_ID_03";

            public const string SP_RESULT_CD = "O_RTN_CD";
            public const string SP_RESULT_MSG = "O_RTN_MSG";

            public const string O_RSLT_LIST = "O_BCD_REGEX_LIST";
            public const string O_RSLT = "O_RSLT";
        }

        public class KioskOverflow
        {
            public const string Count = "999";
            public const string BcrData = "?";
        }

        public class PROCEDURE_RESULT
        {
            /// <summary>
            /// 프로시저 호출 결과 값에서
            /// Chute = 0 이면 IPS 또 읽게 함
            /// Socket에 아무 보고 하지 않음!!
            /// </summary>
            public const string REWORK = "0";
            public const string REJECT = "1";
        }

        public class IO
        {
            public const ushort ON = 1;
            public const ushort OFF = 0;
            public const ushort CONFIRM = 2;
            public const ushort ESTOP = 99;
        }

        public class CenterCode
        {
            public const string Dongtan = "DT";
            public const string Yangji = "YJ";
            public const string OliveYoung = "OY";

        }
        
        public class ChuteVariableName
        {
            public const string CategoryName = "Chute";
            public const string PCS_REST_QTY = "PCS_REST_QTY";
            public const string IPS_COMP_QTY = "IPS_COMP_QTY";
            public const string CUR_DISC_QTY = "CUR_DISC_QTY";
            public const string DSC_COMP_QTY = "DSC_COMP_QTY";
            public const string CUR_BATCH_ID = "CUR_BATCH_ID";
            public const string CUR_BOX_TYPE = "CUR_BOX_TYPE";
            public const string CUR_DISC_QTY_Socket = "CUR_DISC_QTY_Socket";
            public const string Main_Switch_Activity = "Main_Switch_Activity";
            public const string Main_Switch_Activity_OUT = "Main_Switch_Activity_OUT";

            public const string Sub_Switch2_Activity = "Sub_Switch2_Activity";
            public const string Sub_Switch2_Activity_OUT = "Sub_Switch2_Activity_OUT";
        }
        public static char[] seperator = new char[1] { ',' };

        public const char _stx = (char)0x02;
        public const char _etx = (char)0x03;
        public const char _eot = (char)0x04;
        public const char _enq = (char)0x05;
        public const char _ack = (char)0x06;
        public const char _nck = (char)0x15;

        public const char _OD = (char)13;
        public const char _OA = (char)10;

        public static String STX = _stx.ToString();
        public static String ETX = _etx.ToString();
        public static String EOT = _eot.ToString();
        public static String ENQ = _enq.ToString();
        public static String ACK = _ack.ToString();
        public static String NCK = _nck.ToString();

        public const string DATETIMEFORMAT = "yyyy-MM-dd HH:mm:ss";
        public const string DATEFORMAT = "yyyyMMdd";  //yyyyMMddHHmmss
        public const string TIMEFORMAT = "HHmmss";

        public const string YES = "Y";
        public const string NO = "N";        

        public class CHUTE_TYPE
        {
            public const string NORMAL = "N";
            public const string REWORK = "W";
            public const string REJECT = "R";

            public const int MAX_CHUTE = 5;
            public const int MAX_REJECT_CHUTE = 2;
            public const int MAX_REWORK_CHUTE = 2;
        }

        public class FunctionName
        {
            public const string GetInductionMode = "GetInductionMode";
        }

 
    }
}


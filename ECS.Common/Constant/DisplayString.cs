using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ECS.Common 
{
    public class DisplayString_ENG
    {

        public class ClientMessage
        {
            public const string Warning = "Warning";

            public const string MtoAConfirm = "To change induction mode, \r\n(Items on the induction should be all cleared before change)";
            public const string AtoMConfirm = "To change induction mode, \r\n(Items on the induction should be all cleared before change)";
        }

        public class DisplayInfomation
        {
            public const string FUNCTION_F1 = "Press IPS(Auto) Mode Key";
            public const string FUNCTION_F2 = "Press Kiosk(Manual) Mode Key";
            public const string FUNCTION_F3 = "Press Skip Key";
            public const string FUNCTION_F4 = "Press DATACLEAR Key";
            public const string FUNCTION_F5 = "Press Start SKU Barcode Print Mode Key";
            public const string FUNCTION_F6 = "Press End Multi Key-In";
        }

        // client string 과 mapping 이 잘 되어야 하므로 string은 아니지만 여기로 옮겨둠.  
        public enum enumClientErrorMessage
        {
            OK,
            NEWCODEOK,
            NEEDGROUPCODE,
            NOTEXISTPARCELCODE,
            NOTEXISTPOSTALCODE,
            NOTEXISTDESTINATION,
            DUPLICATIONCODE,
            INPUTLENTHCHECK,
            SHOULDFILLLESSONE,
            NORESPONSE,
            // for auto. 
            REGISTED,
            NEEDMOREINFO,
            NEWCODEREGISTED,
            REGISTEDERROR,
            LENTHERROR,
            DBERROR,
            NOCHUTENO,
            IPSMODECHECK,
        }


        public static String[] ClientErrorMessage =
        {
            "OK",
            "Postal code has been registered. ",
            "Please insert Sorting Group Code",
            "Parcel Information cannot be found. Please enter correct postal code, route code or national code. ",
            "The postal code information doesn’t exist. ",
            "This postalcode's destination chute information doesn’t exist. ",
            "The parcel code has already been processed.",
            "Input code lenth is wrong. Please Check Again",
            "You Should fill in one feild at least.",
            "No response.",
            "OK. Resisted item in server.",
            "Input postal code or group code or both.",
            "Postal code has been registered. ",
            "System internal error occurred during regist new postal code to data server. Please contact your system administrator!",
            "Parcel Code Length must be more then {0}",
            "DB Procedure Error",
            "Destination Chute No is Zero",
            "Scan twice mode change or manual barcode scan not allowed.",
        };

        public static string GetClientErrorMessage(int result)
        {
            if (result >= ClientErrorMessage.Length || result < 0)
            {
                //return "코드를 찾는 중 에러가 발생하였습니다. ";
                return "System internal error occurred due to unknown reason. Please contact your system administrator!";
            }

            return ClientErrorMessage[result];
        }
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ECS.Common;

namespace ECS.Driver
{
    public class IpsTelegram
    {
        public const int HEARTBEAT_INTERVAL = 5000;
        public const int HEARTBEAT_TIMEOUT_COUNT = 6;
        public const String IPS_HEARTBEAT_DATA = "OK";
        public static string[] seperator = new string[1] { "," };

        public String DataType;
        public String BcrID;

        public class IPSHeartBeat
        {
            public String ID;

            public String MakeMessage()
            {
                StringBuilder sb = new StringBuilder();

                sb.Append((char)2);
                sb.Append(IPSDataType.HeartBeat);
                sb.Append(seperator[0]);

                sb.Append(ID);
                sb.Append(seperator[0]);

                sb.Append(IPS_HEARTBEAT_DATA);
                sb.Append((char)3);

                return sb.ToString();
            }
        }

        public class IPSResult
        {
            public string DataType;
            public string parcelID;
            public string IPSData;

            public IPSResult()
            {
            }

            public IPSResult(String Message)
            {
                ParseData(Message);
            }

            public void ParseData(String Message)
            {
                try
                {
                    string[] arrParseData = Message.Split(seperator, StringSplitOptions.RemoveEmptyEntries);

                    DataType = arrParseData[0];

                    if (DataType == IPSDataType.HeartBeat)
                    {
                        return;
                    }                    
                    
                    parcelID = CUtil.GetNumber(arrParseData[1]).ToString();
                    IPSData = arrParseData[2];
                    
                    //IPScnt = arrParseData.Length - 4;
                    //IPSData = new List<string>();

                    //for (int i = 4; i < arrParseData.Length; i++)
                    //{
                    //    IPSData.Add(arrParseData[i]);
                    //}
                }
                catch(Exception ex)
                {
                    return;
                }
            }
        }

        public class IPSResultYJ
        {
            public string DataType;
            public string IPSID;
            public string CartNo;
            public string parcelID;
            public short InductionNo;
            public int IPScnt;
            public string IPSData;

            public string Dest;    // Test 후 삭제 요망

            public IPSResultYJ()
            {
            }

            public IPSResultYJ(String Message)
            {
                ParseData(Message);
            }

            public void ParseData(String Message)
            {
                try
                {
                    string[] arrParseData = Message.Split(seperator, StringSplitOptions.RemoveEmptyEntries);

                    DataType = arrParseData[0];

                    if (DataType == IPSDataType.HeartBeat)
                    {
                        return;
                    }

                    IPSID = arrParseData[1];
                    CartNo = CUtil.GetNumber(arrParseData[2]).ToString();
                    parcelID = CUtil.GetNumber(arrParseData[3]).ToString();
                    InductionNo = CUtil.GetShortNumber(arrParseData[4]);
                    //////////////////////////////////////////////////////////////////////// TEST//////////////////////////////////
                    Dest = arrParseData[5];  // Test 후 삭제 요망
                    IPSData = arrParseData[6];
                    IPScnt = arrParseData[6].Split(';').Length;

                }
                catch (Exception ex)
                {
                    return;
                }
            }
        }

        public class IPSDataType
        {
            public const string HeartBeat = "H";
            public const string Data = "D";
            public const string Error = "E";
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ECS.Common;

namespace ECS.Driver
{
    public class CommonHeader
    {
        public String DataType;
        public String ModuleID;
        public short TelegramNo;
        public short DataLength;

        public CommonHeader()
        {
        }

        public CEnum2.EnumRESULT SetHeader(byte[] Packet)
        {
            if (Packet.Length < GetHeaderSize())
                return CEnum2.EnumRESULT.HEADER_INVAILD;

            DataType = ((char)Packet[0]).ToString();
            ModuleID = Encoding.Default.GetString(Packet, 1, 4);
            TelegramNo = CUtil.ByteToShort(Packet[6], Packet[5]);
            DataLength = CUtil.ByteToShort(Packet[8], Packet[7]);

            return CEnum2.EnumRESULT.OK;
        }

        public static int GetHeaderSize()
        {
            // stx 제외
            return 9;
        }

        public class SOCKET_PACKET_TYPE
        {
            public const string HeartBeat = "H";
            public const string Data = "D";
            public const string Request = "R";
            public const string Acknowledge = "A";
        }

    }
}

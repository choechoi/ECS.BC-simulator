using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ECS.Common;

namespace ECS.Driver
{
    public class WheelSorterTelegram
    {
        public const int HEARTBEAT_INTERVAL = 5000;
        public const int HEARTBEAT_TIMEOUT_COUNT = 6;

        public enum TELEGRAM
        {
            HEARTBEAT = 1,
            ITEM_SORTEDCONFIRM = 22,
            DESTINATION_SEND = 30,
            SET_CONFIG_REQUEST = 60,
            SET_CONFIG_ACK = 61,
            TIME_ALIGNMENT_REQUEST = 70, 
        }

        public String DataType;
        public String ModuleID;
        public short TelegramNo;
        public short DataLength;

        public WheelSorterTelegram()
        {
        }

        public byte[] MakePacketHeader()
        {
            byte[] packetData = new byte[9];

            packetData[0] = Convert.ToByte(DataType[0]);
            packetData[1] = Convert.ToByte(ModuleID[0]);
            packetData[2] = Convert.ToByte(ModuleID[1]);
            packetData[3] = Convert.ToByte(ModuleID[2]);
            packetData[4] = Convert.ToByte(ModuleID[3]);

            packetData[5] = CUtil.ShortToByte(TelegramNo)[0];
            packetData[6] = CUtil.ShortToByte(TelegramNo)[1];

            packetData[7] = CUtil.ShortToByte(DataLength)[0];
            packetData[8] = CUtil.ShortToByte(DataLength)[1];

            return packetData;
        }

        public byte[] MakePacket(List<short> data)
        {
            List<Byte> packetData = new List<byte>();
            packetData.Add((byte)CConstant._stx);
            packetData.AddRange(MakePacketHeader());
            packetData.AddRange(CUtil.ShortListToByte(data));
            packetData.Add((byte)CConstant._etx);

            return packetData.ToArray();
        }

        public byte[] MakePacket(List<short> data, string labelData, List<short> data2)
        {
            List<byte> packetData = new List<byte>();
            packetData.Add((byte)CConstant._stx);
            packetData.AddRange(MakePacketHeader());
            packetData.AddRange(CUtil.ShortListToByte(data));
            packetData.AddRange(CUtil.StringToUTF8ByteList(labelData));           
            packetData.AddRange(CUtil.ShortListToByte(data2));
            packetData.Add((byte)CConstant._etx);

            return packetData.ToArray();
        }

        #region Send message

        public class Heartbeat
        {
            public short status = 1;
 
            public List<short> StructToShortList()
            {
                List<short> data = new List<short>();
                data.Add(status);

                return data;
            }

            public byte[] MakePacket(String moduleID)
            {
                List<short> data = StructToShortList();

                if (data == null)
                    return null;

                WheelSorterTelegram header = new WheelSorterTelegram();

                header.DataType = CommonHeader.SOCKET_PACKET_TYPE.HeartBeat;
                header.ModuleID = moduleID;
                header.TelegramNo = (short)TELEGRAM.HEARTBEAT;
                header.DataLength = (short)(data.Count * 2);

                return header.MakePacket(data);
            }
        }         

        public class DestinationSend
        {            
            public short ParcelID;
            public List<short> Destination;

            public DestinationSend()
            { }

            public DestinationSend(List<short> data)
            {
                Destination = new List<short> { 0, 0, 0, 0, 0 };

                ParcelID = data[0];                
                Destination[0] = data[1];
            }

            public List<short> StructToShortList()
            {
                List<short> data = new List<short>();         
                data.Add(ParcelID);
                data.AddRange(Destination);

                return data;
            }

            public byte[] MakePacket(String moduleID)
            {
                List<short> data = StructToShortList();

                if (data == null)
                    return null;

                WheelSorterTelegram header = new WheelSorterTelegram();

                header.DataType = CommonHeader.SOCKET_PACKET_TYPE.Data;
                header.ModuleID = moduleID;
                header.TelegramNo = (short)TELEGRAM.DESTINATION_SEND;
                header.DataLength = (short)(data.Count * 2);

                return header.MakePacket(data);
            }
        } 

        public class SetConfiguration
        {           
            public short ErrorChute1;
            public short ErrorChute2;

            public SetConfiguration()
            {

            }

            public SetConfiguration(List<short> data)
            {                
                ErrorChute1 = data[0];
                ErrorChute2 = data[1];
            }
            public List<short> StructToShortList()
            {
                List<short> data = new List<short>();
                
                data.Add(ErrorChute1);
                data.Add(ErrorChute2);
                return data;
            }

            public byte[] MakePacket(String moduleID)
            {
                List<short> data = StructToShortList();

                if (data == null)
                    return null;

                WheelSorterTelegram header = new WheelSorterTelegram()
                {
                    DataType = CommonHeader.SOCKET_PACKET_TYPE.Request,
                    ModuleID = moduleID,
                    TelegramNo = (short)TELEGRAM.SET_CONFIG_REQUEST,
                    DataLength = (short)(data.Count * 2)
                };
                return header.MakePacket(data);
            }

        }        

        public class TimeAlignmentRequest
        {
            public short Year;
            public short Month;
            public short Day;
            public short Hour;
            public short Minute;
            public short Second;

            public TimeAlignmentRequest()
            {

            }

            public TimeAlignmentRequest(List<short> data)
            {
                Year = data[0];
                Month = data[1];
                Day = data[2];
                Hour = data[3];
                Minute = data[4];
                Second = data[5];
            }

            public List<short> StructToShortList()
            {
                List<short> data = new List<short>();

                data.Add(Year);
                data.Add(Month);
                data.Add(Day);
                data.Add(Hour);
                data.Add(Minute);
                data.Add(Second);

                return data;
            }

            public byte[] MakePacket(String moduleID)
            {
                List<short> data = StructToShortList();

                if (data == null)
                    return null;

                WheelSorterTelegram header = new WheelSorterTelegram()
                {
                    DataType = CommonHeader.SOCKET_PACKET_TYPE.Data,
                    ModuleID = moduleID,
                    TelegramNo = (short)TELEGRAM.TIME_ALIGNMENT_REQUEST,
                    DataLength = (short)(data.Count * 2)
                };
                return header.MakePacket(data);
            }
        }
       
        #endregion

        #region Receive message

        public class ItemSortedConfirm
        {            
            public short ParcelID;
            public short ChuteNumber;
            public short ReasCode;
            public short SensorYN; //

            public ItemSortedConfirm()
            { }

            public ItemSortedConfirm(List<short> data)
            {
                ParcelID = data[0];
                ChuteNumber = data[1];
                ReasCode = data[2];
                SensorYN = data[3]; 
            }

            public List<short> StructToShortList()
            {
                List<short> data = new List<short>();   
                data.Add(ParcelID);
                data.Add(ChuteNumber);
                data.Add(ReasCode);
                data.Add(SensorYN); 
                return data;
            }

            public byte[] MakePacket(String moduleID)
            {
                List<short> data = StructToShortList();

                if (data == null)
                    return null;

                WheelSorterTelegram header = new WheelSorterTelegram()
                {
                    DataType = CommonHeader.SOCKET_PACKET_TYPE.Data,
                    ModuleID = moduleID,
                    TelegramNo = (short)TELEGRAM.ITEM_SORTEDCONFIRM,
                    DataLength = (short)(data.Count * 2)
                };
                return header.MakePacket(data);
            }
        }        

        public class InductionMode
        {
            public short InductionNo;
            public short Mode;

            public InductionMode(List<short> data)
            {
                InductionNo = data[0];
                Mode = data[1];
            }
        }

        public class SetConfigurationAck
        {            
            public short ErrorChute1;
            public short ErrorChute2;
            public short Reason;

            public SetConfigurationAck()
            {

            }

            public SetConfigurationAck(List<short> data)
            {                
                ErrorChute1 = data[0];
                ErrorChute2 = data[1];
                Reason = data[2];
            }

            public List<short> StructToShortList()
            {
                List<short> data = new List<short>();
                data.Add(ErrorChute1);
                data.Add(ErrorChute2);
                data.Add(Reason);

                return data;
            }
            public byte[] MakePacket(String moduleID)
            {
                List<short> data = StructToShortList();

                if (data == null)
                    return null;

                WheelSorterTelegram header = new WheelSorterTelegram()
                {
                    DataType = CommonHeader.SOCKET_PACKET_TYPE.Data,
                    ModuleID = moduleID,
                    TelegramNo = (short)WheelSorterTelegram.TELEGRAM.SET_CONFIG_ACK,
                    DataLength = (short)(data.Count * 2)
                };
                return header.MakePacket(data);
            }
        }                       

        #endregion

    }
}

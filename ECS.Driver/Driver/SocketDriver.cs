using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LGCNS.ezControl.Core;
using LGCNS.ezControl.Driver.Serial;

using ECS.Common;
using LGCNS.ezControl.Diagnostics;

namespace ECS.Driver
{
    public class SocketDriver 
    {
        #region delegate
        public delegate void delegateUpdateText(String Message);
        public delegate void delegateUpdateMakeLogText(int direction, byte[] packet);
        public delegate void delegateRecievePacket(CommonHeader sender, List<short> listdata);

        public delegate void delegateConnectionState(string key, bool connected);
        #endregion

        #region event
        public event delegateUpdateText OnUpdateText;
        public event delegateRecievePacket OnRecievePacket;
        public event delegateUpdateMakeLogText OnUpdateMakeLogText;

        public event delegateConnectionState OnConnect;
        #endregion

        #region variable
        Dictionary<String , CSerialDriver > sock_dic = null;
        private CommonHeader commonHeader = null;
        object _keylock = new object();
        private string EqpId = string.Empty;
        byte[] _byteModule = null;
        private List<byte> _sbPacket = new List<byte>();
        #endregion

        #region public
        public SocketDriver()
        {
            
        }

        public SocketDriver(string module, int socketCount, string IP, int Port, bool isActive)
        {
            if (String.IsNullOrEmpty(IP))
                return;

            int iPort = Port;

            sock_dic = new Dictionary<string, CSerialDriver>();

            EqpId = module;

            for(int i=1; i <= socketCount; i++)
            {
                CSerialDriver serialDrv = new CSerialDriver();
                serialDrv.Name = module + "_" + iPort.ToString();
                serialDrv.ConnectionStateChanged += Sock_ConnectionStateChanged;
                serialDrv.OnReceivedBytes += Recv_sock_OnReceivedBytes;
                if (isActive)
                {
                    serialDrv.ConnectionInfoString = String.Format("MODE=TCP_ACTIVE, IP={0}, PORT={1}, KEEP_ALIVE=1", IP, iPort);
                    serialDrv.ActiveOpen();
                }
                else
                {
                    serialDrv.ConnectionInfoString = String.Format("MODE=TCP_PASSIVE, PORT={0}, KEEP_ALIVE=1", iPort);
                    serialDrv.Open();
                }
                serialDrv.EnableLog(false, false);
                sock_dic.Add(serialDrv.Name, serialDrv);
                iPort++;
            }

            _byteModule = new byte[EqpId.Length];

            for (int i = 0; i < EqpId.Length; i++)
            {
                _byteModule[i] = Convert.ToByte(EqpId[i]);
            }
        }

        public void SocketConnected(string module, int socketCount, string IP, int Port, bool isActive)
        {
            if (String.IsNullOrEmpty(IP))
                return;

            int iPort = Port;

            sock_dic = new Dictionary<string, CSerialDriver>();

            EqpId = module;

            for (int i = 1; i <= socketCount; i++)
            {
                CSerialDriver serialDrv = new CSerialDriver();
                serialDrv.Name = module + "_" + iPort.ToString();
                serialDrv.ConnectionStateChanged += Sock_ConnectionStateChanged;
                serialDrv.OnReceivedBytes += Recv_sock_OnReceivedBytes;
                if (isActive)
                {
                    serialDrv.ConnectionInfoString = string.Format("MODE=TCP_ACTIVE, IP={0}, PORT={1}, KEEP_ALIVE=1", IP, iPort);
                    serialDrv.ActiveOpen();
                }
                else
                {
                    serialDrv.ConnectionInfoString = string.Format("MODE=TCP_PASSIVE, PORT={0}, KEEP_ALIVE=1", iPort);
                    serialDrv.Open();
                }
                serialDrv.EnableLog(false, false);
                sock_dic.Add(serialDrv.Name, serialDrv);
                iPort++;
            }

            _byteModule = new byte[EqpId.Length];

            for (int i = 0; i < EqpId.Length; i++)
            {
                _byteModule[i] = Convert.ToByte(EqpId[i]);
            }
        }

        private void Init()
        {
            foreach (CSerialDriver sock in sock_dic.Values)
            {
                SystemLogger.Log(Level.Info, $"Socket Count = {sock_dic.Count}, Socket Name = {sock.Name}", "Driver");
                sock.ActiveOpen();
            }
        }

        private void PlcInit()
        {
            foreach (CSerialDriver sock in sock_dic.Values)
            {
                SystemLogger.Log(Level.Info, $"Socket Count = {sock_dic.Count}, Socket Name = {sock.Name}", "Driver");
                sock.Open();
            }
        }

        public void EnableLog(bool ASCIILog, bool HEXLog)
        {
            foreach (CSerialDriver sock in sock_dic.Values)
            {
                sock.EnableLog(ASCIILog, HEXLog);
            }
        }

        public void SendData(byte[] buffer, int portNo)
        {
            try
            {
                if (sock_dic[EqpId + "_" + portNo] != null)
                {
                    if (sock_dic[EqpId + "_" + portNo].Connected)
                    {
                        sock_dic[EqpId + "_" + +portNo].Send(buffer);
                    }
                }
                else
                {
                    SystemLogger.Log(Level.Exception, EqpId + portNo + " socket is null.", "Driver");
                }

            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, ex.Message, "Driver");
            }
        }

        public void Close()
        {
            foreach (CSerialDriver sock in sock_dic.Values)
            {
                sock.Close();
            }
        }

        public void TryReconnection()
        {
            Close();

            System.Threading.Thread.Sleep(3000);
            
            Init();
        }

        public void TryPLCReconnection()
        {
            Close();

            System.Threading.Thread.Sleep(3000);

            PlcInit();
        }
        #endregion

        #region private

        private void Sock_ConnectionStateChanged(CDriver driver, LGCNS.ezControl.Common.enumConnectionState connectionState)
        {
            if (connectionState == LGCNS.ezControl.Common.enumConnectionState.Connected)
            {
                OnConnect?.Invoke(driver.Name, true);
            }
            else if (connectionState == LGCNS.ezControl.Common.enumConnectionState.Disconnected)
            {
                OnConnect?.Invoke(driver.Name, false);
            }
        }

        #region communication callback
        List<byte> _bytelist;

        void Recv_sock_OnReceivedBytes(CSerialDriver driver, byte[] bytes)
        {
            // 드라이버 이름이 필요하면  driver.Name 확인 

            lock (_keylock)
            {
                if (EqpId.Length != 4)
                    return;

                try
                {
                    if (_bytelist == null)
                        _bytelist = new List<byte>();

                    _bytelist.AddRange(bytes);

                    bool retry = false;

                    do
                    {
                        retry = false;

                        int startPos = CUtil.PatternAt(_bytelist.ToArray(), _byteModule) - 1;
                     
                        if (startPos > 0)
                        {
                            _sbPacket = new List<byte>();

                            for (int i = startPos; i < _bytelist.Count; i++)
                            {
                                _sbPacket.Add(_bytelist[i]);

                                if (_sbPacket.Count == CommonHeader.GetHeaderSize())
                                {
                                    // parsing header. & 앞으로 받을 남은 길이 확인. 
                                    commonHeader = new CommonHeader();
                                    commonHeader.SetHeader(_sbPacket.ToArray());

                                    if (commonHeader.DataLength > 1000)
                                    {
                                        OnUpdateText?.Invoke("Driver Error : Packet Length Error");

                                        _bytelist.RemoveRange(0, i);
                                        retry = true;
                                        break;
                                    }
                                }
                                else if (_sbPacket.Count > CommonHeader.GetHeaderSize())
                                {
                                    //_sbPacket 의 카운터가 header 보다 크면 header 생성이 되었다고 보고.
                                    //총 길이가 header size + Data 길이 etx 만큼 계속 받음.
                                    if (_sbPacket.Count == commonHeader.DataLength + CommonHeader.GetHeaderSize() +1)
                                    {
                                        int index = _sbPacket.Count;
                                        byte check = _sbPacket[index - 1];
                                        if (check != (byte)CConstant._etx)
                                        {
                                            OnUpdateText?.Invoke("Error Packet: not exist ETX");
                                            OnUpdateMakeLogText?.Invoke(2, bytes);
                                        }
                                        else
                                        {
                                            byte[] dataPacket = new byte[commonHeader.DataLength];
                                            Array.Copy(_sbPacket.ToArray(), CommonHeader.GetHeaderSize(), dataPacket, 0, commonHeader.DataLength);
                                            List<short> listdata = CUtil.ByteToShortList(dataPacket);

                                            OnUpdateMakeLogText?.Invoke(1, _sbPacket.ToArray());

                                            if (OnRecievePacket != null)
                                            {
                                                try
                                                {
                                                    //OnRecievePacket(commonHeader, listdata);
                                                    OnRecievePacket.BeginInvoke(commonHeader, listdata, null, null);
                                                }
                                                catch (Exception ex)
                                                {
                                                    SystemLogger.Log(Level.Exception, ex.Message, "Driver");
                                                }
                                            }
                                        }
                                        _bytelist.RemoveRange(0, i);
                                        retry = true;
                                        break;
                                    }
                                }
                            }
                        }
                    } while (retry);
                }
                catch (Exception ex)
                {
                    SystemLogger.Log(Level.Exception, ex.Message, "Driver");
                }
            }
            //driver.Send("R");
        }

        #endregion

        #endregion

    }
}

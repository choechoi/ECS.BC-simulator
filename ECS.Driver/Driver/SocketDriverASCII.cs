using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LGCNS.ezControl.Core;
using LGCNS.ezControl.Driver.Serial;

using ECS.Common;
using LGCNS.ezControl.Diagnostics;
using System.Threading;

namespace ECS.Driver
{
    public class SocketDriverASCII
    {
        #region delegate
        public delegate void delegateRecieveMessage(String message);
        public delegate void delegateRecieveMessage2(String message);
        public delegate void delegateConnectionState(string key, bool connected);
        #endregion

        #region event
        public event delegateRecieveMessage OnRecieveMessage;
        public event delegateRecieveMessage2 OnRecieveMessage2;
        public event delegateConnectionState OnConnect;
        #endregion

        #region variable
        CSerialDriver receiveSock;
        CSerialDriver SendSock = null;
        public String _ip = null;
        public int receivePort = 0;
        public int sendPort = 0;
        private StringBuilder _sbPacket = new StringBuilder();
        private StringBuilder _sbPacket2 = new StringBuilder();
        public string _mouldeID = string.Empty;
        object _keylock = new object();
        object _keylock2 = new object();
        #endregion

        #region public
        public SocketDriverASCII(String module, String IP, int Port)
        {
            _mouldeID = module;

            receiveSock = new CSerialDriver();
            SendSock = new CSerialDriver();
            _ip = IP;
            receivePort = Port;
            sendPort = Port + 1;

            receiveSock.Name = module + "_" + receivePort.ToString();
            receiveSock.OnReceived += RecvreceiveSock_OnReceived;
            receiveSock.ConnectionStateChanged += Sock_ConnectionStateChanged;

            SendSock.Name = module + "_" + sendPort.ToString();
            SendSock.OnReceived += SendSock_OnReceived;
            SendSock.ConnectionStateChanged += Sock_ConnectionStateChanged;

            receiveSock.EnableLog(false, false);
        }

        void Sock_ConnectionStateChanged(CDriver driver, LGCNS.ezControl.Common.enumConnectionState connectionState)
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

        public void Init()
        {
            receiveSock.ConnectionInfoString = String.Format("MODE=TCP_ACTIVE, IP={0}, PORT={1}, KEEP_ALIVE=1", _ip, receivePort);
            receiveSock.ActiveOpen();
            SendSock.ConnectionInfoString = String.Format("MODE=TCP_ACTIVE, IP={0}, PORT={1}, KEEP_ALIVE=1", _ip, sendPort);
            SendSock.Encoder = Encoding.Default;
            SendSock.ActiveOpen();
        }

        public void InitServer()
        {
            receiveSock.Close();
            System.Threading.Thread.Sleep(100);
            receiveSock.ConnectionInfoString = String.Format("MODE=TCP_PASSIVE, PORT={0}", receivePort);
            receiveSock.Open();

            SendSock.Close();
            System.Threading.Thread.Sleep(100);
            SendSock.ConnectionInfoString = String.Format("MODE=TCP_PASSIVE, PORT={0}", sendPort);
            SendSock.Open();
        }

        public void InitSocket1()
        {
            SendSock.ConnectionInfoString = String.Format("MODE=TCP_ACTIVE, IP={0}, PORT={1}, KEEP_ALIVE=1", _ip, sendPort);
            SendSock.ActiveOpen();
        }

        public void EnableLog(bool bLog)
        {
            receiveSock.EnableLog(bLog, bLog);
            SendSock.EnableLog(bLog, bLog);
        }

        public void SendMessage(String message)
        {
            try
            {
                if (SendSock != null && SendSock.Connected)
                {
                    SendSock.Send(message);
                }
                else
                {
                    SystemLogger.Log(Level.Exception, _mouldeID + " Socket is disconnected.", "Driver");
                }
            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, ex.Message, SendSock.Name + "_Driver");
            }
        }

        public void Close()
        {
            receiveSock.Close();
            SendSock.Close();
        }
        #endregion

        #region private


        public void TryReconnection()
        {
            Close();
            System.Threading.Thread.Sleep(3000);
            Init();
        }

        #region communication callback

        void RecvreceiveSock_OnReceived(CSerialDriver driver, string strMessage)
        {
            if (!Monitor.TryEnter(_keylock, TimeSpan.FromSeconds(1)))
            {
                SystemLogger.Log(Level.Exception, "Monitor.TryEnter Timeout 1 second.", receiveSock.Name + "_Driver");
            }
            try
            {
                for (int i = 0; i < strMessage.Length; i++)
                {
                    switch (strMessage[i])
                    {
                        case (char)0x02:
                            _sbPacket = new StringBuilder();
                            break;

                        case (char)0x03:
                            OnRecieveMessage.BeginInvoke(_sbPacket.ToString(), null, null);
                            break;

                        default:
                            _sbPacket.Append(strMessage[i]);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, ex.Message, receiveSock.Name + "_Driver");
            }
            finally
            {
                Monitor.Exit(_keylock);
            }

        }

        private void SendSock_OnReceived(CSerialDriver driver, string strMessage)
        {
            if (!Monitor.TryEnter(_keylock2, TimeSpan.FromSeconds(1)))
            {
                SystemLogger.Log(Level.Exception, "Monitor.TryEnter Timeout 1 second.", SendSock.Name + "_Driver");
            }
            try
            {
                for (int i = 0; i < strMessage.Length; i++)
                {
                    switch (strMessage[i])
                    {
                        case (char)0x02:
                            _sbPacket2 = new StringBuilder();
                            break;

                        case (char)0x03:
                            OnRecieveMessage2.BeginInvoke(_sbPacket2.ToString(), null, null);
                            break;

                        default:
                            _sbPacket2.Append(strMessage[i]);
                            break;
                    }

                }
            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, ex.Message, SendSock.Name + "Driver");
            }
            finally
            {
                Monitor.Exit(_keylock2);
            }

        }

        #endregion
        #endregion

    }
}

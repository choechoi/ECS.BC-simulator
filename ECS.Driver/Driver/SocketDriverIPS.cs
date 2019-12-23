using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LGCNS.ezControl.Core;
using LGCNS.ezControl.Driver.Serial;

using LGCNS.ezControl.Diagnostics;

using ECS.Common;
using System.Threading.Tasks;
using System.Threading;

namespace ECS.Driver
{
    public class SocketDriverIPS
    {
        #region delegate
        public delegate void delegateUpdateText(String data);
        public delegate void delegateRecieveMessage(String message);
        public delegate void delegateRecieveSerialCommand(char command);

        public delegate void delegateConnectionState(string key, bool connected);

        public delegate void delegateUpdateMakeLogText(int direction, byte[] packet);
        #endregion

        #region event
        public event delegateUpdateText OnUpdateText;
        public event delegateRecieveMessage OnRecieveMessage;
        public event delegateRecieveSerialCommand OnReceiveSerialCommand;

        public event delegateConnectionState OnConnect;
        public event delegateUpdateMakeLogText OnUpdateMakeLogText;
        #endregion

        #region variable
        Dictionary<String, CSerialDriver> sock_dic = null;

        CSerialDriver _sock = null;
        public String _ip = null;
        public int _port = 0;
        private StringBuilder _sbPacket = new StringBuilder();
        System.Timers.Timer heartbeatTimer;
        public bool bHeartBeat = true;
        private int heartbeatCounter = 0;
        public string _mouldeID = string.Empty;
        object _keylock = new object();
        #endregion

        #region public
        public SocketDriverIPS(String module, String IP, int Port)
        {
            _sock = new CSerialDriver();
            _ip = IP;
            _port = Port;

            sock_dic = new Dictionary<string, CSerialDriver>();

            _sock.Name = module;
            _sock.OnReceived += Recv_sock_OnReceived;
            _sock.ConnectionStateChanged += Sock_ConnectionStateChanged;

            _mouldeID = module;

            heartbeatTimer = new System.Timers.Timer();
            heartbeatTimer.Interval = IpsTelegram.HEARTBEAT_INTERVAL;
            heartbeatTimer.Elapsed += new System.Timers.ElapsedEventHandler(HeartbeatTimer_Elapsed);

            _sock.EnableLog(false, false);

            sock_dic.Add(_sock.Name, _sock);
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
            Close();
            _sock.ConnectionInfoString = String.Format("MODE=TCP_ACTIVE, IP={0}, PORT={1}, KEEP_ALIVE=1", _ip, _port);
            _sock.ActiveOpen();

            //heartbeatTimer.Start();
        }

        public void InitServer()
        {
            Close();
            _sock.ConnectionInfoString = String.Format("MODE=TCP_PASSIVE, PORT={0}, KEEP_ALIVE=1", _port);
            _sock.Open();

            //heartbeatTimer.Start();
        }

        public void HeartBeatTimerEnabled(bool flag, int interval)
        {
            if (flag)
            {
                heartbeatTimer.Enabled = false;
                heartbeatTimer.Interval = interval;
                heartbeatTimer.Enabled = true;
            }
            else
            {
                heartbeatTimer.Enabled = false;
            }
        }

        public void EnableLog(bool bLog)
        {
            _sock.EnableLog(bLog, bLog);
        }

        public void SendData(byte[] buffer)
        {
            try
            {
                if (sock_dic[_mouldeID] != null)
                {
                    sock_dic[_mouldeID].Send(buffer);
                }
                else
                {
                    SystemLogger.Log(Level.Exception, _mouldeID + " socket is null.", "_Driver");
                }

            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, ex.Message);
            }
        }

        public void SetHeartBeatMode(bool bSet)
        {
            bHeartBeat = bSet;
        }

        public void SendMessage(String message)
        {
            if (_sock.Connected)
            {
                _sock.Send(message);
            }
        }

        public void Close()
        {
            _sock.Close();
            heartbeatCounter = 0;
        }
        #endregion

        #region private
        private void HeartbeatTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (bHeartBeat)
            {
                HeartBeat();
                if (heartbeatCounter > IpsTelegram.HEARTBEAT_TIMEOUT_COUNT)
                {
                    heartbeatCounter = 0;
                    TryReconnection();
                }

                heartbeatCounter++;
            }
        }

        public void TryReconnection()
        {
            heartbeatTimer.Stop();

            Close();
            System.Threading.Thread.Sleep(1000);
            Init();
        }

        public virtual void HeartBeat()
        {
            String data = string.Empty;

            IpsTelegram.IPSHeartBeat heart = new IpsTelegram.IPSHeartBeat();
            heart.ID = _mouldeID;

            SendMessage(heart.MakeMessage());
        }

        #region communication callback

        public void Recv_sock_OnReceived(CSerialDriver driver, string strMessage)
        {
            heartbeatCounter = 0;
            if (!Monitor.TryEnter(_keylock, TimeSpan.FromSeconds(1)))
            {
                SystemLogger.Log(Level.Exception, "Monitor.TryEnter Timeout 1 second.", _mouldeID + "_Driver");
            }
            try
            {
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
            }
            catch (Exception ex)
            {
                SystemLogger.Log(Level.Exception, ex.Message, _sock.Name + "_Driver");
            }
            finally
            {
                Monitor.Exit(_keylock);
            }
        }


        #endregion
        #endregion

    }
}

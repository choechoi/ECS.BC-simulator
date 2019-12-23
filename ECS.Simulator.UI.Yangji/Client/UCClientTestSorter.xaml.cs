using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Windows.Threading;
using System.Data;

using LGCNS.ezControl.Common;
using LGCNS.ezControl.Diagnostics;
using LGCNS.ezControl.Core;
using LGCNS.ezControl.Presentation;

using ECS.Common;
using Database.IF;

using System.Collections.Generic;
using System.Collections.ObjectModel;


namespace SMS.TEST.UI
{
    /// <summary>
    /// UCClientTestSorter.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UCClientTestSorter : UCFramePanel
    {
        #region Variable        
        CReference _reference = null;      

        DispatcherTimer connectionTimer = new DispatcherTimer();

        #endregion

        public UCClientTestSorter()
        {
            InitializeComponent();

            Init();

            ConnectElement();

            connectionTimer.Tick += ConnectionTimer_Tick;
            connectionTimer.Interval = TimeSpan.FromSeconds(10);
            connectionTimer.Start();

            tabControl.SelectionChanged += TabControl_SelectionChanged;

            radioListBoxEdit.EditValueChanged += RadioListBoxEdit_EditValueChanged;
            

        }
                
        void Init()
        {
            ModeList source = new ModeList();            
            radioListBoxEdit.ItemsSource = source;
            radioListBoxEdit.EditValue = 0;
           
        }

        private void RadioListBoxEdit_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
           switch(e.NewValue.ToString())
            {
                case "0":
                    ucConn.SendCommand(CConstant.EnumToCoreEvent.TestModeChange, 0);
                    break;
                case "1":
                    ucConn.SendCommand(CConstant.EnumToCoreEvent.TestModeChange, 1);
                    break;
                default:
                    break;
            }

        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int tabItem = ((sender as TabControl)).SelectedIndex;
            if (e.Source is TabControl) // This is a soultion of those problem.
            {
                switch (tabItem)
                {
                    case 0:
                        GetStatus(radioListBoxEdit.EditValue.ToString());
                        txtResult.Text = "Status 화면 입니다";
                        break;
                    case 1:
                        GetError();
                        txtResult.Text = "Error 목록 화면 입니다";
                        break;
                    case 2:
                        GetComplete(radioListBoxEdit.EditValue.ToString());
                        txtResult.Text = "결과 화면 입니다";
                        break;
                    default:                        
                        break;
                }
            }
        }

        #region Connect
        private void ConnectionTimer_Tick(object sender, EventArgs e)
        {
            if (ucConn.LinkedReference == null)
            {
                ConnectElement();
            }
            if (ucConn.LinkedReference != null && ucConn.LinkedReference.ReferenceState != enumReferenceState.Active)
            {
                ConnectElement();
            }
        }

        private void ConnectElement()
        {
            ucConn.ElementNo = Convert.ToInt32(CUtil.GetConfigValue("REFERENCE_ELEMENTS"));
            ucConn.OnPanelUIEvent += uCCommunication_OnPanelUIEvent;

        }

        private void DisconnectElement()
        {
            if (ucConn.LinkedReference != null)
            {
                ucConn.LinkedReference.Stop();
                ucConn.OnPanelUIEvent -= uCCommunication_OnPanelUIEvent;
                ucConn.LinkedReference = null;
            }
        }

        #endregion

        #region Core => UCConnection  UI => OnPanelUIEvent

        void uCCommunication_OnPanelUIEvent(int iEventID, params object[] args)
        {
            try
            {
                switch ((ClientConstant.EnumToUIEvent)iEventID)
                {
                    case ClientConstant.EnumToUIEvent.Connected:
                        {
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                            {
                                SetConnectState(true);
                            }));

                            //Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                            //{
                            //    SetConnectState(true);
                            //}));
                        }
                        break;

                    case ClientConstant.EnumToUIEvent.Disconnected:
                        {
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                            {
                                SetConnectState(false);
                            }));
                        }
                        break;

                    case ClientConstant.EnumToUIEvent.kioskRead:
                        {
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                            {
                                GetStatus(args[0].ToString());
                            }));
                        }
                        break;

                    case ClientConstant.EnumToUIEvent.Inducted:
                        {
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                            {
                                GetStatus(args[0].ToString());
                            }));
                        }
                        break;

                    case ClientConstant.EnumToUIEvent.IPSRead:
                        {
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                            {
                                GetStatus(args[0].ToString());
                            }));
                        }
                        break;

                    case ClientConstant.EnumToUIEvent.Discharged:
                        {
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                            {
                                GetStatus(args[0].ToString());
                            }));
                        }
                        break;

                    case ClientConstant.EnumToUIEvent.SortedConfirm:
                        {
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                            {
                                GetStatus(args[0].ToString());
                            }));
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        #endregion

        #region Method
        void SetConnectState(bool state)
        {
            if (state)
            {
                lblConnSts.Content = "Connected";                
                lblConnSts.FontStyle = FontStyles.Normal;
                lblDate.Content = DateTime.Now.ToString("yyyy-MM-dd ") + DateTime.Now.ToShortTimeString();
                bdConnSts.Background = new SolidColorBrush(Color.FromRgb(102, 209, 27));
                
                //txtStauts.Text = "Connected";
                //txtStauts.Foreground = connected;
                //txtStauts.Background = new SolidColorBrush(Colors.Lime);
            }
            else
            {
                lblConnSts.Content = "Disconnected";
                //lblConnSts.Background = new SolidColorBrush(Colors.Gray);
                lblConnSts.FontStyle = FontStyles.Italic;
                bdConnSts.Background = new SolidColorBrush(Color.FromRgb(255, 1, 59));
                btnStart.Background = new SolidColorBrush(Color.FromRgb(102, 209, 27));
                //txtStauts.Text = "Disconnected";
                //txtStauts.Foreground = disconnected;
                //txtStauts.Background = new SolidColorBrush(Colors.Gray);
            }
        }

        void GetStatus(string batch_seq)
        {
            DataSet dsStatus = null;

            dsStatus = DBInterface.SP_TEST_GET_STATUS("YJ", "YS02", batch_seq);

            DataTable dtStatus = dsStatus.Tables[0];
            DataTable dtKiosk = dsStatus.Tables[1];
            DataTable dtInducted = dsStatus.Tables[2];
            DataTable dtIPS = dsStatus.Tables[3];
            DataTable dtDischarged = dsStatus.Tables[4];
            DataTable dtSortedCFM = dsStatus.Tables[5];

            DataTable table1 = new DataTable("Items");

            dtStatus.PrimaryKey = new DataColumn[] { dtStatus.Columns[0] };

            dtStatus.Merge(dtKiosk, false, MissingSchemaAction.Add);
            dtStatus.Merge(dtInducted, false, MissingSchemaAction.Add);
            dtStatus.Merge(dtIPS, false, MissingSchemaAction.Add);
            dtStatus.Merge(dtDischarged, false, MissingSchemaAction.Add);
            dtStatus.Merge(dtSortedCFM, false, MissingSchemaAction.Add);

            dxgStatus.ItemsSource = dtStatus.DefaultView;

        }

        void GetError()
        {
            DataSet dataSet = null;

            dataSet = DBInterface.SP_TEST_GET_ERROR("YJ", "YS02", "1");

            dxgErrResult.ItemsSource = dataSet.Tables[0].DefaultView;
        }

        void GetComplete(string batch_seq)
        {
            DataSet dataSet = null;

            dataSet = DBInterface.SP_TEST_GET_SORTED_CFM(batch_seq); //Batch ID : 1

            dxgResult.ItemsSource = dataSet.Tables[0].DefaultView;
        }
        #endregion


        #region Button Event
        #region Main Button

        private void btnSocket_Click(object sender, RoutedEventArgs e)
        {
            if (ucConn.LinkedReference != null)
            {
                ucConn.SendCommand(CConstant.EnumToCoreEvent.TestSocketClose, null);
            }
            else
            {
                MessageBox.Show("서버와 연결되지 않았거나, 테스트 데이터가 없습니다.");
            }

        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            if (ucConn.LinkedReference != null)
            {
                ucConn.SendCommand(CConstant.EnumToCoreEvent.CommandData, 15);
            }
            else
            {
                MessageBox.Show("서버와 연결되지 않았거나, 테스트 데이터가 없습니다.");
            }

        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (ucConn.LinkedReference != null)
            {
                ucConn.SendCommand(CConstant.EnumToCoreEvent.CommandData, 3);
            }
            else
            {
                MessageBox.Show("서버와 연결되지 않았거나, 테스트 데이터가 없습니다.");
            }

        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            if (ucConn.LinkedReference != null)
            {
                ucConn.SendCommand(CConstant.EnumToCoreEvent.CommandData, 4);
            }
            else
            {
                MessageBox.Show("서버와 연결되지 않았거나, 테스트 데이터가 없습니다.");
            }
        }

        private void btnBatch_Click(object sender, RoutedEventArgs e)
        {
            string[] retVal = new string[2];

            retVal = DBInterface.SP_TEST_MAKE_BATCH();

            txtResult.Text = retVal[1];

            dxgStatus.ItemsSource = null;
            dxgErrResult.ItemsSource = null;
            dxgResult.ItemsSource = null;
        }

        #endregion

        

        #region Error Button
        private void btnErrRetrieve_Click(object sender, RoutedEventArgs e)
        {
            GetError();
        }
        #endregion

        #region Result Button
        private void btnRetrieve_Click(object sender, RoutedEventArgs e)
        {
            GetComplete(radioListBoxEdit.EditValue.ToString());
        }
        #endregion



        #endregion

        
    }

    #region RadioButton
    public class Mode
    {
        public string ModeName { get; set; }
        public int ModeCode { get; set; }
    }

    public class ModeList : ObservableCollection<Mode>
    {
        public ModeList()
            : base()
        {
            Add(new Mode() { ModeName = "Test Mode", ModeCode = 0 });
            Add(new Mode() { ModeName = "Batch Mode", ModeCode = 1 });
        }
    }
        
    #endregion
}

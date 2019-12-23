using Database.IF;
using ECS.Common;
using LGCNS.ezControl.Core;
using LGCNS.ezControl.Presentation;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using ECS.Simulator;
using System.Threading.Tasks;

namespace ECS.Simulator.UI
{
    /// <summary>
    /// UCClientTest3FCV.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UCSimulSorter : UCFramePanel
    {
        DispatcherTimer timer = new DispatcherTimer();

        DataTable dtChute = new DataTable();
        DataTable dtInducted;
        DataTable dtPlan;

        CReference _reference = null;
        CReference _reference2 = null;

        protected DBInterface dbinterface;

        int connectElementNo = 0;
        string selectedEQPID = string.Empty;
        // 테스트에 사용될 정보들
        List<SimulCase> Cases;
        List<SimulCaseStatus> CasesStatus;
        List<ErrorLog> errList = new List<ErrorLog>();
        List<SimulCase> newCases = new List<SimulCase>();
        string curTime = string.Empty;
        // 플래그
        bool isSearched = false;

        public UCSimulSorter()
        {
            InitializeComponent();

            Unloaded += UCSimulSorter_Unloaded;

            dbinterface = new DBInterface();

            dtPlan = new DataTable();
            // DataTable에 Column 추가

            ConnectElement();
            //ConnectElement2();
            this.dgData.MouseLeftButtonUp += dgDataGrid_PreviewMouseLeftButtonUp;
        }

        private void UCSimulSorter_Unloaded(object sender, RoutedEventArgs e)
        {
            if (ucConn1.LinkedReference != null)
            {
                DisconnectElement();
            }
        }

        #region Connect
        private void ConnectElement()
        {
            try
            {
                ucConn1.ElementNo = 5;
                _reference = CReferenceManager.GetReference("Simulator", 5);
                ucConn1.OnPanelUIEvent += uCCommunication_OnPanelUIEvent;
                ucConn1.LinkedReference = _reference;

                if (_reference != null)
                    ucConn1.LinkedReference.Start();
            }
            catch { throw; }
        }

        private void ConnectElement2()
        {
            ucConn2.ElementNo = 126;
            _reference2 = CReferenceManager.GetReference("IPS", 126);
            ucConn2.OnPanelUIEvent += uCCommunication2_OnPanelUIEvent;
            ucConn2.LinkedReference = _reference2;
            ucConn2.LinkedReference.Start();
        }

        private void DisconnectElement()
        {
            if (ucConn1.LinkedReference != null)
            {
                ucConn1.LinkedReference.Stop();
                ucConn1.OnPanelUIEvent -= uCCommunication_OnPanelUIEvent;
                ucConn1.LinkedReference = null;
            }

            if (ucConn2.LinkedReference != null)
            {
                ucConn2.LinkedReference.Stop();
                ucConn2.OnPanelUIEvent -= uCCommunication2_OnPanelUIEvent;
                ucConn2.LinkedReference = null;
            }
        }

        #endregion

        #region Core => ucConn1ection  UI => OnPanelUIEvent

        void uCCommunication_OnPanelUIEvent(int iEventID, params object[] args)
        {
            try
            {
                switch ((CEnum2.EnumToUIEvent)iEventID)
                {
                    case CEnum2.EnumToUIEvent.Connected:
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

                    case CEnum2.EnumToUIEvent.Disconnected:
                        {
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                            {
                                SetConnectState(false);
                            }));
                        }
                        break;


                    case CEnum2.EnumToUIEvent.IPSRead:
                        {
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                            {
                                GetStatus();
                            }));
                        }
                        break;


                    case CEnum2.EnumToUIEvent.SortedConfirm:
                        {
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                            {
                                GetStatus();
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

        void uCCommunication2_OnPanelUIEvent(int iEventID, params object[] args)
        {
            try
            {
                switch ((CEnum2.EnumToUIEvent)iEventID)
                {
                    case CEnum2.EnumToUIEvent.Connected:
                        {
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                            {
                                SetConnectState2(true);
                            }));

                            //Dispatcher.CurrentDispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                            //{
                            //    SetConnectState(true);
                            //}));
                        }
                        break;

                    case CEnum2.EnumToUIEvent.Disconnected:
                        {
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                            {
                                SetConnectState2(false);
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

        #region Button Event
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (ucConn1.LinkedReference != null)
            {
                //ucConn1.SendCommand(CEnum2.EnumToCoreEventForSimulator.TestDAS, txtWeight.Text, txtBarcode.Text, txtRange.Text);
            }
            else
            {
                MessageBox.Show("서버와 연결되지 않았거나, 테스트 데이터가 없습니다.");
            }
        }
        #region Status Button
        private void btnDataReset_Click(object sender, RoutedEventArgs e)
        {
            #region 기존 로직
            //List<OracleParameter> listPara2 = new List<OracleParameter>
            //{
            //    //new OracleParameter("P_EQP_ID", OracleDbType.Varchar2, txtEquip.Text, ParameterDirection.Input),
            //    //new OracleParameter("P_BTCH_SEQ", OracleDbType.Varchar2, txtBatch.Text, ParameterDirection.Input),               
            //    new OracleParameter("O_RTN_CD", OracleDbType.Varchar2, 32767, string.Empty, ParameterDirection.Output),
            //    new OracleParameter("O_RTN_MSG", OracleDbType.Varchar2, 32767, string.Empty, ParameterDirection.Output),
            //};

            //string[] outpara = new string[2] { "O_RTN_CD", "O_RTN_MSG" };

            //string[] retval = dbinterface.WcsProcedureCallGetValue("SP_TEST_MAKE_PCS_DATA", listPara2, outpara);

            //txtResult.Text = retval[0] + " : " + retval[1];

            //Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            //{
            //    GetStatus();
            //}));
            #endregion
            try
            {
                bool isRtnValue = dbinterface.resetSimulCase();
            }
            catch(Exception ex)
            {
                getLog(ex.ToString(), MethodBase.GetCurrentMethod().Name);
            }
        }

        private void btnStartTest_Click(object sender, RoutedEventArgs e)
        {
            //ucConn1.SendCommand(CEnum2.EnumToCoreEventForSimulator.TestSorter);
            // 로직

            curTime = DateTime.Now.ToString("yyyyMMddHHmmss");
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                GetStatus();
            }));

        }

        #endregion

        #endregion

        #region Add Button
        private void btnDataAdd_Click(object sender, RoutedEventArgs e)
        {
           
        }
        #endregion

        #region Result Button
        private void btnRsltRetrieve_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                getResult();
            }));
        }
        #endregion

        #region Search Button
        private void btnRsltSearch_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                getData();
            }));
            isSearched = true;
        }
        #endregion

        #region Update Button
        private void btnRsltUpdate_Click(object sender, RoutedEventArgs e)
        {
            if(isSearched == false)
            {
                MessageBox.Show("수정한 데이터가 없습니다.");
                return;
            }

            MessageBoxResult result = MessageBox.Show("수정을 반영하시겠습니까?", "Confirmation", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // 기존의 시뮬레이터 케이스
                    foreach (SimulCase c in Cases)
                    {
                        if (c.IS_UPDATED == true)
                        {
                            // 바인드변수  쿼리
                            dbinterface.UpdateSimulCases(c.SID,  c.BCD_INFO);
                        }
                    }
                    // 재조회
                    getData();
                }
                catch(Exception ex)
                {
                    getLog(ex.ToString(), MethodBase.GetCurrentMethod().Name);
                }
            }
            else
            {
                return;
            }
            MessageBox.Show("수정이 반영되었습니다.");
            return;
        }
        #endregion

        #region Error Button
        private void btnErrRetrieve_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                getError();
            }));
        }
        #endregion        

        #region IPS Button
        private void btnIPS_Click(object sender, RoutedEventArgs e)
        {
            if (ucConn1.LinkedReference != null)
            {
                ucConn1.SendCommand(CEnum2.EnumToCoreEventForSimulator.TestIPSData, txtBcr1.Text);
                MessageBox.Show("ECS에 전송하였습니다.");
            }
            else
            {
                MessageBox.Show("서버와 연결되지 않았거나, 테스트 데이터가 없습니다.");
            }
        }

        private void btnSortedConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (ucConn1.LinkedReference != null)
            {
                ucConn1.SendCommand(CEnum2.EnumToCoreEventForSimulator.TestSortedConfirm);
            }
            else
            {
                MessageBox.Show("서버와 연결되지 않았거나, 테스트 데이터가 없습니다.");
            }
        }
        #endregion

        #region IPS All Button
        private void btnAllScan_Click(object sender, RoutedEventArgs e)
        {
            if (ucConn2.LinkedReference != null)
            {
                int i = 0;
                foreach (DataRow dr in dtInducted.Rows)
                {
                    string bcrData = dtPlan.Rows[i][1].ToString() + ";" + dtPlan.Rows[i][2].ToString();
                    ucConn2.SendCommand(CEnum2.EnumToCoreEventForSimulator.TestIPSData, dr["CART_NO"].ToString(), dr["PID"].ToString(), bcrData);
                    i++;
                }
            }
            else
            {
                MessageBox.Show("서버와 연결되지 않았거나, 테스트 데이터가 없습니다.");
            }
        }
        #endregion
        void SetConnectState(bool state)
        {
            if (state)
            {
                lblConnSts.Content = "Connected  ";
                lblConnSts.FontStyle = FontStyles.Normal;
                bdConnSts.Background = new SolidColorBrush(Color.FromRgb(102, 209, 27));

                //txtStauts.Text = "Connected";
                //txtStauts.Foreground = connected;
                //txtStauts.Background = new SolidColorBrush(Colors.Lime);
            }
            else
            {
                lblConnSts.Content = "Disconnected  ";
                //lblConnSts.Background = new SolidColorBrush(Colors.Gray);
                lblConnSts.FontStyle = FontStyles.Italic;
                bdConnSts.Background = new SolidColorBrush(Color.FromRgb(255, 1, 59));
                //txtStauts.Text = "Disconnected";
                //txtStauts.Foreground = disconnected;
                //txtStauts.Background = new SolidColorBrush(Colors.Gray);
            }
        }

        void SetConnectState2(bool state)
        {
            if (state)
            {
                lblConnIPS.Content = "IPS Connected  ";
                lblConnIPS.FontStyle = FontStyles.Normal;
                bdConnIPS.Background = new SolidColorBrush(Color.FromRgb(102, 209, 27));
            }
            else
            {
                lblConnIPS.Content = "IPS Disconnected  ";
                lblConnIPS.FontStyle = FontStyles.Italic;
                bdConnIPS.Background = new SolidColorBrush(Color.FromRgb(255, 1, 59));
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
                        txtResult.Text = "시뮬레이션 바코드 데이터 화면 입니다";
                        break;
                    case 1:
                        //getResult();
                        txtResult.Text = "결과 화면 입니다";
                        break;
                    case 2:
                        getError();
                        txtResult.Text = "Error 기록 화면 입니다";
                        break;
                    case 3:
                        txtResult.Text = "시스템 로그 기록 화면 입니다";
                        break;
                    default:
                        break;
                }
            }
        }

        #region Method           
        void GetStatus()
        {
           // DataSet dsStatus = null;
            if(ucConn1.LinkedReference != null)
            {
               //Task task = new Task(async () =>
               //{
                   // 쓰레드로 실행 한다.
                   foreach (SimulCase c in Cases)
                   {
                       try
                       {
                           // sleep 추가   
                           ucConn1.SendCommand(CEnum2.EnumToCoreEventForSimulator.TestIPSData, c.BCD_INFO);
                 //          await Task.Delay(500);
                           
                       }
                       catch (Exception e)
                       {
                           getLog(e.ToString(), MethodBase.GetCurrentMethod().Name);
                       }
                   }

                //});
                //task.Start();
                getStatusData();
            }
            else
            {
                MessageBox.Show("서버와 연결되지 않았거나, 테스트 데이터가 없습니다.");
            }
            //DataTable dtStatus = dsStatus.Tables[0];
            //dtInducted = dsStatus.Tables[2];
            //DataTable dtIPS = dsStatus.Tables[3];
            //DataTable dtDischarged = dsStatus.Tables[4];
            //DataTable dtSortedCFM = dsStatus.Tables[5];

            //DataTable table1 = new DataTable("Items");

            //dtStatus.PrimaryKey = new DataColumn[] { dtStatus.Columns[0] };

            //dtStatus.Merge(dtInducted, false, MissingSchemaAction.Add);
            //dtStatus.Merge(dtIPS, false, MissingSchemaAction.Add);
            //dtStatus.Merge(dtDischarged, false, MissingSchemaAction.Add);
            //dtStatus.Merge(dtSortedCFM, false, MissingSchemaAction.Add);

            //dgStatus.ItemsSource = dtStatus.DefaultView;
          
            // 재조회
        }
 
        void getData()
        {
            try
            {
                DataSet dsResult = dbinterface.GetSimulCases();
                Cases = new List<SimulCase>();
                for (int i = 0; i < dsResult.Tables[0].Rows.Count; i++)
                {
                    Cases.Add(new SimulCase()
                    {
                        SID = Convert.ToInt32(dsResult.Tables[0].Rows[i][1])
                       ,BCD_INFO = dsResult.Tables[0].Rows[i][0].ToString()
                    });
                }
                this.dgData.ItemsSource = Cases;
                this.dgData.Columns[0].Visibility = Visibility.Hidden;
                this.dgData.Columns[2].Visibility = Visibility.Hidden;
            }
            catch (Exception e)
            {
                getLog(e.ToString(), MethodBase.GetCurrentMethod().Name);
            }
        }

        void getStatusData()
        {
            try
            {
                //pid, plan_cd, box_bcd, rgn_bcd, bcd_info, plan_chute_id1, plan_chute_id2, plan_chute_id3, scan_dt, rslt_chute_id, srt_wrk_stat_cd, srt_wrk_cmpt_dt, srt_err_cd, srt_rsn_cd
                DataSet dsResult = dbinterface.GetSimulCaseStatus(curTime);
                CasesStatus = new List<SimulCaseStatus>();
                for (int i = 0; i < dsResult.Tables[0].Rows.Count; i++)
                {
                    CasesStatus.Add(new SimulCaseStatus()
                    {
                        PID = dsResult.Tables[0].Rows[i][0].ToString()
                       ,
                        PLAN_CD = dsResult.Tables[0].Rows[i][1].ToString()
                       ,
                        BOX_BCD = dsResult.Tables[0].Rows[i][2].ToString()
                       ,
                        RGN_BCD = dsResult.Tables[0].Rows[i][3].ToString()
                       ,
                        BCD_INFO = dsResult.Tables[0].Rows[i][4].ToString()
                       ,
                        PLAN_CHUTE_ID1 = dsResult.Tables[0].Rows[i][5].ToString()
                       ,
                        PLAN_CHUTE_ID2 = dsResult.Tables[0].Rows[i][6].ToString()
                       ,
                        PLAN_CHUTE_ID3 = dsResult.Tables[0].Rows[i][7].ToString()
                       ,
                        SCAN_DT = dsResult.Tables[0].Rows[i][8].ToString()
                       ,
                        RSLT_CHUTE_ID = dsResult.Tables[0].Rows[i][9].ToString()
                       ,
                        SRT_WRK_STAT_CD = dsResult.Tables[0].Rows[i][10].ToString()
                       ,
                        SRT_WRK_CMPT_DT = dsResult.Tables[0].Rows[i][11].ToString()
                       ,
                        SRT_ERR_CD = dsResult.Tables[0].Rows[i][12].ToString()
                       ,
                        SRT_RSN_CD = dsResult.Tables[0].Rows[i][13].ToString()
                    });
                }
                this.dgStatus.ItemsSource = CasesStatus;
                for (int i = 0; i < dgStatus.Columns.Count; i++)
                {
                    this.dgStatus.Columns[i].IsReadOnly = true;
                }
            }
            catch(Exception e)
            {
                getLog(e.ToString(), MethodBase.GetCurrentMethod().Name);
            }
        }

        void getResult()
        {
            try
            {
                if (this.txtBcr1.Text != string.Empty)
                {
                    DataSet dsResult = dbinterface.GetRecentDestination(this.txtBcr1.Text);
                    List<SimulCaseStatus> Results = new List<SimulCaseStatus>();

                    for (int i = 0; i < dsResult.Tables[0].Rows.Count; i++)
                    {
                        Results.Add(new SimulCaseStatus()
                        {
                            PID = dsResult.Tables[0].Rows[i][0].ToString()
                       ,
                            PLAN_CD = dsResult.Tables[0].Rows[i][1].ToString()
                       ,
                            BOX_BCD = dsResult.Tables[0].Rows[i][2].ToString()
                       ,
                            RGN_BCD = dsResult.Tables[0].Rows[i][3].ToString()
                       ,
                            BCD_INFO = dsResult.Tables[0].Rows[i][4].ToString()
                       ,
                            PLAN_CHUTE_ID1 = dsResult.Tables[0].Rows[i][5].ToString()
                       ,
                            PLAN_CHUTE_ID2 = dsResult.Tables[0].Rows[i][6].ToString()
                       ,
                            PLAN_CHUTE_ID3 = dsResult.Tables[0].Rows[i][7].ToString()
                       ,
                            SCAN_DT = dsResult.Tables[0].Rows[i][8].ToString()
                       ,
                            RSLT_CHUTE_ID = dsResult.Tables[0].Rows[i][9].ToString()
                       ,
                            SRT_WRK_STAT_CD = dsResult.Tables[0].Rows[i][10].ToString()
                       ,
                            SRT_WRK_CMPT_DT = dsResult.Tables[0].Rows[i][11].ToString()
                       ,
                            SRT_ERR_CD = dsResult.Tables[0].Rows[i][12].ToString()
                       ,
                            SRT_RSN_CD = dsResult.Tables[0].Rows[i][13].ToString()
                        });
                    }
                    this.dgResult.ItemsSource = Results;
                }
            }
            catch(Exception e)
            {
                getLog(e.ToString(), MethodBase.GetCurrentMethod().Name);
            }
            #region 기존 로직
            //try
            //{
            //    dataset dsstatus = null;

            //    list<oracleparameter> listpara = new list<oracleparameter>();
            //    listpara.add(new oracleparameter("p_center_cd", oracledbtype.varchar2, "yj", parameterdirection.input));
            //    //listpara.add(new oracleparameter("p_eqp_id", oracledbtype.varchar2, txtequip.text, parameterdirection.input));
            //    //listpara.add(new oracleparameter("p_btch_seq", oracledbtype.varchar2, txtbatch.text, parameterdirection.input));
            //    listpara.add(new oracleparameter("t_cursor", oracledbtype.refcursor, parameterdirection.output));

            //    dsstatus = dbinterface.wcsprocedurecallgetdataset("sp_test_get_sorter_rslt", listpara);

            //    dgresult.itemssource = dsstatus.tables[0].defaultview;
            //}
            //catch (exception ex)
            //{
            //    txtresult.text = ex.tostring();
            //}
            #endregion
        }

        void getError()
        {
            try
            {
                DataSet dsStatus = null;
                List<OracleParameter> listPara = new List<OracleParameter>();
                listPara.Add(new OracleParameter("P_CENTER_CD", OracleDbType.Varchar2, "YJ", ParameterDirection.Input));
                //listPara.Add(new OracleParameter("P_EQP_ID", OracleDbType.Varchar2, txtEquip.Text, ParameterDirection.Input));
                //listPara.Add(new OracleParameter("P_BTCH_SEQ", OracleDbType.Varchar2, txtBatch.Text, ParameterDirection.Input));
                listPara.Add(new OracleParameter("T_CURSOR", OracleDbType.RefCursor, ParameterDirection.Output));

                dsStatus = dbinterface.WcsProcedureCallGetDataSet("SP_TEST_GET_SORTER_ERROR", listPara);
                dgError.ItemsSource = dsStatus.Tables[0].DefaultView;
            }
            catch (Exception ex)
            {
                txtResult.Text = ex.ToString();
            }
        }
        void getLog(string err_msg, string err_come_from)
        {
            errList.Add(new ErrorLog
            {
                ERR_MSG = err_msg
                , ERR_COME_FROM = err_come_from
                , ERR_TIME = DateTime.Now
            });
         
            this.dgError.ItemsSource = errList;
        }
        #endregion

        #region 이벤트
        private void dgDataGrid_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SimulCase sc = this.dgData.CurrentItem as SimulCase;
            if(sc != null)
            {
                sc.IS_UPDATED = true;
            }
        }
        #endregion

        private void DgError_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
    // IPS SCAN 이후 결과 
    class Result
    {
        public string WRK_NO { get; set; }
        public string PID { get; set; }
        public string BCD_INFO { get; set; }
        public string BOX_BCD { get; set; }
        public string RGN_BCD { get; set; }
        public string UPD_DT { get; set; }
    }

    class SimulCase
    {
        public int SID { get; set; }
        public string BCD_INFO { get; set; }
        public bool IS_UPDATED { get; set; }
    }

    //pid, plan_cd, box_bcd, rgn_bcd, bcd_info, plan_chute_id1, plan_chute_id2, plan_chute_id3, scan_dt, rslt_chute_id, srt_wrk_stat_cd, srt_wrk_cmpt_dt, srt_err_cd, srt_rsn_cd
    class SimulCaseStatus
    {
        public string PID { get; set; }
        public string PLAN_CD { get; set; }

        public string BOX_BCD { get; set; }
        public string RGN_BCD { get; set; }
        public string BCD_INFO { get; set; }
        public string PLAN_CHUTE_ID1 { get; set; }
        public string PLAN_CHUTE_ID2 { get; set; }
        public string PLAN_CHUTE_ID3 { get; set; }
        public string SCAN_DT { get; set; }
        public string RSLT_CHUTE_ID { get; set; }
        public string SRT_WRK_STAT_CD { get; set; }
        public string SRT_WRK_CMPT_DT { get; set; }
        public string SRT_ERR_CD { get; set; }
        public string SRT_RSN_CD { get; set; }
    } 
    class ErrorLog
    {
        public string ERR_MSG { get; set; }
        public string ERR_COME_FROM { get; set; }
        public DateTime ERR_TIME { get; set; }
    }
}

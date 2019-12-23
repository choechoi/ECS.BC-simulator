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

using System.Collections.Concurrent;

using LGCNS.ezControl.Core;
using LGCNS.ezControl.Presentation;

namespace ECS.Simulator.UI
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        #region event
        public event delegateFrameServiceUserEvent OnUserEvent;        
        #endregion

        #region variable
        private ConcurrentDictionary<int, UCFramePanel> _DicClinet;
        private ConcurrentDictionary<int, UCFrameBase> _DicSubNavigation;
        #endregion
        public MainWindow()
        {
            InitializeComponent();

            //Unloaded += MainWindow_Unloaded;
            //Loaded += MainWindow_Loaded;

            _DicClinet = new ConcurrentDictionary<int, UCFramePanel>();
            _DicSubNavigation = new ConcurrentDictionary<int, UCFrameBase>();

            //RegisterTitlePanel(new UCUITitle());
            //RegisterSubNavigationPanel(new UCClientMenu(), 0, true);

            uCBaseWindowGrid.Loaded += new RoutedEventHandler(uCBaseWindowGrid_Loaded);
            uCBaseWindowGrid.Unloaded += new RoutedEventHandler(uCBaseWindowGrid_Unloaded);
            //uCBaseWindowGrid.OnUserEvent += UCBaseWindowGrid_OnUserEvent;

            uCBaseWindowGrid.Initialize();

        }

        private void ClearClientFrame(string panelNo)
        {
            uCBaseWindowGrid.ClearClient();

            switch (panelNo)
            {                
                case "2":
                    //uCBaseWindowGrid.RegisterClientPanel(new UCSimul1FRtnWH(), 2, true);
                    break;
                case "3":
                    //uCBaseWindowGrid.RegisterClientPanel(new SPCS403(), 3, true);
                    break;
                case "4":
                    //uCBaseWindowGrid.RegisterClientPanel(new SPCS404(), 4, true);
                    break;
                case "5":
                    //uCBaseWindowGrid.RegisterClientPanel(new SPCS405(), 5, true);
                    break;
                case "6":
                    //uCBaseWindowGrid.RegisterClientPanel(new SPCS406(), 6, true);
                    break;                
                case "13":
                    uCBaseWindowGrid.RegisterClientPanel(new UCSimulSorter(), 13, true);
                    break;
                default:
                    uCBaseWindowGrid.RegisterClientPanel(new UCSimulSorter(), 13, true);
                    break;
            }


        }

        void uCBaseWindowGrid_Unloaded(object sender, RoutedEventArgs e)
        {
            CReferenceManager.StopReferences();
        }

        void uCBaseWindowGrid_Loaded(object sender, RoutedEventArgs e)
        {
            RegisterTitlePanel(new UCUITitle());
            RegisterSubNavigationPanel(new UCClientMenu(), 0, true);
            // uCBaseWindowGrid.RegisterClientPanel(new UCClientTestSorter(), 0, false);
            uCBaseWindowGrid.RegisterClientPanel(new UCSimulSorter(), 13, true);

            CReferenceManager.StartReferences();

        }


        //private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        //{
        //    RegisterTitlePanel(new UCUITitle());
        //    RegisterSubNavigationPanel(new UCClientMenu(), 0, true);

        //    //Sorter
        //    RegisterClientPanel(new UCSimulSorter(), 0, false);

        //    //2F
        //    //RegisterClientPanel(new UCSimul2FPre(), 8, false);
        //    //RegisterClientPanel(new UCSimulTallyGI(), 9, false);
        //    //RegisterClientPanel(new UCSimulMVFloor(), 10, false);


        //    ////3~4F
        //    //RegisterClientPanel(new UCSimulWH(), 11, false);
        //    //RegisterClientPanel(new UCSimulDAS(), 12, false);



        //    CReferenceManager.StartReferences();
        //}

        //private void MainWindow_Unloaded(object sender, RoutedEventArgs e)
        //{
        //    CReferenceManager.StopReferences();
        //}

        ////void uCBaseWindowGrid_Unloaded(object sender, RoutedEventArgs e)
        ////{
        ////    CReferenceManager.StopReferences();
        ////}

        ////void uCBaseWindowGrid_Loaded(object sender, RoutedEventArgs e)
        ////{

        ////    //uCBaseWindowGrid.RegisterTitlePanel(new UCUITitle());
        ////    //uCBaseWindowGrid.RegisterSubNavigationPanel(new UCClientMenu(), 0, true);
        ////    //// uCBaseWindowGrid.RegisterClientPanel(new UCClientTestSorter(), 0, false);

        ////    //uCBaseWindowGrid.RegisterClientPanel(new UCSimulWH(), 0, true);
        ////    //uCBaseWindowGrid.RegisterClientPanel(new UCSimulSorter(), 1, false);

        ////    CReferenceManager.StartReferences();

        ////}

        public void RegisterTitlePanel(UCFrameBase panel)
        {
            ucTitle.Child = panel;
            panel.OnRequestParentService += OnRequestParentService;
        }

        public void RegisterSubNavigationPanel(UCFrameBase panel, int iKey, bool bShow)
        {
            try
            {
                bool ret = _DicSubNavigation.TryAdd(iKey, panel);

                if (ret)
                {
                    panel.OnRequestParentService += OnRequestParentService;
                    if (bShow)
                    {
                        ucSubNavigation.Child = panel;
                    }
                }
            }
            catch (Exception ex)
            {
                string.Format("[{0}]{1} : {2}", ex.Source, ex.StackTrace, ex.Message);

            }


        }

        //public void RegisterClientPanel(UCFramePanel panel, int iKey, bool bShow)
        //{
        //    bool ret = _DicClinet.TryAdd(iKey, panel);

        //    if (ret)
        //    {
        //        panel.OnRequestParentService += OnRequestParentService;
        //        if (bShow)
        //        {
        //            ucClient.Child = panel;
        //        }
        //    }
        //}

        //#region private
        //void ChangeClient(int key)
        //{
        //    UCFramePanel current = (UCFramePanel)ucClient.Child;            

        //    UCFramePanel panel = _DicClinet[key];
        //    if (panel != null)
        //    {
        //        ucClient.Child = panel;
        //    }

        //}

        void OnRequestParentService(object sender, enumFrameService enService, params object[] args)
        {
            switch (enService)
            {
                case enumFrameService.ChangeClientFrame:
                    //ucClient.Child = null;
                    ClearClientFrame(args[0].ToString());
                    break;

                case enumFrameService.ChangeSideFrame:
                    {
                        UCFrameBase panel = _DicSubNavigation[(int)args[0]];
                        if (panel != null)
                        {
                            ucSubNavigation.Child = panel;
                        }
                    }
                    break;

                case enumFrameService.ClearClientFrame:
                    ucClient.Child = null;                    
                    break;

                case enumFrameService.ClearSideFrame:
                    ucSubNavigation.Child = null;
                    break;

                case enumFrameService.PopUpWindow:

                case enumFrameService.UserEvent:
                    OnUserEvent?.Invoke(sender, enService, args);
                    break;

                case enumFrameService.Close:
                    Application.Current.Shutdown();
                    break;

                default:
                    break;
            }
        }
        //#endregion


    }
}

using LGCNS.ezControl.Presentation;
using System.Collections.Concurrent;
using System.Windows;
using System.Windows.Controls;

namespace ECS.BaseUI
{
    public partial class UcWindowBase : UserControl
    {
        #region event
        public event delegateFrameServiceUserEvent OnUserEvent;
        #endregion 

        #region variable
        private ConcurrentDictionary<int, UcFrameBase> _DicClinet;
        private ConcurrentDictionary<int, UcFrameBase> _DicSubNavigation;
        #endregion

        public UcWindowBase()
        {
            InitializeComponent();

            _DicClinet = new ConcurrentDictionary<int, UcFrameBase>();
            _DicSubNavigation = new ConcurrentDictionary<int, UcFrameBase>();
        }

        public void RegisterTitlePanel(UcFrameBase panel)
        {
            ucTitle.Child = panel;
            panel.OnRequestParentService += OnRequestParentService;
        }

        public void RegisterNavigationPanel(UcFrameBase panel)
        {
            ucNavigation.Child = panel;
            panel.OnRequestParentService += OnRequestParentService;
        }

        public void RegisterSubNavigationPanel(UcFrameBase panel, int iKey, bool bShow)
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

        public void RegisterClientPanel(UcFrameBase panel, int iKey, bool bShow)
        {
            bool ret = _DicClinet.TryAdd(iKey, panel);

            if (ret)
            {
                panel.OnRequestParentService += OnRequestParentService;
                if (bShow)
                {
                    ucClient.Child = panel;
                }
            }
        }        

        #region private
        void ChangeClient(int key)
        {
            UcFrameBase current = (UcFrameBase) ucClient.Child;

            if (current.HideFrame())
            {
                UcFrameBase panel = _DicClinet[key];
                if (panel != null)
                {
                    ucClient.Child = panel;
                }
            }
        }

        void OnRequestParentService(object sender, enumFrameService enService, params object[] args)
        {
            switch (enService)
            {
                case enumFrameService.ChangeClientFrame:
                    {
                        ChangeClient((int)args[0]);
                    }
                    break;

                case enumFrameService.ChangeSideFrame:
                    {
                        UcFrameBase panel = _DicSubNavigation[(int)args[0]];
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
        #endregion 
    }
}
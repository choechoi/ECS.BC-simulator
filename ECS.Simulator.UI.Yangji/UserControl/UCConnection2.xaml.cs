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

using LGCNS.ezControl.Presentation;
using LGCNS.ezControl.Diagnostics;
using LGCNS.ezControl.Core;

using ECS.Common;

namespace ECS.Simulator.UI
{
    /// <summary>
    /// UCConnection2.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UCConnection2 : UCCustomUserControl
    {
        #region Delegate
        public delegate void delegatePanelUIHandeler(int iEventID, params object[] args);
        #endregion

        #region Event Define
        public event delegatePanelUIHandeler OnPanelUIEvent;
        #endregion

        #region Constructor

        public UCConnection2()
        {
            InitializeComponent();
        }

        #endregion

        #region Override Method
        protected override void OnLinkedReferenceEvent(string strSubjectName, int iEventID, params object[] args)
        {
            base.OnLinkedReferenceEvent(strSubjectName, iEventID, args);

            DispatchEvent(iEventID, args);
        }

        protected override void OnReferenceLinked(LGCNS.ezControl.Core.CReference reference)
        {
            base.OnReferenceLinked(reference);

            if (reference.ReferenceState == LGCNS.ezControl.Common.enumReferenceState.Active)
            {
                OnPanelUIEvent?.Invoke((int)CEnum2.EnumToUIEvent.Connected, null);

            }
            else if (reference.ReferenceState == LGCNS.ezControl.Common.enumReferenceState.Fault)
            {
                OnPanelUIEvent?.Invoke((int)CEnum2.EnumToUIEvent.Disconnected, null);
            }
        }

        protected override void OnLinkedReferenceStateChanged(LGCNS.ezControl.Core.CReference refer, LGCNS.ezControl.Common.enumReferenceState state)
        {
            base.OnLinkedReferenceStateChanged(refer, state);

            if (refer.ReferenceState == LGCNS.ezControl.Common.enumReferenceState.Active)
            {
                OnPanelUIEvent?.Invoke((int)CEnum2.EnumToUIEvent.Connected, null);
            }
            else if (refer.ReferenceState == LGCNS.ezControl.Common.enumReferenceState.Fault)
            {
                OnPanelUIEvent?.Invoke((int)CEnum2.EnumToUIEvent.Disconnected, null);
            }
        }

        #endregion

        #region Public Method
        public object SendCommand(CEnum2.EnumToCoreEventForSimulator cmd, params object[] args)
        {
            return FireToCoreEvent(cmd, args);
        }
        #endregion

        #region Private Method
        #region DispatchEvent (Core -> UI Event 실행 Method)
        private void DispatchEvent(int iEventID, params object[] args)
        {
            try
            {
                OnPanelUIEvent?.Invoke(iEventID, args);
            }
            catch (Exception ex)
            {
                SystemLogger.Log(LGCNS.ezControl.Diagnostics.Level.Exception, ex, this.Name);
            }
        }
        #endregion
        #region Common (UI 애니메이션 및 Button Click Event)
        #region Execute Wrapper
        private object FireToCoreEvent(CEnum2.EnumToCoreEventForSimulator eventName, params object[] args)
        {
            try
            {
                object ret = new object();

                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate {
                    ret = _LinkedReference.Execute(ClientConstant.UIEventReceiver, (int)eventName, args);
                }));

                return ret;
            }
            catch (Exception ex)
            {
                SystemLogger.Log(LGCNS.ezControl.Diagnostics.Level.Exception, ex, this.Name);
                return -1;
            }
        }

        #endregion
        #endregion
        #endregion


    }
}

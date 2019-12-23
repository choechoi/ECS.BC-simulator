using LGCNS.ezControl.Presentation;
using System.Windows.Controls;

namespace ECS.BaseUI
{
    public class UcFrameBase : UserControl
    {
        public event delegateFramePanelServiceRequest OnRequestParentService;        

        protected void RequestParentService(object sender, enumFrameService enService, params object[] args)
        {
            OnRequestParentService?.Invoke(sender, enService, args);
        }

        public bool HideFrame()
        {
            bool bCancel = true;
            OnHideFrame(out bCancel);

            return bCancel;
        }

        protected virtual void OnHideFrame(out bool bCancel)
        {
            bCancel = true;
        }
    }
}

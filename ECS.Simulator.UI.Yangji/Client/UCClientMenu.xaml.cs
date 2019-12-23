using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Media;
using System.Net;
using System.Data;
using LGCNS.ezControl.Common;
using LGCNS.ezControl.Diagnostics;

using ECS.Common;
using Database.IF;
using LGCNS.ezControl.Core;
using ECS.BaseUI;
using LGCNS.ezControl.Presentation;

namespace ECS.Simulator.UI
{
    /// <summary>
    /// UCClient_Input.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UCClientMenu : UCFrameBase
    {
        #region Variable
        #endregion

        #region Constructor
        public UCClientMenu()
        {
            InitializeComponent();

        }



        #endregion

        

        #region button 1F
        private void btnHUBSorter_Click(object sender, RoutedEventArgs e)
        {
            RequestParentService(this, enumFrameService.ChangeClientFrame, 13);
        }
        private void btnConv_1F_Wh_Insp_Click(object sender, RoutedEventArgs e)
        {
            RequestParentService(this, enumFrameService.ChangeClientFrame, 1);
        }

        //private void btnConv_1F_Wh_Rtn_Click(object sender, RoutedEventArgs e)
        //{
        //    RequestParentService(this, enumFrameService.ChangeClientFrame, 2);
        //}

        //private void btnConv_1F_Wh_Hge_Click(object sender, RoutedEventArgs e)
        //{
        //    RequestParentService(this, enumFrameService.ChangeClientFrame, 3);
        //}

        //private void btnConv_1F_Rtn_Spr_Click(object sender, RoutedEventArgs e)
        //{
        //    RequestParentService(this, enumFrameService.ChangeClientFrame, 4);
        //}

        //private void btnConv_1F_Pre_Click(object sender, RoutedEventArgs e)
        //{
        //    RequestParentService(this, enumFrameService.ChangeClientFrame, 5);
        //}

        private void btnConv_Insp_Reinput_Click(object sender, RoutedEventArgs e)
        {
            RequestParentService(this, enumFrameService.ChangeClientFrame, 6);
        }

        private void btnConv_MV_2F1F_Click(object sender, RoutedEventArgs e)
        {
            RequestParentService(this, enumFrameService.ChangeClientFrame, 7);
        }

        #endregion

        #region Button 2F
        private void btnSorter_Click(object sender, RoutedEventArgs e) //
        {
            RequestParentService(this, enumFrameService.ChangeClientFrame, 0);
        }
        private void btnConv_2F_Pre_Click(object sender, RoutedEventArgs e)
        {
            RequestParentService(this, enumFrameService.ChangeClientFrame, 8);
        }

        private void btnConv_2F_GI_Click(object sender, RoutedEventArgs e)
        {
            RequestParentService(this, enumFrameService.ChangeClientFrame, 9);
        }

        private void btnConv_2F_MV_Click(object sender, RoutedEventArgs e)
        {
            RequestParentService(this, enumFrameService.ChangeClientFrame, 10);
        }
        #endregion

        #region Button 3F
        private void btnConv_WH_Click(object sender, RoutedEventArgs e)  //
        {
            RequestParentService(this, enumFrameService.ChangeClientFrame, 11);            
        }

        private void btnConv_DAS_Click(object sender, RoutedEventArgs e)  //
        {
            RequestParentService(this, enumFrameService.ChangeClientFrame, 12);
        }

        #endregion

        
    }
}

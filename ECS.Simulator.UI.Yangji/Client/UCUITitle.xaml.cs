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

using LGCNS.ezControl.Common;
using LGCNS.ezControl.Diagnostics;
using LGCNS.ezControl.Core;
using LGCNS.ezControl.Presentation;

using System.Windows.Threading;
using System.Data;

using ECS.Common;
using Database.IF;
using ECS.BaseUI;

namespace ECS.Simulator.UI
{
    /// <summary>
    /// UCClientTest3FCV.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UCUITitle : UCFrameBase
    {       
        public UCUITitle()
        {
            InitializeComponent();
        }

        private void cbxSimulSel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //if (ucConn.LinkedReference != null)
                //{
                //    ucConn.LinkedReference.Stop();
                //    ucConn.OnPanelUIEvent -= uCCommunication_OnPanelUIEvent;
                //    _reference = null;
                //}
                //dtg1.ItemsSource = null;
                //dtTestData = null;
                //connectElementNo = 0;

                //selectedEQPID = cbxSelSorter.SelectedValue.ToString();

                //if (selectedEQPID == "DS01")
                //{
                //    connectElementNo = 76;
                //}
                //else if (selectedEQPID == "DS02")
                //{
                //    connectElementNo = 86;
                //}
                //else if (selectedEQPID == "DS03")
                //{
                //    connectElementNo = 88;
                //}
                //_reference = CReferenceManager.GetReference("Simulator_Sorter", connectElementNo);
                //ucConn.OnPanelUIEvent += uCCommunication_OnPanelUIEvent;
                //ucConn.LinkedReference = _reference;
                //if (ucConn.LinkedReference != null)
                //{
                //    ucConn.LinkedReference.Start();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

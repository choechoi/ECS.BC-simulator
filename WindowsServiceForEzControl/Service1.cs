using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WindowsServiceForEzControl
{
    public partial class Service1 : ServiceBase
    {
        System.Timers.Timer _ProcessChecktimer;
        Configuration _config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        public Service1()
        {
            InitializeComponent();

            _ProcessChecktimer = new System.Timers.Timer();
        }

        protected override void OnStart(string[] args)
        {
            ProcessStartInfo _processStartInfo = new ProcessStartInfo();
            _processStartInfo.WorkingDirectory = @GetConfigValue("DIRECTORY");
            _processStartInfo.FileName = @GetConfigValue("FILENAME");
            _processStartInfo.Arguments = GetConfigValue("ELEMENTNO");
        

            _processStartInfo.CreateNoWindow = false;
            Process myProcess = Process.Start(_processStartInfo);

            if (!_ProcessChecktimer.Enabled)
            {
                _ProcessChecktimer.Interval = 60 * 1000;
                _ProcessChecktimer.Elapsed += _ProcessChecktimer_Elapsed;
                _ProcessChecktimer.Start();
            }
        }

        private void _ProcessChecktimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (GetaProcess("LGCNS.ezControl.Console") == null)
            {
                OnStart(null);
            }
        }

        protected override void OnStop()
        {
            foreach (Process process in Process.GetProcesses())
            {
                if (process.ProcessName.StartsWith("LGCNS.ezControl.Console"))
                {
                    process.Kill();
                }
            }
        }

        private System.Diagnostics.Process GetaProcess(string processname)
        {
            System.Diagnostics.Process[] aProc = Process.GetProcessesByName(processname);

            if (aProc.Length > 0)
                return aProc[0];
            else
                return null;
        }


        public string GetConfigValue(String type)
        {
            string value = string.Empty;
            foreach (KeyValueConfigurationElement setting in _config.AppSettings.Settings)
            {
                if (setting.Key.Equals(type))
                {
                    value = setting.Value;
                }
            }

            return value;
        }
    }
}

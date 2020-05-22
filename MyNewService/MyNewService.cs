using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Runtime.InteropServices;

namespace MyNewService
{
    public partial class MyNewService : ServiceBase
    {
        private int eventId = 1;
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(System.IntPtr handle, ref ServiceStatus serviceStatus);
        public MyNewService(string[] args)
        {
            string eventSourceName = "MySource";
            string logName = "MyNewLog";

            if (args.Length > 0)
            {
                eventSourceName = args[0];
            }

            if (args.Length > 1)
            {
                logName = args[1];
            }
            InitializeComponent();
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists(eventSourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    eventSourceName, logName);
            }
            eventLog1.Source = eventSourceName;
            eventLog1.Log = logName;
        }

        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            // Update the service state to Running.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            //set timer
            string strPath = AppDomain.CurrentDomain.BaseDirectory + "Log.txt";
            System.IO.File.AppendAllLines(strPath, new[] { "Starting time : " + DateTime.Now.ToString() });
            //this.timer1.Start();
            DateTime todaysDate = DateTime.Now.Date;
            int day = todaysDate.Day;
            int month = todaysDate.Month;
            int year = todaysDate.Year;
            int totalDays = DateTime.DaysInMonth(year, month);
            if (day == 22)
            {
                Timer timer = new Timer();
                timer.Interval = totalDays * 100; //86400000
                timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
                timer.Start();
            }
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("In OnStop.");
            // Update the service state to Stop Pending.
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            // Update the service state to Stopped.
            serviceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            DBConnect dBConnect = new DBConnect();
            dBConnect.CloseConnection();
            string strPath = AppDomain.CurrentDomain.BaseDirectory + "Log.txt";
            System.IO.File.AppendAllLines(strPath, new[] { "Stop time : " + DateTime.Now.ToString() });
            this.timer1.Stop();
        }

        protected override void OnContinue()
        {
            eventLog1.WriteEntry("In OnContinue.");
        }

        private void eventLog1_EntryWritten(object sender, EntryWrittenEventArgs e)
        {

        }

        public void OnTimer(object sender, ElapsedEventArgs args)
        {
            DBConnect dBConnect = new DBConnect();
            string a = dBConnect.Insert();
            eventLog1.WriteEntry(a.ToString());
            string strPath = AppDomain.CurrentDomain.BaseDirectory + "Log.txt";
            System.IO.File.AppendAllLines(strPath, new[] { "..calling time : " + DateTime.Now.ToString() });
        }

        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public int dwServiceType;
            public ServiceState dwCurrentState;
            public int dwControlsAccepted;
            public int dwWin32ExitCode;
            public int dwServiceSpecificExitCode;
            public int dwCheckPoint;
            public int dwWaitHint;
        };

        private void timer1_Elapsed(object sender, ElapsedEventArgs e)
        {
          //  DateTime todaysDate = DateTime.Now.Date;
           // int month = todaysDate.Month;
            //int year = todaysDate.Year;
            //int totalDays = DateTime.DaysInMonth(year, month);
          //  string strPath = AppDomain.CurrentDomain.BaseDirectory + "Log.txt";
            //System.IO.File.AppendAllLines(strPath, new[] { "..calling time : " + totalDays.ToString() });
        }
    }
}

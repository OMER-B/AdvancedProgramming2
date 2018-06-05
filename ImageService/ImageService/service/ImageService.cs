using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using Tools;
using Logic;

namespace ImageService
{
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

    public partial class ImageService : ServiceBase
    {
        private EventLog eventLog;
        private ImageServer server;
        private ILogger logger;
        public delegate List<string> GetLog();

        public ImageService(string[] args)
        {
            InitializeComponent();
            reader = new ConfigReader();

            if (!EventLog.SourceExists(reader.LogName))
            {
                EventLog.CreateEventSource(
                    reader.SourceName, reader.LogName);
            }
            eventLog.Source = reader.SourceName;
            eventLog.Log = reader.LogName;

            IImageModel imageModel = new ImageModel(reader.OutputDir, reader.ThumbnailSize);
            logger = new Logger();
            logger.MessageRecieved += OnMsg;
            DebugLogger.Instance.MessageRecieved += OnMsg;

            this.server = new ImageServer(logger, imageModel, reader);
            foreach (string path in reader.Handler)
            {
                server.AddNewDirectoryHandler(path);
            }
            server.StopHandler += RemoveHandler;
        }

        public void OnMsg(object sender, LogMessageArgs args)
        {
            eventLog.WriteEntry(args.Message);
        }

        private ConfigReader reader;
        public void RemoveHandler(object sender, DirectoryCloseEventArgs args)
        {
            reader.Remove(args.DirectoryPath);
        }

        protected override void OnStart(string[] args)
        {
            logger.Log(this, new LogMessageArgs(LogMessageTypeEnum.INFO, "Service started."));
            //eventLog.WriteEntry("Service started.");
            // Update the service state to Running.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        protected override void OnStop()
        {
            server.CloseAll();
            eventLog.WriteEntry("Service stopped.");
        }

        protected override void OnContinue()
        {
            eventLog.WriteEntry("Service continued.");
        }


        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);
    }
}

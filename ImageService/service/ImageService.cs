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

        public ImageService(string[] args)
        {
            InitializeComponent();
            ConfigReader reader = new ConfigReader();

            if (!EventLog.SourceExists(reader.LogName))
            {
                EventLog.CreateEventSource(
                    reader.SourceName, reader.LogName);
            }
            eventLog.Source = reader.SourceName;
            eventLog.Log = reader.LogName;

            ImageModel imageModel = new ImageModel(reader.OutputDir, reader.ThumbnailSize);

            ILogger logger = new Logger();
            
            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ TODO delete this
            global.Instance.logger = logger;
            logger.MessageRecieved += OnMsg;

            this.server = new ImageServer(logger, imageModel);
            foreach (string path in reader.Handler)
            {
                server.AddNewDirectoryHandler(path);
            }


            // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ TODO delete this
            global.Instance.logger.Log(this, new MessageRecievedEventArgs(MessageTypeEnum.INFO, "end of service constructor"));
            ////////////////
        }

        public void OnMsg(object sender, MessageRecievedEventArgs args)
        {
            eventLog.WriteEntry(args.Message);
        }

        protected override void OnStart(string[] args)
        {

            eventLog.WriteEntry("The service started.");

            // Update the service state to Running.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        protected override void OnStop()
        {
            // TODO: send commands to close all the handlers
            eventLog.WriteEntry("The service stopped.");
        }

        protected override void OnContinue()
        {
            eventLog.WriteEntry("In OnContinue.");
        }


        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);
    }
}

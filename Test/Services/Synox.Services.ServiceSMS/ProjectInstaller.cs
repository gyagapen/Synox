using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.IO;


namespace Synox.Services.ServiceSMS
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void serviceInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {
            System.Management.ConnectionOptions coOptions;
            System.Management.ManagementScope mgmtScope;
            System.Management.ManagementObject wmiService;
            System.ServiceProcess.ServiceController serviceController;
            System.Management.ManagementBaseObject InParam;
            System.Management.ManagementBaseObject OutParam;

            coOptions = new System.Management.ConnectionOptions();
            coOptions.Impersonation = System.Management.ImpersonationLevel.Impersonate;

            mgmtScope = new System.Management.ManagementScope(@"root\CIMV2", coOptions);
            mgmtScope.Connect();


            serviceController = new System.ServiceProcess.ServiceController(serviceInstaller1.ServiceName);

            wmiService = new System.Management.ManagementObject("Win32_Service.Name='" + serviceController.ServiceName + "'");

            InParam = wmiService.GetMethodParameters("Change");

            InParam["DesktopInteract"] = true;

            OutParam = wmiService.InvokeMethod("Change", InParam, null);

        }
        /// <summary>
        /// Suppression des fichiers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void serviceInstaller1_BeforeUninstall(object sender, System.Configuration.Install.InstallEventArgs e)
        {
            String serviceName = serviceInstaller1.ServiceName;
            String serviceFullName = Path.Combine(EnvironmentApplicationHelper.ApplicationPath, "ServiceSMS.exe");
            System.ServiceProcess.ServiceController serviceController;

            try
            {
                serviceController = new System.ServiceProcess.ServiceController(serviceName);
                if (serviceController.Status == System.ServiceProcess.ServiceControllerStatus.Running)
                {
                    serviceController.Stop();
                }
                serviceController.Dispose();

            }
            catch { }
        }
    }
}

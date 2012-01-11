using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace Synox.Services.ServiceSMS
{
    public partial class ServicePrincipal : ServiceBase
    {
        public ServiceManager manager;
        public ServicePrincipal()
        {
            InitializeComponent();
            manager = new ServiceManager();
        }

        /// <summary>
        /// Debut du service
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            manager.Start();
        }

        protected override void OnStop()
        {
            manager.Stop();
            manager.Dispose();
        }
    }
}

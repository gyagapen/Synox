using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;

namespace WindowsService1
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            StreamWriter sw = new StreamWriter(@"C:/MyService.log", true);
            sw.WriteLine("Démarrage de MyService : " + DateTime.Now.ToLongTimeString());
            sw.Close();
        }

        protected override void OnStop()
        {
            StreamWriter sw = new StreamWriter(@"C:/MyService.log", true);
            sw.WriteLine("Arrêt de MyService : " + DateTime.Now.ToLongTimeString());
            sw.Close();
        }

        protected override void OnContinue()
        {
            StreamWriter sw = new StreamWriter(@"C:/MyService.log", true);
            sw.WriteLine("Reprise de MyService : " + DateTime.Now.ToLongTimeString());
            sw.Close();
        }

        protected override void OnPause()
        {
            StreamWriter sw = new StreamWriter(@"C:/MyService.log", true);
            sw.WriteLine("Mise en pause de MyService : " + DateTime.Now.ToLongTimeString());
            sw.Close();
        }

        protected override void OnShutdown()
        {
            base.OnShutdown();
        }
    }
}

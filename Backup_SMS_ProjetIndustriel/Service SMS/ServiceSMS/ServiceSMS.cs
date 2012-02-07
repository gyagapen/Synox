using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;
using System.IO;

namespace ServiceSMS
{
    public partial class ServiceSMS : ServiceBase
    {

        public ServiceManager manager;
        public ServiceSMS()
        {
            InitializeComponent();
            manager = new ServiceManager();
        }

        protected override void OnShutdown()
        {
            base.OnShutdown();
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        protected override void OnContinue()
        {
            base.OnContinue();
        }


        

        protected override void OnStart(string[] args)
        {
            manager.Start();
            
        }

        protected override void OnStop()
        {
            manager.Stop();
            manager.Dispose();

        }


        protected void t_Elapsed(object sender, EventArgs e)
        {
            if (File.Exists(@"C:\temp\test.txt"))
            {
                StreamReader sr = new StreamReader(@"C:\temp\test.txt");
                string txt = sr.ReadToEnd();
                sr.Close();
                StreamWriter sw = new StreamWriter(@"C:\temp\test.txt");
                sw.WriteLine(txt);
                sw.WriteLine(DateTime.Now.ToString());
                sw.Close();
            }
            else
            {
                TextWriter file = File.CreateText(@"C:\temp\test.txt");
                file.WriteLine(DateTime.Now.ToString());
                file.Close();
            }
        }
    }
}

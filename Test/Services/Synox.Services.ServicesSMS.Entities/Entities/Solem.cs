using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Synox.Services.ServiceSMS.Entity
{
    public class Solem
    {
        // infos du transmetteur
        public string  NumeroGsm { get; set; }
        public string Alarme { get; set; }
        public string NumeroUid { get; set; }

        // ip et port
        public string ServerIpOrDns { get; set; }
        public int ServerPort { get; set; }
        public string BackupIpOrDns { get; set; }
        public int BackupPort { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Synox.Services.ServiceSMS.Entity
{
    class BirdyBox
    {
        // infos du transmetteur
        public string NumeroGsm { get; set; }
        public string Alarme01 { get; set; }
        public string Alarme02 { get; set; }
        public string Alarme03 { get; set; }
        public string Alarme04 { get; set; }
        public string Alarme05 { get; set; }
        public int NbAlarme { get; set; }
        public string Tim { get; set; }
        public string Sn { get; set; }
        public string NumeroUid { get; set; }

        // ip et port
        public string ServerIpOrDns { get; set; }
        public int ServerPort { get; set; }
        public string BackupIpOrDns { get; set; }
        public int BackupPort { get; set; }
    }

}

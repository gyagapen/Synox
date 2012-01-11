using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Synox.Services.ServiceSMS.Entity.ServiceSIM;

namespace Synox.Services.ServiceSMS.Entity.Helpers
{
    public static class SimHelper
    {
        private const string loginServiceSimM2M = "backup";
        private const string passwordServiceSimM2M = "Solem.BackUpSms@2011";

        /// <summary>
        /// Permet de récupérer le N° ICCID de la SIM à partir du N° Téléphone GSM
        /// Via un Web Service M2M France
        /// </summary>
        public static string GetICCIDFromGsmNumber(string gsmNumber)
        {
            string iccidNumber = null;
            ServiceSIMSoapClient serviceSimM2M = null;
            SoapHeaderAuthentification header = null;

            try
            {
                serviceSimM2M = new Entity.ServiceSIM.ServiceSIMSoapClient();

                header = new SoapHeaderAuthentification() { Login = loginServiceSimM2M, Password = passwordServiceSimM2M };
                if (gsmNumber.StartsWith("0"))
                    gsmNumber = "+33"+ gsmNumber.Substring(1);
                iccidNumber = serviceSimM2M.GetICCIDFromGsmNumber(header, gsmNumber);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return iccidNumber;
        }
    }
}

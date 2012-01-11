using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;

using Synox.Services.ServiceSMS.Entity;
using Synox.Services.ServiceSMS.Helpers;
using Synox.Services.ServiceSMS;

namespace Synox.Web.ServiceSms
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ServiceWcfSms" in code, svc and config file together.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class ServiceWcfSms : IServiceWcfSms
    {
        public void SendSms(string numeroGsm, string message)
        {
            try
            {
                SmsHelper.SaveSmsAEnvoyer(numeroGsm, message);
            }
            catch (Exception ex)
            {
                LogHelper.Trace("SendSms", ex, LogHelper.EnumCategorie.Erreur);
                LogHelper.Trace(ex.StackTrace, LogHelper.EnumCategorie.Erreur);
                throw;
            }
        }

        public List<Sms> Get()
        {
            List<Sms> smsList;
            smsList = WindowsMobileHelper.GetSms();
            return smsList;
        }

        public void SaveState(List<Sms> smsList)
        {
            SmsHelper.SaveDone(smsList);
        }
    }
}

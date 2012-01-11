using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using Synox.Services.ServiceSMS.Helpers;
using Synox.Services.ServiceSMS;
using Synox.Services.ServiceSMS.Entity;

namespace Synox.Web.ServiceSms
{
    /// <summary>
    /// Summary description for WebServiceSms
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebServiceSms : System.Web.Services.WebService
    {

        [WebMethod]
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

        [WebMethod]
        public List<Sms> Get()
        {
            List<Sms> smsList;
            smsList = WindowsMobileHelper.GetSms();
            return smsList;
        }
        [WebMethod]
        public void SaveState(List<Sms> smsList)
        {
            SmsHelper.SaveDone(smsList);
        }
    }
}

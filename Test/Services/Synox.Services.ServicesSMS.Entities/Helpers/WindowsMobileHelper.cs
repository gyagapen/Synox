using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Synox.Services.ServiceSMS.Entity;

namespace Synox.Services.ServiceSMS.Helpers
{
    public class WindowsMobileHelper
    {

        public static List<Sms> GetSms()
        {
            List<Sms> smsList;
            Entity.Entities dataContext = null;
            try
            {
                dataContext = new Entity.Entities();
                smsList = dataContext.Sms
                    .Include("SmsStatut")
                    .Where(d => (d.DateEnvoi == null || d.SmsStatut== null)
                        && d.ProjetId == (int)EnumProjet.WindowsMobile
                    )
                    .ToList();

                return smsList;


            }
            catch (Exception ex) { LogHelper.Trace("WindowsMobile.GetSms", ex, LogHelper.EnumCategorie.Erreur); throw; }
            finally
            {
                if (dataContext != null)
                {
                    dataContext.Dispose();
                }
            }
        }
    }
}

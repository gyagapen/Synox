using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Synox.Services.ServiceSMS.Entity.ServiceGeocity;
using Synox.Services.ServiceSMS.Entity;

namespace Synox.Services.ServiceSMS.Helpers
{
    public class GeocityHelper
    {
        public static DateTime? SendSms(string numeroGsm, string message, string urlWebService)
        {
            try
            {
                SyncMobileService client = new SyncMobileService();
                client.Url = urlWebService;
                client.SendSms(numeroGsm, message);
                return DateTime.Now;
            }
            catch (Exception ex)
            {
                LogHelper.Trace("GeocityHelper.SendSms", ex, LogHelper.EnumCategorie.Erreur);
                return null;
            }
        }
        public static int SendSmsEnAttente()
        {
            List<SmsReception> smsList;
            Entity.Entities dataContext = null;
            List<NumeroGsmProjet> numeroGsms = null;
            NumeroGsmProjet projet = null;
            int nbTraites=0;
            try
            {
                dataContext = new Entities();
                // liste des numéros de téléphones en mode push
                numeroGsms = dataContext.NumeroGsmProjet
                            .Where(d => !d.Suppression)
                            .ToList();

                // liste des sms non lu 
                smsList = dataContext.SmsReception
                    .Where(d => d.DateLecture == null)
                    .ToList();

                // traitement des sms provenant d'un num de telephone push
                foreach (SmsReception sms in smsList)
                {
                    // cherche le numero de telephone correspondant au sms
                    if ((projet = numeroGsms.Where(n => n.NumeroGsm == sms.NumeroGsm).FirstOrDefault()) != null)
                    {
                        switch ((EnumProjet)projet.ProjetId)
                        {
                            case EnumProjet.GeocityCielVert:
                                sms.DateLecture = GeocityHelper.SendSms(sms.NumeroGsm, sms.Message, projet.FonctionName);
                                if (sms.DateLecture.HasValue) nbTraites++;
                                break;
                            case EnumProjet.GeocityA2C:
                                sms.DateLecture = GeocityHelper.SendSms(sms.NumeroGsm, sms.Message, projet.FonctionName);
                                if (sms.DateLecture.HasValue) nbTraites++;
                                break;
                        }
                    }
                }
                dataContext.SaveChanges();
            }
            catch (Exception ex)
            {
                LogHelper.Trace("GeocityHelper.SendSmsEnAttente", ex, LogHelper.EnumCategorie.Erreur);
            }
            return nbTraites;
        }
    }
}

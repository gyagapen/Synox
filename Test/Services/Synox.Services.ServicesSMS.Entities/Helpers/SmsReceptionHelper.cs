using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Synox.Services.ServiceSMS.Entity;
using Synox.Services.ServiceSMS.Donnees;

namespace Synox.Services.ServiceSMS.Helpers
{
    public class SmsReceptionHelper
    {
        /// <summary>
        /// Affecte au SMS un projet : Chaque SMS doit être traité spécifiquement en fonction du projet auquel il appartient
        /// </summary>
        public static void AffecteProjet()
        {
            List<SmsReception> smsList;
            Entity.Entities dataContext = null;
            List<NumeroGsmProjet> numeroGsms = null;
            NumeroGsmProjet projet=null;
            bool projetPush = false;
            try
            {
                dataContext = new Entity.Entities();

                // liste des sms non affecté à un projet
                smsList = dataContext.SmsReception
                    .Where(d => d.ProjetId == null)
                    .ToList();

                // liste des numéros de téléphones liés à un projet
                numeroGsms = dataContext.NumeroGsmProjet
                    .Where(d => d.ProjetId != null && !d.Suppression)
                    .ToList();
                
                // traitement pour chaque sms
                foreach (SmsReception sms in smsList)
                {
                    projet=null;
                    if ((projet = numeroGsms.Where(n => n.NumeroGsm == sms.NumeroGsm).FirstOrDefault()) != null)
                    {
                        sms.ProjetId = projet.ProjetId;
                        if (projet.Push)
                            projetPush = true;
                    }
                    else if (sms.Message.StartsWith("V"))
                        sms.ProjetId = (int)EnumProjet.BirdyBox;
                    else if (sms.Message.StartsWith("G06") || sms.Message.StartsWith("G07") || sms.Message.StartsWith("G+33") || sms.Message.StartsWith("G33"))
                        sms.ProjetId = (int)EnumProjet.Solem;
                    else
                        sms.ProjetId = (int)EnumProjet.Autres;

                }

                dataContext.SaveChanges();

                // nouveau SMS recu d'un projet en mode push 
                if (projetPush)
                {
                    // liste des numéros de téléphones en mode push
                    numeroGsms = numeroGsms
                                .Where(d => d.Push && !d.Suppression)
                                .ToList();
                    // traitement des sms provenant d'un num de telephone push
                    foreach (SmsReception sms in smsList)
                    {
                        // cherche le numero de telephone correspondant au sms
                        if ((projet = numeroGsms.Where(n => n.NumeroGsm == sms.NumeroGsm).FirstOrDefault()) != null)
                        {
                            switch((EnumProjet)projet.ProjetId)
                            {
                                case EnumProjet.GeocityCielVert:
                                    sms.DateLecture = GeocityHelper.SendSms(sms.NumeroGsm, sms.Message, projet.FonctionName);
                                    break;
                            }
                        }
                    }
                }
                dataContext.SaveChanges();

            }
            catch (Exception ex) { LogHelper.Trace("Reception.AffecteProjet", ex, LogHelper.EnumCategorie.Erreur); throw; }
            finally
            {
                if (dataContext != null)
                {
                    dataContext.Dispose();
                }
            }

        }
        /// <summary>
        /// Recupere les Sms Recus et non traités
        /// </summary>
        /// <returns></returns>
        public static List<SmsReception> GetSmsRecus()
        {
            List<SmsReception> smsList;
            Entity.Entities dataContext = null;
            try
            {
                dataContext = new Entity.Entities();
                smsList = dataContext.SmsReception
                    .Include("Projet")
                    .Take(1000)
                    .ToList();

                return smsList;


            }
            catch (Exception ex) { LogHelper.Trace("GetSmsRecus", ex, LogHelper.EnumCategorie.Erreur); throw; }
            finally
            {
                if (dataContext != null)
                {
                    dataContext.Dispose();
                }
            }
        }
        /// <summary>
        /// Recupere les Sms Recus et non traités
        /// </summary>
        /// <returns></returns>
        public static List<SmsReception> GetSmsRecus(int projetId)
        {
            List<SmsReception> smsList;
            Entity.Entities dataContext = null;
            try
            {
                dataContext = new Entity.Entities();
                smsList = dataContext.SmsReception
                    .Where(d => d.DateLecture == null
                     && d.ProjetId == projetId)
                    .ToList();

                return smsList;


            }
            catch (Exception ex) { LogHelper.Trace("GetSmsRecus", ex, LogHelper.EnumCategorie.Erreur); throw; }
            finally
            {
                if (dataContext != null)
                {
                    dataContext.Dispose();
                }
            }
        }
        /// <summary>
        /// enregistrement du sms en base
        /// </summary>
        /// <param name="sms"></param>
        /// <returns></returns>
        internal static SmsReception SaveReception(SmsReception sms)
        {
            string procedureEnregistrement = Synox.Services.ServiceSMS.Entity.Properties.Resources.SP_NouveauSmsRecu;
            Hashtable parametres = new Hashtable();
            SqlServer server = null;

            try
            {
                if (sms == null) return sms;
                LogHelper.Trace(string.Format("Sms : [{0}] => '{1}'", sms.NumeroGsm, sms.Message), LogHelper.EnumCategorie.Information);

                parametres.Add("@NumeroGsm", sms.NumeroGsm);
                parametres.Add("@Message", sms.Message);
                parametres.Add("@DateReception", sms.DateReception);

                server = new SqlServer(EnvironmentApplicationHelper.ConnectionString);
                server.ExecuteSp(procedureEnregistrement, parametres);
                server.Close();
            }
            catch (System.Data.SqlClient.SqlException dbe)
            {
                if (server != null)
                    server.Close();
                LogHelper.Trace(string.Format("une erreur s'est produit sur le serveur '{0}' dans la procédure '{1}' à la ligne {2}", dbe.Server, dbe.Procedure, dbe.LineNumber), LogHelper.EnumCategorie.Erreur);
                throw;
            }
            return sms;
        }

        /// <summary>
        /// Envoi des trames SMS à la plateforme IP
        /// </summary>
        /// <returns></returns>
        public static int SolemEnvoiPlateformeIp()
        {
            int nbSms = 0;
            List<SmsReception> smsRecus;
            try
            {
                // récupération des SMS en attente de traitement
                smsRecus = GetSmsRecus((int)EnumProjet.Solem);

                // traitement des sms
                smsRecus = SolemHelper.SendListSms(smsRecus);

                // on retourne le nombre de sms traités
                    //List<SmsReception> r = smsRecus.Where(s => s.DateLecture.HasValue).ToList();
                    //if (r != null)
                    //    nbSms = r.Count;
                nbSms = smsRecus.Where(s => s.DateLecture.HasValue).Count();

                if(nbSms>0)
                    SmsReceptionHelper.TraitementEffectue(smsRecus);
                LogHelper.Trace("[SOLEM] "+nbSms + " sms envoyés", LogHelper.EnumCategorie.Information);

            }
            catch (Exception ex)
            {
                LogHelper.Trace("SolemEnvoiPlateformeIp",ex, LogHelper.EnumCategorie.Erreur);
            }

            return nbSms;
        }

        private static void TraitementEffectue(List<SmsReception> smsRecus)
        {
            Entity.Entities dataContext = null;
            try
            {
                dataContext = new Entity.Entities();

                foreach (SmsReception sms in smsRecus)
                {
                    Entity.SmsReception smsServeur = dataContext.SmsReception.Where(s => s.Id == sms.Id).FirstOrDefault();
                    if (smsServeur != null)
                    {
                        smsServeur.Commentaire = sms.Commentaire;
                        smsServeur.DateLecture = sms.DateLecture;
                    }
                }

                dataContext.SaveChanges();


            }
            catch (Exception ex) { LogHelper.Trace("TraitementEffectue", ex, LogHelper.EnumCategorie.Erreur); throw; }
            finally
            {
                if (dataContext != null)
                {
                    dataContext.Dispose();
                }
            }
        }

        public static int BirdyEnvoiPlateformeIp()
        {
            int nbSms = 0;
            List<SmsReception> smsRecus;
            try
            {
                // récupération des SMS en attente de traitement
                smsRecus = GetSmsRecus((int)EnumProjet.BirdyBox);

                // traitement des sms
                smsRecus = BirdyHelper.SendListSms(smsRecus);

                // on retourne le nombre de sms traités
                //List<SmsReception> r = smsRecus.Where(s => s.DateLecture.HasValue).ToList();
                //if (r != null)
                //    nbSms = r.Count;
                nbSms = smsRecus.Where(s => s.DateLecture.HasValue).Count();

                if (nbSms > 0)
                    SmsReceptionHelper.TraitementEffectue(smsRecus);
                LogHelper.Trace("[BIRDY] "+nbSms + " sms envoyés", LogHelper.EnumCategorie.Information);

            }
            catch (Exception ex)
            {
                LogHelper.Trace("BirdyEnvoiPlateformeIp", ex, LogHelper.EnumCategorie.Erreur);
            }

            return nbSms;
        }
    }
}

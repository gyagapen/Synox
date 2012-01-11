using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Synox.Services.ServiceSMS.Entity;
using Synox.Services.ServiceSMS.Donnees;

namespace Synox.Services.ServiceSMS.Helpers
{
    /// <summary>
    /// Classe de méthodes Sms
    /// </summary>
    public class SmsHelper
    {

        /// <summary>
        ///  en deuxieme recours, si il y a un probleme avec la base de données on écrit le sms dns un fichier texte
        /// </summary>
        /// <param name="commande"></param>
        internal static void EcritSmsErreur(string commande)
        {
            try
            {
                StreamWriter writer = new StreamWriter(new FileStream(EnvironmentApplicationHelper.FichierSmsErreur, FileMode.Append, FileAccess.Write));
                writer.WriteLine("----------------------------------------------------------------");
                writer.WriteLine(commande);
                writer.Close();
                writer.Dispose();
            }
            catch { } // si encore une erreur - tant pis
        }

        /// <summary>
        /// Recupere la liste des sms à envoyer, les transmet au modem et enregistre leur date d'envoi en base
        /// </summary>
        /// <returns></returns>
        public static int SendSmsEnAttente()
        {
            List<Sms> smsList;
            try
            {
                smsList = GetSms();
                RouteurSmsHelper.SendHttpSms(smsList);
                SaveEnvoi(smsList.Where(l => l.DateEnvoi.HasValue).ToList());

                return smsList.Where(l => l.DateEnvoi.HasValue).Count();
            }
            catch (Exception ex) { LogHelper.Trace("SendSms", ex, LogHelper.EnumCategorie.Erreur); throw; }
        }

        /// <summary>
        /// Met a jour le statut du message
        /// </summary>
        /// <returns></returns>
        public static int UpdateStatusSmsEnAttente()
        {
            List<Sms> smsList;
            try
            {
                smsList = GetSmsInProgress();
                RouteurSmsHelper.QueryHttpSms(smsList);
                SaveEnvoi(smsList);


                return smsList.Where(l => l.DateEnvoi.HasValue).Count();
            }
            catch (Exception ex) { LogHelper.Trace("SendSms", ex, LogHelper.EnumCategorie.Erreur); throw; }
        }


        #region Accès Données
        /// <summary>
        /// Enregistre les dates d'envoi des SMS
        /// </summary>
        /// <param name="list"></param>
        private static void SaveEnvoi(List<Sms> list)
        {
            #region ADO
            string procedureEnregistrement = Synox.Services.ServiceSMS.Entity.Properties.Resources.SP_SmsEnvoyes;
            Hashtable parametres = new Hashtable();
            SqlServer server;

            try
            {
                server = new SqlServer(EnvironmentApplicationHelper.ConnectionString);
                foreach (Sms sms in list)
                {
                    parametres.Clear();
                    parametres.Add("@Id", sms.Id);
                    parametres.Add("@DateEnvoi", sms.DateEnvoi);
                    parametres.Add("@RouteurMsgId", sms.RouteurMsgId);
                    if (sms.SmsStatut != null)
                        parametres.Add("@Statut", sms.SmsStatut.Nom);

                    server.ExecuteSp(procedureEnregistrement, parametres);
                }
                server.Close();
            }
            catch (System.Data.SqlClient.SqlException dbe)
            {
                LogHelper.Trace(string.Format("une erreur s'est produit sur le serveur '{0}' dans la procédure '{1}' à la ligne {2}", dbe.Server, dbe.Procedure, dbe.LineNumber), LogHelper.EnumCategorie.Erreur);
                throw;
            }
            #endregion

            #region entity
            //Sms smsServeur;
            //Entity.Entities dataContext = null;
            //try
            //{
            //    dataContext = new Entity.Entities();
            //    foreach (Sms sms in list)
            //    {
            //        smsServeur = dataContext.Sms
            //            .Where(d => d.Id == sms.Id)
            //            .FirstOrDefault();
            //        smsServeur.DateEnvoi = sms.DateEnvoi;
            //        smsServeur.RouteurMsgId = sms.RouteurMsgId;
            //    }
            //    dataContext.SaveChanges();
            //}
            //catch (Exception ex) { LogHelper.Trace("SaveEnvoi", ex, LogHelper.EnumCategorie.Erreur); throw; }
            //finally
            //{
            //    if (dataContext != null)
            //    {
            //        dataContext.Dispose();
            //    }
            //}
            #endregion
        }

        /// <summary>
        /// Recupere les SMS du routeur SMS en attente d'envoi
        /// </summary>
        /// <returns></returns>
        public static List<Sms> GetSms()
        {
            List<Sms> smsList;
            Entity.Entities dataContext = null;
            try
            {
                dataContext = new Entity.Entities();
                smsList = dataContext.Sms
                    .Include("SmsStatut")
                    .Include("Projet")
                    .Where(d => (d.DateEnvoi == null || d.SmsStatut.Id == null)
                    && d.Projet.Id == (int)EnumProjet.RouteurSms)
                    .ToList();

                return smsList;
            }
            catch (Exception ex) { LogHelper.Trace("GetSms", ex, LogHelper.EnumCategorie.Erreur); throw; }
            finally
            {
                if (dataContext != null)
                {
                    dataContext.Dispose();
                }
            }
        }

        private static List<Sms> GetSmsInProgress()
        {
            List<Sms> smsList;
            Entity.Entities dataContext = null;
            try
            {
                dataContext = new Entity.Entities();
                smsList = dataContext.Sms
                    .Include("SmsStatut")
                    .Include("Projet")
                    .Where(d => d.RouteurMsgId != null && d.SmsStatut.Id > 3
                    && d.Projet.Id == (int)EnumProjet.RouteurSms)
                    .ToList();

                return smsList;
            }
            catch (Exception ex) { LogHelper.Trace("GetSmsInProgress", ex, LogHelper.EnumCategorie.Erreur); throw; }
            finally
            {
                if (dataContext != null)
                {
                    dataContext.Dispose();
                }
            }
        }

        /// <summary>
        /// Recupere les SMS du routeur SMS en attente d'envoi
        /// </summary>
        /// <returns></returns>
        public static List<Sms> GetSmsEnAttente()
        {
            List<Sms> smsList;
            Entity.Entities dataContext = null;
            try
            {
                dataContext = new Entity.Entities();
                smsList = dataContext.Sms
                    .Include("SmsStatut")
                    .Include("Projet")
                    .Where(d => d.DateEnvoi == null || d.SmsStatut.Id == null)
                    .Take(1000)
                    .ToList();

                return smsList;


            }
            catch (Exception ex) { LogHelper.Trace("GetSmsEnAttente", ex, LogHelper.EnumCategorie.Erreur); throw; }
            finally
            {
                if (dataContext != null)
                {
                    dataContext.Dispose();
                }
            }
        }

        /// <summary>
        /// Recuperation des sms en erreur
        /// </summary>
        /// <returns></returns>
        public static List<Sms> GetSmsErreur()
        {
            List<Sms> smsList;
            Entity.Entities dataContext = null;
            try
            {
                dataContext = new Entity.Entities();
                smsList = dataContext.Sms
                    .Include("SmsStatut")
                    .Include("Projet")
                    .Where(d => d.SmsStatut.Id == 2 || d.SmsStatut.Id == 3)
                    .Take(1000)
                    .ToList();

                return smsList;


            }
            catch (Exception ex) { LogHelper.Trace("GetSmsErreur", ex, LogHelper.EnumCategorie.Erreur); throw; }
            finally
            {
                if (dataContext != null)
                {
                    dataContext.Dispose();
                }
            }
        }
        public static List<Sms> GetSmsEnvoyes()
        {
            List<Sms> smsList;
            Entity.Entities dataContext = null;
            try
            {
                dataContext = new Entity.Entities();
                smsList = dataContext.Sms
                    .Include("Projet")
                    .Include("SmsStatut")
                    .Where(d => d.DateEnvoi != null)
                    .Take(1000)
                    .ToList();

                return smsList;


            }
            catch (Exception ex) { LogHelper.Trace("GetSmsEnvoyes", ex, LogHelper.EnumCategorie.Erreur); throw; }
            finally
            {
                if (dataContext != null)
                {
                    dataContext.Dispose();
                }
            }
        }

        /// <summary>
        /// Enregistre le sms en BDD pour qu'il soit envoyé dans la minute qui suit
        /// </summary>
        /// <param name="sms"></param>
        /// <returns></returns>
        public static void SaveSmsAEnvoyer(string numeroGsm, string message)
        {
            string procedureEnregistrement = Synox.Services.ServiceSMS.Entity.Properties.Resources.SP_NouveauSmsAEnvoyer;
            Hashtable parametres = new Hashtable();
            SqlServer server = null;

            try
            {
                if (string.IsNullOrEmpty(numeroGsm)) throw new Exception("'numeroGsm' est inccorect");
                if (string.IsNullOrEmpty(message)) throw new Exception("'message' est inccorect");

                LogHelper.Trace(string.Format("Sms : [{0}] => '{1}'", numeroGsm, message), LogHelper.EnumCategorie.Information);
                parametres.Add("@NumeroGsm", numeroGsm);
                parametres.Add("@Message", message);
                parametres.Add("@DateDemande", DateTime.Now);

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
        }

        /// <summary>
        /// Sauvegarde les sms traités
        /// </summary>
        /// <param name="smsList"></param>
        public static void SaveDone(List<Sms> smsList)
        {
            Entity.Entities dataContext;
            dataContext = new Entity.Entities();
            Sms smsServeur;
            foreach (Sms sms in smsList.Where(s => s.DateEnvoi != null).ToList())
            {

                smsServeur = dataContext.Sms
                    .Where(s => s.Id == sms.Id)
                    .FirstOrDefault();

                if (smsServeur != null)
                {
                    smsServeur.DateEnvoi = sms.DateEnvoi;
                    smsServeur.SmsStatutId = 1;
                }
            }
            dataContext.SaveChanges();
        }
        /// <summary>
        /// Enregistre un nouveau sms en base
        /// </summary>
        /// <param name="numeroGsm"></param>
        /// <param name="message"></param>
        public static void Save(string numeroGsm, string message)
        {
            Entity.Entities dataContext;
            Sms sms;
            dataContext = new Entity.Entities();

            sms = new Sms();
            sms.NumeroGsm = numeroGsm.Replace(" ", "").Trim();
            sms.Message = message;
            sms.DateDemande = DateTime.Now;

            dataContext.AddToSms(sms);
            dataContext.SaveChanges();
        }
        #endregion




        /// <summary>
        /// Affecte au SMS un projet : Chaque SMS doit être traité spécifiquement en fonction du projet auquel il appartient
        /// </summary>
        public static void AffecteProjet()
        {
            List<Sms> smsList;
            Entity.Entities dataContext = null;
            bool alerteWindowsMobile = false;
            try
            {
                dataContext = new Entity.Entities();
                smsList = dataContext.Sms
                    .Where(d => d.ProjetId == null)
                    .ToList();

                foreach (Sms sms in smsList)
                {
                    if (sms.Message.Length > 60)
                    {
                        sms.ProjetId = (int)EnumProjet.WindowsMobile;
                        alerteWindowsMobile = true;
                    }
                    else
                        sms.ProjetId = (int)EnumProjet.RouteurSms;
                }

                dataContext.SaveChanges();

                // si alerte Windows Mobile: envoyé une trame d'avertissement au service mobile
                if (alerteWindowsMobile)
                {
                    Net.CommunicationServicesSync mobile = new Net.CommunicationServicesSync();
                    mobile.ConnexionServeur("192.168.7.12", 2011, false);
                    mobile.EnvoiEnString("SendSMS");
                    mobile.Disconnect();
                }


            }
            catch (System.Net.Sockets.SocketException sex) { LogHelper.Trace("SMS.AffecteProjet", sex, LogHelper.EnumCategorie.Alerte); throw; }
            catch (Exception ex) { LogHelper.Trace("SMS.AffecteProjet", ex, LogHelper.EnumCategorie.Erreur); throw; }
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;

using Synox.Services.ServiceSMS.Entity;
using Synox.Services.ServiceSMS.Donnees;

namespace Synox.Services.ServiceSMS.Helpers
{
    public class RouteurSmsHelper
    {

        #region MODEM
        /// <summary>
        /// Envoi les sms au modem
        /// </summary>
        /// <param name="smsList"></param>
        /// <returns></returns>
        public static List<Sms> SendSmsToModem(List<Sms> smsList)
        {
            int idMessage = -1;
            string trame;
            string telephone;

            if (smsList == null || smsList.Count == 0)
                return new List<Sms>();

            Net.CommunicationServicesSync service = new Net.CommunicationServicesSync();
            service.ConnexionServeur(EnvironmentApplicationHelper.SmsServer, EnvironmentApplicationHelper.SmsServerPort);

            service.EnvoiEnString("AT+CMGF=1\r\n");
            trame = service.RecoitEnString();

            foreach (Sms sms in smsList)
            {
                telephone = sms.NumeroGsm;
                trame = string.Format("AT+CMGW={0}\r\n", telephone);
                byte[] ctrlZ = new byte[3] { 26, 13, 10 };// ctrlZ[0] = 26;
                service.EnvoiEnString(trame);
                trame = service.RecoitEnString();
                service.EnvoiEnString(sms.Message, ctrlZ);
                trame = service.RecoitEnString();


                if (trame.Contains("OK")) // "\r\n+CMGS: 6\r\n\r\nOK\r\n"
                {

                    //if (trame.Replace("\r\n", "").Trim().Equals("OK"))
                    //    trame = service.RecoitEnString();

                    idMessage = RecupereIdMessage(trame);

                    sms.RouteurMsgId = idMessage;

                    if (sms.RouteurMsgId > 0)
                    {
                        trame = string.Format("AT+CMSS={0}\r\n", sms.RouteurMsgId);
                        service.EnvoiEnString(trame);
                        trame = service.RecoitEnString();
                        Console.WriteLine(">>>>>>>>>>" + trame);
                        sms.DateEnvoi = DateTime.Now;
                    }
                }
                LogHelper.Trace(string.Format("Sms : [{0}] => '{1}'", sms.NumeroGsm, sms.Message), LogHelper.EnumCategorie.Information);

            }
            System.Threading.Thread.Sleep(2000);
            foreach (Sms sms in smsList)
            {
                if (sms.RouteurMsgId >= 0)
                {

                    trame = string.Format("AT+CMGR={0}\r\n", sms.RouteurMsgId);
                    service.EnvoiEnString(trame);
                    trame = service.RecoitEnString();

                    //if(trame.Replace("\r\n", "").Trim().Equals("OK"))
                    //    trame = service.RecoitEnString();

                    if (trame.Contains("ERROR"))
                    {
                        sms.SmsStatut = new SmsStatut();
                        sms.SmsStatut.Nom = "ERROR";
                    }
                    else
                    {
                        string statut = RecupereStatus(trame);
                        if (!string.IsNullOrEmpty(statut))
                        {
                            sms.SmsStatut = new SmsStatut();
                            sms.SmsStatut.Nom = statut;
                        }
                    }
                }
            }

            // suppression de tous les messages envoyés
            // 0: suppression des du message à l'index renseigné
            // 1: Suppression des messsages Lus (READ)
            // 2: Suppression des messages Lus et envoyés (READ and SENT)
            // 3: Suppression des messages Lus, envoyés et non envoyés (READ, SENT and UNSENT)
            // 4: suppression de tous les messages
            trame = "AT+CMGD=0,2\r\n";
            service.EnvoiEnString(trame);

            service.Disconnect();

            return smsList;
        }

        public static List<Sms> SendSmsPduToModem(List<Sms> smsList)
        {
            int idMessage = -1;
            string trame;
            string telephone;
            byte[] pdu;
            byte[] ctrlZ = new byte[3] { 26, 13, 10 };// ctrlZ[0] = 26;

            if (smsList == null || smsList.Count == 0)
                return new List<Sms>();

            Net.CommunicationServicesSync service = new Net.CommunicationServicesSync();
            service.ConnexionServeur(EnvironmentApplicationHelper.SmsServer, EnvironmentApplicationHelper.SmsServerPort);
            // Format des messages : 1 pour texte et 0 pour pdu
            service.EnvoiEnString("AT+CMGF=0\r\n");
            trame = service.RecoitEnString();
            foreach (Sms sms in smsList)
            {
                telephone = sms.NumeroGsm;
                sms.Message = "07913336241677f111000b916407498653f300f6002930000000000066010b000001001b04343731314e79e06c4e79e06c0831323334353637380000126701";
                pdu = PduHelper.HexaStringToByteArray("07913336241677f111000b916407498653f300f6002930000000000066010b000001001b04343731314e79e06c4e79e06c0831323334353637380000126701");

                trame = string.Format("AT+CMGW={0}\r\n", pdu.Length);
                service.EnvoiEnString(trame);
                trame = service.RecoitEnString();
                service.Send(pdu);
                service.Send(ctrlZ);
                trame = service.RecoitEnString();


                if (trame.Contains("OK")) // "\r\n+CMGS: 6\r\n\r\nOK\r\n"
                {

                    //if (trame.Replace("\r\n", "").Trim().Equals("OK"))
                    //    trame = service.RecoitEnString();

                    idMessage = RecupereIdMessage(trame);

                    sms.RouteurMsgId = idMessage;

                    if (sms.RouteurMsgId > 0)
                    {
                        trame = string.Format("AT+CMSS={0}\r\n", sms.RouteurMsgId);
                        service.EnvoiEnString(trame);
                        trame = service.RecoitEnString();
                        Console.WriteLine(">>>>>>>>>>" + trame);
                        sms.DateEnvoi = DateTime.Now;
                    }
                }
                LogHelper.Trace(string.Format("Sms : [{0}] => '{1}'", sms.NumeroGsm, sms.Message), LogHelper.EnumCategorie.Information);

            }
            service.EnvoiEnString("AT+CMGF=1\r\n");
            trame = service.RecoitEnString();
            System.Threading.Thread.Sleep(2000);
            foreach (Sms sms in smsList)
            {
                if (sms.RouteurMsgId >= 0)
                {

                    trame = string.Format("AT+CMGR={0}\r\n", sms.RouteurMsgId);
                    service.EnvoiEnString(trame);
                    trame = service.RecoitEnString();

                    //if(trame.Replace("\r\n", "").Trim().Equals("OK"))
                    //    trame = service.RecoitEnString();

                    if (trame.Contains("ERROR"))
                    {
                        sms.SmsStatut = new SmsStatut();
                        sms.SmsStatut.Nom = "ERROR";
                    }
                    else
                    {
                        string statut = RecupereStatus(trame);
                        if (!string.IsNullOrEmpty(statut))
                        {
                            sms.SmsStatut = new SmsStatut();
                            sms.SmsStatut.Nom = statut;
                        }
                    }
                }
            }

            // suppression de tous les messages envoyés
            // 0: suppression des du message à l'index renseigné
            // 1: Suppression des messsages Lus (READ)
            // 2: Suppression des messages Lus et envoyés (READ and SENT)
            // 3: Suppression des messages Lus, envoyés et non envoyés (READ, SENT and UNSENT)
            // 4: suppression de tous les messages
            trame = "AT+CMGD=0,2\r\n";
            service.EnvoiEnString(trame);

            service.Disconnect();

            return smsList;
        }
        #endregion


        #region Decodage des trames
        /// <summary>
        /// Recupere l'identifiant du message
        /// </summary>
        /// <param name="trame"></param>
        /// <returns></returns>
        public static int RecupereIdMessage(string trame)
        {
            try
            {
                string message = trame.Split("\r\n".ToArray(), StringSplitOptions.RemoveEmptyEntries)[0];
                string id = message.Split(" ".ToArray(), StringSplitOptions.RemoveEmptyEntries)[1].Trim();
                return Convert.ToInt32(id);
            }
            catch { return -1; }
        }

        /// <summary>
        /// Recupere le statut de retour de trame CMGR du modem
        /// </summary>
        /// <param name="trame"></param>
        /// <returns></returns>
        internal static string RecupereStatus(string trame)
        {
            try
            {   // +CMGR: "STO SENT","+33663429874",,145,1,0,0,,"+33609001390",145,17
                string message = trame.Split("\r\n".ToArray(), StringSplitOptions.RemoveEmptyEntries)[0]; // \r\nOK\r\n+CMGR: "STO SENT","+33663429874"
                if (message.Equals("OK"))
                    message = trame.Split("\r\n".ToArray(), StringSplitOptions.RemoveEmptyEntries)[1]; // +CMGR: "STO SENT","+33663429874"
                message = message.Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries)[0].Trim();  // +CMGR: "STO SENT"
                message = message.Split(":".ToArray(), StringSplitOptions.RemoveEmptyEntries)[1].Replace("\"", "").Trim(); // STO SENT
                return message;
            }
            catch { return null; }
        }

        /// <summary>
        /// Récupération du sms dans la trame reçue
        /// </summary>
        /// <param name="trameTcp"></param>
        /// <returns></returns>
        internal static SmsReception DeserializeSms(string trameTcp)
        {
            SmsReception sms = null;
            StreamReader reader;
            MemoryStream ms;
            try
            {
                byte[] byteArray = Encoding.Default.GetBytes(trameTcp);
                sms = new SmsReception();
                ms = new MemoryStream(byteArray);
                reader = new StreamReader(ms);
                reader.ReadLine(); // From: MultiModem iSMS/1.30
                reader.ReadLine(); // Authentication: OFF
                reader.ReadLine(); // Data-Length: 48
                reader.ReadLine(); // 
                reader.ReadLine(); // 
                reader.ReadLine(); // 1:33623060456
                sms.NumeroGsm = reader.ReadLine(); // +33617841310
                sms.DateReception = Convert.ToDateTime("20" + reader.ReadLine() + " " + reader.ReadLine(), EnvironmentApplicationHelper.CultureFr); // 11/01/13 09:36:33
                sms.Message = reader.ReadToEnd();
                reader.Close();
            }
            catch (Exception ex)
            {
                LogHelper.Trace("DeserializeSms", ex, LogHelper.EnumCategorie.Erreur);
                throw;
            }
            return sms;
        }

        /// <summary>
        /// Envoi de la liste des sms en mode HTTP
        /// </summary>
        /// <param name="smsToSend"></param>
        /// <returns></returns>
        public static void SendHttpSms(List<Sms> smsToSend)
        {
            foreach (Sms sms in smsToSend)
            {
                try
                {
                    string id = SendHttpSms(sms.NumeroGsm, sms.Message);
                    sms.DateEnvoi = DateTime.Now;
                    sms.RouteurMsgId = Convert.ToInt32(id.Replace("ID:", ""));
                }
                catch(Exception ex)
                {
                    LogHelper.Trace("SendHttpSms(liste)", ex, LogHelper.EnumCategorie.Erreur);
                }
            }
            System.Threading.Thread.Sleep(2000);

            foreach (Sms sms in smsToSend)
            {
                if (!sms.RouteurMsgId.HasValue) continue;

                string status = QueryHttpSms(sms.RouteurMsgId.Value);
                status = status.Replace(string.Format("ID: {0} Status: ", sms.RouteurMsgId), "").Trim();

                sms.SmsStatut = new SmsStatut();
                switch (status)
                {
                    case "0":
                        sms.SmsStatut.Nom = "Done";
                        break;
                    case "1":
                        sms.SmsStatut.Nom = "Done with error";
                        break;
                    case "2":
                        sms.SmsStatut.Nom = "In Progress";
                        break;
                    case "3":
                        sms.SmsStatut.Nom = "Request Received";
                        break;
                    case "4":
                        sms.SmsStatut.Nom = "Error";
                        break;
                    case "5":
                        sms.SmsStatut.Nom = "Message ID Not Found";
                        break;
                }
            }
        }

        /// <summary>
        /// Envoi des SMS un par un en mode HTTP
        /// </summary>
        /// <param name="numeroGsm"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string SendHttpSms(string numeroGsm, string message)
        {
            WebRequest request = null;
            WebResponse response = null;

            string enc = "&enc=3";           // 0=ASCII, 1=ASCII Extended, 2=Hexadecimal, 3=Decimal
            string priority = "&priority=2"; // 1=Low, 2=Normal, 3=High
            string urlUnicode = @"http://" + EnvironmentApplicationHelper.SmsServer + ":81/sendmsg?user=m2m&passwd=m2mfrance&cat=1&enc=3&priority=2&to={0}&text={1}";
            string urlAscii = @"http://" + EnvironmentApplicationHelper.SmsServer + ":81/sendmsg?user=m2m&passwd=m2mfrance&cat=1&enc=0&priority=2&to={0}&text={1}";
            // string urlTest = "http://192.168.7.253:81/sendmsg?user=m2m&passwd=m2mfrance&cat=1" + enc + priority + "&to=+33663429874&text=%3C%54%72%78%26%3E%33%34%35%33%34%32%35%32%3C%5C%54%72%78%26%3E";
            string urlTest = "http://192.168.7.253:81/sendmsg?user=m2m&passwd=m2mfrance&cat=1" + enc + priority + "&to=+33663429874&text=60;84;114;120;38;62;51;52;60;92;84;114;120;38";
            
            string reponse = null;
            string url;
            try
            {
                // 
                if (message.Contains("\\"))
                    url = string.Format(urlUnicode, numeroGsm, ConvertMessageToDecimal(message));
                else
                    url = string.Format(urlAscii, numeroGsm, ConvertToHexa(message));

                request = WebRequest.Create(url);
                response = request.GetResponse();

                // Read the text from the response stream.
                using (StreamReader r = new StreamReader(response.GetResponseStream()))
                {
                    reponse = r.ReadToEnd();
                    r.Close();
                }

            }
            catch (Exception ex)
            {
                LogHelper.Trace("SendHttpSms", ex, LogHelper.EnumCategorie.Erreur);
                throw;
            }
            return reponse;
        }

        /// <summary>
        /// Interroge le routeur pour savoir si le message a bien été envoyé
        /// </summary>
        /// <param name="smsToSend"></param>
        public static void QueryHttpSms(List<Sms> smsToSend)
        {
            foreach (Sms sms in smsToSend)
            {
                try
                {
                    if (!sms.RouteurMsgId.HasValue) continue;

                    string status = QueryHttpSms(sms.RouteurMsgId.Value);
                    status = status.Replace(string.Format("ID: {0} Status: ", sms.RouteurMsgId), "").Trim();

                    sms.SmsStatut = new SmsStatut();
                    switch (status)
                    {
                        case "0":
                            sms.SmsStatut.Nom = "Done";
                            break;
                        case "1":
                            sms.SmsStatut.Nom = "Done with error";
                            break;
                        case "2":
                            sms.SmsStatut.Nom = "In Progress";
                            break;
                        case "3":
                            sms.SmsStatut.Nom = "Request Received";
                            break;
                        case "4":
                            sms.SmsStatut.Nom = "Error";
                            break;
                        //case "5":
                        //    sms.SmsStatut.Nom = "Message ID Not Found";
                        //    break;
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.Trace("QueryHttpSms", ex, LogHelper.EnumCategorie.Erreur);
                }
            }
        }

        /// <summary>
        /// Recupere le code status au format : "ID: 16 Status: 0"
        /// </summary>
        /// <param name="numeroGsm"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string QueryHttpSms(int id)
        {
            WebRequest request = null;
            WebResponse response = null;

            string url = "http://" + EnvironmentApplicationHelper.SmsServer + ":81/querymsg?user=m2m&passwd=m2mfrance&apimsgid=" + id;

            string reponse = null;
            try
            {
                request = WebRequest.Create(url);
                response = request.GetResponse();

                // Read the text from the response stream.
                using (StreamReader r = new StreamReader(response.GetResponseStream()))
                {
                    reponse = r.ReadToEnd();
                    r.Close();
                }

            }
            catch (Exception ex)
            {
                LogHelper.Trace("QueryHttpSms", ex, LogHelper.EnumCategorie.Erreur);
                throw;
            }
            return reponse;
        }
        #endregion


        /// <summary>
        /// Convertit le message texte en Décimal séparé de points virgules
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static string ConvertMessageToDecimal(string message)
        {
            byte[] buf = Encoding.ASCII.GetBytes(message);
            string trame = string.Empty;
            for (int i = 0; i < buf.Length; i++)
            {
                trame += Convert.ToString((int)buf[i])+";";
            }
            trame = trame.Substring(0, trame.Length - 1);
            return trame;
        }
        /// <summary>
        /// Convertit le message texte en Hexa séparé de %
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string ConvertToHexa(string message)
        {
            byte[] value = Encoding.ASCII.GetBytes(message);

            string s = "";
            int val = 0;
            for (int i = 0; i < value.Length; i++)
            {
                val = value[i];
                s += "%" + val.ToString("X").PadLeft(2, '0');
            }
            return s;
        }
    }
}

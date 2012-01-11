using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Synox.Services.ServiceSMS.Entity;

namespace Synox.Services.ServiceSMS.Helpers
{
    class BirdyHelper
    {
        internal static List<SmsReception> SendListSms(List<SmsReception> smsRecus)
        {
            foreach (SmsReception sms in smsRecus)
            {
                try
                {
                    bool echecTransmission = true;
                    BirdyBox transmetteur = DeserializeSms(sms.Message, sms.NumeroGsm);
                    if (transmetteur != null)
                    {
                        string trameBirdy = ConvertToTrame(transmetteur);

                        LogHelper.Trace(trameBirdy, LogHelper.EnumCategorie.Information);

                        Net.CommunicationServicesSync serveur = new Net.CommunicationServicesSync();
                        #region connexion
                        try
                        {
                            //transmetteur.ServerIpOrDns = "62.100.154.189";
                            //transmetteur.ServerPort = 2012;
                            serveur.ConnexionServeur(transmetteur.ServerIpOrDns, transmetteur.ServerPort, !Synox.Helpers.NetworkHelper.IsIp(transmetteur.ServerIpOrDns));
                            echecTransmission = false;
                        }
                        catch
                        {
                            LogHelper.Trace(string.Format("Echec de connexion au serveur {0}:{1}", transmetteur.ServerIpOrDns, transmetteur.ServerPort), LogHelper.EnumCategorie.Erreur);
                            //try
                            //{
                            //    serveur.ConnexionServeur(transmetteur.BackupIpOrDns, transmetteur.BackupPort, !Synox.Helpers.NetworkHelper.IsIp(transmetteur.ServerIpOrDns));
                            //    echecTransmission = false;
                            //}
                            //catch
                            //{
                            //    LogHelper.Trace(string.Format("Echec de connexion au serveur Backup {0}:{1}", transmetteur.BackupIpOrDns, transmetteur.BackupPort), LogHelper.EnumCategorie.Erreur);
                            //}
                        }
                        #endregion
                        string etat = "KO";
                        if (!echecTransmission)
                        {
                            try
                            {
                                byte[] trame = Encoding.Default.GetBytes(trameBirdy);
                                // envoi du SMS à la plateforme
                                int nbOctets = serveur.Send(trame);
                                etat = "trame envoyée mais Echec ACK";
                                byte[] ack = serveur.Receive(50);
                                etat = "trame envoyée et ACK recu";
                                sms.DateLecture = DateTime.Now;
                                sms.Commentaire = "[" + Encoding.Default.GetString(ack) + "]";
                                LogHelper.Trace("Trame envoyée", LogHelper.EnumCategorie.Information);
                            }
                            catch (Exception tex)
                            {
                                LogHelper.Trace("Echec d'échange de trame: " + etat + "\r\n", tex, LogHelper.EnumCategorie.Erreur);
                            }
                            serveur.Disconnect();
                        }
                    }
                    else
                    {
                        sms.DateLecture = DateTime.Now;
                        sms.Commentaire = "ERREUR: Le sms n'a pu être deserialisé";
                        LogHelper.Trace("Le sms n'a pu être deserialisé", LogHelper.EnumCategorie.Erreur);
                    }
                }
                catch (Exception ex)
                {
                    sms.DateLecture = DateTime.Now;
                    sms.Commentaire = "ERREUR: " + ex.Message;
                    LogHelper.Trace("Erreur Traitement Sms BirdyBox", ex, LogHelper.EnumCategorie.Erreur);
                }
            }
            return smsRecus;
        }

        private static string ConvertToTrame(BirdyBox transmetteur)
        {
            string trameMasque = "<Body&><Trx>{0}<\\Trx><Sim>{1}<\\Sim><Nb&>{2}<\\Nb&>{3}{4}{5}{6}{7}<Tim&>{8}<\\Tim&><Sn>{9}<\\Sn><Imei>000000000000000<\\Imei><T°24>x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x<\\T°24><QGsm>x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x<\\QGsm><Volt>x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x<\\Volt><Mod>Bkp<\\Mod><Bts>0000,0000,0000,0000,0000<\\Bts><Origin>BKPSMS<\\Origin><Caller>{10}<\\Caller><Sw>00.000<\\Sw><\\Body&>";
            string tramePlateformeIp = string.Format(trameMasque, 
                transmetteur.NumeroUid, // <Trx>
                transmetteur.NumeroGsm, // <Sim>
                transmetteur.NbAlarme.ToString().PadLeft(2, '0'), // <Nb&>
                transmetteur.Alarme01, // <&01>
                transmetteur.Alarme02, // <&02>
                transmetteur.Alarme03, // <&03>
                transmetteur.Alarme04, // <&04>
                transmetteur.Alarme05, // <&05>
                transmetteur.Tim, // <&Tim>
                transmetteur.Sn, // <Sn>
                EnvironmentApplicationHelper.NumeroGsmRouteurSms
                );
            //return "<Body&><Trx>44444444<\\Trx><Sim>0601270422<\\Sim><Nb&>05<\\Nb&><&01>13010022709171<\\&01><&02>30520022709171<\\&02><&03>60220022709171<\\&03><&04>60220022709171<\\&04><&05>60220022709171<\\&05><Tim&>101103120900<\\Tim&><Sn>09171227001<\\Sn><Imei>000000000000000<\\Imei><T°24>x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x<\\T°24><QGsm>x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x<\\QGsm><Volt>x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x,x<\\Volt><Mod>Bkp<\\Mod><Bts>0000,0000,0000,0000,0000<\\Bts><Origin>BKPSMS<\\Origin><Caller>+33630633722<\\Caller><Sw>00.000<\\Sw><\\Body&>";
            return tramePlateformeIp;
        }

        private static BirdyBox DeserializeSms(string messageSms, string numeroGsm)
        {
            BirdyBox transmission = null;
            string[] lignes;
            try
            {
                transmission = new  BirdyBox();
                transmission.NumeroGsm = numeroGsm;
                messageSms = messageSms.Replace("\r", "");
                lignes = messageSms.Split("<".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string ligne in lignes)
                {
                    if (ligne.IndexOf('>')>0)
                    switch (ligne.Substring(0, ligne.IndexOf('>')))
                    {
                        case "Trx":
                            transmission.NumeroUid = ligne.Substring(ligne.IndexOf('>')+1).Trim();
                            break;
                        case "Tim":
                            transmission.Tim = ligne.Substring(ligne.IndexOf('>') + 1).Trim();
                            break;
                        case "Sn":
                            transmission.Sn = ligne.Substring(ligne.IndexOf('>') + 1).Trim();
                            break;
                        case "IP":
                            transmission.ServerIpOrDns = ligne.Substring(ligne.IndexOf('>') + 1, 12).Trim();
                            transmission.ServerIpOrDns = TransformeIp(transmission.ServerIpOrDns);
                            transmission.ServerPort = Convert.ToInt32(ligne.Substring(ligne.IndexOf('>')+1+12).Trim());

                            break;
                        case "01":
                            transmission.Alarme01 = "<&01>"+ligne.Substring(ligne.IndexOf('>') + 1).Trim()+"<\\&01>";
                            transmission.NbAlarme++;
                            break;
                        case "02":
                            transmission.Alarme02 = "<&02>" + ligne.Substring(ligne.IndexOf('>') + 1).Trim() + "<\\&02>";
                            transmission.NbAlarme++;
                            break;
                        case "03":
                            transmission.Alarme03 = "<&03>" + ligne.Substring(ligne.IndexOf('>') + 1).Trim() + "<\\&03>";
                            transmission.NbAlarme++;
                            break;
                        case "04":
                            transmission.Alarme04 = "<&04>" + ligne.Substring(ligne.IndexOf('>') + 1).Trim() + "<\\&04>";
                            transmission.NbAlarme++;
                            break;
                        case "05":
                            transmission.Alarme05 = "<&05>" + ligne.Substring(ligne.IndexOf('>') + 1).Trim() + "<\\&05>";
                            transmission.NbAlarme++;
                            break;
                    }
                }
                // controle

                // controle
                if (string.IsNullOrEmpty(transmission.NumeroGsm))
                    throw new Exception("pas de gsm");
                //if (string.IsNullOrEmpty(transmission.Alarme01))
                //    throw new Exception("pas de alarme");
                if (string.IsNullOrEmpty(transmission.NumeroUid))
                    throw new Exception("pas de Trx");
                if (string.IsNullOrEmpty(transmission.ServerIpOrDns))
                    throw new Exception("pas de ServerIpOrDns");
                if (transmission.ServerPort == 0)
                    throw new Exception("pas de ServerPort");
                //if (string.IsNullOrEmpty(transmission.BackupIpOrDns))
                //    throw new Exception("pas de BackupIpOrDns");
                //if (transmission.BackupPort==0)
                //    throw new Exception("pas de BackupPort");
            }
            catch (Exception tex)
            {
                LogHelper.Trace("DeserializeSms(Message)", tex, LogHelper.EnumCategorie.Erreur);
                transmission = null;
            }


            return transmission;
        }

        private static string TransformeIp(string ipBirdy)
        {
            string ip = "";
            ip += Convert.ToInt32(ipBirdy.Substring(0, 3));
            ip += "." + Convert.ToInt32(ipBirdy.Substring(3, 3));
            ip += "." + Convert.ToInt32(ipBirdy.Substring(6, 3));
            ip += "." + Convert.ToInt32(ipBirdy.Substring(9, 3));
            return ip;
        }
    }
}

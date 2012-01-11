using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Synox.Services.ServiceSMS.Entity;
using Synox.Services.ServiceSMS.Entity.Helpers;

namespace Synox.Services.ServiceSMS.Helpers
{
    /// <summary>
    /// Class Solem 
    /// </summary>
    public class SolemHelper
    {


        public static byte[] ConvertToPacket(string messageSms)
        {
            return ConvertToPacket(DeserializeSms(messageSms));
        }

        public static Solem DeserializeSms(string messageSms, string numeroGsm=null)
        {
            Solem transmission = null;
            string[] lignes;
            try
            {
                transmission = new Solem();
                messageSms = messageSms.Replace("\n", "\r");
                lignes = messageSms.Split("\r".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string ligne in lignes)
                {
                    switch (ligne.Substring(0, 1))
                    {
                        case "G":
                            if (string.IsNullOrEmpty(numeroGsm))
                                transmission.NumeroGsm = ligne.Substring(1).Trim();
                            else
                                transmission.NumeroGsm = numeroGsm;
                            break;
                        case "T":
                            transmission.NumeroUid = ligne.Substring(1).Trim();
                            break;
                        case "A":
                            transmission.Alarme = ligne.Substring(1).Trim();
                            break;
                        case "F":
                            if (ligne.StartsWith("FIP") || ligne.StartsWith("FDN"))
                                transmission.ServerIpOrDns = ligne.Substring(3).Trim();
                            if (ligne.StartsWith("FPO"))
                                transmission.ServerPort = Convert.ToInt32(ligne.Substring(3).Trim());
                            if (ligne.StartsWith("FBIP") || ligne.StartsWith("FBDN"))
                                transmission.BackupIpOrDns = ligne.Substring(4).Trim();
                            if (ligne.StartsWith("FBPO"))
                            {
                                if (!string.IsNullOrEmpty(ligne.Substring(4).Trim()))
                                transmission.BackupPort = Convert.ToInt32(ligne.Substring(4).Trim());
                            }
                            break;
                    }
                }
                // controle

                // controle
                if (string.IsNullOrEmpty(transmission.NumeroGsm))
                    throw new Exception("pas de gsm");
                if (string.IsNullOrEmpty(transmission.Alarme))
                    throw new Exception("pas de alarme");
                if (string.IsNullOrEmpty(transmission.NumeroUid))
                    throw new Exception("pas de NumeroUid");
                if (string.IsNullOrEmpty(transmission.ServerIpOrDns))
                    throw new Exception("pas de ServerIpOrDns");
                if (transmission.ServerPort==0)
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

        public static byte[] ConvertToPacket(Solem transmetteur)
        {
            byte[] dataGlobal = null;

            try
            {
                //transmetteur.NumeroGsm = "+33622031566";
                string iccidNumber = SimHelper.GetICCIDFromGsmNumber(transmetteur.NumeroGsm);

                transmetteur.NumeroGsm = transmetteur.NumeroGsm.Replace("+33", "0");
                
                byte[] dataNumIMEI = null;
                byte[] dataEvtContactIdEtendu = null;
                byte[] dataCodePIN = null;
                byte[] dataNumIMSI = null;
                byte[] dataNumTelephoneGSM = null;
                byte[] dataNumTelephoneRTC = null;
                byte[] dataNumVersionFirmware = null;

                #region Alimentation des trames

                #region dataNumIMEI

                dataNumIMEI = new byte[10]; // Taille = Code Identifiant (1) + Données (8) + Checksum (1)
                dataNumIMEI[0] = ConvertFromHexa("01"); // Code Identifiant

                // Données
                string numIMEI = "3568960317092320";                     // ICCID ???
                dataNumIMEI[1] = ConvertFromHexa(numIMEI.Substring(0, 2));
                dataNumIMEI[2] = ConvertFromHexa(numIMEI.Substring(2, 2));
                dataNumIMEI[3] = ConvertFromHexa(numIMEI.Substring(4, 2));
                dataNumIMEI[4] = ConvertFromHexa(numIMEI.Substring(6, 2));
                dataNumIMEI[5] = ConvertFromHexa(numIMEI.Substring(8, 2));
                dataNumIMEI[6] = ConvertFromHexa(numIMEI.Substring(10, 2));
                dataNumIMEI[7] = ConvertFromHexa(numIMEI.Substring(12, 2));
                dataNumIMEI[8] = ConvertFromHexa(numIMEI.Substring(14, 2));

                dataNumIMEI[9] = GetChekSum(dataNumIMEI); // CheckSum

	            #endregion

                #region dataEvtContactIdEtendu

                dataEvtContactIdEtendu = new byte[13]; // Taille = Code Identifiant (1) + Données (11) + Checksum (1)
                dataEvtContactIdEtendu[0] = ConvertFromHexa("03"); // Code Identifiant

                // Données
                dataEvtContactIdEtendu[1] = ConvertFromHexa("01"); // Nombre d'évènements (1)

                string donneesEvt = String.Empty;
                donneesEvt += transmetteur.NumeroUid.Substring(4, 4); // ACCT (4 bytes de poids faible du N° Transmetteur)
                donneesEvt += "20"; // Message Type = Etendu (20)
                donneesEvt += "1"; // Qualificateur d'évènement = Open (1)
                donneesEvt += "099"; // Code d'évènement (099)
                donneesEvt += "00"; // N° de groupe (00?)
                donneesEvt += "000"; // N° de zone (000?)
                donneesEvt += transmetteur.NumeroUid.Substring(0, 4); // (4 bytes de poids fort du N° Transmetteur)

                donneesEvt += GetChekSumContactId(donneesEvt); //  IL MANQUE A CALCULER LE CHECKSUM DE donneesEvt

                byte[] dataEvtContactIdEtenduTemp = new byte[10];
                dataEvtContactIdEtenduTemp[0] = ConvertFromHexa(donneesEvt.Substring(0, 2));
                dataEvtContactIdEtenduTemp[1] = ConvertFromHexa(donneesEvt.Substring(2, 2));
                dataEvtContactIdEtenduTemp[2] = ConvertFromHexa(donneesEvt.Substring(4, 2));
                dataEvtContactIdEtenduTemp[3] = ConvertFromHexa(donneesEvt.Substring(6, 2));
                dataEvtContactIdEtenduTemp[4] = ConvertFromHexa(donneesEvt.Substring(8, 2));
                dataEvtContactIdEtenduTemp[5] = ConvertFromHexa(donneesEvt.Substring(10, 2));
                dataEvtContactIdEtenduTemp[6] = ConvertFromHexa(donneesEvt.Substring(12, 2));
                dataEvtContactIdEtenduTemp[7] = ConvertFromHexa(donneesEvt.Substring(14, 2));
                dataEvtContactIdEtenduTemp[8] = ConvertFromHexa(donneesEvt.Substring(16, 2));
                dataEvtContactIdEtenduTemp[9] = ConvertFromHexa(donneesEvt.Substring(18, 2));

                dataEvtContactIdEtendu = Merge(dataEvtContactIdEtenduTemp, dataEvtContactIdEtendu, 2);

                dataEvtContactIdEtendu[12] = GetChekSum(dataEvtContactIdEtendu); // CheckSum

                #endregion

                #region Code PIN
                dataCodePIN = new byte[4];
                dataCodePIN[0] = ConvertFromHexa("04");
                dataCodePIN[1] = ConvertFromHexa("00");
                dataCodePIN[2] = ConvertFromHexa("00");
                dataCodePIN[3] = GetChekSum(dataCodePIN);
                #endregion

                #region IMSI / ICCID
                dataNumIMSI = new byte[10];
                dataNumIMSI[0] = ConvertFromHexa("05");
                // 89 33       10 42 09 02 21 06 01 67
                //iccidNumber = "89331042100122313167";
                if (string.IsNullOrEmpty(iccidNumber))
                { iccidNumber = "FFFFFFFFFFFFFFFF";  }
                else
                    iccidNumber = iccidNumber.Substring(iccidNumber.Length - (8 * 2));

                dataNumIMSI[1] = ConvertFromHexa(iccidNumber.Substring(0, 2));
                dataNumIMSI[2] = ConvertFromHexa(iccidNumber.Substring(2, 2));
                dataNumIMSI[3] = ConvertFromHexa(iccidNumber.Substring(4, 2));
                dataNumIMSI[4] = ConvertFromHexa(iccidNumber.Substring(6, 2));
                dataNumIMSI[5] = ConvertFromHexa(iccidNumber.Substring(8, 2));
                dataNumIMSI[6] = ConvertFromHexa(iccidNumber.Substring(10, 2));
                dataNumIMSI[7] = ConvertFromHexa(iccidNumber.Substring(12, 2));
                dataNumIMSI[8] = ConvertFromHexa(iccidNumber.Substring(14, 2));
                dataNumIMSI[9] = GetChekSum(dataNumIMSI);
                #endregion

                #region N° GSM
                dataNumTelephoneGSM = new byte[12];
                dataNumTelephoneGSM[0] = ConvertFromHexa("06");
                // 89 33       10 42 09 02 21 06 01 67
                dataNumTelephoneGSM[1] = ConvertFromHexa(transmetteur.NumeroGsm.Substring(0, 2));
                dataNumTelephoneGSM[2] = ConvertFromHexa(transmetteur.NumeroGsm.Substring(2, 2));
                dataNumTelephoneGSM[3] = ConvertFromHexa(transmetteur.NumeroGsm.Substring(4, 2));
                dataNumTelephoneGSM[4] = ConvertFromHexa(transmetteur.NumeroGsm.Substring(6, 2));
                dataNumTelephoneGSM[5] = ConvertFromHexa(transmetteur.NumeroGsm.Substring(8, 2));
                dataNumTelephoneGSM[6] = ConvertFromHexa("FF");
                dataNumTelephoneGSM[7] = ConvertFromHexa("FF");
                dataNumTelephoneGSM[8] = ConvertFromHexa("FF");
                dataNumTelephoneGSM[9] = ConvertFromHexa("FF");
                dataNumTelephoneGSM[10] = ConvertFromHexa("FF");
                dataNumTelephoneGSM[11] = GetChekSum(dataNumTelephoneGSM);
                #endregion

                #region N° RTC
                dataNumTelephoneRTC = new byte[12];
                dataNumTelephoneRTC[0] = ConvertFromHexa("07");
                // 89 33       10 42 09 02 21 06 01 67
                dataNumTelephoneRTC[1] = ConvertFromHexa("FF");
                dataNumTelephoneRTC[2] = ConvertFromHexa("FF");
                dataNumTelephoneRTC[3] = ConvertFromHexa("FF");
                dataNumTelephoneRTC[4] = ConvertFromHexa("FF");
                dataNumTelephoneRTC[5] = ConvertFromHexa("FF");
                dataNumTelephoneRTC[6] = ConvertFromHexa("FF");
                dataNumTelephoneRTC[7] = ConvertFromHexa("FF");
                dataNumTelephoneRTC[8] = ConvertFromHexa("FF");
                dataNumTelephoneRTC[9] = ConvertFromHexa("FF");
                dataNumTelephoneRTC[10] = ConvertFromHexa("FF");
                dataNumTelephoneRTC[11] = GetChekSum(dataNumTelephoneRTC);
                #endregion

                #region FW
                dataNumVersionFirmware = new byte[4];
                dataNumVersionFirmware[0] = ConvertFromHexa("08");
                dataNumVersionFirmware[1] = ConvertFromHexa("00");
                dataNumVersionFirmware[2] = ConvertFromHexa("00");
                dataNumVersionFirmware[3] = GetChekSum(dataNumVersionFirmware);
                #endregion

                
                #endregion

                #region Regroupement des (sous)trames

                dataGlobal = new byte[1 + dataNumIMEI.Length + dataEvtContactIdEtendu.Length + dataCodePIN.Length + dataNumIMSI.Length + dataNumTelephoneGSM.Length + dataNumTelephoneRTC.Length + dataNumVersionFirmware.Length];

                int indexDataGlobal = 0;

                dataGlobal[indexDataGlobal] = ConvertFromHexa("07"); // Nombre de trames dans le groupe
                indexDataGlobal += 1;
                dataGlobal = Merge(dataNumIMEI, dataGlobal, indexDataGlobal);
                indexDataGlobal += dataNumIMEI.Length;
                dataGlobal = Merge(dataEvtContactIdEtendu, dataGlobal, indexDataGlobal);
                indexDataGlobal += dataEvtContactIdEtendu.Length;
                dataGlobal = Merge(dataCodePIN, dataGlobal, indexDataGlobal);
                indexDataGlobal += dataCodePIN.Length;
                dataGlobal = Merge(dataNumIMSI, dataGlobal, indexDataGlobal);
                indexDataGlobal += dataNumIMSI.Length;
                dataGlobal = Merge(dataNumTelephoneGSM, dataGlobal, indexDataGlobal);
                indexDataGlobal += dataNumTelephoneGSM.Length;
                dataGlobal = Merge(dataNumTelephoneRTC, dataGlobal, indexDataGlobal);
                indexDataGlobal += dataNumTelephoneRTC.Length;
                dataGlobal = Merge(dataNumVersionFirmware, dataGlobal, indexDataGlobal);
                indexDataGlobal += dataNumVersionFirmware.Length;

                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dataGlobal;
        }

        #region Util construction Trames
        /// <summary>
        /// Calcule le complément à Zéro
        /// (Le dernier byte n'est pas pris en compte)
        /// </summary>
        private static byte GetChekSum(byte[] data)
        {
            int checksum = 0;
            int sum = 0;

            for (int i = 0; i < data.Length - 1; i++)
                sum += (int)data[i];

            
            checksum = sum % 256;
            // JO : pas sûr => sum % 255 + checksum = 0
            checksum = 256 - checksum;

            return Convert.ToByte(checksum);
        }
        /// <summary>
        /// Calcule le complément à Zéro
        /// (Le dernier byte n'est pas pris en compte)
        /// </summary>
        private static string GetChekSumContactId(string data)
        {
            int checksum = 0;
            int sum = 0;

            for (int i = 0; i < data.Length; i++)
            {
                // Mode Etendu
                //int num = (int)byte.Parse("0"+data[i], System.Globalization.NumberStyles.HexNumber);
                // Mode Standard
                int num = 0;
                if (data.Substring(i, 1).ToUpper() == "A")
                    num = 10;
                else if (data.Substring(i, 1).ToUpper() == "B")
                    num = 11;
                else if (data.Substring(i, 1).ToUpper() == "C")
                    num = 12;
                else if (data.Substring(i, 1).ToUpper() == "D")
                    num = 13;
                else if (data.Substring(i, 1).ToUpper() == "E")
                    num = 14;
                else if (data.Substring(i, 1).ToUpper() == "F")
                    num = 15;
                else
                    num = Convert.ToInt32(data.Substring(i,1));
                if (num == 0) num = 10;
                sum += num;
            }

            checksum = sum % 15;

            checksum = 15 - checksum;

            return checksum.ToString("X");
        }

        #region Hexa
        /// <summary>
        /// Convertit une valeur Hexadecimal en Octet
        /// </summary>
        private static byte ConvertFromHexa(string hexValue)
        {
            if (hexValue.Length != 2)
                throw new Exception("Format HexaDecimal invalide : " + hexValue);

            return byte.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);
        }

        /// <summary>
        /// Convertit une valeur décimal en hexa
        /// </summary>
        private static string ConvertToHexa(int decValue)
        {
            return decValue.ToString("X");
        }
        private static string ConvertToHexa(byte[] value)
        {
            if (value == null) return "";

            string s = "";
            int val = 0;
            for (int i = 0; i < value.Length; i++)
            {
                val = value[i];
                s += val.ToString("X").PadLeft(2, '0');
            }
            return s;
        }
        #endregion


        private static byte[] Merge(byte[] dataSource, byte[] dataDestination, int offset)
        {
            for (int i = 0; i < dataSource.Length; i++)
            {
                dataDestination[i + offset] = dataSource[i];
            }

            return dataDestination;
        }

        #endregion

        /// <summary>
        /// Envoi des sms à la plateforme
        /// </summary>
        /// <param name="smsRecus"></param>
        /// <returns></returns>
        internal static List<SmsReception> SendListSms(List<SmsReception> smsRecus)
        {
            foreach (SmsReception sms in smsRecus)
            {
                try
                {
                    bool echecTransmission = true;
                    Solem transmetteur = DeserializeSms(sms.Message, sms.NumeroGsm);
                    if (transmetteur != null)
                    {
                        Byte[] trame = ConvertToPacket(transmetteur);

                        string trameHexa = ConvertToHexa(trame);
                        LogHelper.Trace(trameHexa, LogHelper.EnumCategorie.Information);

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

                            if (!string.IsNullOrEmpty(transmetteur.BackupIpOrDns))
                            {
                                try
                                {
                                    serveur.ConnexionServeur(transmetteur.BackupIpOrDns, transmetteur.BackupPort, !Synox.Helpers.NetworkHelper.IsIp(transmetteur.ServerIpOrDns));
                                    echecTransmission = false;
                                }
                                catch
                                {
                                    LogHelper.Trace(string.Format("Echec de connexion au serveur Backup {0}:{1}", transmetteur.BackupIpOrDns, transmetteur.BackupPort), LogHelper.EnumCategorie.Erreur);
                                }
                            }
                        }
                        #endregion
                        string etat = "KO";
                        if (!echecTransmission)
                        {
                            try
                            {
                                // envoi du SMS à la plateforme
                                int nbOctets = serveur.Send(trame);
                                etat = "trame envoyée mais Echec ACK";
                                byte[] ack = serveur.Receive(4);
                                etat = "trame envoyée et ACK recu";
                                sms.DateLecture = DateTime.Now;
                                sms.Commentaire = "[" + ConvertToHexa(ack) + "]";
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
                catch(Exception ex)
                {
                    LogHelper.Trace("Erreur Traitement Sms Solem", ex, LogHelper.EnumCategorie.Erreur);
                }
            }
            return smsRecus;
        }
    }
}

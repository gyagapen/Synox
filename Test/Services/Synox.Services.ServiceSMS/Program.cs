using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Synox;
using Synox.Services.ServiceSMS.Helpers;

namespace Synox.Services.ServiceSMS
{
    static class Program
    {
        private static string ConvertToHexa(byte[] value)
        {
            if (value == null) return "";

            string s = "";
            int val = 0;
            for (int i = 0; i < value.Length; i++)
            {
                val = value[i];
                s += "%"+val.ToString("X").PadLeft(2, '0');
            }
            return s;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {

            EnvironmentApplicationHelper.ChargementSettings();
            EnvironmentApplicationHelper.ApplicationName = "ServiceSMS";

            //byte[] buf = Encoding.ASCII.GetBytes("<Trx&>34534252<\\Trx&>");
            // Helpers.RouteurSmsHelper.SendHttpSms("+33663429874", "01234567890123456789012345678901234567890123456789012345678901234567890123456789");
            //Console.WriteLine(trameHexa);

#if DEBUG
            args = new string[1] { "/cons" };
#endif
            if (args.Length > 0)
            {
                EnvironmentApplicationHelper.ModeConsole = true;
                Console.WriteLine("ServiceSMS v" + EnvironmentSx.ApplicationVersion.ToString(3) + " du " + EnvironmentSx.GetCompiledDate(EnvironmentSx.ApplicationVersion));
                Console.WriteLine("Framework Synox v" + EnvironmentSx.VersionAssembly.ToString(3) + " du " + EnvironmentSx.GetCompiledDate(EnvironmentSx.VersionAssembly));
                ServiceManager serverSync = new ServiceManager();
                serverSync.Start();

                #region Attend une saisie de la commande 'quit' pour sortir
                string quit = string.Empty;
                Console.Title = "ServiceBirdy";

                while (string.IsNullOrEmpty(quit) || !quit.Equals("quit"))
                {
                    Console.WriteLine("///////////////////////////////");
                    Console.WriteLine("taper la commande 'quit' pour quitter l'application\r\n");
                    quit = Console.ReadLine();
                    switch (quit.ToLower())
                    {
                        case "clr":
                        case "clear":
                            Console.Clear();
                            break;
                        //case "color":
                        //    Console.ForegroundColor = ConsoleColor.Red;
                        //    Console.BackgroundColor = ConsoleColor.White;
                        //    Console.Clear();
                        //    break;
                        case "sms":
                            try
                            {
                                System.Net.WebRequest request = System.Net.HttpWebRequest.Create(EnvironmentApplicationHelper.SmsRequestTest);
                                System.Net.WebResponse response = request.GetResponse();
                                LogHelper.Trace(response.ToString(), LogHelper.EnumCategorie.Information);
                                response.Close();
                            }
                            catch (Exception ex)
                            {
                                LogHelper.Trace("Erreur SmsRequest", ex, LogHelper.EnumCategorie.Erreur);
                            }
                            break;
                        case "send":
                            try
                            {

                                Console.Clear();
                                Console.WriteLine("Message...\r\n");
                                string message = Console.ReadLine();
                                int idMessage = 0;
                                string trame = "";
                                string telephone = "+33663429874";
                                Net.CommunicationServicesSync service = new Net.CommunicationServicesSync();
                                service.ConnexionServeur(EnvironmentApplicationHelper.SmsServer, 5000);
                                // format PDU
                                service.EnvoiEnString("AT+CMGF=0\r\n");
                                trame = service.RecoitEnString();

                               // message = string.Format("AT+CMGS=\"{0}\"\r\n{1}{2}\r\n", telephone, message, (char)26 /* "\u001A"*/);
                                trame = string.Format("AT+CMGS={0}\r", telephone);
                                byte[] ctrlZ = new byte[3]{26,13,10};// ctrlZ[0] = 26;
                                byte[] pdu = PduHelper.HexaStringToByteArray("07913336241677f111000b916407498653f300f6002930000000000066010b000001001b04343731314e79e06c4e79e06c0831323334353637380000126701");

                                trame = string.Format("AT+CMGW={0}\r\n", pdu.Length);
                                service.EnvoiEnString(trame);
                                trame = service.RecoitEnString();
                                service.Send(pdu);
                                service.Send(ctrlZ);
                                trame = service.RecoitEnString();


                                if (trame.Contains("OK")) // "\r\n+CMGS: 6\r\n\r\nOK\r\n"
                                {
                                    Console.WriteLine("message ecrit dans routeur");

                                    idMessage = RouteurSmsHelper.RecupereIdMessage(trame);

                                    trame = string.Format("AT+CMSS={0}\r\n", idMessage);
                                    service.EnvoiEnString(trame);
                                    trame = service.RecoitEnString();
                                    Console.WriteLine(">>>>>>>>>>" + trame);

                                }

                                // format Texte avant de quitter
                                service.EnvoiEnString("AT+CMGF=1\r\n");
                                trame = service.RecoitEnString();

                                service.Disconnect();

                                Console.WriteLine(trame);
                                Console.WriteLine("taper 'query' pour connaitre l'état de l'envoi suivi du numéro du message\r\n");
                            }
                            catch (Exception ex)
                            {
                                LogHelper.Trace("Erreur SmsRequest", ex, LogHelper.EnumCategorie.Erreur);
                            }
                            break;
                        case "pdu":
                            try
                            {
                                byte[] ctrlZ = new byte[3] { 26, 13, 10 };// ctrlZ[0] = 26;
                                Console.Clear();
                                string trame = "";
                                string message;
                                string telephone = "33663429874";
                                string centerAddress = "+33634261771";
                                string pduText = @"<@R><PASS>8988<\PASS><NUMTELSIM><\NUMTELSIM><IP01><\IP01><@\R>";
                                string pdu ;//= TextToPdu(pduText);
                                pdu = Helpers.PduHelper.GetPDUString(centerAddress, telephone, pduText);

                                Net.CommunicationServicesSync service = new Net.CommunicationServicesSync();
                                service.ConnexionServeur(EnvironmentApplicationHelper.SmsServer, 5000);

                                // format PDU
                                service.EnvoiEnString("AT+CMGF=0\r");
                                trame = service.RecoitEnString();
                                Console.WriteLine(">> " + trame.Replace("\r", "").Replace("\n", ""));
                                //Verbosite
                                service.EnvoiEnString("AT+CMEE=1\r");
                                trame = service.RecoitEnString();
                                Console.WriteLine(">> " + trame.Replace("\r", "").Replace("\n", ""));

                                service.EnvoiEnString("AT+CSCA=+33634261771\r");
                                trame = service.RecoitEnString();
                                Console.WriteLine(">> " + trame.Replace("\r", "").Replace("\n", ""));

                                pdu = "0691639246124111000A9163362489470004AA0B626F6E6A6F7572206A6F3F";
                                pdu = "07913336040158F611000B913366439278F40004AA0841543A544553545C"; // test convertisseur
                                pdu = "07913336040158F611000B913366439278F40004AA0941543A544553545C6D"; // exemple Doc
                                pdu = "07913336040158F311000B913366439278F40000FF08C2B75BFDAECBB9"; // VB.NET
                                pdu = "07913336040158F311000B913366439278F40008FF100042006F006E006A006F00750072005C";
                                pdu = "07913336040158F311000B913366439278F40000FF08C2B75BFDAECB63";
                                
                                        //07915892000000F0
                                        //01000B915892214365F7000021493A283D0795C3F33C88FE06CDCB6E32885EC6D341EDF27C1E3E97E72E
                                // 07913336241677f111000b916407498653f300f6002930000000000066010b000001001b04343731314e79e06c4e79e06c0831323334353637380000126701
                                //     07913336241677f1 11 00 0b 91 6407498653f3       00           f6                   00                               29 30000000000066010b000001001b04343731314e79e06c4e79e06c0831323334353637380000126701
                                pdu = "07913336241677f1 11 00 0b 91 3326692411F4       00           f6                   17                               29 30000000000066010b000001001b04300000000009e06c4e79e06c0831323334353637380000126701";
                                // encodage en ASCII Etendu (8bits) => 04
                                pdu = "07913336241677f1 01 00 0b 91 3326692411F4       00           04                                                  03 616161".Replace(" ", "");
                                //     taille-n°routeur-sansAR+0validity-?-?-n°dest  -Protocol Id -?    data ou text encrypition  + duree validity    - taille message - message
                                // pdu = "07913336241677f111000b A816092461241 00f6002930000000000066010b000001001b04343731314e79e06c4e79e06c0831323334353637380000126707".Replace(" ", "");

                                 // converter ASCII 7bits : http://twit88.com/home/utility/sms-pdu-encode-decode
                                 //pdu = "07913336241677F111000B913366439278F40000AA0EF4F29C0E1ABFDDF6B29C5E9603";
                                 // converter ASCII 7bits : http://rednaxela.net/pdu.php
                                 //pdu = "07913336241677F101000B913366439278F400080E0062006F006E006A006F00750072";




                                // programme VB .NET
                                 //pdu = "07913336241677F111000B913366439278F40000FF07E2B75BFDAECB01";
                                //336 29 1 56 51 f9  
                                //3326195651f9 
                                //3326195661F9

                                //    33629165169

                                // trame test du 29/09/2011 14:42

                                // test 1 trame anders origine
                                pdu = "0691333624167711000b913326195651f900f6002930000000000066010b000001001b04343731314e84454f5066ca4f0831323334353637380000126701";
                                // test 2 trame anders avec n° dest corrigé
                                pdu = "0691333624167711000b913326195661f900f6002930000000000066010b000001001b04343731314e84454f5066ca4f0831323334353637380000126701";
                                //// test 3 trame anders avec 2 n° corrigés
                                pdu = "07913336241677F111000b913326195661F900f6002930000000000066010b000001001b04343731314e84454f5066ca4f0831323334353637380000126701";
                                //// test 4 trame anders avec n° central corrigé
                                //pdu = "07913336241677F111000b913326195651f900f6002930000000000066010b000001001b04343731314e84454f5066ca4f0831323334353637380000126701";
                                
                                // pdu = "07913336241677f111000b913326195651f900f6002930000000000066010b000001001b04343731314e84454f5066ca4f0831323334353637380000126701";

                                // 0 - 0663429874 - Bonjour joffrey
                               // pdu = "07913336241677f111000B913366439278F40000010FE2B75BFDAECB41EAB7D92C2FE701";
                                pdu = "0011000B913366439278F40000010FE2B75BFDAECB41EAB7D92C2FE701";
                                int taille = 0;

                                taille =(pdu.Length/2)- (Convert.ToInt32(pdu.Substring(0, 2)) + 1);

                                Console.WriteLine(pdu.Replace(" ", ""));
                                pdu = pdu + (char)26;
                                byte[] pduStream = Encoding.Default.GetBytes(pdu);

                                //pduStream = PduHelper.GetBytes(pdu);

                                message = string.Format("AT+CMGS={0}\r", taille);
                                Console.WriteLine(">> " + message.Replace("\r", "").Replace("\n", ""));
                                

                                // COMMANDE AT + LENGTH
                                service.EnvoiEnString(message);
                                trame = service.RecoitEnString();
                                
                                // Envoi PDU
                                service.Send(pduStream);
                                trame = service.RecoitEnString();
                                Console.WriteLine(">> " + trame.Replace("\r", "").Replace("\n", ""));
                                // Recupération de l'id
                                //string id = trame.Split("\r\n".ToArray(), StringSplitOptions.RemoveEmptyEntries)[0].Replace("+CMGW:", "").Trim();
                                //service.EnvoiEnString(string.Format("AT+CMSS={0}\r", id));
                                //trame = service.RecoitEnString();

                                Console.WriteLine("ETAT ENVOI : " + trame.Replace("\r", "").Replace("\n", ""));

                                // format Texte avant de quitter
                                service.EnvoiEnString("AT+CMGF=1\r\n");
                                trame = service.RecoitEnString();

                                service.Disconnect();

                                Console.WriteLine(trame.Replace("\r", "").Replace("\n", ""));
                                Console.WriteLine("taper 'query' pour connaitre l'état de l'envoi suivi du numéro du message\r\n");
                            }
                            catch (Exception ex)
                            {
                                LogHelper.Trace("Erreur SmsRequest", ex, LogHelper.EnumCategorie.Erreur);
                            }
                            break;
                        case "p":
                            try
                            {

                                Console.Clear();
                                string trame = "";
                                string message;
                                string pdu = Console.ReadLine();

                                Net.CommunicationServicesSync service = new Net.CommunicationServicesSync();
                                service.ConnexionServeur(EnvironmentApplicationHelper.SmsServer, 5000);
                                service.EnvoiEnString("AT+CMGF=0\r"); // mode pdu
                                trame = service.RecoitEnString();

                                message = string.Format("AT+CMGS={0}\r", Helpers.PduHelper.GetLength(pdu));
                                byte[] ctrlZ = new byte[3] { 26, 13, 10 };// ctrlZ[0] = 26;

                                // COMMANDE AT + LENGTH
                                service.EnvoiEnString(message);
                                trame = service.RecoitEnString();

                                // message + CTRL Z
                                service.EnvoiEnString(pdu, ctrlZ);
                                trame = service.RecoitEnString();

                                service.Disconnect();

                                Console.WriteLine(trame);
                                Console.WriteLine("taper 'query' pour connaitre l'état de l'envoi suivi du numéro du message\r\n");
                            }
                            catch (Exception ex)
                            {
                                LogHelper.Trace("Erreur SmsRequest", ex, LogHelper.EnumCategorie.Erreur);
                            }
                            break;
                        case "query":
                            try
                            {

                                Console.Clear();
                                Console.WriteLine("Saisir l'identifiant (numero) du message...\r\n");
                                string numero = Console.ReadLine();
                                Net.CommunicationServicesSync service = new Net.CommunicationServicesSync();
                                service.ConnexionServeur(EnvironmentApplicationHelper.SmsServer, 5000);
                                service.EnvoiEnString(string.Format("AT+CMGR={0}", numero));
                                numero = service.RecoitEnString();
                                service.Disconnect();
                                Console.WriteLine(numero);
                            }
                            catch (Exception ex)
                            {
                                LogHelper.Trace("Erreur SmsRequest", ex, LogHelper.EnumCategorie.Erreur);
                            }
                            break;
                    }
                    Console.WriteLine(string.Empty);
                }
                #endregion

                serverSync.Stop();
                serverSync.Dispose();
            }
            else
            {

                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
			{ 
				new ServicePrincipal() 
			};
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}

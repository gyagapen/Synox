using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;


namespace ServiceSMS
{
    class modemSMS
    {
        //evenement qui va etre etre declencher quand on recoit des donnees
        public AutoResetEvent receiveNow;

        //variables 
        /// <summary>
        /// Le port auquel le modem est connecté
        /// </summary>
        SerialPort PortCom { get; set; }

        /// <summary>
        /// Le nom du port
        /// </summary>
        String serialPortName;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="port">Le port de connexion au modem</param>
        public modemSMS(String port)
        {
            serialPortName = port;

        }


        //fonction qui est declenche lorsqu'on recoit des donnees
        //Receive data from port
        public void port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (e.EventType == SerialData.Chars)
                {
                    //on active l'evenement
                    receiveNow.Set();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Connecte le modem
        /// </summary>
        public void connectToModem()
        {
            PortCom = new SerialPort()
            {
                PortName = serialPortName, //no de port, ex : COM11
                BaudRate = 115200, //vitesse a laquelle on envoie/recoit les donnees (bps)
                DataBits = 8, //longueur standard des bits de données par octet
                StopBits = StopBits.One,
                RtsEnable = true
            };

            //on initialise l'evenement de la reception du signal a faux
            receiveNow = new AutoResetEvent(false);

            //on associe la fonction port_datareceived a l'evenement data received
            PortCom.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
            //on ouvre le port -- on connecte le modem (port en serie)
            try
            {
                PortCom.Open();
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.Message);
                Console.Read();
            }

            //IMPORTANT : permet la reception des accuses reception et la lecture des messages
            Send("AT+CNMI=2,1,1,2,1"); 
            //Send("AT+CNMI=1,1,1,0,0"); // Réception message

            Console.Out.WriteLine("Modem Connecte");

        }

        /// <summary>
        /// Déconnecte le modem
        /// </summary>
        public void disconnectToModem()
        {
            if (PortCom != null)
            {
                PortCom.Close();
                PortCom.Dispose();
                PortCom = null;
            }


            Console.Out.WriteLine("Modem Deconnecte");

        }


        /// <summary>
        /// Envoi d'un sms en mode texte
        /// </summary>
        /// <param name="no">Le numéro du destinataire</param>
        /// <param name="message">Le message texte</param>
        public void sendSMSText(string no, string message)
        {
            //mode text
            Send("AT+CMGF=1");

            Send("AT+CMGS=\"" + no + "\"");

            Send(message + char.ConvertFromUtf32(26));

        }

        /// <summary>
        /// Envoi d'un sms en mode PDU, receipt = accuse de reception
        /// </summary>
        /// <param name="no">le numéro du destinataire</param>
        /// <param name="message">Le message</param>
        /// <param name="receipt">true si accusé demandé, false par défaut</param>
        /// <param name="typeEncodage">"7bits", "8bits" ou "16bits"</param>
        /// <param name="validityPeriod">La période de validité (-1 par défaut = aucune durée)</param>
        /// <returns>La référence du message envoyé</returns>
        public String sendSMSPDU(string no, string message, Boolean receipt = false, string typeEncodage = "16bits", int validityPeriod = -1)
        {
            string pduMSG = encodeMsgPDU(message, no, receipt, typeEncodage, validityPeriod);

            //mode pdu
            Send("AT+CMGF=0");

            //longueur du message 
            int lenght = (pduMSG.Length - 2) / 2;

            //on envoie le sms
            Send("AT+CMGS=" + lenght);

            Send(pduMSG + char.ConvertFromUtf32(26), 1);

            //recuperation et affichage de la reponse

            string result = null;

            result = Recv();

            Console.WriteLine(result);

            //on retourne la reference du message envoye ou "ERROR" en cas d'erreur
            return getRefSentSMS(result);

        }


        /// <summary>
        /// Envoi une trame PDU
        /// </summary>
        /// <param name="trame">La trame PDU à envoyer</param>
        /// <returns>La référence du message envoyé</returns>
        public String sendTramePDU(string trame)
        {
            //mode pdu
            Send("AT+CMGF=0");

            //longueur du message 
            int lenght = (trame.Length - 2) / 2;

            //on envoie le sms
            Send("AT+CMGS=" + lenght);

            Send(trame + char.ConvertFromUtf32(26), 1);

            //recuperation et affichage de la reponse
            string result = Recv();
            Console.WriteLine(result);

            //on retourne la reference du message envoye ou "ERROR" en cas d'erreur
            string reference = getRefSentSMS(result);


            return reference;
        }


        /// <summary>
        /// Lit tous les SMS
        /// </summary>
        /// <remarks>Obsolète</remarks>
        /// <see cref="readPDUMessage()"/>
        public void readAllSMSText()
        {
            //mode text
            Send("AT+CMGF=1");

            readAllSMS("\"ALL\"");
        }


        /// <summary>
        /// Lecture des messages en mode texte
        /// </summary>
        /// <param name="typeLecture">Le type de lecture</param>
        /// <param name="reponseAutomatique">La réponse automatique</param>
        public void readAllSMS(string typeLecture, int reponseAutomatique = 0)
        {
            Send("AT+CPMS=\"SM\"");

            Send("AT+CMGL=" + typeLecture, reponseAutomatique);
        }



        /// <summary>
        /// Compte le nombre de message présent sur la SIM
        /// </summary>
        /// <returns>Le nombre de SMS sur la SIM</returns>
        public int countSMSOnSim()
        {
            Send("AT+CPMS=\"SM\"", 1);
            string rep = Recv(); 
            int nb;

            if (int.TryParse(rep.Substring(22, 2), out nb))
                return nb;
            else
                return int.Parse(rep.Substring(22, 1));
        }

        /// <summary>
        /// Retourne le nombre d'accuses de reception sur le modem
        /// </summary>
        /// <returns>Le nombre d'accuses de reception sur le modem</returns>
        public int countReceiptOnSIM()
        {
            Send("AT+CPMS=\"SR\"", 1);
            string rep = Recv();
            int nb;

            if (int.TryParse(rep.Substring(22, 2), out nb))
                return nb;
            else
                return int.Parse(rep.Substring(22, 1));
        }

        /// <summary>
        /// Reçoit un message du port com
        /// </summary>
        /// <returns>Le message reçu</returns>
        public string Recv()
        {

            string response = "";

            do //tantque la reponse n'est pas complete
            {
                //si le signal comme quoi on a recu un message est active dans la prochaine demie seconde
                if (receiveNow.WaitOne(10000, false))
                {
                    //on recupere le message
                    string message = PortCom.ReadExisting();

                    //on l'ajoute a la reponse car on n'est pas sur que le message soit complet
                    response += message;

                }
                else
                {
                    if (response.Length > 0)

                        throw new ApplicationException("Response received is incomplete.");
                    else
                        throw new ApplicationException("No data received from phone.");

                }

            }
            while (!response.EndsWith("\r\nOK\r\n") && !response.EndsWith("\r\n> ") && !response.EndsWith("\r\nERROR\r\n"));

            return response;
        }



        /// <summary>
        /// Permet d'envoyer des commandes AT au modem
        /// </summary>
        /// <param name="query">la commande AT</param>
        /// <param name="estLu">Si 0 la réponse du modem est lue (Recv()) et affichée, si 1 elle n'est pas traitée</param>
        public void Send(string query, int estLu = 0)
        {
            byte[] buffer = Encoding.Default.GetBytes(query + "\r");
            if (PortCom.IsOpen)
            {
                //nettoyage des buffers
                PortCom.DiscardOutBuffer();
                PortCom.DiscardInBuffer();

                //reinitialisation de l'evenement
                receiveNow.Reset();

                //buffer : Tableau d'octets qui contient les données à écrire sur le port. 
                //offset : 0, index du buffer partir duquel commencer la copie des octets vers le port.
                //buffer.length : Nombre d'octets à écrire sur le port
                PortCom.Write(buffer, 0, buffer.Length);

                if (estLu == 0)
                {
                    //on affiche la reponse de la commande
                    Console.Out.WriteLine(Recv());
                }
            }
        }

        /// <summary>
        /// retourne les SMS recus
        /// </summary>
        /// <returns>Les SMS reçus</returns>
        public SMS[] readPDUMessage()
        {
            //passage en mode PDU
            Send("AT+CMGF=0");

            readAllSMS("4", 1);

            //messages en text brut
            string message = Recv();

            //Console.Out.WriteLine("Message brut : " + message);

            //on recupere les reponses a decoder
            string[] tabRep = decouperChaineLecturePDU(message);

            SMS[] tabSMSRecus = new SMS[tabRep.Length];

            //on decode chaque reponse
            for (int i = 0; i < tabRep.Length; i++)
            {
                tabSMSRecus[i] = decodeSMSPDU(tabRep[i]);
            }

            return tabSMSRecus;
        }


        /**
        * Retourne un tableau avec les informations de l'accuse reception
         * pour le message i
        * [i]0 - Reference
        * [i]1 - Destinataire
        * [i]2 - Date d'envoi SMS
        * [i]3 - Date Reception
        * [i]4 - Heure de reception 
        * */
        public String[][] readDeliveryReport()
        {
            //passage en mode PDU
            Send("AT+CMGF=1");

            Send("AT+CPMS=\"SR\"");

            Send("AT+CMGL=\"ALL\"", 1);



            //messages en texte brut
            string message = Recv();

            //on recupere les reponses a decoder
            string[] tabRep = message.Split('\n');

            String[][] result = new String[tabRep.Length][];

            //on decode chaque reponse
            for (int i = 0; i < tabRep.Length; i++)
            {
                result[i] = decoderDeliveryReport(tabRep[i]);
            }

            return result;
        }

        /**
         * Encode un message texte
         * Receipt : demande accuse reception ou non
         * typeEncodage : 7,8 ou 16 bits ?
         * validityPeriod : periode de validite du message, par defaut 0 (aucune periode)
         * */
        public string encodeMsgPDU(string message, string no, Boolean receipt, string typeEncodage, int validityPeriod = -1)
        {
            SMS sms = new SMS();
            String result = null;

            //Setting direction of sms
            sms.Direction = SMSDirection.Submited;

            //Set the recipient number
            sms.PhoneNumber = no;

            sms.Message = message;

            //accuse de recepetion
            sms.StatusReportIndication = true;


            //periode de validite 
            if (validityPeriod != -1)
            {
                sms.ValidityPeriod = decoderValidityPeriod(validityPeriod);
            }


            //Encodage
            switch (typeEncodage)
            {
                case "7bits":
                    result = sms.Compose(SMS.SMSEncoding._7bit);
                    break;
                case "8bits":
                    result = sms.Compose(SMS.SMSEncoding._8bit);
                    break;
                case "16bits":
                    result = sms.Compose(SMS.SMSEncoding.UCS2);
                    break;
            }

            return result;

        }



        public SMS decodeSMSPDU(string message)
        {
            //on recupere le sms
            SMS sms = new SMS();

            SMS.Fetch(sms, ref message);
            //afficherContenuMessagePDU(sms);

            return sms;
        }


        //fonction qui affiche le contenu d'un message PDU
        public void afficherContenuMessagePDU(SMS unSMS)
        {
            Console.Out.WriteLine("--------- Contenu message PDU ------------");
            Console.Out.WriteLine("Expediteur : " + unSMS.PhoneNumber);
            Console.Out.WriteLine("Message : " + unSMS.Message);
            Console.Out.WriteLine("Date : " + unSMS.ServiceCenterTimeStamp);
            Console.Out.WriteLine("Accuse reception : " + unSMS.StatusReportIndication);
            Console.Out.WriteLine("Type de message : " + unSMS.Type);


        }

        //
        //supprime tous les messages de la sim
        //
        public void deleteAllSMS()
        {
            Console.Out.WriteLine("Deleting all messages");
            Send("AT+CMGD=0,4");
        }


        
        /// <summary>
        /// Supprime les messages lus si la mémoire et pleine ainsi que les accusés de réception si la mémoire est pleine
        /// </summary>
        public void deleteAllReadSMS()
        {
            // Si on est proche de la saturation mémoire des accusés (limitée à 50 accusés)
            if (countReceiptOnSIM() >= 45)
            {
                Console.Out.WriteLine("Deleting all RECEIPTS (" + countReceiptOnSIM().ToString() + ")");
                Send("AT+CPMS=\"SR\"");
                Send("AT+CMGD=1,2");
            }

            // Si il y a de nombreux messages lus sur la SIM
            /*if (countSMSOnSim() >= 25)
            {*/
                Console.Out.WriteLine("Deleting all READ messages (" + countSMSOnSim().ToString() + ")");
                Send("AT+CPMS=\"SM\"");
                Send("AT+CMGD=1,2");
            //}
        }


        //on decoupe la reponse du modem lorsqu'on veut lire des sms en mode PDU
        //on retourne un tableau de chaines PDU
        public string[] decouperChaineLecturePDU(string reponse)
        {
            string[] temp = reponse.Split('\n');
            List<string> tabRep = new List<string>();

            for (int i = 0; i < temp.Length; i++)
            {
                if (!temp[i].Contains("+") && !temp[i].Contains("OK") && temp[i] != null && temp[i] != "" && temp[i] != "\r" && temp[i] != "\n")
                {
                    //on ajoute au tableau
                    tabRep.Add(temp[i]);

                }
            }
            return tabRep.ToArray();
        }

        /**
         * Retourne un tableau avec les informations de l'accuse reception
         * 0 - Reference
         * 1 - Destinataire
         * 2 - Date d'envoi SMS
         * 3 - Date Reception
         * 4 - Heure de reception
         * */
        public String[] decoderDeliveryReport(string message)
        {
            String[] result = new String[5];
            //on enleve toutes les reponses non pertinentes
            if (message.StartsWith("+"))
            {
                String[] tabAttributs = message.Split(',');


                result[0] = tabAttributs[3];
                result[1] = tabAttributs[4];
                result[2] = tabAttributs[6] + " " + tabAttributs[7];
                result[3] = tabAttributs[8].Replace("\"", "");
                result[4] = tabAttributs[9].Replace("\"", "").Remove(8);



                /*Console.Out.WriteLine("--------- Contenu Accuse reception------------");
                Console.Out.WriteLine("Reference"+tabAttributs[3]);
                Console.Out.WriteLine("Destinataire"+tabAttributs[4]);
                Console.Out.WriteLine("Date envoi SMS" + tabAttributs[6] +" @ "+ tabAttributs[7]);
                Console.Out.WriteLine("Date Reception Accuse" + tabAttributs[8]+" @ " + tabAttributs[9]);*/
            }

            return result;
        }

        //recupere la reference d'une reponse issue de l'envoie d'un SMS
        public string getRefSentSMS(string response)
        {
            string[] temp = response.Split('\n');
            string result = null;

            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i].Contains("CMGS"))
                {
                    result = temp[i].Split(':').ElementAt(1).Trim();
                }
                else if (temp[i].Contains("ERROR")) //si erreur
                {
                    result = "ERROR";
                }
            }
            return result;
        }


        //calcul la periode de validite
        public int calculValidityPeriod(TimeSpan unePeriode)
        {

            SMS sms = new SMS();
            return sms.calculValidityPeriod(unePeriode);
        }


        //permet de retrouver un timespan depuis une valeur en int de validity period
        public TimeSpan decoderValidityPeriod(int intValue)
        {
            
            int minutes=0;
            int heures=0;
            int jours = 0;

            if (intValue < 144)
            {
                //minutes
                int temp = (intValue + 1) * 5;
                heures = (int)(temp / 60);
                minutes = (int)(temp - 60 * heures);
            }
            else if (intValue < 168)
            {
                 int temp = (int)(intValue - 143) * 30;
                 heures = 12 + (int)(temp / 60);
                 minutes = temp - (int)((heures-12)*60);
            }
            else if (intValue < 197)
            {
                jours = (int)(intValue - 166);
            }
            else if (intValue < 256)
            {
                jours = (int)(intValue - 192) * 7;
            }

            //construction du time span
            TimeSpan resultat = new TimeSpan(jours, heures, minutes,0);

            return resultat;
        }



        public string Recv()
        {

            string response = "";

            do //tantque la reponse n'est pas complete
            {
                //si le signal comme quoi on a recu un message est active dans les 10 prochaines secondes
                if (receiveNow.WaitOne(10000, false))
                {
                    //on recupere les donnees presentes sur le port serie
                    string message = PortCom.ReadExisting();

                    //on l'ajoute a la reponse car on n'est pas sur que le message soit complet
                    response += message;
                }
                else
                {
                    //message incomplet ...
                }

            }
            while (!response.EndsWith("\r\nOK\r\n") && !response.EndsWith("\r\nERROR\r\n"));

            return response;
        }

    }
}

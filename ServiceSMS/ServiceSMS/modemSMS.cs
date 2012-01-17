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
        SerialPort PortCom { get; set; }
        String serialPortName;

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

        //fonction pour connecter le modem
        public void connectToModem()
        {
            PortCom = new SerialPort()
            {
                PortName = serialPortName,
                BaudRate = 115200,
                DataBits = 8,
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

            //permet la reception des accuses reception
            Send("AT+CSMP=49,167,0,0");
            Send("AT+CNMI=2,2,3,2,1");

            Console.Out.WriteLine("Modem Connecte");

        }
        //fonction pour connecter le modem
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


        //envoi d'un sms en mode texte
        public void sendSMSText(string no, string message)
        {
            //mode text
            Send("AT+CMGF=1");

            Send("AT+CMGS=\"" + no + "\"");

            Send(message + char.ConvertFromUtf32(26));

        }

        //envoi d'un sms en MODE pdu, receipt = accuse de reception
        public String sendSMSPDU(string no, string message, Boolean receipt = false, string typeEncodage = "16bits", int validityPeriod = 0)
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
            string result = Recv();
            Console.WriteLine(result);

            //on retourne la reference du message envoye ou "ERROR" en cas d'erreur
            return getRefSentSMS(result);

        }


        //envoi d'une TRAME pdu, receipt = accuse de reception
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

            Console.WriteLine("REPONSE " + reference + " FiN");

            return reference;
        }



        public void readAllSMSText()
        {
            //mode text
            Send("AT+CMGF=1");

            readAllSMS("\"ALL\"");
        }

        //lecture des messages en mode texte
        public void readAllSMS(string typeLecture, int reponseAutomatique = 0)
        {
            Send("AT+CPMS=\"SM\"");

            Send("AT+CMGL=" + typeLecture, reponseAutomatique);

        }



        //compte le nombre de message présent sur la sim
        public int countSMSOnSim()
        {
            Send("AT+CPMS?");

            return 0;
        }

        //recoit un message du port com
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

                    //Console.Out.WriteLine("message en cours : " + response);
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





        //envoie des commandes AT au modem
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

                PortCom.Write(buffer, 0, buffer.Length);

                if (estLu == 0)
                {
                    //on affiche la reponse de la commande
                    Console.Out.WriteLine(Recv());
                }


            }


        }


        public void readPDUMessage()
        {
            //passage en mode PDU
            Send("AT+CMGF=0");

            readAllSMS("4", 1);

            //messages en text brut
            string message = Recv();

            //Console.Out.WriteLine("Message brut : " + message);

            //on recupere les reponses a decoder
            string[] tabRep = decouperChaineLecturePDU(message);

            //on decode chaque reponse
            for (int i = 0; i < tabRep.Length; i++)
            {
                decodeSMSPDU(tabRep[i]);
            }
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



            //messages en text brut
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
        public string encodeMsgPDU(string message, string no, Boolean receipt, string typeEncodage, int validityPeriod = 0)
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
            if (validityPeriod != 0)
            {
                sms.ValidityPeriod = new TimeSpan(0, 0, 5, 0, 0);
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


        //
        //supprime tous les messages lus de la sim
        //
        public void deleteAllReadSMS()
        {
            Console.Out.WriteLine("Deleting all READ messages");
            Send("AT+CMGD=1,2");
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
            TimeSpan resultat = new TimeSpan();

            return resultat;
        }





        public int calculValidityPeriod(TimeSpan value)
        {
            int result;

            //si plus de 4 semaines
            if (value.Days >= 35)
            {
                result = (int)(value.Days / 7) + 192;
            }
            else if (value.Days >= 2)
            {
                result = (int)(value.Days + 166);
            }
            else if (value.Hours >= 12)
            {
                result = (int)((value.Hours - 12) * 2 + 143 + (int)(value.Minutes / 30));
            }
            else
            {
                result = (int)(value.Minutes / 5 - 1 + value.Hours * 12);
            }

            Console.Out.WriteLine("Valeur TP VP : " + result);

            return result;
        }

    }
}

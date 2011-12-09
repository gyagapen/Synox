using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace ConsoleApplicationTestSMS
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

            Console.Out.WriteLine("Modem Connecte");

        }


        //envoi d'un sms en mode texte
        public void sendSMS(string no, string message)
        {
            Send("AT+CMGS=\""+no+"\"");
            
            Send(message + char.ConvertFromUtf32(26));
            
        }

        //envoi d'un sms en mode pdu
        public void sendSMSPDU(string message, string no)
        {
            string pduMSG = encodeMsgPDU(message, no);


            //mode pdu

            Send("AT+CMGF=0");

            //longueur du message 
            int lenght = (pduMSG.Length - 2) / 2;

            //on envoie le sms
            Send("AT+CMGS="+lenght);
            
            Send(pduMSG + char.ConvertFromUtf32(26));
            
        }


        public void readAllSMSText()
        {
            //mode text
            Send("AT+CMGF=1");

            readAllSMS("\"ALL\"");
        }

        //lecture des messages en mode texte
        public void readAllSMS(string typeLecture, int reponseAutomatique = 0 )
        {
            Send("AT+CPMS=\"SM\"");
            Send("AT+CMGL="+typeLecture,reponseAutomatique);
         
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

            //on recupere les reponses a decoder
            string[] tabRep = decouperChaineLecturePDU(Recv());

            //on decode chaque reponse
            for (int i = 0; i < tabRep.Length; i++)
            {
                decodeSMSPDU(tabRep[i]);
            }
        }

        public string encodeMsgPDU(string message, string no)
        {
            SMS sms = new SMS();

            //Setting direction of sms
            sms.Direction = SMSDirection.Submited;

            //Set the recipient number
            sms.PhoneNumber = no;

            sms.Message = message;

            //accuse de recepetion
            sms.StatusReportIndication = true;

            //periode de validite de deux jours
            sms.ValidityPeriod = new TimeSpan(0,0,5,0,0);
            
            return sms.Compose(SMS.SMSEncoding._7bit);
        }


        public void decodeSMSPDU(string message)
        {
            //on determine le type du sms recu
            SMSType smsType = SMSBase.GetSMSType(message);

            //si c'est un sms classique
            if (smsType == SMSType.SMS)
            {
                //on recupere le sms
                SMS sms = new SMS();
                SMS.Fetch(sms, ref message);

                Console.Out.WriteLine("Message "+sms.Message);
            }
            else // c'est un accuse de recpetion
            {
                //on recupere l'accuse
                SMSStatusReport smsStatus = new SMSStatusReport();
                SMSStatusReport.Fetch(smsStatus, ref message);
                Console.Out.WriteLine("Accuse de reception " + smsStatus.MessageReference);
            }
        }


        //supprime tous les messages de la sim
        public void deleteAllSMS()
        {
            Console.Out.WriteLine("Deleting all messages");
            Send("AT+CMGD=0,4");
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

                    //Console.Out.WriteLine("A DECODER :" + temp[i]);
                }
            }


            return tabRep.ToArray(); 
        }
    }
}

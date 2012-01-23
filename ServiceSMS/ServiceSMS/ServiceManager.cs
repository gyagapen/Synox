using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace ServiceSMS
{
    public class ServiceManager
    {

        /// <summary>
        /// Flag pour savoir si les services sont démarrés
        /// </summary>
        public bool IsStarted = false;

        /// <summary>
        /// Timer Général
        /// </summary>
        private System.Timers.Timer _timerService;
        /// <summary>
        /// Flag qui indique si la tache de fond est en cours de travail
        /// </summary>
        private bool _busy;
        /// <summary>
        /// Date de la derniere execution du timer
        /// </summary>
        private DateTime _dateDerniereLecture;
        /// <summary>
        /// Initialisé à 1 heure
        /// </summary>
        private TimeSpan _dureeEntre2Lectures = new TimeSpan(0, 0, 15);
        /// <summary>
        /// Date de la derniere execution du timer
        /// </summary>
        private DateTime _dateDerniereEnvoi;
        /// <summary>
        /// Initialisé à 1 heure
        /// </summary>
        private TimeSpan _dureeEntre2Envois = new TimeSpan(0, 0, 15);

        //private Timer t = null;

        //on declare une interface pour dialoguer avec le modem
        private modemSMS modem = null;

        //informations sur le modem
        private const String numeroModem = "+33604655154";
        private const String noPortModem = "COM5";

        //Reference vers la base de donnees
        private DBSMSContextDataContext dbContext = new DBSMSContextDataContext();

        /// <summary>
        /// Constructeur
        /// </summary>
        public ServiceManager()
        {
            // Taches de fons toutes les 2 secondes
            _timerService = new System.Timers.Timer();
            _timerService.Interval = 2000;
            _timerService.Elapsed += new ElapsedEventHandler(timerService_Elapsed);

            _dateDerniereLecture = DateTime.Now;
        }


        #region Start / Stop
        /// <summary>
        /// Lancement du Timer
        /// </summary>
        public void Start()
        {
            // Démarrage des taches de fond
            IsStarted = true;
            this._timerService.Start();


            //on initialise le modem
            modem = new modemSMS(noPortModem);

        }
        /// <summary>
        /// Arret des services
        /// </summary>
        public void Stop()
        {
            // Arret des taches de fond
            this._timerService.Stop();
            modem = null;
            IsStarted = false;
        }
        /// <summary>
        /// Arret des thread en cours
        /// </summary>
        public void Dispose()
        {

        }
        #endregion


        #region Events
        /// <summary>
        /// Se produit à fréquence régulière (toutes les 2 secondes)
        /// </summary>
        /// <param name="sender">Objet qui appelle la méthode</param>
        /// <param name="e">contient les parametres de l'evenement</param>
        void timerService_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!_busy)
                {
                    _busy = true;
                    // tqche de lecture des sms recus
                    if (_dateDerniereLecture.Add(_dureeEntre2Lectures) < DateTime.Now)
                    {
                        //  le process de lecture


                        //lecture des accuses reception
                        Console.WriteLine("Lecture des accuses reception");
                        getDeliveryReport();

                        //lecture des messages
                        readMessagesOnSim();

                        //on supprime tous les messages lus du modem
                        modem.connectToModem();
                        modem.deleteAllReadSMS();
                        modem.disconnectToModem();

                        _dateDerniereLecture = DateTime.Now;
                    }


                    // tache de envoi des sms
                    if (_dateDerniereEnvoi.Add(_dureeEntre2Envois) < DateTime.Now)
                    {
                        //on recupere tous les message en attente d'envoi dans la base de donnees


                        MessageEnvoi[] lesSMSAEnvoyer = (from msg in dbContext.MessageEnvoi
                                                         where msg.Statut.libelleStatut == "En attente"
                                                         select msg).ToArray();




                        //connexion au modem
                        if (lesSMSAEnvoyer.Length > 0)
                            modem.connectToModem();

                        //pour chaque message a envoyer
                        foreach (MessageEnvoi sms in lesSMSAEnvoyer)
                        {
          
                                //on envoie le sms
                                envoyerSMS(sms);

                        }



                        if (lesSMSAEnvoyer.Length > 0)
                            modem.disconnectToModem();
                        // to do
                        _dateDerniereEnvoi = DateTime.Now;
                    }

                    _busy = false;
                }
            }
            catch (Exception ex)
            {
                //_busy = false;
                //LogHelper.Trace("timerService_Elapsed:" + ex.Message, LogHelper.EnumCategorie.Erreur); 
                Console.WriteLine("Erreur : " + ex.Message);
                Console.WriteLine(ex.StackTrace);
                // On redémarre le service
                Stop();
                Start();
            }
        }


        /// <summary>
        /// methode qui communique avec le modem pour une demande d'envoi de SMS avec un message texte
        /// </summary>
        /// <param name="sms"></param>
        private void envoyerSMS(MessageEnvoi sms)
        {
            //on recupere le statut "ENVOYE"
            Statut statutEnvoye = (from stat in dbContext.Statut where stat.libelleStatut == "Envoye" select stat).First();

            //on recupere le booleen concernant la demande d'accuse reception
            Boolean demandeAccuse = true; //par defaut a vrai

            if (sms.Message.accuseReception == 0) //si specifier le contraire en BD, on change la valeur
            {
                demandeAccuse = false;
            }

            //on determine le message a envoyer
            String messageAEnvoyer = null;

            //pour sauvegarder la reference du SMS envoye
            String reference = null;

            //si message Texte est nul alors c'est une trame PDU (si pas nul aussi)
            if (sms.Message.messageTexte == null && sms.Message.messagePDU != null)  //TRAME PDU
            {
                messageAEnvoyer = sms.Message.messagePDU;
                //on recupere la reference du message envoye
                reference = modem.sendTramePDU(messageAEnvoyer);

                //decodage de la trame PDU
                SMS smsPDU = modem.decodeSMSPDU(messageAEnvoyer);

                sms.Message.messageTexte = smsPDU.Message;
                sms.Message.noDestinataire = smsPDU.PhoneNumber;
                sms.dureeValidite = modem.calculValidityPeriod(smsPDU.ValidityPeriod);

                //status report
                if(smsPDU.StatusReportIndication)
                    sms.Message.accuseReception = 1;
                else
                    sms.Message.accuseReception = 0;

            }
            else if (sms.Message.messageTexte != null && sms.Message.messagePDU == null) //si trame PDU est nul alors c'est un message Texte (si pas nul aussi)
            {
                messageAEnvoyer = sms.Message.messageTexte;

                //on recupere la duree de validite

                int dureeValiditite = -1; 
                
                if (sms.dureeValidite.HasValue)
                    dureeValiditite= sms.dureeValidite.Value;


                //on recupere la reference du message envoye
                reference = modem.sendSMSPDU(sms.Message.noDestinataire, messageAEnvoyer, demandeAccuse, sms.Message.Encodage.libelleEncodage, dureeValiditite);
            }

            //verifie s'il y a une erreur
            if (reference.Contains("ERROR"))
            {
                //on passe le statut du sms a erreur
                sms.Statut = (from stat in dbContext.Statut where stat.libelleStatut == "Erreur" select stat).First();
            }
            else //on sauvegarde la reference en BD
            {
                sms.referenceEnvoi = reference;
            }


            //on sauvegarde le sms comme envoye s'il y a une reference d'envoi
            if (reference != null)
            {
                //on complete les informations sur l'envoi des SMS
                sms.Message.noEmetteur = numeroModem;
                sms.accuseReceptionRecu = 0; //pas encore recu

                //on passe le sms en statut envoye
                sms.Statut = statutEnvoye;
                sms.dateEnvoi = DateTime.Now; //date du jour

            }
            else // erreur
            {
                //on passe le statut du sms a erreur
                sms.Statut = (from stat in dbContext.Statut where stat.libelleStatut == "Erreur" select stat).First();
            }


            //on valide les changements dans la BD
            dbContext.SubmitChanges();

        }


        /// <summary>
        /// lit les messages presents sur la sim
        /// </summary>
        public void readMessagesOnSim()
        {
            //connexion au modem
            modem.connectToModem();
            
            //on recupere les messages sur la sim
            SMS[] lesMessagesSurSIM = modem.readPDUMessage();

            //pour chaque message
            foreach (SMS sms in lesMessagesSurSIM)
            {
                //on fait le mapping avec un nouvel objet de la BD
                
                //initialisation
                MessageRecu msg = new MessageRecu();
                msg.Message = new Message();

                //remplissage
                msg.dateReception = sms.ServiceCenterTimeStamp;
                msg.Message.messageTexte = sms.Message;
                msg.Message.noEmetteur = sms.PhoneNumber;
                msg.Message.noDestinataire = numeroModem;
                msg.Message.accuseReception = 0; //faux par defaut

                Console.WriteLine("lecture message : " + sms.Message);

                if (sms.StatusReportIndication)
                    msg.Message.accuseReception =1;
              
                //encodage
                msg.Message.Encodage = (from enc in dbContext.Encodage where enc.libelleEncodage == "PDU" select enc).First();


                //enregistre du message
                dbContext.Message.InsertOnSubmit(msg.Message);
                dbContext.MessageRecu.InsertOnSubmit(msg);
            }

            //sauvegarde des changements
            dbContext.SubmitChanges();

            //deconnexion
            modem.disconnectToModem();
            
        }


        /// <summary>
        /// 
        /// </summary>
        public void getDeliveryReport()
        {
            //on selectionne les messages dont la reception de l'accuse reception est a verifier
            MessageEnvoi[] lesSMSAVerifier = (from msg in dbContext.MessageEnvoi
                                              where msg.Statut.libelleStatut == "Envoye"
                                              && msg.Message.accuseReception == 1
                                              select msg).ToArray();


            String[][] accuses = null;

            if (lesSMSAVerifier.Count() > 0)
            {
                //connexion modem
                modem.connectToModem();

                 //on recupere les accuses du modem
            /**
       * Retourne un tableau avec les informations de l'accuse reception
        * pour le message i
       * [i]0 - Reference
       * [i]1 - Destinataire
       * [i]2 - Date d'envoi SMS
       * [i]3 - Date Reception
       * [i]4 - Heure de reception
       * */
             accuses = modem.readDeliveryReport();
            }

           

            //pour chaque message
            foreach (MessageEnvoi sms in lesSMSAVerifier)
            {
                //si on a recu l'accuse
                if (verifierAccusePresentDansModem(sms, accuses))
                {
                    //on change le statut
                    sms.Statut = (from stat in dbContext.Statut where stat.libelleStatut == "Accuse" select stat).First();
                    sms.accuseReceptionRecu = 1;
                }
            }

            //on submit les changement
            dbContext.SubmitChanges();

            if (lesSMSAVerifier.Count() > 0)
            {
                modem.disconnectToModem();
            }

        }


        /// <summary>
        /// retourne vrai si l'accuse du sms a ete recu par le modem
        /// </summary>
        /// <param name="sms"></param>
        /// <param name="listeAccusesModem"></param>
        /// <returns></returns>
        public Boolean verifierAccusePresentDansModem(MessageEnvoi sms, String[][] listeAccusesModem)
        {
            Boolean estPresent = false;
            int compteur = 0;

            while (estPresent == false && compteur<listeAccusesModem.Count())
            {
                if ((listeAccusesModem[compteur][0] != null) && (sms.referenceEnvoi != null))
                {
 
                    if (sms.referenceEnvoi.Trim() == listeAccusesModem[compteur][0].Trim())
                    {
                        estPresent = true;

                        //on sauvegarde la date de l'accuse reception
                        Console.WriteLine("Date origine : " + listeAccusesModem[compteur][3] + " " + listeAccusesModem[compteur][4]);

                        DateTime dateReception = DateTime.ParseExact(listeAccusesModem[compteur][3] + " " + listeAccusesModem[compteur][4], "yy/MM/dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

                        sms.dateReceptionAccuse = dateReception;

                        
                    }
                    
                }
                compteur++;
                
            }

            return estPresent;
        }

        #endregion
    }
}

﻿using System;
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
        private const String noPortModem = "COM11";

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
                        // to do : fqire le process de lecture
                        // ...
                        _dateDerniereLecture = DateTime.Now;
                    }


                    // tache de envoi des sms
                    if (_dateDerniereEnvoi.Add(_dureeEntre2Envois) < DateTime.Now)
                    {
                        //on recupere tous les message en attente d'envoi dans la base de donnees
                        /*Message[] lesSMSAEnvoyer = (from msg in dbContext.Message
                                                    join status in dbContext.Statut
                                                    on msg.idStatut equals status.idStatut
                                                    where status.libelleStatut == "En attente"
                                                    select msg).ToArray();*/

                        MessageEnvoi[] lesSMSAEnvoyer = (from msg in dbContext.MessageEnvoi
                                                         where msg.Message.Statut.libelleStatut == "Entente"
                                                         select msg).ToArray();
                                                         

                        //on cherche un MessageEnvoi correspondant au SMS envoye en BD
                        /*var result = (from smsE in dbContext.MessageEnvoi where smsE.idMessage == sms.idMessage select smsE);



                        MessageEnvoi smsEnvoi;

                        //si deja present BD
                        if (result.Count() > 0)
                        {
                            smsEnvoi = result.First();
                        }
                        else //si aucune entree en BD
                        {
                            //on  instancie un nouveau MessageEnvoi
                            smsEnvoi = new MessageEnvoi();
                            smsEnvoi.Message = sms;

                            //sauvegarde
                            dbContext.MessageEnvoi.InsertOnSubmit(smsEnvoi);
                        }*/


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
            }
        }

        //methode qui communique avec le modem pour une demande d'envoi de SMS
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
            if (sms.Message.messageTexte == null && sms.Message.messagePDU!=null)
            {
                messageAEnvoyer = sms.Message.messagePDU;
                //on recupere la reference du message envoye
                reference = modem.sendTramePDU(messageAEnvoyer); 

                
            }
            else if (sms.Message.messageTexte != null && sms.Message.messagePDU == null) //si trame PDU est nul alors c'est un message Texte (si pas nul aussi)
            {
                messageAEnvoyer = sms.Message.messageTexte;
                //on recupere la reference du message envoye
                reference = modem.sendSMSPDU(sms.Message.noDestinataire, messageAEnvoyer, demandeAccuse, sms.Message.Encodage.libelleEncodage, sms.dureeValidite.Value); 
            }

            //verifie s'il y a une erreur
            if (reference.Contains("ERROR"))
            {
                //on passe le statut du sms a erreur
                sms.Message.Statut = (from stat in dbContext.Statut where stat.libelleStatut == "Erreur" select stat).First();
            }
            else //on sauvegarde la reference en BD
            {
                sms.referenceEnvoi = reference;            
            }
            
            

            //on envoie le sms si le contenu n'est pas nul
            if (messageAEnvoyer != null)
            {
                //on complete les informations sur l'envoi des SMS
                sms.Message.noEmetteur = numeroModem;
                sms.accuseReceptionRecu = 0; //pas encore recu

                //on passe le sms en statut envoye
                sms.Message.Statut = statutEnvoye;
                sms.dateEnvoi = DateTime.Now; //date du jour

            }
            else // erreur
            {
                //on passe le statut du sms a erreur
                sms.Message.Statut = (from stat in dbContext.Statut where stat.libelleStatut == "Erreur" select stat).First();
            }


            //on valide les changements dans la BD
            dbContext.SubmitChanges();

        }

        #endregion
    }
}

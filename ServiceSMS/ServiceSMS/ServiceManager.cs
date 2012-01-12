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
                        // to do : fqire le process de lecture
                        // ...
                        _dateDerniereLecture = DateTime.Now;
                    }


                    // tache de envoi des sms
                    if (_dateDerniereEnvoi.Add(_dureeEntre2Envois) < DateTime.Now)
                    {
                        //on recupere tous les message en attente d'envoi dans la base de donnees
                        Message[] lesSMSAEnvoyer = (from msg in dbContext.Message
                                                    join status in dbContext.Statut
                                                    on msg.idStatut equals status.idStatut
                                                    where status.libelleStatut == "En attente"
                                                    select msg).ToArray();

                        //on recupere le statut "ENVOYE"
                        Statut statutEnvoye = (from stat in dbContext.Statut where stat.libelleStatut == "Envoye" select stat).First();

                        if (lesSMSAEnvoyer.Length > 0)
                            modem.connectToModem();
                        //pour chaque message a envoyer
                        foreach (Message sms in lesSMSAEnvoyer)
                        {
                            //on envoie le sms
                            modem.sendSMSPDU(sms.noDestinataire, sms.messagePDU, true);

                            System.Console.WriteLine(sms.messagePDU);

                            sms.noEmetteur = numeroModem;

                            //on passe le sms en statut envoye
                            sms.Statut = statutEnvoye;


                            //on cree un SMSEnvoi pour sauvegarder les infos sur l'envoi des SMS
                            MessageEnvoi smsEnvoi = new MessageEnvoi();
                            smsEnvoi.dateEnvoi = new DateTime();
                            smsEnvoi.Message = sms;

                            //sauvegarde
                            dbContext.MessageEnvoi.InsertOnSubmit(smsEnvoi);
                            

                        }

                        //on valide les changements dans la BD
                        dbContext.SubmitChanges();

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
                _busy = false;
                //LogHelper.Trace("timerService_Elapsed:" + ex.Message, LogHelper.EnumCategorie.Erreur); 
            }
        }
        #endregion
    }
}

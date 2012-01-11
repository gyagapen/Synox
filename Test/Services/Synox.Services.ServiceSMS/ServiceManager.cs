using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Timers;
using Synox.Services.ServiceSMS.Helpers;
using Synox.Services.ServiceSMS.Entity.Helpers;

namespace Synox.Services.ServiceSMS
{
    /// <summary>
    /// Classe de gestion des services du Service (LOL)
    /// </summary>
    public class ServiceManager
    {
        #region Proprietes
        /// <summary>
        /// Flag pour savoir si les services sont démarrés
        /// </summary>
        public bool IsStarted = false;

        #region Taches de fond
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
        private DateTime _dateDerniereAction;
        /// <summary>
        /// Initialisé à 1 heure
        /// </summary>
        private TimeSpan _dureeEntre2Actions = new TimeSpan(1, 0, 0);
        /// <summary>
        /// Date de la derniere execution du timer
        /// </summary>
        private DateTime _dateDerniereEnvoiSms;
        /// <summary>
        /// Initialisé à 1 minutes
        /// </summary>
        private TimeSpan _dureeEntre2LectureSms = new TimeSpan(0, 0, 15);
        /// <summary>
        /// Date de la derniere execution du timer
        /// </summary>
        private DateTime _dateDerniereEnvoiPlateformeSolem;
        /// <summary>
        /// Initialisé à 5 minutes
        /// </summary>
        private TimeSpan _dureeEntre2EnvoiPlateformeSolem = new TimeSpan(0, 5, 0);
        /// <summary>
        /// Date de la derniere execution du timer
        /// </summary>
        private DateTime _dateDerniereEnvoiPlateformeBirdy;
        /// <summary>
        /// Initialisé à 5 minutes
        /// </summary>
        private TimeSpan _dureeEntre2EnvoiPlateformeBirdy = new TimeSpan(0, 5, 0);
        /// <summary>
        /// Date de la derniere execution du timer
        /// </summary>
        private DateTime _dateDerniereEnvoiPlateformeGeocity;
        /// <summary>
        /// Initialisé à 5 minutes
        /// </summary>
        private TimeSpan _dureeEntre2EnvoiPlateformeGeocity = new TimeSpan(0, 5, 0);
        #endregion

        #region Reseau (Socket, Thread, Port)
        /// <summary>
        /// Liste des thread en cours (communications tcp avec les birdy)
        /// </summary>
        private List<Thread> _listThread;
        /// <summary>
        /// Liste des socket en cours d'utilisation
        /// </summary>
        private List<Net.ConnexionSockets> _serversTcp;
        /// <summary>
        /// Liste des ports d'écoute Birdy
        /// </summary>
        private List<int> _listePortEcoute = null;
        #endregion
        #endregion

        #region Constructeur
        /// <summary>
        /// Constructeur
        /// </summary>
        public ServiceManager()
        {
            _listePortEcoute = new List<int>() { EnvironmentApplicationHelper.PortEcoute };

            _listThread = new List<Thread>();
            _serversTcp = new List<Net.ConnexionSockets>();

            // Taches de fons toutes les 2 secondes
            _timerService = new System.Timers.Timer();
            _timerService.Interval = 2000;
            _timerService.Elapsed += new ElapsedEventHandler(timerService_Elapsed);

            _dateDerniereAction = DateTime.Now;
            _dateDerniereEnvoiSms = DateTime.Now.AddMinutes(-2);
            _dateDerniereEnvoiPlateformeSolem = DateTime.Now.AddMinutes(-10);
            _dateDerniereEnvoiPlateformeBirdy = DateTime.Now.AddMinutes(-10);
            _dateDerniereEnvoiPlateformeGeocity = DateTime.Now.AddMinutes(-10);
        }
        #endregion

        #region Start / Stop
        /// <summary>
        /// Lancement du Timer
        /// </summary>
        public void Start()
        {
            // Démarrage des taches de fond
            IsStarted = true;
            this._timerService.Start();

            // Démarrage du service d'écoute Birdy
            this.StartServerAdmin(EnvironmentApplicationHelper.PortAdmin);
            foreach (int portEcoute in _listePortEcoute)
            {
                this.StartServer(portEcoute);
            }



        }
        /// <summary>
        /// Arret des services
        /// </summary>
        public void Stop()
        {
            // Arret des taches de fond
            this._timerService.Stop();
            // Arret des services d'écoute
            foreach (int portEcoute in _listePortEcoute)
            {
                this.StopServer(portEcoute);
            }
            this.StopServer(EnvironmentApplicationHelper.PortAdmin);


            IsStarted = false;
        }
        /// <summary>
        /// Arret des thread en cours
        /// </summary>
        public void Dispose()
        {
            foreach (Thread action in _listThread)
            {
                _listThread.Remove(action);
                action.Abort();
            }
        }
        #endregion

        #region Socket Start/Stop
        /// <summary>
        /// debute l'ecoute sur le port TCP de la machine
        /// </summary>
        private void StartServerAdmin(int port)
        {
            Net.ConnexionSockets serverTcp;
            try
            {
                serverTcp = new Net.ConnexionSockets(true);
                serverTcp.StartServer(port);
                _serversTcp.Add(serverTcp);
                Console.WriteLine("StartServerAdmin: port " + port + " en écoute...");
                LogHelper.Trace("StartServerAdmin:Server Socket sur " + port + " : En écoute...", LogHelper.EnumCategorie.Information);
            }
            catch (Exception e)
            {
                LogHelper.Trace("Erreur Start Server Socket : " + e.Message, LogHelper.EnumCategorie.Erreur);
            }
        }
        /// <summary>
        /// debute l'ecoute sur le port TCP de la machine
        /// </summary>
        private void StartServer(int port)
        {
            Net.ConnexionSockets serverTcp;
            try
            {
                serverTcp = new Net.ConnexionSockets();
                serverTcp.StartServer(port);
                _serversTcp.Add(serverTcp);
                Console.WriteLine("StartServer: port " + port + " en écoute...");
                LogHelper.Trace("StartServer:Server Socket sur " + port + " : En écoute...", LogHelper.EnumCategorie.Information);
            }
            catch (Exception e)
            {
                LogHelper.Trace("Erreur Start Server Socket : " + e.Message, LogHelper.EnumCategorie.Erreur);
            }
        }
        /// <summary>
        /// Ferme la socket d'ecoute dont le port est défini en parametre
        /// </summary>
        /// <param name="port">0 pour tous</param>
        private void StopServer(int port)
        {
            try
            {
                // parcours des socket pour fermer celle dont le port est spécifié en parametre
                foreach (Net.ConnexionSockets serverTcp in _serversTcp)
                {
                    // vérifie que le port de la socket est celui défini dans les parametres
                    if (port == 0 || port == serverTcp.Port)
                    {
                        // fermeture de la socket
                        if (serverTcp != null) { serverTcp.Close(); }
                        LogHelper.Trace("Server Socket : Arrêté", LogHelper.EnumCategorie.Information);
                        if (port > 0)
                            break;
                    }

                }
            }
            catch (Exception e)
            {
                LogHelper.Trace("Erreur Stop Server Socket : " + e.Message, LogHelper.EnumCategorie.Erreur);
            }
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
                    // toutes les heures
                    if (_dateDerniereAction.Add(_dureeEntre2Actions) < DateTime.Now)
                    {
                        // supprime les dossiers log de plus de 2 mois
                        FileHelper.PurgeDossier(EnvironmentApplicationHelper.DossierLogs, EnvironmentApplicationHelper.DureeDeVieDesLogs);

                        _dateDerniereAction = DateTime.Now;
                    }

                    // définition des projets auxquels sont associés les SMS
                    SmsHelper.AffecteProjet();
                    SmsReceptionHelper.AffecteProjet();

                    // toutes les minutes : si actif dans le App.Config
                    if (EnvironmentApplicationHelper.EnvoiSmsActif && _dateDerniereEnvoiSms.Add(_dureeEntre2LectureSms) < DateTime.Now)
                    {
                        EnvironmentApplicationHelper.EnvoiSmsActif = false;

                        int nombreSmsEnvoyes = SmsHelper.SendSmsEnAttente();
                        nombreSmsEnvoyes = SmsHelper.UpdateStatusSmsEnAttente();

                        _dateDerniereEnvoiSms = DateTime.Now;
                        EnvironmentApplicationHelper.EnvoiSmsActif = true;
                    }

                    // toutes les 5 minutes : si actif dans le App.Config
                    if (EnvironmentApplicationHelper.EnvoiPlateformeSolem && _dateDerniereEnvoiPlateformeSolem.Add(_dureeEntre2EnvoiPlateformeSolem) < DateTime.Now)
                    {
                        int nombreSmsTraites = SmsReceptionHelper.SolemEnvoiPlateformeIp();
                        _dateDerniereEnvoiPlateformeSolem = DateTime.Now;
                    }

                    // toutes les 5 minutes : si actif dans le App.Config
                    if (EnvironmentApplicationHelper.EnvoiPlateformeBirdy && _dateDerniereEnvoiPlateformeBirdy.Add(_dureeEntre2EnvoiPlateformeBirdy) < DateTime.Now)
                    {
                        int nombreSmsTraites = SmsReceptionHelper.BirdyEnvoiPlateformeIp();
                        _dateDerniereEnvoiPlateformeBirdy = DateTime.Now;
                    }

                    // toutes les 5 minutes : si actif dans le App.Config
                    if (EnvironmentApplicationHelper.EnvoiPlateformeGeocity && _dateDerniereEnvoiPlateformeGeocity.Add(_dureeEntre2EnvoiPlateformeGeocity) < DateTime.Now)
                    {
                        int nombreSmsTraites = GeocityHelper.SendSmsEnAttente();
                        _dateDerniereEnvoiPlateformeGeocity = DateTime.Now;
                    }

                    _busy = false;
                }
            }
            catch (Exception ex) { _busy = false; LogHelper.Trace("timerService_Elapsed:" + ex.Message, LogHelper.EnumCategorie.Erreur); }
        }
        #endregion
    }
}

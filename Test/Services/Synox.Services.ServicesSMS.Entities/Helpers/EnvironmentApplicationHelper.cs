using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Configuration;
using Synox;
using Synox.Exceptions;

namespace Synox.Services.ServiceSMS
{
    /// <summary>
    /// Class permettant de stocker des variables static dans le projet
    /// </summary>
    /// <author>Joffrey VERDIER</author>
    /// <date>10/12/2008</date>
    /// <changelog>
    ///  <change author="Joffrey VERDIER" date="10/12/2008">ajout du chemin de la base de données</change>
    /// </changelog>
    public class EnvironmentApplicationHelper
    {
        public static new Version ApplicationVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

        /// <summary>
        /// Culture Francaise utilisée principalement pour les conversions de Type
        /// </summary>
        public static System.Globalization.CultureInfo CultureFr { get; set; }
        /// <summary>
        /// Culture Us utilisée principalement pour les conversions de Type
        /// </summary>
        public static System.Globalization.CultureInfo CultureUs { get; set; }
        /// <summary>
        /// Port d'écoute du service
        /// </summary>
        public static bool EnvoiSmsActif { get; set; }
        /// <summary>
        /// Port d'écoute du service
        /// </summary>
        public static bool EnvoiSmsRatcomActif { get; set; }
        /// <summary>
        /// Port d'écoute du service
        /// </summary>
        public static bool EnvoiPlateformeSolem { get; set; }
        /// <summary>
        /// Port d'écoute du service
        /// </summary>
        public static bool EnvoiPlateformeBirdy { get; set; }
        /// <summary>
        /// Port d'écoute du service
        /// </summary>
        public static bool EnvoiPlateformeGeocity { get; set; }
        /// <summary>
        /// Port d'écoute du service
        /// </summary>
        public static bool ModeConsole { get; set; }
        /// <summary>
        /// Port d'écoute du service
        /// </summary>
        public static int PortEcoute { get; set; }
        /// <summary>
        /// Port d'écoute Administrateur du service
        /// </summary>
        public static int PortAdmin { get; set; }
        /// <summary>
        /// Nom de l'application
        /// </summary>
        public static string ApplicationName { get; set; }
        /// <summary>
        /// Numéro de Téléphone du routeur SMS
        /// </summary>
        public static string NumeroGsmRouteurSms { get; set; }

        /// <summary>
        /// ConnectionString pour la connexion à la base de données SqlServer
        /// </summary>
        public static string ConnectionString { get; set; }
        #region Chemins
        /// <summary>
        /// Donne le dossier où s'execute actuellement le programme
        /// </summary>
        public static string ApplicationPath { get; set; }
        /// <summary>
        /// Dossier des logs
        /// </summary>
        public static string DossierLogs { get; set; }
        /// <summary>
        /// Chemin complet du repertoire temporaire
        /// </summary>
        public static String DossierTemp { get; set; }
        /// <summary>
        /// Chemin complet du fichier FichierSmsErreur
        /// </summary>
        public static String FichierSmsErreur { get; set; }
        #endregion

        #region Durees
        public static TimeSpan DureeDeVieDesLogs { get; set; }
        #endregion


        public static string SmsServer { get; set; }
        public static int SmsServerPort { get; set; }
        public static string SmsRequestTest { get; set; }

        public static void ChargementSettings()
        {
            string parametresIntrouvables = string.Empty;
            try
            {
                CultureFr = System.Globalization.CultureInfo.GetCultureInfo("fr-Fr");
                CultureUs = System.Globalization.CultureInfo.GetCultureInfo("en-US");
                ApplicationPath = AppDomain.CurrentDomain.BaseDirectory;

                if (ConfigurationManager.AppSettings["EnvoiSmsActif"] == null)
                    parametresIntrouvables += "EnvoiSmsActif;";
                else EnvoiSmsActif = Convert.ToBoolean(ConfigurationManager.AppSettings["EnvoiSmsActif"]);

                if (ConfigurationManager.AppSettings["EnvoiPlateformeSolem"] == null)
                    parametresIntrouvables += "EnvoiPlateformeSolem;";
                else EnvoiPlateformeSolem = Convert.ToBoolean(ConfigurationManager.AppSettings["EnvoiPlateformeSolem"]);

                if (ConfigurationManager.AppSettings["EnvoiPlateformeBirdy"] == null)
                    parametresIntrouvables += "EnvoiPlateformeBirdy;";
                else EnvoiPlateformeBirdy = Convert.ToBoolean(ConfigurationManager.AppSettings["EnvoiPlateformeBirdy"]);

                if (ConfigurationManager.AppSettings["EnvoiPlateformeGeocity"] == null)
                    parametresIntrouvables += "EnvoiPlateformeGeocity;";
                else EnvoiPlateformeGeocity = Convert.ToBoolean(ConfigurationManager.AppSettings["EnvoiPlateformeGeocity"]);

                if (ConfigurationManager.AppSettings["PortEcoute"] == null)
                    parametresIntrouvables += "PortEcoute;";
                else PortEcoute = Convert.ToInt32(ConfigurationManager.AppSettings["PortEcoute"]);

                if (ConfigurationManager.AppSettings["PortEcouteAdministration"] == null)
                    parametresIntrouvables += "PortEcouteAdministration;";
                else PortAdmin = Convert.ToInt32(ConfigurationManager.AppSettings["PortEcouteAdministration"]);

                if (ConfigurationManager.AppSettings["SmsServer"] == null)
                    parametresIntrouvables += "SmsServer;";
                else SmsServer = ConfigurationManager.AppSettings["SmsServer"];

                if (ConfigurationManager.AppSettings["SmsServerPort"] == null)
                    parametresIntrouvables += "SmsServerPort;";
                else SmsServerPort = Convert.ToInt32(ConfigurationManager.AppSettings["SmsServerPort"]);

                if (ConfigurationManager.AppSettings["SmsRequestTest"] == null)
                    parametresIntrouvables += "SmsRequestTest;";
                else SmsRequestTest = ConfigurationManager.AppSettings["SmsRequestTest"].Replace("|", "&");

                if (ConfigurationManager.AppSettings["DossierLogs"] == null)
                    parametresIntrouvables += "DossierLogs;";
                else DossierLogs = ConfigurationManager.AppSettings["DossierLogs"];

                if (ConfigurationManager.AppSettings["LifeTimeOfLogMonth"] == null)
                    parametresIntrouvables += "LifeTimeOfLogMonth;";
                else DureeDeVieDesLogs = new TimeSpan(Convert.ToInt32(ConfigurationManager.AppSettings["LifeTimeOfLogMonth"]), 0, 0, 0);

                if (ConfigurationManager.AppSettings["DossierTemp"] == null)
                    parametresIntrouvables += "DossierTemp;";
                else DossierTemp = ConfigurationManager.AppSettings["DossierTemp"];

                if (ConfigurationManager.AppSettings["FichierSmsErreur"] == null)
                    parametresIntrouvables += "FichierSmsErreur;";
                else FichierSmsErreur = ConfigurationManager.AppSettings["FichierSmsErreur"];

                if (ConfigurationManager.AppSettings["NumeroGsmRouteurSms"] == null)
                    parametresIntrouvables += "NumeroGsmRouteurSms;";
                else NumeroGsmRouteurSms = ConfigurationManager.AppSettings["NumeroGsmRouteurSms"];

                if (ConfigurationManager.ConnectionStrings["ConnectionString"] == null)
                    parametresIntrouvables += "ConnectionString;";
                else ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                if (!string.IsNullOrEmpty(parametresIntrouvables))
                    throw new Exception("Des variables n'ont pas été trouvées dans le fichier de configuration");

            }
            catch
            {
                StreamWriter writer = new StreamWriter(Path.Combine(ApplicationPath, "ErreurCritique.txt"));
                writer.WriteLine(string.Format("{0}:[v{1}] {2}", DateTime.Now.ToString("HH:mm:ss.fff", CultureFr), ApplicationVersion.ToString(2), "Les paramètres '" + parametresIntrouvables + "' ne figurent pas dans le fichier de configuration de l'application."));
                writer.Close();
                writer.Dispose();
                throw;
            }
        }

    }
}

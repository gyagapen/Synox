using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;

namespace Synox.Services.ServiceSMS
{
    /// <summary>
    /// 
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        /// Enumération de catégorie (erreur, information, alerte)
        /// </summary>
        public enum EnumCategorie
        {
            Erreur = 1,
            Alerte = 2,
            Information = 4
        }
        
        /// <summary>
        /// Enregistrement des infos dans le journal l'application
        /// </summary>
        /// <param name="message"></param>
        /// <param name="severite"></param>
        public static void Trace(object message, Exception ex, EnumCategorie severite)
        {
            Trace(message +" >> "+ ((ex.InnerException != null) ? ex.InnerException.Message : ex.Message), severite);
        }
        /// <summary>
        /// Enregistrement des infos dans le journal l'application
        /// </summary>
        /// <param name="message"></param>
        /// <param name="severite"></param>
        public static void Trace(object message, EnumCategorie severite)
        {
            StreamWriter writer = null;
            string fileName = "{0}_{1}.log";
            try
            {
                if (string.IsNullOrEmpty(EnvironmentApplicationHelper.DossierLogs)) throw new Exception("Le dossier Log n'est pas spécifié dans le fichier de configuration de l'application. (key=\"DossierLog\" value =\"\"");
                if (!Directory.Exists(EnvironmentApplicationHelper.DossierLogs)) Directory.CreateDirectory(EnvironmentApplicationHelper.DossierLogs);
                writer = new StreamWriter(
                    Path.Combine(EnvironmentApplicationHelper.DossierLogs, string.Format(fileName, DateTime.Now.ToString("yyyy-MM-dd", EnvironmentApplicationHelper.CultureFr), EnvironmentApplicationHelper.ApplicationName)),
                    true, 
                    System.Text.Encoding.UTF8);
                if (message == null) message = "message 'null'";
                writer.WriteLine(string.Format("{0}:[v{1}] [{2}] {3}", DateTime.Now.ToString("HH:mm:ss.fff", EnvironmentApplicationHelper.CultureFr), EnvironmentApplicationHelper.ApplicationVersion.ToString(2), severite, message.ToString()));
                Debug(string.Format("{0}:[v{1}] [{2}] {3}", DateTime.Now.ToString("HH:mm:ss.fff", EnvironmentApplicationHelper.CultureFr), EnvironmentApplicationHelper.ApplicationVersion.ToString(2), severite, message.ToString()), severite);
                writer.Close();

                // si erreur => envoi de mail?
            }
            catch (Exception ex)
            {
                try
                {
                    File.WriteAllText("c:\\logErreur.txt", ex.Message);
                }
                catch { }
                System.Diagnostics.Debug.WriteLine("//////////////////// LOG ERROR /////////////////");
                System.Diagnostics.Debug.WriteLine((ex.InnerException!=null)?ex.InnerException.Message:ex.Message);
            }
            finally
            { 
                if(writer!=null)
                    writer.Dispose();
            }
        }
        private static void Debug(string message, EnumCategorie severite)
        {
            if (EnvironmentApplicationHelper.ModeConsole)
            {
                if (severite == EnumCategorie.Erreur)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.White;
                }
                Console.WriteLine(message);
                if (severite == EnumCategorie.Erreur)
                {
                    Console.ResetColor();
                }
                
            }
        }

        /// <summary>
        /// Range les fichiers logs dans des dossiers 'Mois' et suppriment les logs plus vieux que le nombre de mois spécifié dans le fichier de configuration
        /// </summary>
        /// <param name="dossier"></param>
        public static void PurgeLog(string dossier, TimeSpan dureeDeVie)
        {
            Helpers.FileHelper.PurgeDossier(dossier, dureeDeVie);
        }

    }
}

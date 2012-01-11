using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Synox.Services.ServiceSMS.Helpers
{
    public class FileHelper
    {
        /// <summary>
        /// Range les fichiers logs dans des dossiers 'Mois' et suppriment les logs plus vieux que le nombre de mois spécifié dans le fichier de configuration
        /// </summary>
        /// <param name="dossier"></param>
        public static void PurgeDossier(string dossier, TimeSpan dureeDeVie)
        {
            DirectoryInfo dossierRacine;
            List<FileInfo> fichiersContenus;
            List<DirectoryInfo> sousDossiersMois;
            string dossierMois;
            try
            {
                dossierRacine = new DirectoryInfo(dossier);
                if (!dossierRacine.Exists) dossierRacine.Create();

                // suppression des vieux logs
                fichiersContenus = dossierRacine.GetFiles("*", SearchOption.AllDirectories).ToList();
                foreach (FileInfo f in fichiersContenus)
                {
                    if (f.CreationTime.Add(dureeDeVie) < DateTime.Now)
                    {
                        f.Delete();
                    }
                }

                // rangement des fichiers logs dans dossiers Mois 
                fichiersContenus = dossierRacine.GetFiles("*", SearchOption.TopDirectoryOnly).ToList();
                foreach (FileInfo f in fichiersContenus)
                {
                    if (f.CreationTime < DateTime.Now.Date)
                    {
                        // rangement dans dossier
                        dossierMois = f.CreationTime.ToString("yyyy-MM", EnvironmentApplicationHelper.CultureFr);
                        if (!Directory.Exists(Path.Combine(dossier, dossierMois))) Directory.CreateDirectory(Path.Combine(dossier, dossierMois));
                        f.MoveTo(Path.Combine(Path.Combine(dossier, dossierMois), f.Name));
                    }
                }

                // suppression des dossiers vides
                sousDossiersMois = dossierRacine.GetDirectories("*", SearchOption.AllDirectories).ToList();
                foreach (DirectoryInfo d in sousDossiersMois)
                {
                    if (d.GetFiles().Length == 0)
                    {
                        d.Delete();
                    }
                }


            }
            catch (Exception ex)
            {
                LogHelper.Trace("PurgeDossier : " + ((ex.InnerException != null) ? ex.InnerException.Message : ex.Message), LogHelper.EnumCategorie.Alerte);
                // pas arret l'application pour ce genre d'erreur mais plutot surveiller les logs pour voir l'erreur
            }
        }
    }
}

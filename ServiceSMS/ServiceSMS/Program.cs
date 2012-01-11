using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace ServiceSMS
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        static void Main(string[] args)
        {

            
#if DEBUG
            args = new string[1] { "/cons" };
#endif
            if (args.Length > 0)
            {

                ServiceManager serverSync = new ServiceManager();
                serverSync.Start();

                #region Attend une saisie de la commande 'quit' pour sortir
                string quit = string.Empty;
                Console.Title = "Service SMS";

                while (string.IsNullOrEmpty(quit) || !quit.Equals("quit"))
                {
                    Console.WriteLine("///////////////////////////////");
                    Console.WriteLine("taper la commande 'quit' pour quitter l'application\r\n");
                    quit = Console.ReadLine();
                    switch (quit.ToLower())
                    {
                        case "clr":
                        case "clear":
                            Console.Clear();
                            break;

                        case "test":

                            Console.WriteLine("test de jo");
                            break;
                    }
                    Console.WriteLine(string.Empty);
                }
                #endregion

                serverSync.Stop();
                serverSync.Dispose();
            }
            else{
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
			{ 
				new ServiceSMS() 
			};
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}

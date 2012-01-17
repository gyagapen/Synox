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

                            modemSMS modSMS = new modemSMS("COM11");
                            modSMS.connectToModem();
                            modSMS.sendTramePDU("0001000b913376650111F800f6002930000000000066010b000001001b04343731314e85b65950683b590831323334353637380000126701");
                            modSMS.readDeliveryReport();
                            modSMS.disconnectToModem();
                            break;

                        case "read":
                            modemSMS modSMSRead = new modemSMS("COM11");
                            modSMSRead.connectToModem();
                            modSMSRead.readPDUMessage();
                            modSMSRead.disconnectToModem();
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

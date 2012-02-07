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

            //si mode debug on passe un argument a l'application     
#if DEBUG
            args = new string[1] { "/cons" };
#endif
            if (args.Length > 0) //si un argument est passé à l'application
            {
                //on lance l'application en mode console
                ServiceManager serverSync = new ServiceManager();
                serverSync.Start();


                //boucle qui permet de passer des commandes a l'application pendant l'execution
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
                            //modSMS.sendTramePDU("0001000b913376650111F800f6002930000000000066010b000001001b04343731314e85b65950683b590831323334353637380000126701");
                            modSMS.sendTramePDU("0021000A816057161081000015F4F29C0E0A8FC7F57919242F8FCB707AFAED06");
                            modSMS.readDeliveryReport();
                            modSMS.disconnectToModem();
                            break;

                        case "read":
                            modemSMS modSMSRead = new modemSMS("COM11");
                            modSMSRead.connectToModem();
                            modSMSRead.readPDUMessage();
                            modSMSRead.disconnectToModem();
                            break;


                        case "test vp":
                            modemSMS modSMSVP = new modemSMS("COM11");

                            TimeSpan tim = new TimeSpan(1, 0, 0, 0);

                            int intValue = modSMSVP.calculValidityPeriod(tim);
                            Console.WriteLine("INT VALUE : " + intValue);

                            TimeSpan tim2 = modSMSVP.decoderValidityPeriod(intValue);

                            Console.WriteLine("Redecodage : " + tim2.Days + ", " + tim2.Hours + ", " + tim2.Minutes);

                            break;
                    }
                    Console.WriteLine(string.Empty);
                }
                #endregion

                serverSync.Stop();
                serverSync.Dispose();
            }
            else //on lance l'application en tant que service windows
            {
                //on instancie le service
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] 
			    { 
				            new ServiceSMS() 
			    };
                
                //on lance le service
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}

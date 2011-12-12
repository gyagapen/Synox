using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace ConsoleApplicationTestSMS
{
    class Program
    {
        const string portCom = "COM5";
        const string numero = "0625123338";

        static void Main(string[] args)
        {
            //ouverture de la connexion

            modemSMS modSMS = new modemSMS(portCom);
            modSMS.connectToModem();

            //modSMS.Send("ATE0");

            //on supprime tous les messages
            //modSMS.deleteAllSMS();


            for (int i = 1; i <= 3; i++)
            {
                //modSMS.sendSMS(numero, i+" Galaxy Tab a vendre ! ");
            }


            //modSMS.sendSMSPDU("toto2", numero, true);


            ///modSMS.readPDUMessage();


          modSMS.sendSMSText("test mode text", "0604655154");


            //modSMS.readPDUMessage();

            
           //modSMS.readAllSMSText();


            Console.Out.WriteLine("Appuyez sur une touche pour quitter...");
            Console.Read();
        }

    }
}

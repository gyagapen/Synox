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
        const string numeroG = "0625123338";
        const string numeroY = "0675610118";
        const string numeroM = "0604655154";
        const string statutSyagapen = "Andouille !";

        static void Main(string[] args)
        {
            //ouverture de la connexion

            modemSMS modSMS = new modemSMS(portCom);
            modSMS.connectToModem();

            //modSMS.Send("ATE0");

            //modSMS.Send("AT+CSMP?");
            // Paramètre le modem pour les accusés de réception
            modSMS.Send("AT+CSMP=7,167,0,0");

            //on supprime tous les messages
            //modSMS.deleteAllSMS();


            for (int i = 1; i <= 3; i++)
            {
                //modSMS.sendSMS(numero, i+" Galaxy Tab a vendre ! ");
            }


            modSMS.sendSMSPDU("Test accusé réception PDU 15h", numeroM, false);
            //modSMS.sendSMSText(numeroG, "Test accuse reception Texte");


            //modSMS.readPDUMessage();
            modSMS.readAllSMSText();


            //modSMS.sendSMSText(numeroM, "test mode texte");


            Console.Out.WriteLine("Appuyez sur une touche pour quitter...");
            Console.Read();
        }

    }
}

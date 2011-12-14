using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace ConsoleApplicationTestSMS
{
    class Program
    {
        const string portCom = "COM2";
        const string numeroG = "0625123338";
        const string numeroY = "0675610118";
        const string numeroM = "0604655154";

        //TEST 3

        static void Main(string[] args)
        {
            //ouverture de la connexion

            modemSMS modSMS = new modemSMS(portCom);
            modSMS.connectToModem();

            //modSMS.Send("ATE0");

            // Paramètre le modem pour les accusés de réception
            modSMS.Send("AT+CSMP=17,167,0,16");

            //on supprime tous les messages
            //modSMS.deleteAllSMS();


            for (int i = 1; i <= 10; i++)
            {
                modSMS.sendSMSText("0630854796", i+" saucisson(s) d'Auvergne ! ");
            }



            modSMS.sendSMSPDU(numeroY, "Test accusé réception PDU 15h", false);
            //modSMS.sendSMSText(numeroG, "Test accuse reception Texte");


            //modSMS.readPDUMessage();
            //modSMS.readAllSMSText();


            //modSMS.sendSMSText(numeroM, "test mode texte");


            Console.Out.WriteLine("Appuyez sur une touche pour quitter...");
            Console.Read();
        }

    }
}

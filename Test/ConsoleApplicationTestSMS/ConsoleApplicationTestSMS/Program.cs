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

        //TEST 3

        static void Main(string[] args)
        {
            //ouverture de la connexion

            modemSMS modSMS = new modemSMS(portCom);
            modSMS.connectToModem();

            //modSMS.Send("ATE1");

            // Paramètre le modem pour les accusés de réception
            //modSMS.Send("AT+CSMP=49,0,0,0 ");

            //on supprime tous les messages
            //modSMS.deleteAllSMS();
            //modSMS.Send("AT+CMGD=20,0");


            for (int i = 1; i <= 5; i++)
            {
                //modSMS.sendSMSText("0630854796", i+" saucisson(s) d'Auvergne ! ");

            }



            //modSMS.Send("AT+CSMP=17,167,0,16");
            //modSMS.sendSMSPDU(numeroG, "Alors, ca fait quoi d'etre flashe ?", true);

            //modSMS.Send("AT+CSMP=49,167,0,0");
            //modSMS.Send("AT+CNMI=2,1,3,2,1");

            //modSMS.Send("AT+CNMI=2,1,1,2,1");
            modSMS.Send("AT+CNMI?");
            //modSMS.sendSMSPDU(numeroG, "Test accusé réception PDU", true);
            //modSMS.Send("AT+CNMI=1,1,0,0,0");

            //modSMS.sendSMSPDU(numeroY, "Test accusé réception PDU 17h", true);
            //modSMS.sendSMSPDU("0680787112", "Alors, ca fait quoi d'etre spamme ?", true);

            //modSMS.sendSMSText(numeroY, "Test accusé réception PDU 15h");
            //modSMS.readPDUMessage();
            //modSMS.sendSMSText(numeroG, "Test accuse reception Texte");

            //modSMS.Send("AT+CMGF=0");
            //modSMS.Send("AT+CPMS=\"SR\"");
            //modSMS.Send("AT+CMGL=\"ALL\""); 
            //modSMS.Send("AT+CMGL=4");

            modSMS.readDeliveryReport();

            //modSMS.Send("AT+CPMS?");

            //modSMS.readPDUMessage();
            //modSMS.readAllSMSText();


            //modSMS.sendSMSText(numeroM, "test mode texte");

            //modSMS.Recv();
            Console.Out.WriteLine("Appuyez sur une touche pour quitter...");
            
            Console.Read();
            
        }

    }
}

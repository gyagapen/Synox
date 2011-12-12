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


            //modSMS.sendSMSPDU("0604655154", "test2011");


            modSMS.readPDUMessage();

            //modSMS.Send("AT+CSMP?");

            //modSMS.sendSMSPDU("0604655154", "test mode text periode validite 2d PDU test 455563");


            //modSMS.readPDUMessage();

            
           //modSMS.readAllSMSText();


            Console.Out.WriteLine("Appuyez sur une touche pour quitter...");
            Console.Read();
        }

    }
}

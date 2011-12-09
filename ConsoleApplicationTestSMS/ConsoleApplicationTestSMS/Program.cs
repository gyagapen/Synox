using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace ConsoleApplicationTestSMS
{
    class Program
    {
      
         

        static void Main(string[] args)
        {
            //ouverture de la connexion

            modemSMS modSMS = new modemSMS("COM2");
            modSMS.connectToModem();

            

           for (int i = 1; i <= 3; i++)
            {
                //Console.Out.WriteLine("no :"+i);
                //modSMS.sendSMS("0675610118", i+" Galaxy Tab a vendre ! ");
                //modSMS.sendSMSPDU(i+" salami(s)", "0675610118");

            }

          /* modSMS.sendSMSPDU("Du the ? 1/1000 msg(s) envoye", "0622031216");
           modSMS.sendSMSPDU("Du the ? 2/1000 msg(s) envoye", "0622031216");
           modSMS.sendSMSPDU("Du the ? 3/1000 msg(s) envoye", "0622031216");*/
            


            //modSMS.sendSMSPDU("toto");
           //modSMS.Send("ATE1");
           //modSMS.Recv();
            modSMS.readAllSMS();

          

            //modSMS.ExecCommand("AT", 300, "No phone connected");
            //modSMS.writeOnPort("ATI");

            //on affiche la reponse
            //Console.Out.WriteLine("Message du modem : "+modSMS.Recv());
            //Console.Out.WriteLine("Message du modem : " + modSMS.Recv());
            Console.Out.WriteLine("Lecture effectue");
            Console.Read();


        }





    }
}

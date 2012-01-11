using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Synox.Services.ServiceSMS.Helpers
{

    public class PduHelper
    {
        //byte[] bytearray;
        // List<string> octet = new List<string>(); 
        //string temp;
        //string pad="";
        //string PDU = "";
        public static string GetLength(string messagePdu)
        {

            int firstarg = Convert.ToInt32(messagePdu.Substring(0, 2));
            firstarg = firstarg * 2;
            string tmp = messagePdu.Substring(2);
            tmp = tmp.Substring(firstarg);
            firstarg = tmp.Length / 2;
            return Convert.ToString(firstarg);
        }
        public static string GetPDUString(string NumeroCenterAdress,string numeroGsm,string message)
        {
            int textlen = message.Length;
           List<string> septet = stringToSeptet(message);

            string messageHexa = septetToOctet(septet);
            string messagePdu = CreatePDU(NumeroCenterAdress, numeroGsm, textlen, messageHexa);
            return messagePdu;
        }

        public static byte[] HexaStringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }
        public static string ByteArrayToStringHexa(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);

            for (int i = 0; i < ba.Length; i++)       // <-- use for loop is faster than foreach    
                hex.Append(ba[i].ToString("X2"));   // <-- ToString is faster than AppendFormat    

            return hex.ToString();
        } 
        private static List<string> stringToSeptet(string text)
        {
            List<string> septet = new List<string>();
            int msglen = text.Length;
            for (int i = 0; i < msglen; i++)
            {
                byte b = Convert.ToByte(text[i]);
                string space = Convert.ToString(b, 2);
                if (space.Length == 6)
                {
                    space = "0" + space;
                }
                //txtresult.Text += space + "-"; //Converting to binary
                septet.Add(space); //storing in a list
            }
            septet.Add("0110111");
            return septet;
        }

        private static string septetToOctet(List<string> septet)
        {
            List<string> octet = new List<string>(); 
            #region Converting from septet to octet
             //-------------
            int len = 1;
            int j = 0;
            //-----------
            int septetcount = septet.Count;
            //MessageBox.Show(septetcount.ToString());
            //---------------
            while (j < septet.Count - 1)
            {

                string tmp = septet[j]; // storing jth value
                string tmp1 = septet[j + 1]; //storing j+1 th value
                //-------------- Swapping----------
                string mid = SWAP(tmp1);

                //---------------------
                tmp1 = mid;
                tmp1 = tmp1.Substring(0, len);
                //-----------reverse swapping
                string add = SWAP(tmp1);
                //-------------------
                tmp = add + tmp;// +"-";
                tmp = tmp.Substring(0, 8);
                //txtoctet.Text += tmp + " || ";
                octet.Add(tmp);
                len++;
                if (len == 8)
                {
                    len = 1;
                    j = j + 1;
                }
                j = j + 1;

                //}


            }
#endregion

            string messageHexa = "";

            #region Converting from octet to hex
            for (int x = 0; x < octet.Count; x++)
            {
                string oct = octet[x];
                //MessageBox.Show(oct.Length.ToString());
                string Fhalf = oct.Substring(0, 4);
                string Shalf = oct.Substring(4, 4).ToString();
                string hex1 = "";
                string hex2 = "";

                switch (Fhalf)
                {
                    case "0000": hex1 = "0"; break;
                    case "0001": hex1 = "1"; break;
                    case "0010": hex1 = "2"; break;
                    case "0011": hex1 = "3"; break;
                    case "0100": hex1 = "4"; break;
                    case "0101": hex1 = "5"; break;
                    case "0110": hex1 = "6"; break;
                    case "0111": hex1 = "7"; break;
                    case "1000": hex1 = "8"; break;
                    case "1001": hex1 = "9"; break;
                    case "1010": hex1 = "A"; break;
                    case "1011": hex1 = "B"; break;
                    case "1100": hex1 = "C"; break;
                    case "1101": hex1 = "D"; break;
                    case "1110": hex1 = "E"; break;
                    case "1111": hex1 = "F"; break;
                    default: break;
                }

                switch (Shalf)
                {
                    case "0000": hex2 = "0"; break;
                    case "0001": hex2 = "1"; break;
                    case "0010": hex2 = "2"; break;
                    case "0011": hex2 = "3"; break;
                    case "0100": hex2 = "4"; break;
                    case "0101": hex2 = "5"; break;
                    case "0110": hex2 = "6"; break;
                    case "0111": hex2 = "7"; break;
                    case "1000": hex2 = "8"; break;
                    case "1001": hex2 = "9"; break;
                    case "1010": hex2 = "A"; break;
                    case "1011": hex2 = "B"; break;
                    case "1100": hex2 = "C"; break;
                    case "1101": hex2 = "D"; break;
                    case "1110": hex2 = "E"; break;
                    case "1111": hex2 = "F"; break;
                    default: break;
                }


                messageHexa += hex1 + hex2;

            }
            #endregion


            //------------------
            return messageHexa;
        
        }


        private static string CreatePDU(string msgsndrno, string senderno, int textlen, string messageHexa)
        {
            string PDU = "";
            //--------CREATING PDU

            //STEP 1. LENGTH OF MESSAGE CENTRE NUMBER

            const string step1 = "07";

            //STEP 2. 91 for International format and 81 for unknown format

            const string step2 = "91";

            //STEP 3. MEssage centre number in decimal half-octet format 

            //---------
            //WORKING
            string step3 = msgsndrno;
            step3 = DecimalHalfOctet(step3);
            //MessageBox.Show("Message Centre no in half octets: = " + step3);
            //Thread.Sleep(1000);
            //----------

            //STEP 4. 11 (FIRST OCTET FOR MESSAGE DELIVER)

            const string step4 = "11";

            //STEP 5. 000C (message reference number (00) + length of reciver phone no)

            string step5 = "";
            string lenn = "";
            int ttp = senderno.Length;
            byte bb = Convert.ToByte(ttp);
            lenn = bb.ToString("X");//Getting message length in HEX
            if (lenn.Length == 1)
            {
                lenn = "0" + lenn;//padding to make a 2 digit hex 
            }
            step5 = "00" + lenn;
            //STEP 6. RECIEPIENTS NUMBER in decimal half-octet format preceded by 91 for internatioal format
            //Thread.Sleep(200);

             string step6 = senderno;

            step6 = DecimalHalfOctet(step6);
            //Thread.Sleep(100);

            //STEP 7. 00 for default protocol identifier

            const string step7 = "00";

            //STEP 8. 00 for default data coding scheme

            const string step8 = "00";

            //STEP 9. AA (Validity period set to 4 days)

            const string step9 = "AA";

            //STEP 10. LENGTH OF MESSAGE

            string step10 = "";
            byte b = Convert.ToByte(textlen);
            step10 = b.ToString("X");//Getting message length in HEX
            if (step10.Length == 1)
            {
                step10 = "0" + step10;//padding to make a 2 digit hex 
            }


            //STEP 11. ACTUAL USER MESSAGE
            string step11 = messageHexa;

            //STEP 12.send message terminated with  CNTRL^Z (take char of 26)
            //Thread.Sleep(2000);
            PDU = "";
            PDU = step1 + step2 + step3 + step4 + step5 + step2 + step6 + step7 + step8 + step9 + step10 + step11;// +Convert.ToChar(26);
            return PDU;
        }


        #region Common METHODS
        private static string SWAP(string str)
        {
            char[] org = str.ToCharArray();
            char[] swap = new char[org.Length];
            string uy = "";
            int pppp = 0;
            for (int sw = (org.Length - 1); sw >= 0; sw--)
            {
                swap[pppp] = org[sw];
                uy += swap[pppp].ToString();
                pppp++;

            }
            return uy;
        }

        private static string DecimalHalfOctet(string str)
        {
            char[] convert = new char[str.Length];
            convert = str.ToCharArray(0, str.Length);
            str = "";
            int x = 0;
            while (x < convert.Length)
            {
                string tty;
                if (x + 1 == convert.Length)
                    tty = "F" + convert[x].ToString();
                else
                    tty = convert[x + 1].ToString() + convert[x].ToString();
                str = str + tty;
                x = x + 2;
            }
            return str;
        }
        #endregion

        public static byte[] GetBytes(string pduHexa)
        {
            if (string.IsNullOrEmpty(pduHexa))
                return null;
            else
            {
                string pduRestant = pduHexa.Clone().ToString();
                byte[] buffer = new byte[pduHexa.Length / 2];
                int index = 0;
                while (pduRestant.Length >= 2)
                {
                    buffer[index] = ConvertFromHexa(pduRestant.Substring(0, 2));
                    index++;
                    pduRestant = pduRestant.Substring(2);
                }
                return buffer;
            }
        }
        private static byte ConvertFromHexa(string hexValue)
        {
            if (hexValue.Length != 2)
                throw new Exception("Format HexaDecimal invalide : " + hexValue);

            return byte.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);
        }
    }
}



//  =============================================================================
//  Fichier : CommunicationServicesSync.cs
//  Autheur : Joffrey VERDIER
//  Description: classe de transfert Reseau
//  Version : 1.0
//  =============================================================================
//  Historique :
//  DATE		AUTHEUR	        COMMENTAIRE
//	=============================================================================
//	2009	JVER	    Création du fichier
//	=============================================================================
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Linq;
using System.IO;

namespace Synox.Services.ServiceSMS.Net
{
    public class CommunicationServicesSync
    {
        private Socket socketServeur = null;
        private int Port;
        public string DossierDeTravail = @"\\localhost\temp";

        public CommunicationServicesSync()
        {
        }

        /// <summary>
        /// Fermeture de la socket
        /// </summary>
        public void Disconnect()
        {
            if (socketServeur != null)
            {
                try
                {
                    if (socketServeur.Connected) socketServeur.Disconnect(true);
                    socketServeur.Close();
                }
                catch { }
                socketServeur.Dispose();
                socketServeur = null;
            }
        }
        /// <summary>
        /// Connexion au serveur sur une adresse ip et port specifique à définir dans les parametres de l'application
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="myPort"></param>
        /// <returns></returns>
        public IPEndPoint ConnexionServeur(string ip, int myPort, bool isDns = false)
        {
            IPAddress ipAddress;
            Port = myPort;

            if (isDns)
            {
                // construction de l'IPEndPoint
                IPHostEntry IPHost = Dns.GetHostEntry(ip);
                IPAddress[] addr = IPHost.AddressList;
                ipAddress = addr[0];
                foreach (IPAddress address in addr)
                {
                    if (address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipAddress = address;
                        break;
                    }
                }
            }
            else
                ipAddress = IPAddress.Parse(ip);

            IPEndPoint ipepServer = new IPEndPoint(ipAddress, Port);
            ConnexionServeur(ipepServer);
            return ipepServer;
        }

        /* Fonction: ConnexionServeur()
         * 
         * Connexion au serveur sur le point de terminaison sauvegardé dans la classe systeme
         */
        public void ConnexionServeur(IPEndPoint ipepServer)
        {
            socketServeur = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socketServeur.Blocking = true;
            socketServeur.Connect(ipepServer);
        }
        //-------------------------------------------------------------------------------

        #region Envoyer/recevoir
        /// <summary>
        /// Fonction qui envoi en tableau d'octets la chaine de caracteres sur la socket
        /// </summary>
        /// <param name="texte"></param>
        public void EnvoiEnString(string texte, byte[] finSequence = null)
        {
            // on envoi la chaine 
            Byte[] outbytes = new Byte[256];
            int a = System.Text.Encoding.Default.GetBytes(texte, 0, texte.Length, outbytes, 0);
            //if (a > 512)
            //{
            //    int index = 0;
            //    for (int i = 0; i < a / 512 + 1; i++)
            //    {
            //        index += socketServeur.Send(outbytes, index, 512, 0);
            //    }
            //}
            //else
            if (finSequence != null)
            {
                byte[] buffer = new byte[a + finSequence.Length];
                int i = 0;
                for (i = 0; i < a; i++)
                {
                    buffer[i] = outbytes[i];
                }
                for (int j = 0; j < finSequence.Length; j++)
                {
                    buffer[i + j] = finSequence[j];
                }

                socketServeur.Send(buffer, 0, buffer.Length, 0);
                LogHelper.Trace("EnvoiStringToModem: " + System.Text.Encoding.Default.GetString(buffer).Replace("\n", "").Replace("\r", ""), LogHelper.EnumCategorie.Information);
            }
            else
            {
                socketServeur.Send(outbytes, 0, a, 0);
                LogHelper.Trace("EnvoiStringToModem: " + System.Text.Encoding.Default.GetString(outbytes).Replace("\n", "").Replace("\r", ""), LogHelper.EnumCategorie.Information);
            }

        }
        public String RecoitEnString()
        {
            Byte[] buffer = new Byte[256];
            int octets_recu = 0;
            String res = String.Empty;

            System.Threading.Thread.Sleep(1000);

            // démarrage d'un thread

            octets_recu = socketServeur.Receive(buffer, 0, buffer.Length, 0);

            // tant que compteur < 10 sec ou que trame est vide, on attends 1 seconde
            res = System.Text.Encoding.Default.GetString(buffer, 0, octets_recu);

            return res;
        }
        private void Sleep()
        {
            System.Threading.Thread.Sleep(1); // 75 1s/photo // 60 0.9~0.8s/photo
        }
        private void Sleep(int tmp)
        {
            System.Threading.Thread.Sleep(tmp);
        }

        #endregion


        public int Send(byte[] trame)
        {
            return socketServeur.Send(trame, 0, trame.Length, 0);
        }
        internal byte[] Receive()
        {
            return Receive(1024);
        }
        internal byte[] Receive(int taille)
        {
            byte[] trame = new byte[taille];
            int a = socketServeur.Receive(trame);
            return trame;
        }
    }
}


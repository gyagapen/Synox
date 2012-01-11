using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Synox.Services.ServiceSMS;
using Synox.Services.ServiceSMS.Helpers;
using System.Collections;

using Synox.Services.ServiceSMS.Entity;
using Synox.Services.ServiceSMS.Helpers;

namespace Synox.Services.ServiceSMS.Net
{
    public class ClientBirdyTcp
    {
        public static String FichierXmlRecu = String.Empty;
        //Structure qui permet au serveur de stocker le thread et le socket de chaque client
        #region Variables
        private Socket _socketServeur = null;//socketClient
        public event EventHandler<Events.MessageEventArgs> EventSocketsMessage = null;
        int _port;
        #endregion

        #region Contructeur
        /// <summary>
        /// Constructeur qui crée le tableau de clients et la pile fifo de sortie.
        /// </summary>
        /// <param name="socket"></param>
        public ClientBirdyTcp(Socket socket, int port)
        {
            this._socketServeur = socket;
            this._port = port;
        }
        #endregion

        #region StopSocket
        /// <summary>
        /// Fonction qui, après avoir testé si le socket est client ou serveur, stoppe tous
        /// les sockets etles threds lancés. Si aucun socket n'a été créé, la fonction n'a aucun effet.
        /// </summary>
        public void Disconnect()
        {
            string remoteEndPoint = "SocketDisposed";
            try
            {
                if (this._socketServeur != null)
                {
                    remoteEndPoint = this._socketServeur.RemoteEndPoint.ToString();
                    LogHelper.Trace("Disconnect : Fermeture Socket " + this._socketServeur.RemoteEndPoint.ToString(), LogHelper.EnumCategorie.Information);

                    if (_socketServeur.Connected) _socketServeur.Disconnect(true);
                    try { this._socketServeur.Shutdown(SocketShutdown.Both); }
                    catch { }
                    this._socketServeur.Close();
                    this._socketServeur = null;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Trace("Disconnect ERR : Fermeture Socket " + remoteEndPoint + " : " + ex.Message, LogHelper.EnumCategorie.Erreur);
            }
        }
        #endregion


        public void SendMessage(object message)
        {
            if (EventSocketsMessage != null)
                EventSocketsMessage(this, new Events.MessageEventArgs(message.ToString()));

            LogHelper.Trace(message.ToString(), LogHelper.EnumCategorie.Information);
        }

        #region Sleep
        /// <summary>
        /// 1 ms
        /// </summary>
        public void Sleep()
        {
            System.Threading.Thread.Sleep(1); // 75 1s/photo // 60 0.9~0.8s/photo // 50 0.9~0.8s/photo //10
        }
        /// <summary>
        /// en ms
        /// </summary>
        /// <param name="tmp"></param>
        public void Sleep(int tmp)
        {
            System.Threading.Thread.Sleep(tmp);
        }
        #endregion

        #region Traitement des demandes
        public void EcouteClient()
        {
            bool traitementReussi = false;
            try
            {
                System.Threading.Thread.Sleep(100);
                Console.WriteLine("/////////////////////////////////////");
                Console.WriteLine("Communication :");
                string commande = SocketUtil.RecvString(_socketServeur);
                traitementReussi = Traitement(commande);
                this.Disconnect();
                LogHelper.Trace("\r\n/////////////////////////////////////", LogHelper.EnumCategorie.Erreur);
            }
            catch
            {
                this.Disconnect();
            }
        }
        /// <summary>
        /// Traitement de la trame
        /// </summary>
        /// <param name="commande"></param>
        /// <returns></returns>
        public bool Traitement(String commande)
        {
            
            if (string.IsNullOrEmpty(commande))
                return true;
            try
            {
                SmsReception sms = RouteurSmsHelper.DeserializeSms(commande);
                sms = SmsReceptionHelper.SaveReception(sms);
            }
            catch
            {
                SmsHelper.EcritSmsErreur(commande);
                return false;
            }
            return true;
        }
        #endregion

    }


}
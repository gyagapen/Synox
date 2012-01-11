using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Synox.Helpers;

namespace Synox.Services.ServiceSMS.Net
{
    public class ClientTcp
    {
        public static String FichierXmlRecu = String.Empty;
        //Structure qui permet au serveur de stocker le thread et le socket de chaque client
        #region Variables
        private string separateur = "&";
        private Socket _socketServeur = null;//socketClient
        public event EventHandler<Events.MessageEventArgs> EventSocketsMessage = null;
        #endregion

        #region Contructeur
        /// <summary>
        /// Constructeur qui crée le tableau de clients et la pile fifo de sortie.
        /// </summary>
        /// <param name="socket"></param>
        public ClientTcp(Socket socket)
        {
            this._socketServeur = socket;
        }
        #endregion

        #region StopSocket
        /// <summary>
        /// Fonction qui, après avoir testé si le socket est client ou serveur, stoppe tous
        /// les sockets etles threds lancés. Si aucun socket n'a été créé, la fonction n'a aucun effet.
        /// </summary>
        public void Disconnect()
        {
            try
            {
                if (this._socketServeur != null)
                {
                    LogHelper.Trace("Disconnect : Fermeture Socket " + this._socketServeur.RemoteEndPoint.ToString(), LogHelper.EnumCategorie.Information);

                    if (_socketServeur.Connected) _socketServeur.Disconnect(true);
                    this._socketServeur.Close();
                    this._socketServeur = null;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Trace("Disconnect ERR : Fermeture Socket " + this._socketServeur.RemoteEndPoint.ToString() + " : " + ex.Message, LogHelper.EnumCategorie.Erreur);
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
            try
            {
                System.Threading.Thread.Sleep(100);
                string commande = SocketUtil.RecvString(_socketServeur);

                Traitement(commande);
                this.Disconnect();
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
            string[] resultat;
            // decoupage de la chaine reçue grâce au caractere '&' comme en html
            resultat = commande.Split(separateur.ToCharArray());
            if (resultat.Length == 0)
                return false;
            switch (resultat[0])
            {
                #region traitement

                case "help":
                case "h":
                case "?":
                    try
                    {
                        SocketUtil.SendString(_socketServeur, "version\r\nchemin LOG : " + EnvironmentSx.ApplicationPath);
                    }
                    catch (Exception ex)
                    {
                        LogHelper.Trace("Envoi de la trame de config à l'imprimante Colis : " + ex.Message, LogHelper.EnumCategorie.Erreur);
                        SocketUtil.SendString(_socketServeur, ex.Message);
                    }
                    break;
                case "version":
                    SocketUtil.SendString(_socketServeur, System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
                    break;
                #endregion
                default:
                    SendMessage(">>>>Admin.Traitement : aucune commande :" + commande);
                    break;
            }
            return true;
        }

        #endregion
    }


}
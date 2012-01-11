using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Synox.Helpers;
using Synox;

namespace Synox.Services.ServiceSMS.Net
{
    public class ConnexionSockets
    {

        private Socket _serverSocket;
        private List<InfoClient> _listeClients;
        private Thread _threadServeurEcoute = null;
        private Thread pingPongThread = null;
        /// <summary>
        /// Le client qui se connecte sur cette connecte accès aus fonctionnalités de l'administration du service
        /// </summary>
        private bool _clientAdministrateur = false;

        private int _port;
        /// <summary>
        /// port d'écoute
        /// </summary>
        public int Port
        {
            get { return _port; }
        }
        public event EventHandler<Events.MessageEventArgs> EventSocketsMessage = null;


        /// <summary>
        /// 
        /// </summary>
        public ConnexionSockets(bool admin)
        {
            Initialisation(admin);
        }

        /// <summary>
        /// 
        /// </summary>
        public ConnexionSockets()
        {
            Initialisation(false);
        }
        /// <summary>
        /// initialisation
        /// </summary>
        /// <param name="admin"></param>
        private void Initialisation(bool admin)
        {
            try
            {
                _clientAdministrateur = admin;
                // liste de clients (socket+thread)
                _listeClients = new List<InfoClient>();
            }
            catch
            {
                throw;
            }
        }

        public void Close()
        {
            try
            {
                if (_listeClients.Count != 0)
                {
                    foreach (InfoClient client in _listeClients)
                    {
                        if (client.SocketConnexion.Connected) client.SocketConnexion.Disconnect(true);
                        client.SocketConnexion.Close();
                    }
                    _listeClients.Clear();
                }
                if (_serverSocket != null)
                {
                    if (_serverSocket.Connected) _serverSocket.Disconnect(true);
                    _serverSocket.Close();
                    _serverSocket = null;
                }
                if (_threadServeurEcoute != null)
                {
                    _threadServeurEcoute.Abort();
                    _threadServeurEcoute = null;
                }
                if (pingPongThread != null)
                {
                    pingPongThread.Abort();
                    pingPongThread = null;
                }
            }
            catch (Exception ex) { LogHelper.Trace("ConnexionSockets.Close : "+ex.Message, LogHelper.EnumCategorie.Erreur); }
        }
        //-------------------------------------------------------------------------------

        /// <summary>
        /// Fonction qui lance un thread de surveillance du port donné en argument.
        /// </summary>
        /// <param name="myPort"></param>
        public void StartServer(int myPort)
        {
            try
            {
                _port = myPort;

                // ecoute TCP
                _threadServeurEcoute = new Thread(new ThreadStart(EtablissementConnexionSocketClient));
                _threadServeurEcoute.Start();

                if (!_clientAdministrateur)
                {
                    // suppression des sockets fantomes
                    pingPongThread = new Thread(new ThreadStart(CheckIfStillConnected));
                    pingPongThread.Start();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Connexion d'un client, ouverture de la connexion dans nouveau un thread
        /// </summary>
        private void EtablissementConnexionSocketClient()
        {
            IPEndPoint toutesInterfaces;

            try
            {
                _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _serverSocket.Blocking = true;

                toutesInterfaces = new IPEndPoint(IPAddress.Any, _port);
                _serverSocket.Bind(toutesInterfaces);
                // ecoute sans timeout
                _serverSocket.Listen(-1);

                while (true)
                {
                    Socket SocketRemote = _serverSocket.Accept();
                    if (SocketRemote.Connected)
                    {
                        Thread tc;
                        if (_clientAdministrateur)
                        {
                            //total_clients_connected++;
                            ClientTcp clientppc = new ClientTcp(SocketRemote);
                            tc = new Thread(new ThreadStart(clientppc.EcouteClient));
                        }
                        else
                        {
                            //total_clients_connected++;
                            ClientBirdyTcp clientBirdy = new ClientBirdyTcp(SocketRemote, _port);
                            tc = new Thread(new ThreadStart(clientBirdy.EcouteClient));
                        }

                        tc.Start();

                        // partie facultative qui kill les connexions mal fermées (cela ne devrait pas se produir)
                        InfoClient c = new InfoClient();
                        c.SocketConnexion = SocketRemote;
                        c.ThreadEnCours = tc;
                        c.EndPointSocket = SocketRemote.RemoteEndPoint.ToString();
                        _listeClients.Add(c);
                        CheckIfStillConnected();
                        SendMessage("Nouvelle connexion sur " + c.EndPointSocket + " | nb clients connectés : " + _listeClients.Count);
                    }
                }
            }
            catch (Exception ex)
            {
                SendMessage("Erreur constructeur Sockets : " + ex.Message);
                Console.WriteLine("Erreur constructeur Sockets : " + ex.Message);
            }
        }
        /// <summary>
        /// Envoi d'un message dans un Handler pour les abonnés à l'evenement
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(String message)
        {
            if (EventSocketsMessage != null)
                EventSocketsMessage(this, new Events.MessageEventArgs(message));
        }
        /// <summary>
        /// Etant donné que la propriété .Connected d'une socket n'est pas 	  
        /// mise à jour lors de la déconnexion d'un client sans que l'on ait
        /// prélablement essayé de lire ou d'écrire sur cette socket, cette méthode
        /// parvient à déterminer si une socket cliente s'est déconnectée grce à la méthode
        /// poll. On effectue un poll en lecture sur la socket, si le poll retourne vrai et que
        /// le nombre de bytes disponible est 0, il s'agit d'une connexion terminée
        /// </summary>
        public void CheckIfStillConnected()
        {
            try
            {
                LogHelper.Trace("lancement du controleur", LogHelper.EnumCategorie.Information);
                //while (true)
                //{
                    foreach (InfoClient client in _listeClients)
                    {
                        if (client.ThreadEnCours!=null && !client.ThreadEnCours.IsAlive)
                        {
                            if (client.SocketConnexion != null)
                            {
                                if (client.SocketConnexion.Connected) client.SocketConnexion.Disconnect(true);
                                client.SocketConnexion.Close();
                            }
                            client.ThreadEnCours.Abort();
                            SendMessage(">>> Déconnexion de " + client.EndPointSocket + " (reste " + (_listeClients.Count - 1) + " client(s) connecté(s))");
                            _listeClients.Remove(client);
                            break;
                        }
                        else
                            LogHelper.Trace(client.EndPointSocket + " est vivant", LogHelper.EnumCategorie.Erreur);
                    }
                    _listeClients.TrimExcess();
                    //Thread.Sleep(2000);

                //}
            }
            catch (Exception ex) { LogHelper.Trace("ChekStillAlive: "+ex.Message, LogHelper.EnumCategorie.Erreur); }

        }

        private void KillSocketWithSameIp(String ep)
        {
            ep = ep.Substring(0, ep.IndexOf(':'));
            for (int i = 0; i < _listeClients.Count; i++)
            {
                try
                {
                    String nom = ((InfoClient)_listeClients[i]).EndPointSocket;
                    nom = nom.Substring(0, nom.IndexOf(':'));
                    if (ep.Equals(nom))
                    {
                        Socket s = ((InfoClient)_listeClients[i]).SocketConnexion;
                        Thread t = ((InfoClient)_listeClients[i]).ThreadEnCours;
                        if (s != null)
                        {
                            s.Close();
                            s = null;
                        }
                        t.Abort();
                        t = null;
                        nom = ((InfoClient)_listeClients[i]).EndPointSocket;
                        _listeClients.Remove((InfoClient)_listeClients[i]);
                        i--;
                        SendMessage(">>> Déconnexion d'une socket morte : " + nom + " (reste " + (_listeClients.Count - 1) + " client(s) connecté(s))");
                    }
                }
                catch { }
            }
        }




        /// <summary>
        /// Renvoi simplement le nombre de clients connectés au serveur.
        /// </summary>
        public int NombreClientsConnectes
        {
            get
            {
                if (_listeClients != null)
                    return _listeClients.Count;
                else
                    return 0;
            }
        }

    }

    public struct InfoClient
    {
        public String EndPointSocket;
        public Thread ThreadEnCours;
        public Socket SocketConnexion;
    }
}
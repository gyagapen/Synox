using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Synox.Exceptions;
using Synox.Services.ServiceSMS;

namespace Synox.Services.ServiceSMS.Net
{
    public static class SocketUtil
    {
        public enum SocketEncoding{ASCII,UTF8, Defaut}
        /// <summary>
        /// Envoi un message sur la socket specifiée
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static int SendString(Socket socket, String message, SocketEncoding codage = SocketEncoding.Defaut)
        {
            // on envoi la chaine 
            Byte[] outbytes = null;
            int tailleMessage = -1;
            try
            {
              Console.WriteLine(message); LogHelper.Trace(message.Replace("\r", "\r\n"), LogHelper.EnumCategorie.Information);
                switch (codage)
                {
                    case SocketEncoding.UTF8:
                        outbytes = System.Text.Encoding.UTF8.GetBytes(message);
                        break;
                    case SocketEncoding.ASCII:
                        outbytes = System.Text.Encoding.ASCII.GetBytes(message);
                        break;
                    default:
                        outbytes = System.Text.Encoding.Default.GetBytes(message);
                        break;
                }
                tailleMessage = Send(socket, outbytes);
                return tailleMessage;
            }
            catch (Exception ex)
            {
                LogHelper.Trace(ex.Message, LogHelper.EnumCategorie.Erreur);
                throw new NetworkException("SendString :"+message,ex);
            }
        }
        /// <summary>
        /// Envoi un message sur la socket specifiée
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static int Send(Socket socket, byte[] outbytes)
        {
            int tailleMessage = -1;
            int taillePaquet = 512;
            int totalEnvoyes = 0;
            int nbOctetsEnvoyes = 0;
            try
            {
                tailleMessage = outbytes.Length;
                if (IsConnected(socket))
                {
                    while (totalEnvoyes < tailleMessage)
                    {
                        if (tailleMessage - totalEnvoyes > taillePaquet)
                            nbOctetsEnvoyes = socket.Send(outbytes, totalEnvoyes, taillePaquet, 0);
                        else
                            nbOctetsEnvoyes = socket.Send(outbytes, totalEnvoyes, tailleMessage - totalEnvoyes, 0);
                        totalEnvoyes += nbOctetsEnvoyes;
                    }
                    return tailleMessage;
                }
                else
                    throw new Exception("La socket n'est pas connecté");
            }
            catch (Exception ex)
            {
                LogHelper.Trace(ex.Message, LogHelper.EnumCategorie.Erreur);
                throw new NetworkException("Send:"+ex.Message, ex);
            }
        }

        static void sockAsync_Completed(object sender, SocketAsyncEventArgs e)
        {
            e.DisconnectReuseSocket = true;
        }


        /// <summary>
        /// Recoi un message sur la socket specifiée
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public static String RecvString(Socket socket)
        {
            Byte[] buffer = null;
            int nbOctetsRecus = 0;
            String message = String.Empty;
            try
            {
                buffer = new Byte[10000];
                if (IsConnected(socket))
                {
                    nbOctetsRecus = socket.Receive(buffer, 0, buffer.Length, 0);

                    LogHelper.Trace("nbOctetsRecus=" + nbOctetsRecus, LogHelper.EnumCategorie.Information);

                    message = System.Text.Encoding.Default.GetString(buffer, 0, nbOctetsRecus); LogHelper.Trace(message.Replace("\r","\r\n"), LogHelper.EnumCategorie.Information);
                    // LogHelper.Write("reçoit en string", TraceLevel.INFO);
                    return message;
                }
                else
                    throw new Exception("La socket n'est pas connecté");
            }
            catch (Exception ex)
            {
                LogHelper.Trace(ex.Message, LogHelper.EnumCategorie.Erreur);
                throw new NetworkException("RecvString :" + message, ex);
            }
        }


        public static Byte[] RecvBytes(Socket socket)
        {
            Byte[] inBytes = null;
            Byte[] buffer = null;
            int nbOctetsRecus = 0;
            String message = String.Empty;
            try
            {
                buffer = new Byte[10000];
                if (IsConnected(socket))
                {
                    nbOctetsRecus = socket.Receive(buffer, 0, buffer.Length, 0);
                    LogHelper.Trace("nbOctetsRecus=" + nbOctetsRecus, LogHelper.EnumCategorie.Information);
                    inBytes = new byte[nbOctetsRecus];

                    for (int i = 0; i < nbOctetsRecus; i++)
                        inBytes[i] = buffer[i];
                }
                else
                    throw new Exception("La socket n'est pas connecté");
            }
            catch (Exception ex)
            {
                LogHelper.Trace(ex.Message, LogHelper.EnumCategorie.Erreur);
                throw new NetworkException("RecvString :" + message, ex);
            }
            return inBytes;
        }

        /// <summary>
        /// Recoi un message sur la socket specifiée
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public static String RecvStringBirdy(Socket socket)
        {
            Byte[] buffer = null;
            int nbOctetsRecus = 0;
            String message = String.Empty;
            try
            {
                buffer = new Byte[10000];

                if (socket.Available>0)
                {
                    nbOctetsRecus = socket.Receive(buffer, 0, buffer.Length, 0);
                    message = System.Text.Encoding.Default.GetString(buffer, 0, nbOctetsRecus); LogHelper.Trace(message.Replace("\r", "\r\n"), LogHelper.EnumCategorie.Information);
                    // LogHelper.Write("reçoit en string", TraceLevel.INFO);
                    return message;
                }
                else
                    throw new Exception("La socket n'est pas connecté");
            }
            catch (Exception ex)
            {
                LogHelper.Trace(ex.Message, LogHelper.EnumCategorie.Erreur);
                throw new NetworkException("RecvString :" + message, ex);
            }
        }


        /// <summary>
        /// Determine if the givven socket is connected or not
        /// </summary>
        /// <remarks>
        /// There is a problem to based the Socket.Connected property since it's show the last operaion status: (from msdn)
        /// The Connected property gets the connection state of the Socket as of the last I/O operation. When it returns false, the Socket was either never connected, or is no longer connected.
        /// The value of the Connected property reflects the state of the connection as of the most recent operation. If you need to determine the current state of the connection, make a nonblocking, zero-byte Send call. If the call returns successfully or throws a WAEWOULDBLOCK error code (10035), then the socket is still connected; otherwise, the socket is no longer connected.
        /// </remarks>
        /// <seealso cref="http://groups.google.com/group/microsoft.public.dotnet.languages.csharp/browse_thread/thread/3cf03e0641731659/76e2563d28f1b256?lnk=st&q=c%23+determine+if+socket +connected&rnum=5#76e2563d28f1b256"/>
        /// <seealso cref="http://windowssdk.msdn.microsoft.com/en-us/library/system.net.sockets.socket.connected.aspx"/>
        /// <seealso cref="http://msdn.microsoft.com/en-us/library/system.net.sockets.socket.connected.aspx"/>
        /// <returns>Bool , True if connected , False if not</returns>
        public static bool IsConnected(Socket checkSocket)
        {
            bool blockingState = true;

            try
            {
                return true;

                int available = checkSocket.Available;

                if (available > 0)
                    return true;
                else
                    return false;

                blockingState = checkSocket.Blocking;
                //if (checkSocket.GetSocketOption(SocketOptionLevel.Soc ket, SocketOptionName.KeepAlive, 1)[0].Equals(1))
                // return checkSocket.Connected;

                if (checkSocket.Connected == false)
                    return false;

                //checkSocket.BeginSend(new byte[0], 0, 0, SocketFlags.None, null, null);

                bool bSelectRead = checkSocket.Poll(1, SelectMode.SelectRead);
                bool bSelectWrite = checkSocket.Poll(1, SelectMode.SelectWrite);
                //if (bSelectWrite && bSelectRead && available 0)

                if (bSelectWrite && bSelectRead)
                {
                    //return true;
                    //checkSocket.BeginReceive(new byte[1], 0, 1, SocketFlags.Peek, null, null);
                    checkSocket.Blocking = false;
                    checkSocket.Receive(new byte[0], 0, 0, SocketFlags.Peek);
                    checkSocket.Send(new byte[0], 0, 0,  SocketFlags.None);
                    return checkSocket.Connected;
                }
                else
                    return false;
            }
            catch (SocketException e)
            {    
                // 10035 == WSAEWOULDBLOCK
                if (e.NativeErrorCode.Equals(10035))
                    LogHelper.Trace("Still Connected, but the Send would block", LogHelper.EnumCategorie.Erreur);
                else
                {
                    LogHelper.Trace(string.Format("Disconnected: error code {0}!", e.NativeErrorCode), LogHelper.EnumCategorie.Erreur);
                }
                return false;
            }
            catch (ObjectDisposedException)
            {
                LogHelper.Trace(string.Format("ObjectDisposedException"), LogHelper.EnumCategorie.Erreur);
                return false;
            }
            finally
            {
                checkSocket.Blocking = blockingState;
            }



        }


    }
}

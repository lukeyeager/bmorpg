/*  This file is part of BMORPG.

    BMORPG is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    BMORPG is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with BMORPG.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using BMORPG_Server.Listeners;

namespace BMORPG_Server
{
    /// <summary>
    /// Main class for this Project
    /// </summary>
    class Server
    {
        public const string ServerVersion = "1.0";

        /// <summary>
        /// This list gets appended with Streams from the TcpListener,
        /// and Streams are removed by the Authenticator
        /// </summary>
        public static List<Stream> incomingConnections = new List<Stream>();

        /// <summary>
        /// This list gets appended with Players from the Authenticator,
        /// and Players get removed by the Matchmaker
        /// </summary>
        public static List<Player> authenticatedPlayers = new List<Player>();

        /// <summary>
        /// The MatchMaker adds Games to this list
        /// </summary>
        public static List<Game> currentGames = new List<Game>();

        public static Thread listenThread = null;
        public static ConnectionListener listener = null;

        public static Thread authenticatorThread = null;
        public static Authenticator authenticator = null;

        public static Thread matchMakerThread = null;
        public static MatchMaker matchMaker = null;
        
        // Program entry point
        static void Main(string[] args)
        {
            // Default
            int port = 11000;
            if (args.Length > 0)
            {
                try
                {
                    port = Convert.ToInt32(args[0]);
                }
                catch(Exception)
                {
                    Console.WriteLine("Failed to convert " + args[0] + " to a port#.");
                }
            }
            Console.WriteLine("Using port: " + port);

            listener = new SecureListener("certificates/BmorpgCA.cer", true);
            listenThread = new Thread(() => listener.Listen(port));
            listenThread.IsBackground = false;
            listenThread.Start();

            authenticator = new Authenticator();
            authenticatorThread = new Thread(authenticator.RunAuthenticator);
            authenticatorThread.IsBackground = false;
            authenticatorThread.Start();

            matchMaker = new MatchMaker();
            matchMakerThread = new Thread(matchMaker.RunMatchMaker);
            matchMakerThread.IsBackground = false;
            matchMakerThread.Start();

        }

        // In the future, we may use this function to update SVN, then restart the server

        /// <summary>
        /// uhgjyhgkh
        /// </summary>
        /// <param name="updateSvn"></param>
        public static void Restart( bool updateSvn = false )
        {
            Process proc = new Process();
            proc.StartInfo.FileName = "RestartServer.bat";
            if (updateSvn)
                proc.StartInfo.Arguments = "svn";

            // Get this directory's parent's parent (../..)
            string dir = new FileInfo(Directory.GetCurrentDirectory()).Directory.Parent.Parent.FullName;
            Console.WriteLine("Dir: " + dir);
            proc.StartInfo.WorkingDirectory = dir;
            proc.Start();

            Environment.Exit(0);
        }

/*

        // Source: http://msdn.microsoft.com/en-us/library/5w7b7x5f.aspx
        // Welcomes a new client and listens.
        public static void AcceptConnection(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            connectionMade.Set();

            // Get the socket that handles the client request.
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // WELCOME
            Console.WriteLine("Welcoming new client...");
            WelcomePacket sendPacket = new WelcomePacket();
            sendPacket.version = ServerVersion;
            sendPacket.socket = handler;
            //packet.Send(handler);
            Send(sendPacket);

            // Create the state object.
            NetworkPacket receivePacket = new NetworkPacket();
            receivePacket.socket = handler;

            // Receive game type from client.
            handler.BeginReceive(receivePacket.buffer, 0, receivePacket.buffer.Length, 0, new AsyncCallback(GetPlayerLogin), receivePacket);
        }

        // Source: http://msdn.microsoft.com/en-us/library/5w7b7x5f.aspx
        // Sends a string from the server to a client using the given socket.
        private static void Send(NetworkPacket packet)
        {
            try
            {
                byte[] data = packet.Serialize();
                // Begin sending the data to the remote device.
                packet.socket.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallback), packet);
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Source: http://msdn.microsoft.com/en-us/library/5w7b7x5f.aspx
        // Specifies what was sent to the client by the server.
        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.
                NetworkPacket packet = (NetworkPacket)ar.AsyncState;

                if (packet.socket.Connected)
                {
                    // Complete sending the data to the remote device.
                    int bytesSent = packet.socket.EndSend(ar);
                    //Console.WriteLine("Sent {0} bytes to client.", bytesSent);
                }
                else
                {
                    Console.WriteLine("Unable to send. Player left.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        // Format Source: http://msdn.microsoft.com/en-us/library/5w7b7x5f.aspx
        // Receive Player information from the Client
        public static void GetPlayerLogin(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket from the asynchronous state object.
            NetworkPacket packet = (NetworkPacket)ar.AsyncState;
            Socket handler = packet.socket;

            try
            {
                if (handler.Connected)
                {
                    // Read data from the client socket. 
                    int bytesRead = handler.EndReceive(ar);
                    //Console.WriteLine("\tBytes Read = {0}", bytesRead);

                    if (bytesRead > 0)
                    {
                        for (int i = 0; i < bytesRead; i++)
                        {
                            packet.TransmissionBuffer.Add(packet.buffer[i]);
                        }

                        //we need to read again if this is true
                        if (bytesRead == packet.buffer.Length)
                        {
                            packet.socket.BeginReceive(packet.buffer, 0, packet.buffer.Length, SocketFlags.None, GetPlayerLogin, packet);
                            Console.Out.WriteLine("Receiving more of the LoginPacket");
                            return;
                        }
                    }
                    NetworkPacket receivePacket = packet.Deserialize();
                    if (receivePacket != null && receivePacket is LoginPacket)
                    {
                        LoginPacket loginPacket = (LoginPacket)receivePacket;
                
                        Console.WriteLine("Attempted login with username=" + loginPacket.username + ", password=" + loginPacket.password);
                        
                        //TODO: Check login against database

                        // Example of how to start a game thread
                        if (false)
                        {
                            Game game = new Game(loginPacket.username, "player2");
                            Thread thread = new Thread(game.Start);
                            thread.Start();
                        }
                    }
                    else if (receivePacket != null && receivePacket is RestartPacket)
                    {
                        Console.WriteLine("Received RESTART command");
                        Restart(((RestartPacket)receivePacket).updateSvn);
                    }
                    else
                    {
                        Console.WriteLine("Received unknown packet");
                    }
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
         
*/

    }
}

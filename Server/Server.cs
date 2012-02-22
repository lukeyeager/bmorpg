/*  This file is part of BMORPG.

    Foobar is free software: you can redistribute it and/or modify
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Threading;
using BMORPG.NetworkPackets;

namespace BMORPG_Server
{
    // Source: http://msdn.microsoft.com/en-us/library/5w7b7x5f.aspx
    // State object for reading client data asynchronously.
    public class StateObject
    {
        // Client  socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 1024;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public StringBuilder sb = new StringBuilder();
    }

    class Server
    {
        // Thread signal.
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                try
                {
                    int port = Convert.ToInt32(args[0]);
                    Console.WriteLine("Using port: " + port);
                    Start(port);
                    return;
                }
                catch(Exception)
                {
                    Console.WriteLine("Failed to convert " + args[0] + " to a port#.");
                }
            }
            Start();
        }

        // In the future, we may use this function to update SVN, then restart the server
        public static void Restart()
        {
            Process proc = new Process();
            proc.StartInfo.FileName = "RestartServer.bat";

            // Get this directory's parent's parent (../..)
            string dir = new FileInfo(Directory.GetCurrentDirectory()).Directory.Parent.Parent.FullName;
            Console.WriteLine("Dir: " + dir);
            proc.StartInfo.WorkingDirectory = dir;
            proc.Start();
        }

        // Source: http://msdn.microsoft.com/en-us/library/5w7b7x5f.aspx
        // Listens for clients to connect.
        public static void Start(int port=0)
        {
            if (port == 0)
            {
                Console.WriteLine("Port Number:");
                port = Convert.ToInt32(Console.ReadLine());
            }

            // Establish the local endpoint for the socket.
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);

            // Create a TCP/IP socket.
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    // Set the event to nonsignaled state.
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.
                    Console.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener);

                    // Wait until a connection is made before continuing.
                    allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();
        }

        // Source: http://msdn.microsoft.com/en-us/library/5w7b7x5f.aspx
        // Welcomes a new client and listens.
        public static void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.
            allDone.Set();

            // Get the socket that handles the client request.
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // WELCOME
            Console.WriteLine("Welcoming new client...");
            Send(handler, "WELCOME\n\r");

            // Create the state object.
            StateObject state = new StateObject();
            state.workSocket = handler;

            // Receive game type from client.
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }

        // Source: http://msdn.microsoft.com/en-us/library/5w7b7x5f.aspx
        // Sends a string from the server to a client using the given socket.
        private static void Send(Socket handler, String data)
        {
            Console.WriteLine("SENT:\t{0}", data);
            // Convert the string data to byte data using ASCII encoding.
            Byte[] byteData = System.Text.Encoding.ASCII.GetBytes(data);

            try
            {
                // Begin sending the data to the remote device.
                handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
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
                Socket handler = (Socket)ar.AsyncState;

                if (handler.Connected)
                {
                    // Complete sending the data to the remote device.
                    int bytesSent = handler.EndSend(ar);
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
        // Receive game type from client.
        public static void ReadCallback(IAsyncResult ar)
        {
            String content = String.Empty;

            // Retrieve the state object and the handler socket from the asynchronous state object.
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            try
            {
                if (handler.Connected)
                {
                    // Read data from the client socket. 
                    int bytesRead = handler.EndReceive(ar);
                    //Console.WriteLine("\tBytes Read = {0}", bytesRead);

                    if (bytesRead > 0)
                    {
                        // data.ToString()
                        state.sb.Clear();
                        state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                        content = state.sb.ToString();

                        // ignore newline and the initial read
                        if (content == "\r\n" || content == "???? ????'??????")
                        {
                            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                            return;
                        }

                        Console.WriteLine("READ:\t{0}", content);

                        // Decide what to do with the input here...

                        if (true)
                        {
                            // Example of how to start a game thread
                            Game game = new Game("player1", "player2");
                            Thread thread = new Thread(game.Start);
                            thread.Start();
                        }

                    }
                    else
                    {
                        // ILLEGAL
                        Send(handler, "ILLEGAL\n\r");
                    }
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}

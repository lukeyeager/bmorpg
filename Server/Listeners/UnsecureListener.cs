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
using System.Net;
using System.Net.Sockets;

namespace BMORPG_Server.Listeners
{
    /// <summary>
    /// Accepts connections from an unsecured TCP socket
    /// </summary>
    public class UnsecureListener : ConnectionListener
    {
        // Source: http://msdn.microsoft.com/en-us/library/5w7b7x5f.aspx
        public override void Listen(int port)
        {
            // Establish the local endpoint for the socket.
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, port);

            // Create a TCP/IP socket.
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.
            try
            {
                Console.WriteLine("Waiting for unsecure connections...");

                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    try
                    {
                        // Thread blocks while waiting to accept new client
                        Socket client = listener.Accept();
                        NetworkStream stream = new NetworkStream(client);
                        AddConnection(stream);

                        Console.WriteLine("Client connected: " + client.RemoteEndPoint);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error while listening: " + ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to begin listening: " + ex.Message);
            }
        }
    }
}

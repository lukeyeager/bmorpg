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

using System.IO;
using BMORPG.NetworkPackets;
using System;

namespace BMORPG_Server.Listeners
{
    /// <summary>
    /// Listens to incoming connections on a TCP socket, then updates the list at Server.connectedPlayers
    /// </summary>
    public abstract class ConnectionListener
    {
        /// <summary>
        /// Begin listening for new connections
        /// </summary>
        /// <param name="port"></param>
        public abstract void Listen(int port);

        /// <summary>
        /// Welcome an incoming stream to the server
        /// </summary>
        /// <param name="stream"></param>
        public void AddConnection(Stream stream)
        {
            // Send a welcome to the new connection
            WelcomePacket packet = new WelcomePacket();
            packet.version = Server.ServerVersion;
            packet.stream = stream;

            if (!packet.Send(SendCallback, stream))
            {
                Console.WriteLine("ConnectionListener failed to send welcome packet.");
                stream.Close();
            }
        }

        /// <summary>
        /// Callback after sending a WelcomePacket, 
        /// Adds the stream to Server.incomingConnections if the send was successful
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="parameter"></param>
        private void SendCallback(Exception ex, object parameter)
        {
            Stream stream = (Stream)parameter;
            if (ex != null)
            {
                Console.WriteLine("ConnectionListener received exception while sending: " + ex.Message);
                stream.Close();
            }
            else
            {
                Server.incomingConnections.Push(stream);
            }
        }
    }
}
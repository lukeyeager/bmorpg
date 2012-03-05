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

        public void AddConnection(Stream stream)
        {
            // Send a welcome to the new connection
            WelcomePacket packet = new WelcomePacket();
            packet.version = Server.ServerVersion;

            byte[] buffer = packet.Serialize();
            stream.Write(buffer, 0, buffer.Length);

            // Add stream to Server's list of incoming connections
            Server.incomingConnections.push(stream);
        }
    }
}

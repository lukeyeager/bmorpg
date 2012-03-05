﻿/*  This file is part of BMORPG.

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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using BMORPG.NetworkPackets;
using System.Threading;
using System.Data.SqlClient;

namespace BMORPG_Server
{
    /// <summary>
    /// This class runs within its own thread. It watches Server.incomingConnections,
    /// authenticates the Players, then adds them to Server.authenticatedPlayers
    /// </summary>
    public class Authenticator
    {
        /// <summary>
        /// This is the Start() method for this thread, it watches Server.incomingConnections
        /// </summary>
        public void RunAuthenticator()
        {
            while (true)
            {
                Stream incoming = null;
                if (Server.incomingConnections.pop(out incoming))
                {
                    NetworkPacket packet = new NetworkPacket();
                    packet.stream = incoming;
                    incoming.BeginRead(packet.buffer, 0, packet.buffer.Length, ReceivePacket, packet);
                }

                Thread.Sleep(Server.SleepTime());
            }
        }

        public void ReceivePacket(IAsyncResult result)
        {
            NetworkPacket packet = (NetworkPacket)result.AsyncState;
            int bytesRead = packet.stream.EndRead(result);

            if (bytesRead == 0)
            {
                packet.stream.BeginRead(packet.buffer, 0, packet.buffer.Length, ReceivePacket, packet);
                return;
            }
            else
            {
                for (int i = 0; i < bytesRead; i++)
                {
                    packet.TransmissionBuffer.Add(packet.buffer[i]);
                }

                //we need to read again if this is false
                if (packet.TransmissionBuffer.Count != packet.buffer.Length)
                {
                    packet.stream.BeginRead(packet.buffer, 0, packet.buffer.Length, ReceivePacket, packet);
                    return;
                }
            }

            NetworkPacket receivePacket = packet.Deserialize();
            if (receivePacket is LoginPacket)
            {
                LoginPacket loginPacket = (LoginPacket)receivePacket;
                Console.WriteLine("Attempted login with username=" + loginPacket.username + ", password=" + loginPacket.password);
                // TODO: database

                SqlConnection connect = new SqlConnection("UID=username;PWD=password;Addr=(local);Trusted_Connection=sspi;" +
                    "Database=database;Connection Timeout=5;ApplicationIntent=ReadOnly");

                try
                {
                    connect.Open();
                    SqlDataReader reader = null;
                    SqlCommand command = new SqlCommand("", connect);
                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        //read in from schema
                    }
                    connect.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
            else
            {
                String msg = "Authenticator received unexpected packet type: " + receivePacket.PacketType;
                Console.WriteLine(msg);

                ErrorPacket errorPacket = new ErrorPacket();
                errorPacket.message = msg;
                byte[] buffer = errorPacket.Serialize();
                receivePacket.stream.Write(buffer, 0, buffer.Length);

                NetworkPacket newReceivePacket = new NetworkPacket();
                receivePacket.stream.BeginRead(newReceivePacket.buffer, 0, newReceivePacket.buffer.Length, ReceivePacket, newReceivePacket);
            }
        }
    }
}

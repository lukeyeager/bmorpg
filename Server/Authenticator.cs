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
                if (Server.incomingConnections.Pop(out incoming))
                {
                    Console.WriteLine("Authenticator popping connection from Server.incomingConnections.");
                    NetworkPacket packet = new NetworkPacket();
                    packet.stream = incoming;
                    try
                    {
                        incoming.BeginRead(packet.buffer, 0, packet.buffer.Length, ReceivePacket, packet);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Authenticator: " + ex.Message);
                        try
                        {
                            packet.stream.Close();
                        }
                        finally
                        { }
                    }
                }

                Thread.Sleep(Server.SleepTime());
            }
        }

        /// <summary>
        /// Asynchronous callback when a packet is received
        /// </summary>
        /// <param name="result"></param>
        public void ReceivePacket(IAsyncResult result)
        {
            NetworkPacket packet = (NetworkPacket)result.AsyncState;

            // This gets set and sent at the end of the function if an error occurs
            String errorMessage = null;
            Player player = null;

            try
            {
                int bytesRead = packet.stream.EndRead(result);

                if (bytesRead == 0)
                {
                    Console.WriteLine("Authenticator received 0 bytes");
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
                        Console.WriteLine("Authenticator has received " + packet.TransmissionBuffer.Count + " bytes.");
                        packet.stream.BeginRead(packet.buffer, 0, packet.buffer.Length, ReceivePacket, packet);
                        return;
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Authenticator lost a connection.");
                packet.stream.Close();
                return;
            }


            NetworkPacket receivePacket = packet.Deserialize();
            if (receivePacket is LoginRequestPacket)
            {
                LoginRequestPacket loginPacket = (LoginRequestPacket)receivePacket;
                Console.WriteLine("Attempting login with username=" + loginPacket.username + ", password=" + loginPacket.password);

                if (Server.dbConnection == null)
                {
                    // For now, we'll just assume they're authenticated
                    player = new Player();
                    player.netStream = packet.stream;
                    player.username = loginPacket.username;
                    player.current_health = 10;
                }
                else
                {
                    SqlDataReader reader = null;
                    SqlCommand command = new SqlCommand("SELECT Password\nFROM Authenticate\nWHERE Username = " +
                        loginPacket.username, Server.dbConnection);
                    command.CommandTimeout = 15;
                    try
                    {
                        reader = command.ExecuteReader();
                        reader.Read();
                        Console.WriteLine("Password in database for " + loginPacket.username + ": " + reader[0]);
                        if (loginPacket.password == (string)reader[0])
                        {
                            Console.WriteLine("Login succeeded for: " + loginPacket.username);
                            player = new Player();
                            player.username = loginPacket.username;

                            // TODO: Read rest of attributes

                        }
                        else
                        {
                            errorMessage = "Unrecognized username/password combination.";
                        }
                        reader.Close();
                    }
                    catch (Exception e)
                    {
                        errorMessage = e.ToString();
                    }
                }
            }
            else if (receivePacket is CreateAccountPacket)
            {
                CreateAccountPacket createAccountPacket = (CreateAccountPacket)receivePacket;

                Console.WriteLine("Trying to create account with username=" + createAccountPacket.username + ", password=" + createAccountPacket.password);

                if (Server.dbConnection == null)
                {
                    // For now, assume player is authenticated
                    player = new Player();
                    player.netStream = packet.stream;
                    player.username = createAccountPacket.username;
                    player.current_health = 10;
                }
                else
                {
                    //TODO: Database

                    errorMessage = "Not implemented.";
                }
            }
            else if (receivePacket is RestartPacket)
            {
                Server.Restart(((RestartPacket)receivePacket).updateSvn);
                return;
            }
            else
            {
                errorMessage = "Authenticator received unexpected packet type: " + receivePacket.PacketType;
            }


            if (player != null) //The login succeeded
            {
                LoginStatusPacket statusPacket = new LoginStatusPacket();
                statusPacket.success = true;
                byte[] buffer = statusPacket.Serialize();

                try
                {
                    packet.stream.Write(buffer, 0, buffer.Length);
                    Server.authenticatedPlayers.Push(player);
                    Console.WriteLine("Login success: " + player.username);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    packet.stream.Close();
                }
            }
            else if (errorMessage != null) //The login failed
            {
                Console.WriteLine(errorMessage);

                LoginStatusPacket statusPacket = new LoginStatusPacket();
                statusPacket.success = false;
                statusPacket.errorMessage = errorMessage;
                byte[] buffer = statusPacket.Serialize();

                try
                {
                    packet.stream.Write(buffer, 0, buffer.Length);

                    NetworkPacket newReceivePacket = new NetworkPacket();
                    packet.stream.BeginRead(newReceivePacket.buffer, 0, newReceivePacket.buffer.Length, ReceivePacket, newReceivePacket);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    packet.stream.Close();
                }

            }
        }
    }
}

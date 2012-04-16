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
        private static SqlConnection authenticatorDBConnection = null;
        private static object authenticatorDBConnectionLock = new Object();

        /// <summary>
        /// This is the Start() method for this thread, it watches Server.incomingConnections
        /// </summary>
        public void RunAuthenticator()
        {
            // Make database connection
            try
            {
                authenticatorDBConnection = new SqlConnection(Server.dbConnectionString);
                authenticatorDBConnection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to open authenticator database connection");
                authenticatorDBConnection = null;
            }

            while (true)
            {
                Stream incoming = Server.incomingConnections.Pop();
                if (incoming != null)
                {
                    Console.WriteLine("Authenticator popping connection from Server.incomingConnections.");
                    NetworkPacket packet = new NetworkPacket();
                    packet.stream = incoming;

                    packet.Receive(ReceivePacketCallback);
                }

                Thread.Sleep(Server.SleepTime());
            }
        }

        /// <summary>
        /// Callback after receiving a LoginRequestPacket or CreateAccountPacket
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="packet"></param>
        /// <param name="obj"></param>
        public void ReceivePacketCallback(Exception exception, NetworkPacket packet, object obj)
        {
            if (exception != null)
            {
                Console.WriteLine("Authenticator receieved an error while receiving: " + exception.Message);
                packet.stream.Close();
                return;
            }

            Player player = null;
            String errorMessage = null;

            if (packet is LoginRequestPacket)
            {
                LoginRequestPacket loginPacket = (LoginRequestPacket)packet;
                Console.WriteLine("Attempting login with username=" + loginPacket.username + ", password=" + loginPacket.password);

                if (authenticatorDBConnection == null)
                {
                    // For now, we'll just assume they're authenticated
                    player = new Player(packet.stream, loginPacket.username, 1);
                }
                else
                {
                    SqlDataReader reader = null;
                    SqlCommand command = new SqlCommand("SELECT PID, Password\nFROM Player\nWHERE Username = '" +
                        loginPacket.username + "'" , authenticatorDBConnection);
                    command.CommandTimeout = 3;
                    try
                    {
                        string dbPwd = "";
                        int UID = 0;
                        lock (authenticatorDBConnectionLock)
                        {
                            reader = command.ExecuteReader();
                            if (reader.Read())
                            {
                                dbPwd = reader.GetString(1);
                                UID = reader.GetInt32(0);
                                reader.Close();
                            }
                            else
                            {
                                errorMessage = "Username does not exist.";
                            }
                        }
                        Console.WriteLine("Password in database for " + loginPacket.username + ": " + dbPwd);
                        if (loginPacket.password == dbPwd)
                        {
                            Console.WriteLine("Login succeeded for: " + loginPacket.username);
                            player = new Player(packet.stream, loginPacket.username, UID);
                            Server.authenticatedPlayers.Push(player);
                        }
                        else
                        {
                            errorMessage = "Unrecognized username/password combination.";
                        }
                    }
                    catch (Exception ex)
                    {
                        errorMessage = ex.ToString();
                        if(reader != null)
                            reader.Close();
                    }
                }
            }
            else if (packet is CreateAccountPacket)
            {
                CreateAccountPacket createAccountPacket = (CreateAccountPacket)packet;

                Console.WriteLine("Trying to create account with username=" + createAccountPacket.username + ", password=" + createAccountPacket.password);

                if (Server.dbConnection == null)
                {
                    // For now, assume player is authenticated
                    player = new Player(packet.stream, createAccountPacket.username, 1);
                }
                else
                {
                    //TODO: Database

                    errorMessage = "Not implemented.";
                }
            }
            else if (packet is RestartPacket)
            {
                // Restart here because it's the first chance we get from the client's perspective
				// Can add this check to any place where we receive any packet
                Server.Restart(((RestartPacket)packet).updateSvn);
                return;
            }
            else
            {
                errorMessage = "Authenticator received unexpected packet type: " + packet.PacketType;
            }


            if (player != null) //The login succeeded
            {
                LoginStatusPacket statusPacket = new LoginStatusPacket();
                statusPacket.success = true;
                statusPacket.stream = packet.stream;

                if (!statusPacket.Send(SendSuccessCallback, player))
                {
                    Console.WriteLine("Authenticator could not send LoginStatusPacket.");
                    packet.stream.Close();
                }
            }
            else if (errorMessage != null) //The login failed
            {
                Console.WriteLine(errorMessage);
                player = null;

                LoginStatusPacket statusPacket = new LoginStatusPacket();
                statusPacket.success = false;
                statusPacket.errorMessage = errorMessage;
                statusPacket.stream = packet.stream;
                if (!statusPacket.Send(SendFailureCallback, packet.stream))
                {
                    Console.WriteLine("Authenticator could not send LoginStatusPacket.");
                    packet.stream.Close();
                }
            }
        }

        /// <summary>
        /// Callback after sending a successful LoginStatusPacket
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="parameter"></param>
        private void SendSuccessCallback(Exception exception, object parameter)
        {
            Player player = (Player)parameter;
            if (exception != null)
            {
                Console.WriteLine("Authenticator received exception while sending: " + exception.Message);
                player.netStream.Close();
            }
            else
            {
                Console.WriteLine("Login success: " + player.Username);
                Server.authenticatedPlayers.Push(player);
            }
        }

        /// <summary>
        /// Callback after sending a failed LoginStatusPacket
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="parameter"></param>
        private void SendFailureCallback(Exception exception, object parameter)
        {
            Stream stream = (Stream)parameter;

            if (exception != null)
            {
                Console.WriteLine("Authenticator received exception while sending: " + exception.Message);
                stream.Close();
            }
            else
            {
                NetworkPacket packet = new NetworkPacket();
                packet.Receive(ReceivePacketCallback);
            }
        }

    }
}

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
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using BMORPG.NetworkPackets;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Threading;

namespace BMORPG_Client
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private Stream stream;

        // Source: http://msdn.microsoft.com/en-us/library/system.net.security.sslstream.aspx
        // The following method is invoked by the RemoteCertificateValidationDelegate.
        public static bool ValidateServerCertificate(
              object sender,
              X509Certificate certificate,
              X509Chain chain,
              SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            MessageBox.Show("Certificate error: " + sslPolicyErrors);

            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }

        private void Connect_Click(object sender, EventArgs e)
        {
            int port = 11000;
            string certName = "BmorpgCA";

            IPAddress ipAddress;
            if (IpAddress.Text == "")
                ipAddress = IPAddress.Parse("127.0.0.1");
            else
                ipAddress = IPAddress.Parse(IPAddressBox.Text);

            IPEndPoint ipEndpoint = new IPEndPoint(ipAddress, port);

            if (SecureBox.Checked)
            {
                TcpClient client = null;
                try
                {
                    // Create a TCP/IP client socket.
                    client = new TcpClient();
                    client.Connect(ipEndpoint);
                    // Create an SSL stream that will close the client's stream.
                    SslStream sslStream = new SslStream(
                        client.GetStream(),
                        false,
                        new RemoteCertificateValidationCallback(ValidateServerCertificate),
                        null
                        );
                    // The server name must match the name on the server certificate.
                    sslStream.AuthenticateAsClient(certName);

                    stream = sslStream;
                }
                catch (Exception ex)
                {
                    ConnectionStatusBox.Text = "Exception: " + ex.Message;
                    if (client != null)
                        client.Close();
                    return;
                }
            }
            else
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                try
                {
                    socket.Connect(ipEndpoint);
                    stream = new NetworkStream(socket);
                    stream.WriteTimeout = 30;
                    stream.ReadTimeout = 30;
                }
                catch (Exception ex)
                {
                    ConnectionStatusBox.Text = ex.Message;
                    socket.Close();
                    return;
                }
            }

            ConnectionStatusBox.Text = "Connected.";

            NetworkPacket packet = new NetworkPacket();
            packet.stream = stream;
            stream.BeginRead(packet.buffer, 0, packet.buffer.Length, ReceiveWelcome, packet);
        }

        private void ReceiveWelcome(IAsyncResult result)
        {
            try
            {
                NetworkPacket packet = (NetworkPacket)result.AsyncState;
                int read = packet.stream.EndRead(result);
                if (read > 0)
                {
                    for (int i = 0; i < read; i++)
                    {
                        packet.TransmissionBuffer.Add(packet.buffer[i]);
                    }

                    //we need to read again if this is true
                    if (read == packet.buffer.Length)
                    {
                        packet.stream.BeginRead(packet.buffer, 0, packet.buffer.Length, ReceiveWelcome, packet);
                        return;
                    }
                }
                NetworkPacket receivePacket = packet.Deserialize();
                if (receivePacket != null && receivePacket is WelcomePacket)
                {
                    WelcomePacket welcomePacket = (WelcomePacket)receivePacket;

                    MessageBox.Show("Server version: " + welcomePacket.version);
                }
                else
                {
                    MessageBox.Show("Received unknown packet.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception: " + ex.Message);
            }
        }

        private void Login_Click(object sender, EventArgs e)
        {
            if (stream == null || !stream.CanRead || !stream.CanWrite)
                LoginStatusBox.Text = "Socket is not connected";
            else
            {
                LoginRequestPacket packet = new LoginRequestPacket();
                packet.username = UsernameBox.Text;
                packet.password = PasswordBox.Text;

                try
                {
                    byte[] buffer = packet.Serialize();
                    stream.Write(buffer, 0, buffer.Length);
                    LoginStatusBox.Text = "Sent login information.";

                    LoginStatusPacket statusPacket = new LoginStatusPacket();
                    statusPacket.stream = stream;
                    stream.BeginRead(statusPacket.buffer, 0, statusPacket.buffer.Length, ReceiveLoginStatus, statusPacket);
                    LoginStatusBox.Text = "Waiting for login status";
                }
                catch (Exception ex)
                {
                    LoginStatusBox.Text = ex.Message;
                }
            }
        }

        private void ReceiveLoginStatus(IAsyncResult result)
        {
            LoginStatusPacket packet = (LoginStatusPacket)result.AsyncState;

            try
            {
                int bytesRead = packet.stream.EndRead(result);

                if (bytesRead == 0)
                {
                    MessageBox.Show("ReceiveLoginStatus read 0 bytes");
                    stream.BeginRead(packet.buffer, 0, packet.buffer.Length, ReceiveLoginStatus, packet);
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
                        MessageBox.Show("ReceiveLoginStatus read " + packet.TransmissionBuffer.Count + " bytes");
                        stream.BeginRead(packet.buffer, 0, packet.buffer.Length, ReceiveLoginStatus, packet);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            String text;
            if (packet.success)
                text = "Login successful";
            else
                text = packet.errorMessage;
            MessageBox.Show(text);

            if (packet.success)
            {
                StartGamePacket gamePacket = new StartGamePacket();
                stream.BeginRead(gamePacket.buffer, 0, gamePacket.buffer.Length, ReceiveGameStart, gamePacket);
            }
        }

        private void ReceiveGameStart(IAsyncResult result)
        {
            StartGamePacket packet = (StartGamePacket)result.AsyncState;

            int bytesRead = packet.stream.EndRead(result);

            if (bytesRead == 0)
            {
                stream.BeginRead(packet.buffer, 0, packet.buffer.Length, ReceiveGameStart, packet);
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
                    stream.BeginRead(packet.buffer, 0, packet.buffer.Length, ReceiveGameStart, packet);
                    return;
                }
            }

            MessageBox.Show("Playing " + packet.opponentUsername);
            stream.Close();
        }

        private void Restart_Click(object sender, EventArgs e)
        {
            if (stream == null || !stream.CanRead || !stream.CanWrite)
                ConnectionStatusBox.Text = "Socket is not connected";
            else
            {
                RestartPacket packet = new RestartPacket();
                packet.updateSvn = SvnCheckBox.Checked;

                try
                {
                    byte[] buffer = packet.Serialize();
                    stream.Write(buffer, 0, buffer.Length);
                    ConnectionStatusBox.Text = "Sent restart packet";
                }
                catch (Exception ex)
                {
                    ConnectionStatusBox.Text = ex.Message;
                }
            }
        }
    }
}

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
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using BMORPG.NetworkPackets;

namespace BMORPG_Client
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        Socket socket;

        private void Connect_Click(object sender, EventArgs e)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEndpoint = new IPEndPoint(ipAddress, Int32.Parse(textBox1.Text));

            try
            {
                socket.BeginConnect(ipEndpoint, ConnectCallback, socket);
            }
            catch (SocketException ex)
            {
                textBox4.Text = ex.Message;
            }
        }

        public void ConnectCallback(IAsyncResult result)
        {
            Socket socket = (Socket)result.AsyncState;
            socket.EndConnect(result);

            NetworkPacket packet = new NetworkPacket();
            packet.socket = socket;
            socket.BeginReceive(packet.buffer, 0, packet.buffer.Length, SocketFlags.None, ReceiveWelcome, packet);
        }

        public void ReceiveWelcome(IAsyncResult result)
        {
            NetworkPacket packet = (NetworkPacket)result.AsyncState;
            int read = packet.socket.EndReceive(result);
            if (read > 0)
            {
                for (int i = 0; i < read; i++)
                {
                    packet.TransmissionBuffer.Add(packet.buffer[i]);
                }

                //we need to read again if this is true
                if (read == packet.buffer.Length)
                {
                    packet.socket.BeginReceive(packet.buffer, 0, packet.buffer.Length, SocketFlags.None, ReceiveWelcome, packet);
                    Console.Out.WriteLine("Receiving more of the WelcomePacket");
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


        private void Login_Click(object sender, EventArgs e)
        {
            LoginPacket packet = new LoginPacket();
            packet.username = textBox2.Text;
            packet.password = textBox3.Text;
            packet.socket = socket;

            byte[] buffer = packet.Serialize();
            packet.socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendLoginCallback, packet);
        }

        public void SendLoginCallback(IAsyncResult result)
        {
            LoginPacket packet = (LoginPacket)result.AsyncState;

            packet.socket.EndSend(result);
        }

        private void Restart_Click(object sender, EventArgs e)
        {
            RestartPacket packet = new RestartPacket();
            packet.updateSvn = true;
            packet.socket = socket;

            byte[] buffer = packet.Serialize();
            packet.socket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendRestartCallback, packet);
        }

        public void SendRestartCallback(IAsyncResult result)
        {
            RestartPacket packet = (RestartPacket)result.AsyncState;

            packet.socket.EndSend(result);
        }
    }
}

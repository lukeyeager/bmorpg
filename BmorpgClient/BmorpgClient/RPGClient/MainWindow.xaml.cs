using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using BMORPG.NetworkPackets;

namespace BMORPGClient
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
        private Stream stream;

		public MainWindow()
		{
			this.InitializeComponent();
		}

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            tabControl1.Visibility = System.Windows.Visibility.Hidden;
        }

        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            stream.Close();
            stream = null;
            Environment.Exit(1);
        }

        private void buttonNewAcct_Click(object sender, RoutedEventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }



        #region Login Tab

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

        private void ConnectButton_Click(object sender, EventArgs e)
        {
            if (stream != null)
            {
                ConnectionStatusBox.Text = "Already connected.";
                return;
            }

            int port = 11000;
            string certName = "BmorpgCA";

            IPAddress ipAddress;
            if (IPAddressBox.Text == "")
                ipAddress = IPAddress.Parse("127.0.0.1");
            else
                ipAddress = IPAddress.Parse(IPAddressBox.Text);

            IPEndPoint ipEndpoint = new IPEndPoint(ipAddress, port);

            if (SecureBox.IsChecked==true)
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
            packet.Receive(ReceiveWelcome);
        }

        private void ReceiveWelcome(Exception exception, NetworkPacket packet, object parameter)
        {
            if (exception != null)
            {
                MessageBox.Show("Received exception while waiting for welcome: " + exception.Message);
                packet.stream.Close();
                stream = null;
                return;
            }

            if (packet != null && packet is WelcomePacket)
            {
                WelcomePacket welcomePacket = (WelcomePacket)packet;
                // TODO: Less intrusive message
                //MessageBox.Show("Server version: " + welcomePacket.version);
            }
            else
            {
                MessageBox.Show("Received unknown packet.");
            }
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (stream == null)
                LoginStatusBox.Text = "Socket is not connected";
            else
            {
                LoginRequestPacket packet = new LoginRequestPacket();
                packet.username = UsernameBox.Text;
                packet.password = PasswordBox.Password;
                packet.stream = stream;

                if (packet.Send(SendLoginCallback))
                    LoginStatusBox.Text = "Sent login information.";
                else
                {
                    LoginStatusBox.Text = "Couldn't send login information.";
                    stream = null;
                }
            }
        }

        private void SendLoginCallback(Exception exception, object parameter)
        {
            if (exception != null)
            {
                MessageBox.Show("Received exception wile sending login packet: " + exception.Message);
                stream.Close();
                stream = null;
            }
            else
            {
                NetworkPacket packet = new NetworkPacket();
                packet.stream = stream;
                packet.Receive(ReceiveLoginStatus);
            }
        }

        private void ReceiveLoginStatus(Exception exception, NetworkPacket packet, object parameter)
        {
            if (exception != null)
            {
                MessageBox.Show("Received error while waiting for login status: " + exception.Message);
                stream.Close();
                stream = null;
            }
            else if (!(packet is LoginStatusPacket))
            {
                MessageBox.Show("Received unknown packet when waiting for LoginStatus packet: " + packet.PacketType);
            }
            else
            {
                LoginStatusPacket statusPacket = (LoginStatusPacket)packet;

                if (statusPacket.success)
                {
                    tabControl1.SelectedIndex = 3;
                }
                else
                    MessageBox.Show("ERROR from login: " + statusPacket.errorMessage);
            }
        }

        private void Restart_Click(object sender, EventArgs e)
        {
            if (stream == null || !stream.CanRead || !stream.CanWrite)
                ConnectionStatusBox.Text = "Socket is not connected";
            else
            {
                RestartPacket packet = new RestartPacket();
                if (SvnCheckBox.IsChecked == true)
                    packet.updateSvn = true;
                else
                    packet.updateSvn = false;
                packet.stream = stream;
                packet.Send(SendRestartCallback);
            }
        }

        private void SendRestartCallback(Exception ex, object parameter)
        {
            if (ex != null)
                MessageBox.Show("Received exception while sending restart packet: " + ex.Message);
            else
                MessageBox.Show("Sent restart packet");

            stream.Close();
            stream = null;
        }
        #endregion

        #region New Account Tab

        private void buttonCreateAccount_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxClassPick.SelectedIndex == -1) 
            {
                MessageBox.Show("Don't hate Mass Effect 3. Choose a class.");
                return;
            }
            CreateAccountPacket newAcct = new CreateAccountPacket();
            newAcct.username = textBoxPickUsername.Text;
            newAcct.password = textBoxPickPassword.Password;
            switch (comboBoxClassPick.SelectedIndex) 
            {
                case 0:
                    newAcct.userClass = "Soldier";
                    break;
                case 1:
                    newAcct.userClass = "Adept";
                    break;
                case 2:
                    newAcct.userClass = "Engineer";
                    break;
            }
            newAcct.stream = stream;

            newAcct.Send(SendCreateAccountPacketCallback);
        }

        void SendCreateAccountPacketCallback(Exception ex, object parameter)
        {
            if (ex != null)
            {
                MessageBox.Show("Received error while sending CreateAccountPacket: " + ex.Message);
                stream.Close();
                stream = null;
                return;
            }

            MessageBox.Show("Sent CreateAccountPacket!");

            NetworkPacket packet = new NetworkPacket();
            packet.stream = stream;
            packet.Receive(ReceiveLoginStatus);
        }

        #endregion

        #region Game Tab

        private void ReceiveGameStart(Exception exception, NetworkPacket packet, object parameter)
        {
            if (exception != null)
            {
                MessageBox.Show("Received exception while waiting for game start: " + exception.Message);
                stream.Close();
                stream = null;
            }
            else
            {
                if (packet is StartGamePacket)
                {
                    StartGamePacket startGamePacket = (StartGamePacket)packet;
                    MessageBox.Show("Playing " + startGamePacket.opponentUsername);
                    if (!startGamePacket.myTurn)
                    {
                        NetworkPacket state = new NetworkPacket();
                        state.stream = stream;
                        state.Receive(ReceiveGameState);
                    }
                }
                else
                {
                    MessageBox.Show("Received unknown packet type");
                }
            }
        }

        private void ReceiveGameState(Exception exception, NetworkPacket packet, object parameter) 
        {
            StatePacket state = (StatePacket)packet;
            labelP1Health.Content = state.p1Health.ToString();
            labelP2Health.Content = state.p2Health.ToString();
            labelLastKnownStatus.Content = state.status;
        }

        private void buttonGoBack_Click(object sender, RoutedEventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        public void makeMove(int moveIndex) 
        {
            PlayerMovePacket movePacket = new PlayerMovePacket();
            switch (moveIndex) 
            {
                case 1:
                    movePacket.moveType = PlayerMovePacket.MoveType.Ability;
                    break;
                case 2:
                    movePacket.moveType = PlayerMovePacket.MoveType.Equipment;
                    break;
                case 3:
                    movePacket.moveType = PlayerMovePacket.MoveType.Item;
                    break;
                default:
                    movePacket.moveType = PlayerMovePacket.MoveType.None;
                    break;
            };
            movePacket.stream = stream;
            movePacket.Send(SendMovePacketCallback);            
        }

        void SendMovePacketCallback(Exception ex, object parameter)
        {
            if (ex != null)
            {
                MessageBox.Show("Received error while sending PlayerMovePacket: " + ex.Message);
                stream.Close();
                stream = null;
                return;
            }

            StatePacket packet = new StatePacket();
            packet.stream = stream;
            packet.Receive(ReceiveGameState);
        }

        private void buttonAttack1(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You have attacked.");
            makeMove(1);
        }

        private void buttonAttack2(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You have attacked.");
            makeMove(2);
        }

        private void buttonDefend(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("You have defended.");
            makeMove(3);
        }

        private void buttonSpecial(object sender, RoutedEventArgs e)
        {
            //Check mana eventually
            MessageBox.Show("You have used your special move.");
            makeMove(4);
        }

        #endregion

        private void buttonStartGame_Click(object sender, RoutedEventArgs e)
        {
            NetworkPacket receivePacket = new NetworkPacket();
            receivePacket.stream = stream;

            if (!receivePacket.Receive(ReceiveGameStart))
            {
                MessageBox.Show("Could not start receiving gamestart packet.");
                stream.Close();
                stream = null;
            }
            else 
            {
                tabControl1.SelectedIndex = 2;
            }
        }

        private void Attack1Button_Click(object sender, RoutedEventArgs e)
        {
            PlayerMovePacket packet = new PlayerMovePacket();
            packet.moveType = PlayerMovePacket.MoveType.Ability;
            packet.stream = stream;

            packet.Send(SendMoveCallback);
        }

        private void Attack2Button_Click(object sender, RoutedEventArgs e)
        {
            PlayerMovePacket packet = new PlayerMovePacket();
            packet.moveType = PlayerMovePacket.MoveType.Equipment;
            packet.stream = stream;

            packet.Send(SendMoveCallback);
        }

        private void DefendButton_Click(object sender, RoutedEventArgs e)
        {
            PlayerMovePacket packet = new PlayerMovePacket();
            packet.moveType = PlayerMovePacket.MoveType.Item;
            packet.stream = stream;

            packet.Send(SendMoveCallback);
        }

        private void SpecialButton_Click(object sender, RoutedEventArgs e)
        {
            PlayerMovePacket packet = new PlayerMovePacket();
            packet.moveType = PlayerMovePacket.MoveType.None;
            packet.stream = stream;

            packet.Send(SendMoveCallback);
        }

        private void SendMoveCallback(Exception exception, object parameter)
        {
            // Empty
        }
    }
}
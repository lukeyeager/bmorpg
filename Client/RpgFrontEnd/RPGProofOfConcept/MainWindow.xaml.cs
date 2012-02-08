using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RPGProofOfConcept
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

        private IPAddress ipAddress;
        private IPEndPoint ipEndpoint;
        private TcpClient client;

		public MainWindow()
		{
			this.InitializeComponent();
            client = new TcpClient();
            connectToServer();
		}

        public void connectToServer()
        {
            ipAddress = IPAddress.Parse("127.0.0.1");
            ipEndpoint = new IPEndPoint(ipAddress, Int32.Parse("11000"));
            client.Connect(ipEndpoint);
            NetworkStream stream = client.GetStream();
            Byte[] data = new Byte[1024];
            String responseData = String.Empty;

            try
            {
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                MessageBox.Show(responseData);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
	}
}
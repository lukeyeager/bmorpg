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
using System.Net.Sockets;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Security.Authentication;

namespace BMORPG_Server.Listeners
{
    /// <summary>
    /// Accepts connections from an SSL socket
    /// </summary>
    public class SecureListener : ConnectionListener
    {
        Boolean verbose;
        X509Certificate serverCertificate = null;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="certificate">Filename of the SSL certificate</param>
        /// <param name="_verbose">Display detailed connection information</param>
        public SecureListener(string certificate, bool _verbose = false)
        {
            serverCertificate = X509Certificate.CreateFromCertFile(certificate);
            verbose = _verbose;
        }

        //Source: http://msdn.microsoft.com/en-us/library/system.net.security.sslstream.aspx
        public override void Listen(int port)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            listener.Start();
            while (true)
            {
                Console.WriteLine("Waiting for secure connections...");
                // Application blocks while waiting for an incoming connection.
                // Type CNTL-C to terminate the server.
                TcpClient client = listener.AcceptTcpClient();
                ProcessClient(client);
            }
        }

        //Source: http://msdn.microsoft.com/en-us/library/system.net.security.sslstream.aspx
        void ProcessClient(TcpClient client)
        {
            Console.WriteLine("Processing client: " + client.Client.RemoteEndPoint);

            // A client has connected. Create the 
            // SslStream using the client's network stream.
            SslStream sslStream = new SslStream(
                client.GetStream(), false);
            // Authenticate the server but don't require the client to authenticate.
            try
            {
                sslStream.AuthenticateAsServer(serverCertificate,
                    false, SslProtocols.Tls, true);
                if (verbose)
                {
                    // Display the properties and settings for the authenticated stream.
                    DisplaySecurityLevel(sslStream);
                    DisplaySecurityServices(sslStream);
                    DisplayCertificateInformation(sslStream);
                    DisplayStreamProperties(sslStream);
                }

                // Set timeouts for the read and write to 5 seconds.
                sslStream.ReadTimeout = 5000;
                sslStream.WriteTimeout = 5000;

                AddConnection(sslStream);
                
                Console.WriteLine("Client connected: " + client.Client.RemoteEndPoint);

            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: {0}", ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner exception: {0}", ex.InnerException.Message);
                }
                Console.WriteLine("Authentication failed - closing the connection.");
                sslStream.Close();
                client.Close();
                return;
            }
            finally
            {
                // The client stream will be closed with the sslStream
                // because we specified this behavior when creating
                // the sslStream.
                sslStream.Close();
                client.Close();
            }
        }

        // Source: http://msdn.microsoft.com/en-us/library/system.net.security.sslstream.aspx
        static void DisplaySecurityLevel(SslStream stream)
        {
            Console.WriteLine("Cipher: {0} strength {1}", stream.CipherAlgorithm, stream.CipherStrength);
            Console.WriteLine("Hash: {0} strength {1}", stream.HashAlgorithm, stream.HashStrength);
            Console.WriteLine("Key exchange: {0} strength {1}", stream.KeyExchangeAlgorithm, stream.KeyExchangeStrength);
            Console.WriteLine("Protocol: {0}", stream.SslProtocol);
        }
        // Source: http://msdn.microsoft.com/en-us/library/system.net.security.sslstream.aspx
        static void DisplaySecurityServices(SslStream stream)
        {
            Console.WriteLine("Is authenticated: {0} as server? {1}", stream.IsAuthenticated, stream.IsServer);
            Console.WriteLine("IsSigned: {0}", stream.IsSigned);
            Console.WriteLine("Is Encrypted: {0}", stream.IsEncrypted);
        }
        // Source: http://msdn.microsoft.com/en-us/library/system.net.security.sslstream.aspx
        static void DisplayStreamProperties(SslStream stream)
        {
            Console.WriteLine("Can read: {0}, write {1}", stream.CanRead, stream.CanWrite);
            Console.WriteLine("Can timeout: {0}", stream.CanTimeout);
        }
        // Source: http://msdn.microsoft.com/en-us/library/system.net.security.sslstream.aspx
        static void DisplayCertificateInformation(SslStream stream)
        {
            Console.WriteLine("Certificate revocation list checked: {0}", stream.CheckCertRevocationStatus);

            X509Certificate localCertificate = stream.LocalCertificate;
            if (stream.LocalCertificate != null)
            {
                Console.WriteLine("Local cert was issued to {0} and is valid from {1} until {2}.",
                    localCertificate.Subject,
                    localCertificate.GetEffectiveDateString(),
                    localCertificate.GetExpirationDateString());
            }
            else
            {
                Console.WriteLine("Local certificate is null.");
            }
            // Display the properties of the client's certificate.
            X509Certificate remoteCertificate = stream.RemoteCertificate;
            if (stream.RemoteCertificate != null)
            {
                Console.WriteLine("Remote cert was issued to {0} and is valid from {1} until {2}.",
                    remoteCertificate.Subject,
                    remoteCertificate.GetEffectiveDateString(),
                    remoteCertificate.GetExpirationDateString());
            }
            else
            {
                Console.WriteLine("Remote certificate is null.");
            }
        }
    }
}

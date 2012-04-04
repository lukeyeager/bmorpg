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
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Net.Sockets;
using System.Reflection;
using System.Net.Security;

namespace BMORPG.NetworkPackets
{
    public delegate void PacketSendCallback(Exception ex, object parameter);
    public delegate void PacketReceiveCallback(Exception ex, NetworkPacket packet, object parameter);

    /// <summary>
    /// This class lets us serialize and deserialize the same objects in different applications
    /// </summary>
    sealed class AllowAllAssemblyVersionsDeserializationBinder : System.Runtime.Serialization.SerializationBinder
    {
        public override Type BindToType(string assemblyName, string typeName)
        {
            Type typeToDeserialize = null;
 
            String currentAssembly = Assembly.GetExecutingAssembly().FullName;
 
            // In this case we are always using the current assembly
            assemblyName = currentAssembly;
 
            // Get the type using the typeName and assemblyName
            typeToDeserialize = Type.GetType(String.Format("{0}, {1}",
                typeName, assemblyName));
 
            return typeToDeserialize;
        }
    }

    /// <summary>
    /// Parent class for all packets sent over the network between the Server and the Client.
    /// </summary>
    /// <remarks>
    /// Technically, this class is abstract, since every NetworkPacket is
    /// really an implementation of a derived class. But due to the serialization,
    /// it cannot be marked as abstract.
    /// </remarks>
    [Serializable]
    public class NetworkPacket
    {
        [NonSerialized]
        public Stream stream;

        [NonSerialized]
        public List<byte> TransmissionBuffer = new List<byte>();
        [NonSerialized]
        public const int BufferSize = 256;
        [NonSerialized]
        public byte[] buffer = new byte[BufferSize];
        [NonSerialized]
        protected BinaryFormatter formatter = new BinaryFormatter();

        // Technically, this IS serialized, but we do it manually
        [NonSerialized]
        public String PacketType;

        /// <summary>
        /// Convert this object into a byte array
        /// </summary>
        /// <returns></returns>
        private byte[] Serialize()
        {
            MemoryStream mem = new MemoryStream();
            formatter.Serialize(mem, PacketType);
            formatter.Serialize(mem, this);
            return mem.GetBuffer(); ;
        }

        /// <summary>
        /// Converts the byte array contained in TransmissionBuffer
        /// into a specific Networkpacket
        /// </summary>
        /// <returns>A base NetworkPacket, with PacketType set if the packet cannot be deserialized</returns>
        private NetworkPacket Deserialize()
        {
            byte[] dataBuffer = TransmissionBuffer.ToArray();
            MemoryStream mem = new MemoryStream();
            mem.Write(dataBuffer, 0, dataBuffer.Length);
            mem.Seek(0, 0);

            formatter.Binder = new AllowAllAssemblyVersionsDeserializationBinder();

            try
            {
                PacketType = (String)formatter.Deserialize(mem);
                switch (PacketType)
                {
                    case WelcomePacket.Identifier:
                        return (WelcomePacket)formatter.Deserialize(mem);
                    case StatePacket.Identifier:
                        return (WelcomePacket)formatter.Deserialize(mem);
                    case LoginRequestPacket.Identifier:
                        return (LoginRequestPacket)formatter.Deserialize(mem);
                    case LoginStatusPacket.Identifier:
                        return (LoginStatusPacket)formatter.Deserialize(mem);
                    case RestartPacket.Identifier:
                        return (RestartPacket)formatter.Deserialize(mem);
                    case StartGamePacket.Identifier:
                        return (StartGamePacket)formatter.Deserialize(mem);
                    case CreateAccountPacket.Identifier:
                        return (CreateAccountPacket)formatter.Deserialize(mem);
                    case ErrorPacket.Identifier:
                        return (ErrorPacket)formatter.Deserialize(mem);
                    default:
                        return this;
                }
            }
            catch (SerializationException ex)
            {
                PacketType = "<none>";
                Console.WriteLine(ex.Message);
                return this;
            }
        }

        /// <summary>
        /// Sends a packet over the network
        /// </summary>
        /// <param name="callBack">The callback (PacketSendCallback) used when this operation is completed</param>
        /// <param name="parameter">Any parameter you might want to pass to the callback function</param>
        /// <returns>True if the asynchronous write was kicked off successfully</returns>
        public bool Send(PacketSendCallback callBack, object parameter = null)
        {
            if (stream == null)
                return false;

            List<object> list = new List<object>();
            list.Add(callBack);
            list.Add(parameter);

            try
            {
                buffer = Serialize();
                stream.BeginWrite(buffer, 0, buffer.Length, SendCallback, list);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Internal callback after a Send()
        /// </summary>
        /// <param name="result"></param>
        private void SendCallback(IAsyncResult result)
        {
            Exception exception = null;
            try
            {
                stream.EndWrite(result);

            }
            catch (Exception ex)
            {
                exception = ex;
            }

            List<object> list = (List<object>)result.AsyncState;

            PacketSendCallback handler = (PacketSendCallback)list[0];
            object parameter = list[1];

            handler(exception, parameter);
        }

        /// <summary>
        /// Receives a packet from over the network
        /// </summary>
        /// <param name="callBack">The callback (PacketReceiveCallback) used when this operation is completed</param>
        /// <param name="parameter">Any parameter you might want to pass to the callback function</param>
        /// <returns>True if the asynchronous read was kicked off successfully</returns>
        public bool Receive(PacketReceiveCallback callBack, object parameter = null)
        {
            if (stream == null)
                return false;

            List<object> list = new List<object>();
            list.Add(callBack);
            list.Add(parameter);

            try
            {
                stream.BeginRead(buffer, 0, buffer.Length, ReceiveCallback, list);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Internal callback after a Receive()
        /// </summary>
        /// <param name="result"></param>
        private void ReceiveCallback(IAsyncResult result)
        {
            Exception exception = null;
            List<object> list = (List<object>)result.AsyncState;

            try
            {
                int bytesRead = stream.EndRead(result);

                if (bytesRead == 0)
                {
                    stream.BeginRead(buffer, 0, buffer.Length, ReceiveCallback, list);
                    return;
                }
                else
                {
                    for (int i = 0; i < bytesRead; i++)
                    {
                        TransmissionBuffer.Add(buffer[i]);
                    }

                    //we need to read again if this is false
                    if (TransmissionBuffer.Count != buffer.Length)
                    {
                        stream.BeginRead(buffer, 0, buffer.Length, ReceiveCallback, list);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            PacketReceiveCallback handler = (PacketReceiveCallback)list[0];
            object parameter = list[1];

            NetworkPacket receivedPacket = this.Deserialize();
            receivedPacket.stream = stream;
            handler(exception, receivedPacket, parameter);
        }

    }
}

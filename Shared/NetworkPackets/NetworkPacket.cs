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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Net.Sockets;
using System.Reflection;

namespace BMORPG.NetworkPackets
{
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

    [Serializable]
    class NetworkPacket
    {
        [NonSerialized]
        public Socket socket;
        [NonSerialized]
        public List<byte> TransmissionBuffer = new List<byte>();
        [NonSerialized]
        public const int BufferSize = 1024;
        [NonSerialized]
        public byte[] buffer = new byte[BufferSize];
        [NonSerialized]
        protected BinaryFormatter formatter = new BinaryFormatter();

        // Technically, this IS serialized, but we do it manually
        [NonSerialized]
        public String PacketType;

        public byte[] Serialize()
        {
            MemoryStream mem = new MemoryStream();
            formatter.Serialize(mem, PacketType);
            formatter.Serialize(mem, this);
            return mem.GetBuffer(); ;
        }

        public NetworkPacket Deserialize()
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
                    case LoginPacket.Identifier:
                        return (LoginPacket)formatter.Deserialize(mem);
                    default:
                        return null;
                }
            }
            catch (SerializationException ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}

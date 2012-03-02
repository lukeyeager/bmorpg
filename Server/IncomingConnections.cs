using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace BMORPG_Server
{
    /// <summary>
    /// This class is used to contain all of the incoming connection streams
    /// </summary>
    public sealed class IncomingConnections
    {
        private static List<Stream> inCons = new List<Stream>();
        private static object inConLock = new Object();

        private IncomingConnections() { }

        /// <summary>
        /// This method adds a given Stream object to the end of the List of Streams.
        /// </summary>
        /// <param name="inCon">Stream object for an incoming connection to the server.  To be added to the List of Stream objects</param>
        public static void push(Stream inCon)
        {
            //need implementation
            lock (inConLock)
            {
                inCons.Add(inCon);
            }
            return;
        }

        /// <summary>
        /// Removes a Stream object from the front of the List of Stream objects and returns it.  Returns null if the List is empty.
        /// </summary>
        /// <returns>The Stream object at the front of the List</returns>
        public static Stream pop()
        {
            //need implementation
            Stream temp = null;
            lock (inConLock)
            {
                if (inCons.Count != 0)
                {
                    temp = inCons[0];
                    inCons.RemoveAt(0);
                }
            }
            return null;
        }
    }
}

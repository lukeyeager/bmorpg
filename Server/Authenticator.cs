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
using System.Linq;
using System.Text;
using System.IO;

namespace BMORPG_Server
{
    /// <summary>
    /// This class runs within its own thread. It watches Server.incomingConnections,
    /// authenticates the Players, then adds them to Server.authenticatedPlayers
    /// </summary>
    public class Authenticator
    {
        /// <summary>
        /// Temporary stub. This is the function that watches Server.incomingConnections
        /// </summary>
        public void RunAuthenticator()
        {
            // For example
            //Stream stream = Server.incomingConnections[0];
            //stream.BeginRead();
            while (true)
            {
                Stream incoming = IncomingConnections.pop();
                if (incoming == null)
                    continue;
                //need-to-do list: figure out buffers for different streams; determine what Object to use for last parameter; get packet info from Luke.
                IAsyncResult result = incoming.BeginRead(null, 0, 0, authenticate, null);
            }
        }

        public void authenticate(IAsyncResult result)
        {
            if (!result.IsCompleted)
            {
                //ummm....
            }
            else
            {
                //need-to-do list: figure out how to get information from the buffer; decipher that information and make it useable; authenticate.
            }
        }
    }
}

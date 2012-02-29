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

namespace BMORPG_Server
{
    /// <summary>
    /// This class runs within its own thread. It watches Server.authenticatedPlayers,
    /// matches players together, and starts a Game between them
    /// </summary>
    public class MatchMaker
    {
        List<Player> players = new List<Player>();

        /// <summary>
        /// Temporary stub. We need some function to run within a thread and watch Server.authenticatedPlayers
        /// </summary>
        public void RunMatchMaker()
        {
        }
    }
}

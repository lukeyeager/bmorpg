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
using System.Threading;
using BMORPG.NetworkPackets;
using System.Net.Sockets;

/*To Do:
 * Get player objects from rest of  server
 * Decide how to link the players attacks to effect list
 * 
 * */


namespace BMORPG_Server
{
    /// <summary>
    /// Represents a game being played between two Players
    /// </summary>
    class Game
    {
        public Player player1, player2;

        public Game(Player p1, Player p2)
        {
            player1 = p1;
            player2 = p2;
        }

        /// <summary>
        /// Entry point for the Thread
        /// </summary>
        public void Start()
        {
            Console.WriteLine("Starting Game between " + player1.username + " and " + player2.username);

            if (!SendStartGamePackets())
                return;

            while ((player1.CurrentHealth > 0) && (player2.CurrentHealth > 0))    // fight to the death
            {
                Console.WriteLine("Playing game between " + player1.username + " and " + player2.username);

                // receive input from one && receive input from two
                // after both commands rec, compute effect list
                    //speed affects the order of populating the effect list
                    //user provided commands(attacks/items) should be last two items in list
                Thread.Sleep(Server.SleepTime());
            }

            // For now, let's just quit
            Console.WriteLine("Ending game between " + player1.username + " and " + player2.username);
            player1.netStream.Close();
            player2.netStream.Close();
        }

        /// <summary>
        /// Lets each Player know the game has started, and sends all relevant data to each player
        /// </summary>
        /// <returns>True if both Clients are still connected</returns>
        private bool SendStartGamePackets()
        {
            // Step 1: send to player1

            StartGamePacket packet = new StartGamePacket();
            packet.opponentUsername = player2.username;
            packet.stream = player1.netStream;

            if (!packet.Send(SendStartGamePacketsCallback, player1))
                return false;

            // Step 2: send to player2

            packet = new StartGamePacket();
            packet.opponentUsername = player1.username;
            packet.stream = player2.netStream;

            if (!packet.Send(SendStartGamePacketsCallback, player2))
                return false;

            return true;
        }

        /// <summary>
        /// Callback after sending a StartGamePacket
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="parameter"></param>
        private void SendStartGamePacketsCallback(Exception ex, object parameter)
        {
            Player player = (Player)parameter;
            if (ex != null)
            {
                Console.WriteLine("Game received exception while sending start game packets: " + ex.Message);
                player.netStream.Close();
            }
            else
                Console.WriteLine("Sent start game packet to " + player.username);

            //player.current_health = 0;    is this necessary?
        }

        //private void calculateEffects has been deleted and replaced with attribute Properties in the Player class. (JDF)

    }
}

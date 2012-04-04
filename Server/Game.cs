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

/* Note: This is how we calculate damage:
 ((2A/5+2)*B*C)/D)/50)+2)*X)*Y/10)*Z)/255

A = attacker's Level
B = attacker's Attack or Special
C = attack Power
D = defender's Defense or Special
X = same-Type attack bonus (1 or 1.5)
Y = Type modifiers (40, 20, 10, 5, 2.5, o

From http://www.math.miami.edu/~jam/azure/compendium/battdam.htm
 */

namespace BMORPG_Server
{
    /// <summary>
    /// Represents a game being played between two Players
    /// </summary>
    class Game
    {
        public Player player1, player2;
        bool playerOneTurn;
        public ManualResetEvent allDone = new ManualResetEvent(false);

        public Game(Player p1, Player p2)
        {
            player1 = p1;
            player2 = p2;
            playerOneTurn = true;   // player 1 goes first
        }

        /// <summary>
        /// Entry point for the Thread
        /// </summary>
        public void Start()
        {
            Console.WriteLine("Starting Game between " + player1.Username + " and " + player2.Username);

            if (!SendStartGamePackets())
                return;


            while ((player1.CurrentHealth > 0) && (player2.CurrentHealth > 0))    // fight to the death
            {
                // Note: only call CurrentHealth once each turn so as not to deal damage twice. (JDF)
                allDone.Reset();
                Console.WriteLine("Playing game between " + player1.Username + " and " + player2.Username);

                // TODO: 
                // after both commands rec, compute effect list based on the player and moveID
                    // speed affects the order of populating the effect list
                    // user provided commands(attacks/items) should be last two items in list

                NetworkPacket packet = new NetworkPacket();
                
                if (playerOneTurn)
                {
                    packet.stream = player1.netStream;
                    packet.Receive(ReceivePacketCallback, player1);
                }
                else // player2's turn
                {
                    packet.stream = player2.netStream;
                    packet.Receive(ReceivePacketCallback, player2);
                }

                allDone.WaitOne();
                // After both PlayerMovePackets are received, calculate their healths
                // and send a StatePacket back to each player

                // age active effects for each player.


                player1.expireTurn();
                player2.expireTurn();

                playerOneTurn = !playerOneTurn; // change whose turn it is

            }

            // For now, let's just quit
            Console.WriteLine("Ending game between " + player1.Username + " and " + player2.Username);
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
            packet.opponentUsername = player2.Username;
            packet.stream = player1.netStream;

            if (!packet.Send(SendStartGamePacketsCallback, player1))
                return false;

            // Step 2: send to player2

            packet = new StartGamePacket();
            packet.opponentUsername = player1.Username;
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
                Console.WriteLine("Sent start game packet to " + player.Username);

            //player.current_health = 0;    is this necessary?
        }

        /// <summary>
        /// Callback after receiving a PlayerMovePacket
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="packet"></param>
        /// <param name="obj"></param>
        public void ReceivePacketCallback(Exception exception, NetworkPacket packet, object obj)
        {
            if (exception != null)
            {
                Console.WriteLine("Game Thread received an error while receiving a move: " + exception.Message);
                packet.stream.Close();
                return;
            }

            Player player = (Player) obj;
            Player opponent = playerOneTurn ? player2 : player1;
            bool validMove = false;
    
            if (packet is PlayerMovePacket)
            {
                PlayerMovePacket movePacket = (PlayerMovePacket) packet;

                Console.WriteLine("Player ID: " + player.UserID);
                Console.WriteLine("Move Type: " + ((PlayerMovePacket)packet).moveType);

               //     opponent.receiveMove(movePacket.moveType, player);
                int ID = movePacket.moveID;
                switch (movePacket.moveType)
                {
                    case PlayerMovePacket.MoveType.Item:
                        Item item = Item.masterList[ID];
                        for (int i = 0; i < item.Effects.Count; i++)
                        {
                            Effect tempE = Effect.masterList[item.Effects[i]];
                            bool enemy = item.Enemy[i];
                            if (enemy)
                            {
                                //apply to opponent
                            }
                            else
                            {
                                //apply to me
                            }
                        }
                        break;

                    case PlayerMovePacket.MoveType.Ability:

                        break;

                    case PlayerMovePacket.MoveType.Equipment:
                        break;

                    default:
                        Console.WriteLine("No move type specified in PlayerMovePacket");
                        break;

                }

                // TODO: make sure it was a valid move (player wasn't somehow cheating)
                // only allow to move on to the next turn if it was a valid move
                if(validMove)   
                   allDone.Set();  //allow Start() thread to continue if it was a valid move
            }
            else {
                Console.WriteLine("Received invalid packet in game thread");
            }
        }       

        //private void calculateEffects has been deleted and replaced with attribute Properties in the Player class. (JDF)

    }
}

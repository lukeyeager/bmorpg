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
using System.Data.SqlClient;

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

            NetworkPacket packet = new NetworkPacket();
            
 //           while ((player1.CurrentHealth > 0) && (player2.CurrentHealth > 0))    // fight to the death
            while ((player1.simpleHealth > 0) && (player2.simpleHealth > 0))    // fight to the death
            {
                // Note: only call CurrentHealth once each turn so as not to deal damage twice. (JDF)
                // TODO
                // send state packet to clients

                Console.WriteLine("Playing game between " + player1.Username + " and " + player2.Username);

                allDone.Reset();

                if (playerOneTurn)
                {
                    packet.stream = player1.netStream;
                    packet.Receive(ReceivePacketCallback);
                }
                else // player2's turn
                {
                    packet.stream = player2.netStream;
                    packet.Receive(ReceivePacketCallback);
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

        #region Methods for handling Players' moves

        /// <summary>
        /// Given a Player and the ID of an Item, this will remove an instance of the Item from the Player's inventory, both in memory and the database.
        /// </summary>
        /// <param name="p">The Player whose inventory the Item is being removed from.</param>
        /// <param name="item">The ID number of the Item to be removed (as in the Items table in the db).</param>
        /// <param name="enemy">The enemy Player to place effects on by the Item.</param>
        /// <returns>Whether the removal of the Item from the Player's inventory was successful or not.</returns>
        private bool usePlayerItem(Player p, int item, Player enemy)
        {
            if (!p.hasItem(item))
            {
                //means the player does not have this item
                return false;
            }
            if (!p.useItem(item, enemy))
            {
                //do something because removing the valid item failed.
                Console.WriteLine("Removing an Item from a Player's inventory failed!");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="equipment"></param>
        /// <param name="enemy"></param>
        /// <returns></returns>
        private bool usePlayerEquipment(Player p, int equipment, Player enemy)
        {
            if (!p.hasEquipment(equipment))
            {
                //means the player does not have this equipment
                return false;
            }
            if (!p.useEquipment(equipment, enemy))
            {
                //do something because removing the valid item failed.
                Console.WriteLine("Equipping something from a Player's inventory failed!");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="ability"></param>
        /// <param name="enemy"></param>
        /// <returns></returns>
        private bool usePlayerAbility(Player p, int ability, Player enemy)
        {
            if (!p.hasAbility(ability))
            {
                //means the player does not have this equipment
                return false;
            }
            if (!p.useAbility(ability, enemy))
            {
                //do something because removing the valid item failed.
                Console.WriteLine("Using a Player's Ability failed!");
                return false;
            }
            return true;
        }

        #endregion

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
        /// <param name="exception">Exception thrown if an error occurs.</param>
        /// <param name="packet">NetworkPacet, should be a PlayerMovePacket.</param>
        /// <param name="obj">A Player object which is the player who sent the move.</param>
        public void ReceivePacketCallback(Exception exception, NetworkPacket packet, object obj)
        {
            if (exception != null)
            {
                Console.WriteLine("Game Thread received an error while receiving a move: " + exception.Message);
                packet.stream.Close();
                return;
            }

            Player player = (Player)obj;
            bool validMove = false;

            if (packet is PlayerMovePacket)
            {
                PlayerMovePacket movePacket = (PlayerMovePacket)packet;

                Console.WriteLine("Player ID: " + player.UserID);
                Console.WriteLine("Move Type: " + ((PlayerMovePacket)packet).moveType);

                //     opponent.receiveMove(movePacket.moveType, player);
                int ID = movePacket.moveID;
               
         // ------------------------------
         // used for simpleHealth calculations

                Random rnd = new Random();  //used for simple health calculations
                                            //subtract a random number from opponent's health

                int randomAttack = rnd.Next(10, 30); //between the interval [10, 30]
                int randomDefense = rnd.Next(15, 25);

                // if "special" is clicked, it might hit with high damage
                // but can possible miss entirely
                int randomSpecial = (rnd.Next(4) == 1) ? rnd.Next(40, 50) : 0;
                
        // ------------------------------

                switch (movePacket.moveType)
                {
                    case PlayerMovePacket.MoveType.Item:    // "defend" clicked on client

                        if (player.UserID == player1.UserID){
                            validMove = usePlayerItem(player1, movePacket.moveID, player2);
                            player1.simpleHealth += randomDefense;
                        }
                        else{
                            validMove = usePlayerItem(player2, movePacket.moveID, player1);
                            player2.simpleHealth += randomDefense;
                        }
                        break;

                    case PlayerMovePacket.MoveType.Ability:    // "attack1" clicked on client
                        
                        if (player.UserID == player1.UserID){
                            validMove = usePlayerAbility(player1, movePacket.moveID, player2);
                            player2.simpleHealth -= randomAttack;
                        }
                        else{
                            validMove = usePlayerAbility(player2, movePacket.moveID, player1);
                            player1.simpleHealth -= randomAttack;
                        }
                        break;

                    case PlayerMovePacket.MoveType.Equipment:    // "attack2" clicked on client
                      
                        if (player.UserID == player1.UserID){
                            validMove = usePlayerEquipment(player1, movePacket.moveID, player2);
                            player2.simpleHealth -= randomAttack;
                        }
                        else{
                            validMove = usePlayerEquipment(player2, movePacket.moveID, player1);
                            player1.simpleHealth -= randomAttack;
                        }
                        break;

                    case PlayerMovePacket.MoveType.None:

                        if (player.UserID == player1.UserID)
                        {
                            validMove = usePlayerEquipment(player1, movePacket.moveID, player2);
                            player2.simpleHealth -= randomSpecial;
                        }
                        else
                        {
                            validMove = usePlayerEquipment(player2, movePacket.moveID, player1);
                            player1.simpleHealth -= randomSpecial;
                        }
                        break;

                    default:    // "special" clicked on client
                        Console.WriteLine("No move type specified in PlayerMovePacket");
                        break;

                }

                allDone.Set();  //allow Start() thread to continue if it was a valid move

            }
            else
            {
                Console.WriteLine("Received invalid packet in game thread");
            }

            //private void calculateEffects has been deleted and replaced with attribute Properties in the Player class. (JDF)

        }
    }

}
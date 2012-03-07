﻿/*  This file is part of BMORPG.

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

/*To Do:
 * Get player objects from rest of  server
 * Decide how to link the players attacks to effect list
 * 
 * */


namespace BMORPG_Server
{
    /// <summary>
    /// an enumeration for the different types of effects that there are.
    /// </summary>
    /// <remarks>Move to new file?</remarks>
    public enum EffectType
    {
        nullEffect, maxHealth, currentHealth, attack, defense, accuracy, evasion, speed
    };
    
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>Move to new file?</remarks>
    public class Effect
    {
        EffectType type;
        int magnitude;
        int turnsToLive;
        bool persistent;
        Effect linkedEffect;

        public Effect(EffectType t, int m, int ttl, bool p = false, Effect l = null)
        {
            type = t;
            magnitude = m;
            turnsToLive = ttl;
            persistent = p;
            linkedEffect = l;
        }

        public EffectType Type
        {
            get { return type; }
        }

        public int Magnitude
        {
            get { return magnitude; }
        }

        public int TurnsToLive
        {
            get { return turnsToLive; }
        }

        public bool Persistent
        {
            get { return persistent; }
        }

        public Effect LinkedEffect
        {
            get { return linkedEffect; }
        }

        public void anotherTurn()
        {
            turnsToLive--;
        }
    }

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

        public void Start()
        {
            Console.WriteLine("Starting Game between " + player1.username + " and " + player2.username);

            SendStartGamePackets();


            while ((player1.current_health > 0) || (player2.current_health > 0))    // fight to the death
            {
                // rec input from one && rec input from two
                // after both commands rec, compute effect list
                    //speed affects the order of populating the effect list
                    //user provided commands(attacks/items) should be last two items in list
            }
           
        }

        private void SendStartGamePackets()
        {
        }

        private void calculateEffects()
        {
            foreach (Effect e in player1.effects)
            {
                //calculate actual effect on player
                if (e.Type == EffectType.nullEffect) { }
                else if (e.Type == EffectType.maxHealth) { }
                else if (e.Type == EffectType.currentHealth) { }
                else if (e.Type == EffectType.attack) { }
                else if (e.Type == EffectType.defense) { }
                else if (e.Type == EffectType.accuracy) { }
                else if (e.Type == EffectType.evasion) { }
                else if (e.Type == EffectType.speed) { }
                
                //continue in the Effect's expiration
                if (e.TurnsToLive == 1)
                {
                    //remove the effect from this player's list and check for linked effects.
                    player1.effects.Remove(e);
                    if (e.LinkedEffect != null)
                        player1.addNextTurn.Add(e.LinkedEffect);
                }
                else
                    e.anotherTurn();
            }
            foreach (Effect a in player1.addNextTurn)
            {
                player1.effects.Add(a);
                player1.addNextTurn.Remove(a);
            }

            //now do the same for the second player
            foreach (Effect e in player1.effects)
            {
                //calculate actual effect on player
                if (e.Type == EffectType.nullEffect) { }
                else if (e.Type == EffectType.maxHealth) { }
                else if (e.Type == EffectType.currentHealth) { }
                else if (e.Type == EffectType.attack) { }
                else if (e.Type == EffectType.defense) { }
                else if (e.Type == EffectType.accuracy) { }
                else if (e.Type == EffectType.evasion) { }
                else if (e.Type == EffectType.speed) { }

                //continue in the Effect's expiration
                if (e.TurnsToLive == 1)
                {
                    //remove the effect from this player's list and check for linked effects.
                    player1.effects.Remove(e);
                    if (e.LinkedEffect != null)
                        player1.addNextTurn.Add(e.LinkedEffect);
                }
                else
                    e.anotherTurn();
            }
            foreach (Effect a in player1.addNextTurn)
            {
                player1.effects.Add(a);
                player1.addNextTurn.Remove(a);
            }
        }

        /*
        /// <summary>
        /// 
        /// </summary>
        private class Player2
        {

            public string name;
            public int base_max_health;
            public int tot_max_health;
            public int current_health;
            public int base_attack;
            public int tot_attack;
            public int base_defence;
            public int tot_defence;
            public int base_accuracy;
            public int tot_accuracy;
            public int base_evasion;
            public int tot_evasion;
            public int base_speed;
            public int tot_speed;
            public List<Effect> effects;
            public List<Effect> addNextTurn;

            public Player2()
            {
                effects = new List<Effect>();
                addNextTurn = new List<Effect>();
                name = "";
                base_max_health = 0;
                tot_max_health = 0;
                current_health = 0;
                base_attack = 0;
                tot_attack = 0;
                base_defence = 0;
                tot_defence = 0;
                base_accuracy = 0;
                tot_accuracy = 0;
                base_evasion = 0;
                tot_evasion = 0;
                base_speed = 0;
                tot_speed = 0;
            }
        }
         */
    }
}

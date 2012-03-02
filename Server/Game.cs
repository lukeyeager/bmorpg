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

namespace BMORPG_Server
{
    /// <summary>
    /// 
    /// </summary>
    class Game
    {
        string username1, username2;
        Player one, two;

        public Game(string _n1, string _n2)
        {
            username1 = _n1;
            username2 = _n2;
        }

        public void Start()
        {
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(1000);
                Console.WriteLine(username1 + " vs. " + username2);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class Effect
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
        }

        /// <summary>
        /// an enumeration for the different types of effects that there are.
        /// </summary>
        private enum EffectType
        {

        };

        /// <summary>
        /// 
        /// </summary>
        private class Player2
        {
            string name;
            List<Effect> effects;

            public Player2()
            {
                effects = new List<Effect>();
            }
        }
    }
}

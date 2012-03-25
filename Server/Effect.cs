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
    /// An enumeration for the different types of effects that there are.
    /// </summary>
    public enum EffectType
    {
        nullEffect, defaultAttack, maxHealth, currentHealth, attack, defense, accuracy, evasion, speed
        //Note: use defaultAttack in order to do an attack.  If we want later we can change code to allow for attack at 1.5X power, etc.  (JDF)
    };

    /// <summary>
    /// Changes one of the values of a player's attributes if added the the player's list of Effects.  
    /// </summary>
    public class Effect
    {
        private EffectType type;
        private int magnitude;
        private int turnsToLive;
        private bool persistent;
        private int linkedEffect;

        /// <summary>
        /// Constructor fot the Effect class.  It's pretty self-explanatory.
        /// </summary>
        /// <param name="t">The type of this effect.</param>
        /// <param name="m">The magnitude of this effect.  The higher a magnitude, the greater the Effect's effect.</param>
        /// <param name="ttl">The turns for this effect to remain active on a player.</param>
        /// <param name="p">Whether this effect is indefinitely permanent or not.</param>
        /// <param name="l">An Effect to add to the player's list of Effects when this effect is over.</param>
        public Effect(EffectType t, int m, int ttl, bool p = false, int l = -1)
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

        public int LinkedEffect
        {
            get { return linkedEffect; }
        }

        public void anotherTurn()
        {
            if(turnsToLive > 0)
                turnsToLive--;
        }
    }
}

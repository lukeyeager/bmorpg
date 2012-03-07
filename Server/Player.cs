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
    // This class should be linked to the database somehow
    class Player
    {
        /// <summary>
        /// The socket connection related to this user
        /// </summary>
        public Stream netStream = null;

        public String username;

        // Consider this maybe? (LCY)
        // private int maxHealth;
        //      Base value based on player stats
        // public int getMaxHealth();
        //      This parses the effects list for effects of type MaxHealth, and adds/subtracts the modifiers from the base

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

        public Player()
        {
            effects = new List<Effect>();
            addNextTurn = new List<Effect>();
            username = "";
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
}

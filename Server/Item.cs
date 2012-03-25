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
    class Item
    {
        private string name;
        private string description;
        private List<int> effects;
        //indicates whether each Effect is placed on the opposing player or the using/casting player.
        private List<bool> enemy;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="d"></param>
        /// <param name="ef"></param>
        /// <param name="en"></param>
        public Item(string n, string d, List<int> ef, List<bool> en)
        {
            name = n;
            description = d;
            effects = new List<int>();
            foreach (int i in ef)
            {
                effects.Add(i);
            }
            enemy = new List<bool>();
            foreach (bool b in en)
            {
                enemy.Add(b);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="d"></param>
        /// <param name="ef"></param>
        /// <param name="en"></param>
        public Item(string n, string d, List<byte> ef, List<byte> en)
        {
            name = n;
            description = d;
            effects = new List<int>();
            // testing on converting from an array of bytes to integers is needed!
            for (int x = 0; x < ef.Count; x += 4)
            {
                int one = Convert.ToInt32(ef[x]);
                int two = Convert.ToInt32(ef[x + 1]);
                int three = Convert.ToInt32(ef[x + 2]);
                int four = Convert.ToInt32(ef[x + 3]);
                int total = one * 16777216 + two * 65536 + three * 256 + four;
                effects.Add(total);
            }
            enemy = new List<bool>();
            foreach (byte b in en)
            {
                enemy.Add(Convert.ToBoolean(b));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            get
            {
                return description;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<int> Effects
        {
            get
            {
                return effects;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<bool> Enemy
        {
            get
            {
                return enemy;
            }
        }
    }
}

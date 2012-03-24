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
                int total = one + two * 256 + three * 65536 + four * 16777216;
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

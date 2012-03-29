using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMORPG_Server
{
    /// <summary>
    /// An abstract class meant to encapsulate all of the different types of objects that contain Effects (e.g. Items, Equipment, Abilities, etc.)
    /// </summary>
    class EffectContainer
    {
        protected string name;
        protected string description;
        protected List<int> effects;
        //indicates whether each Effect is placed on the opposing player or the using/casting player.
        protected List<bool> enemy;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="d"></param>
        /// <param name="ef"></param>
        /// <param name="en"></param>
        public EffectContainer(string n, string d, List<int> ef, List<bool> en)
        {
            if (ef.Count != en.Count)
                throw new Exception("There must be a matching number of enemy booleans for every Effect (and vice versa).");//throw exception?
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
        public EffectContainer(string n, string d)
        {
            name = n;
            description = d;
            effects = new List<int>();
            enemy = new List<bool>();
        }

        //also plan to make this obsolete
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ef"></param>
        /// <param name="en"></param>
        public void AddEffect(int ef, bool en)
        {
            effects.Add(ef);
            enemy.Add(en);
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

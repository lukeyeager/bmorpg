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
using System.Data.SqlClient;

namespace BMORPG_Server
{
    /// <summary>
    /// Represents a Player on a connected Client
    /// </summary>
    class Player
    {
        /// <summary>
        /// The socket connection related to this user
        /// </summary>
        public Stream netStream = null;

        #region Attributes with their own database column

        /// <summary>
        /// A Player's name
        /// </summary>
        /// <remarks>DB: Players.Username</remarks>
        private String username;
        public String Username
        {
            get { return username; }
        }

        /// <summary>
        /// This Player's unique identifier in the database
        /// </summary>
        /// <remarks>DB: Players.ID</remarks>
        private int userID;
        public int UserID
        {
            get { return userID; }
        }

        /// <summary>
        /// The higher a Player's level, the more difficult an opponent they should be
        /// </summary>
        /// <remarks>DB: Players.Level</remarks>
        private int level;
        public int Level
        {
            get { return level; }
        }

        /// <summary>
        /// As a Player gains experience through playing Games, their Level will increase
        /// </summary>
        private int experience;
        public int Experience
        {
            get { return experience; }
        }

        #endregion

        public List<Effect> effects;
        public List<Effect> addNextTurn;
        public List<int> items;
        public List<int> equipment;
        public List<int> abilities;

        /// <summary>
        /// Constructor for the Player class.
        /// </summary>
        /// <remarks>Maybe change the constructor to connect to the database and retrieve player information?</remarks>
        /// <param name="nStream">Stream associated with the logged in user.</param>
        /// <param name="name">Name of the character.</param>
        /// <param name="id">Personal ID# associated with the player.</param>
        public Player(Stream nStream, string name, int id)
        {
            netStream = nStream;
            username = name;
            userID = id;
            effects = new List<Effect>();
            addNextTurn = new List<Effect>();
            items = new List<int>();
            equipment = new List<int>();
            abilities = new List<int>();

            //populate base stats using database

            SqlDataReader reader = null;
            SqlCommand command = new SqlCommand("SELECT MaxHealth, Attack, Defense, Accuracy, Evasion, Speed\nFROM Player\nWHERE PID="
                + userID, Server.dbConnection);
            command.CommandTimeout = 10;
            try
            {
                lock (Server.dbConnectionLock)
                {
                    reader = command.ExecuteReader();
                }
                if (reader.Read())
                {
                    base_max_health = reader.GetInt32(0);
                    base_attack = reader.GetInt32(1);
                    base_defense = reader.GetInt32(2);
                    base_accuracy = reader.GetInt32(3);
                    base_evasion = reader.GetInt32(4);
                    base_speed = reader.GetInt32(5);
                    Console.WriteLine("Read in stats from database for: " + username);
                }
                else
                {
                    Console.WriteLine("\nFailed to read in attributes for Player " + username + " UID: " + userID
                        + " from the database:\n\nRecords for this Player do not exist in the database");
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nFailed to read in attributes for Player " + username + " UID: " + userID 
                    + " from the database:\n");
                Console.WriteLine(ex.ToString());
                if (reader != null)
                    reader.Close();
            }

            //read in Items for the player

            reader = null;
            command = new SqlCommand("SELECT Item_ID\nFROM PlayerItems\nWHERE Player_ID="
                + userID, Server.dbConnection);
            command.CommandTimeout = 10;
            try
            {
                lock (Server.dbConnectionLock)
                {
                    reader = command.ExecuteReader();
                }
                while (reader.Read())
                {
                    int item_ID = reader.GetInt32(0);
                    items.Add(item_ID);
                };
                Console.WriteLine("Read in items from database for: " + username);
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nFailed to read in items for Player " + username + " UID: " + userID
                    + " from the database:\n");
                Console.WriteLine(ex.ToString());
                if (reader != null)
                    reader.Close();
            }

            //read in Equipment for the player

            reader = null;
            command = new SqlCommand("SELECT Equipment_ID\nFROM PlayerEquipments\nWHERE Player_ID="
                + userID, Server.dbConnection);
            command.CommandTimeout = 10;
            try
            {
                lock (Server.dbConnectionLock)
                {
                    reader = command.ExecuteReader();
                }
                while (reader.Read())
                {
                    int equipment_ID = reader.GetInt32(0);
                    equipment.Add(equipment_ID);
                };
                Console.WriteLine("Read in equipment from database for: " + username);
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nFailed to read in equipment for Player " + username + " UID: " + userID
                    + " from the database:\n");
                Console.WriteLine(ex.ToString());
                if (reader != null)
                    reader.Close();
            }

            //read in Abilities for the player

            reader = null;
            command = new SqlCommand("SELECT Ability_ID\nFROM PlayerAbilities\nWHERE Player_ID="
                + userID, Server.dbConnection);
            command.CommandTimeout = 10;
            try
            {
                lock (Server.dbConnectionLock)
                {
                    reader = command.ExecuteReader();
                }
                while (reader.Read())
                {
                    int ability_ID = reader.GetInt32(0);
                    abilities.Add(ability_ID);
                };
                Console.WriteLine("Read in abilities from database for: " + username);
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nFailed to read in abilities for Player " + username + " UID: " + userID
                    + " from the database:\n");
                Console.WriteLine(ex.ToString());
                if (reader != null)
                    reader.Close();
            }

            Console.WriteLine("Successfully logged in: " + username + "\tUID: " + userID);
	        tot_max_health = 0;
	        current_health = 0;
	        tot_attack = 0;
	        tot_defense = 0;
	        tot_accuracy = 0;
	        tot_evasion = 0;
            tot_speed = 0;
        }

        /// <summary>
        /// Checks all active effects and updates them as necessary (in terms of turns to live).
        /// </summary>
        public void expireTurn()
        {
            foreach (Effect e in effects)
            {
                if (!e.Persistent)
                {
                    e.anotherTurn();
                    if (e.TurnsToLive <= 0 && e.LinkedEffect != -1)
                    {
                        addNextTurn.Add(Server.masterListEffects[e.LinkedEffect]);
                        effects.Remove(e);
                    }
                }
            }
            foreach (Effect e in addNextTurn)
            {
                effects.Add(e);
                addNextTurn.Remove(e);
            }
        }

        #region Effect Related Attributes

        /// <summary>
        /// The maximum health of the character.
        /// </summary>
        public int MaxHealth
        {
            get
            {
                tot_max_health = base_max_health;
                //iterate through the list of effects and see which ones affect the maximum health value.
                foreach (Effect e in effects)
                {
                    if (e.Type == EffectType.maxHealth)
                        tot_max_health += e.Magnitude;
                }
                return tot_max_health;
            }
        }
        private int base_max_health;
        private int tot_max_health;

        /// <summary>
        /// The character's current health.
        /// </summary>
        /// <remarks>NOTE: This updates the current_health variable, so only call it when you're sure you want to.</remarks>
        public int CurrentHealth
        {
            get
            {
                //iterate through the list of effects and see which ones affect the current health value.
                foreach (Effect e in effects)
                {
                    if (e.Type == EffectType.currentHealth)
                        current_health += e.Magnitude;
                }
                //cap health to maximum health
                if (current_health > MaxHealth)
                    current_health = tot_max_health;
                //iterate through the list again and check for attacks.
                foreach (Effect e in effects)
                {
                    if (e.Type == EffectType.defaultAttack)
                    {
                        // (FIXME) perform some type of calculation to determine amount of damage to health.
                        // it's possible that this calculation should be done elsewhere. (JDF)
                        current_health -= 0;
                    }
                }
                return current_health;
            }
        }
        private int current_health;

        /// <summary>
        /// The character's attack value.
        /// </summary>
        public int Attack
        {
            get
            {
                tot_attack = base_attack;
                //iterate through the list of effects and see which ones affect the attack value.
                foreach (Effect e in effects)
                {
                    if (e.Type == EffectType.attack)
                        tot_attack += e.Magnitude;
                }
                return tot_attack;
            }
        }
        private int base_attack;
        private int tot_attack;

        /// <summary>
        /// The character's defense value.
        /// </summary>
        public int Defense
        {
            get
            {
                tot_defense = base_defense;
                //iterate through the list of effects and see which ones affect the defense value.
                foreach (Effect e in effects)
                {
                    if (e.Type == EffectType.defense)
                        tot_defense += e.Magnitude;
                }
                return tot_defense;
            }
        }
        private int base_defense;
        private int tot_defense;

        /// <summary>
        /// The character's accuracy value.
        /// </summary>
        public int Accuracy
        {
            get
            {
                tot_accuracy = base_accuracy;
                //iterate through the list of effects and see which ones affect the accuracy value.
                foreach (Effect e in effects)
                {
                    if (e.Type == EffectType.accuracy)
                        tot_accuracy += e.Magnitude;
                }
                return tot_accuracy;
            }
        }
        private int base_accuracy;
        private int tot_accuracy;

        /// <summary>
        /// The character's evasion value.
        /// </summary>
        public int Evasion
        {
            get
            {
                tot_evasion = base_evasion;
                //iterate through the list of effects and see which ones affect the evasion health value.
                foreach (Effect e in effects)
                {
                    if (e.Type == EffectType.evasion)
                        tot_evasion += e.Magnitude;
                }
                return tot_evasion;
            }
        }
        private int base_evasion;
        private int tot_evasion;

        /// <summary>
        /// The character's speed value.
        /// </summary>
        public int Speed
        {
            get
            {
                tot_speed = base_speed;
                //iterate through the list of effects and see which ones affect the speed value.
                foreach (Effect e in effects)
                {
                    if (e.Type == EffectType.speed)
                        tot_speed += e.Magnitude;
                }
                return tot_speed;
            }
        }
        private int base_speed;
        private int tot_speed;

        #endregion
    }
}

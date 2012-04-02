using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace BMORPG_Server
{
    class Ability : EffectContainer
    {
        /// <summary>
        /// Holds all of the Abilities that exist in the game.
        /// </summary>
        public static Dictionary<int, Ability> masterList = new Dictionary<int, Ability>();

        public Ability(string n, string d, int id)
            : base(n, d)
        {
            SqlDataReader reader = null;
            SqlCommand command = new SqlCommand("SELECT Effect_ID, Enemy\nFROM AbilityEffects\nWHERE Ability_ID = " + id, Server.dbConnectionSecondary);
            command.CommandTimeout = 3;
            try
            {
                lock (Server.dbConnectionSecondaryLock)
                {
                    reader = command.ExecuteReader();
                }
                while (reader.Read())
                {
                    int AbilityID = reader.GetInt32(0);
                    bool en = reader.GetBoolean(1);
                    effects.Add(AbilityID);
                    enemy.Add(en);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nFailed to read in Linked Effects for Ability \'" + n + "\' from the database:\n");
                Console.WriteLine(ex.ToString());
                if (reader != null)
                    reader.Close();
            }
        }

        /// <summary>
        /// Reads in the information for all of the Abilities from the database.
        /// </summary>
        public static void populateMasterList()
        {
            SqlDataReader reader = null;
            SqlCommand command = new SqlCommand("SELECT *\nFROM Abilities", Server.dbConnection);
            command.CommandTimeout = 3;
            try
            {
                lock (Server.dbConnectionLock)
                {
                    reader = command.ExecuteReader();
                }
                while (reader.Read())
                {
                    int AbID = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    string description = reader.GetString(2);
                    Equipment temp = new Equipment(name, description, AbID);
                    Console.WriteLine("Item: AbID = " + AbID + "; Name = " + name + "; Description = \"" + description + "\"");
                    Equipment.masterList.Add(AbID, temp);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nFailed to read in Abilities from the database:\n");
                Console.WriteLine(ex.ToString());
                if (reader != null)
                    reader.Close();
            }
        }
    }
}

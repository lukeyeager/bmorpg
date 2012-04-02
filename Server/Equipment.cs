using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace BMORPG_Server
{
    class Equipment : EffectContainer
    {
        /// <summary>
        /// Holds all of the Equipment that exist in the game.
        /// </summary>
        public static Dictionary<int, Equipment> masterList = new Dictionary<int, Equipment>();

        public Equipment(string n, string d, int id)
            : base(n, d)
        {
            SqlDataReader reader = null;
            SqlCommand command = new SqlCommand("SELECT Effect_ID, Enemy\nFROM EquipmentEffects\nWHERE Equipment_ID = " + id, Server.dbConnectionSecondary);
            command.CommandTimeout = 3;
            try
            {
                lock (Server.dbConnectionSecondaryLock)
                {
                    reader = command.ExecuteReader();
                }
                while (reader.Read())
                {
                    int EffectID = reader.GetInt32(0);
                    bool en = reader.GetBoolean(1);
                    effects.Add(EffectID);
                    enemy.Add(en);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nFailed to read in Linked Effects for Equipment \'" + n + "\' from the database:\n");
                Console.WriteLine(ex.ToString());
                if (reader != null)
                    reader.Close();
            }
        }

        /// <summary>
        /// Reads in the information for all of the Equipment from the database.
        /// </summary>
        public static void populateMasterList()
        {
            SqlDataReader reader = null;
            SqlCommand command = new SqlCommand("SELECT *\nFROM Equipments", Server.dbConnection);
            command.CommandTimeout = 3;
            try
            {
                lock (Server.dbConnectionLock)
                {
                    reader = command.ExecuteReader();
                }
                while (reader.Read())
                {
                    int EqID = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    string description = reader.GetString(2);
                    Equipment temp = new Equipment(name, description, EqID);
                    Console.WriteLine("Item: EqID = " + EqID + "; Name = " + name + "; Description = \"" + description + "\"");
                    Equipment.masterList.Add(EqID, temp);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nFailed to read in Equipment from the database:\n");
                Console.WriteLine(ex.ToString());
                if (reader != null)
                    reader.Close();
            }
        }
    }
}

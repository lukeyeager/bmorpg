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
using System.Data.SqlClient;

namespace BMORPG_Server
{
    class Item : EffectContainer
    {
        /// <summary>
        /// Holds all of the Items that exist in the game.
        /// </summary>
        public static Dictionary<int, Item> masterList = new Dictionary<int, Item>();

        public Item(string n, string d, int id)
            : base(n, d)
        {
            SqlDataReader reader = null;
            SqlCommand command = new SqlCommand("SELECT Effect_ID, Enemy\nFROM ItemEffects\nWHERE Item_ID = " + id, Server.dbConnectionSecondary);
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
                Console.WriteLine("\nFailed to read in Linked Effects for Item \'" + n + "\' from the database:\n");
                Console.WriteLine(ex.ToString());
                if (reader != null)
                    reader.Close();
            }
        }

        /// <summary>
        /// Reads in the information for all of the Items from the database.
        /// </summary>
        public static void populateMasterList()
        {
            SqlDataReader reader = null;
            SqlCommand command = new SqlCommand("SELECT *\nFROM Items", Server.dbConnection);
            command.CommandTimeout = 3;
            try
            {
                lock (Server.dbConnectionLock)
                {
                    reader = command.ExecuteReader();
                }
                while (reader.Read())
                {
                    int ItID = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    string description = reader.GetString(2);
                    Item temp = new Item(name, description, ItID);
                    Console.WriteLine("Item: ItID = " + ItID + "; Name = " + name + "; Description = \"" + description + "\"");
                    Item.masterList.Add(ItID, temp);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nFailed to read in Items from the database");
                //Console.WriteLine(ex.ToString());
                if (reader != null)
                    reader.Close();
            }
        }
    }
}

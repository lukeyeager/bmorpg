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
    class Item: EffectContainer
    {
        public Item(string n, string d)
            : base(n, d)
        {
        }

        /// <summary>
        /// Reads in the information for all of the Items from the database.
        /// </summary>
        public static void populateMasterList()
        {
            SqlDataReader reader = null;
            SqlCommand command = new SqlCommand("SELECT *\nFROM Items", Server.dbConnection);
            command.CommandTimeout = 10;
            bool readLinks = true;
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
                    Console.WriteLine("Item: ItID = " + ItID + "; Name = " + name + "; Description = \"" + description + "\"");
                    Item temp = new Item(name, description);
                    //maybe make the master list a static object in this class?
                    Server.masterListItems.Add(ItID, temp);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                readLinks = false;
                Console.WriteLine("\nFailed to read in Items from the database:\n");
                Console.WriteLine(ex.ToString());
                if (reader != null)
                    reader.Close();
            }

            //if more db connections added, this will be moved to Item constructor
            if (readLinks)
            {
                //read in links to effects from database (but not if there was an error reading in Items from the db
                foreach (KeyValuePair<int, Item> i in Server.masterListItems)
                {
                    reader = null;
                    command = new SqlCommand("Select Effect_ID, Enemy\nFROM ItemEffects\nWHERE Item_ID = \'" + i.Key + "\'", Server.dbConnection);
                    command.CommandTimeout = 10;
                    try
                    {
                        lock (Server.dbConnectionLock)
                        {
                            reader = command.ExecuteReader();
                        }
                        while (reader.Read())
                        {
                            int Effect_ID = reader.GetInt32(0);
                            bool enemy = reader.GetBoolean(1);
                            i.Value.AddEffect(Effect_ID, enemy);
                        }
                        Console.WriteLine("Another Effect read in for an Item.");
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("\nFailed to read in links to Effects table for Items from the database:\n");
                        Console.WriteLine(ex.ToString());
                        if (reader != null)
                            reader.Close();
                        break;
                    }
                }
            }
        }
    }
}

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
using System.IO;

namespace BMORPG_Server
{
    class ProfileFetcher
    {
        public Player fetchProfile(string playerName, Stream netStream)
        {
            Player playerProfile;
            //need to get details for UID, PWD, and Database
            SqlConnection connect = new SqlConnection("UID=username;PWD=password;Addr=(local);Trusted_Connection=sspi;" +
                "Database=database;Connection Timeout=5;ApplicationIntent=ReadOnly");
            SqlDataReader reader = null;
            SqlCommand command = new SqlCommand("SELECT *\nFROM Profile\nWHERE Username = '" + playerName + "'" , connect);
            try
            {
                connect.Open();
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //read in from schema
                }
                reader.Close();
                connect.Close();
                //dummy until schema developed
                playerProfile = null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                playerProfile = null;
            }
            return playerProfile;
        }
    }
}

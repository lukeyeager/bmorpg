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
            SqlCommand command = new SqlCommand("SELECT *\nFROM Profile\nWHERE Username = " + playerName, connect);
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

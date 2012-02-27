using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace BMORPG_Server
{
    class ProfileFetcher
    {
        string playerName;
        string playerProfile;

        public ProfileFetcher(string name)
        {
            playerName = name;
            //need to get details for UID, PWD, and Database
            SqlConnection connect = new SqlConnection("UID=username;PWD=password;Addr=(local);Trusted_Connection=sspi;" +
                "Database=database;Connection Timeout=5;ApplicationIntent=ReadOnly");
            try
            {
                connect.Open();

                SqlDataReader reader = null;
                SqlCommand command = new SqlCommand("", connect);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //read in from schema
                }

                connect.Close();
                //dummy until schema developed
                playerProfile = "";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                playerProfile = "";
            }
        }

        public string getProfile()
        {
            return playerProfile;
        }
    }
}

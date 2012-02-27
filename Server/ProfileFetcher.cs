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
            SqlConnection connect = new SqlConnection("user id=username;Pwd=password;server=localhost;" +
                "Trusted_Connection=yes;database=database;Connection Timeout=5");
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
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            playerProfile = "";
        }

        public string getProfile()
        {
            return playerProfile;
        }
    }
}

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
using System.Threading;
using System.Diagnostics;
using System.IO;
using BMORPG_Server.Listeners;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace BMORPG_Server
{
    /// <summary>
    /// Main class for this Project
    /// </summary>
    class Server
    {
        public const string ServerVersion = "1.0";

        //Lock dbConnectionLock before using dbConnection in order to avoid multi-threaded problems.
        //Note: it may be impractical to have one universal connection to the database.  Consider having a collection of them? (JDF)
        public static SqlConnection dbConnection = null;
        public static object dbConnectionLock = new Object();

        //Might want to have this in Game class instead of Server class.
        /// <summary>
        /// Holds all of the Effects needed for items, abilities, etc.
        /// </summary>
        public static Dictionary<int, Effect> masterListEffects = new Dictionary<int, Effect>();
        /// <summary>
        /// Holds all of the Items that exist in the game.
        /// </summary>
        public static Dictionary<int, Item> masterListItems = new Dictionary<int, Item>();

        /// <summary>
        /// The ConnectionListener adds Streams to this list,
        /// and the Authenticator removes them
        /// </summary>
        public static SafeList<Stream> incomingConnections = new SafeList<Stream>(null);

        /// <summary>
        /// The Authenticator adds Players to this list,
        /// and the Matchmaker removes them
        /// </summary>
        public static SafeList<Player> authenticatedPlayers = new SafeList<Player>(null);

        /// <summary>
        /// The MatchMaker adds Games to this list
        /// </summary>
        public static SafeList<Game> currentGames = new SafeList<Game>(null);

        public static Thread listenThread = null;
        public static ConnectionListener listener = null;

        public static Thread authenticatorThread = null;
        public static Authenticator authenticator = null;

        public static Thread matchMakerThread = null;
        public static MatchMaker matchMaker = null;
        
        /// <summary>
        /// Program entry point
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Make database connection
            try
            {
                dbConnection = new SqlConnection("UID=records;PWD=aBCfta13;Addr=(local)\\BMORPG;Trusted_Connection=sspi;" +
                    "Database=BMORPG;Connection Timeout=5;");
                dbConnection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to open database connection: " + ex.Message);
                dbConnection = null;
            }

            // Read in necessary information from the database
            // Effects
            SqlDataReader reader = null;
            SqlCommand command = new SqlCommand("SELECT *\nFROM Effects", Server.dbConnection);
            command.CommandTimeout = 10;
            try
            {
                lock (Server.dbConnectionLock)
                {
                    reader = command.ExecuteReader();
                }
                while (reader.Read())
                {
                    int EID = reader.GetInt32(0);
                    EffectType type = (EffectType) reader.GetInt32(1);
                    int magnitude = reader.GetInt32(2);
                    int turnsToLive = reader.GetInt32(3);
                    bool persistent = reader.GetBoolean(4);
                    if (reader.IsDBNull(5))
                    {
                        Console.WriteLine("Effect: EID = " + EID + "; type = " + ((int)type) + "; magnitude = " + magnitude
                            + "; turnsToLive = " + turnsToLive + "; persistent = " + persistent + "; linked effect = NULL");
                        Effect temp = new Effect(type, magnitude, turnsToLive, persistent);
                        masterListEffects.Add(EID, temp);
                    }
                    else
                    {
                        int linkedEffect = reader.GetInt32(5);
                        Console.WriteLine("Effect: EID = " + EID + "; type = " + ((int)type) + "; magnitude = " + magnitude
                            + "; turnsToLive = " + turnsToLive + "; persistent = " + persistent + "; linked effect = " + linkedEffect);
                        Effect temp = new Effect(type, magnitude, turnsToLive, persistent, linkedEffect);
                        masterListEffects.Add(EID, temp);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nFailed to read in Effects from the database:\n");
                Console.WriteLine(ex.ToString());
                if (reader != null)
                    reader.Close();
            }

            // Items
            reader = null;
            command = new SqlCommand("SELECT *\nFROM Items", Server.dbConnection);
            command.CommandTimeout = 10;
            try
            {
                lock (Server.dbConnectionLock)
                {
                    reader = command.ExecuteReader();
                }
                while (reader.Read())
                {
                    int ItID = reader.GetInt32(0);
                    byte[] tempBArray = new byte[40];
                    long bytesRead = reader.GetBytes(1, 0, tempBArray, 0, 40);
                    byte[] reallyTemp = new byte[bytesRead];
                    for (long x = 0; x < bytesRead; x++)
                    {
                        reallyTemp[x] = tempBArray[x];
                    }
                    List<byte> effects = new List<byte>(reallyTemp);
                    tempBArray = new byte[10];
                    bytesRead = reader.GetBytes(2, 0, tempBArray, 0, 10);
                    reallyTemp = new byte[bytesRead];
                    for (long x = 0; x < bytesRead; x++)
                    {
                        reallyTemp[x] = tempBArray[x];
                    }
                    List<byte> enemy = new List<byte>(reallyTemp);
                    string name = reader.GetString(3);
                    string description = reader.GetString(4);
                    Console.Write("Item: ItID = " + ItID + "; Name = " + name + "; Description = \"" + description + "\"; Effect indices = ");
                    foreach (byte b in effects)
                    {
                        Console.Write(Convert.ToInt32(b));
                        Console.Write(',');
                    }
                    Console.Write("\b; Enemy indices = ");
                    foreach (byte b in enemy)
                    {
                        Console.Write(Convert.ToBoolean(b));
                        Console.Write(',');
                    }
                    Console.WriteLine("\b ");
                    Item temp = new Item(name, description, effects, enemy);
                    masterListItems.Add(ItID, temp);
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                //errorMessage = ex.ToString();
                Console.WriteLine("\nFailed to read in Items from the database:\n");
                Console.WriteLine(ex.ToString());
                if (reader != null)
                    reader.Close();
            }

            // Decide which port to use
            int port = 11000;   //Default
            if (args.Length > 0)
            {
                try
                {
                    port = Convert.ToInt32(args[0]);
                }
                catch(Exception)
                {
                    Console.WriteLine("Failed to convert " + args[0] + " to a port#.");
                }
            }
            Console.WriteLine("Using port: " + port);

            // Start the Connection Listener
            //listener = new SecureListener("certificates/BmorpgCA.cer", true);
            listener = new UnsecureListener();
            listenThread = new Thread(() => listener.Listen(port));
            listenThread.IsBackground = false;
            listenThread.Start();

            // Start the Authenticator
            authenticator = new Authenticator();
            authenticatorThread = new Thread(authenticator.RunAuthenticator);
            authenticatorThread.IsBackground = false;
            authenticatorThread.Start();

            // Start the MatchMaker
            matchMaker = new MatchMaker();
            matchMakerThread = new Thread(matchMaker.RunMatchMaker);
            matchMakerThread.IsBackground = false;
            matchMakerThread.Start();
        }

        /// <summary>
        /// Restarts the server via a batch script
        /// </summary>
        /// <param name="updateSvn">Should the script update the SVN before restarting the Server?</param>
        public static void Restart( bool updateSvn = false )
        {
            Process proc = new Process();
            proc.StartInfo.FileName = "RestartServer.bat";
            if (updateSvn)
                proc.StartInfo.Arguments = "svn";

            // Get this directory's parent's parent (../..)
            string dir = new FileInfo(Directory.GetCurrentDirectory()).Directory.Parent.FullName;
            Console.WriteLine("Dir: " + dir);
            proc.StartInfo.WorkingDirectory = dir;
            proc.Start();

            Environment.Exit(0);
        }

        /// <summary>
        /// Creates random numbers
        /// </summary>
        static private Random random = new Random();

        /// <summary>
        /// Returns a random amount of time for a thread to sleep
        /// </summary>
        /// <returns>Time in milliseconds</returns>
        public static int SleepTime()
        {
            return 1000 + random.Next(500);
        }

    }
}

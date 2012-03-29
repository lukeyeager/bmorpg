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
            Effect.populateMasterList();

            // Items
            Item.populateMasterList();

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

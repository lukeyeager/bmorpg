using BMORPG_Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BMORPG.NetworkPackets;

namespace BMORPG_Server_Test
{
    
    
    /// <summary>
    ///This is a test class for GameTest and is intended
    ///to contain all GameTest Unit Tests
    ///</summary>
    [TestClass()]
    public class GameTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for Start
        ///</summary>
        [TestMethod()]
        public void StartTest()
        {
            Player p1 = null; // TODO: Initialize to an appropriate value
            Player p2 = null; // TODO: Initialize to an appropriate value
            Game target = new Game(p1, p2); // TODO: Initialize to an appropriate value
            target.Start();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ReceivePacketCallback
        ///</summary>
        [TestMethod()]
        public void ReceivePacketCallbackTest()
        {
            Player p1 = null; // TODO: Initialize to an appropriate value
            Player p2 = null; // TODO: Initialize to an appropriate value
            Game target = new Game(p1, p2); // TODO: Initialize to an appropriate value
            Exception exception = null; // TODO: Initialize to an appropriate value
            NetworkPacket packet = null; // TODO: Initialize to an appropriate value
            object obj = null; // TODO: Initialize to an appropriate value
            target.ReceivePacketCallback(exception, packet, obj);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SendStartGamePackets
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BMORPG_Server.exe")]
        public void SendStartGamePacketsTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            Game_Accessor target = new Game_Accessor(param0); // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.SendStartGamePackets();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for SendStartGamePacketsCallback
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BMORPG_Server.exe")]
        public void SendStartGamePacketsCallbackTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            Game_Accessor target = new Game_Accessor(param0); // TODO: Initialize to an appropriate value
            Exception ex = null; // TODO: Initialize to an appropriate value
            object parameter = null; // TODO: Initialize to an appropriate value
            target.SendStartGamePacketsCallback(ex, parameter);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for usePlayerItem
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BMORPG_Server.exe")]
        public void usePlayerItemTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            Game_Accessor target = new Game_Accessor(param0); // TODO: Initialize to an appropriate value
            Player_Accessor p1 = null; // TODO: Initialize to an appropriate value
            Player_Accessor p2 = null; // TODO: Initialize to an appropriate value
            int item = 0; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.usePlayerItem(p1, item, p2);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Game Constructor
        ///</summary>
        [TestMethod()]
        public void GameConstructorTest()
        {
            Player p1 = null; // TODO: Initialize to an appropriate value
            Player p2 = null; // TODO: Initialize to an appropriate value
            Game target = new Game(p1, p2);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }
    }
}

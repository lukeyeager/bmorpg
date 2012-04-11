using BMORPG_Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace BMORPG_Server_Test
{
    
    
    /// <summary>
    ///This is a test class for PlayerTest and is intended
    ///to contain all PlayerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PlayerTest
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
        ///A test for Player Constructor
        ///</summary>
        [TestMethod()]
        public void PlayerConstructorTest()
        {
            Stream nStream = null; // TODO: Initialize to an appropriate value
            string name = string.Empty; // TODO: Initialize to an appropriate value
            int id = 0; // TODO: Initialize to an appropriate value
            Player target = new Player(nStream, name, id);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for expireTurn
        ///</summary>
        [TestMethod()]
        public void expireTurnTest()
        {
            Stream nStream = null; // TODO: Initialize to an appropriate value
            string name = string.Empty; // TODO: Initialize to an appropriate value
            int id = 0; // TODO: Initialize to an appropriate value
            Player target = new Player(nStream, name, id); // TODO: Initialize to an appropriate value
            target.expireTurn();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for hasItem
        ///</summary>
        [TestMethod()]
        public void hasItemTest()
        {
            Stream nStream = null; // TODO: Initialize to an appropriate value
            string name = string.Empty; // TODO: Initialize to an appropriate value
            int id = 0; // TODO: Initialize to an appropriate value
            Player target = new Player(nStream, name, id); // TODO: Initialize to an appropriate value
            int item = 0; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.hasItem(item);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for instantiateDBConnection
        ///</summary>
        [TestMethod()]
        public void instantiateDBConnectionTest()
        {
            Player.instantiateDBConnection();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for useItem
        ///</summary>
        [TestMethod()]
        public void useItemTest()
        {
            Stream nStream = null; // TODO: Initialize to an appropriate value
            string name = string.Empty; // TODO: Initialize to an appropriate value
            int id = 0; // TODO: Initialize to an appropriate value
            Player target = new Player(nStream, name, id); // TODO: Initialize to an appropriate value
            Player enemy = new Player(nStream, name, id); // TODO: Initialize to an appropriate value
            int item = 0; // TODO: Initialize to an appropriate value
            bool expected = false; // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.useItem(item, enemy);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}

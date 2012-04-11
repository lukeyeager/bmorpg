using BMORPG_Server.Listeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace BMORPG_Server_Test
{
    
    
    /// <summary>
    ///This is a test class for ConnectionListenerTest and is intended
    ///to contain all ConnectionListenerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ConnectionListenerTest
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


        internal virtual ConnectionListener CreateConnectionListener()
        {
            // TODO: Instantiate an appropriate concrete class.
            ConnectionListener target = null;
            return target;
        }

        /// <summary>
        ///A test for AddConnection
        ///</summary>
        [TestMethod()]
        public void AddConnectionTest()
        {
            ConnectionListener target = CreateConnectionListener(); // TODO: Initialize to an appropriate value
            Stream stream = null; // TODO: Initialize to an appropriate value
            target.AddConnection(stream);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Listen
        ///</summary>
        [TestMethod()]
        public void ListenTest()
        {
            ConnectionListener target = CreateConnectionListener(); // TODO: Initialize to an appropriate value
            int port = 0; // TODO: Initialize to an appropriate value
            target.Listen(port);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        internal virtual ConnectionListener_Accessor CreateConnectionListener_Accessor()
        {
            // TODO: Instantiate an appropriate concrete class.
            ConnectionListener_Accessor target = null;
            return target;
        }

        /// <summary>
        ///A test for SendCallback
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BMORPG_Server.exe")]
        public void SendCallbackTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            ConnectionListener_Accessor target = new ConnectionListener_Accessor(param0); // TODO: Initialize to an appropriate value
            Exception ex = null; // TODO: Initialize to an appropriate value
            object parameter = null; // TODO: Initialize to an appropriate value
            target.SendCallback(ex, parameter);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}

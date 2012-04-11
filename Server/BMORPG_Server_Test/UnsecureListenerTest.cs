using BMORPG_Server.Listeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BMORPG_Server_Test
{
    
    
    /// <summary>
    ///This is a test class for UnsecureListenerTest and is intended
    ///to contain all UnsecureListenerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class UnsecureListenerTest
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
        ///A test for UnsecureListener Constructor
        ///</summary>
        [TestMethod()]
        public void UnsecureListenerConstructorTest()
        {
            UnsecureListener target = new UnsecureListener();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for Listen
        ///</summary>
        [TestMethod()]
        public void ListenTest()
        {
            UnsecureListener target = new UnsecureListener(); // TODO: Initialize to an appropriate value
            int port = 0; // TODO: Initialize to an appropriate value
            target.Listen(port);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}

using BMORPG_Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BMORPG_Server_Test
{
    
    
    /// <summary>
    ///This is a test class for MatchMakerTest and is intended
    ///to contain all MatchMakerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MatchMakerTest
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
        ///A test for MatchMaker Constructor
        ///</summary>
        [TestMethod()]
        public void MatchMakerConstructorTest()
        {
            MatchMaker target = new MatchMaker();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for RunMatchMaker
        ///</summary>
        [TestMethod()]
        public void RunMatchMakerTest()
        {
            MatchMaker target = new MatchMaker(); // TODO: Initialize to an appropriate value
            target.RunMatchMaker();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}

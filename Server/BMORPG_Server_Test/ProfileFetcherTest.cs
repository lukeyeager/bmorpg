using BMORPG_Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace BMORPG_Server_Test
{
    
    
    /// <summary>
    ///This is a test class for ProfileFetcherTest and is intended
    ///to contain all ProfileFetcherTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ProfileFetcherTest
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
        ///A test for ProfileFetcher Constructor
        ///</summary>
        [TestMethod()]
        public void ProfileFetcherConstructorTest()
        {
            ProfileFetcher target = new ProfileFetcher();
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for fetchProfile
        ///</summary>
        [TestMethod()]
        public void fetchProfileTest()
        {
            ProfileFetcher target = new ProfileFetcher(); // TODO: Initialize to an appropriate value
            string playerName = string.Empty; // TODO: Initialize to an appropriate value
            Stream netStream = null; // TODO: Initialize to an appropriate value
            Player expected = null; // TODO: Initialize to an appropriate value
            Player actual;
            actual = target.fetchProfile(playerName, netStream);
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}

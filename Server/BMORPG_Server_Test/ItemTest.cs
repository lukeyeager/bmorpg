using BMORPG_Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BMORPG_Server_Test
{
    
    
    /// <summary>
    ///This is a test class for ItemTest and is intended
    ///to contain all ItemTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ItemTest
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
        ///A test for Item Constructor
        ///</summary>
        [TestMethod()]
        public void ItemConstructorTest()
        {
            string n = string.Empty; // TODO: Initialize to an appropriate value
            string d = string.Empty; // TODO: Initialize to an appropriate value
            int id = 0; // TODO: Initialize to an appropriate value
            Item target = new Item(n, d, id);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for populateMasterList
        ///</summary>
        [TestMethod()]
        public void populateMasterListTest()
        {
            Item.populateMasterList();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}

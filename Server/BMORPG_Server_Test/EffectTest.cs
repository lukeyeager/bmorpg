using BMORPG_Server;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BMORPG_Server_Test
{
    
    
    /// <summary>
    ///This is a test class for EffectTest and is intended
    ///to contain all EffectTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EffectTest
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
        ///A test for Effect Constructor
        ///</summary>
        [TestMethod()]
        public void EffectConstructorTest()
        {
            EffectType t = new EffectType(); // TODO: Initialize to an appropriate value
            int m = 0; // TODO: Initialize to an appropriate value
            int ttl = 0; // TODO: Initialize to an appropriate value
            bool p = false; // TODO: Initialize to an appropriate value
            int l = 0; // TODO: Initialize to an appropriate value
            Effect target = new Effect(t, m, ttl, p, l);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for anotherTurn
        ///</summary>
        [TestMethod()]
        public void anotherTurnTest()
        {
            EffectType t = new EffectType(); // TODO: Initialize to an appropriate value
            int m = 0; // TODO: Initialize to an appropriate value
            int ttl = 0; // TODO: Initialize to an appropriate value
            bool p = false; // TODO: Initialize to an appropriate value
            int l = 0; // TODO: Initialize to an appropriate value
            Effect target = new Effect(t, m, ttl, p, l); // TODO: Initialize to an appropriate value
            target.anotherTurn();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for populateMasterList
        ///</summary>
        [TestMethod()]
        public void populateMasterListTest()
        {
            Effect.populateMasterList();
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for LinkedEffect
        ///</summary>
        [TestMethod()]
        public void LinkedEffectTest()
        {
            EffectType t = new EffectType(); // TODO: Initialize to an appropriate value
            int m = 0; // TODO: Initialize to an appropriate value
            int ttl = 0; // TODO: Initialize to an appropriate value
            bool p = false; // TODO: Initialize to an appropriate value
            int l = 0; // TODO: Initialize to an appropriate value
            Effect target = new Effect(t, m, ttl, p, l); // TODO: Initialize to an appropriate value
            int actual;
            actual = target.LinkedEffect;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Magnitude
        ///</summary>
        [TestMethod()]
        public void MagnitudeTest()
        {
            EffectType t = new EffectType(); // TODO: Initialize to an appropriate value
            int m = 0; // TODO: Initialize to an appropriate value
            int ttl = 0; // TODO: Initialize to an appropriate value
            bool p = false; // TODO: Initialize to an appropriate value
            int l = 0; // TODO: Initialize to an appropriate value
            Effect target = new Effect(t, m, ttl, p, l); // TODO: Initialize to an appropriate value
            int actual;
            actual = target.Magnitude;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Persistent
        ///</summary>
        [TestMethod()]
        public void PersistentTest()
        {
            EffectType t = new EffectType(); // TODO: Initialize to an appropriate value
            int m = 0; // TODO: Initialize to an appropriate value
            int ttl = 0; // TODO: Initialize to an appropriate value
            bool p = false; // TODO: Initialize to an appropriate value
            int l = 0; // TODO: Initialize to an appropriate value
            Effect target = new Effect(t, m, ttl, p, l); // TODO: Initialize to an appropriate value
            bool actual;
            actual = target.Persistent;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for TurnsToLive
        ///</summary>
        [TestMethod()]
        public void TurnsToLiveTest()
        {
            EffectType t = new EffectType(); // TODO: Initialize to an appropriate value
            int m = 0; // TODO: Initialize to an appropriate value
            int ttl = 0; // TODO: Initialize to an appropriate value
            bool p = false; // TODO: Initialize to an appropriate value
            int l = 0; // TODO: Initialize to an appropriate value
            Effect target = new Effect(t, m, ttl, p, l); // TODO: Initialize to an appropriate value
            int actual;
            actual = target.TurnsToLive;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Type
        ///</summary>
        [TestMethod()]
        public void TypeTest()
        {
            EffectType t = new EffectType(); // TODO: Initialize to an appropriate value
            int m = 0; // TODO: Initialize to an appropriate value
            int ttl = 0; // TODO: Initialize to an appropriate value
            bool p = false; // TODO: Initialize to an appropriate value
            int l = 0; // TODO: Initialize to an appropriate value
            Effect target = new Effect(t, m, ttl, p, l); // TODO: Initialize to an appropriate value
            EffectType actual;
            actual = target.Type;
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}

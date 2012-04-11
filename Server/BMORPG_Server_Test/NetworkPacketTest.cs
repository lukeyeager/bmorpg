using BMORPG.NetworkPackets;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace BMORPG_Server_Test
{
    
    
    /// <summary>
    ///This is a test class for NetworkPacketTest and is intended
    ///to contain all NetworkPacketTest Unit Tests
    ///</summary>
    [TestClass()]
    public class NetworkPacketTest
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
        ///A test for Deserialize
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BMORPG_Server.exe")]
        public void DeserializeTest()
        {
            NetworkPacket_Accessor target = new NetworkPacket_Accessor();
            NetworkPacket result = target.Deserialize();

            Assert.AreEqual(result.PacketType, "<none>", "Should not be able to deserialize an uninitialized packet");

            target = new NetworkPacket_Accessor();
            target.PacketType = ErrorPacket.Identifier;
            result = target.Deserialize();

            Assert.AreEqual(result.PacketType, "<none>", "Should not be able to deserialize an empty packet");
        }

        /// <summary>
        ///A test for Serialize
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BMORPG_Server.exe")]
        public void SerializeTest()
        {
            ErrorPacket_Accessor target = new ErrorPacket_Accessor();
            target.message = "Test";

            byte[] result = null;
            result = target.Serialize();
            Assert.IsNotNull(result, "Serialization returned a null byte array");
        }

        /// <summary>
        ///A test for Send
        ///</summary>
        [TestMethod()]
        public void SendTest()
        {
            NetworkPacket target = new NetworkPacket();
            PacketSendCallback callBack = null;
            bool actual;
            actual = target.Send(callBack);
            Assert.AreEqual(false, actual, "Should fail to send a packet without a stream");
        }

        /// <summary>
        ///A test for Receive
        ///</summary>
        [TestMethod()]
        public void ReceiveTest()
        {
            NetworkPacket target = new NetworkPacket();
            PacketReceiveCallback callBack = null;
            bool actual;
            actual = target.Receive(callBack);
            Assert.AreEqual(false, actual, "Should fail to receive a packet without a stream");
        }
    }
}

using BMORPG_Server.Listeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net.Security;
using System.Net.Sockets;

namespace BMORPG_Server_Test
{
    
    
    /// <summary>
    ///This is a test class for SecureListenerTest and is intended
    ///to contain all SecureListenerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SecureListenerTest
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
        ///A test for SecureListener Constructor
        ///</summary>
        [TestMethod()]
        public void SecureListenerConstructorTest()
        {
            string certificate = string.Empty; // TODO: Initialize to an appropriate value
            bool _verbose = false; // TODO: Initialize to an appropriate value
            SecureListener target = new SecureListener(certificate, _verbose);
            Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for DisplayCertificateInformation
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BMORPG_Server.exe")]
        public void DisplayCertificateInformationTest()
        {
            SslStream stream = null; // TODO: Initialize to an appropriate value
            SecureListener_Accessor.DisplayCertificateInformation(stream);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for DisplaySecurityLevel
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BMORPG_Server.exe")]
        public void DisplaySecurityLevelTest()
        {
            SslStream stream = null; // TODO: Initialize to an appropriate value
            SecureListener_Accessor.DisplaySecurityLevel(stream);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for DisplaySecurityServices
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BMORPG_Server.exe")]
        public void DisplaySecurityServicesTest()
        {
            SslStream stream = null; // TODO: Initialize to an appropriate value
            SecureListener_Accessor.DisplaySecurityServices(stream);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for DisplayStreamProperties
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BMORPG_Server.exe")]
        public void DisplayStreamPropertiesTest()
        {
            SslStream stream = null; // TODO: Initialize to an appropriate value
            SecureListener_Accessor.DisplayStreamProperties(stream);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for Listen
        ///</summary>
        [TestMethod()]
        public void ListenTest()
        {
            string certificate = string.Empty; // TODO: Initialize to an appropriate value
            bool _verbose = false; // TODO: Initialize to an appropriate value
            SecureListener target = new SecureListener(certificate, _verbose); // TODO: Initialize to an appropriate value
            int port = 0; // TODO: Initialize to an appropriate value
            target.Listen(port);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ProcessClient
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BMORPG_Server.exe")]
        public void ProcessClientTest()
        {
            PrivateObject param0 = null; // TODO: Initialize to an appropriate value
            SecureListener_Accessor target = new SecureListener_Accessor(param0); // TODO: Initialize to an appropriate value
            TcpClient client = null; // TODO: Initialize to an appropriate value
            target.ProcessClient(client);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}

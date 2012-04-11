using BMORPGClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows;
using BMORPG.NetworkPackets;

namespace BmorpgClientTests
{
    
    
    /// <summary>
    ///This is a test class for MainWindowTest and is intended
    ///to contain all MainWindowTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MainWindowTest
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
        
        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
            //Connect to server, or something like that
        }
        
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for buttonCreateAccount_Click
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BMORPG_Client.exe")]
        public void buttonCreateAccount_ClickTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            RoutedEventArgs e = null; // TODO: Initialize to an appropriate value
            target.buttonCreateAccount_Click(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ConnectButton_Click
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BMORPG_Client.exe")]
        public void ConnectButton_ClickTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.ConnectButton_Click(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for LoginButton_Click
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BMORPG_Client.exe")]
        public void LoginButton_ClickTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            EventArgs e = null; // TODO: Initialize to an appropriate value
            target.LoginButton_Click(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ReceiveGameStart
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BMORPG_Client.exe")]
        public void ReceiveGameStartTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            Exception exception = null; // TODO: Initialize to an appropriate value
            NetworkPacket packet = null; // TODO: Initialize to an appropriate value
            object parameter = null; // TODO: Initialize to an appropriate value
            target.ReceiveGameStart(exception, packet, parameter);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ReceiveGameState
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BMORPG_Client.exe")]
        public void ReceiveGameStateTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            Exception exception = null; // TODO: Initialize to an appropriate value
            NetworkPacket packet = null; // TODO: Initialize to an appropriate value
            object parameter = null; // TODO: Initialize to an appropriate value
            target.ReceiveGameState(exception, packet, parameter);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ReceiveLoginStatus
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BMORPG_Client.exe")]
        public void ReceiveLoginStatusTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            Exception exception = null; // TODO: Initialize to an appropriate value
            NetworkPacket packet = null; // TODO: Initialize to an appropriate value
            object parameter = null; // TODO: Initialize to an appropriate value
            target.ReceiveLoginStatus(exception, packet, parameter);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for ReceiveWelcome
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BMORPG_Client.exe")]
        public void ReceiveWelcomeTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            Exception exception = null; // TODO: Initialize to an appropriate value
            NetworkPacket packet = null; // TODO: Initialize to an appropriate value
            object parameter = null; // TODO: Initialize to an appropriate value
            target.ReceiveWelcome(exception, packet, parameter);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SendCreateAccountPacketCallback
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BMORPG_Client.exe")]
        public void SendCreateAccountPacketCallbackTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            Exception ex = null; // TODO: Initialize to an appropriate value
            object parameter = null; // TODO: Initialize to an appropriate value
            target.SendCreateAccountPacketCallback(ex, parameter);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");

        }

        /// <summary>
        ///A test for SendLoginCallback
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BMORPG_Client.exe")]
        public void SendLoginCallbackTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            Exception exception = null; // TODO: Initialize to an appropriate value
            object parameter = null; // TODO: Initialize to an appropriate value
            target.SendLoginCallback(exception, parameter);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SendMovePacketCallback
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BMORPG_Client.exe")]
        public void SendMovePacketCallbackTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            Exception ex = null; // TODO: Initialize to an appropriate value
            object parameter = null; // TODO: Initialize to an appropriate value
            target.SendMovePacketCallback(ex, parameter);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for SendRestartCallback
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BMORPG_Client.exe")]
        public void SendRestartCallbackTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            Exception ex = null; // TODO: Initialize to an appropriate value
            object parameter = null; // TODO: Initialize to an appropriate value
            target.SendRestartCallback(ex, parameter);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for buttonAttack1
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BMORPG_Client.exe")]
        public void buttonAttack1Test()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            RoutedEventArgs e = null; // TODO: Initialize to an appropriate value
            target.buttonAttack1(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for buttonAttack2
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BMORPG_Client.exe")]
        public void buttonAttack2Test()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            RoutedEventArgs e = null; // TODO: Initialize to an appropriate value
            target.buttonAttack2(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for buttonDefend
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BMORPG_Client.exe")]
        public void buttonDefendTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            RoutedEventArgs e = null; // TODO: Initialize to an appropriate value
            target.buttonDefend(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for buttonGoBack_Click
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BMORPG_Client.exe")]
        public void buttonGoBack_ClickTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            RoutedEventArgs e = null; // TODO: Initialize to an appropriate value
            target.buttonGoBack_Click(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        /// <summary>
        ///A test for buttonSpecial
        ///</summary>
        [TestMethod()]
        [DeploymentItem("BMORPG_Client.exe")]
        public void buttonSpecialTest()
        {
            MainWindow_Accessor target = new MainWindow_Accessor(); // TODO: Initialize to an appropriate value
            object sender = null; // TODO: Initialize to an appropriate value
            RoutedEventArgs e = null; // TODO: Initialize to an appropriate value
            target.buttonSpecial(sender, e);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}

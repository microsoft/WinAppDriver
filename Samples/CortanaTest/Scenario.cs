using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.iOS; // Temporary placeholder until Windows namespace exists
using OpenQA.Selenium.Remote;

namespace CortanaTest
{
    [TestClass]
    public class Scenario
    {
        protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        protected static IOSDriver<IOSElement> CortanaSession;      // Temporary placeholder until Windows namespace exists
        protected static IOSDriver<IOSElement> DesktopSession;      // Temporary placeholder until Windows namespace exists
        protected static AppiumWebElement CortanaButton;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            // Create a session for Desktop
            DesiredCapabilities desktopCapabilities = new DesiredCapabilities();
            desktopCapabilities.SetCapability("app", "Root");
            DesktopSession = new IOSDriver<IOSElement>(new Uri(WindowsApplicationDriverUrl), desktopCapabilities);
            Assert.IsNotNull(DesktopSession);

            // Launch Cortana Window to allow session creation to find it
            CortanaButton = DesktopSession.FindElementByAccessibilityId("4101"); // AutomationId for Cortana button on the task bar
            CortanaButton.Click();
            System.Threading.Thread.Sleep(1000);

            // Create session for already running Cortana
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", "Microsoft.Windows.Cortana_cw5n1h2txyewy!CortanaUI");
            CortanaSession = new IOSDriver<IOSElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(CortanaSession);
            CortanaSession.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
        }

        [ClassCleanup]
        public static void TearDown()
        {
            CortanaButton = null;
            CortanaSession.Dispose();
            CortanaSession = null;
            DesktopSession.Dispose();
            DesktopSession = null;
        }

        [TestInitialize]
        public void OpenCortanaWindow()
        {
            if (CortanaButton != null)
            {
                CortanaButton.Click();
            }
        }

        [TestMethod]
        public void BingLocalSearch()
        {
            var searchBox = CortanaSession.FindElementByAccessibilityId("SearchTextBox");
            Assert.IsNotNull(searchBox);
            searchBox.SendKeys("add");

            var bingPane = CortanaSession.FindElementByName("Bing");
            Assert.IsNotNull(bingPane);

            var bingResult = bingPane.FindElementByName("Add or remove programs, System settings");
            Assert.IsNotNull(bingResult);
        }

        [TestMethod]
        public void BingRemoteSearch()
        {
            var searchBox = CortanaSession.FindElementByAccessibilityId("SearchTextBox");
            Assert.IsNotNull(searchBox);
            searchBox.SendKeys("What is eight times eleven");

            var bingPane = CortanaSession.FindElementByName("Bing");
            Assert.IsNotNull(bingPane);

            var bingResult = bingPane.FindElementByName("What is eight times eleven");
            Assert.IsNotNull(bingResult);
        }

        [TestMethod]
        public void MenuBarScenario()
        {
            var settingsListItem = CortanaSession.FindElementByAccessibilityId("SettingsListItem");
            Assert.IsNotNull(settingsListItem);

            var settingsListItemLabel = settingsListItem.FindElementByAccessibilityId("TextLabel");
            Assert.IsNotNull(settingsListItem);
            Assert.IsFalse(settingsListItemLabel.Displayed);

            CortanaSession.FindElementByAccessibilityId("HomeburgerTopItem").Click();
            System.Threading.Thread.Sleep(1000);
            Assert.IsTrue(settingsListItemLabel.Displayed);

            CortanaSession.FindElementByAccessibilityId("SettingsListItem").Click();
            var clearAllButton = CortanaSession.FindElementByAccessibilityId("DeviceSearchHistoryClearButton");
            Assert.IsNotNull(clearAllButton);
            Assert.IsTrue(clearAllButton.Displayed);
        }
    }
}

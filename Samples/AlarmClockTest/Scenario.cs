using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.iOS; // Temporary placeholder until Windows namespace exists
using OpenQA.Selenium.Remote;

namespace AlarmClockTest
{
    [TestClass]
    public class Scenario
    {
        protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        protected static IOSDriver<IOSElement> AlarmClockSession;   // Temporary placeholder until Windows namespace exists
        protected static IOSDriver<IOSElement> DesktopSession;      // Temporary placeholder until Windows namespace exists

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            // Launch the AlarmClock app
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", "Microsoft.WindowsAlarms_8wekyb3d8bbwe!App");
            AlarmClockSession = new IOSDriver<IOSElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(AlarmClockSession);
            AlarmClockSession.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));

            // Create a session for Desktop
            DesiredCapabilities desktopCapabilities = new DesiredCapabilities();
            desktopCapabilities.SetCapability("app", "Root");
            DesktopSession = new IOSDriver<IOSElement>(new Uri(WindowsApplicationDriverUrl), desktopCapabilities);
            Assert.IsNotNull(DesktopSession);
        }

        [ClassCleanup]
        public static void TearDown()
        {
            ReturnToMainPage();
            SwitchToAlarmTab();

            // Delete all created alarms
            var alarmEntries = AlarmClockSession.FindElementsByName("Windows Application Driver Test Alarm");
            foreach (var alarmEntry in alarmEntries)
            {
                AlarmClockSession.Mouse.ContextClick(alarmEntry.Coordinates);
                AlarmClockSession.FindElementByName("Delete").Click();
            }

            AlarmClockSession.Dispose();
            AlarmClockSession = null;
        }

        [TestMethod]
        public void FullScenario()
        {
            // Read the current local time
            SwitchToWorldClockTab();
            string localTimeText = ReadLocalTime();
            Assert.IsTrue(localTimeText.Length > 0);

            // Add an alarm at 1 minute after local time
            SwitchToAlarmTab();
            AddAlarm(localTimeText);
            var alarmEntries = AlarmClockSession.FindElementsByName("Windows Application Driver Test Alarm");
            Assert.IsTrue(alarmEntries.Count > 0);

            // Try to dismiss notification after 1 minute
            System.Threading.Thread.Sleep(60000);
            DismissNotification();
        }

        public static void SwitchToAlarmTab()
        {
            AlarmClockSession.FindElementByAccessibilityId("AlarmPivotItem").Click();
        }

        public void SwitchToWorldClockTab()
        {
            AlarmClockSession.FindElementByAccessibilityId("WorldClockPivotItem").Click();
        }

        public string ReadLocalTime()
        {
            string localTimeText = "";
            AppiumWebElement worldClockPivotItem = AlarmClockSession.FindElementByAccessibilityId("WorldClockPivotItem");
            if (worldClockPivotItem != null)
            {
                localTimeText = worldClockPivotItem.FindElementByClassName("ClockCardItem").Text;
                var timeStrings = localTimeText.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                if (timeStrings.Length >= 5)
                {
                    // Get the time. E.g. "Local time, Monday, February 22, 2016, 11:32 AM, "
                    localTimeText = timeStrings[4];
                }
            }

            return localTimeText;
        }

        public void AddAlarm(string timeText)
        {
            if (timeText.Length > 0)
            {
                // Create a test alarm 1 minute after the read local time
                string alarmTimeString = timeText;
                DateTimeFormatInfo fi = new CultureInfo("en-US", false).DateTimeFormat;
                DateTime alarmTime = DateTime.ParseExact(alarmTimeString, " h:mm tt", fi);
                alarmTime = alarmTime.AddMinutes(1.0);
                string hourString = alarmTime.ToString("%h", fi);
                string minuteString = alarmTime.ToString("mm", fi);
                string period = alarmTime.ToString("tt", fi);

                AlarmClockSession.FindElementByAccessibilityId("AddAlarmButton").Click();
                AlarmClockSession.FindElementByAccessibilityId("AlarmNameTextBox").Clear();
                AlarmClockSession.FindElementByAccessibilityId("AlarmNameTextBox").SendKeys("Windows Application Driver Test Alarm");
                AlarmClockSession.FindElementByAccessibilityId("AlarmTimePicker").FindElementByAccessibilityId("FlyoutButton").Click();
                AlarmClockSession.FindElementByAccessibilityId("HourLoopingSelector").FindElementByName(hourString).Click();
                AlarmClockSession.FindElementByAccessibilityId("MinuteLoopingSelector").FindElementByName(minuteString).Click();
                AlarmClockSession.FindElementByAccessibilityId("PeriodLoopingSelector").FindElementByName(period).Click();
                AlarmClockSession.FindElementByName("timepicker").FindElementByAccessibilityId("AcceptButton").Click();
                AlarmClockSession.FindElementByAccessibilityId("AlarmSaveButton").Click();
            }
        }

        public void DismissNotification()
        {
            try
            {
                AppiumWebElement newNotification = DesktopSession.FindElementByName("New notification");
                Assert.IsTrue(newNotification.FindElementByAccessibilityId("MessageText").Text.Contains("Windows Application Driver Test Alarm"));
                newNotification.FindElementByName("Dismiss").Click();
            }
            catch { }
        }

        private static void ReturnToMainPage()
        {
            // Try to return to main page in case application is started in nested view
            try
            {
                AppiumWebElement backButton = null;
                do
                {
                    backButton = AlarmClockSession.FindElementByAccessibilityId("Back");
                    backButton.Click();
                }
                while (backButton != null);
            }
            catch { }
        }
    }
}

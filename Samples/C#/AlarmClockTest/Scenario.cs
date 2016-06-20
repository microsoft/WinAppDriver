//******************************************************************************
//
// Copyright (c) 2016 Microsoft Corporation. All rights reserved.
//
// This code is licensed under the MIT License (MIT).
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//******************************************************************************

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

            // Ensure app is started in the default main page
            ReturnToMainPage();
        }

        [ClassCleanup]
        public static void TearDown()
        {
            ReturnToMainPage();
            SwitchToAlarmTab();

            // Delete all created alarms
            var alarmEntries = AlarmClockSession.FindElementsByXPath("//ListItem[starts-with(@Name, \"Windows Application Driver Test Alarm\")]");
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
            var alarmEntries = AlarmClockSession.FindElementsByXPath("//ListItem[starts-with(@Name, \"Windows Application Driver Test Alarm\")]");
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

                foreach (string timeString in timeStrings)
                {
                    // Get the time. E.g. "11:32 AM" from "Local time, Monday, February 22, 2016, 11:32 AM, "
                    if (timeString.Contains(":"))
                    {
                        localTimeText = timeString;
                        break;
                    }
                }
            }

            return localTimeText;
        }

        public void AddAlarm(string timeText)
        {
            if (timeText.Length > 0)
            {
                // Create a test alarm 1 minute after the read local time
                DateTimeFormatInfo fi = CultureInfo.CurrentUICulture.DateTimeFormat;
                DateTime alarmTime = DateTime.Parse(timeText, fi);
                alarmTime = alarmTime.AddMinutes(1.0);
                string hourString = alarmTime.ToString("%h", fi);
                string minuteString = alarmTime.ToString("mm", fi);
                string period = alarmTime.ToString("tt", fi);

                AlarmClockSession.FindElementByAccessibilityId("AddAlarmButton").Click();
                AlarmClockSession.FindElementByAccessibilityId("AlarmNameTextBox").Clear();
                AlarmClockSession.FindElementByAccessibilityId("AlarmNameTextBox").SendKeys("Windows Application Driver Test Alarm");
                AlarmClockSession.FindElementByAccessibilityId("HourSelector").FindElementByName(hourString).Click();
                AlarmClockSession.FindElementByAccessibilityId("MinuteSelector").FindElementByName(minuteString).Click();
                AlarmClockSession.FindElementByAccessibilityId("PeriodSelector").FindElementByName(period).Click();
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

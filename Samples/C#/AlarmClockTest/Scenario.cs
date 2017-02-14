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
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;

namespace AlarmClockTest
{
    [TestClass]
    public class Scenario
    {
        protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        protected static WindowsDriver<WindowsElement> AlarmClockSession;
        protected static WindowsDriver<WindowsElement> DesktopSession;
        private const string NewAlarmName = "Windows Application Driver Test Alarm";

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            // Launch the AlarmClock app
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", "Microsoft.WindowsAlarms_8wekyb3d8bbwe!App");
            AlarmClockSession = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(AlarmClockSession);
            AlarmClockSession.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));

            // Create a session for Desktop
            DesiredCapabilities desktopCapabilities = new DesiredCapabilities();
            desktopCapabilities.SetCapability("app", "Root");
            DesktopSession = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), desktopCapabilities);
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
            while (true)
            {
                try
                {
                    var alarmEntry = AlarmClockSession.FindElementByXPath(string.Format("//ListItem[starts-with(@Name, \"{0}\")]", NewAlarmName));
                    AlarmClockSession.Mouse.ContextClick(alarmEntry.Coordinates);
                    AlarmClockSession.FindElementByName("Delete").Click();
                }
                catch
                {
                    break;
                }
            }

            AlarmClockSession.Quit();
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
            var alarmEntries = AlarmClockSession.FindElementsByXPath(string.Format("//ListItem[starts-with(@Name, \"{0}\")]", NewAlarmName));
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
                try
                {
                    localTimeText = worldClockPivotItem.FindElementByClassName("ClockCardItem").Text;
                }
                catch (Exception)
                {
                    // On Windows 10 anniversary edition, the ClockCardItem has been changed to ListViewItem
                    // If the previous item wasn't found, then look for the new one.
                    localTimeText = worldClockPivotItem.FindElementByClassName("ListViewItem").Text;
                }
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
                // Remove any zero-width spaces (\u200b, \u200e, \u200f)
                timeText = timeText.Replace("\u200b", String.Empty);
                timeText = timeText.Replace("\u200e", String.Empty);
                timeText = timeText.Replace("\u200f", String.Empty);

                // Create a test alarm 1 minute after the read local time
                DateTimeFormatInfo fi = CultureInfo.CurrentUICulture.DateTimeFormat;
                DateTime alarmTime = DateTime.Parse(timeText, fi);
                alarmTime = alarmTime.AddMinutes(1.0);

                // The alarm app uses the long time format specifier
                var timeFormat = fi.LongTimePattern;
                var formatStrings = timeFormat.Split(new char[] { fi.TimeSeparator[0], ' ' });

                // A single character format string is treated as a standard format. Escape it
                if (formatStrings[0].Length == 1)
                {
                    formatStrings[0] = "%" + formatStrings[0];
                }
                string hourString = alarmTime.ToString(formatStrings[0]);

                // If the format for the hour includes a preceding 0, remove it
                if (hourString.StartsWith("0"))
                {
                    hourString = hourString.Substring(1);
                }

                // A single character format string is treated as a standard format. Escape it
                if (formatStrings[1].Length == 1)
                {
                    formatStrings[1] = "%" + formatStrings[1];
                }
                string minuteString = alarmTime.ToString(formatStrings[1]);

                // Only add the period if the time format has it
                string period = string.Empty;
                if (formatStrings.Length > 3)
                {
                    period = alarmTime.ToString(formatStrings[3]);
                }

                AlarmClockSession.FindElementByAccessibilityId("AddAlarmButton").Click();
                AlarmClockSession.FindElementByAccessibilityId("AlarmNameTextBox").Clear();
                AlarmClockSession.FindElementByAccessibilityId("AlarmNameTextBox").SendKeys(NewAlarmName);
                AlarmClockSession.FindElementByAccessibilityId("HourSelector").FindElementByName(hourString).Click();
                AlarmClockSession.FindElementByAccessibilityId("MinuteSelector").FindElementByName(minuteString).Click();
                if (!string.IsNullOrEmpty(period))
                {
                    AlarmClockSession.FindElementByAccessibilityId("PeriodSelector").FindElementByName(period).Click();
                }
                AlarmClockSession.FindElementByAccessibilityId("AlarmSaveButton").Click();
            }
        }

        public void DismissNotification()
        {
            try
            {
                AppiumWebElement newNotification = DesktopSession.FindElementByName("New notification");
                Assert.IsTrue(newNotification.FindElementByAccessibilityId("MessageText").Text.Contains(NewAlarmName));
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

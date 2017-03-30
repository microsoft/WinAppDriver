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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;

namespace W3CWebDriver
{
    public class AlarmClockBase
    {
        protected static WindowsDriver<WindowsElement> session;
        protected static RemoteTouchScreen touchScreen;
        protected WindowsElement alarmTabElement;

        public static void Setup(TestContext context)
        {
            // Cleanup leftover objects from previous test if exists
            TearDown();

            // Launch Alarm Clock
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.AlarmClockAppId);
            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);

            // Initialize touch screen object
            touchScreen = new RemoteTouchScreen(session);
            Assert.IsNotNull(touchScreen);
        }

        public static void TearDown()
        {
            // Cleanup RemoteTouchScreen object if initialized
            touchScreen = null;

            // Close the application and delete the session
            if (session != null)
            {
                session.Quit();
                session = null;
            }
        }

        [TestInitialize]
        public void TestInit()
        {
            // Attempt to go back to the main page in case Alarm & Clock app is started in EditAlarm view
            try
            {
                alarmTabElement = session.FindElementByAccessibilityId("AlarmPivotItem");
            }
            catch
            {
                session.Navigate().Back();
                alarmTabElement = session.FindElementByAccessibilityId("AlarmPivotItem");
            }

            Assert.IsNotNull(alarmTabElement);
            alarmTabElement.Click();
        }

        protected void AddAlarmEntry(string alarmName)
        {
            session.FindElementByAccessibilityId("AddAlarmButton").Click();
            session.FindElementByAccessibilityId("AlarmNameTextBox").Clear();
            session.FindElementByAccessibilityId("AlarmNameTextBox").SendKeys(alarmName);
            session.FindElementByAccessibilityId("AlarmSaveButton").Click();
        }

        protected void DeletePreviouslyCreatedAlarmEntry(string alarmName)
        {
            while (true)
            {
                try
                {
                    var alarmEntry = session.FindElementByXPath(string.Format("//ListItem[starts-with(@Name, \"{0}\")]", alarmName));
                    session.Mouse.ContextClick(alarmEntry.Coordinates);
                    session.FindElementByName("Delete").Click();
                }
                catch
                {
                    break;
                }
            }
        }

        protected void CreateStopwatchLapEntries(uint numberOfEntry)
        {
            // Navigate to Stopwatch tab
            var stopwatchPivotItem = session.FindElementByAccessibilityId("StopwatchPivotItem");
            stopwatchPivotItem.Click();

            // Reset stopwatch
            var stopwatchResetButton = stopwatchPivotItem.FindElementByAccessibilityId("StopWatchResetButton");
            var stopwatchPlayPauseButton = stopwatchPivotItem.FindElementByAccessibilityId("StopwatchPlayPauseButton");
            stopwatchResetButton.Click();

            // Collect lap entries
            stopwatchPlayPauseButton.Click();
            var stopwatchLapButton = stopwatchPivotItem.FindElementByAccessibilityId("StopWatchLapButton");
            for (uint count = 0; count < numberOfEntry; count++)
            {
                stopwatchLapButton.Click();
            }
            stopwatchPlayPauseButton.Click();
        }
    }
}
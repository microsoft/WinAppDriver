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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using System;
using System.Threading;

namespace WebDriverAPI
{
    public class AlarmClockBase
    {
        protected static WindowsDriver<WindowsElement> session;
        protected static RemoteTouchScreen touchScreen;
        protected WindowsElement alarmTabElement;

        // UI elements attributes that differ between Alarms & Clock versions
        protected string AlarmTabAutomationId;
        protected string AlarmTabClassName;
        protected string StopwatchTabAutomationId;
        protected string WorldClockTabAutomationId;

        public static void Setup(TestContext context)
        {
            // Launch Alarms & Clock application if it is not yet launched
            if (session == null || touchScreen == null || !Utility.CurrentWindowIsAlive(session))
            {
                TearDown();
                session = Utility.CreateNewSession(CommonTestSettings.AlarmClockAppId);
                Assert.IsNotNull(session);
                Assert.IsNotNull(session.SessionId);

                // Set implicit timeout to 2.5 seconds to make element search to retry every 500 ms for at most five times
                session.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(2.5);

                // Initialize touch screen object
                touchScreen = new RemoteTouchScreen(session);
                Assert.IsNotNull(touchScreen);
            }
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
        public virtual void TestInit()
        {
            // Attempt to go back to the main page in case Alarms & Clock app is started in EditAlarm view
            try
            {
                alarmTabElement = FindAlarmTabElement();
            }
            catch
            {
                DismissAddAlarmPage();
                alarmTabElement = FindAlarmTabElement();
            }

            Assert.IsNotNull(alarmTabElement);
            if (!alarmTabElement.Selected)
            {
                alarmTabElement.Click();
            }

            // Different Alarm & Clock application version uses different UI elements
            if (alarmTabElement.GetAttribute("AutomationId") == "AlarmButton")
            {
                // Latest version of Alarms & Clock application
                AlarmTabClassName = "ListViewItem";
                AlarmTabAutomationId = "AlarmButton";
                StopwatchTabAutomationId = "StopwatchButton";
                WorldClockTabAutomationId = "ClockButton";
            }
            else
            {
                // Earlier version of Alarms & Clock application
                AlarmTabClassName = "PivotItem";
                AlarmTabAutomationId = "AlarmPivotItem";
                StopwatchTabAutomationId = "StopwatchPivotItem";
                WorldClockTabAutomationId = "WorldClockPivotItem";
            }
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
                    var alarmEntry = session.FindElementByXPath($"//ListItem[starts-with(@Name, \"{alarmName}\")]");
                    session.Mouse.ContextClick(alarmEntry.Coordinates);
                    Thread.Sleep(TimeSpan.FromSeconds(3));
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
            var stopwatchPivotItem = session.FindElementByAccessibilityId(StopwatchTabAutomationId);
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

        protected static WindowsElement GetStaleElement()
        {
            // Open the add alarm page, locate the save button, and click it to get a stale save button
            session.FindElementByAccessibilityId("AddAlarmButton").Click();
            Thread.Sleep(TimeSpan.FromSeconds(0.5));
            WindowsElement staleElement = session.FindElementByAccessibilityId("AlarmSaveButton");
            DismissAddAlarmPage();
            Thread.Sleep(TimeSpan.FromSeconds(2));
            return staleElement;
        }

        protected static void DismissAddAlarmPage()
        {
            try
            {
                session.FindElementByAccessibilityId("CancelButton").Click(); // Press cancel button to dismiss any non-main page
            }
            catch
            {
                session.FindElementByAccessibilityId("Back").Click(); // Press back button if cancel button above somehow failed
                Thread.Sleep(TimeSpan.FromSeconds(1));
                session.DismissAlarmDialogIfThere();
            }
        }

        protected static WindowsElement FindAlarmTabElement()
        {
            WindowsElement element;
            try
            {
                // The latest Alarms & Clock application uses a ListViewItem
                // with "AlarmButton" automation id as the alarm tab selector
                element = session.FindElementByAccessibilityId("AlarmButton");
            }
            catch (InvalidOperationException)
            {
                // The previous version of Alarms & Clock app uses a PivotItem with
                // "AlarmPivotItem" automation id as the alarm tab selector
                element = session.FindElementByAccessibilityId("AlarmPivotItem");
            }
            return element;
        }

        protected static WindowsElement FindAppTitleBar()
        {
            WindowsElement element;
            try
            {
                element = session.FindElementByAccessibilityId("AppName");
            }
            catch (InvalidOperationException)
            {
                element = session.FindElementByAccessibilityId("TitleBar");
            }
            return element;
        }
    }
}
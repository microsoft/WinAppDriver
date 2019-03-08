//******************************************************************************
//
// Copyright (c) 2017 Microsoft Corporation. All rights reserved.
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
using System.Threading;
using System;

namespace AlarmClockTest
{
    [TestClass]
    public class ScenarioAlarm : AlarmClockSession
    {
        private const string NewAlarmName = "Sample Test Alarm";

        [TestMethod]
        public void AlarmAdd()
        {
            // Navigate to New Alarm page
            Thread.Sleep(TimeSpan.FromSeconds(1));
            session.FindElementByAccessibilityId("AddAlarmButton").Click();

            // Set alarm name
            session.FindElementByAccessibilityId("AlarmNameTextBox").Clear();
            session.FindElementByAccessibilityId("AlarmNameTextBox").SendKeys(NewAlarmName);

            // Set alarm hour
            WindowsElement hourSelector = session.FindElementByAccessibilityId("HourLoopingSelector");
            hourSelector.FindElementByName("3").Click();
            Assert.AreEqual("3", hourSelector.Text);

            // Set alarm minute
            WindowsElement minuteSelector = session.FindElementByAccessibilityId("MinuteLoopingSelector");
            minuteSelector.FindElementByName("55").Click();
            Assert.AreEqual("55", minuteSelector.Text);

            // Save the newly configured alarm
            session.FindElementByAccessibilityId("AlarmSaveButton").Click();
            Thread.Sleep(TimeSpan.FromSeconds(3));

            // Verify that a new alarm entry is created with the given hour, minute, and name
            WindowsElement alarmEntry = session.FindElementByXPath($"//ListItem[starts-with(@Name, \"{NewAlarmName}\")]");
            Assert.IsNotNull(alarmEntry);
            Assert.IsTrue(alarmEntry.Text.Contains("3"));
            Assert.IsTrue(alarmEntry.Text.Contains("55"));
            Assert.IsTrue(alarmEntry.Text.Contains(NewAlarmName));

            // Verify that the alarm is active and deactivate it
            WindowsElement alarmEntryToggleSwitch = alarmEntry.FindElementByAccessibilityId("AlarmToggleSwitch") as WindowsElement;
            Assert.IsTrue(alarmEntryToggleSwitch.Selected);
            alarmEntryToggleSwitch.Click();
            Assert.IsFalse(alarmEntryToggleSwitch.Selected);
        }

        [TestMethod]
        public void AlarmDelete()
        {
            WindowsElement alarmEntry = null;
            
            // Check if an alarm entry has been created previously. Otherwise create one by calling AddAlarmEntry();
            try
            {
                alarmEntry = session.FindElementByXPath($"//ListItem[starts-with(@Name, \"{NewAlarmName}\")]");
            }
            catch
            {
                AlarmAdd();
                alarmEntry = session.FindElementByXPath($"//ListItem[starts-with(@Name, \"{NewAlarmName}\")]");
            }

            Assert.IsNotNull(alarmEntry);
            touchScreen.LongPress(alarmEntry.Coordinates);
            session.FindElementByName("Delete").Click();
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            // Try to delete any alarm entry that may have been created
            while (true)
            {
                try
                {
                    var alarmEntry = session.FindElementByXPath($"//ListItem[starts-with(@Name, \"{NewAlarmName}\")]");
                    session.Mouse.ContextClick(alarmEntry.Coordinates);
                    session.FindElementByName("Delete").Click();
                }
                catch
                {
                    break;
                }
            }

            TearDown();
        }

        [TestInitialize]
        public override void TestInit()
        {
            // Invoke base class test initialization to ensure that the app is in the main page
            base.TestInit();

            // Navigate to Alarm tab
            session.FindElementByAccessibilityId("AlarmButton").Click();
        }
    }
}

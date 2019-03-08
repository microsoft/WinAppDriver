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
using System;

namespace WebDriverAPI
{
    [TestClass]
    public class ElementClick : AlarmClockBase
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TearDown();
        }

        [TestMethod]
        public void ClickElement()
        {
            // Open a new alarm page and try clicking on visible and non-visible element in the time picker
            session.FindElementByAccessibilityId("AddAlarmButton").Click();

            // initially visible element
            WindowsElement hourSelector = session.FindElementByAccessibilityId("HourLoopingSelector");
            hourSelector.FindElementByName("8").Click();
            Assert.AreEqual("8", hourSelector.Text);

            // initially non-visible element that is implicitly scrolled into view once clicked
            WindowsElement minuteSelector = session.FindElementByAccessibilityId("MinuteLoopingSelector");
            minuteSelector.FindElementByName("30").Click();
            Assert.AreEqual("30", minuteSelector.Text);

            // Return to main page and click on pivot items to switch between tabs
            DismissAddAlarmPage();
            WindowsElement worldPivot = session.FindElementByAccessibilityId(WorldClockTabAutomationId);
            WindowsElement alarmPivot = session.FindElementByAccessibilityId(AlarmTabAutomationId);

            worldPivot.Click();
            Assert.IsTrue(worldPivot.Selected);
            Assert.IsFalse(alarmPivot.Selected);

            alarmPivot.Click();
            Assert.IsFalse(worldPivot.Selected);
            Assert.IsTrue(alarmPivot.Selected);
        }

        [TestMethod]
        public void ClickElementError_ElementNotVisible()
        {
            // Different Alarm & Clock application version uses different UI elements
            if (AlarmTabClassName == "ListViewItem")
            {
                // The latest Alarms & Clock application destroys the previous view instead of hiding it
            }
            else
            {
                // Navigate to Stopwatch tab and attempt to click on addAlarmButton that is no longer displayed
                WindowsElement addAlarmButton = session.FindElementByAccessibilityId("AddAlarmButton");
                session.FindElementByAccessibilityId(StopwatchTabAutomationId).Click();
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                Assert.IsFalse(addAlarmButton.Displayed);

                try
                {
                    addAlarmButton.Click();
                    Assert.Fail("Exception should have been thrown");
                }
                catch (InvalidOperationException exception)
                {
                    Assert.AreEqual(ErrorStrings.ElementNotVisible, exception.Message);
                }
            }
        }

        [TestMethod]
        public void ClickElementError_NoSuchWindow()
        {
            try
            {
                Utility.GetOrphanedElement().Click();
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }

        [TestMethod]
        public void ClickElementError_StaleElement()
        {
            try
            {
                GetStaleElement().Click();
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.StaleElementReference, exception.Message);
            }
        }
    }
}

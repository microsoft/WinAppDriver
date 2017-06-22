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

namespace W3CWebDriver
{
    [TestClass]
    public class ElementDisplayed : AlarmClockBase
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
        public void ErrorGetElementDisplayedStateNoSuchWindow()
        {
            try
            {
                var displayed = Utility.GetOrphanedElement().Displayed;
                Assert.Fail("Exception should have been thrown");
            }
            catch (System.InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }

        [TestMethod]
        public void ErrorGetElementDisplayedStateStaleElement()
        {
            try
            {
                var displayed = GetStaleElement().Displayed;
                Assert.Fail("Exception should have been thrown");
            }
            catch (System.InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.StaleElementReference, exception.Message);
            }
        }

        [TestMethod]
        public void GetElementDisplayed()
        {
            WindowsElement alarmPivotItem = session.FindElementByAccessibilityId("AlarmPivotItem");
            WindowsElement addAlarmButton = session.FindElementByAccessibilityId("AddAlarmButton");
            Assert.IsTrue(addAlarmButton.Displayed);

            // Navigate to Stopwatch tab
            WindowsElement stopwatchPivotItem = session.FindElementByAccessibilityId("StopwatchPivotItem");
            stopwatchPivotItem.Click();
            WindowsElement stopwatchResetButton = session.FindElementByAccessibilityId("StopWatchResetButton");
            Assert.IsTrue(stopwatchResetButton.Displayed);
            Assert.IsFalse(addAlarmButton.Displayed);

            // Navigate back to Alarm tab
            alarmPivotItem.Click();
            Assert.IsTrue(addAlarmButton.Displayed);
            Assert.IsFalse(stopwatchResetButton.Displayed);

            // Open a new alarm page and verify that 00 minute is displayed while 30 minute is hidden in the time picker
            addAlarmButton.Click();
            WindowsElement minuteLoopingSelector = session.FindElementByAccessibilityId("MinuteLoopingSelector");
            Assert.IsTrue(minuteLoopingSelector.FindElementByName("00").Displayed);
            Assert.IsFalse(minuteLoopingSelector.FindElementByName("30").Displayed);
        }
    }
}

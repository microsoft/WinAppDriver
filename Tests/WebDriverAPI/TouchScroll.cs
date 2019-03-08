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
using System;
using System.Threading;

namespace WebDriverAPI
{
    [TestClass]
    public class TouchScroll : AlarmClockBase
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
        public void TouchScroll_Arbitrary()
        {
            // Different Alarm & Clock application version uses different UI elements
            if (AlarmTabClassName == "ListViewItem")
            {
                // The latest Alarms & Clock application no longer has horizontal scroll UI elements
            }
            else
            {
                var alarmPivotItem = session.FindElementByAccessibilityId(AlarmTabAutomationId);
                var stopwatchPivotItem = session.FindElementByAccessibilityId(StopwatchTabAutomationId);
                Assert.IsNotNull(alarmPivotItem);
                Assert.IsNotNull(stopwatchPivotItem);
                Assert.IsTrue(alarmPivotItem.Selected);
                Assert.IsFalse(stopwatchPivotItem.Selected);

                // Perform scroll right touch action to switch from Alarm tab to Stopwatch tab
                touchScreen.Scroll(100, 0);
                Thread.Sleep(TimeSpan.FromSeconds(1));
                Assert.IsFalse(alarmPivotItem.Selected);
                Assert.IsTrue(stopwatchPivotItem.Selected);

                // Perform scroll left touch action to scroll back from Stopwatch tab to Alarm tab
                touchScreen.Scroll(-100, 0);
                Thread.Sleep(TimeSpan.FromSeconds(1));
                Assert.IsTrue(alarmPivotItem.Selected);
                Assert.IsFalse(stopwatchPivotItem.Selected);
            }
        }

        [TestMethod]
        public void TouchScrollOnElement_Horizontal()
        {
            // Different Alarm & Clock application version uses different UI elements
            if (AlarmTabClassName == "ListViewItem")
            {
                // The latest Alarms & Clock application no longer has horizontal scroll UI elements
            }
            else
            {
                var homePagePivot = session.FindElementByAccessibilityId("HomePagePivot");
                var alarmPivotItem = session.FindElementByAccessibilityId(AlarmTabAutomationId);
                var worldClockPivotItem = session.FindElementByAccessibilityId(WorldClockTabAutomationId);
                Assert.IsNotNull(homePagePivot);
                Assert.IsNotNull(alarmPivotItem);
                Assert.IsNotNull(worldClockPivotItem);
                Assert.IsTrue(alarmPivotItem.Selected);
                Assert.IsFalse(worldClockPivotItem.Selected);

                // Perform scroll left touch action to switch from Alarm tab to WorldClock tab
                touchScreen.Scroll(homePagePivot.Coordinates, -100, 0);
                Thread.Sleep(TimeSpan.FromSeconds(1));
                Assert.IsFalse(alarmPivotItem.Selected);
                Assert.IsTrue(worldClockPivotItem.Selected);

                // Perform scroll right touch action to scroll back from WorldClock tab to Alarm tab
                touchScreen.Scroll(homePagePivot.Coordinates, 100, 0);
                Thread.Sleep(TimeSpan.FromSeconds(1));
                Assert.IsTrue(alarmPivotItem.Selected);
                Assert.IsFalse(worldClockPivotItem.Selected);
            }
        }

        [TestMethod]
        public void TouchScrollOnElement_Vertical()
        {
            // Navigate to add alarm page
            session.FindElementByAccessibilityId("AddAlarmButton").Click();
            session.FindElementByAccessibilityId("AlarmTimePicker").Click();
            Thread.Sleep(TimeSpan.FromSeconds(1));

            var minuteSelector = session.FindElementByAccessibilityId("MinuteLoopingSelector");
            var minute00 = session.FindElementByName("00");
            Assert.IsNotNull(minuteSelector);
            Assert.IsNotNull(minute00);
            Assert.IsTrue(minute00.Displayed);

            // Perform scroll down touch action to scroll the minute hiding 00 minutes that was shown
            touchScreen.Scroll(minuteSelector.Coordinates, 0, -55);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Assert.IsFalse(minute00.Displayed);

            // Perform scroll up touch action to scroll the the minute back showing 00 minutes that was hidden
            touchScreen.Scroll(minuteSelector.Coordinates, 0, 55);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Assert.IsTrue(minute00.Displayed);

            // Dismiss add alarm page
            DismissAddAlarmPage();
        }

        [TestMethod]
        public void TouchScrollOnElementError_StaleElement()
        {
            try
            {
                // Perform double tap touch on stale element
                touchScreen.Scroll(GetStaleElement().Coordinates, 0, 100);
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.StaleElementReference, exception.Message);
            }
        }

    }
}

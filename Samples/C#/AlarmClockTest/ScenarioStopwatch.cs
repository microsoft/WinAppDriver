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
using System.Threading;
using System.Drawing;
using System;

namespace AlarmClockTest
{
    [TestClass]
    public class ScenarioStopwatch : AlarmClockSession
    {
        private static Size originalSize;

        [TestMethod]
        public void StopwatchLap()
        {
            int numberOfEntry = 5;
            var stopwatchPivotItem = session.FindElementByAccessibilityId("StopwatchPivotItem");

            // Start the stopwatch
            var stopwatchPlayPauseButton = stopwatchPivotItem.FindElementByAccessibilityId("StopwatchPlayPauseButton");
            stopwatchPlayPauseButton.Click();

            // Create lap entries
            var stopwatchLapButton = stopwatchPivotItem.FindElementByAccessibilityId("StopWatchLapButton");
            for (uint count = 0; count < numberOfEntry; count++)
            {
                stopwatchLapButton.Click();
            }

            // Pause the stopwatch
            stopwatchPlayPauseButton.Click();
            Thread.Sleep(TimeSpan.FromSeconds(0.5));

            // Verify that the lap entries are created
            var lapListView = stopwatchPivotItem.FindElementByAccessibilityId("LapsAndSplitsListView");
            var lapEntries = lapListView.FindElementsByClassName("ListViewItem");
            Assert.IsNotNull(lapEntries);
            Assert.AreEqual(numberOfEntry, lapEntries.Count);

            // Verify that the fist lap entry is now hidden after there are at least 3 entries in a small window
            var firstLapEntry = lapEntries[numberOfEntry - 1];
            var lastLapEntry = lapEntries[0];
            Assert.IsTrue(lastLapEntry.Displayed);
            Assert.IsFalse(firstLapEntry.Displayed);

            // Horizontally scroll the list up and verify that the fist lap entry is now displayed while the last entry is now hidden
            touchScreen.Scroll(lapListView.Coordinates, 0, -150);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Assert.IsTrue(firstLapEntry.Displayed);
            Assert.IsFalse(lastLapEntry.Displayed);

            // Horizontally scroll the list down and verify that the last lap entry is now displayed while the first entry is now hidden again
            touchScreen.Scroll(lapListView.Coordinates, 0, 150);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Assert.IsTrue(lastLapEntry.Displayed);
            Assert.IsFalse(firstLapEntry.Displayed);
        }

        [TestMethod]
        public void StopwatchStart()
        {
            var stopwatchPivotItem = session.FindElementByAccessibilityId("StopwatchPivotItem");
            var stopwatchResetButton = stopwatchPivotItem.FindElementByAccessibilityId("StopWatchResetButton");

            // Track the reset stopwatch timer text for comparison
            var stopwatchTimer = stopwatchPivotItem.FindElementByAccessibilityId("StopwatchTimerText");
            string stopwatchTimerText = stopwatchTimer.GetAttribute("Name");

            // Verify that the stopwatchPlayPauseButton button says Start and the stopwatchResetButton is disabled
            var stopwatchPlayPauseButton = stopwatchPivotItem.FindElementByAccessibilityId("StopwatchPlayPauseButton");
            Assert.AreEqual("Start", stopwatchPlayPauseButton.Text);
            Assert.IsFalse(stopwatchResetButton.Enabled);

            // Start the stopwatch and verify that stopwatchPlayPauseButton changed from Start to Pause while stopwatchResetButton is hidden
            stopwatchPlayPauseButton.Click();
            Assert.AreEqual("Pause", stopwatchPlayPauseButton.Text);
            Assert.IsFalse(stopwatchResetButton.Displayed);
            Thread.Sleep(TimeSpan.FromSeconds(1));

            // Pause the stopwatch and verify that stopwatchPlayPauseButton switched back from Pause to Start while stopwatchResetButton is enabled
            stopwatchPlayPauseButton.Click();
            Assert.AreEqual("Start", stopwatchPlayPauseButton.Text);
            Assert.IsTrue(stopwatchResetButton.Displayed);
            Assert.IsTrue(stopwatchResetButton.Enabled);

            // Verify that the timer text has been changed
            Assert.AreNotEqual(stopwatchTimerText, stopwatchTimer.GetAttribute("Name"));
        }


        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);

            // Save application window original size and temporarily set it to 500 x 500
            originalSize = session.Manage().Window.Size;
            Assert.IsNotNull(originalSize);
            session.Manage().Window.Size = new Size(500, 500);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            // Restore application window original size and position
            session.Manage().Window.Size = originalSize;

            TearDown();
        }

        [TestInitialize]
        public override void TestInit()
        {
            // Invoke base class test initialization to ensure that the app is in the main page
            base.TestInit();

            // Navigate to Stopwatch tab
            session.FindElementByAccessibilityId("StopwatchPivotItem").Click();

            // Stop the stopwatch if it is running and reset it
            TestCleanup();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Stop the stopwatch if it is running
            try
            {
                session.FindElementByName("Pause").Click();
            }
            catch { }

            // Reset stopwatch to remove all lap entries that may have been created
            session.FindElementByAccessibilityId("StopWatchResetButton").Click();
        }
    }
}

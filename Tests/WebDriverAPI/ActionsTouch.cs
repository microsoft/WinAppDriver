//******************************************************************************
//
// Copyright (c) 2018 Microsoft Corporation. All rights reserved.
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
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Appium.Interactions;
using OpenQA.Selenium.Appium.Windows;

// Define an alias to OpenQA.Selenium.Appium.Interactions.PointerInputDevice to hide
// inherited OpenQA.Selenium.Interactions.PointerInputDevice that causes ambiguity.
// In the future, all functions of OpenQA.Selenium.Appium.Interactions should be moved
// up to OpenQA.Selenium.Interactions and this alias can simply be removed.
using PointerInputDevice = OpenQA.Selenium.Appium.Interactions.PointerInputDevice;

namespace WebDriverAPI
{
    [TestClass]
    public class ActionsTouch : AlarmClockBase
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
        public void Touch_Click_OriginElement()
        {
            var alarmPivotItem = session.FindElementByAccessibilityId(AlarmTabAutomationId);
            var worldClockPivotItem = session.FindElementByAccessibilityId(WorldClockTabAutomationId);
            Assert.IsNotNull(alarmPivotItem);
            Assert.IsNotNull(worldClockPivotItem);
            Assert.IsTrue(alarmPivotItem.Selected);
            Assert.IsFalse(worldClockPivotItem.Selected);

            // Perform touch click action using element coordinate origin to switch from Alarm to WorldClock tab
            PointerInputDevice touchDevice = new PointerInputDevice(PointerKind.Touch);
            ActionSequence sequence = new ActionSequence(touchDevice, 0);
            sequence.AddAction(touchDevice.CreatePointerMove(worldClockPivotItem, 0, 0, TimeSpan.Zero));
            sequence.AddAction(touchDevice.CreatePointerDown(PointerButton.TouchContact));
            sequence.AddAction(touchDevice.CreatePointerUp(PointerButton.TouchContact));
            session.PerformActions(new List<ActionSequence> { sequence });

            Thread.Sleep(TimeSpan.FromSeconds(1));
            Assert.IsFalse(alarmPivotItem.Selected);
            Assert.IsTrue(worldClockPivotItem.Selected);

            // Perform touch click action using element coordinate origin to switch from WorldClock to Alarm tab
            sequence = new ActionSequence(touchDevice, 0);
            sequence.AddAction(touchDevice.CreatePointerMove(alarmPivotItem, 0, 0, TimeSpan.Zero));
            sequence.AddAction(touchDevice.CreatePointerDown(PointerButton.TouchContact));
            sequence.AddAction(touchDevice.CreatePointerUp(PointerButton.TouchContact));
            session.PerformActions(new List<ActionSequence> { sequence });

            Thread.Sleep(TimeSpan.FromSeconds(1));
            Assert.IsTrue(alarmPivotItem.Selected);
            Assert.IsFalse(worldClockPivotItem.Selected);
        }

        [TestMethod]
        public void Touch_Click_OriginPointer()
        {
            WindowsElement alarmPivotItem = session.FindElementByAccessibilityId(AlarmTabAutomationId);
            WindowsElement worldClockPivotItem = session.FindElementByAccessibilityId(WorldClockTabAutomationId);
            int relativeX = 0; // Initial x coordinate
            int relativeY = 0; // Initial y coordinate
            Assert.IsNotNull(alarmPivotItem);
            Assert.IsNotNull(worldClockPivotItem);
            Assert.IsTrue(alarmPivotItem.Selected);
            Assert.IsFalse(worldClockPivotItem.Selected);

            // Perform touch click action using pointer coordinate origin to switch from Alarm to WorldClock tab
            PointerInputDevice touchDevice = new PointerInputDevice(PointerKind.Touch);
            ActionSequence sequence = new ActionSequence(touchDevice, 0);
            relativeX = worldClockPivotItem.Location.X - relativeX;
            relativeY = worldClockPivotItem.Location.Y - relativeY;
            sequence.AddAction(touchDevice.CreatePointerMove(CoordinateOrigin.Pointer, relativeX, relativeY, TimeSpan.Zero));
            sequence.AddAction(touchDevice.CreatePointerDown(PointerButton.TouchContact));
            sequence.AddAction(touchDevice.CreatePointerUp(PointerButton.TouchContact));
            session.PerformActions(new List<ActionSequence> { sequence });

            Thread.Sleep(TimeSpan.FromSeconds(1));
            Assert.IsFalse(alarmPivotItem.Selected);
            Assert.IsTrue(worldClockPivotItem.Selected);

            // Perform touch click action using pointer coordinate origin to switch from WorldClock to Alarm tab
            sequence = new ActionSequence(touchDevice, 0);
            relativeX = alarmPivotItem.Location.X - relativeX;
            relativeY = alarmPivotItem.Location.Y - relativeY;
            sequence.AddAction(touchDevice.CreatePointerMove(CoordinateOrigin.Pointer, relativeX, relativeY, TimeSpan.Zero));
            sequence.AddAction(touchDevice.CreatePointerDown(PointerButton.TouchContact));
            sequence.AddAction(touchDevice.CreatePointerUp(PointerButton.TouchContact));
            session.PerformActions(new List<ActionSequence> { sequence });

            Thread.Sleep(TimeSpan.FromSeconds(1));
            Assert.IsTrue(alarmPivotItem.Selected);
            Assert.IsFalse(worldClockPivotItem.Selected);
        }

        [TestMethod]
        public void Touch_Click_OriginViewport()
        {
            WindowsElement alarmPivotItem = session.FindElementByAccessibilityId(AlarmTabAutomationId);
            WindowsElement worldClockPivotItem = session.FindElementByAccessibilityId(WorldClockTabAutomationId);
            int x = worldClockPivotItem.Location.X; // x coordinate of UI element relative to application window
            int y = worldClockPivotItem.Location.Y; // y coordinate of UI element relative to application window
            Assert.IsNotNull(alarmPivotItem);
            Assert.IsNotNull(worldClockPivotItem);
            Assert.IsTrue(alarmPivotItem.Selected);
            Assert.IsFalse(worldClockPivotItem.Selected);

            // Perform touch click action using viewport coordinate origin to switch from Alarm to WorldClock tab
            PointerInputDevice touchDevice = new PointerInputDevice(PointerKind.Touch);
            ActionSequence sequence = new ActionSequence(touchDevice, 0);
            sequence.AddAction(touchDevice.CreatePointerMove(CoordinateOrigin.Viewport, x, y, TimeSpan.Zero));
            sequence.AddAction(touchDevice.CreatePointerDown(PointerButton.TouchContact));
            sequence.AddAction(touchDevice.CreatePointerUp(PointerButton.TouchContact));
            session.PerformActions(new List<ActionSequence> { sequence });

            Thread.Sleep(TimeSpan.FromSeconds(1));
            Assert.IsFalse(alarmPivotItem.Selected);
            Assert.IsTrue(worldClockPivotItem.Selected);

            // Perform touch click action using viewport coordinate origin to switch from WorldClock to Alarm tab
            sequence = new ActionSequence(touchDevice, 0);
            x = alarmPivotItem.Location.X;
            y = alarmPivotItem.Location.Y;
            sequence.AddAction(touchDevice.CreatePointerMove(CoordinateOrigin.Viewport, x, y, TimeSpan.Zero));
            sequence.AddAction(touchDevice.CreatePointerDown(PointerButton.TouchContact));
            sequence.AddAction(touchDevice.CreatePointerUp(PointerButton.TouchContact));
            session.PerformActions(new List<ActionSequence> { sequence });

            Thread.Sleep(TimeSpan.FromSeconds(1));
            Assert.IsTrue(alarmPivotItem.Selected);
            Assert.IsFalse(worldClockPivotItem.Selected);
        }

        [TestMethod]
        public void Touch_DoubleClick()
        {
            WindowsElement appNameTitle = FindAppTitleBar();
            WindowsElement maximizeButton = session.FindElementByAccessibilityId("Maximize");

            // Set focus on the application by switching window to itself
            session.SwitchTo().Window(session.CurrentWindowHandle);

            // Restore the application window if it is currently maximized
            if (!maximizeButton.Text.Contains("Maximize"))
            {
                maximizeButton.Click();
            }

            // Verify that window is currently not maximized
            Assert.IsTrue(maximizeButton.Text.Contains("Maximize"));

            // Perform touch double click action on the title bar to maximize the application window
            PointerInputDevice touchDevice = new PointerInputDevice(PointerKind.Touch);
            ActionSequence sequence = new ActionSequence(touchDevice, 0);
            sequence.AddAction(touchDevice.CreatePointerMove(appNameTitle, 0, 0, TimeSpan.Zero));
            sequence.AddAction(touchDevice.CreatePointerDown(PointerButton.TouchContact));
            sequence.AddAction(touchDevice.CreatePointerUp(PointerButton.TouchContact));
            sequence.AddAction(touchDevice.CreatePointerDown(PointerButton.TouchContact));
            sequence.AddAction(touchDevice.CreatePointerUp(PointerButton.TouchContact));
            session.PerformActions(new List<ActionSequence> { sequence });
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Assert.IsFalse(maximizeButton.Text.Contains("Maximize"));

            // Perform touch double click action on the title bar to restore the application window
            sequence = new ActionSequence(touchDevice, 0);
            sequence.AddAction(touchDevice.CreatePointerMove(appNameTitle, 0, 0, TimeSpan.Zero));
            sequence.AddAction(touchDevice.CreatePointerDown(PointerButton.TouchContact));
            sequence.AddAction(touchDevice.CreatePointerUp(PointerButton.TouchContact));
            sequence.AddAction(touchDevice.CreatePointerDown(PointerButton.TouchContact));
            sequence.AddAction(touchDevice.CreatePointerUp(PointerButton.TouchContact));
            session.PerformActions(new List<ActionSequence> { sequence });
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Assert.IsTrue(maximizeButton.Text.Contains("Maximize"));
        }

        [TestMethod]
        public void Touch_DragAndDrop()
        {
            WindowsElement appNameTitle = FindAppTitleBar();
            const int offset = 100;

            // Save application window original position
            Point originalPosition = session.Manage().Window.Position;
            Assert.IsNotNull(originalPosition);

            // Send touch down, move, and up actions combination to perform a drag and drop 
            // action on the app title bar. These actions reposition the application window.
            PointerInputDevice touchDevice = new PointerInputDevice(PointerKind.Touch);
            ActionSequence sequence = new ActionSequence(touchDevice, 0);
            sequence.AddAction(touchDevice.CreatePointerMove(appNameTitle, 0, 0, TimeSpan.Zero));
            sequence.AddAction(touchDevice.CreatePointerDown(PointerButton.TouchContact));
            sequence.AddAction(touchDevice.CreatePointerMove(CoordinateOrigin.Pointer, offset, offset, TimeSpan.FromSeconds(1)));
            sequence.AddAction(touchDevice.CreatePointerUp(PointerButton.TouchContact));
            session.PerformActions(new List<ActionSequence> { sequence });
            Thread.Sleep(TimeSpan.FromSeconds(1));

            // Verify that application window is now re-positioned from the original location
            Assert.AreNotEqual(originalPosition, session.Manage().Window.Position);
            Assert.IsTrue(originalPosition.Y < session.Manage().Window.Position.Y);

            // Restore application window original position
            session.Manage().Window.Position = originalPosition;
            Assert.AreEqual(originalPosition, session.Manage().Window.Position);
        }

        [TestMethod]
        public void Touch_Flick()
        {
            // Navigate to add alarm page
            session.FindElementByAccessibilityId("AddAlarmButton").Click();
            session.FindElementByAccessibilityId("AlarmTimePicker").Click();
            Thread.Sleep(TimeSpan.FromSeconds(1));

            WindowsElement minuteSelector = session.FindElementByAccessibilityId("MinuteLoopingSelector");
            WindowsElement minute00 = session.FindElementByName("00");
            Assert.IsNotNull(minuteSelector);
            Assert.IsNotNull(minute00);
            Assert.IsTrue(minute00.Displayed);

            // Perform touch flick down action to scroll the minute hiding 00 minutes that was shown
            PointerInputDevice touchDevice = new PointerInputDevice(PointerKind.Touch);
            ActionSequence sequence = new ActionSequence(touchDevice, 0);
            sequence.AddAction(touchDevice.CreatePointerMove(minuteSelector, 0, 0, TimeSpan.Zero));
            sequence.AddAction(touchDevice.CreatePointerDown(PointerButton.TouchContact));
            sequence.AddAction(touchDevice.CreatePointerMove(minuteSelector, 0, 500, TimeSpan.Zero));
            sequence.AddAction(touchDevice.CreatePointerUp(PointerButton.TouchContact));
            session.PerformActions(new List<ActionSequence> { sequence });
            Thread.Sleep(TimeSpan.FromSeconds(2));
            Assert.IsFalse(minute00.Displayed);
        }

        [TestMethod]
        public void Touch_LongClick()
        {
            // Create a new test alarm
            string alarmName = "LongTapTest";
            DeletePreviouslyCreatedAlarmEntry(alarmName);
            AddAlarmEntry(alarmName);
            Thread.Sleep(TimeSpan.FromSeconds(3));

            var alarmEntries = session.FindElementsByXPath($"//ListItem[starts-with(@Name, \"{alarmName}\")]");
            Assert.IsNotNull(alarmEntries);
            Assert.AreEqual(1, alarmEntries.Count);

            // Open a the context menu on the alarm entry using long tap (press and hold) action and click delete
            PointerInputDevice touchDevice = new PointerInputDevice(PointerKind.Touch);
            ActionSequence sequence = new ActionSequence(touchDevice, 0);
            sequence.AddAction(touchDevice.CreatePointerMove(alarmEntries[0], 0, 0, TimeSpan.Zero));
            sequence.AddAction(touchDevice.CreatePointerDown(PointerButton.TouchContact));
            sequence.AddAction(touchDevice.CreatePointerMove(alarmEntries[0], 0, 0, TimeSpan.FromSeconds(3)));
            sequence.AddAction(touchDevice.CreatePointerUp(PointerButton.TouchContact));
            session.PerformActions(new List<ActionSequence> { sequence });

            Thread.Sleep(TimeSpan.FromSeconds(1));
            session.FindElementByName("Delete").Click();
            Thread.Sleep(TimeSpan.FromSeconds(1));

            alarmEntries = session.FindElementsByXPath($"//ListItem[starts-with(@Name, \"{alarmName}\")]");
            Assert.IsNotNull(alarmEntries);
            Assert.AreEqual(0, alarmEntries.Count);
        }

        [TestMethod]
        public void Touch_Scroll_Horizontal()
        {
            // Different Alarm & Clock application version uses different UI elements
            if (AlarmTabClassName == "ListViewItem")
            {
                // The latest Alarms & Clock application no longer has horizontal scroll UI elements
            }
            else
            {
                WindowsElement homePagePivot = session.FindElementByAccessibilityId("HomePagePivot");
                WindowsElement alarmPivotItem = session.FindElementByAccessibilityId(AlarmTabAutomationId);
                WindowsElement worldClockPivotItem = session.FindElementByAccessibilityId(WorldClockTabAutomationId);
                Assert.IsNotNull(homePagePivot);
                Assert.IsNotNull(alarmPivotItem);
                Assert.IsNotNull(worldClockPivotItem);
                Assert.IsTrue(alarmPivotItem.Selected);
                Assert.IsFalse(worldClockPivotItem.Selected);

                // Perform scroll left touch action to switch from Alarm to WorldClock tab
                PointerInputDevice touchDevice = new PointerInputDevice(PointerKind.Touch);
                ActionSequence sequence = new ActionSequence(touchDevice, 0);
                sequence.AddAction(touchDevice.CreatePointerMove(homePagePivot, 0, 0, TimeSpan.Zero));
                sequence.AddAction(touchDevice.CreatePointerDown(PointerButton.TouchContact));
                sequence.AddAction(touchDevice.CreatePointerMove(homePagePivot, -session.Manage().Window.Size.Width / 2, 0, TimeSpan.FromSeconds(.5)));
                sequence.AddAction(touchDevice.CreatePointerUp(PointerButton.TouchContact));
                session.PerformActions(new List<ActionSequence> { sequence });

                Thread.Sleep(TimeSpan.FromSeconds(1));
                Assert.IsFalse(alarmPivotItem.Selected);
                Assert.IsTrue(worldClockPivotItem.Selected);

                // Perform scroll right touch action to switch back from WorldClock to Alarm tab
                sequence = new ActionSequence(touchDevice, 0);
                sequence.AddAction(touchDevice.CreatePointerMove(homePagePivot, 0, 0, TimeSpan.Zero));
                sequence.AddAction(touchDevice.CreatePointerDown(PointerButton.TouchContact));
                sequence.AddAction(touchDevice.CreatePointerMove(homePagePivot, session.Manage().Window.Size.Width / 2, 0, TimeSpan.FromSeconds(.5)));
                sequence.AddAction(touchDevice.CreatePointerUp(PointerButton.TouchContact));
                session.PerformActions(new List<ActionSequence> { sequence });

                Thread.Sleep(TimeSpan.FromSeconds(1));
                Assert.IsTrue(alarmPivotItem.Selected);
                Assert.IsFalse(worldClockPivotItem.Selected);
            }
        }

        [TestMethod]
        public void Touch_Scroll_Vertical()
        {
            // Navigate to add alarm page
            session.FindElementByAccessibilityId("AddAlarmButton").Click();
            session.FindElementByAccessibilityId("AlarmTimePicker").Click();
            Thread.Sleep(TimeSpan.FromSeconds(1));

            WindowsElement minuteSelector = session.FindElementByAccessibilityId("MinuteLoopingSelector");
            WindowsElement minute00 = session.FindElementByName("00");
            Assert.IsNotNull(minuteSelector);
            Assert.IsNotNull(minute00);
            Assert.IsTrue(minute00.Displayed);

            // Perform scroll down touch action to scroll the minute hiding 00 minutes that was shown
            PointerInputDevice touchDevice = new PointerInputDevice(PointerKind.Touch);
            ActionSequence sequence = new ActionSequence(touchDevice, 0);
            sequence.AddAction(touchDevice.CreatePointerMove(minuteSelector, 0, 0, TimeSpan.Zero));
            sequence.AddAction(touchDevice.CreatePointerDown(PointerButton.TouchContact));
            sequence.AddAction(touchDevice.CreatePointerMove(minuteSelector, 0, -200, TimeSpan.FromSeconds(.5)));
            sequence.AddAction(touchDevice.CreatePointerUp(PointerButton.TouchContact));
            session.PerformActions(new List<ActionSequence> { sequence });
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Assert.IsFalse(minute00.Displayed);

            // Perform scroll up touch action to scroll the the minute back showing 00 minutes that was hidden
            sequence = new ActionSequence(touchDevice, 0);
            sequence.AddAction(touchDevice.CreatePointerMove(minuteSelector, 0, 0, TimeSpan.Zero));
            sequence.AddAction(touchDevice.CreatePointerDown(PointerButton.TouchContact));
            sequence.AddAction(touchDevice.CreatePointerMove(minuteSelector, 0, 200, TimeSpan.FromSeconds(.5)));
            sequence.AddAction(touchDevice.CreatePointerUp(PointerButton.TouchContact));
            session.PerformActions(new List<ActionSequence> { sequence });
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Assert.IsTrue(minute00.Displayed);
        }
    }
}

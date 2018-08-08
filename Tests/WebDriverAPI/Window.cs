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
using System.Collections.Generic;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;
using System.Drawing;
using OpenQA.Selenium;

namespace WebDriverAPI
{
    [TestClass]
    public class Window
    {
        private WindowsDriver<WindowsElement> session = null;

        [TestCleanup]
        public void TestCleanup()
        {
            if (session != null)
            {
                session.Quit();
                session = null;
            }
        }

        [TestMethod]
        public void CloseWindow()
        {
            session = Utility.CreateNewSession(CommonTestSettings.AlarmClockAppId);
            Assert.IsNotNull(session.SessionId);

            // Close the application window without deleting the session
            session.Close();
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);

            // Delete the session
            session.Quit();
            session = null;
        }

        [TestMethod]
        public void CloseWindowError_NoSuchWindow()
        {
            // Attempt to close the previously closed application window
            try
            {
                Utility.GetOrphanedSession().Close();
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }

        [TestMethod]
        public void GetWindowHandle()
        {
            session = Utility.CreateNewSession(CommonTestSettings.CalculatorAppId);
            Assert.IsNotNull(session.SessionId);

            string windowHandle = session.CurrentWindowHandle;
            Assert.IsNotNull(windowHandle);
        }

        [TestMethod]
        public void GetWindowHandleError_NoSuchWindow()
        {
            try
            {
                string windowHandle = Utility.GetOrphanedSession().CurrentWindowHandle;
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }

        [TestMethod]
        public void GetWindowHandles_ClassicApp()
        {
            session = Utility.CreateNewSession(CommonTestSettings.NotepadAppId);
            Assert.IsNotNull(session);

            var handles = session.WindowHandles;
            Assert.IsNotNull(handles);
            Assert.IsTrue(handles.Count > 0);
        }

        [TestMethod]
        public void GetWindowHandles_Desktop()
        {
            session = Utility.CreateNewSession(CommonTestSettings.DesktopAppId);
            Assert.IsNotNull(session);

            var handles = session.WindowHandles;
            Assert.IsNotNull(handles);
            Assert.AreEqual(0, handles.Count);
        }

        [TestMethod]
        public void GetWindowHandles_ModernApp()
        {
            session = Utility.CreateNewSession(CommonTestSettings.EdgeAppId, "-private");
            Assert.IsNotNull(session);

            var windowHandlesBefore = session.WindowHandles;
            Assert.IsNotNull(windowHandlesBefore);
            Assert.IsTrue(windowHandlesBefore.Count > 0);

            // Preserve previously opened Edge window(s) and only manipulate newly opened windows
            List<string> previouslyOpenedEdgeWindows = new List<string>(windowHandlesBefore);
            previouslyOpenedEdgeWindows.Remove(session.CurrentWindowHandle);

            // Set focus on itself
            session.SwitchTo().Window(session.CurrentWindowHandle);

            // Open a new window in private mode
            session.Keyboard.SendKeys(Keys.Control + Keys.Shift + "p" + Keys.Shift + Keys.Control);
            Thread.Sleep(TimeSpan.FromSeconds(3));
            var windowHandlesAfter = session.WindowHandles;
            Assert.IsNotNull(windowHandlesAfter);
            Assert.AreEqual(windowHandlesBefore.Count + 1, windowHandlesAfter.Count);

            // Preserve previously opened Edge Windows by only closing newly opened windows
            List<string> newlyOpenedEdgeWindows = new List<string>(windowHandlesAfter);
            foreach (var previouslyOpenedEdgeWindow in previouslyOpenedEdgeWindows)
            {
                newlyOpenedEdgeWindows.Remove(previouslyOpenedEdgeWindow);
            }

            foreach (var windowHandle in newlyOpenedEdgeWindows)
            {
                session.SwitchTo().Window(windowHandle);
                EdgeBase.CloseEdge(session);
            }
        }

        [TestMethod]
        public void SwitchWindows()
        {
            session = Utility.CreateNewSession(CommonTestSettings.EdgeAppId, CommonTestSettings.EdgeAboutBlankURL);
            Assert.IsNotNull(session);

            // Preserve previously opened Edge window(s) and only manipulate newly opened windows
            List<string> previouslyOpenedEdgeWindows = new List<string>(session.WindowHandles);
            previouslyOpenedEdgeWindows.Remove(session.CurrentWindowHandle);

            // Set focus on itself
            session.SwitchTo().Window(session.CurrentWindowHandle);

            // Open a new window in private mode
            session.Keyboard.SendKeys(Keys.Control + Keys.Shift + "p" + Keys.Shift + Keys.Control);
            Thread.Sleep(TimeSpan.FromSeconds(3));
            var multipleWindowHandles = session.WindowHandles;
            Assert.IsTrue(multipleWindowHandles.Count > 1);

            // Preserve previously opened Edge Windows by only operating on newly opened windows
            List<string> newlyOpenedEdgeWindows = new List<string>(multipleWindowHandles);
            foreach (var previouslyOpenedEdgeWindow in previouslyOpenedEdgeWindows)
            {
                newlyOpenedEdgeWindows.Remove(previouslyOpenedEdgeWindow);
            }

            string previousWindowHandle = string.Empty;

            foreach (var windowHandle in newlyOpenedEdgeWindows)
            {
                session.SwitchTo().Window(windowHandle);
                Assert.AreEqual(session.CurrentWindowHandle, windowHandle);
                Assert.AreNotEqual(windowHandle, previousWindowHandle);
                previousWindowHandle = windowHandle;
                EdgeBase.CloseEdge(session);
            }
        }

        [TestMethod]
        public void SwitchWindowsError_EmptyValue()
        {
            session = Utility.CreateNewSession(CommonTestSettings.CalculatorAppId);

            try
            {
                session.SwitchTo().Window(string.Empty);
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual("Missing Command Parameter: name", exception.Message);
            }
        }

        [TestMethod]
        public void SwitchWindowsError_ForeignWindowHandle()
        {
            WindowsDriver<WindowsElement> mainSession = null;
            WindowsDriver<WindowsElement> foreignSession = null;

            try
            {
                mainSession = Utility.CreateNewSession(CommonTestSettings.CalculatorAppId);
                foreignSession = Utility.CreateNewSession(CommonTestSettings.AlarmClockAppId);
                Assert.IsNotNull(mainSession.SessionId);
                Assert.IsNotNull(foreignSession.SessionId);

                // Get a foreign window handle from a different application/process under foreignSession
                var foreignTopLevelWindow = foreignSession.CurrentWindowHandle;
                Assert.IsFalse(string.IsNullOrEmpty(foreignTopLevelWindow));

                mainSession.SwitchTo().Window(foreignTopLevelWindow);
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual("Window handle does not belong to the same process/application", exception.Message);
            }
            finally
            {
                if (foreignSession != null)
                {
                    foreignSession.Quit();
                }

                if (foreignSession != null)
                {
                    mainSession.Quit();
                }
            }
        }

        [TestMethod]
        public void SwitchWindowsError_InvalidValue()
        {
            session = Utility.CreateNewSession(CommonTestSettings.CalculatorAppId);

            try
            {
                session.SwitchTo().Window("-1");
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual("String cannot contain a minus sign if the base is not 10.", exception.Message);
            }
        }

        [TestMethod]
        public void SwitchWindowsError_NonTopLevelWindowHandle()
        {
            session = Utility.CreateNewSession(CommonTestSettings.CalculatorAppId);
            var nonTopLevelWindowHandle = session.FindElementByClassName("Windows.UI.Core.CoreWindow").GetAttribute("NativeWindowHandle");
            var nonTopLevelWindowHandleHex = Convert.ToInt32(nonTopLevelWindowHandle).ToString("x");

            try
            {
                session.SwitchTo().Window(nonTopLevelWindowHandleHex); // This needs to be in Hex e.g. 0x00880088
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.IsTrue(exception.Message.EndsWith("is not a top level window handle"));
            }
        }

        [TestMethod]
        public void SwitchWindowsError_NoSuchWindow()
        {
            session = Utility.CreateNewSession(CommonTestSettings.CalculatorAppId);

            // Get an orphaned window handle from a closed application
            var orphanedTopLevelWindow = Utility.GetOrphanedWindowHandle();
            Thread.Sleep(TimeSpan.FromSeconds(3));

            try
            {
                session.SwitchTo().Window(orphanedTopLevelWindow);
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual("A request to switch to a window could not be satisfied because the window could not be found.", exception.Message);
            }
        }
    }

    [TestClass]
    public class WindowTransform
    {
        protected static WindowsDriver<WindowsElement> WindowTransformSession;
        protected static Size OriginalSize;
        protected static Point OriginalPosition;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            // Launch the Calculator app
            WindowTransformSession = Utility.CreateNewSession(CommonTestSettings.CalculatorAppId);
            Assert.IsNotNull(WindowTransformSession);
        }

        [ClassCleanup]
        public static void TearDown()
        {
            // Close the application and delete the session
            WindowTransformSession.Quit();
            WindowTransformSession = null;
        }

        [TestInitialize]
        public void TestInitialize()
        {
            // Save application window original size and position
            OriginalSize = WindowTransformSession.Manage().Window.Size;
            Assert.IsNotNull(OriginalSize);
            OriginalPosition = WindowTransformSession.Manage().Window.Position;
            Assert.IsNotNull(OriginalPosition);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            // Restore application window original size and position
            WindowTransformSession.Manage().Window.Size = OriginalSize;
            Assert.AreEqual(OriginalSize, WindowTransformSession.Manage().Window.Size);
            WindowTransformSession.Manage().Window.Position = OriginalPosition;
            Assert.AreEqual(OriginalPosition, WindowTransformSession.Manage().Window.Position);
        }

        [TestMethod]
        public void GetWindowPosition()
        {
            var windowPosition = WindowTransformSession.Manage().Window.Position;
            Assert.IsNotNull(windowPosition);
            Assert.AreEqual(OriginalPosition.X, windowPosition.X);
            Assert.AreEqual(OriginalPosition.Y, windowPosition.Y);
        }

        [TestMethod]
        public void GetWindowPositionError_NoSuchWindow()
        {
            try
            {
                var windowPosition = Utility.GetOrphanedSession().Manage().Window.Position;
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }

        [TestMethod]
        public void GetWindowSize()
        {
            var windowSize = WindowTransformSession.Manage().Window.Size;
            Assert.IsNotNull(windowSize);
            Assert.AreEqual(OriginalSize.Height, windowSize.Height);
            Assert.AreEqual(OriginalSize.Width, windowSize.Width);
        }

        [TestMethod]
        public void GetWindowSizeError_NoSuchWindow()
        {
            try
            {
                var windowSize = Utility.GetOrphanedSession().Manage().Window.Size;
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }

        [TestMethod]
        public void MaximizeWindow()
        {
            WindowTransformSession.Manage().Window.Maximize();
            var windowSize = WindowTransformSession.Manage().Window.Size;
            Assert.IsNotNull(windowSize);
            Assert.IsTrue(OriginalSize.Height <= windowSize.Height);
            Assert.IsTrue(OriginalSize.Width <= windowSize.Width);
            Assert.IsTrue(WindowTransformSession.FindElementByAccessibilityId("Maximize").Text.Contains("Restore"));
        }

        [TestMethod]
        public void MaximizeWindowError_NoSuchWindow()
        {
            try
            {
                Utility.GetOrphanedSession().Manage().Window.Maximize();
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }

        [TestMethod]
        public void SetWindowPosition()
        {
            int offset = 100;
            WindowTransformSession.Manage().Window.Position = new Point(OriginalPosition.X + offset, OriginalPosition.Y + offset);
            var windowPosition = WindowTransformSession.Manage().Window.Position;
            Assert.IsNotNull(windowPosition);
            Assert.AreEqual(OriginalPosition.X + offset, windowPosition.X);
            Assert.AreEqual(OriginalPosition.Y + offset, windowPosition.Y);
        }

        [TestMethod]
        public void SetWindowPosition_ToOrigin()
        {
            var origin = new Point(0, 0);
            WindowTransformSession.Manage().Window.Position = origin;
            var position = WindowTransformSession.Manage().Window.Position;
            Assert.AreEqual(origin.X, position.X);
            Assert.AreEqual(origin.Y, position.Y);
        }

        [TestMethod]
        public void SetWindowPositionError_NoSuchWindow()
        {
            try
            {
                Utility.GetOrphanedSession().Manage().Window.Position = new Point(0, 0);
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }

        [TestMethod]
        public void SetWindowSize()
        {
            // The calculator app has a minimum width and height. Setting below the minimum values will resize it to the
            // minimum size instead of the new width and height. The values below are chosen to exceed this minimum size.
            int offset = 100;
            int newWidth = 350;
            int newHeight = 550;

            WindowTransformSession.Manage().Window.Size = new Size(newWidth, newHeight);
            var windowSize = WindowTransformSession.Manage().Window.Size;
            Assert.AreEqual(newWidth, windowSize.Width);
            Assert.AreEqual(newHeight, windowSize.Height);

            WindowTransformSession.Manage().Window.Size = new Size(newWidth + offset, newHeight + offset);
            windowSize = WindowTransformSession.Manage().Window.Size;
            Assert.AreEqual(newWidth + offset, windowSize.Width);
            Assert.AreEqual(newHeight + offset, windowSize.Height);
        }

        [TestMethod]
        public void SetWindowSizeError_NoSuchWindow()
        {
            try
            {
                Utility.GetOrphanedSession().Manage().Window.Size = new Size(1000, 1000);
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }
    }
}
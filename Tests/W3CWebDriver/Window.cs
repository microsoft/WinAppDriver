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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Remote;

namespace W3CWebDriver
{
    [TestClass]
    public class Window
    {
        [TestMethod]
        public void CloseWindowSingleInstanceApplication()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.AlarmClockAppId);
            IOSDriver<IOSElement> singleWindowSession = new IOSDriver<IOSElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(singleWindowSession);
            Assert.IsNotNull(singleWindowSession.SessionId);

            // Close the application window without deleting the session
            singleWindowSession.Close();
            Assert.IsNotNull(singleWindowSession);
            Assert.IsNotNull(singleWindowSession.SessionId);

            // Delete the session
            singleWindowSession.Quit();
        }

        [TestMethod]
        public void GetWindowHandle()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.CalculatorAppId);
            IOSDriver<IOSElement> session = new IOSDriver<IOSElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);

            string windowHandle = session.CurrentWindowHandle;
            Assert.IsNotNull(windowHandle);
            session.Quit();
            session = null;
        }

        [TestMethod]
        public void GetWindowHandles()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.NotepadAppId);

            IOSDriver<IOSElement> multiWindowsSession = new IOSDriver<IOSElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(multiWindowsSession);
            Assert.IsNotNull(multiWindowsSession.SessionId);

            var handles = multiWindowsSession.WindowHandles;
            Assert.IsNotNull(handles);
            Assert.IsTrue(handles.Count > 0);
            multiWindowsSession.Quit();
        }

        [TestMethod]
        public void GetWindowHandlesClassicApp()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.NotepadAppId);
            IOSDriver<IOSElement> multiWindowsSession = new IOSDriver<IOSElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(multiWindowsSession);
            Assert.IsNotNull(multiWindowsSession.SessionId);

            var windowHandlesBefore = multiWindowsSession.WindowHandles;
            Assert.IsNotNull(windowHandlesBefore);
            Assert.IsTrue(windowHandlesBefore.Count > 0);

            multiWindowsSession.FindElementByName("File").Click();
            multiWindowsSession.FindElementByName("Save As...").Click();

            System.Threading.Thread.Sleep(1000); // Sleep for 1 second
            var windowHandlesAfter = multiWindowsSession.WindowHandles;
            Assert.IsNotNull(windowHandlesAfter);
            Assert.AreEqual(windowHandlesBefore.Count + 1, windowHandlesAfter.Count);

            foreach (var windowHandle in windowHandlesAfter)
            {
                multiWindowsSession.SwitchTo().Window(windowHandle);
                multiWindowsSession.Close();
            }

            multiWindowsSession.Quit();
        }

        [TestMethod]
        public void GetWindowHandlesModernApp()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.EdgeAppId);
            IOSDriver<IOSElement> multiWindowsSession = new IOSDriver<IOSElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(multiWindowsSession);
            Assert.IsNotNull(multiWindowsSession.SessionId);

            var windowHandlesBefore = multiWindowsSession.WindowHandles;
            Assert.IsNotNull(windowHandlesBefore);
            Assert.IsTrue(windowHandlesBefore.Count > 0);

            // Preserve previously opened Edge window(s) and only manipulate newly opened windows
            List<String> previouslyOpenedEdgeWindows = new List<String>(windowHandlesBefore);
            previouslyOpenedEdgeWindows.Remove(multiWindowsSession.CurrentWindowHandle);

            // Open a new window
            // The menu item names have changed between Windows 10 and the anniversary update
            // account for both combinations.
            try
            {
                multiWindowsSession.FindElementByAccessibilityId("m_actionsMenuButton").Click();
                multiWindowsSession.FindElementByAccessibilityId("m_newWindow").Click();
            }
            catch (System.InvalidOperationException)
            {
                multiWindowsSession.FindElementByAccessibilityId("ActionsMenuButton").Click();
                multiWindowsSession.FindElementByAccessibilityId("ActionsMenuNewWindow").Click();
            }

            System.Threading.Thread.Sleep(3000); // Sleep for 3 seconds
            var windowHandlesAfter = multiWindowsSession.WindowHandles;
            Assert.IsNotNull(windowHandlesAfter);
            Assert.AreEqual(windowHandlesBefore.Count + 1, windowHandlesAfter.Count);

            // Preserve previously opened Edge Windows by only closing newly opened windows
            List<String> newlyOpenedEdgeWindows = new List<String>(windowHandlesAfter);
            foreach (var previouslyOpenedEdgeWindow in previouslyOpenedEdgeWindows)
            {
                newlyOpenedEdgeWindows.Remove(previouslyOpenedEdgeWindow);
            }

            foreach (var windowHandle in newlyOpenedEdgeWindows)
            {
                multiWindowsSession.SwitchTo().Window(windowHandle);
                multiWindowsSession.Close();
            }

            multiWindowsSession.Quit();
        }

        [TestMethod]
        public void SwitchWindows()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.EdgeAppId);
            IOSDriver<IOSElement> multiWindowsSession = new IOSDriver<IOSElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(multiWindowsSession);
            Assert.IsNotNull(multiWindowsSession.SessionId);

            // Preserve previously opened Edge window(s) and only manipulate newly opened windows
            List<String> previouslyOpenedEdgeWindows = new List<String>(multiWindowsSession.WindowHandles);
            previouslyOpenedEdgeWindows.Remove(multiWindowsSession.CurrentWindowHandle);

            // Open a new window
            // The menu item names have changed between Windows 10 and the anniversary update
            // account for both combinations.
            try
            {
                multiWindowsSession.FindElementByAccessibilityId("m_actionsMenuButton").Click();
                multiWindowsSession.FindElementByAccessibilityId("m_newWindow").Click();
            }
            catch (System.InvalidOperationException)
            {
                multiWindowsSession.FindElementByAccessibilityId("ActionsMenuButton").Click();
                multiWindowsSession.FindElementByAccessibilityId("ActionsMenuNewWindow").Click();
            }

            System.Threading.Thread.Sleep(3000); // Sleep for 3 second
            var multipleWindowHandles = multiWindowsSession.WindowHandles;
            Assert.IsTrue(multipleWindowHandles.Count > 1);

            // Preserve previously opened Edge Windows by only operating on newly opened windows
            List<String> newlyOpenedEdgeWindows = new List<String>(multipleWindowHandles);
            foreach (var previouslyOpenedEdgeWindow in previouslyOpenedEdgeWindows)
            {
                newlyOpenedEdgeWindows.Remove(previouslyOpenedEdgeWindow);
            }

            string previousWindowHandle = String.Empty;

            foreach (var windowHandle in newlyOpenedEdgeWindows)
            {
                multiWindowsSession.SwitchTo().Window(windowHandle);
                Assert.AreEqual(multiWindowsSession.CurrentWindowHandle, windowHandle);
                Assert.AreNotEqual(windowHandle, previousWindowHandle);
                previousWindowHandle = windowHandle;
                multiWindowsSession.Close();
            }

            multiWindowsSession.Quit();
        }

        [TestMethod]
        public void ErrorCloseWindowAlreadyClosedApplication()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.AlarmClockAppId);
            IOSDriver<IOSElement> singleWindowSession = new IOSDriver<IOSElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(singleWindowSession);
            Assert.IsNotNull(singleWindowSession.SessionId);

            // Close the application window without deleting the session
            singleWindowSession.Close();
            Assert.IsNotNull(singleWindowSession);
            Assert.IsNotNull(singleWindowSession.SessionId);

            // Attempt to close the previously closed application window
            try
            {
                singleWindowSession.Close();
                Assert.Fail("Exception should have been thrown");
            }
            catch (System.InvalidOperationException e)
            {
                Assert.AreEqual("Currently selected window has been closed", e.Message);
            }

            singleWindowSession.Quit();
        }

        [TestMethod]
        public void ErrorGetWindowHandleAlreadyClosedApplication()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.AlarmClockAppId);
            IOSDriver<IOSElement> singleWindowSession = new IOSDriver<IOSElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(singleWindowSession);
            Assert.IsNotNull(singleWindowSession.SessionId);

            // Close the application window without deleting the session
            singleWindowSession.Close();
            Assert.IsNotNull(singleWindowSession);
            Assert.IsNotNull(singleWindowSession.SessionId);

            try
            {
                string windowHandle = singleWindowSession.CurrentWindowHandle;
                Assert.Fail("Exception should have been thrown");
            }
            catch (System.InvalidOperationException e)
            {
                Assert.AreEqual("Currently selected window has been closed", e.Message);
            }

            singleWindowSession.Quit();
        }
    }

    [TestClass]
    public class WindowTransform
    {
        protected static IOSDriver<IOSElement> WindowTransformSession;   // Temporary placeholder until Windows namespace exists
        protected static System.Drawing.Size OriginalSize;
        protected static System.Drawing.Point OriginalPosition;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            // Launch the Calculator app
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.CalculatorAppId);
            WindowTransformSession = new IOSDriver<IOSElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
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
        public void GetWindowSize()
        {
            var windowSize = WindowTransformSession.Manage().Window.Size;
            Assert.IsNotNull(windowSize);
            Assert.AreEqual(OriginalSize.Height, windowSize.Height);
            Assert.AreEqual(OriginalSize.Width, windowSize.Width);
        }

        [TestMethod]
        public void MaximizeWindow()
        {
            WindowTransformSession.Manage().Window.Maximize();
            var windowSize = WindowTransformSession.Manage().Window.Size;
            Assert.IsNotNull(windowSize);
            Assert.IsTrue(OriginalSize.Height <= windowSize.Height);
            Assert.IsTrue(OriginalSize.Width <= windowSize.Width);
        }

        [TestMethod]
        public void SetWindowPosition()
        {
            int offset = 100;
            WindowTransformSession.Manage().Window.Position = new System.Drawing.Point(OriginalPosition.X + offset, OriginalPosition.Y + offset);
            var windowPosition = WindowTransformSession.Manage().Window.Position;
            Assert.IsNotNull(windowPosition);
            Assert.AreEqual(OriginalPosition.X + offset, windowPosition.X);
            Assert.AreEqual(OriginalPosition.Y + offset, windowPosition.Y);
        }

        [TestMethod]
        public void SetWindowSize()
        {
            int offset = 100;
            WindowTransformSession.Manage().Window.Size = new System.Drawing.Size(OriginalSize.Width + offset, OriginalSize.Height + offset);
            var windowSize = WindowTransformSession.Manage().Window.Size;
            Assert.AreEqual(OriginalSize.Width + offset, windowSize.Width);
            Assert.AreEqual(OriginalSize.Height + offset, windowSize.Height);
        }
    }
}

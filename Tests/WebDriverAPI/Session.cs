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
using System.Net;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium;

namespace WebDriverAPI
{
    [TestClass]
    public class Session
    {
        private WindowsDriver<WindowsElement> session = null;
        private WindowsDriver<WindowsElement> secondarySession = null;

        [TestCleanup]
        public void TestCleanup()
        {
            if (session != null)
            {
                session.Quit();
                session = null;
            }

            if (secondarySession != null)
            {
                secondarySession.Quit();
                secondarySession = null;
            }
        }

        [TestMethod]
        public void CreateSession_ClassicApp()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.NotepadAppId);
            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);
            Assert.IsTrue(session.Title.Contains("Notepad"));
        }

        [TestMethod]
        public void CreateSession_ClassicAppShortName()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", "notepad"); // Use "notepad" instead of the full path with extension
            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);
            Assert.IsTrue(session.Title.Contains("Notepad"));
        }

        [TestMethod]
        public void CreateSession_Desktop()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.DesktopAppId);
            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);
            Assert.IsTrue(session.Title.Contains("Desktop"));
        }

        [TestMethod]
        public void CreateSession_ModernApp()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.CalculatorAppId);
            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);
            Assert.IsTrue(session.Title.Contains("Calculator"));
        }

        [TestMethod]
        public void CreateSession_SystemApp()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.ExplorerAppId);
            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);
            Assert.IsTrue(session.Title == "File Explorer" || session.Title == "This PC");
        }

        [TestMethod]
        public void CreateSessionError_EmptyAppId()
        {
            try
            {
                DesiredCapabilities appCapabilities = new DesiredCapabilities();
                appCapabilities.SetCapability("app", string.Empty);
                session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual("Capability: app cannot be empty", exception.Message);
            }
        }

        [TestMethod]
        public void CreateSessionError_InvalidAppIdClassicApp()
        {
            try
            {
                DesiredCapabilities appCapabilities = new DesiredCapabilities();
                appCapabilities.SetCapability("app", @"C:\Windows\System32\BadAppId...");
                session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual("The system cannot find the file specified", exception.Message);
            }
        }

        [TestMethod]
        public void CreateSessionError_InvalidAppIdModernApp()
        {
            try
            {
                DesiredCapabilities appCapabilities = new DesiredCapabilities();
                appCapabilities.SetCapability("app", "Microsoft.BadAppId!App");
                session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual("Value does not fall within the expected range.", exception.Message);
            }
        }

        [TestMethod]
        public void CreateSessionError_InvalidSessionId()
        {
            try
            {
                HttpWebResponse response = WebRequest.Create(CommonTestSettings.WindowsApplicationDriverUrl + "/session/" + "INVALID-SESSION-ID").GetResponse() as HttpWebResponse;
                Assert.Fail("Exception should have been thrown");
            }
            catch { }
        }

        [TestMethod]
        public void CreateSessionError_MissingAppLaunchSpecifier()
        {
            try
            {
                // Either app or appTopLevelWindow has to be specified to create a session. Leaving them empty would return an error below.
                DesiredCapabilities appCapabilities = new DesiredCapabilities();
                session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual("Bad capabilities. Specify either app or appTopLevelWindow to create a session", exception.Message);
            }
        }

        [TestMethod]
        public void CreateSessionError_RedundantAppLaunchSpecifiers()
        {
            try
            {
                // Either app or appTopLevelWindow has to be specified to create a session. Specifying both would return an error below.
                DesiredCapabilities appCapabilities = new DesiredCapabilities();
                appCapabilities.SetCapability("app", "Microsoft.AnAppId!App");
                appCapabilities.SetCapability("appTopLevelWindow", "77777");
                session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual("Bad capabilities. Specify either app or appTopLevelWindow to create a session", exception.Message);
            }
        }

        [TestMethod]
        public void CreateSessionFromExistingWindowHandle_ClassicApp()
        {
            // Get the top level window handle of a running application
            secondarySession = Utility.CreateNewSession(CommonTestSettings.NotepadAppId);
            var existingApplicationTopLevelWindow = secondarySession.CurrentWindowHandle;

            // Create a new session by attaching to an existing application top level window
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("appTopLevelWindow", existingApplicationTopLevelWindow);
            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);

            // Verify that newly created session is indeed attached to the correct application top level window
            Assert.AreEqual(secondarySession.CurrentWindowHandle, session.CurrentWindowHandle);
            Assert.AreEqual(secondarySession.FindElementByClassName("Edit"), session.FindElementByClassName("Edit"));
        }

        [TestMethod]
        public void CreateSessionFromExistingWindowHandle_ModernApp()
        {
            // Get the top level window handle of a running application
            secondarySession = Utility.CreateNewSession(CommonTestSettings.CalculatorAppId);
            var existingApplicationTopLevelWindow = secondarySession.CurrentWindowHandle;

            // Create a new session by attaching to an existing application top level window
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("appTopLevelWindow", existingApplicationTopLevelWindow);
            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);

            // Verify that newly created session is indeed attached to the correct application top level window
            Assert.AreEqual(secondarySession.CurrentWindowHandle, session.CurrentWindowHandle);
            Assert.AreEqual(secondarySession.FindCalculatorTitleByAccessibilityId(), session.FindCalculatorTitleByAccessibilityId());
        }

        [TestMethod]
        public void CreateSessionFromExistingWindowHandleError_EmptyValue()
        {
            try
            {
                DesiredCapabilities appCapabilities = new DesiredCapabilities();
                appCapabilities.SetCapability("appTopLevelWindow", string.Empty);
                session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual("Capability: appTopLevelWindow cannot be empty", exception.Message);
            }
        }

        [TestMethod]
        public void CreateSessionFromExistingWindowHandleError_InvalidValue()
        {
            try
            {
                DesiredCapabilities appCapabilities = new DesiredCapabilities();
                appCapabilities.SetCapability("appTopLevelWindow", "-1");
                session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual("String cannot contain a minus sign if the base is not 10.", exception.Message);
            }
        }

        [TestMethod]
        public void CreateSessionFromExistingWindowHandleError_NonTopLevelWindowHandle()
        {
            // Get a non top level window element
            secondarySession = Utility.CreateNewSession(CommonTestSettings.CalculatorAppId);
            var nonTopLevelWindowHandle = secondarySession.FindElementByClassName("Windows.UI.Core.CoreWindow").GetAttribute("NativeWindowHandle");

            try
            {
                DesiredCapabilities appCapabilities = new DesiredCapabilities();
                appCapabilities.SetCapability("appTopLevelWindow", nonTopLevelWindowHandle);
                session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual("Cannot find active window specified by capabilities: appTopLevelWindow", exception.Message);
            }
        }

        [TestMethod]
        public void CreateSessionFromExistingWindowHandleError_StaleWindowHandle()
        {
            // Get a stale window handle from an orphaned session which window has been closed
            var staleTopLevelWindow = Utility.GetOrphanedWindowHandle();
            Thread.Sleep(TimeSpan.FromSeconds(3));

            try
            {
                DesiredCapabilities appCapabilities = new DesiredCapabilities();
                appCapabilities.SetCapability("appTopLevelWindow", staleTopLevelWindow);
                session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual("Cannot find active window specified by capabilities: appTopLevelWindow", exception.Message);
            }
        }

        [TestMethod]
        public void CreateSessionWithArguments_ClassicApp()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.NotepadAppId);
            appCapabilities.SetCapability("appArguments", "NonExistentTextFile.txt");
            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);

            // Verify that Notepad file not found dialog is displayed and dismiss it
            var notFoundDialog = session.FindElementByName("Notepad");
            Assert.AreEqual("ControlType.Window", notFoundDialog.TagName);
            notFoundDialog.FindElementByName("No").Click();

            Assert.IsTrue(session.Title.Contains("Notepad"));
        }

        [TestMethod]
        public void CreateSessionWithArguments_ModernApp()
        {
            // Open about:blank page in Microsoft Edge using appArguments
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.EdgeAppId);
            appCapabilities.SetCapability("appArguments", CommonTestSettings.EdgeAboutBlankURL);
            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);
            Thread.Sleep(TimeSpan.FromSeconds(2));
            Assert.IsTrue(session.Title.Contains("Blank page"));
            EdgeBase.CloseEdge(session);
        }

        [TestMethod]
        public void CreateSessionWithArguments_SystemApp()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.ExplorerAppId);
            appCapabilities.SetCapability("appArguments", "/"); // Open the explorer window at My Documents
            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);
            Assert.AreEqual("Documents", session.Title);
        }

        [TestMethod]
        public void CreateSessionWithWorkingDirectoryAndArguments()
        {
            // Use File Explorer to get the temporary folder full path
            secondarySession = Utility.CreateNewSession(CommonTestSettings.ExplorerAppId);
            secondarySession.Keyboard.SendKeys(Keys.Alt + "d" + Keys.Alt + CommonTestSettings.TestFolderLocation + Keys.Enter);
            Thread.Sleep(TimeSpan.FromSeconds(2));
            secondarySession.Keyboard.SendKeys(Keys.Alt + "d" + Keys.Alt); // Select the address edit box using Alt + d shortcut
            string tempFolderFullPath = secondarySession.SwitchTo().ActiveElement().Text;
            Assert.IsFalse(string.IsNullOrEmpty(tempFolderFullPath));

            // Launch Notepad with a filename argument and temporary folder path as the working directory
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.NotepadAppId);
            appCapabilities.SetCapability("appArguments", CommonTestSettings.TestFileName);
            appCapabilities.SetCapability("appWorkingDir", tempFolderFullPath);
            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);

            try
            {
                // Verify that Notepad file not found dialog is displayed and save it
                var notFoundDialog = session.FindElementByName("Notepad");
                Assert.AreEqual("ControlType.Window", notFoundDialog.TagName);
                notFoundDialog.FindElementByName("Yes").Click();
            }
            catch
            {
                // Verify that the window title matches the filename if somehow a leftover test file exists from previous incomplete test run
                Assert.IsTrue(session.Title.Contains(CommonTestSettings.TestFileName));
            }

            session.Quit();
            session = null;

            // Verify that the file is indeed saved in the working directory and delete it
            secondarySession.FindElementByAccessibilityId("SearchEditBox").SendKeys(CommonTestSettings.TestFileName + Keys.Enter);
            Thread.Sleep(TimeSpan.FromSeconds(2));
            WindowsElement testFileEntry = null;
            try
            {
                testFileEntry = secondarySession.FindElementByName("Items View").FindElementByName(CommonTestSettings.TestFileName + ".txt") as WindowsElement;  // In case extension is added automatically
            }
            catch
            {
                testFileEntry = secondarySession.FindElementByName("Items View").FindElementByName(CommonTestSettings.TestFileName) as WindowsElement;
            }

            Assert.IsNotNull(testFileEntry);
            testFileEntry.Click();
            testFileEntry.SendKeys(Keys.Delete);
            Thread.Sleep(TimeSpan.FromSeconds(1));
        }

        [TestMethod]
        public void CreateSessionWithWorkingDirectoryError_InvalidValue()
        {
            try
            {
                DesiredCapabilities appCapabilities = new DesiredCapabilities();
                appCapabilities.SetCapability("app", CommonTestSettings.NotepadAppId);
                appCapabilities.SetCapability("appWorkingDir", @"BadDriver:\BadValue");
                session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual("The directory name is invalid", exception.Message);
            }
        }

        [TestMethod]
        public void DeleteSession_ClassicApp()
        {
            session = Utility.CreateNewSession(CommonTestSettings.NotepadAppId);
            Assert.IsNotNull(session.SessionId);

            session.Quit();
            Assert.IsNull(session.SessionId);
            session = null;
        }

        [TestMethod]
        public void DeleteSession_Desktop()
        {
            session = Utility.CreateNewSession(CommonTestSettings.DesktopAppId);
            Assert.IsNotNull(session.SessionId);

            session.Quit();
            Assert.IsNull(session.SessionId);
            session = null;
        }

        [TestMethod]
        public void DeleteSession_ModernApp()
        {
            session = Utility.CreateNewSession(CommonTestSettings.CalculatorAppId);
            Assert.IsNotNull(session.SessionId);

            session.Quit();
            Assert.IsNull(session.SessionId);
            session = null;
        }

        [TestMethod]
        public void DeleteSession_SystemApp()
        {
            session = Utility.CreateNewSession(CommonTestSettings.ExplorerAppId);
            Assert.IsNotNull(session.SessionId);

            session.Quit();
            Assert.IsNull(session.SessionId);
            session = null;
        }

        [TestMethod]
        public void MiscellaneousSession_GetSessionCapabilities()
        {
            session = Utility.CreateNewSession(CommonTestSettings.AlarmClockAppId);
            Assert.IsNotNull(session.SessionId);

            ICapabilities capabilities = session.Capabilities;
            Assert.AreEqual(CommonTestSettings.AlarmClockAppId, capabilities.GetCapability("app"));
            Assert.AreEqual("Windows", capabilities.GetCapability("platformName"));
        }

        [TestMethod]
        public void MiscellaneousSession_MultiSessionsMultiInstances()
        {
            session = Utility.CreateNewSession(CommonTestSettings.NotepadAppId);
            Assert.IsNotNull(session.SessionId);
            Assert.IsTrue(session.Title.Contains("Notepad"));

            secondarySession = Utility.CreateNewSession(CommonTestSettings.NotepadAppId);
            Assert.IsNotNull(secondarySession.SessionId);
            Assert.IsTrue(secondarySession.Title.Contains("Notepad"));

            Assert.AreNotEqual(session.SessionId, secondarySession.SessionId);
            Assert.AreNotEqual(session.CurrentWindowHandle, secondarySession.CurrentWindowHandle);
        }

        [TestMethod]
        public void MiscellaneousSession_MultiSessionsSingleInstance()
        {
            session = Utility.CreateNewSession(CommonTestSettings.AlarmClockAppId);
            Assert.IsNotNull(session.SessionId);
            Assert.AreEqual("Alarms & Clock", session.Title);

            secondarySession = Utility.CreateNewSession(CommonTestSettings.AlarmClockAppId);
            Assert.IsNotNull(secondarySession.SessionId);
            Assert.AreEqual("Alarms & Clock", secondarySession.Title);

            Assert.AreNotEqual(session.SessionId, secondarySession.SessionId);
            Assert.AreEqual(session.CurrentWindowHandle, secondarySession.CurrentWindowHandle);
        }

        [TestMethod]
        public void MiscellaneousSessionError_StaleSessionId()
        {
            try
            {
                session = Utility.CreateNewSession(CommonTestSettings.CalculatorAppId);
                session.Quit();
                string applicationTitle = session.Title;
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.IsTrue(exception.Message.StartsWith("No active session with ID title"));
            }
        }
    }
}

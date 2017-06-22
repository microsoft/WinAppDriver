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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium;

namespace W3CWebDriver
{
    [TestClass]
    public class Session
    {
        private WindowsDriver<WindowsElement> session = null;

        [TestMethod]
        public void CreateSession_ClassicApp()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.NotepadAppId);
            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);
            Assert.IsTrue(session.Title.Contains("Notepad"));
            session.Quit();
            session = null;
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
            session.Quit();
            session = null;
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
            session.Quit();
            session = null;
        }

        [TestMethod]
        public void CreateSession_ModernApp()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.CalculatorAppId);
            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);
            Assert.AreEqual("Calculator", session.Title);
            session.Quit();
            session = null;
        }

        [TestMethod]
        public void CreateSession_SystemApp()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.ExplorerAppId);
            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);
            Assert.AreEqual("File Explorer", session.Title);
            session.Quit();
            session = null;
        }

        [TestMethod]
        public void CreateSessionErrorEmptyAppId()
        {
            try
            {
                DesiredCapabilities appCapabilities = new DesiredCapabilities();
                appCapabilities.SetCapability("app", string.Empty);
                session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
                Assert.Fail("Exception should have been thrown");
            }
            catch (Exception e)
            {
                Assert.AreEqual("Capability: app cannot be empty", e.Message);
            }
        }

        [TestMethod]
        public void CreateSessionErrorInvalidAppIdClassicApp()
        {
            try
            {
                DesiredCapabilities appCapabilities = new DesiredCapabilities();
                appCapabilities.SetCapability("app", @"C:\Windows\System32\BadAppId...");
                session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
                Assert.Fail("Exception should have been thrown");
            }
            catch (Exception e)
            {
                Assert.AreEqual("The system cannot find the file specified", e.Message);
            }
        }

        [TestMethod]
        public void CreateSessionErrorInvalidAppIdModernApp()
        {
            try
            {
                DesiredCapabilities appCapabilities = new DesiredCapabilities();
                appCapabilities.SetCapability("app", "Microsoft.BadAppId!App");
                session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
                Assert.Fail("Exception should have been thrown");
            }
            catch (Exception e)
            {
                Assert.AreEqual("Value does not fall within the expected range.", e.Message);
            }
        }

        [TestMethod]
        public void CreateSessionErrorInvalidSessionId()
        {
            try
            {
                HttpWebResponse response = WebRequest.Create(CommonTestSettings.WindowsApplicationDriverUrl + "/session/" + "INVALID-SESSION-ID").GetResponse() as HttpWebResponse;
                Assert.Fail("Exception should have been thrown");
            }
            catch { }
        }

        [TestMethod]
        public void CreateSessionErrorMissingAppLaunchSpecifier()
        {
            try
            {
                // Either app or appTopLevelWindow has to be specified to create a session. Leaving them empty would return an error below.
                DesiredCapabilities appCapabilities = new DesiredCapabilities();
                session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
                Assert.Fail("Exception should have been thrown");
            }
            catch (Exception e)
            {
                Assert.AreEqual("Bad capabilities. Specify either app or appTopLevelWindow to create a session", e.Message);
            }
        }

        [TestMethod]
        public void CreateSessionErrorRedundantAppLaunchSpecifiers()
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
            catch (Exception e)
            {
                Assert.AreEqual("Bad capabilities. Specify either app or appTopLevelWindow to create a session", e.Message);
            }
        }

        [TestMethod]
        public void CreateSessionFromExistingWindowHandle_ClassicApp()
        {
            // Get the top level window handle of a running application
            WindowsDriver<WindowsElement> existingSession = Utility.CreateNewSession(CommonTestSettings.NotepadAppId);
            var existingApplicationTopLevelWindow = existingSession.CurrentWindowHandle;

            // Create a new session by attaching to an existing application top level window
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("appTopLevelWindow", existingApplicationTopLevelWindow);
            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);

            // Verify that newly created session is indeed attached to the correct application top level window
            Assert.AreEqual(existingSession.CurrentWindowHandle, session.CurrentWindowHandle);
            Assert.AreEqual(existingSession.FindElementByClassName("Edit"), session.FindElementByClassName("Edit"));
            existingSession.Quit();
            session.Quit();
            session = null;
        }

        [TestMethod]
        public void CreateSessionFromExistingWindowHandle_ModernApp()
        {
            // Get the top level window handle of a running application
            WindowsDriver<WindowsElement> existingSession = Utility.CreateNewSession(CommonTestSettings.CalculatorAppId);
            var existingApplicationTopLevelWindow = existingSession.CurrentWindowHandle;

            // Create a new session by attaching to an existing application top level window
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("appTopLevelWindow", existingApplicationTopLevelWindow);
            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);

            // Verify that newly created session is indeed attached to the correct application top level window
            Assert.AreEqual(existingSession.CurrentWindowHandle, session.CurrentWindowHandle);
            Assert.AreEqual(existingSession.FindElementByAccessibilityId("Header"), session.FindElementByAccessibilityId("Header"));
            existingSession.Quit();
            session.Quit();
            session = null;
        }

        [TestMethod]
        public void CreateSessionFromExistingWindowHandleErrorEmptyValue()
        {
            try
            {
                DesiredCapabilities appCapabilities = new DesiredCapabilities();
                appCapabilities.SetCapability("appTopLevelWindow", string.Empty);
                session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
                Assert.Fail("Exception should have been thrown");
            }
            catch (Exception e)
            {
                Assert.AreEqual("Capability: appTopLevelWindow cannot be empty", e.Message);
            }
        }

        [TestMethod]
        public void CreateSessionFromExistingWindowHandleErrorInvalidValue()
        {
            try
            {
                DesiredCapabilities appCapabilities = new DesiredCapabilities();
                appCapabilities.SetCapability("appTopLevelWindow", "-1");
                session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
                Assert.Fail("Exception should have been thrown");
            }
            catch (Exception e)
            {
                Assert.AreEqual("String cannot contain a minus sign if the base is not 10.", e.Message);
            }
        }

        [TestMethod]
        public void CreateSessionFromExistingWindowHandleErrorNonTopLevelWindowHandle()
        {
            // Get a non top level window element
            WindowsDriver<WindowsElement> sideSession = Utility.CreateNewSession(CommonTestSettings.CalculatorAppId);
            var nonTopLevelWindowHandle = sideSession.FindElementByClassName("Windows.UI.Core.CoreWindow").GetAttribute("NativeWindowHandle");

            try
            {
                DesiredCapabilities appCapabilities = new DesiredCapabilities();
                appCapabilities.SetCapability("appTopLevelWindow", nonTopLevelWindowHandle);
                session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
                Assert.Fail("Exception should have been thrown");
            }
            catch (Exception e)
            {
                Assert.AreEqual("Cannot find active window specified by capabilities: appTopLevelWindow", e.Message);
            }

            sideSession.Quit();
        }

        [TestMethod]
        public void CreateSessionFromExistingWindowHandleErrorStaleWindowHandle()
        {
            // Get a stale window handle from an orphaned session which window has been closed
            var staleTopLevelWindow = Utility.GetOrphanedWindowHandle();
            System.Threading.Thread.Sleep(3000);

            try
            {
                DesiredCapabilities appCapabilities = new DesiredCapabilities();
                appCapabilities.SetCapability("appTopLevelWindow", staleTopLevelWindow);
                session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
                Assert.Fail("Exception should have been thrown");
            }
            catch (Exception e)
            {
                Assert.AreEqual("Cannot find active window specified by capabilities: appTopLevelWindow", e.Message);
            }
        }

        [TestMethod]
        public void CreateSessionWithArgumentsClassicApp()
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
            session.Quit();
            session = null;
        }

        [TestMethod]
        public void CreateSessionWithArgumentsModernApp()
        {
            // Launch a new Edge window in private mode using appArguments
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.EdgeAppId);
            appCapabilities.SetCapability("appArguments", "-private");
            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);
            Assert.IsTrue(session.Title.Contains("InPrivate"));
            session.Quit();
            session = null;
        }

        [TestMethod]
        public void CreateSessionWithArgumentsSystemApp()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.ExplorerAppId);
            appCapabilities.SetCapability("appArguments", "/"); // Open the explorer window at My Documents
            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);
            Assert.AreEqual("Documents", session.Title);
            session.Quit();
            session = null;
        }

        [TestMethod]
        public void CreateSessionWithWorkingDirectory()
        {
            // Use File Explorer to get the root folder full path
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.ExplorerAppId);
            appCapabilities.SetCapability("appArguments", @"\"); // Open File Explorer at the root folder using argument '\'
            WindowsDriver<WindowsElement> explorerSession = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            string rootFolderFullPath = explorerSession.Title;
            explorerSession.Quit();

            // Launch Notepad with root folder path as the working directory
            // NOTE: The working directory parameter is only applied to classic windows application and ignored for modern application
            appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.NotepadAppId);
            appCapabilities.SetCapability("appWorkingDir", rootFolderFullPath); // Open Notepad using the root folder as current working directory
            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);

            // Verify that the open dialog is indeed starting from the working directory provided
            session.Keyboard.SendKeys(OpenQA.Selenium.Keys.Control + "o" + OpenQA.Selenium.Keys.Control); // Launch file open dialog
            session.Keyboard.SendKeys(OpenQA.Selenium.Keys.Alt + "d" + OpenQA.Selenium.Keys.Alt); // Use shortcut to highlight the address bar
            System.Threading.Thread.Sleep(3000);
            Assert.AreEqual(rootFolderFullPath, session.FindElementByName("Address").Text);
            session.FindElementByName("Cancel").Click();
            session.Quit();
            session = null;
        }

        [TestMethod]
        public void CreateSessionWithWorkingDirectoryAndArguments()
        {
            // Use File Explorer to get the temporary folder full path
            WindowsDriver<WindowsElement> explorerSession = Utility.CreateNewSession(CommonTestSettings.ExplorerAppId);
            explorerSession.Keyboard.SendKeys(OpenQA.Selenium.Keys.Alt + "d" + OpenQA.Selenium.Keys.Alt + CommonTestSettings.TestFolderLocation + OpenQA.Selenium.Keys.Enter);
            System.Threading.Thread.Sleep(2000);
            string tempFolderFullPath = explorerSession.Title;

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
            explorerSession.FindElementByAccessibilityId("SearchEditBox").SendKeys(CommonTestSettings.TestFileName + OpenQA.Selenium.Keys.Enter);
            System.Threading.Thread.Sleep(2000);
            WindowsElement testFileEntry = explorerSession.FindElementByName(CommonTestSettings.TestFileName);
            testFileEntry.Click();
            testFileEntry.SendKeys(OpenQA.Selenium.Keys.Delete);
            explorerSession.Quit();
            explorerSession = null;
        }

        [TestMethod]
        public void CreateSessionWithWorkingDirectoryErrorInvalidValue()
        {
            try
            {
                DesiredCapabilities appCapabilities = new DesiredCapabilities();
                appCapabilities.SetCapability("app", CommonTestSettings.NotepadAppId);
                appCapabilities.SetCapability("appWorkingDir", @"BadDriver:\BadValue");
                session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
                Assert.Fail("Exception should have been thrown");
            }
            catch (Exception e)
            {
                Assert.AreEqual("The directory name is invalid", e.Message);
            }
        }

        [TestMethod]
        public void DeleteSessionClassicApp()
        {
            session = Utility.CreateNewSession(CommonTestSettings.NotepadAppId);
            Assert.IsNotNull(session.SessionId);

            session.Quit();
            Assert.IsNull(session.SessionId);
            session = null;
        }

        [TestMethod]
        public void DeleteSessionDesktop()
        {
            session = Utility.CreateNewSession(CommonTestSettings.DesktopAppId);
            Assert.IsNotNull(session.SessionId);

            session.Quit();
            Assert.IsNull(session.SessionId);
            session = null;
        }

        [TestMethod]
        public void DeleteSessionModernApp()
        {
            session = Utility.CreateNewSession(CommonTestSettings.CalculatorAppId);
            Assert.IsNotNull(session.SessionId);

            session.Quit();
            Assert.IsNull(session.SessionId);
            session = null;
        }

        [TestMethod]
        public void DeleteSessionSystemApp()
        {
            session = Utility.CreateNewSession(CommonTestSettings.ExplorerAppId);
            Assert.IsNotNull(session.SessionId);

            session.Quit();
            Assert.IsNull(session.SessionId);
            session = null;
        }

        [TestMethod]
        public void GetSessionCapabilities()
        {
            session = Utility.CreateNewSession(CommonTestSettings.AlarmClockAppId);
            Assert.IsNotNull(session.SessionId);

            ICapabilities capabilities = session.Capabilities;
            Assert.AreEqual(CommonTestSettings.AlarmClockAppId, capabilities.GetCapability("app"));
            Assert.AreEqual("Windows", capabilities.GetCapability("platformName"));

            session.Quit();
            session = null;
        }

        [TestMethod]
        public void MultipleSessions()
        {
            WindowsDriver<WindowsElement> session1 = null;
            WindowsDriver<WindowsElement> session2 = null;

            try
            {
                session1 = Utility.CreateNewSession(CommonTestSettings.NotepadAppId);
                Assert.IsNotNull(session1.SessionId);
                Assert.IsTrue(session1.Title.Contains("Notepad"));

                session2 = Utility.CreateNewSession(CommonTestSettings.NotepadAppId);
                Assert.IsNotNull(session2.SessionId);
                Assert.IsTrue(session2.Title.Contains("Notepad"));

                Assert.AreNotEqual(session1.SessionId, session2.SessionId);
                Assert.AreNotEqual(session1.CurrentWindowHandle, session2.CurrentWindowHandle);
            }
            finally
            {
                session1.Quit();
                session1 = null;
                session2.Quit();
                session2 = null;
            }
        }

        [TestMethod]
        public void MultipleSessionsSingleInstanceApplication()
        {
            WindowsDriver<WindowsElement> session1 = null;
            WindowsDriver<WindowsElement> session2 = null;

            try
            {
                session1 = Utility.CreateNewSession(CommonTestSettings.AlarmClockAppId);
                Assert.IsNotNull(session1.SessionId);
                Assert.AreEqual("Alarms & Clock", session1.Title);

                session2 = Utility.CreateNewSession(CommonTestSettings.AlarmClockAppId);
                Assert.IsNotNull(session2.SessionId);
                Assert.AreEqual("Alarms & Clock", session2.Title);

                Assert.AreNotEqual(session1.SessionId, session2.SessionId);
                Assert.AreEqual(session1.CurrentWindowHandle, session2.CurrentWindowHandle);
            }
            finally
            {
                session1.Quit();
                session1 = null;
                session2.Quit();
                session2 = null;
            }
        }

        [TestMethod]
        public void SessionOperationErrorStaleSessionId()
        {
            try
            {
                session = Utility.CreateNewSession(CommonTestSettings.CalculatorAppId);
                session.Quit();
                string applicationTitle = session.Title;
                Assert.Fail("Exception should have been thrown");
            }
            catch (Exception e)
            {
                Assert.IsTrue(e.Message.StartsWith("No active session with ID title"));
            }
        }
    }
}

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
using OpenQA.Selenium.Remote;
using OpenQA.Selenium;
using System.Threading;
using System;

namespace NotepadTest
{
    [TestClass]
    public class ScenarioPopupDialog : NotepadSession
    {
        private const string ExplorerAppId = @"C:\Windows\System32\explorer.exe";
        private const string TestFileName = "NotepadTestFile";
        private const string TargetSaveLocation = @"%TEMP%\";

        [TestMethod]
        public void PopupDialogSaveFile()
        {
            session.FindElementByName("File").Click();
            session.FindElementByName("Save As...").Click();
            Thread.Sleep(TimeSpan.FromSeconds(1)); // Wait for 1 second until the save dialog appears
            session.FindElementByAccessibilityId("FileNameControlHost").SendKeys(TargetSaveLocation + TestFileName);
            session.FindElementByName("Save").Click();

            // Check if the Save As dialog appears when there's a leftover test file from previous test run
            try
            {
                Thread.Sleep(TimeSpan.FromSeconds(1)); // Wait for 1 second in case save as dialog appears
                session.FindElementByName("Confirm Save As").FindElementByName("Yes").Click();
            }
            catch { }

            // Verify that Notepad has saved the edited text file with the given name
            Thread.Sleep(TimeSpan.FromSeconds(1.5)); // Wait for 1.5 seconds until the window title is updated
            Assert.IsTrue(session.Title.Contains(TestFileName));
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            // Create a Windows Explorer session to delete the saved text file above
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", ExplorerAppId);
            appCapabilities.SetCapability("deviceName", "WindowsPC");

            WindowsDriver<WindowsElement> windowsExplorerSession = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(windowsExplorerSession);

            // Navigate Windows Explorer to the target save location folder
            windowsExplorerSession.Keyboard.SendKeys(Keys.Alt + "d" + Keys.Alt + TargetSaveLocation + Keys.Enter);

            // Verify that the file is indeed saved in the working directory and delete it
            windowsExplorerSession.FindElementByAccessibilityId("SearchEditBox").SendKeys(TestFileName + Keys.Enter);
            Thread.Sleep(TimeSpan.FromSeconds(2));
            WindowsElement testFileEntry = null;
            try
            {
                testFileEntry = windowsExplorerSession.FindElementByName("Items View").FindElementByName(TestFileName + ".txt") as WindowsElement;  // In case extension is added automatically
            }
            catch
            {
                testFileEntry = windowsExplorerSession.FindElementByName("Items View").FindElementByName(TestFileName) as WindowsElement;
            }

            // Delete the test file when it exists
            if (testFileEntry != null)
            {
                testFileEntry.Click();
                testFileEntry.SendKeys(Keys.Delete);
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }

            windowsExplorerSession.Quit();
            windowsExplorerSession = null;
            TearDown();
        }
    }
}

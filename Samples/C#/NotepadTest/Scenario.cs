using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Appium.iOS;

namespace NotepadTest
{
    [TestClass]
    public class Scenario
    {
        protected const string AppDriverUrl = "http://127.0.0.1:4723";
        protected const string TextValue = "This is an automated test on Classic Windows Application!";
        protected const string NotepadAppId = @"C:\Windows\System32\notepad.exe";
        protected const string ExplorerAppId = @"C:\Windows\System32\explorer.exe";
        protected const string TestFileName = "NotepadTestOutputFile.txt";
        protected const string TargetSaveLocation = @"%TEMP%\";
        protected static IOSDriver<IOSElement> NotepadSession;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            // Launch Notepad Classic Windows Application
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", NotepadAppId);
            NotepadSession = new IOSDriver<IOSElement>(new Uri(AppDriverUrl), appCapabilities);
            Assert.IsNotNull(NotepadSession);
        }

        [TestMethod]
        public void BasicScenario()
        {
            // Verify that Notepad is started with untitled new file
            Assert.AreEqual("Untitled - Notepad", NotepadSession.Title);

            EnterText();
            SaveFile();

            // Verify that Notepad has saved the edited text file with the given name
            Thread.Sleep(1000); // Wait for 1 second until the window title is updated
            Assert.AreEqual(TestFileName + " - Notepad", NotepadSession.Title);
        }

        public void EnterText()
        {
            // Enter text into the edit field
            var editBox = NotepadSession.FindElementByClassName("Edit");
            editBox.SendKeys(TextValue);
            Assert.AreEqual(TextValue, editBox.Text);

            // Enter TimeStamp
            NotepadSession.FindElementByName("Edit").Click();
            NotepadSession.FindElementByName("Time/Date").Click();
            Assert.AreNotEqual(TextValue, editBox.Text);
        }

        public void SaveFile()
        {
            NotepadSession.FindElementByName("File").Click();
            NotepadSession.FindElementByName("Save As...").Click();

            System.Threading.Thread.Sleep(1000); // Wait for 1 second until the save dialog appears
            var fileNameBox = NotepadSession.FindElementByAccessibilityId("FileNameControlHost");
            fileNameBox.SendKeys(TargetSaveLocation + TestFileName);
            NotepadSession.FindElementByName("Save").Click();

            // Confirm save as dialog in case there's leftover test file from previous test run
            try
            {
                Thread.Sleep(1000); // Wait for 1 second until the dialog comes up
                var confirmSaveAsDialog = NotepadSession.FindElementByName("Confirm Save As");
                confirmSaveAsDialog.FindElementByName("Yes").Click();
            }
            catch { }
        }

        [ClassCleanup]
        public static void TearDown()
        {
            // Close Notepad
            NotepadSession.Quit();
            NotepadSession = null;

            // Launch Windows Explorer to delete the saved text file above
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", ExplorerAppId);
            IOSDriver<IOSElement> WindowsExplorerSession = new IOSDriver<IOSElement>(new Uri(AppDriverUrl), appCapabilities);
            Assert.IsNotNull(WindowsExplorerSession);

            // Create Desktop session to control context menu and access dialogs
            DesiredCapabilities desktopCapabilities = new DesiredCapabilities();
            desktopCapabilities.SetCapability("app", "Root");
            IOSDriver<IOSElement> DesktopSession = new IOSDriver<IOSElement>(new Uri(AppDriverUrl), desktopCapabilities);
            Assert.IsNotNull(DesktopSession);

            // Navigate Windows Explorer to the target save location folder
            Thread.Sleep(1000); // Wait for 1 second
            var addressBandRoot = WindowsExplorerSession.FindElementByClassName("Address Band Root");
            var addressToolbar = addressBandRoot.FindElementByAccessibilityId("1001"); // Address Band Toolbar
            WindowsExplorerSession.Mouse.Click(addressToolbar.Coordinates);
            addressBandRoot.FindElementByAccessibilityId("41477").SendKeys(TargetSaveLocation + OpenQA.Selenium.Keys.Enter);

            // Locate the saved test file
            WindowsExplorerSession.FindElementByAccessibilityId("SearchEditBox").SendKeys(TestFileName);

            // Delete the located saved test file
            Thread.Sleep(1000); // Wait for 1 second
            var shellFolderView = WindowsExplorerSession.FindElementByName("Shell Folder View");
            var targetFileItem = shellFolderView.FindElementByName("NotepadTestOutputFile.txt");
            Assert.IsNotNull(targetFileItem);
            WindowsExplorerSession.Mouse.Click(targetFileItem.Coordinates);
            WindowsExplorerSession.Keyboard.SendKeys(OpenQA.Selenium.Keys.Delete);
            WindowsExplorerSession.Keyboard.SendKeys(OpenQA.Selenium.Keys.Escape);

            WindowsExplorerSession.Quit();
            WindowsExplorerSession = null;
            DesktopSession.Quit();
            DesktopSession = null;
        }
    }
}

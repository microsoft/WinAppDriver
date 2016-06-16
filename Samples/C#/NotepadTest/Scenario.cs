using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Appium.iOS;

namespace NotepadTest
{
    [TestClass]
    public class Scenario
    {
        protected const string AppDriverUrl = "http://127.0.0.1:4723";
        protected const string TextValue = "This is an automated test on Classic Windows Application!\n";
        protected const string NotepadAppId = @"C:\Windows\System32\notepad.exe";
        protected const string ExplorerAppId = @"C:\Windows\System32\explorer.exe";
        protected const string TestFileName = "NotepadTestOutputFile.txt";
        protected static IOSDriver<RemoteWebElement> NotepadSession;


        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            // Launch Notepad Classic Windows Application
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", NotepadAppId);
            NotepadSession = new IOSDriver<RemoteWebElement>(new Uri(AppDriverUrl), appCapabilities);
            Assert.IsNotNull(NotepadSession);
        }

        [TestMethod]
        public void BasicScenario()
        {
            // Verify that Notepad is started with untitled new file
            Assert.AreEqual("Untitled - Notepad", NotepadSession.Title);

            EnterText();
            SaveFile();
            System.Threading.Thread.Sleep(1000); // Wait for 1 second until the window is updated

            // Verify that Notepad has saved the edited text file with the given name
            Assert.AreEqual(TestFileName + " - Notepad", NotepadSession.Title);
        }

        public void EnterText()
        {
            // Enter text into the edit field
            RemoteWebElement editBox = NotepadSession.FindElementByClassName("Edit") as RemoteWebElement;
            editBox.SendKeys(TextValue);

            // Enter TimeStamp
            NotepadSession.FindElementByName("Edit").Click();
            NotepadSession.FindElementByName("Time/Date").Click();
        }

        public void SaveFile()
        {
            NotepadSession.FindElementByName("File").Click();
            NotepadSession.FindElementByName("Save As...").Click();

            System.Threading.Thread.Sleep(1000); // Wait for 1 second until the dialog comes up
            var fileNameBox = NotepadSession.FindElementByAccessibilityId("FileNameControlHost") as IOSElement;
            var fileNameEdit = fileNameBox.FindElementByAccessibilityId("1001"); // File name edit box in the Save As dialog
            fileNameBox.SendKeys(@"\" + TestFileName);
            NotepadSession.FindElementByName("Save").Click();

            try
            {
                System.Threading.Thread.Sleep(1000); // Wait for 1 second until the dialog comes up
                var confirmSaveAsDialog = NotepadSession.FindElementByName("Confirm Save As") as IOSElement;
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

            // Launch Windows Explorer
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", ExplorerAppId);
            IOSDriver<RemoteWebElement> WindowsExplorerSession = new IOSDriver<RemoteWebElement>(new Uri(AppDriverUrl), appCapabilities);
            Assert.IsNotNull(WindowsExplorerSession);

            // Desktop session to control context menu and access dialogs
            DesiredCapabilities desktopCapabilities = new DesiredCapabilities();
            desktopCapabilities.SetCapability("app", "Root");
            IOSDriver<RemoteWebElement> DesktopSession = new IOSDriver<RemoteWebElement>(new Uri(AppDriverUrl), desktopCapabilities);
            Assert.IsNotNull(DesktopSession);

            // Switch to root folder
            var addressBandRoot = WindowsExplorerSession.FindElementByClassName("Address Band Root") as IOSElement;
            var addressToolbar = addressBandRoot.FindElementByAccessibilityId("1001"); // Address Band Toolbar
            WindowsExplorerSession.Mouse.Click(addressToolbar.Coordinates);
            addressBandRoot.FindElementByAccessibilityId("41477").SendKeys(@"\");
            var gotoButton = addressBandRoot.FindElementByName("Go to \"\\\"");
            WindowsExplorerSession.Mouse.Click(gotoButton.Coordinates);

            // Locate the saved test file
            WindowsExplorerSession.FindElementByAccessibilityId("SearchEditBox").SendKeys(TestFileName);

            // Delete the located saved test file
            System.Threading.Thread.Sleep(1000); // Wait for 1 second
            var shellFolderView = WindowsExplorerSession.FindElementByName("Shell Folder View") as IOSElement;
            var targetFileItem = shellFolderView.FindElementByName("NotepadTestOutputFile.txt");
            Assert.IsNotNull(targetFileItem);
            WindowsExplorerSession.Mouse.ContextClick(targetFileItem.Coordinates);

            System.Threading.Thread.Sleep(1000); // Wait for 1 second for the context menu to appear
            var contextMenu = DesktopSession.FindElementByName("Context") as IOSElement;
            Assert.IsNotNull(contextMenu);
            contextMenu.FindElementByAccessibilityId("30994").Click(); // Select Delete from the context menu item

            try
            {
                System.Threading.Thread.Sleep(1000); // Wait for 1 second for the dialog to appear
                var accessDeniedDialog = DesktopSession.FindElementByName("File Access Denied");
                accessDeniedDialog.FindElementByName("Continue with automatic administrator rights").Click();
            }
            catch { }

            WindowsExplorerSession.Quit();
            WindowsExplorerSession = null;
            DesktopSession.Quit();
            DesktopSession = null;
        }

    }
}

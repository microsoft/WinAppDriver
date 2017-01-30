using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Appium.Windows;
using System.Collections.Generic;

namespace NotepadTest
{
    [TestClass]
    public class AdvancedScenario
    {
        protected const string AppDriverUrl = "http://127.0.0.1:4723";
        protected const string NotepadAppId = @"C:\Windows\System32\notepad.exe";
        protected const string ExplorerAppId = @"C:\Windows\System32\explorer.exe";
        protected const string TargetSaveLocation = @"%TEMP%\";


        protected static WindowsDriver<WindowsElement> NotepadSession;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            // Launch Notepad Classic Windows Application
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", NotepadAppId);
            NotepadSession = new WindowsDriver<WindowsElement>(new Uri(AppDriverUrl), appCapabilities);
            Assert.IsNotNull(NotepadSession);
            // Verify that Notepad is started with untitled new file
            Assert.AreEqual("Untitled - Notepad", NotepadSession.Title);

            //Change the font to be more interesting
            NotepadSession.FindElementByName("Format").Click();
            NotepadSession.FindElementByName("Font...").Click();
            System.Threading.Thread.Sleep(1000);
            NotepadSession.FindElementByName("Cooper").Click();
            NotepadSession.FindElementByName("Black").Click();

            //Change the font size to be really big
            NotepadSession.FindElementByName("72").Click();

            //close the font dialog
            NotepadSession.FindElementByName("OK").Click();

            Thread.Sleep(1000);// Wait for 1 second until the main windows is displayed
            //Enable word wrap
            NotepadSession.FindElementByName("Format").Click();
            NotepadSession.FindElementByName("Word Wrap").Click();

        }


        [DataTestMethod]
        [DataRow("This is the first advanced automated test on Classic Windows Application! ",  "NotepadAdvancedTestOutputFile1.txt")]
        [DataRow("This is the second advanced automated test on Classic Windows Application! ", "NotepadAdvancedTestOutputFile2.txt")]
        [DataRow("This is the third advanced automated test on Classic Windows Application! ",  "NotepadAdvancedTestOutputFile3.txt")]
        [DataRow("This is the fourth advanced automated test on Classic Windows Application! ", "NotepadAdvancedTestOutputFile4.txt")]
        [DataRow("This is the fifth advanced automated test on Classic Windows Application! ",  "NotepadAdvancedTestOutputFile5.txt")]
        public void AdvancedScenarioTest(string AdvancedText,string TestFileName)
        {
            EnterText(AdvancedText);
            SaveFile(TestFileName);

            // Verify that Notepad has saved the edited text file with the given name
            Thread.Sleep(1000); // Wait for 1 second until the window title is updated
            Assert.AreEqual(TestFileName + " - Notepad", NotepadSession.Title);
            ClearText();
        }


        public void EnterText(string inputtext)
        {
            // Enter text into the edit field
            var editBox = NotepadSession.FindElementByClassName("Edit");
            // Clear the edit field of any values
            editBox.SendKeys(inputtext);
            Assert.AreEqual(inputtext, editBox.Text);

            // Enter TimeStamp
            NotepadSession.FindElementByName("Edit").Click();
            NotepadSession.FindElementByName("Time/Date").Click();
            Assert.AreNotEqual(inputtext, editBox.Text);
        }

        public void SaveFile(string testfilename)
        {
            NotepadSession.FindElementByName("File").Click();
            NotepadSession.FindElementByName("Save As...").Click();

            System.Threading.Thread.Sleep(1000); // Wait for 1 second until the save dialog appears
            var fileNameBox = NotepadSession.FindElementByAccessibilityId("FileNameControlHost");
            fileNameBox.SendKeys(TargetSaveLocation + testfilename);
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

        public void ClearText()
        {
            NotepadSession.FindElementByName("Edit").Click();
            NotepadSession.FindElementByName("Select All").Click();
            NotepadSession.FindElementByName("Edit").Click();
            NotepadSession.FindElementByName("Delete").Click();
        }


        [ClassCleanup]
        public static void TearDown()
        {
            List<string> filelist = new List<string>();
            filelist.Add("NotepadAdvancedTestOutputFile1.txt");
            filelist.Add("NotepadAdvancedTestOutputFile2.txt");
            filelist.Add("NotepadAdvancedTestOutputFile3.txt");
            filelist.Add("NotepadAdvancedTestOutputFile4.txt");
            filelist.Add("NotepadAdvancedTestOutputFile5.txt");


            //Change the font back to the default
            NotepadSession.FindElementByName("Format").Click();

            System.Threading.Thread.Sleep(1000);// Wait for 1 second until the dialog comes up
            NotepadSession.FindElementByName("Font...").Click();

            //Change the font size back to the default Lucida Console
            NotepadSession.FindElementByName("Lucida Console").Click();

            //Change the font format back to the default regular
            NotepadSession.FindElementByName("Regular").Click();

            //Change the font size back to the default size
            NotepadSession.FindElementByName("10").Click();

            //close the font dialog
            NotepadSession.FindElementByName("OK").Click();

            //Disable word wrap
            NotepadSession.FindElementByName("Format").Click();
            NotepadSession.FindElementByName("Word Wrap").Click();
            
            // Close Notepad
            NotepadSession.FindElementByName("File").Click();
            NotepadSession.FindElementByName("Exit").Click();
           System.Threading.Thread.Sleep(1000);// Wait for 1 second until the dialog comes up
            NotepadSession.FindElementByName("Don't Save").Click();

            NotepadSession = null;

            // Launch Windows Explorer to delete the saved text file above
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", ExplorerAppId);
            WindowsDriver<WindowsElement> WindowsExplorerSession = new WindowsDriver<WindowsElement>(new Uri(AppDriverUrl), appCapabilities);
            Assert.IsNotNull(WindowsExplorerSession);

            // Create Desktop session to control context menu and access dialogs
            DesiredCapabilities desktopCapabilities = new DesiredCapabilities();
            desktopCapabilities.SetCapability("app", "Root");
            WindowsDriver<WindowsElement> DesktopSession = new WindowsDriver<WindowsElement>(new Uri(AppDriverUrl), desktopCapabilities);
            Assert.IsNotNull(DesktopSession);

            // Navigate Windows Explorer to the target save location folder
            Thread.Sleep(1000); // Wait for 1 second
            var addressBandRoot = WindowsExplorerSession.FindElementByClassName("Address Band Root");
            var addressToolbar = addressBandRoot.FindElementByAccessibilityId("1001"); // Address Band Toolbar
            WindowsExplorerSession.Mouse.Click(addressToolbar.Coordinates);
            addressBandRoot.FindElementByAccessibilityId("41477").SendKeys(TargetSaveLocation + OpenQA.Selenium.Keys.Enter);

            // Locate the saved test files

            foreach (string file in filelist)
            {
              WindowsExplorerSession.FindElementByAccessibilityId("SearchEditBox").SendKeys(file);

                // Delete the located saved test file
                Thread.Sleep(1000); // Wait for 1 second
                var shellFolderView = WindowsExplorerSession.FindElementByName("Shell Folder View");
                var targetFileItem = shellFolderView.FindElementByName(file);
                Assert.IsNotNull(targetFileItem);
                WindowsExplorerSession.Mouse.Click(targetFileItem.Coordinates);
                WindowsExplorerSession.Keyboard.SendKeys(OpenQA.Selenium.Keys.Delete);
                WindowsExplorerSession.Keyboard.SendKeys(OpenQA.Selenium.Keys.Escape);
            }

            WindowsExplorerSession.Quit();
            WindowsExplorerSession = null;
            DesktopSession.Quit();
            DesktopSession = null;
        }
    }
}

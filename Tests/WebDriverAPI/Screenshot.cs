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
using System.Drawing;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;

namespace WebDriverAPI
{
    [TestClass]
    public class Screenshot : AlarmClockBase
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
        public void GetElementScreenshot()
        {
            WindowsDriver<WindowsElement> desktopSession = null;

            try
            {
                // Locate the AlarmPivotItem element in Alarms & Clock app to be captured
                WindowsElement alarmPivotItem1 = session.FindElementByAccessibilityId(AlarmTabAutomationId);
                OpenQA.Selenium.Screenshot alarmPivotItemScreenshot1 = alarmPivotItem1.GetScreenshot();               

                // Save the AlarmPivotItem screenshot capture locally on the machine running the test
                alarmPivotItemScreenshot1.SaveAsFile(@"ScreenshotAlarmPivotItem.png", ScreenshotImageFormat.Png);

                // Using the Desktop session, locate the same AlarmPivotItem element in Alarms & Clock app to be captured
                desktopSession = Utility.CreateNewSession(CommonTestSettings.DesktopAppId);
                WindowsElement alarmPivotItem2 = desktopSession.FindElementByAccessibilityId(AlarmTabAutomationId);
                OpenQA.Selenium.Screenshot alarmPivotItemScreenshot2 = alarmPivotItem2.GetScreenshot();

                // Using the Desktop session, locate the Alarms & Clock app top level window to be captured
                WindowsElement alarmsClockWindowTopWindow = desktopSession.FindElementByName("Alarms & Clock");
                OpenQA.Selenium.Screenshot alarmsClockWindowTopWindowScreenshot = alarmsClockWindowTopWindow.GetScreenshot();

                using (MemoryStream msScreenshot1 = new MemoryStream(alarmPivotItemScreenshot1.AsByteArray))
                using (MemoryStream msScreenshot2 = new MemoryStream(alarmPivotItemScreenshot2.AsByteArray))
                using (MemoryStream msScreenshot3 = new MemoryStream(alarmsClockWindowTopWindowScreenshot.AsByteArray))
                {
                    // Verify that the element screenshot has a valid size
                    Image screenshotImage1 = Image.FromStream(msScreenshot1);
                    Assert.AreEqual(alarmPivotItem1.Size.Height, screenshotImage1.Height);
                    Assert.AreEqual(alarmPivotItem1.Size.Width, screenshotImage1.Width);

                    // Verify that the element screenshot captured using the Alarms & Clock session
                    // is identical in size with the one taken using the desktop session
                    Image screenshotImage2 = Image.FromStream(msScreenshot2);
                    Assert.AreEqual(screenshotImage1.Height, screenshotImage2.Height);
                    Assert.AreEqual(screenshotImage1.Width, screenshotImage2.Width);

                    // Verify that the element screenshot is smaller in size compared to the application top level window
                    Image screenshotImage3 = Image.FromStream(msScreenshot3);
                    Assert.AreEqual(alarmsClockWindowTopWindow.Size.Height, screenshotImage3.Height);
                    Assert.AreEqual(alarmsClockWindowTopWindow.Size.Width, screenshotImage3.Width);
                    Assert.IsTrue(screenshotImage3.Height > screenshotImage1.Height);
                    Assert.IsTrue(screenshotImage3.Width > screenshotImage1.Width);
                }
            }
            finally
            {
                if (desktopSession != null)
                {
                    desktopSession.Quit();
                }
            }
        }

        [TestMethod]
        public void GetElementScreenshotError_NoSuchWindow()
        {
            try
            {
                var screenshot = Utility.GetOrphanedElement().GetScreenshot();
                Assert.Fail("Exception should have been thrown because there is no such window");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }

        [TestMethod]
        public void GetElementScreenshotError_StaleElement()
        {
            try
            {
                var screenshot = GetStaleElement().GetScreenshot();
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.StaleElementReference, exception.Message);
            }
        }

        [TestMethod]
        public void GetScreenshot()
        {
            WindowsDriver<WindowsElement> notepadSession = null;
            WindowsDriver<WindowsElement> desktopSession = null;

            try
            {
                // Launch and capture a screenshot of a maximized Notepad application. The steps below intentionally use
                // the Notepad application window to fully cover the Alarms & Clock application. This setup demonstrates
                // that capturing Alarms & Clock screenshot afterward will implicitly bring its window to foreground.
                notepadSession = Utility.CreateNewSession(CommonTestSettings.NotepadAppId);
                notepadSession.Manage().Window.Maximize();
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                OpenQA.Selenium.Screenshot notepadScreenshot = notepadSession.GetScreenshot();

                // Capture a screenshot of Alarms & Clock application
                // This implicitly brings the application window to foreground
                OpenQA.Selenium.Screenshot alarmsClockScreenshot = session.GetScreenshot();

                // Save the application screenshot capture locally on the machine running the test
                alarmsClockScreenshot.SaveAsFile(@"ScreenshotAlarmsClockApplication.png", ScreenshotImageFormat.Png);

                // Capture the entire desktop using the Desktop session
                desktopSession = Utility.CreateNewSession(CommonTestSettings.DesktopAppId);
                OpenQA.Selenium.Screenshot desktopScreenshot = desktopSession.GetScreenshot();

                // Save the desktop screenshot capture locally on the machine running the test
                desktopScreenshot.SaveAsFile(@"ScreenshotDesktop.png", ScreenshotImageFormat.Png);

                using (MemoryStream msScreenshot1 = new MemoryStream(alarmsClockScreenshot.AsByteArray))
                using (MemoryStream msScreenshot2 = new MemoryStream(notepadScreenshot.AsByteArray))
                using (MemoryStream msScreenshot3 = new MemoryStream(desktopScreenshot.AsByteArray))
                {
                    // Verify that the Alarms & Clock application screenshot has a valid size
                    Image screenshotImage1 = Image.FromStream(msScreenshot1);
                    Assert.AreEqual(session.Manage().Window.Size.Height, screenshotImage1.Height);
                    Assert.AreEqual(session.Manage().Window.Size.Width, screenshotImage1.Width);

                    // Verify that the maximized Notepad application screenshot has a valid size
                    Image screenshotImage2 = Image.FromStream(msScreenshot2);
                    Assert.AreEqual(notepadSession.Manage().Window.Size.Height, screenshotImage2.Height);
                    Assert.AreEqual(notepadSession.Manage().Window.Size.Width, screenshotImage2.Width);

                    // Verify that the application screenshot is smaller in size compared to the entire desktop
                    Image screenshotImage3 = Image.FromStream(msScreenshot3);
                    Assert.IsTrue(screenshotImage2.Height >= screenshotImage1.Height);
                    Assert.IsTrue(screenshotImage2.Width >= screenshotImage1.Width);
                }
            }
            finally
            {
                if (notepadSession != null)
                {
                    notepadSession.Quit();
                }

                if (desktopSession != null)
                {
                    desktopSession.Quit();
                }
            }
        }

        [TestMethod]
        public void GetScreenshotError_NoSuchWindow()
        {
            try
            {
                Utility.GetOrphanedSession().GetScreenshot();
                Assert.Fail("Exception should have been thrown because there is no such window");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }
    }
}

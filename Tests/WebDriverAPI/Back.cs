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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Threading;

namespace WebDriverAPI
{
    [TestClass]
    public class Back
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
        public void NavigateBack_Browser()
        {
            session = Utility.CreateNewSession(CommonTestSettings.EdgeAppId, "-private " + CommonTestSettings.EdgeAboutFlagsURL);
            session.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
            Thread.Sleep(TimeSpan.FromSeconds(2.5));
            var originalTitle = session.Title;
            Assert.AreNotEqual(string.Empty, originalTitle);

            // Navigate to different URLs
            session.FindElementByAccessibilityId("addressEditBox").SendKeys(Keys.Alt + 'd' + Keys.Alt + CommonTestSettings.EdgeAboutBlankURL + Keys.Enter);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Assert.AreNotEqual(originalTitle, session.Title);

            // Navigate back to original URL
            session.Navigate().Back();
            Thread.Sleep(TimeSpan.FromSeconds(3));
            Assert.AreEqual(originalTitle, session.Title);
            EdgeBase.CloseEdge(session);
        }

        [TestMethod]
        public void NavigateBack_ModernApp()
        {
            session = Utility.CreateNewSession(CommonTestSettings.AlarmClockAppId);
            session.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);

            // Ensure alarms & clock are in Alarm Pivot view
            session.Navigate().Back();
            session.DismissAlarmDialogIfThere();
            try
            {
                session.FindElementByAccessibilityId("AlarmButton").Click();
            }
            catch (InvalidOperationException)
            {
                session.FindElementByAccessibilityId("AlarmPivotItem").Click();
            }

            // Navigate to New Alarm view
            session.FindElementByAccessibilityId("AddAlarmButton").Click();
            Assert.IsNotNull(session.FindElementByAccessibilityId("EditAlarmHeader"));

            // Navigate back to the original view
            session.Navigate().Back();
            session.DismissAlarmDialogIfThere();
            Assert.IsNotNull(session.FindElementByAccessibilityId("AddAlarmButton"));
        }

        [TestMethod]
        public void NavigateBack_SystemApp()
        {
            session = Utility.CreateNewSession(CommonTestSettings.ExplorerAppId);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            var originalTitle = session.Title;
            Assert.AreNotEqual(string.Empty, originalTitle);

            // Navigate Windows Explorer to change folder
            session.Keyboard.SendKeys(Keys.Alt + "d" + Keys.Alt + CommonTestSettings.TestFolderLocation + Keys.Enter);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Assert.AreNotEqual(originalTitle, session.Title);

            // Navigate back to the original folder
            session.Navigate().Back();
            Assert.AreEqual(originalTitle, session.Title);
        }

        [TestMethod]
        public void NavigateBackError_NoSuchWindow()
        {
            try
            {
                Utility.GetOrphanedSession().Navigate().Back();
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }
    }
}

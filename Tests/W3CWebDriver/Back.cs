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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Remote;

namespace W3CWebDriver
{
    [TestClass]
    public class Back
    {
        private IOSDriver<IOSElement> session = null;

        [TestMethod]
        public void NavigateBackBrowser()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.EdgeAppId);
            session = new IOSDriver<IOSElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);

            session.FindElementByAccessibilityId("addressEditBox").SendKeys(CommonTestSettings.MicrosoftUrl + OpenQA.Selenium.Keys.Enter);
            System.Threading.Thread.Sleep(3000); // Sleep for 3 seconds
            var originalTitle = session.Title;
            Assert.AreNotEqual(String.Empty, originalTitle);

            // Navigate to different URLs
            var addressEditBox = session.FindElementByAccessibilityId("addressEditBox");
            addressEditBox.SendKeys(CommonTestSettings.GitHubUrl + OpenQA.Selenium.Keys.Enter);

            System.Threading.Thread.Sleep(3000); // Sleep for 3 seconds
            Assert.AreNotEqual(originalTitle, session.Title);

            // Navigate back to original URL
            session.Navigate().Back();
            System.Threading.Thread.Sleep(1000); // Sleep for 1 second
            Assert.AreEqual(originalTitle, session.Title);

            session.Quit();
            session = null;
        }

        [TestMethod]
        public void NavigateBackModernApp()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.AlarmClockAppId);
            session = new IOSDriver<IOSElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);

            // Navigate to New Alarm view
            session.FindElementByAccessibilityId("AlarmPivotItem").Click();
            session.FindElementByAccessibilityId("AddAlarmButton").Click();
            Assert.IsNotNull(session.FindElementByAccessibilityId("EditAlarmHeader"));

            // Navigate back to the original view
            session.Navigate().Back();
            Assert.IsNotNull(session.FindElementByAccessibilityId("AlarmPivotItem"));

            session.Quit();
            session = null;
        }

        [TestMethod]
        public void NavigateBackSystemApp()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.ExplorerAppId);
            session = new IOSDriver<IOSElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);

            System.Threading.Thread.Sleep(1000); // Sleep for 1 second
            var originalTitle = session.Title;
            Assert.AreNotEqual(String.Empty, originalTitle);

            // Navigate Windows Explorer to change folder
            var targetLocation = @"%TEMP%\";
            var addressBandRoot = session.FindElementByClassName("Address Band Root");
            var addressToolbar = addressBandRoot.FindElementByAccessibilityId("1001"); // Address Band Toolbar
            session.Mouse.Click(addressToolbar.Coordinates);
            addressBandRoot.FindElementByAccessibilityId("41477").SendKeys(targetLocation + OpenQA.Selenium.Keys.Enter);
            System.Threading.Thread.Sleep(1000); // Sleep for 1 second
            Assert.AreNotEqual(originalTitle, session.Title);

            // Navigate back to the original folder
            session.Navigate().Back();
            Assert.AreEqual(originalTitle, session.Title);

            session.Quit();
            session = null;
        }

        [TestMethod]
        public void ErrorNavigateBackNoSuchWindow()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.AlarmClockAppId);
            session = new IOSDriver<IOSElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);

            try
            {
                session.Close();
                session.Navigate().Back();
                Assert.Fail("Exception should have been thrown");
            }
            catch (System.InvalidOperationException exception)
            {
                Assert.AreEqual("Currently selected window has been closed", exception.Message);
            }

            session.Quit();
            session = null;
        }
    }
}

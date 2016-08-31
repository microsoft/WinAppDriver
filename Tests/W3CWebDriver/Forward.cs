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
    public class Forward : TestBase
    {
        [TestMethod]
        public void NavigateForwardBrowser()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.EdgeAppId);
            session = new IOSDriver<IOSElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);

            var originalTitle = session.Title;
            Assert.AreNotEqual(String.Empty, originalTitle);

            // Navigate to different URLs
            var addressEditBox = session.FindElementByAccessibilityId("addressEditBox");
            addressEditBox.SendKeys("https://github.com/Microsoft/WinAppDriver");
            session.FindElementByAccessibilityId("m_newTabPageGoButton").Click();

            System.Threading.Thread.Sleep(3000); // Sleep for 3 seconds
            var newTitle = session.Title;
            Assert.AreNotEqual(originalTitle, newTitle);

            // Navigate back to original URL
            session.Navigate().Back();
            System.Threading.Thread.Sleep(1000); // Sleep for 1 second
            Assert.AreEqual(originalTitle, session.Title);

            // Navigate forward to original URL
            session.FindElementByName("Microsoft Edge").Click(); // Set focus on the Microsoft Edge window
            session.Navigate().Forward();
            System.Threading.Thread.Sleep(3000); // Sleep for 3 seconds
            Assert.AreEqual(newTitle, session.Title);

            session.Quit();
        }

        [TestMethod]
        public void NavigateForwardSystemApp()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.ExplorerAppId);
            session = new IOSDriver<IOSElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);

            var originalTitle = session.Title;
            //Assert.AreNotEqual(String.Empty, originalTitle);

            // Navigate Windows Explorer to change folder
            var targetLocation = @"%TEMP%\";
            var addressBandRoot = session.FindElementByClassName("Address Band Root");
            var addressToolbar = addressBandRoot.FindElementByAccessibilityId("1001"); // Address Band Toolbar
            session.Mouse.Click(addressToolbar.Coordinates);
            addressBandRoot.FindElementByAccessibilityId("41477").SendKeys(targetLocation);
            var gotoButton = addressBandRoot.FindElementByName("Go to \"" + targetLocation + "\"");
            session.Mouse.Click(gotoButton.Coordinates);

            System.Threading.Thread.Sleep(1000); // Sleep for 1 second
            var newTitle = session.Title;
            Assert.AreNotEqual(originalTitle, newTitle);

            // Navigate back to the original folder
            session.Navigate().Back();
            Assert.AreEqual(originalTitle, session.Title);

            // Navigate forward to the target folder
            session.Navigate().Forward();
            Assert.AreEqual(newTitle, session.Title);

            session.Quit();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ErrorNavigateForwardNoSuchWindow()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.AlarmClockAppId);
            session = new IOSDriver<IOSElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);

            session.Close();
            session.Navigate().Forward();
            Assert.Fail("Exception should have been thrown");
        }
    }
}

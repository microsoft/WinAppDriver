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
        [TestMethod]
        public void NavigateBackBrowser()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.EdgeAppId);
            IOSDriver<IOSElement> browserSession = new IOSDriver<IOSElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(browserSession);

            var originalTitle = browserSession.Title;
            Assert.AreNotEqual(String.Empty, originalTitle);

            // Navigate to different URLs
            var addressEditBox = browserSession.FindElementByAccessibilityId("addressEditBox");
            addressEditBox.SendKeys("https://github.com/Microsoft/WinAppDriver");
            browserSession.FindElementByAccessibilityId("m_newTabPageGoButton").Click();

            System.Threading.Thread.Sleep(3000); // Sleep for 3 seconds
            Assert.AreNotEqual(originalTitle, browserSession.Title);

            // Navigate back to original URL
            browserSession.Navigate().Back();
            System.Threading.Thread.Sleep(1000); // Sleep for 1 second
            Assert.AreEqual(originalTitle, browserSession.Title);

            browserSession.Quit();
        }

        [TestMethod]
        public void NavigateBackModernApp()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.AlarmClockAppId);
            IOSDriver<IOSElement> appSession = new IOSDriver<IOSElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(appSession);

            // Navigate to New Alarm view
            appSession.FindElementByAccessibilityId("AlarmPivotItem").Click();
            appSession.FindElementByAccessibilityId("AddAlarmButton").Click();
            Assert.IsNotNull(appSession.FindElementByAccessibilityId("EditAlarmHeader"));

            // Navigate back to the original view
            appSession.Navigate().Back();
            Assert.IsNotNull(appSession.FindElementByAccessibilityId("AlarmPivotItem"));

            appSession.Quit();
        }

        [TestMethod]
        public void NavigateBackSystemApp()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.ExplorerAppId);
            IOSDriver<IOSElement> explorerSession = new IOSDriver<IOSElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(explorerSession);

            var originalTitle = explorerSession.Title;
            Assert.AreNotEqual(String.Empty, originalTitle);

            // Navigate Windows Explorer to change folder
            var targetLocation = @"%TEMP%\";
            var addressBandRoot = explorerSession.FindElementByClassName("Address Band Root");
            var addressToolbar = addressBandRoot.FindElementByAccessibilityId("1001"); // Address Band Toolbar
            explorerSession.Mouse.Click(addressToolbar.Coordinates);
            addressBandRoot.FindElementByAccessibilityId("41477").SendKeys(targetLocation);
            var gotoButton = addressBandRoot.FindElementByName("Go to \"" + targetLocation + "\"");
            explorerSession.Mouse.Click(gotoButton.Coordinates);

            System.Threading.Thread.Sleep(1000); // Sleep for 1 second
            Assert.AreNotEqual(originalTitle, explorerSession.Title);

            // Navigate back to the original folder
            explorerSession.Navigate().Back();
            Assert.AreEqual(originalTitle, explorerSession.Title);

            explorerSession.Quit();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ErrorNavigateBackNoSuchWindow()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.AlarmClockAppId);
            IOSDriver<IOSElement> session = new IOSDriver<IOSElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);

            session.Close();
            session.Navigate().Back();
            Assert.Fail("Exception should have been thrown");
        }
    }
}

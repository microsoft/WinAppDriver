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
using OpenQA.Selenium.Appium.Windows;

namespace W3CWebDriver
{
    [TestClass]
    public class Forward
    {
        private WindowsDriver<WindowsElement> session = null;

        [TestMethod]
        public void NavigateForwardBrowser()
        {
            session = Utility.CreateNewSession(CommonTestSettings.EdgeAppId);
            Assert.IsNotNull(session);
            System.Threading.Thread.Sleep(3000); // Sleep for 3 seconds

            session.FindElementByAccessibilityId("addressEditBox").SendKeys(CommonTestSettings.MicrosoftUrl + OpenQA.Selenium.Keys.Enter);
            System.Threading.Thread.Sleep(3000); // Sleep for 3 seconds
            var originalTitle = session.Title;
            Assert.AreNotEqual(String.Empty, originalTitle);

            // Navigate to different URLs
            session.FindElementByAccessibilityId("addressEditBox").SendKeys(CommonTestSettings.GitHubUrl + OpenQA.Selenium.Keys.Enter);
            System.Threading.Thread.Sleep(3000); // Sleep for 3 seconds
            var newTitle = session.Title;
            Assert.AreNotEqual(originalTitle, newTitle);

            // Navigate back to original URL
            session.Navigate().Back();
            System.Threading.Thread.Sleep(5000); // Sleep for 5 seconds
            Assert.AreEqual(originalTitle, session.Title);

            // Navigate forward to original URL
            session.Navigate().Forward();
            System.Threading.Thread.Sleep(5000); // Sleep for 5 seconds
            Assert.AreEqual(newTitle, session.Title);

            session.Quit();
        }

        [TestMethod]
        public void NavigateForwardSystemApp()
        {
            session = Utility.CreateNewSession(CommonTestSettings.ExplorerAppId);
            Assert.IsNotNull(session);

            var originalTitle = session.Title;
            //Assert.AreNotEqual(String.Empty, originalTitle);

            // Navigate Windows Explorer to change folder
            var targetLocation = @"%TEMP%\";
            var addressBandRoot = session.FindElementByClassName("Address Band Root");
            var addressToolbar = addressBandRoot.FindElementByAccessibilityId("1001"); // Address Band Toolbar
            session.Mouse.Click(addressToolbar.Coordinates);
            addressBandRoot.FindElementByAccessibilityId("41477").SendKeys(targetLocation + OpenQA.Selenium.Keys.Enter);
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
        public void ErrorNavigateForwardNoSuchWindow()
        {
            try
            {
                Utility.GetOrphanedSession().Navigate().Forward();
                Assert.Fail("Exception should have been thrown");
            }
            catch (System.InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }
    }
}

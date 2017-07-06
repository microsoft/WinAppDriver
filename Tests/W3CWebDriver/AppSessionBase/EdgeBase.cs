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
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;

namespace W3CWebDriver
{
    public class EdgeBase
    {
        protected static WindowsDriver<WindowsElement> session;
        protected static RemoteTouchScreen touchScreen;
        protected static string startingPageTitle = string.Empty;
        private const int maxNavigationHistory = 5;

        public static void Setup(TestContext context)
        {
            // Launch Edge browser app if it is not yet launched
            if (session == null || touchScreen == null || !Utility.CurrentWindowIsAlive(session))
            {
                // Cleanup leftover objects from previous test if exists
                TearDown();

                // Launch the Edge browser app
                session = Utility.CreateNewSession(CommonTestSettings.EdgeAppId);
                session.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
                Assert.IsNotNull(session);
                Assert.IsNotNull(session.SessionId);

                // Initialize touch screen object
                touchScreen = new RemoteTouchScreen(session);
                Assert.IsNotNull(touchScreen);
            }

            // Track the Microsoft Edge starting page title to be used to initialize all test cases
            System.Threading.Thread.Sleep(3000); // Sleep for 3 seconds
            startingPageTitle = session.Title;

            // Handle Microsoft Edge restored state by starting fresh
            if (startingPageTitle.StartsWith("Start fresh and "))
            {
                try
                {
                    session.FindElementByXPath("//Button[@Name='Start fresh']").Click();
                    System.Threading.Thread.Sleep(3000); // Sleep for 3 seconds
                    startingPageTitle = session.Title;
                }
                catch { }
            }
        }

        public static void TearDown()
        {
            // Cleanup RemoteTouchScreen object if initialized
            touchScreen = null;

            // Close the application and delete the session
            if (session != null)
            {
                try
                {
                    session.Close();
                    var currentHandle = session.CurrentWindowHandle; // This should throw if the window is closed successfully

                    // When the Edge window remains open because of multiple tabs are open, attempt to close modal dialog
                    var closeAllButton = session.FindElementByXPath("//Button[@Name='Close all']");
                    closeAllButton.Click();

                }
                catch { }

                session.Quit();
                session = null;
            }
        }

        [TestInitialize]
        public void TestInit()
        {
            // Restore Microsoft Edge to the main page by navigating the browser back in history
            for (int attempt = maxNavigationHistory; attempt > 0 && session.Title != startingPageTitle; attempt--)
            {
                session.Navigate().Back();
            }
        }

        protected static RemoteWebElement GetStaleElement()
        {
            RemoteWebElement staleElement = null;

            session.FindElementByAccessibilityId("addressEditBox").SendKeys(CommonTestSettings.EdgeAboutTabsURL + OpenQA.Selenium.Keys.Enter);
            System.Threading.Thread.Sleep(2000); // Sleep for 2 seconds
            var originalTitle = session.Title;
            Assert.AreNotEqual(string.Empty, originalTitle);

            // Navigate to Edge about:flags page
            session.FindElementByAccessibilityId("addressEditBox").SendKeys(CommonTestSettings.EdgeAboutFlagsURL + OpenQA.Selenium.Keys.Enter);
            System.Threading.Thread.Sleep(2000); // Sleep for 2 seconds
            Assert.AreNotEqual(originalTitle, session.Title);

            // Save a reference to Reset all flags button on the page and navigate back to home
            staleElement = session.FindElementByAccessibilityId("ResetAllFlags");
            Assert.IsNotNull(staleElement);
            session.Navigate().Back();
            System.Threading.Thread.Sleep(1000);
            Assert.AreEqual(originalTitle, session.Title);

            return staleElement;
        }
    }
}
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
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Remote;

namespace W3CWebDriver
{
    public class TouchBase
    {
        protected static IOSDriver<IOSElement> session;
        protected static RemoteTouchScreen touchScreen;
        protected static string startingPageTitle = string.Empty;
        private const int maxNavigationHistory = 5;

        public static void Setup(TestContext context)
        {
            // Launch the Edge browser app
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.EdgeAppId);
            session = new IOSDriver<IOSElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);

            // Initialize touch screen object
            touchScreen = new RemoteTouchScreen(session);
            Assert.IsNotNull(touchScreen);

            // Track the Microsoft Edge starting page title to be used to initialize all test cases
            startingPageTitle = session.Title;
            Assert.AreNotEqual(string.Empty, startingPageTitle);
        }

        public static void TearDown()
        {
            // Cleanup RemoteTouchScreen object if initialized
            touchScreen = null;

            // Close the application and delete the session
            if (session != null)
            {
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

        protected RemoteWebElement GetOrphanedElement(IOSDriver<IOSElement> remoteSession)
        {
            RemoteWebElement orphanedElement = null;

            // Track existing opened Edge window(s) and only manipulate newly opened windows
            var previouslyOpenedEdgeWindows = remoteSession.WindowHandles;
            var originalActiveWindowHandle = remoteSession.CurrentWindowHandle;

            // Open a new window
            // The menu item names have changed between Windows 10 and the anniversary update
            // account for both combinations.
            try
            {
                remoteSession.FindElementByAccessibilityId("m_actionsMenuButton").Click();
                remoteSession.FindElementByAccessibilityId("m_newWindow").Click();
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                remoteSession.FindElementByAccessibilityId("ActionsMenuButton").Click();
                remoteSession.FindElementByAccessibilityId("ActionsMenuNewWindow").Click();
            }

            System.Threading.Thread.Sleep(3000); // Sleep for 3 second
            var multipleWindowHandles = remoteSession.WindowHandles;
            Assert.IsTrue(multipleWindowHandles.Count == previouslyOpenedEdgeWindows.Count + 1);

            // Ensure we get the newly opened window by removing other previously known windows from the list
            List<String> newlyOpenedEdgeWindows = new List<String>(multipleWindowHandles);
            foreach (var previouslyOpenedEdgeWindow in previouslyOpenedEdgeWindows)
            {
                newlyOpenedEdgeWindows.Remove(previouslyOpenedEdgeWindow);
            }
            Assert.IsTrue(newlyOpenedEdgeWindows.Count == 1);

            // Switch to new window and use the address edit box as orphaned element
            remoteSession.SwitchTo().Window(newlyOpenedEdgeWindows[0]);
            System.Threading.Thread.Sleep(1000); // Sleep for 1 second
            Assert.AreEqual(newlyOpenedEdgeWindows[0], remoteSession.CurrentWindowHandle);
            orphanedElement = remoteSession.FindElementByAccessibilityId("addressEditBox");
            Assert.IsNotNull(orphanedElement);

            // Close the newly opened window and return to previously active window
            remoteSession.Close();
            remoteSession.SwitchTo().Window(originalActiveWindowHandle);
            System.Threading.Thread.Sleep(1000); // Sleep for 1 second

            return orphanedElement;
        }

        protected static RemoteWebElement GetStaleElement(IOSDriver<IOSElement> remoteSession)
        {
            RemoteWebElement staleElement = null;

            remoteSession.FindElementByAccessibilityId("addressEditBox").SendKeys(CommonTestSettings.MicrosoftGitHubUrl + OpenQA.Selenium.Keys.Enter);
            System.Threading.Thread.Sleep(3000); // Sleep for 3 seconds
            var originalTitle = remoteSession.Title;
            Assert.AreNotEqual(string.Empty, originalTitle);

            // Navigate to GitHub page
            remoteSession.FindElementByAccessibilityId("addressEditBox").SendKeys(CommonTestSettings.WinAppDriverGitHubUrl + OpenQA.Selenium.Keys.Enter);
            System.Threading.Thread.Sleep(3000); // Sleep for 3 seconds
            Assert.AreNotEqual(originalTitle, remoteSession.Title);

            // Save a reference to Homepage web link on the GitHub page and navigate back to home
            staleElement = remoteSession.FindElementByName("Homepage");
            Assert.IsNotNull(staleElement);
            remoteSession.Navigate().Back();
            System.Threading.Thread.Sleep(2000);
            Assert.AreEqual(originalTitle, session.Title);

            return staleElement;
        }

        protected HttpWebResponse SendTouchPost(String touchType, JObject requestObject)
        {
            var request = WebRequest.Create(CommonTestSettings.WindowsApplicationDriverUrl + "/session/" + session.SessionId + "/touch/" + touchType);
            request.Method = "POST";
            request.ContentType = "application/json";

            String postData = requestObject.ToString();
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            return request.GetResponse() as HttpWebResponse;
        }

        public void ErrorTouchInvalidElement(string touchType)
        {
            JObject enterRequestObject = new JObject();
            enterRequestObject["element"] = "InvalidElementId";
            HttpWebResponse response = SendTouchPost(touchType, enterRequestObject);
            Assert.Fail("Exception should have been thrown because there is no such element");
        }

        public void ErrorTouchInvalidArguments(string touchType)
        {
            JObject enterRequestObject = new JObject();
            HttpWebResponse response = SendTouchPost(touchType, enterRequestObject);
            session.Close();
            Assert.Fail("Exception should have been thrown because there are insufficient arguments");
        }
    }
}
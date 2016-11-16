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
using Newtonsoft.Json.Linq;
using System.Net;

namespace W3CWebDriver
{
    [TestClass]
    public class TouchScroll : TouchBase
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
        public void Scroll()
        {
            // Navigate to GitHub
            session.FindElementByAccessibilityId("addressEditBox").SendKeys(CommonTestSettings.GitHubUrl + OpenQA.Selenium.Keys.Enter);
            System.Threading.Thread.Sleep(2000); // Sleep for 2 seconds

            // Use Homepage link in GitHub page as a reference element
            var gitHubHomePageLink = session.FindElementByName("Homepage");
            Assert.IsNotNull(gitHubHomePageLink);
            Assert.IsTrue(gitHubHomePageLink.Displayed);

            // Perform scroll down touch action to scroll the page down hiding the Homepage link element from the view
            touchScreen.Scroll(0, -50);
            System.Threading.Thread.Sleep(3000); // Sleep for 3 seconds
            Assert.IsFalse(gitHubHomePageLink.Displayed);

            // Perform scroll up touch action to scroll the page up restoring the Homepage link element into the view
            touchScreen.Scroll(0, 50);
            System.Threading.Thread.Sleep(3000); // Sleep for 3 seconds
            Assert.IsTrue(gitHubHomePageLink.Displayed);
        }

        [TestMethod]
        public void ScrollOnElementHorizontal()
        {
            session.FindElementByAccessibilityId("addressEditBox").SendKeys(CommonTestSettings.MicrosoftUrl + OpenQA.Selenium.Keys.Enter);
            System.Threading.Thread.Sleep(3000); // Sleep for 3 seconds
            var originalTitle = session.Title;
            Assert.AreNotEqual(string.Empty, originalTitle);

            // Navigate to GitHub page
            session.FindElementByAccessibilityId("addressEditBox").SendKeys(CommonTestSettings.GitHubUrl + OpenQA.Selenium.Keys.Enter);
            System.Threading.Thread.Sleep(3000); // Sleep for 3 seconds
            Assert.AreNotEqual(originalTitle, session.Title);

            // Save application window original size and maximize temporarily
            var originalSize = session.Manage().Window.Size;
            var originalPosition = session.Manage().Window.Position;
            session.Manage().Window.Maximize();
            System.Threading.Thread.Sleep(3000); // Sleep for 3 seconds

            // Locate Microsoft Edge window that scrolls
            var edgeNavScroller = session.FindElementByAccessibilityId("m_navSwipeScroller");
            Assert.IsNotNull(edgeNavScroller);

            // Perform scroll right touch action to go back in browsing history to the original start page
            touchScreen.Scroll(edgeNavScroller.Coordinates, 500, 0);
            System.Threading.Thread.Sleep(3000); // Sleep for 3 seconds
            Assert.AreEqual(originalTitle, session.Title);

            // Perform scroll left touch action to go forward in browsing history to the GitHub page
            touchScreen.Scroll(edgeNavScroller.Coordinates, -500, 0);
            System.Threading.Thread.Sleep(3000); // Sleep for 3 seconds
            Assert.AreNotEqual(originalTitle, session.Title);

            // Restore application window original size and position
            session.Manage().Window.Size = originalSize;
            session.Manage().Window.Position = originalPosition;
        }

        [TestMethod]
        public void ScrollOnElementVertical()
        {
            // Navigate to GitHub
            session.FindElementByAccessibilityId("addressEditBox").SendKeys(CommonTestSettings.GitHubUrl + OpenQA.Selenium.Keys.Enter);
            System.Threading.Thread.Sleep(3000); // Sleep for 3 seconds

            // Use Homepage link in GitHub page as a reference element
            var gitHubHomePageLink = session.FindElementByName("Homepage");
            Assert.IsNotNull(gitHubHomePageLink);
            Assert.IsTrue(gitHubHomePageLink.Displayed);

            // Locate Microsoft Edge window that scrolls
            var edgeNavScroller = session.FindElementByAccessibilityId("m_navSwipeScroller");
            Assert.IsNotNull(edgeNavScroller);

            // Perform scroll down touch action to scroll the page down hiding the Homepage link element from the view
            touchScreen.Scroll(edgeNavScroller.Coordinates, 0, -50);
            System.Threading.Thread.Sleep(3000); // Sleep for 3 seconds
            Assert.IsFalse(gitHubHomePageLink.Displayed);

            // Perform scroll up touch action to scroll the page up restoring the Homepage link element into the view
            touchScreen.Scroll(edgeNavScroller.Coordinates, 0, 50);
            System.Threading.Thread.Sleep(3000); // Sleep for 3 seconds
            Assert.IsTrue(gitHubHomePageLink.Displayed);
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorTouchClosedWindow()
        {
            // Open a new window, retrieve an element, and close the window to get an orphaned element
            var orphanedElement = GetOrphanedElement(session);
            Assert.IsNotNull(orphanedElement);

            // Scroll on the orphaned element
            touchScreen.Scroll(orphanedElement.Coordinates, 0, 50);
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Net.WebException))]
        public void ErrorTouchInvalidElement()
        {
            ErrorTouchInvalidElement("scroll");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorTouchStaleElement()
        {
            // Navigate to a webpage, save a reference to an element, and navigate away to get a stale element
            var staleElement = GetStaleElement(session);
            Assert.IsNotNull(staleElement);

            // Scroll on stale element
            touchScreen.Scroll(staleElement.Coordinates, 0, 50);
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Net.WebException))]
        public void ErrorTouchInvalidArguments()
        {
            ErrorTouchInvalidArguments("scroll");
        }
    }
}

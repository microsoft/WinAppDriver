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

namespace W3CWebDriver
{
    [TestClass]
    public class TouchLongClick : TouchBase
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
        public void LongTap()
        {
            // Get the list of tabs and keep track of how many tabs are open
            System.Threading.Thread.Sleep(1000); // Sleep for 1 second
            var tabsList = session.FindElementByAccessibilityId("TabsList");
            var tabs = tabsList.FindElementsByClassName("GridViewItem");
            var originalTabsCount = tabs.Count;
            Assert.IsTrue(originalTabsCount >= 1);

            // Open a the context menu on the tab using long tap (press and hold) action and click duplicate
            touchScreen.LongPress(tabs[0].Coordinates);
            System.Threading.Thread.Sleep(3000); // Sleep for 3 seconds
            session.FindElementByName("Duplicate").Click();
            System.Threading.Thread.Sleep(1000); // Sleep for 1 second
            Assert.IsTrue(tabsList.FindElementsByClassName("GridViewItem").Count == originalTabsCount + 1);

            // Close all tabs except for the very fist one
            touchScreen.LongPress(tabs[0].Coordinates);
            System.Threading.Thread.Sleep(1000); // Sleep for 1 second
            session.FindElementByName("Close other tabs").Click();
            System.Threading.Thread.Sleep(1000); // Sleep for 1 second
            Assert.IsTrue(tabsList.FindElementsByClassName("GridViewItem").Count == 1);
        }

        [TestMethod]
        [ExpectedException(typeof(OpenQA.Selenium.NoSuchElementException))]
        public void ErrorTouchClosedWindow()
        {
            // Open a new window, retrieve an element, and close the window to get an orphaned element
            var orphanedElement = GetOrphanedElement(session);
            Assert.IsNotNull(orphanedElement);

            // Long press on orphaned element
            touchScreen.LongPress(orphanedElement.Coordinates);
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Net.WebException))]
        public void ErrorTouchInvalidElement()
        {
            ErrorTouchInvalidElement("longclick");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorTouchStaleElement()
        {
            // Navigate to a webpage, save a reference to an element, and navigate away to get a stale element
            var staleElement = GetStaleElement(session);
            Assert.IsNotNull(staleElement);

            // Long press on stale element
            touchScreen.LongPress(staleElement.Coordinates);
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Net.WebException))]
        public void ErrorTouchInvalidArguments()
        {
            ErrorTouchInvalidArguments("longclick");
        }
    }
}

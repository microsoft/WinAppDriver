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
    public class TouchClick : TouchBase
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
        public void SingleTap()
        {
            session.FindElementByAccessibilityId("addressEditBox").SendKeys(CommonTestSettings.MicrosoftUrl + OpenQA.Selenium.Keys.Enter);
            System.Threading.Thread.Sleep(2000); // Sleep for 2 seconds
            var originalTitle = session.Title;
            Assert.AreNotEqual(string.Empty, originalTitle);

            // Navigate to GitHub
            session.FindElementByAccessibilityId("addressEditBox").SendKeys(CommonTestSettings.GitHubUrl + OpenQA.Selenium.Keys.Enter);
            System.Threading.Thread.Sleep(2000); // Sleep for 2 seconds
            Assert.AreNotEqual(originalTitle, session.Title);

            // Perform single tap touch on the back button
            touchScreen.SingleTap(session.FindElementByName("Back").Coordinates);
            System.Threading.Thread.Sleep(2000); // Sleep for 2 seconds

            // Make sure the page you went to is the page we started on
            Assert.AreEqual(originalTitle, session.Title);
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorTouchClosedWindow()
        {
            // Open a new window, retrieve an element, and close the window to get an orphaned element
            var orphanedElement = GetOrphanedElement(session);
            Assert.IsNotNull(orphanedElement);

            // Perform single tap touch on the orphaned element
            touchScreen.SingleTap(orphanedElement.Coordinates);
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Net.WebException))]
        public void ErrorTouchInvalidElement()
        {
            ErrorTouchInvalidElement("click");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorTouchStaleElement()
        {
            // Navigate to a webpage, save a reference to an element, and navigate away to get a stale element
            var staleElement = GetStaleElement(session);
            Assert.IsNotNull(staleElement);

            // Perform single tap touch on stale element
            touchScreen.SingleTap(staleElement.Coordinates);
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Net.WebException))]
        public void ErrorTouchInvalidArguments()
        {
            ErrorTouchInvalidArguments("click");
        }
    }
}

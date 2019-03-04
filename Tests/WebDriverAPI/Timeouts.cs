//******************************************************************************
//
// Copyright (c) 2017 Microsoft Corporation. All rights reserved.
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
using System.Diagnostics;

namespace WebDriverAPI
{
    [TestClass]
    public class Timeouts : AlarmClockBase
    {
        private static Stopwatch stopWatch = null;
        private const int implicitTimeoutMs = 2000;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);
            stopWatch = new Stopwatch();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            stopWatch = null;
            TearDown();
        }

        [TestMethod]
        public void SetImplicitTimeout_FindElementFound()
        {
            session.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(Convert.ToDouble(implicitTimeoutMs));
            stopWatch.Restart();
            WindowsElement element = session.FindElementByAccessibilityId(AlarmTabAutomationId);
            stopWatch.Stop();

            // The element should be found within the implicit timeout
            Assert.IsTrue(stopWatch.ElapsedMilliseconds < implicitTimeoutMs);
            Assert.IsNotNull(element);
            Assert.AreEqual(alarmTabElement, element);
        }

        [TestMethod]
        public void SetImplicitTimeout_FindElementNotFound()
        {
            session.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(Convert.ToDouble(implicitTimeoutMs));

            try
            {
                stopWatch.Restart();
                WindowsElement element = session.FindElementByAccessibilityId("InvalidAccessibiliyId");
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                stopWatch.Stop();

                // The search should fail after the timeout period lapsed
                Assert.IsTrue(stopWatch.ElapsedMilliseconds >= implicitTimeoutMs);
                Assert.AreEqual("An element could not be located on the page using the given search parameters.", exception.Message);
            }
        }

        [TestMethod]
        public void SetImplicitTimeout_FindElementsFound()
        {
            session.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(Convert.ToDouble(implicitTimeoutMs));
            stopWatch.Restart();
            var elements = session.FindElementsByAccessibilityId(AlarmTabAutomationId);
            stopWatch.Stop();

            // The elements should be found within the implicit timeout
            Assert.IsTrue(stopWatch.ElapsedMilliseconds < implicitTimeoutMs);
            Assert.IsNotNull(elements);
            Assert.AreEqual(1, elements.Count);
            Assert.IsTrue(elements.Contains(alarmTabElement));
        }

        [TestMethod]
        public void SetImplicitTimeout_FindElementsNotFound()
        {
            session.Manage().Timeouts().ImplicitWait = TimeSpan.Zero;
            stopWatch.Restart();
            var elements = session.FindElementsByAccessibilityId("InvalidAccessibiliyId");
            stopWatch.Stop();

            // The search should end returning no element after the timeout period lapsed
            Assert.IsTrue(stopWatch.ElapsedMilliseconds >= 0);
            Assert.IsNotNull(elements);
            Assert.AreEqual(0, elements.Count);
        }

        [TestMethod]
        public void SetImplicitTimeoutError_BadValue()
        {
            try
            {
                session.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(Convert.ToDouble(-1));
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual("Bad Command Parameter: ms:-1, type:implicit", exception.Message);
            }
        }

        [TestMethod]
        public void SetImplicitTimeoutError_UnsupportedTypePageLoad()
        {
            try
            {
                session.Manage().Timeouts().PageLoad = TimeSpan.FromMilliseconds(Convert.ToDouble(implicitTimeoutMs));
                Assert.Fail("Exception should have been thrown");
            }
            catch (WebDriverException exception)
            {
                Assert.AreEqual(string.Format(ErrorStrings.UnimplementedCommandTimeoutType, "page load"), exception.Message);
            }
        }

        [TestMethod]
        public void SetImplicitTimeoutError_UnsupportedTypeScript()
        {
            try
            {
                session.Manage().Timeouts().AsynchronousJavaScript = TimeSpan.FromMilliseconds(Convert.ToDouble(implicitTimeoutMs));
                Assert.Fail("Exception should have been thrown");
            }
            catch (WebDriverException exception)
            {
                Assert.AreEqual(string.Format(ErrorStrings.UnimplementedCommandTimeoutType, "script"), exception.Message);
            }
        }
    }
}

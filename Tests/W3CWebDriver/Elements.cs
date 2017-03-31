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
    public class Elements : AlarmClockBase
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
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorFindElementsByInvalidXPath()
        {
            var elements = session.FindElementsByXPath("//*//]");
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(OpenQA.Selenium.WebDriverException))]
        public void ErrorFindElementsByUnsupportedLocatorCSSSelector()
        {
            var elements = session.FindElementsByCssSelector("Query");
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(OpenQA.Selenium.WebDriverException))]
        public void ErrorFindElementsByUnsupportedLocatorLinkText()
        {
            var elements = session.FindElementsByLinkText("Query");
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(OpenQA.Selenium.WebDriverException))]
        public void ErrorFindElementsByUnsupportedLocatorPartialLinkText()
        {
            var elements = session.FindElementsByPartialLinkText("Query");
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        public void FindElementsByAccessibilityId()
        {
            var elements = session.FindElementsByAccessibilityId("AlarmPivotItem");
            Assert.IsNotNull(elements);
            Assert.AreEqual(1, elements.Count);
            Assert.IsTrue(elements.Contains(alarmTabElement));
        }

        [TestMethod]
        public void FindElementsByClassName()
        {
            var elements = session.FindElementsByClassName("PivotItem");
            Assert.IsNotNull(elements);
            Assert.AreEqual(4, elements.Count);
            Assert.IsTrue(elements.Contains(alarmTabElement));
        }

        [TestMethod]
        public void FindElementsByName()
        {
            session.FindElementByAccessibilityId("StopwatchPivotItem").Click();
            var elements = session.FindElementsByName("Start");
            Assert.IsNotNull(elements);
            Assert.AreEqual(1, elements.Count);
        }

        [TestMethod]
        public void FindElementsByNonExistentAccessibilityId()
        {
            var elements = session.FindElementsByAccessibilityId("NonExistentAccessibiliyId");
            Assert.IsNotNull(elements);
            Assert.AreEqual(0, elements.Count);
        }

        [TestMethod]
        public void FindElementsByNonExistentClassName()
        {
            var elements = session.FindElementsByClassName("NonExistentClassName");
            Assert.IsNotNull(elements);
            Assert.AreEqual(0, elements.Count);
        }


        [TestMethod]
        public void FindElementsByNonExistentName()
        {
            var elements = session.FindElementsByName("NonExistentName");
            Assert.IsNotNull(elements);
            Assert.AreEqual(0, elements.Count);
        }

        [TestMethod]
        public void FindElementsByNonExistentRuntimeId()
        {
            var elements = session.FindElementsById("NonExistentRuntimeId");
            Assert.IsNotNull(elements);
            Assert.AreEqual(0, elements.Count);
        }

        [TestMethod]
        public void FindElementsByNonExistentTagName()
        {
            var elements = session.FindElementsByTagName("NonExistentTagName");
            Assert.IsNotNull(elements);
            Assert.AreEqual(0, elements.Count);
        }

        [TestMethod]
        public void FindElementsByNonExistentXPath()
        {
            var elements = session.FindElementsByXPath("//NonExistentElement");
            Assert.IsNotNull(elements);
            Assert.AreEqual(0, elements.Count);
        }

        [TestMethod]
        public void FindElementsByTagName()
        {
            var elements = session.FindElementsByTagName("Button");
            Assert.IsNotNull(elements);

            // There are at least 7 buttons in Windows 10 Alarms & Clock app
            // Version 1511: 10, Version 1607: 7, Version 1703: 8
            Assert.IsTrue(elements.Count >= 7);
        }

        [TestMethod]
        public void FindElementsByXPath()
        {
            var elements = session.FindElementsByXPath("//Button");
            Assert.IsNotNull(elements);

            // There are at least 7 buttons in Windows 10 Alarms & Clock app
            // Version 1511: 10, Version 1607: 7, Version 1703: 8
            Assert.IsTrue(elements.Count >= 7);
        }
    }
}

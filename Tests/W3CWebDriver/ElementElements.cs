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
using OpenQA.Selenium.Appium.iOS;

namespace W3CWebDriver
{
    [TestClass]
    public class ElementElements : AlarmClockBase
    {
        private static IOSElement homePagePivot;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);
            homePagePivot = session.FindElementByAccessibilityId("HomePagePivot");
            Assert.IsNotNull(homePagePivot);
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
            var elements = alarmTabElement.FindElementsByXPath("//*//]");
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(OpenQA.Selenium.WebDriverException))]
        public void ErrorFindElementsByUnsupportedLocatorCSSSelector()
        {
            var elements = alarmTabElement.FindElementsByCssSelector("Query");
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(OpenQA.Selenium.WebDriverException))]
        public void ErrorFindElementsByUnsupportedLocatorLinkText()
        {
            var elements = alarmTabElement.FindElementsByLinkText("Query");
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(OpenQA.Selenium.WebDriverException))]
        public void ErrorFindElementsByUnsupportedLocatorPartialLinkText()
        {
            var elements = alarmTabElement.FindElementsByPartialLinkText("Query");
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        public void FindElementsByAccessibilityId()
        {
            var elements = alarmTabElement.FindElementsByAccessibilityId("AlarmPivotItem");
            Assert.IsNotNull(elements);
            Assert.AreEqual(1, elements.Count);
            Assert.IsTrue(elements.Contains(alarmTabElement));
        }

        [TestMethod]
        public void FindElementsByClassName()
        {
            var elements = homePagePivot.FindElementsByClassName("PivotItem");
            Assert.IsNotNull(elements);
            Assert.AreEqual(4, elements.Count);
            Assert.IsTrue(elements.Contains(alarmTabElement));
        }

        [TestMethod]
        public void FindElementsByName()
        {
            var stopwatchPivotItem = session.FindElementByAccessibilityId("StopwatchPivotItem");
            stopwatchPivotItem.Click();
            var elements = stopwatchPivotItem.FindElementsByName("Start");
            Assert.IsNotNull(elements);
            Assert.AreEqual(1, elements.Count);
        }

        [TestMethod]
        public void FindElementsByNonExistentAccessibilityId()
        {
            var elements = alarmTabElement.FindElementsByAccessibilityId("NonExistentAccessibiliyId");
            Assert.IsNotNull(elements);
            Assert.AreEqual(0, elements.Count);
        }

        [TestMethod]
        public void FindElementsByNonExistentClassName()
        {
            var elements = alarmTabElement.FindElementsByClassName("NonExistentClassName");
            Assert.IsNotNull(elements);
            Assert.AreEqual(0, elements.Count);
        }

        [TestMethod]
        public void FindElementsByNonExistentName()
        {
            var elements = alarmTabElement.FindElementsByName("NonExistentName");
            Assert.IsNotNull(elements);
            Assert.AreEqual(0, elements.Count);
        }

        [TestMethod]
        public void FindElementsByNonExistentRuntimeId()
        {
            var elements = alarmTabElement.FindElementsById("NonExistentRuntimeId");
            Assert.IsNotNull(elements);
            Assert.AreEqual(0, elements.Count);
        }

        [TestMethod]
        public void FindElementsByNonExistentTagName()
        {
            var elements = alarmTabElement.FindElementsByTagName("NonExistentTagName");
            Assert.IsNotNull(elements);
            Assert.AreEqual(0, elements.Count);
        }

        [TestMethod]
        public void FindElementsByNonExistentXPath()
        {
            var elements = alarmTabElement.FindElementsByXPath("//NonExistentElement");
            Assert.IsNotNull(elements);
            Assert.AreEqual(0, elements.Count);
        }

        [TestMethod]
        public void FindElementsByTagName()
        {
            var elements = homePagePivot.FindElementsByTagName("Button");
            Assert.IsNotNull(elements);

            // There are at least 4 buttons in Windows 10 Alarms & Clock HomePagePivot
            // Version 1511: 5, Version 1607: 5, Version 1703: 4
            Assert.IsTrue(elements.Count >= 4);
        }

        [TestMethod]
        public void FindElementsByXPath()
        {
            var elements = homePagePivot.FindElementsByXPath("//Button");
            Assert.IsNotNull(elements);

            // There are at least 4 buttons in Windows 10 Alarms & Clock HomePagePivot
            // Version 1511: 5, Version 1607: 5, Version 1703: 4
            Assert.IsTrue(elements.Count >= 4);
        }
    }
}

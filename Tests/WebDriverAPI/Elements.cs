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
using OpenQA.Selenium;
using System;

namespace WebDriverAPI
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
        public void FindElements_ByAccessibilityId()
        {
            var elements = session.FindElementsByAccessibilityId("AlarmPivotItem");
            Assert.IsNotNull(elements);
            Assert.AreEqual(1, elements.Count);
            Assert.IsTrue(elements.Contains(alarmTabElement));
        }

        [TestMethod]
        public void FindElements_ByClassName()
        {
            var elements = session.FindElementsByClassName("PivotItem");
            Assert.IsNotNull(elements);
            Assert.AreEqual(4, elements.Count);
            Assert.IsTrue(elements.Contains(alarmTabElement));
        }

        [TestMethod]
        public void FindElements_ByName()
        {
            session.FindElementByAccessibilityId("StopwatchPivotItem").Click();
            var elements = session.FindElementsByName("Start");
            Assert.IsNotNull(elements);
            Assert.AreEqual(1, elements.Count);
        }

        [TestMethod]
        public void FindElements_ByRuntimeId()
        {
            var elements = session.FindElementsById(alarmTabElement.Id);
            Assert.IsNotNull(elements);
            Assert.AreEqual(1, elements.Count);
            Assert.IsTrue(elements.Contains(alarmTabElement));
        }

        [TestMethod]
        public void FindElements_ByTagName()
        {
            var elements = session.FindElementsByTagName("Button");
            Assert.IsNotNull(elements);

            // There are at least 7 buttons in Windows 10 Alarms & Clock app
            // Version 1511: 10, Version 1607: 7, Version 1703: 8
            Assert.IsTrue(elements.Count >= 7);
        }

        [TestMethod]
        public void FindElements_ByXPath()
        {
            var elements = session.FindElementsByXPath("//Button");
            Assert.IsNotNull(elements);

            // There are at least 7 buttons in Windows 10 Alarms & Clock app
            // Version 1511: 10, Version 1607: 7, Version 1703: 8
            Assert.IsTrue(elements.Count >= 7);
        }

        [TestMethod]
        public void FindElementsByNonExistent_AccessibilityId()
        {
            var elements = session.FindElementsByAccessibilityId("NonExistentAccessibiliyId");
            Assert.IsNotNull(elements);
            Assert.AreEqual(0, elements.Count);
        }

        [TestMethod]
        public void FindElementsByNonExistent_ClassName()
        {
            var elements = session.FindElementsByClassName("NonExistentClassName");
            Assert.IsNotNull(elements);
            Assert.AreEqual(0, elements.Count);
        }


        [TestMethod]
        public void FindElementsByNonExistent_Name()
        {
            var elements = session.FindElementsByName("NonExistentName");
            Assert.IsNotNull(elements);
            Assert.AreEqual(0, elements.Count);
        }

        [TestMethod]
        public void FindElementsByNonExistent_RuntimeId()
        {
            var elements = session.FindElementsById("NonExistentRuntimeId");
            Assert.IsNotNull(elements);
            Assert.AreEqual(0, elements.Count);
        }

        [TestMethod]
        public void FindElementsByNonExistent_TagName()
        {
            var elements = session.FindElementsByTagName("NonExistentTagName");
            Assert.IsNotNull(elements);
            Assert.AreEqual(0, elements.Count);
        }

        [TestMethod]
        public void FindElementsByNonExistent_XPath()
        {
            var elements = session.FindElementsByXPath("//NonExistentElement");
            Assert.IsNotNull(elements);
            Assert.AreEqual(0, elements.Count);
        }

        [TestMethod]
        public void FindElementsError_NoSuchWindow()
        {
            try
            {
                var elements = Utility.GetOrphanedSession().FindElementsByAccessibilityId("An accessibility id");
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }

        [TestMethod]
        public void FindElementsError_UnsupportedLocatorCSSSelector()
        {
            try
            {
                var elements = session.FindElementsByCssSelector("Query");
                Assert.Fail("Exception should have been thrown");
            }
            catch (WebDriverException exception)
            {
                Assert.AreEqual(string.Format(ErrorStrings.UnimplementedCommandLocator, "css selector"), exception.Message);
            }
        }

        [TestMethod]
        public void FindElementsError_UnsupportedLocatorLinkText()
        {
            try
            {
                var elements = session.FindElementsByLinkText("Query");
                Assert.Fail("Exception should have been thrown");
            }
            catch (WebDriverException exception)
            {
                Assert.AreEqual(string.Format(ErrorStrings.UnimplementedCommandLocator, "link text"), exception.Message);
            }
        }

        [TestMethod]
        public void FindElementsError_UnsupportedLocatorPartialLinkText()
        {
            try
            {
                var elements = session.FindElementsByPartialLinkText("Query");
                Assert.Fail("Exception should have been thrown");
            }
            catch (WebDriverException exception)
            {
                Assert.AreEqual(string.Format(ErrorStrings.UnimplementedCommandLocator, "partial link text"), exception.Message);
            }
        }

        [TestMethod]
        public void FindElementsError_XPathLookupErrorExpression()
        {
            try
            {
                var elements = session.FindElementsByXPath("//*//]");
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(string.Format(ErrorStrings.XPathLookupError, "//*//]"), exception.Message);
            }
        }
    }
}

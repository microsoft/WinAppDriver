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
    public class ElementElements : AlarmClockBase
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
        public void FindNestedElements_ByAccessibilityId()
        {
            var elements = alarmTabElement.FindElementsByAccessibilityId(AlarmTabAutomationId);
            Assert.IsNotNull(elements);
            Assert.AreEqual(1, elements.Count);
            Assert.IsTrue(elements.Contains(alarmTabElement));
        }

        [TestMethod]
        public void FindNestedElements_ByClassName()
        {
            // Different Alarm & Clock application version uses different UI elements
            if (AlarmTabClassName == "ListViewItem")
            {
                var elements = session.FindElementByAccessibilityId("TopNavMenuItemsHost").FindElementsByClassName("ListViewItem");
                Assert.IsNotNull(elements);
                Assert.AreEqual(4, elements.Count);
                Assert.IsTrue(elements.Contains(alarmTabElement));
            }
            else
            {
                var elements = session.FindElementByAccessibilityId("HomePagePivot").FindElementsByClassName("PivotItem");
                Assert.IsNotNull(elements);
                Assert.AreEqual(4, elements.Count);
                Assert.IsTrue(elements.Contains(alarmTabElement));
            }
        }

        [TestMethod]
        public void FindNestedElements_ByName()
        {
            // Different Alarm & Clock application version uses different UI elements
            if (AlarmTabClassName == "ListViewItem")
            {
                var menuItems = session.FindElementByAccessibilityId("TopNavMenuItemsHost");
                var elements = menuItems.FindElementsByName("Stopwatch");
                Assert.IsNotNull(elements);
                Assert.AreEqual(1, elements.Count);
            }
            else
            {
                var stopwatchPivotItem = session.FindElementByAccessibilityId(StopwatchTabAutomationId);
                stopwatchPivotItem.Click();
                var elements = stopwatchPivotItem.FindElementsByName("Start");
                Assert.IsNotNull(elements);
                Assert.AreEqual(1, elements.Count);
            }
        }

        [TestMethod]
        public void FindNestedElements_ByRuntimeId()
        {
            var elements = alarmTabElement.FindElementsById(alarmTabElement.Id);
            Assert.IsNotNull(elements);
            Assert.AreEqual(1, elements.Count);
            Assert.IsTrue(elements.Contains(alarmTabElement));
        }

        [TestMethod]
        public void FindNestedElements_ByTagName()
        {
            // Different Alarm & Clock application version uses different UI elements
            if (AlarmTabClassName == "ListViewItem")
            {
                var elements = session.FindElementByAccessibilityId("TopNavMenuItemsHost").FindElementsByTagName("ListItem");
                Assert.IsNotNull(elements);
                Assert.AreEqual(4, elements.Count);
            }
            else
            {
                var elements = session.FindElementByAccessibilityId("HomePagePivot").FindElementsByTagName("Button");
                Assert.IsNotNull(elements);

                // There are at least 4 buttons in Windows 10 Alarms & Clock HomePagePivot
                // Version 1511: 5, Version 1607: 5, Version 1703: 4
                Assert.IsTrue(elements.Count >= 4);
            }
        }

        [TestMethod]
        public void FindNestedElements_ByXPath()
        {
            // Different Alarm & Clock application version uses different UI elements
            if (AlarmTabClassName == "ListViewItem")
            {
                var elements = session.FindElementByAccessibilityId("TopNavMenuItemsHost").FindElementsByXPath("//ListItem");
                Assert.IsNotNull(elements);
                Assert.AreEqual(4, elements.Count);
            }
            else
            {
                var elements = session.FindElementByAccessibilityId("HomePagePivot").FindElementsByXPath("//Button");
                Assert.IsNotNull(elements);

                // There are at least 4 buttons in Windows 10 Alarms & Clock HomePagePivot
                // Version 1511: 5, Version 1607: 5, Version 1703: 4
                Assert.IsTrue(elements.Count >= 4);
            }
        }

        [TestMethod]
        public void FindNestedElementsByNonExistent_AccessibilityId()
        {
            var elements = alarmTabElement.FindElementsByAccessibilityId("NonExistentAccessibiliyId");
            Assert.IsNotNull(elements);
            Assert.AreEqual(0, elements.Count);
        }

        [TestMethod]
        public void FindNestedElementsByNonExistent_ClassName()
        {
            var elements = alarmTabElement.FindElementsByClassName("NonExistentClassName");
            Assert.IsNotNull(elements);
            Assert.AreEqual(0, elements.Count);
        }

        [TestMethod]
        public void FindNestedElementsByNonExistent_Name()
        {
            var elements = alarmTabElement.FindElementsByName("NonExistentName");
            Assert.IsNotNull(elements);
            Assert.AreEqual(0, elements.Count);
        }

        [TestMethod]
        public void FindNestedElementsByNonExistent_RuntimeId()
        {
            var elements = alarmTabElement.FindElementsById("NonExistentRuntimeId");
            Assert.IsNotNull(elements);
            Assert.AreEqual(0, elements.Count);
        }

        [TestMethod]
        public void FindNestedElementsByNonExistent_TagName()
        {
            var elements = alarmTabElement.FindElementsByTagName("NonExistentTagName");
            Assert.IsNotNull(elements);
            Assert.AreEqual(0, elements.Count);
        }

        [TestMethod]
        public void FindNestedElementsByNonExistent_XPath()
        {
            var elements = alarmTabElement.FindElementsByXPath("//NonExistentElement");
            Assert.IsNotNull(elements);
            Assert.AreEqual(0, elements.Count);
        }

        [TestMethod]
        public void FindNestedElementsError_NoSuchWindow()
        {
            try
            {
                var elements = Utility.GetOrphanedElement().FindElementsByAccessibilityId("An accessibility id");
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }

        [TestMethod]
        public void FindNestedElementsError_StaleElement()
        {
            try
            {
                var elements = GetStaleElement().FindElementsByAccessibilityId("An accessibility id");
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.StaleElementReference, exception.Message);
            }
        }

        [TestMethod]
        public void FindNestedElementsError_UnsupportedLocatorCSSSelector()
        {
            try
            {
                var elements = alarmTabElement.FindElementsByCssSelector("Query");
                Assert.Fail("Exception should have been thrown");
            }
            catch (WebDriverException exception)
            {
                Assert.AreEqual(string.Format(ErrorStrings.UnimplementedCommandLocator, "css selector"), exception.Message);
            }
        }

        [TestMethod]
        public void FindNestedElementsError_UnsupportedLocatorLinkText()
        {
            try
            {
                var elements = alarmTabElement.FindElementsByLinkText("Query");
                Assert.Fail("Exception should have been thrown");
            }
            catch (WebDriverException exception)
            {
                Assert.AreEqual(string.Format(ErrorStrings.UnimplementedCommandLocator, "link text"), exception.Message);
            }
        }

        [TestMethod]
        public void FindNestedElementsError_UnsupportedLocatorPartialLinkText()
        {
            try
            {
                var elements = alarmTabElement.FindElementsByPartialLinkText("Query");
                Assert.Fail("Exception should have been thrown");
            }
            catch (WebDriverException exception)
            {
                Assert.AreEqual(string.Format(ErrorStrings.UnimplementedCommandLocator, "partial link text"), exception.Message);
            }
        }

        [TestMethod]
        public void FindNestedElementsError_XPathLookupErrorExpression()
        {
            try
            {
                var elements = alarmTabElement.FindElementsByXPath("//*//]");
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(string.Format(ErrorStrings.XPathLookupError, "//*//]"), exception.Message);
            }
        }
    }
}

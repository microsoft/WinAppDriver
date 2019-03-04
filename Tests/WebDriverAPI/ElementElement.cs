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
using OpenQA.Selenium.Appium.Windows;
using System;

namespace WebDriverAPI
{
    [TestClass]
    public class ElementElement : AlarmClockBase
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
        public void FindNestedElement_ByAccessibilityId()
        {
            WindowsElement ancestorElement;

            // Different Alarm & Clock application version uses different UI elements
            if (AlarmTabClassName == "ListViewItem")
            {
                ancestorElement = session.FindElementByAccessibilityId("AlarmCollectionPageCommandBar");
            }
            else
            {
                ancestorElement = alarmTabElement;
            }

            WindowsElement element = ancestorElement.FindElementByAccessibilityId("AddAlarmButton") as WindowsElement;
            Assert.IsNotNull(element);

            WindowsElement addAlarmButtonElement = session.FindElementByAccessibilityId("AddAlarmButton") as WindowsElement;
            Assert.IsNotNull(addAlarmButtonElement);

            Assert.AreEqual(addAlarmButtonElement, element);
        }

        [TestMethod]
        public void FindNestedElement_ByClassName()
        {
            // Different Alarm & Clock application version uses different UI elements
            if (AlarmTabClassName == "ListViewItem")
            {
                WindowsElement ancestorElement = session.FindElementByClassName("ApplicationBar");
                WindowsElement element = ancestorElement.FindElementByClassName("AppBarButton") as WindowsElement;
                Assert.IsNotNull(element);
            }
            else
            {
                WindowsElement element = alarmTabElement.FindElementByClassName("ApplicationBar") as WindowsElement;
                Assert.IsNotNull(element);
            }
        }

        [TestMethod]
        public void FindNestedElement_ByName()
        {
            // Different Alarm & Clock application version uses different UI elements
            if (AlarmTabClassName == "ListViewItem")
            {
                WindowsElement ancestorElement = session.FindElementByClassName("ApplicationBar");
                WindowsElement element = ancestorElement.FindElementByName("Add new alarm") as WindowsElement;
                Assert.IsNotNull(element);
            }
            else
            {
                var stopwatchPivotItem = session.FindElementByAccessibilityId(StopwatchTabAutomationId);
                stopwatchPivotItem.Click();
                WindowsElement element = stopwatchPivotItem.FindElementByName("Reset") as WindowsElement;
                Assert.IsNotNull(element);
            }
        }

        [TestMethod]
        public void FindNestedElement_ByRuntimeId()
        {
            WindowsElement addAlarmButtonElement = session.FindElementByAccessibilityId("AddAlarmButton");
            WindowsElement element = alarmTabElement.FindElementById(addAlarmButtonElement.Id) as WindowsElement;
            Assert.IsNotNull(element);
            Assert.AreEqual(addAlarmButtonElement, element);
        }

        [TestMethod]
        public void FindNestedElement_ByTagName()
        {
            // Different Alarm & Clock application version uses different UI elements
            if (AlarmTabClassName == "ListViewItem")
            {
                WindowsElement ancestorElement = session.FindElementByClassName("ApplicationBar");
                WindowsElement element = ancestorElement.FindElementByTagName("Button") as WindowsElement;
                Assert.IsNotNull(element);
            }
            else
            {
                WindowsElement element = alarmTabElement.FindElementByTagName("Button") as WindowsElement;
                Assert.IsNotNull(element);
            }
        }

        [TestMethod]
        public void FindNestedElement_ByXPath()
        {
            // Different Alarm & Clock application version uses different UI elements
            if (AlarmTabClassName == "ListViewItem")
            {
                WindowsElement ancestorElement = session.FindElementByClassName("ApplicationBar");
                WindowsElement element = ancestorElement.FindElementByXPath("//Button[@AutomationId=\"MoreButton\"]") as WindowsElement;
                Assert.IsNotNull(element);
            }
            else
            {
                WindowsElement element = alarmTabElement.FindElementByXPath("//Button[@AutomationId=\"MoreButton\"]") as WindowsElement;
                Assert.IsNotNull(element);
            }
        }

        [TestMethod]
        public void FindNestedElementError_NoSuchElementByAccessibilityId()
        {
            try
            {
                WindowsElement element = alarmTabElement.FindElementByAccessibilityId("InvalidAccessibiliyId") as WindowsElement;
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchElement, exception.Message);
            }
        }

        [TestMethod]
        public void FindNestedElementError_NoSuchElementByClassName()
        {
            try
            {
                WindowsElement element = alarmTabElement.FindElementByClassName("InvalidClassName") as WindowsElement;
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchElement, exception.Message);
            }
        }

        [TestMethod]
        public void FindNestedElementError_NoSuchElementByName()
        {
            try
            {
                WindowsElement element = alarmTabElement.FindElementByName("InvalidName") as WindowsElement;
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchElement, exception.Message);
            }
        }

        [TestMethod]
        public void FindNestedElementError_NoSuchElementByRuntimeId()
        {
            try
            {
                WindowsElement element = alarmTabElement.FindElementById("InvalidRuntimeId") as WindowsElement;
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchElement, exception.Message);
            }
        }

        [TestMethod]
        public void FindNestedElementError_NoSuchElementByTagName()
        {
            try
            {
                WindowsElement element = alarmTabElement.FindElementByTagName("InvalidTagName") as WindowsElement;
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchElement, exception.Message);
            }
        }

        [TestMethod]
        public void FindNestedElementError_NoSuchElementByTagNameMalformed()
        {
            try
            {
                WindowsElement element = alarmTabElement.FindElementByTagName("//@InvalidTagNameMalformed") as WindowsElement;
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchElement, exception.Message);
            }
        }

        [TestMethod]
        public void FindNestedElementError_NoSuchElementByXPath()
        {
            try
            {
                WindowsElement element = session.FindElementByXPath("//*[@Name=\"NonExistentElement\"]") as WindowsElement;
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchElement, exception.Message);
            }
        }

        [TestMethod]
        public void FindNestedElementError_NoSuchWindow()
        {
            try
            {
                WindowsElement element = Utility.GetOrphanedElement().FindElementByAccessibilityId("An accessibility id") as WindowsElement;
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }

        [TestMethod]
        public void FindNestedElementError_StaleElement()
        {
            try
            {
                WindowsElement element = GetStaleElement().FindElementByAccessibilityId("An accessibility id") as WindowsElement;
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.StaleElementReference, exception.Message);
            }
        }

        [TestMethod]
        public void FindNestedElementError_UnsupportedLocatorCSSSelector()
        {
            try
            {
                WindowsElement element = alarmTabElement.FindElementByCssSelector("Query") as WindowsElement;
                Assert.Fail("Exception should have been thrown");
            }
            catch (WebDriverException exception)
            {
                Assert.AreEqual(string.Format(ErrorStrings.UnimplementedCommandLocator, "css selector"), exception.Message);
            }
        }

        [TestMethod]
        public void FindNestedElementError_UnsupportedLocatorLinkText()
        {
            try
            {
                WindowsElement element = alarmTabElement.FindElementByLinkText("Query") as WindowsElement;
                Assert.Fail("Exception should have been thrown");
            }
            catch (WebDriverException exception)
            {
                Assert.AreEqual(string.Format(ErrorStrings.UnimplementedCommandLocator, "link text"), exception.Message);
            }
        }

        [TestMethod]
        public void FindNestedElementError_UnsupportedLocatorPartialLinkText()
        {
            try
            {
                WindowsElement element = alarmTabElement.FindElementByPartialLinkText("Query") as WindowsElement;
                Assert.Fail("Exception should have been thrown");
            }
            catch (WebDriverException exception)
            {
                Assert.AreEqual(string.Format(ErrorStrings.UnimplementedCommandLocator, "partial link text"), exception.Message);
            }
        }

        [TestMethod]
        public void FindNestedElementError_XPathLookupErrorExpression()
        {
            try
            {
                WindowsElement element = alarmTabElement.FindElementByXPath("//*//]") as WindowsElement;
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(string.Format(ErrorStrings.XPathLookupError, "//*//]"), exception.Message);
            }
        }
    }
}

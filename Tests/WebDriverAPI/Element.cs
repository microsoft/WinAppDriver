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
    public class Element : AlarmClockBase
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
        public void FindElement_ByAccessibilityId()
        {
            WindowsElement element = session.FindElementByAccessibilityId(AlarmTabAutomationId);
            Assert.IsNotNull(element);
            Assert.AreEqual(alarmTabElement, element);
        }

        [TestMethod]
        public void FindElement_ByClassName()
        {
            WindowsElement element = session.FindElementByClassName(AlarmTabClassName);
            Assert.IsNotNull(element);
            Assert.AreEqual(alarmTabElement, element);
        }

        [TestMethod]
        public void FindElement_ByName()
        {
            session.FindElementByAccessibilityId(StopwatchTabAutomationId).Click();
            WindowsElement element = session.FindElementByName("Reset");
            Assert.IsNotNull(element);
        }

        [TestMethod]
        public void FindElement_ByRuntimeId()
        {
            WindowsElement element = session.FindElementById(alarmTabElement.Id);
            Assert.IsNotNull(element);
            Assert.AreEqual(alarmTabElement, element);
        }

        [TestMethod]
        public void FindElement_ByTagName()
        {
            WindowsElement element = session.FindElementByTagName("Button");
            Assert.IsNotNull(element);
        }

        [TestMethod]
        public void FindElement_ByXPath()
        {
            WindowsElement element = session.FindElementByXPath("//Button[@AutomationId=\"MoreButton\"]");
            Assert.IsNotNull(element);
        }

        [TestMethod]
        public void FindElementError_NoSuchElementByAccessibilityId()
        {
            try
            {
                WindowsElement element = session.FindElementByAccessibilityId("InvalidAccessibiliyId");
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchElement, exception.Message);
            }
        }

        [TestMethod]
        public void FindElementError_NoSuchElementByClassName()
        {
            try
            {
                WindowsElement element = session.FindElementByClassName("InvalidClassName");
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchElement, exception.Message);
            }
        }

        [TestMethod]
        public void FindElementError_NoSuchElementByName()
        {
            try
            {
                WindowsElement element = session.FindElementByName("InvalidName");
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchElement, exception.Message);
            }
        }

        [TestMethod]
        public void FindElementError_NoSuchElementByRuntimeId()
        {
            try
            {
                WindowsElement element = session.FindElementById("InvalidRuntimeId");
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchElement, exception.Message);
            }
        }

        [TestMethod]
        public void FindElementError_NoSuchElementByTagName()
        {
            try
            {
                WindowsElement element = session.FindElementByTagName("InvalidTagName");
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchElement, exception.Message);
            }
        }

        [TestMethod]
        public void FindElementError_NoSuchElementByTagNameMalformed()
        {
            try
            {
                WindowsElement element = session.FindElementByTagName("//@InvalidTagNameMalformed");
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchElement, exception.Message);
            }
        }

        [TestMethod]
        public void FindElementError_NoSuchElementByXPath()
        {
            try
            {
                WindowsElement alarmTab = session.FindElementByXPath("//*[@Name=\"NonExistentElement\"]");
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchElement, exception.Message);
            }
        }

        [TestMethod]
        public void FindElementError_NoSuchWindow()
        {
            try
            {
                WindowsElement element = Utility.GetOrphanedSession().FindElementByAccessibilityId("An accessibility id") as WindowsElement;
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }

        [TestMethod]
        public void FindElementError_UnsupportedLocatorCSSSelector()
        {
            try
            {
                WindowsElement element = session.FindElementByCssSelector("Query");
                Assert.Fail("Exception should have been thrown");
            }
            catch (WebDriverException exception)
            {
                Assert.AreEqual(string.Format(ErrorStrings.UnimplementedCommandLocator, "css selector"), exception.Message);
            }
        }

        [TestMethod]
        public void FindElementError_UnsupportedLocatorLinkText()
        {
            try
            {
                WindowsElement element = session.FindElementByLinkText("Query");
                Assert.Fail("Exception should have been thrown");
            }
            catch (WebDriverException exception)
            {
                Assert.AreEqual(string.Format(ErrorStrings.UnimplementedCommandLocator, "link text"), exception.Message);
            }
        }

        [TestMethod]
        public void FindElementError_UnsupportedLocatorPartialLinkText()
        {
            try
            {
                WindowsElement element = session.FindElementByPartialLinkText("Query");
                Assert.Fail("Exception should have been thrown");
            }
            catch (WebDriverException exception)
            {
                Assert.AreEqual(string.Format(ErrorStrings.UnimplementedCommandLocator, "partial link text"), exception.Message);
            }
        }

        [TestMethod]
        public void FindElementError_XPathLookupErrorExpression()
        {
            try
            {
                WindowsElement element = session.FindElementByXPath("//*//]");
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(string.Format(ErrorStrings.XPathLookupError, "//*//]"), exception.Message);
            }
        }
    }
}

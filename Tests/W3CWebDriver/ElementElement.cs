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
using OpenQA.Selenium.Appium.Windows;

namespace W3CWebDriver
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
        public void FindElementByAccessibilityId()
        {
            WindowsElement element = alarmTabElement.FindElementByAccessibilityId("AlarmListView") as WindowsElement;
            Assert.IsNotNull(element);

            WindowsElement alarmListViewElement = session.FindElementByAccessibilityId("AlarmListView") as WindowsElement;
            Assert.IsNotNull(alarmListViewElement);

            Assert.AreEqual(alarmListViewElement, element);
        }

        [TestMethod]
        public void FindElementByClassName()
        {
            WindowsElement element = alarmTabElement.FindElementByClassName("ListView") as WindowsElement;
            Assert.IsNotNull(element);
        }

        [TestMethod]
        public void FindElementByName()
        {
            var stopwatchPivotItem = session.FindElementByAccessibilityId("StopwatchPivotItem");
            stopwatchPivotItem.Click();
            WindowsElement element = stopwatchPivotItem.FindElementByName("Start") as WindowsElement;
            Assert.IsNotNull(element);
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorFindElementByInvalidAccessibilityId()
        {
            WindowsElement element = alarmTabElement.FindElementByAccessibilityId("InvalidAccessibiliyId") as WindowsElement;
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorFindElementByInvalidClassName()
        {
            WindowsElement element = alarmTabElement.FindElementByClassName("InvalidClassName") as WindowsElement;
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorFindElementByInvalidName()
        {
            WindowsElement element = alarmTabElement.FindElementByName("InvalidName") as WindowsElement;
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorFindElementByInvalidRuntimeId()
        {
            WindowsElement element = alarmTabElement.FindElementById("InvalidRuntimeId") as WindowsElement;
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorFindElementByInvalidTagName()
        {
            WindowsElement element = alarmTabElement.FindElementByTagName("InvalidTagName") as WindowsElement;
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorFindElementByInvalidTagNameMalformed()
        {
            WindowsElement element = alarmTabElement.FindElementByTagName("//@InvalidTagNameMalformed") as WindowsElement;
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorFindElementByInvalidXPath()
        {
            WindowsElement element = alarmTabElement.FindElementByXPath("//*//]") as WindowsElement;
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorFindElementByNonExistentXPath()
        {
            WindowsElement element = session.FindElementByXPath("//*[@Name=\"NonExistentElement\"]") as WindowsElement;
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(OpenQA.Selenium.WebDriverException))]
        public void ErrorFindElementByUnsupportedLocatorCSSSelector()
        {
            WindowsElement element = alarmTabElement.FindElementByCssSelector("Query") as WindowsElement;
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(OpenQA.Selenium.WebDriverException))]
        public void ErrorFindElementByUnsupportedLocatorLinkText()
        {
            WindowsElement element = alarmTabElement.FindElementByLinkText("Query") as WindowsElement;
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(OpenQA.Selenium.WebDriverException))]
        public void ErrorFindElementByUnsupportedLocatorPartialLinkText()
        {
            WindowsElement element = alarmTabElement.FindElementByPartialLinkText("Query") as WindowsElement;
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        public void FindElementByTagName()
        {
            WindowsElement element = alarmTabElement.FindElementByTagName("Button") as WindowsElement;
            Assert.IsNotNull(element);
        }

        [TestMethod]
        public void FindElementByXPath()
        {
            WindowsElement element = alarmTabElement.FindElementByXPath("//Button[@AutomationId=\"MoreButton\"]") as WindowsElement;
            Assert.IsNotNull(element);
        }
    }
}

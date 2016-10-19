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

        [TestInitialize]
        public void TestInitialize()
        {
            alarmListViewElement = session.FindElementByAccessibilityId("AlarmListView");
            Assert.IsNotNull(alarmListViewElement);
        }

        protected static IOSElement alarmListViewElement;

        [TestMethod]
        public void FindElementByAccessibilityId()
        {
            IOSElement element = alarmTabElement.FindElementByAccessibilityId("AlarmListView") as IOSElement;
            Assert.IsNotNull(element);
            Assert.AreEqual(alarmListViewElement, element);
        }

        [TestMethod]
        public void FindElementByClassName()
        {
            IOSElement element = alarmTabElement.FindElementByClassName("ListView") as IOSElement;
            Assert.IsNotNull(element);
            Assert.AreEqual(alarmListViewElement, element);
        }

        [TestMethod]
        public void FindElementByName()
        {
            IOSElement element = alarmTabElement.FindElementByName("Alarm Collection") as IOSElement;
            Assert.IsNotNull(element);
            Assert.AreEqual(alarmListViewElement, element);
        }

        [TestMethod]
        [ExpectedException(typeof(OpenQA.Selenium.NoSuchElementException))]
        public void ErrorFindElementByInvalidAccessibilityId()
        {
            IOSElement element = alarmTabElement.FindElementByAccessibilityId("InvalidAccessibiliyId") as IOSElement;
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(OpenQA.Selenium.NoSuchElementException))]
        public void ErrorFindElementByInvalidClassName()
        {
            IOSElement element = alarmTabElement.FindElementByClassName("InvalidClassName") as IOSElement;
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(OpenQA.Selenium.NoSuchElementException))]
        public void ErrorFindElementByInvalidName()
        {
            IOSElement element = alarmTabElement.FindElementByName("InvalidName") as IOSElement;
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(OpenQA.Selenium.NoSuchElementException))]
        public void ErrorFindElementByInvalidRuntimeId()
        {
            IOSElement element = alarmTabElement.FindElementById("InvalidRuntimeId") as IOSElement;
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(OpenQA.Selenium.WebDriverException))]
        public void ErrorFindElementByUnsupportedLocatorCSSSelector()
        {
            IOSElement element = alarmTabElement.FindElementByCssSelector("Query") as IOSElement;
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(OpenQA.Selenium.WebDriverException))]
        public void ErrorFindElementByUnsupportedLocatorLinkText()
        {
            IOSElement element = alarmTabElement.FindElementByLinkText("Query") as IOSElement;
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(OpenQA.Selenium.WebDriverException))]
        public void ErrorFindElementByUnsupportedLocatorPartialLinkText()
        {
            IOSElement element = alarmTabElement.FindElementByPartialLinkText("Query") as IOSElement;
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        public void FindElementByTagName()
        {
            IOSElement element = alarmTabElement.FindElementByTagName("ListItem") as IOSElement;
            Assert.IsNotNull(element);
        }

        [TestMethod]
        public void FindElementByXPath()
        {
            IOSElement element = alarmTabElement.FindElementByXPath("//*[@Name=\"Alarm Collection\"]") as IOSElement;
            Assert.IsNotNull(element);
        }
    }
}

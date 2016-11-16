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

        [TestMethod]
        public void FindElementByAccessibilityId()
        {
            IOSElement element = alarmTabElement.FindElementByAccessibilityId("AlarmListView") as IOSElement;
            Assert.IsNotNull(element);

            IOSElement alarmListViewElement = session.FindElementByAccessibilityId("AlarmListView") as IOSElement;
            Assert.IsNotNull(alarmListViewElement);

            Assert.AreEqual(alarmListViewElement, element);
        }

        [TestMethod]
        public void FindElementByClassName()
        {
            IOSElement element = alarmTabElement.FindElementByClassName("ListView") as IOSElement;
            Assert.IsNotNull(element);
        }

        [TestMethod]
        public void FindElementByName()
        {
            IOSElement element = alarmTabElement.FindElementByName("More app bar") as IOSElement;
            Assert.IsNotNull(element);
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorFindElementByInvalidAccessibilityId()
        {
            IOSElement element = alarmTabElement.FindElementByAccessibilityId("InvalidAccessibiliyId") as IOSElement;
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorFindElementByInvalidClassName()
        {
            IOSElement element = alarmTabElement.FindElementByClassName("InvalidClassName") as IOSElement;
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorFindElementByInvalidName()
        {
            IOSElement element = alarmTabElement.FindElementByName("InvalidName") as IOSElement;
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorFindElementByInvalidRuntimeId()
        {
            IOSElement element = alarmTabElement.FindElementById("InvalidRuntimeId") as IOSElement;
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorFindElementByInvalidTagName()
        {
            IOSElement element = alarmTabElement.FindElementByTagName("InvalidTagName") as IOSElement;
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorFindElementByInvalidTagNameMalformed()
        {
            IOSElement element = alarmTabElement.FindElementByTagName("//@InvalidTagNameMalformed") as IOSElement;
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorFindElementByInvalidXPath()
        {
            IOSElement element = alarmTabElement.FindElementByXPath("//*//]") as IOSElement;
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorFindElementByNonExistentXPath()
        {
            IOSElement element = session.FindElementByXPath("//*[@Name=\"NonExistentElement\"]") as IOSElement;
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
            IOSElement element = alarmTabElement.FindElementByTagName("Button") as IOSElement;
            Assert.IsNotNull(element);
        }

        [TestMethod]
        public void FindElementByXPath()
        {
            IOSElement element = alarmTabElement.FindElementByXPath("//Button[@AutomationId=\"MoreButton\"]") as IOSElement;
            Assert.IsNotNull(element);
        }
    }
}

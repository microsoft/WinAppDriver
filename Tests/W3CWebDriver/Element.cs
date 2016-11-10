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
        public void FindElementByAccessibilityId()
        {
            IOSElement element = session.FindElementByAccessibilityId("AlarmPivotItem");
            Assert.IsNotNull(element);
            Assert.AreEqual(alarmTabElement, element);
        }

        [TestMethod]
        public void FindElementByClassName()
        {
            IOSElement element = session.FindElementByClassName("PivotItem");
            Assert.IsNotNull(element);
            Assert.AreEqual(alarmTabElement, element);
        }

        [TestMethod]
        public void FindElementByName()
        {
            IOSElement element = session.FindElementByName("More app bar");
            Assert.IsNotNull(element);
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorFindElementByInvalidAccessibilityId()
        {
            IOSElement element = session.FindElementByAccessibilityId("InvalidAccessibiliyId");
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorFindElementByInvalidClassName()
        {
            IOSElement element = session.FindElementByClassName("InvalidClassName");
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorFindElementByInvalidName()
        {
            IOSElement element = session.FindElementByName("InvalidName");
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorFindElementByInvalidRuntimeId()
        {
            IOSElement element = session.FindElementById("InvalidRuntimeId");
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorFindElementByInvalidTagName()
        {
            IOSElement element = session.FindElementByTagName("InvalidTagName");
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorFindElementByInvalidTagNameMalformed()
        {
            IOSElement element = session.FindElementByTagName("//@InvalidTagNameMalformed");
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorFindElementByInvalidXPath()
        {
            IOSElement element = session.FindElementByXPath("//*//]");
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorFindElementByNonExistentXPath()
        {
            IOSElement alarmTab = session.FindElementByXPath("//*[@Name=\"NonExistentElement\"]");
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(OpenQA.Selenium.WebDriverException))]
        public void ErrorFindElementByUnsupportedLocatorCSSSelector()
        {
            IOSElement element = session.FindElementByCssSelector("Query");
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(OpenQA.Selenium.WebDriverException))]
        public void ErrorFindElementByUnsupportedLocatorLinkText()
        {
            IOSElement element = session.FindElementByLinkText("Query");
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(OpenQA.Selenium.WebDriverException))]
        public void ErrorFindElementByUnsupportedLocatorPartialLinkText()
        {
            IOSElement element = session.FindElementByPartialLinkText("Query");
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        public void FindElementByTagName()
        {
            IOSElement element = session.FindElementByTagName("Button");
            Assert.IsNotNull(element);
        }

        [TestMethod]
        public void FindElementByXPath()
        {
            IOSElement alarmTab = session.FindElementByXPath("//Button[@AutomationId=\"MoreButton\"]");
            Assert.IsNotNull(alarmTab);
        }
    }
}

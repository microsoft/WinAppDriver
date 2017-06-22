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
            WindowsElement element = session.FindElementByAccessibilityId("AlarmPivotItem");
            Assert.IsNotNull(element);
            Assert.AreEqual(alarmTabElement, element);
        }

        [TestMethod]
        public void FindElementByClassName()
        {
            WindowsElement element = session.FindElementByClassName("PivotItem");
            Assert.IsNotNull(element);
            Assert.AreEqual(alarmTabElement, element);
        }

        [TestMethod]
        public void FindElementByName()
        {
            session.FindElementByAccessibilityId("StopwatchPivotItem").Click();
            WindowsElement element = session.FindElementByName("Reset");
            Assert.IsNotNull(element);
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorFindElementByInvalidAccessibilityId()
        {
            WindowsElement element = session.FindElementByAccessibilityId("InvalidAccessibiliyId");
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorFindElementByInvalidClassName()
        {
            WindowsElement element = session.FindElementByClassName("InvalidClassName");
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorFindElementByInvalidName()
        {
            WindowsElement element = session.FindElementByName("InvalidName");
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorFindElementByInvalidRuntimeId()
        {
            WindowsElement element = session.FindElementById("InvalidRuntimeId");
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorFindElementByInvalidTagName()
        {
            WindowsElement element = session.FindElementByTagName("InvalidTagName");
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorFindElementByInvalidTagNameMalformed()
        {
            WindowsElement element = session.FindElementByTagName("//@InvalidTagNameMalformed");
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorFindElementByInvalidXPath()
        {
            WindowsElement element = session.FindElementByXPath("//*//]");
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorFindElementByNonExistentXPath()
        {
            WindowsElement alarmTab = session.FindElementByXPath("//*[@Name=\"NonExistentElement\"]");
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(OpenQA.Selenium.WebDriverException))]
        public void ErrorFindElementByUnsupportedLocatorCSSSelector()
        {
            WindowsElement element = session.FindElementByCssSelector("Query");
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(OpenQA.Selenium.WebDriverException))]
        public void ErrorFindElementByUnsupportedLocatorLinkText()
        {
            WindowsElement element = session.FindElementByLinkText("Query");
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(OpenQA.Selenium.WebDriverException))]
        public void ErrorFindElementByUnsupportedLocatorPartialLinkText()
        {
            WindowsElement element = session.FindElementByPartialLinkText("Query");
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        public void ErrorFindElementNoSuchWindow()
        {
            try
            {
                WindowsElement element = Utility.GetOrphanedSession().FindElementByAccessibilityId("An accessibility id") as WindowsElement;
                Assert.Fail("Exception should have been thrown");
            }
            catch (System.InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }

        [TestMethod]
        public void FindElementByTagName()
        {
            WindowsElement element = session.FindElementByTagName("Button");
            Assert.IsNotNull(element);
        }

        [TestMethod]
        public void FindElementByXPath()
        {
            WindowsElement alarmTab = session.FindElementByXPath("//Button[@AutomationId=\"MoreButton\"]");
            Assert.IsNotNull(alarmTab);
        }
    }
}

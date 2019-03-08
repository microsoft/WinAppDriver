//******************************************************************************
//
// Copyright (c) 2017 Microsoft Corporation. All rights reserved.
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
using System;
using System.Threading;

namespace WebDriverAPI
{
    [TestClass]
    public class ElementDisplayed : AlarmClockBase
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
        public void GetElementDisplayedState()
        {
            // Navigate to add alarm page
            session.FindElementByAccessibilityId("AddAlarmButton").Click();
            session.FindElementByAccessibilityId("AlarmTimePicker").Click();
            Thread.Sleep(TimeSpan.FromSeconds(1));

            var minuteSelector = session.FindElementByAccessibilityId("MinuteLoopingSelector");
            var minute00 = session.FindElementByName("00");
            var minute30 = session.FindElementByName("30");
            Assert.IsNotNull(minuteSelector);
            Assert.IsNotNull(minute00);
            Assert.IsNotNull(minute30);
            Assert.IsTrue(minute00.Displayed);
            Assert.IsFalse(minute30.Displayed);

            // Select minute 30 to scroll it into view
            minute30.Click();
            Assert.IsFalse(minute00.Displayed);
            Assert.IsTrue(minute30.Displayed);

            // Select minute 00 to scroll it back into view
            minute00.Click();
            Assert.IsTrue(minute00.Displayed);
            Assert.IsFalse(minute30.Displayed);

            // Dismiss add alarm page
            DismissAddAlarmPage();
        }

        [TestMethod]
        public void GetElementDisplayedStateError_NoSuchWindow()
        {
            try
            {
                var displayed = Utility.GetOrphanedElement().Displayed;
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }

        [TestMethod]
        public void GetElementDisplayedStateError_StaleElement()
        {
            try
            {
                var displayed = GetStaleElement().Displayed;
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.StaleElementReference, exception.Message);
            }
        }
    }
}

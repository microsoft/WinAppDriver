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
using System;

namespace WebDriverAPI
{
    [TestClass]
    public class ElementSelected : AlarmClockBase
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
        public void GetElementSelectedState()
        {
            WindowsElement elementWorldClock = session.FindElementByAccessibilityId(WorldClockTabAutomationId);
            WindowsElement elementAlarmClock = session.FindElementByAccessibilityId(AlarmTabAutomationId);

            elementWorldClock.Click();
            Assert.IsTrue(elementWorldClock.Selected);
            Assert.IsFalse(elementAlarmClock.Selected);

            elementAlarmClock.Click();
            Assert.IsFalse(elementWorldClock.Selected);
            Assert.IsTrue(elementAlarmClock.Selected);
        }

        [TestMethod]
        public void GetElementSelectedState_UnselectableElement()
        {
            WindowsElement elementAddButton = session.FindElementByAccessibilityId("AddAlarmButton");
            Assert.IsFalse(elementAddButton.Selected);
        }

        [TestMethod]
        public void GetElementSelectedStateError_NoSuchWindow()
        {
            try
            {
                var selected = Utility.GetOrphanedElement().Enabled;
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }

        [TestMethod]
        public void GetElementSelectedStateError_StaleElement()
        {
            try
            {
                var selected = GetStaleElement().Selected;
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.StaleElementReference, exception.Message);
            }
        }
    }
}

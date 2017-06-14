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
        public void ErrorGetElementSelectedStateNoSuchWindow()
        {
            try
            {
                var enabled = Utility.GetOrphanedElement().Enabled;
                Assert.Fail("Exception should have been thrown");
            }
            catch (System.InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }

        [TestMethod]
        public void GetElementSelectedState()
        {
            WindowsElement elementWorldClock = session.FindElementByAccessibilityId("WorldClockPivotItem");
            WindowsElement elementAlarmClock = session.FindElementByAccessibilityId("AlarmPivotItem");

            elementWorldClock.Click();
            Assert.IsTrue(elementWorldClock.Selected);
            Assert.IsFalse(elementAlarmClock.Selected);

            elementAlarmClock.Click();
            Assert.IsFalse(elementWorldClock.Selected);
            Assert.IsTrue(elementAlarmClock.Selected);
        }

        [TestMethod]
        public void ErrorFindUnselectableElement()
        {
            WindowsElement elementAddButton = session.FindElementByAccessibilityId("AddAlarmButton");
            Assert.IsFalse(elementAddButton.Selected);
        }
    }
}

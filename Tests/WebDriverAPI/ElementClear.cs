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

namespace WebDriverAPI
{
    [TestClass]
    public class ElementClear : AlarmClockBase
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
        public void ClearElement()
        {
            // Open a new alarm page and clear the alarm name repeatedly
            session.FindElementByAccessibilityId("AddAlarmButton").Click();
            WindowsElement textBox = session.FindElementByAccessibilityId("AlarmNameTextBox");
            Assert.AreNotEqual(string.Empty, textBox.Text);
            textBox.Clear();
            Assert.AreEqual(string.Empty, textBox.Text);

            textBox.SendKeys("Test alarm name text box!");
            Assert.AreNotEqual(string.Empty, textBox.Text);
            textBox.Clear();
            Assert.AreEqual(string.Empty, textBox.Text);
        }

        [TestMethod]
        public void ClearElementError_ElementNotVisible()
        {
            // Different Alarm & Clock application version uses different UI elements
            if (AlarmTabClassName == "ListViewItem")
            {
                // The latest Alarms & Clock application destroys the previous view instead of hiding it
            }
            else
            {
                // Navigate to Stopwatch tab and attempt to click on addAlarmButton that is no longer displayed
                WindowsElement addAlarmButton = session.FindElementByAccessibilityId("AddAlarmButton");
                session.FindElementByAccessibilityId(StopwatchTabAutomationId).Click();
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                Assert.IsFalse(addAlarmButton.Displayed);

                try
                {
                    addAlarmButton.Clear();
                    Assert.Fail("Exception should have been thrown");
                }
                catch (InvalidOperationException exception)
                {
                    Assert.AreEqual(ErrorStrings.ElementNotVisible, exception.Message);
                }
            }
        }

        [TestMethod]
        public void ClearElementError_NoSuchWindow()
        {
            try
            {
                Utility.GetOrphanedElement().Clear();
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }

        [TestMethod]
        public void ClearElementError_StaleElement()
        {
            try
            {
                GetStaleElement().Clear();
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.StaleElementReference, exception.Message);
            }
        }
    }
}

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

namespace W3CWebDriver
{
    [TestClass]
    public class ElementText : AlarmClockBase
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
        public void ErrorGetElementTextNoSuchWindow()
        {
            try
            {
                var text = Utility.GetOrphanedElement().Text;
                Assert.Fail("Exception should have been thrown");
            }
            catch (System.InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }

        [TestMethod]
        public void ErrorGetElementTextStaleElement()
        {
            try
            {
                var text = GetStaleElement().Text;
                Assert.Fail("Exception should have been thrown");
            }
            catch (System.InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.StaleElementReference, exception.Message);
            }
        }

        [TestMethod]
        public void GetElementText()
        {
            // Pivot Item element returns the name
            WindowsElement pivotItem = session.FindElementByAccessibilityId("StopwatchPivotItem");
            Assert.AreEqual("Stopwatch", pivotItem.Text);

            // Button element returns the button name
            WindowsElement button = session.FindElementByAccessibilityId("AddAlarmButton");
            Assert.AreEqual("Add new alarm", button.Text);
            button.Click();

            // TextBlock element returns the text value
            WindowsElement textBlock = session.FindElementByAccessibilityId("EditAlarmHeader");
            Assert.AreEqual("NEW ALARM", textBlock.Text);

            // List element returns the value of the selected list item
            WindowsElement list = session.FindElementByAccessibilityId("MinuteLoopingSelector");
            Assert.AreEqual("00", list.Text);

            // TextBox element returns the text value
            WindowsElement textBox = session.FindElementByAccessibilityId("AlarmNameTextBox");
            textBox.Clear();
            Assert.AreEqual(string.Empty, textBox.Text);
            textBox.SendKeys("Test alarm name text box!");
            Assert.AreEqual("Test alarm name text box!", textBox.Text);
        }
    }
}

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
using OpenQA.Selenium;

namespace UWPControls
{
    [TestClass]
    public class DatePicker : UWPControlsBase
    {
        private static WindowsElement datePickerElement1 = null;
        private static WindowsElement datePickerElement2 = null;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);
            NavigateTo("Selection and picker controls", "DatePicker");

            datePickerElement1 = session.FindElementByName("Pick a date");
            Assert.IsNotNull(datePickerElement1);
            datePickerElement2 = session.FindElementByAccessibilityId("Control2");
            Assert.IsNotNull(datePickerElement2);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TearDown();
        }

        [TestMethod]
        public void Click()
        {
            // Click datePickerElement1 to show the picker and simply dismiss it
            datePickerElement1.Click();
            var datePickerFlyout = session.FindElementByAccessibilityId("DatePickerFlyoutPresenter");
            Assert.IsNotNull(datePickerFlyout);
            Assert.IsTrue(datePickerFlyout.Displayed);
            session.FindElementByAccessibilityId("DatePickerFlyoutPresenter").FindElementByAccessibilityId("DismissButton").Click();
            System.Threading.Thread.Sleep(1000);

            // Click datePickerElement1 to show the picker and set the year to 2000
            datePickerElement1.Click();
            datePickerFlyout = session.FindElementByAccessibilityId("DatePickerFlyoutPresenter");
            Assert.IsNotNull(datePickerFlyout);
            var yearLoopingSelector = datePickerFlyout.FindElementByAccessibilityId("YearLoopingSelector");
            Assert.IsNotNull(yearLoopingSelector);
            var currentYear = yearLoopingSelector.Text;
            yearLoopingSelector.FindElementByName("2000").Click();
            System.Threading.Thread.Sleep(1000);
            Assert.AreNotEqual(currentYear, yearLoopingSelector.Text);
            datePickerFlyout.FindElementByAccessibilityId("AcceptButton").Click();
        }

        [TestMethod]
        public void Displayed()
        {
            Assert.IsTrue(datePickerElement1.Displayed);
            Assert.IsTrue(datePickerElement2.Displayed);
        }

        [TestMethod]
        public void Enabled()
        {
            Assert.IsTrue(datePickerElement1.Enabled);
            Assert.IsTrue(datePickerElement2.Enabled);
        }

        [TestMethod]
        public void Location()
        {
            Assert.IsTrue(datePickerElement2.Location.X >= datePickerElement1.Location.X);
            Assert.IsTrue(datePickerElement2.Location.Y >= datePickerElement1.Location.Y);
        }

        [TestMethod]
        public void LocationInView()
        {
            Assert.IsTrue(datePickerElement2.LocationOnScreenOnceScrolledIntoView.X >= datePickerElement1.LocationOnScreenOnceScrolledIntoView.X);
            Assert.IsTrue(datePickerElement2.LocationOnScreenOnceScrolledIntoView.Y >= datePickerElement1.LocationOnScreenOnceScrolledIntoView.Y);
        }

        [TestMethod]
        public void Name()
        {
            Assert.AreEqual("ControlType.Group", datePickerElement1.TagName);
            Assert.AreEqual("ControlType.Group", datePickerElement2.TagName);
        }

        [TestMethod]
        public void SendKeys()
        {
            datePickerElement1.SendKeys(Keys.Space);
            var datePickerFlyout = session.FindElementByAccessibilityId("DatePickerFlyoutPresenter");
            var day = datePickerFlyout.FindElementByAccessibilityId("DayLoopingSelector").Text;
            var month = datePickerFlyout.FindElementByAccessibilityId("MonthLoopingSelector").Text;
            var year = datePickerFlyout.FindElementByAccessibilityId("YearLoopingSelector").Text;

            // Alter the DatePicker entries using key presses
            datePickerFlyout.SendKeys(Keys.Up + Keys.Right + Keys.Down + Keys.Right + Keys.Up);
            Assert.AreNotEqual(day, datePickerFlyout.FindElementByAccessibilityId("DayLoopingSelector").Text);
            Assert.AreNotEqual(month, datePickerFlyout.FindElementByAccessibilityId("MonthLoopingSelector").Text);
            Assert.AreNotEqual(year, datePickerFlyout.FindElementByAccessibilityId("YearLoopingSelector").Text);
            datePickerFlyout.SendKeys(Keys.Enter);

            // Restore the DatePicker to original value. Note that datePickerFlyout needs to be looked up again after dismissal
            datePickerElement1.SendKeys(Keys.Space);
            datePickerFlyout = session.FindElementByAccessibilityId("DatePickerFlyoutPresenter");
            datePickerFlyout.SendKeys(Keys.Down + Keys.Right + Keys.Up + Keys.Right + Keys.Down);
            Assert.AreEqual(day, datePickerFlyout.FindElementByAccessibilityId("DayLoopingSelector").Text);
            Assert.AreEqual(month, datePickerFlyout.FindElementByAccessibilityId("MonthLoopingSelector").Text);
            Assert.AreEqual(year, datePickerFlyout.FindElementByAccessibilityId("YearLoopingSelector").Text);
            datePickerFlyout.SendKeys(Keys.Enter);
        }

        [TestMethod]
        public void Size()
        {
            Assert.IsTrue(datePickerElement1.Size.Width > 0);
            Assert.IsTrue(datePickerElement1.Size.Height > 0);
            Assert.IsTrue(datePickerElement2.Size.Width >= datePickerElement1.Size.Width);
            Assert.IsTrue(datePickerElement2.Size.Height <= datePickerElement1.Size.Height);
        }

        [TestMethod]
        public void Text()
        {
            datePickerElement2.Click();
            var datePickerFlyout = session.FindElementByAccessibilityId("DatePickerFlyoutPresenter");
            Assert.AreNotEqual(string.Empty, datePickerFlyout.FindElementByAccessibilityId("DayLoopingSelector").Text);
            Assert.AreNotEqual(string.Empty, datePickerFlyout.FindElementByAccessibilityId("MonthLoopingSelector").Text);
            datePickerFlyout.FindElementByAccessibilityId("DismissButton").Click();
        }
    }
}

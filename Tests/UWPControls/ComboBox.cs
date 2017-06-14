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
    public class ComboBox : UWPControlsBase
    {
        private static WindowsElement comboBoxElement1 = null;
        private static WindowsElement comboBoxElement2 = null;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);
            NavigateTo("Selection and picker controls", "ComboBox");

            comboBoxElement1 = session.FindElementByAccessibilityId("Combo1");
            Assert.IsNotNull(comboBoxElement1);
            comboBoxElement2 = session.FindElementByAccessibilityId("Combo2");
            Assert.IsNotNull(comboBoxElement2);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TearDown();
        }

        [TestMethod]
        public void Click()
        {
            // Click comboBoxElement1 to show the list and simply dismiss it
            var originalSelectedItem = comboBoxElement1.Text;
            comboBoxElement1.Click();
            comboBoxElement1.FindElementByAccessibilityId("Light Dismiss").Click();
            Assert.AreEqual(originalSelectedItem, comboBoxElement1.Text);

            // Click comboBoxElement1 to show the list and select an entry
            comboBoxElement1.Click();
            comboBoxElement1.FindElementByName("Yellow").Click();
            Assert.AreEqual("Yellow", comboBoxElement1.Text);
        }

        [TestMethod]
        public void Displayed()
        {
            Assert.IsTrue(comboBoxElement1.Displayed);
            Assert.IsTrue(comboBoxElement2.Displayed);
        }

        [TestMethod]
        public void Enabled()
        {
            Assert.IsTrue(comboBoxElement1.Enabled);
            Assert.IsTrue(comboBoxElement2.Enabled);
        }

        [TestMethod]
        public void Location()
        {
            Assert.IsTrue(comboBoxElement2.Location.X >= comboBoxElement1.Location.X);
            Assert.IsTrue(comboBoxElement2.Location.Y >= comboBoxElement1.Location.Y);
        }

        [TestMethod]
        public void LocationInView()
        {
            Assert.IsTrue(comboBoxElement2.LocationOnScreenOnceScrolledIntoView.X >= comboBoxElement1.LocationOnScreenOnceScrolledIntoView.X);
            Assert.IsTrue(comboBoxElement2.LocationOnScreenOnceScrolledIntoView.Y >= comboBoxElement1.LocationOnScreenOnceScrolledIntoView.Y);
        }

        [TestMethod]
        public void Name()
        {
            Assert.AreEqual("ControlType.ComboBox", comboBoxElement1.TagName);
            Assert.AreEqual("ControlType.ComboBox", comboBoxElement2.TagName);
        }

        [TestMethod]
        public void SendKeys()
        {
            // Use the cursor key to scroll through the entries in the combo box
            comboBoxElement1.SendKeys(Keys.Down + Keys.Up + Keys.Up + Keys.Up); // Ensure top entry is selected
            Assert.AreEqual("Blue", comboBoxElement1.Text);
            comboBoxElement1.SendKeys(Keys.Down);
            Assert.AreEqual("Green", comboBoxElement1.Text);
            comboBoxElement1.SendKeys(Keys.Up);
            Assert.AreEqual("Blue", comboBoxElement1.Text);
        }

        [TestMethod]
        public void Size()
        {
            Assert.IsTrue(comboBoxElement1.Size.Width > 0);
            Assert.IsTrue(comboBoxElement1.Size.Height > 0);
            Assert.IsTrue(comboBoxElement2.Size.Width >= comboBoxElement1.Size.Width);
            Assert.IsTrue(comboBoxElement2.Size.Height >= comboBoxElement1.Size.Height);
        }

        [TestMethod]
        public void Text()
        {
            comboBoxElement1.SendKeys(Keys.Down + Keys.Up + Keys.Up + Keys.Up); // Ensure top entry is selected
            Assert.AreEqual("Blue", comboBoxElement1.Text);
            Assert.AreEqual("Courier New", comboBoxElement2.Text);
        }
    }
}

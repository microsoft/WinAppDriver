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
using OpenQA.Selenium;

namespace UWPControls
{
    [TestClass]
    public class TextBox : UWPControlsBase
    {
        private WindowsElement textBoxElement1 = null;
        private WindowsElement textBoxElement2 = null;

        protected override void LoadScenarioView()
        {
            session.FindElementByAccessibilityId("splitViewToggle").Click();
            var splitViewPane = session.FindElementByClassName("SplitViewPane");
            splitViewPane.FindElementByName("Text controls").Click();
            splitViewPane.FindElementByName("TextBox").Click();
            System.Threading.Thread.Sleep(1000);

            // Locate the first 2 TextBox in the page and skip the search TextBox on the app bar
            var textBoxes = session.FindElementsByClassName("TextBox");
            Assert.IsTrue(textBoxes.Count > 2);
            textBoxElement1 = textBoxes[1];
            textBoxElement2 = textBoxes[2];
            Assert.IsNotNull(textBoxElement1);
            Assert.IsNotNull(textBoxElement2);
        }

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
        public void Clear()
        {
            Assert.AreEqual(string.Empty, textBoxElement1.Text);
            textBoxElement1.SendKeys("fghij67890^&*()");
            Assert.AreEqual("fghij67890^&*()", textBoxElement1.Text);

            textBoxElement1.Clear();
            Assert.AreEqual(string.Empty, textBoxElement1.Text);
        }

        [TestMethod]
        public void Click()
        {
            // Click textBoxElement1 to set focus and arbitrarily type
            Assert.AreEqual(string.Empty, textBoxElement1.Text);
            textBoxElement1.Click();
            session.Keyboard.SendKeys("1234567890");
            Assert.AreEqual("1234567890", textBoxElement1.Text);

            // Click textBoxElement2 to set focus and arbitrarily type
            Assert.AreEqual(string.Empty, textBoxElement2.Text);
            textBoxElement2.Click();
            session.Keyboard.SendKeys("1234567890");
            Assert.AreEqual("1234567890", textBoxElement2.Text);
        }

        [TestMethod]
        public void Displayed()
        {
            Assert.IsTrue(textBoxElement1.Displayed);
            Assert.IsTrue(textBoxElement2.Displayed);
        }

        [TestMethod]
        public void Enabled()
        {
            Assert.IsTrue(textBoxElement1.Enabled);
            Assert.IsTrue(textBoxElement2.Enabled);
        }

        [TestMethod]
        public void Location()
        {
            Assert.IsTrue(textBoxElement2.Location.X >= textBoxElement1.Location.X);
            Assert.IsTrue(textBoxElement2.Location.Y >= textBoxElement1.Location.Y);
        }

        [TestMethod]
        public void LocationInView()
        {
            Assert.IsTrue(textBoxElement2.LocationOnScreenOnceScrolledIntoView.X >= textBoxElement1.LocationOnScreenOnceScrolledIntoView.X);
            Assert.IsTrue(textBoxElement2.LocationOnScreenOnceScrolledIntoView.Y >= textBoxElement1.LocationOnScreenOnceScrolledIntoView.Y);
        }

        [TestMethod]
        public void Name()
        {
            Assert.AreEqual("ControlType.Edit", textBoxElement1.TagName);
            Assert.AreEqual("ControlType.Edit", textBoxElement2.TagName);
        }

        [TestMethod]
        public void SendKeys()
        {
            Assert.AreEqual(string.Empty, textBoxElement1.Text);
            textBoxElement1.SendKeys("abcde12345!@#$%");
            Assert.AreEqual("abcde12345!@#$%", textBoxElement1.Text);

            // Use Ctrl + A to select all text and backspace to clear the box
            textBoxElement1.SendKeys(Keys.Control + "a" + Keys.Control + Keys.Backspace);
            Assert.AreEqual(string.Empty, textBoxElement1.Text);

            Assert.AreEqual(string.Empty, textBoxElement2.Text);
            textBoxElement2.SendKeys("fghij67890^&*()");
            Assert.AreEqual("fghij67890^&*()", textBoxElement2.Text);
        }

        [TestMethod]
        public void Size()
        {
            Assert.IsTrue(textBoxElement1.Size.Width > 0);
            Assert.IsTrue(textBoxElement1.Size.Height > 0);
            Assert.IsTrue(textBoxElement2.Size.Width >= textBoxElement1.Size.Width);
            Assert.IsTrue(textBoxElement2.Size.Height >= textBoxElement1.Size.Height);
        }

        [TestMethod]
        public void Text()
        {
            Assert.AreEqual(string.Empty, textBoxElement1.Text);
            textBoxElement1.SendKeys("abcde12345!@#$%");
            Assert.AreEqual("abcde12345!@#$%", textBoxElement1.Text);

            // Highlight the first 3 characters in the textBox
            textBoxElement1.SendKeys(Keys.Home + Keys.Shift + Keys.Right + Keys.Right + Keys.Right + Keys.Shift);
            Assert.AreEqual("abc", textBoxElement1.Text); // Only the highlighted characters should be returned

            // Use Ctrl + A to select all text
            textBoxElement1.SendKeys(Keys.Control + "a" + Keys.Control);
            Assert.AreEqual("abcde12345!@#$%", textBoxElement1.Text);
        }
    }
}

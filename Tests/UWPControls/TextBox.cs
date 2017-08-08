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
    public class TextBox : UWPControlsBase
    {
        private static WindowsElement textBoxElement1 = null;
        private static WindowsElement textBoxElement2 = null;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);
            NavigateTo("Text controls", "TextBox");

            // Locate the first 2 TextBox in the page and skip the search TextBox on the app bar
            var textBoxes = session.FindElementsByClassName("TextBox");
            Assert.IsTrue(textBoxes.Count > 2);
            textBoxElement1 = textBoxes[1];
            textBoxElement2 = textBoxes[2];
            Assert.IsNotNull(textBoxElement1);
            Assert.IsNotNull(textBoxElement2);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TearDown();
        }

        [TestMethod]
        public void Clear()
        {
            textBoxElement1.Clear();
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
            textBoxElement1.Clear();
            Assert.AreEqual(string.Empty, textBoxElement1.Text);
            textBoxElement1.Click();
            session.Keyboard.SendKeys("1234567890");
            Assert.AreEqual("1234567890", textBoxElement1.Text);

            // Click textBoxElement2 to set focus and arbitrarily type
            textBoxElement2.Clear();
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
            textBoxElement1.Clear();
            Assert.AreEqual(string.Empty, textBoxElement1.Text);
            textBoxElement1.SendKeys("abcde12345!@#$%");
            Assert.AreEqual("abcde12345!@#$%", textBoxElement1.Text);

            // Use Ctrl + A to select all text and backspace to clear the box
            textBoxElement1.SendKeys(Keys.Control + "a" + Keys.Control + Keys.Backspace);
            Assert.AreEqual(string.Empty, textBoxElement1.Text);

            textBoxElement2.Clear();
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
            textBoxElement1.Clear();
            Assert.AreEqual(string.Empty, textBoxElement1.Text);
            textBoxElement1.SendKeys("abcde12345!@#$%");
            Assert.AreEqual("abcde12345!@#$%", textBoxElement1.Text);
        }
    }
}

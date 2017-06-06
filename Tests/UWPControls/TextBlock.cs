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
    public class TextBlock : UWPControlsBase
    {
        private static WindowsElement textBlockElement1 = null;
        private static WindowsElement textBlockElement2 = null;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);
            NavigateTo("Text controls", "TextBlock");

            textBlockElement1 = session.FindElementByName("I am a TextBlock.");
            Assert.IsNotNull(textBlockElement1);
            textBlockElement2 = session.FindElementByName("I am a styled TextBlock.");
            Assert.IsNotNull(textBlockElement2);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TearDown();
        }

        [TestMethod]
        public void Displayed()
        {
            Assert.IsTrue(textBlockElement1.Displayed);
            Assert.IsTrue(textBlockElement2.Displayed);
        }

        [TestMethod]
        public void Enabled()
        {
            Assert.IsTrue(textBlockElement1.Enabled);
            Assert.IsTrue(textBlockElement1.Enabled);
        }

        [TestMethod]
        public void Location()
        {
            Assert.IsTrue(textBlockElement2.Location.X >= textBlockElement1.Location.X);
            Assert.IsTrue(textBlockElement2.Location.Y >= textBlockElement1.Location.Y);
        }

        [TestMethod]
        public void LocationInView()
        {
            Assert.IsTrue(textBlockElement2.LocationOnScreenOnceScrolledIntoView.X >= textBlockElement1.LocationOnScreenOnceScrolledIntoView.X);
            Assert.IsTrue(textBlockElement2.LocationOnScreenOnceScrolledIntoView.Y >= textBlockElement1.LocationOnScreenOnceScrolledIntoView.Y);
        }

        [TestMethod]
        public void Name()
        {
            Assert.AreEqual("ControlType.Text", textBlockElement1.TagName);
            Assert.AreEqual("ControlType.Text", textBlockElement2.TagName);
        }

        [TestMethod]
        public void Size()
        {
            Assert.IsTrue(textBlockElement1.Size.Width > 0);
            Assert.IsTrue(textBlockElement1.Size.Height > 0);
            Assert.IsTrue(textBlockElement2.Size.Width >= textBlockElement1.Size.Width);
            Assert.IsTrue(textBlockElement2.Size.Height >= textBlockElement1.Size.Height);
        }

        [TestMethod]
        public void Text()
        {
            Assert.AreEqual("I am a TextBlock.", textBlockElement1.Text);
            Assert.AreEqual("I am a styled TextBlock.", textBlockElement2.Text);
        }
    }
}

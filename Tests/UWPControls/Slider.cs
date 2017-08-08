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
    public class Slider : UWPControlsBase
    {
        private static WindowsElement sliderElement1 = null;
        private static WindowsElement sliderElement2 = null;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);
            NavigateTo("Selection and picker controls", "Slider");

            sliderElement1 = session.FindElementByAccessibilityId("Slider1");
            Assert.IsNotNull(sliderElement1);
            sliderElement2 = session.FindElementByAccessibilityId("Slider2");
            Assert.IsNotNull(sliderElement2);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TearDown();
        }

        [TestMethod]
        public void Click()
        {
            sliderElement1.Click();
            Assert.IsTrue(int.Parse(sliderElement1.Text) > 45); // The value of the slider when the center is clicked should be greater than 45 and close to 50

            sliderElement2.Click();
            Assert.AreEqual("750", sliderElement2.Text);  // The value of the slider when the center is clicked
        }

        [TestMethod]
        public void Displayed()
        {
            Assert.IsTrue(sliderElement1.Displayed);
            Assert.IsTrue(sliderElement2.Displayed);
        }

        [TestMethod]
        public void Enabled()
        {
            Assert.IsTrue(sliderElement1.Enabled);
            Assert.IsTrue(sliderElement2.Enabled);
        }

        [TestMethod]
        public void Location()
        {
            Assert.IsTrue(sliderElement1.Location.X >= sliderElement1.Location.X);
            Assert.IsTrue(sliderElement1.Location.Y >= sliderElement1.Location.Y);
        }

        [TestMethod]
        public void LocationInView()
        {
            Assert.IsTrue(sliderElement2.LocationOnScreenOnceScrolledIntoView.X >= sliderElement1.LocationOnScreenOnceScrolledIntoView.X);
            Assert.IsTrue(sliderElement2.LocationOnScreenOnceScrolledIntoView.Y >= sliderElement1.LocationOnScreenOnceScrolledIntoView.Y);
        }

        [TestMethod]
        public void Name()
        {
            Assert.AreEqual("ControlType.Slider", sliderElement1.TagName);
            Assert.AreEqual("ControlType.Slider", sliderElement2.TagName);
        }

        [TestMethod]
        public void SendKeys()
        {
            var originalValue = sliderElement1.Text;

            // Pressing right arrow will move the slider right and the value should increase by 1
            sliderElement1.SendKeys(Keys.Right);
            Assert.AreEqual(int.Parse(originalValue) + 1, int.Parse(sliderElement1.Text));

            // Pressing left arrow will move the slider back to the original value
            sliderElement1.SendKeys(Keys.Left);
            Assert.AreEqual(originalValue, sliderElement1.Text);
        }

        [TestMethod]
        public void Size()
        {
            Assert.IsTrue(sliderElement1.Size.Width > 0);
            Assert.IsTrue(sliderElement1.Size.Height > 0);
        }

        [TestMethod]
        public void Text()
        {
            sliderElement1.Click();
            Assert.IsTrue(int.Parse(sliderElement1.Text) > 45); // The value of the slider when the center is clicked should be greater than 45 and close to 50

            sliderElement2.Click();
            Assert.AreEqual(750, int.Parse(sliderElement2.Text)); // The value of the slider when the center is clicked should be 750
        }
    }
}

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

namespace UWPControls
{
    [TestClass]
    public class Slider : UWPControlsBase
    {
        private WindowsElement sliderElement1 = null;
        private WindowsElement sliderElement2 = null;

        protected override void LoadScenarioView()
        {
            session.FindElementByAccessibilityId("splitViewToggle").Click();
            var splitViewPane = session.FindElementByClassName("SplitViewPane");
            splitViewPane.FindElementByName("Selection and picker controls").Click();
            splitViewPane.FindElementByName("Slider").Click();

            sliderElement1 = session.FindElementByAccessibilityId("Slider1");
            Assert.IsNotNull(sliderElement1);
            sliderElement2 = session.FindElementByAccessibilityId("Slider2");
            Assert.IsNotNull(sliderElement2);
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
        public void Size()
        {
            Assert.IsTrue(sliderElement1.Size.Width > 0);
            Assert.IsTrue(sliderElement1.Size.Height > 0);
        }
    }
}

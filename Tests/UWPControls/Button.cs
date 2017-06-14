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
    public class Button : UWPControlsBase
    {
        private static WindowsElement buttonElement = null;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);
            NavigateTo("Buttons", "Button");

            buttonElement = session.FindElementByAccessibilityId("Button1");
            Assert.IsNotNull(buttonElement);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TearDown();
        }

        [TestMethod]
        public void Click()
        {
            var buttonEventOutput = session.FindElementByAccessibilityId("Control1Output");
            Assert.AreEqual(string.Empty, buttonEventOutput.Text);

            buttonElement.Click();
            Assert.AreEqual("You clicked: Button1", buttonEventOutput.Text);
        }

        [TestMethod]
        public void Displayed()
        {
            Assert.IsTrue(buttonElement.Displayed);
        }

        [TestMethod]
        public void Enabled()
        {
            var disableButtonCheckbox = session.FindElementByAccessibilityId("DisableButton1");
            Assert.IsTrue(buttonElement.Enabled);

            disableButtonCheckbox.Click();
            Assert.IsFalse(buttonElement.Enabled);

            disableButtonCheckbox.Click();
            Assert.IsTrue(buttonElement.Enabled);
        }

        [TestMethod]
        public void Location()
        {
            var disableButtonCheckbox = session.FindElementByAccessibilityId("DisableButton1");
            Assert.IsTrue(buttonElement.Location.X >= disableButtonCheckbox.Location.X);
            Assert.IsTrue(buttonElement.Location.Y >= disableButtonCheckbox.Location.Y);
        }

        [TestMethod]
        public void LocationInView()
        {
            var disableButtonCheckbox = session.FindElementByAccessibilityId("DisableButton1");
            Assert.IsTrue(buttonElement.LocationOnScreenOnceScrolledIntoView.X >= disableButtonCheckbox.LocationOnScreenOnceScrolledIntoView.X);
            Assert.IsTrue(buttonElement.LocationOnScreenOnceScrolledIntoView.Y >= disableButtonCheckbox.LocationOnScreenOnceScrolledIntoView.Y);
        }

        [TestMethod]
        public void Name()
        {
            Assert.AreEqual("ControlType.Button", buttonElement.TagName);
        }

        [TestMethod]
        public void Size()
        {
            Assert.IsTrue(buttonElement.Size.Width > 0);
            Assert.IsTrue(buttonElement.Size.Height > 0);
        }

        [TestMethod]
        public void Text()
        {
            Assert.AreEqual("Button", buttonElement.Text);
        }
    }
}

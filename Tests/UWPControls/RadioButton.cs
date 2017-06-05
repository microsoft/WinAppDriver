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
    public class RadioButton : UWPControlsBase
    {
        private static WindowsElement radioButtonElement1 = null;
        private static WindowsElement radioButtonElement2 = null;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);
            NavigateTo("Selection and picker controls", "RadioButton");

            radioButtonElement1 = session.FindElementByAccessibilityId("Option1RadioButton");
            Assert.IsNotNull(radioButtonElement1);
            radioButtonElement2 = session.FindElementByAccessibilityId("Option2RadioButton");
            Assert.IsNotNull(radioButtonElement2);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TearDown();
        }

        [TestMethod]
        public void Click()
        {
            var radioButtonEventOutput = session.FindElementByAccessibilityId("Control1Output");

            radioButtonElement1.Click();
            Assert.AreEqual("You selected option 1.", radioButtonEventOutput.Text);

            radioButtonElement2.Click();
            Assert.AreEqual("You selected option 2.", radioButtonEventOutput.Text);
        }

        [TestMethod]
        public void Displayed()
        {
            Assert.IsTrue(radioButtonElement1.Displayed);
            Assert.IsTrue(radioButtonElement2.Displayed);
        }

        [TestMethod]
        public void Enabled()
        {
            Assert.IsTrue(radioButtonElement1.Enabled);
            Assert.IsTrue(radioButtonElement2.Enabled);
        }

        [TestMethod]
        public void Location()
        {
            Assert.IsTrue(radioButtonElement2.Location.X >= radioButtonElement1.Location.X);
            Assert.IsTrue(radioButtonElement2.Location.Y >= radioButtonElement1.Location.Y);
        }

        [TestMethod]
        public void LocationInView()
        {
            Assert.IsTrue(radioButtonElement2.LocationOnScreenOnceScrolledIntoView.X >= radioButtonElement1.LocationOnScreenOnceScrolledIntoView.X);
            Assert.IsTrue(radioButtonElement2.LocationOnScreenOnceScrolledIntoView.Y >= radioButtonElement1.LocationOnScreenOnceScrolledIntoView.Y);
        }

        [TestMethod]
        public void Name()
        {
            Assert.AreEqual("ControlType.RadioButton", radioButtonElement1.TagName);
            Assert.AreEqual("ControlType.RadioButton", radioButtonElement2.TagName);
        }

        [TestMethod]
        public void Selected()
        {
            radioButtonElement1.Click();
            Assert.IsTrue(radioButtonElement1.Selected);
            Assert.IsFalse(radioButtonElement2.Selected);

            radioButtonElement2.Click();
            Assert.IsTrue(radioButtonElement2.Selected);
            Assert.IsFalse(radioButtonElement1.Selected);
        }

        [TestMethod]
        public void Size()
        {
            Assert.IsTrue(radioButtonElement1.Size.Width > 0);
            Assert.IsTrue(radioButtonElement1.Size.Height > 0);
            Assert.AreEqual(radioButtonElement1.Size.Width, radioButtonElement2.Size.Width);
            Assert.AreEqual(radioButtonElement1.Size.Height, radioButtonElement2.Size.Height);
        }

        [TestMethod]
        public void Text()
        {
            Assert.AreEqual("Option 1", radioButtonElement1.Text);
            Assert.AreEqual("Option 2", radioButtonElement2.Text);
        }
    }
}

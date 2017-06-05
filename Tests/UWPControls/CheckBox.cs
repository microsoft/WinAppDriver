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
    public class CheckBox : UWPControlsBase
    {
        private static WindowsElement checkBoxElement1 = null;
        private static WindowsElement checkBoxElement2 = null;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);
            NavigateTo("Selection and picker controls", "CheckBox");

            checkBoxElement1 = session.FindElementByName("Two-state CheckBox");
            checkBoxElement2 = session.FindElementByName("Three-state CheckBox");
            Assert.IsNotNull(checkBoxElement2);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TearDown();
        }

        [TestMethod]
        public void Click()
        {
            var checkBoxEventOutput = session.FindElementByAccessibilityId("Control2Output");
            Assert.AreEqual(string.Empty, checkBoxEventOutput.Text);

            checkBoxElement2.Click();
            Assert.AreEqual("CheckBox is checked.", checkBoxEventOutput.Text);

            checkBoxElement2.Click();
            Assert.AreEqual("CheckBox state is indeterminate.", checkBoxEventOutput.Text);

            checkBoxElement2.Click();
            Assert.AreEqual("CheckBox is unchecked.", checkBoxEventOutput.Text);
        }

        [TestMethod]
        public void Displayed()
        {
            Assert.IsTrue(checkBoxElement1.Displayed);
            Assert.IsTrue(checkBoxElement2.Displayed);
        }

        [TestMethod]
        public void Enabled()
        {
            Assert.IsTrue(checkBoxElement1.Enabled);
            Assert.IsTrue(checkBoxElement2.Enabled);
        }

        [TestMethod]
        public void Location()
        {
            Assert.IsTrue(checkBoxElement2.Location.X >= checkBoxElement1.Location.X);
            Assert.IsTrue(checkBoxElement2.Location.Y >= checkBoxElement1.Location.Y);
        }

        [TestMethod]
        public void LocationInView()
        {
            Assert.IsTrue(checkBoxElement2.LocationOnScreenOnceScrolledIntoView.X >= checkBoxElement1.LocationOnScreenOnceScrolledIntoView.X);
            Assert.IsTrue(checkBoxElement2.LocationOnScreenOnceScrolledIntoView.Y >= checkBoxElement1.LocationOnScreenOnceScrolledIntoView.Y);
        }

        [TestMethod]
        public void Name()
        {
            Assert.AreEqual("ControlType.CheckBox", checkBoxElement1.TagName);
            Assert.AreEqual("ControlType.CheckBox", checkBoxElement2.TagName);
        }

        [TestMethod]
        public void Selected()
        {
            var originalState = checkBoxElement1.Selected;
            checkBoxElement1.Click();
            Assert.AreNotEqual(originalState, checkBoxElement1.Selected);
            checkBoxElement1.Click();
            Assert.AreEqual(originalState, checkBoxElement1.Selected);
        }

        [TestMethod]
        public void Size()
        {
            Assert.IsTrue(checkBoxElement1.Size.Width > 0);
            Assert.IsTrue(checkBoxElement1.Size.Height > 0);
            Assert.IsTrue(checkBoxElement1.Size.Width <= checkBoxElement2.Size.Width);
            Assert.AreEqual(checkBoxElement1.Size.Height, checkBoxElement2.Size.Height);
        }

        [TestMethod]
        public void Text()
        {
            Assert.AreEqual("Two-state CheckBox", checkBoxElement1.Text);
            Assert.AreEqual("Three-state CheckBox", checkBoxElement2.Text);
        }
    }
}

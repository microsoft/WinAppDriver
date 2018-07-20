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
using System.Threading;
using System.Drawing;
using System;

namespace WebDriverAPI
{
    [TestClass]
    public class TouchDownMoveUp : CalculatorBase
    {
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

        [TestInitialize]
        public virtual void TestInit()
        {
            // Set focus on the application by switching window to itself
            session.SwitchTo().Window(session.CurrentWindowHandle);
        }

        [TestMethod]
        public void TouchDownMoveUp_DragAndDrop()
        {
            WindowsElement appNameTitle = session.FindCalculatorTitleByAccessibilityId();
            Point titleBarLocation = new Point(appNameTitle.Location.X + appNameTitle.Size.Width / 2, appNameTitle.Location.Y + appNameTitle.Size.Height / 2);
            const int offset = 100;

            // Save application window original position
            Point originalPosition = session.Manage().Window.Position;
            Assert.IsNotNull(originalPosition);

            // Send touch down, move, and up actions combination to perform a drag and drop 
            // action on the app title bar. These actions reposition Calculator window.
            touchScreen.Down(titleBarLocation.X, titleBarLocation.Y);
            touchScreen.Move(titleBarLocation.X + offset, titleBarLocation.Y + offset);
            touchScreen.Up(titleBarLocation.X + offset, titleBarLocation.Y + offset);
            Thread.Sleep(TimeSpan.FromSeconds(1));

            // Verify that application window is now re-positioned from the original location
            Assert.AreNotEqual(originalPosition, session.Manage().Window.Position);
            Assert.IsTrue(originalPosition.Y < session.Manage().Window.Position.Y);

            // Restore application window original position
            session.Manage().Window.Position = originalPosition;
            Assert.AreEqual(originalPosition, session.Manage().Window.Position);
        }

        [TestMethod]
        public void TouchDownMoveUp_SingleTap()
        {
            WindowsElement num8Button = session.FindElementByAccessibilityId("num8Button");
            WindowsElement clearButton = session.FindElementByAccessibilityId("clearButton");
            WindowsElement calculatorResult = session.FindElementByAccessibilityId("CalculatorResults");

            // Send touch down, move, and up actions combination to perform a single tap on the number 8 button
            Point num8ButtonLocation = new Point(num8Button.Location.X + num8Button.Size.Width / 2, num8Button.Location.Y + num8Button.Size.Height / 2);
            touchScreen.Down(num8ButtonLocation.X, num8ButtonLocation.Y);
            touchScreen.Up(num8ButtonLocation.X, num8ButtonLocation.Y);
            Thread.Sleep(TimeSpan.FromSeconds(0.5));
            Assert.AreEqual("8", calculatorResult.Text.Replace("Display is", string.Empty).Trim());

            // Send touch down, move, and up actions combination to perform a single tap on the clear button
            Point clearButtonLocation = new Point(clearButton.Location.X + clearButton.Size.Width / 2, clearButton.Location.Y + clearButton.Size.Height / 2);
            touchScreen.Down(clearButtonLocation.X, clearButtonLocation.Y);
            touchScreen.Up(clearButtonLocation.X, clearButtonLocation.Y);
            Thread.Sleep(TimeSpan.FromSeconds(0.5));
            Assert.AreEqual("0", calculatorResult.Text.Replace("Display is", string.Empty).Trim());
        }
    }
}

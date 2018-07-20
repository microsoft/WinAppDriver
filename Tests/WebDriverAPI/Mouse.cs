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
    public class Mouse : CalculatorBase
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

            // Restore the Calculator window if it is currently maximized
            WindowsElement maximizeButton = session.FindElementByAccessibilityId("Maximize");
            if (!maximizeButton.Text.Contains("Maximize"))
            {
                maximizeButton.Click();
            }
        }

        [TestMethod]
        public void MouseClick()
        {
            // Locate the calculatorResult element
            WindowsElement calculatorResult = session.FindElementByAccessibilityId("CalculatorResults");
            Assert.IsNotNull(calculatorResult);

            // Implicitly invoke /session/:sessionId/moveto and /session/:sessionId/click
            WindowsElement num8Button = session.FindElementByAccessibilityId("num8Button");
            session.Mouse.Click(num8Button.Coordinates);
            Assert.AreEqual("8", calculatorResult.Text.Replace("Display is", string.Empty).Trim());

            // Explicitly invoke /session/:sessionId/moveto and then /session/:sessionId/click on the current position
            WindowsElement clearButton = session.FindElementByAccessibilityId("clearButton");
            session.Mouse.MouseMove(clearButton.Coordinates);
            session.Mouse.Click(null);
            Assert.AreEqual("0", calculatorResult.Text.Replace("Display is", string.Empty).Trim());

            // Open a context menu on the application title bar to expose the context menu and verify that it contains minimize.
            // The context menu is parented on the desktop instead of the application. Thus, a desktop session is used to find it.
            // This command implicitly invoke /session/:sessionId/moveto and /session/:sessionId/click with button 2 parameter
            WindowsElement appNameTitle = session.FindCalculatorTitleByAccessibilityId();
            session.Mouse.ContextClick(appNameTitle.Coordinates);
            Thread.Sleep(TimeSpan.FromSeconds(1.5));
            WindowsDriver<WindowsElement> desktopSession = Utility.CreateNewSession(CommonTestSettings.DesktopAppId);
            try
            {
                Assert.IsNotNull(desktopSession.FindElementByName("System").FindElementByName("Minimize"));
                clearButton.Click(); // Dismiss the context menu
            }
            finally
            {
                if (desktopSession != null)
                {
                    desktopSession.Quit();
                }
            }
        }

        [TestMethod]
        public void MouseDoubleClick()
        {
            WindowsElement maximizeButton = session.FindElementByAccessibilityId("Maximize");
            Assert.IsNotNull(maximizeButton);

            // Verify that the window is currently not maximized
            Assert.IsTrue(maximizeButton.Text.Contains("Maximize"));

            // Perform mouse double click on the title bar to maximize the Calculator window
            WindowsElement appNameTitle = session.FindCalculatorTitleByAccessibilityId();
            session.Mouse.MouseMove(appNameTitle.Coordinates);
            session.Mouse.DoubleClick(null); // Pass null as this command omit the given parameter
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Assert.IsFalse(maximizeButton.Text.Contains("Maximize"));

            // Perform mouse double click on the title bar to restore the Calculator window
            session.Mouse.MouseMove(appNameTitle.Coordinates);
            session.Mouse.DoubleClick(null); // Pass null as this command omit the given parameter
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Assert.IsTrue(maximizeButton.Text.Contains("Maximize"));
        }

        [TestMethod]
        public void MouseDownMoveUp()
        {
            const int offset = 100;
            WindowsElement appNameTitle = session.FindCalculatorTitleByAccessibilityId();
            Assert.IsNotNull(appNameTitle);

            // Save application window original position
            Point originalPosition = session.Manage().Window.Position;
            Assert.IsNotNull(originalPosition);

            // Send mouse down, move, and up actions combination to perform a drag and drop 
            // action on the app title bar. These actions reposition Calculator window.
            session.Mouse.MouseMove(appNameTitle.Coordinates);
            session.Mouse.MouseDown(null); // Pass null as this command omit the given parameter
            session.Mouse.MouseMove(appNameTitle.Coordinates, offset, offset);
            session.Mouse.MouseUp(null); // Pass null as this command omit the given parameter
            Thread.Sleep(TimeSpan.FromSeconds(1));

            // Verify that application window is now re-positioned from the original location
            Assert.AreNotEqual(originalPosition, session.Manage().Window.Position);
            Assert.IsTrue(originalPosition.Y < session.Manage().Window.Position.Y);

            // Restore application window original position
            session.Manage().Window.Position = originalPosition;
            Assert.AreEqual(originalPosition, session.Manage().Window.Position);
        }
    }
}

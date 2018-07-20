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
using System.Threading;
using System;

namespace WebDriverAPI
{
    [TestClass]
    public class TouchDoubleClick : CalculatorBase
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

        [TestMethod]
        public void TouchDoubleTap()
        {
            WindowsElement appNameTitle = session.FindCalculatorTitleByAccessibilityId();
            WindowsElement maximizeButton = session.FindElementByAccessibilityId("Maximize");

            // Set focus on the application by switching window to itself
            session.SwitchTo().Window(session.CurrentWindowHandle);

            // Restore the calculator window if it is currently maximized
            if (!maximizeButton.Text.Contains("Maximize"))
            {
                maximizeButton.Click();
            }

            // Verify that window is currently not maximized
            Assert.IsTrue(maximizeButton.Text.Contains("Maximize"));

            // Perform double tap touch on the title bar to maximize the calculator window
            Assert.IsNotNull(touchScreen);
            touchScreen.DoubleTap(appNameTitle.Coordinates);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Assert.IsFalse(maximizeButton.Text.Contains("Maximize"));

            // Perform double tap touch on the title bar to restore the calculator window
            Assert.IsNotNull(touchScreen);
            touchScreen.DoubleTap(appNameTitle.Coordinates);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            Assert.IsTrue(maximizeButton.Text.Contains("Maximize"));
        }

        [TestMethod]
        public void TouchDoubleTapError_StaleElement()
        {
            try
            {
                // Perform double tap touch on stale element
                touchScreen.DoubleTap(GetStaleElement().Coordinates);
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.StaleElementReference, exception.Message);
            }
        }
    }
}

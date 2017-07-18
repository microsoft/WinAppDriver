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
using System;
using System.Drawing;
using System.Threading;

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
            // Save application window original size and position
            var originalSize = session.Manage().Window.Size;
            var originalPosition = session.Manage().Window.Position;

            // Get maximized window size
            session.Manage().Window.Maximize();
            var maximizedSize = session.Manage().Window.Size;
            Assert.IsNotNull(maximizedSize);

            // Shrink the window size by 100 pixels each side from maximized size to ensure size changes when maximized
            int offset = 500;
            session.Manage().Window.Size = new Size(maximizedSize.Width - offset, maximizedSize.Height - offset);
            session.Manage().Window.Position = new Point(offset, offset);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            var currentWindowSize = session.Manage().Window.Size;
            Assert.IsNotNull(currentWindowSize);
            Assert.IsTrue(maximizedSize.Width >= currentWindowSize.Width);
            Assert.IsTrue(maximizedSize.Height >= currentWindowSize.Height);

            // Perform double tap touch on the title bar to maximize calculator window
            Assert.IsNotNull(touchScreen);
            touchScreen.DoubleTap(session.FindElementByAccessibilityId("AppNameTitle").Coordinates);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            currentWindowSize = session.Manage().Window.Size;
            Assert.IsNotNull(currentWindowSize);
            Assert.AreEqual(maximizedSize.Width, currentWindowSize.Width);
            Assert.AreEqual(maximizedSize.Height, currentWindowSize.Height);

            // Restore application window original size and position
            session.Manage().Window.Size = originalSize;
            session.Manage().Window.Position = originalPosition;
            Thread.Sleep(TimeSpan.FromSeconds(1));
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

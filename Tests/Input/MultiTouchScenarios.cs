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

using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Input
{
    [TestClass]
    public class MultiTouchScenarios : TestBase
    {
        #region Test lifecycle code
        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            BaseSetup(context);
        }

        [ClassCleanup]
        public static void TearDown()
        {
            BaseTearDown();
        }

        [TestInitialize]
        public void Clear()
        {
            var clear = AppSession.FindElementByAccessibilityId("Clear");
            TouchScreen.SingleTap(clear.Coordinates);
            Assert.AreEqual(string.Empty, _GetResultText());
        }
        #endregion

        /// <summary>
        /// Multiple fingers touch the screen and move in the same direction.
        /// Manipulation gesture.
        /// </summary>
        [TestMethod, TestCategory("MultiTouch")]
        public void MultiSlide()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Two or more fingers touch the screen and move a short distance in the same direction.
        /// Manipulation gesture.
        /// </summary>
        [TestMethod, TestCategory("MultiTouch")]
        public void MultiSwipe()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Multiple fingers touch the screen and move in a clockwise or counter-clockwise arc.
        /// Manipulation gesture.
        /// AKA Rotate
        /// </summary>
        [TestMethod, TestCategory("MultiTouch")]
        public void Turn()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Multiple fingers touch the screen and move closer together.
        /// Manipulation gesture.
        /// </summary>
        [TestMethod, TestCategory("MultiTouch")]
        public void Pinch()
        {
            var touchable = AppSession.FindElementByAccessibilityId("Touchable");
            var coords = touchable.Coordinates.LocationInViewport;
            var startSize = new Size(touchable.Size.Width, touchable.Size.Height);
            var x = coords.X + startSize.Width / 2;
            var y = coords.Y + startSize.Height / 2;

            // The Pinch method assumes a 100px difference between the start/end points.
            // Add half (50) to offset the touch to the center of the element.
            AppSession.Pinch(x, y + 50);
            // AppSession.Pinch(touchable); Not supported yet

            Assert.IsTrue(startSize.Height > touchable.Size.Height);
            Assert.IsTrue(startSize.Width > touchable.Size.Width);
        }

        /// <summary>
        /// Multiple fingers touch the screen and move farther apart.
        /// Manipulation gesture.
        /// AKA Zoom
        /// </summary>
        [TestMethod, TestCategory("MultiTouch")]
        public void Stretch()
        {
            var touchable = AppSession.FindElementByAccessibilityId("Touchable");
            var coords = touchable.Coordinates.LocationInViewport;
            var startSize = new Size(touchable.Size.Width, touchable.Size.Height);
            var x = coords.X + startSize.Width / 2;
            var y = coords.Y + startSize.Height / 2;

            // The Zoom method assumes a 100px difference between the start/end points.
            // Add half (50) to offset the touch to the center of the element.
            AppSession.Zoom(x, y + 50);
            // AppSession.Zoom(touchable); Not supported yet

            Assert.IsTrue(startSize.Height < touchable.Size.Height);
            Assert.IsTrue(startSize.Width < touchable.Size.Width);
        }
    }
}

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

namespace Input
{
    [TestClass]
    public class SingleTouchScenarios : TestBase
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
        /// One finger touches the screen and lifts up.
        /// Static gesture.
        /// </summary>
        [TestMethod, TestCategory("SingleTouch")]
        public void Tap()
        {
            var touchable = AppSession.FindElementByAccessibilityId("Touchable");
            TouchScreen.SingleTap(touchable.Coordinates);
            Assert.AreEqual("Tapped", _GetLastResultString());
        }

        /// <summary>
        /// One finger touches the screen and lifts up twice, in quick succession.
        /// Static gesture.
        /// </summary>
        [TestMethod, TestCategory("SingleTouch")]
        public void DoubleTap()
        {
            var touchable = AppSession.FindElementByAccessibilityId("Touchable");
            TouchScreen.DoubleTap(touchable.Coordinates);
            Assert.AreEqual("DoubleTapped", _GetLastResultString());
        }

        /// <summary>
        /// One finger touches the screen and stays in place.
        /// Static gesture.
        /// </summary>
        [TestMethod, TestCategory("SingleTouch")]
        public void PressAndHold()
        {
            var touchable = AppSession.FindElementByAccessibilityId("Touchable");
            TouchScreen.LongPress(touchable.Coordinates);
            Assert.AreEqual("Holding", _GetFirstResultString());
        }

        /// <summary>
        /// One finger touches the screen and stays in place for a moment, then releases.
        /// Static gesture.
        /// </summary>
        [TestMethod, TestCategory("SingleTouch")]
        public void RightTapped()
        {
            var touchable = AppSession.FindElementByAccessibilityId("Touchable");
            TouchScreen.LongPress(touchable.Coordinates);
            Assert.AreEqual("RightTapped", _GetLastResultString());
        }
        
        /// <summary>
        /// One finger touches the screen and moves in a direction.
        /// Manipulation gesture.
        /// </summary>
        [TestMethod, TestCategory("SingleTouch")]
        public void Slide()
        {
            var touchable = AppSession.FindElementByAccessibilityId("Touchable");
            var startCoords = touchable.Coordinates.LocationInViewport;
            var endX = startCoords.X + TouchDistance.Long;
            var endY = startCoords.Y + TouchDistance.Long;

            TouchScreen.Down(startCoords.X, startCoords.Y);
            TouchScreen.Up(endX, endY);

            var endCoords = touchable.Coordinates.LocationInViewport;
            Assert.IsTrue(endCoords.X > startCoords.X);
            Assert.IsTrue(endCoords.Y > startCoords.Y);
        }

        /// <summary>
        /// One finger touches the screen and moves a short distance in a direction.
        /// Manipulation gesture.
        /// </summary>
        [TestMethod, TestCategory("SingleTouch")]
        public void Swipe()
        {
            var touchable = AppSession.FindElementByAccessibilityId("Touchable");
            var startCoords = touchable.Coordinates.LocationInViewport;

            // Not supported
            //var startX = startCoords.X + touchable.Size.Width / 2;
            //var startY = startCoords.Y + touchable.Size.Width / 2;
            //var endX = startX + TouchDistance.Short;
            //var endY = startY + TouchDistance.Short;
            //AppSession.Swipe(startX, startY, endX, endY, TouchDuration.Short);

            TouchScreen.Flick(touchable.Coordinates, TouchDistance.Short, TouchDistance.Short, TouchSpeed.Slow);
            
            var endCoords = touchable.Coordinates.LocationInViewport;
            Assert.IsTrue(endCoords.X > startCoords.X);
            Assert.IsTrue(endCoords.Y > startCoords.Y);
            Assert.IsTrue(endCoords.X - startCoords.X <= TouchDistance.Short);
            Assert.IsTrue(endCoords.Y - startCoords.Y <= TouchDistance.Short);
        }
    }
}

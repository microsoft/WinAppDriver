//******************************************************************************
//
// Copyright (c) 2018 Microsoft Corporation. All rights reserved.
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
using System.Collections.Generic;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Appium.Interactions;
using OpenQA.Selenium.Appium.Windows;

// Define an alias to OpenQA.Selenium.Appium.Interactions.PointerInputDevice to hide
// inherited OpenQA.Selenium.Interactions.PointerInputDevice that causes ambiguity.
// In the future, all functions of OpenQA.Selenium.Appium.Interactions should be moved
// up to OpenQA.Selenium.Interactions and this alias can simply be removed.
using PointerInputDevice = OpenQA.Selenium.Appium.Interactions.PointerInputDevice;

namespace Paint3DTest
{
    [TestClass]
    public class ScenarioZoom : Paint3DSession
    {
        private WindowsElement zoomInteractor;
        private WindowsElement zoomScaleTextBox;

        [TestMethod]
        public void ZoomingInMultiTouch()
        {
            // Drag a touch contact diagonally in NE direction distancing apart from the other contact point
            PointerInputDevice touch1 = new PointerInputDevice(PointerKind.Touch);
            ActionSequence touch1Sequence = new ActionSequence(touch1, 0);
            touch1Sequence.AddAction(touch1.CreatePointerMove(zoomInteractor, 50, -50, TimeSpan.Zero));
            touch1Sequence.AddAction(touch1.CreatePointerDown(PointerButton.TouchContact));
            touch1Sequence.AddAction(touch1.CreatePointerMove(zoomInteractor, 55, -55, TimeSpan.Zero));
            touch1Sequence.AddAction(touch1.CreatePointerMove(zoomInteractor, 60, -60, TimeSpan.Zero));
            touch1Sequence.AddAction(touch1.CreatePointerMove(zoomInteractor, 65, -65, TimeSpan.Zero));
            touch1Sequence.AddAction(touch1.CreatePointerMove(zoomInteractor, 70, -70, TimeSpan.Zero));
            touch1Sequence.AddAction(touch1.CreatePointerMove(zoomInteractor, 75, -75, TimeSpan.Zero));
            touch1Sequence.AddAction(touch1.CreatePointerMove(zoomInteractor, 80, -80, TimeSpan.Zero));
            touch1Sequence.AddAction(touch1.CreatePointerUp(PointerButton.TouchContact));

            // Drag a touch contact diagonally in SW direction distancing apart from the other contact point
            PointerInputDevice touch2 = new PointerInputDevice(PointerKind.Touch);
            ActionSequence touch2Sequence = new ActionSequence(touch2, 0);
            touch2Sequence.AddAction(touch2.CreatePointerMove(zoomInteractor, -50, 50, TimeSpan.Zero));
            touch2Sequence.AddAction(touch2.CreatePointerDown(PointerButton.TouchContact));
            touch2Sequence.AddAction(touch2.CreatePointerMove(zoomInteractor, -55, 55, TimeSpan.Zero));
            touch2Sequence.AddAction(touch2.CreatePointerMove(zoomInteractor, -60, 60, TimeSpan.Zero));
            touch2Sequence.AddAction(touch2.CreatePointerMove(zoomInteractor, -65, 65, TimeSpan.Zero));
            touch2Sequence.AddAction(touch2.CreatePointerMove(zoomInteractor, -70, 70, TimeSpan.Zero));
            touch2Sequence.AddAction(touch2.CreatePointerMove(zoomInteractor, -75, 75, TimeSpan.Zero));
            touch2Sequence.AddAction(touch2.CreatePointerMove(zoomInteractor, -80, 80, TimeSpan.Zero));
            touch2Sequence.AddAction(touch2.CreatePointerUp(PointerButton.TouchContact));

            // Perform the 2 fingers zoom in (expand) multi-touch sequences defined above
            session.PerformActions(new List<ActionSequence> { touch1Sequence, touch2Sequence });

            // Ensure that the zoom level now is greater than 100%
            Assert.IsTrue(int.Parse(zoomScaleTextBox.Text) > 100);
        }

        [TestMethod]
        public void ZoomingOutMultiTouch()
        {
            // Drag a touch contact diagonally in SW direction approaching the other contact point
            PointerInputDevice touch1 = new PointerInputDevice(PointerKind.Touch);
            ActionSequence touch1Sequence = new ActionSequence(touch1, 0);
            touch1Sequence.AddAction(touch1.CreatePointerMove(zoomInteractor, 50, -50, TimeSpan.Zero));
            touch1Sequence.AddAction(touch1.CreatePointerDown(PointerButton.TouchContact));
            touch1Sequence.AddAction(touch1.CreatePointerMove(zoomInteractor, 45, -45, TimeSpan.Zero));
            touch1Sequence.AddAction(touch1.CreatePointerMove(zoomInteractor, 40, -40, TimeSpan.Zero));
            touch1Sequence.AddAction(touch1.CreatePointerMove(zoomInteractor, 35, -35, TimeSpan.Zero));
            touch1Sequence.AddAction(touch1.CreatePointerMove(zoomInteractor, 30, -30, TimeSpan.Zero));
            touch1Sequence.AddAction(touch1.CreatePointerMove(zoomInteractor, 25, -25, TimeSpan.Zero));
            touch1Sequence.AddAction(touch1.CreatePointerMove(zoomInteractor, 20, -20, TimeSpan.Zero));
            touch1Sequence.AddAction(touch1.CreatePointerUp(PointerButton.TouchContact));

            // Drag a touch contact diagonally in NE direction approaching the other contact point
            PointerInputDevice touch2 = new PointerInputDevice(PointerKind.Touch);
            ActionSequence touch2Sequence = new ActionSequence(touch2, 0);
            touch2Sequence.AddAction(touch2.CreatePointerMove(zoomInteractor, -50, 50, TimeSpan.Zero));
            touch2Sequence.AddAction(touch2.CreatePointerDown(PointerButton.TouchContact));
            touch2Sequence.AddAction(touch2.CreatePointerMove(zoomInteractor, -45, 45, TimeSpan.Zero));
            touch2Sequence.AddAction(touch2.CreatePointerMove(zoomInteractor, -40, 40, TimeSpan.Zero));
            touch2Sequence.AddAction(touch2.CreatePointerMove(zoomInteractor, -35, 35, TimeSpan.Zero));
            touch2Sequence.AddAction(touch2.CreatePointerMove(zoomInteractor, -30, 30, TimeSpan.Zero));
            touch2Sequence.AddAction(touch2.CreatePointerMove(zoomInteractor, -25, 25, TimeSpan.Zero));
            touch2Sequence.AddAction(touch2.CreatePointerMove(zoomInteractor, -20, 20, TimeSpan.Zero));
            touch2Sequence.AddAction(touch2.CreatePointerUp(PointerButton.TouchContact));

            // Perform the 2 fingers zoom out (pinch) multi-touch sequences defined above
            session.PerformActions(new List<ActionSequence> { touch1Sequence, touch2Sequence });

            // Ensure that the zoom level now is less than 100%
            Assert.IsTrue(int.Parse(zoomScaleTextBox.Text) < 100);
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            // Create session to launch or bring up Paint 3D application
            Setup(context);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TearDown();
        }

        [TestInitialize]
        public void SetupZoomLevel()
        {
            zoomInteractor = session.FindElementByAccessibilityId("ZoomInteractor");
            zoomScaleTextBox = session.FindElementByAccessibilityId("ZoomScaleTextBox");

            // Ensure that the zoom level starts at 100%
            Assert.IsTrue(int.Parse(zoomScaleTextBox.Text) == 100);
        }
    }
}

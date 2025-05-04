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
using System.Drawing;
using System.Threading;
using System.Collections.Generic;
using OpenQA.Selenium;
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
    public class ScenarioDraw : Paint3DSession
    {
        private WindowsElement inkCanvas;
        private WindowsElement undoButton;
        private WindowsElement brushesPane;
        private const string eraserWidth = "8";

        // A           B
        //  ┌────┬────┐   Draw a Windows logo with corresponding ABCD points
        //  │    │E   │   around the outer square and point E as the middle
        //  ├────┼────┤   of the crosshair.
        //  │    │    │   - X is relative to the horizontal element center point
        //  └────┴────┘   - Y is relative to the vertical element center point
        // D           C
        private static Point A = new Point(-298, -214);
        private static Point B = new Point( 298, -298);
        private static Point C = new Point( 298,  298);
        private static Point D = new Point(-298,  214);
        private static Point E = new Point( -38,    0);

        [TestMethod]
        public void DrawWithPen()
        {
            PointerInputDevice penDevice = new PointerInputDevice(PointerKind.Pen);

            // Draw rectangle ABCD (consisting of AB, BC, CD, and DA lines)
            ActionSequence sequence = new ActionSequence(penDevice, 0);
            sequence.AddAction(penDevice.CreatePointerMove(inkCanvas, A.X, A.Y, TimeSpan.Zero));
            sequence.AddAction(penDevice.CreatePointerDown(PointerButton.PenContact));
            sequence.AddAction(penDevice.CreatePointerMove(inkCanvas, B.X, B.Y, TimeSpan.Zero));
            sequence.AddAction(penDevice.CreatePointerMove(inkCanvas, C.X, C.Y, TimeSpan.Zero));
            sequence.AddAction(penDevice.CreatePointerMove(inkCanvas, D.X, D.Y, TimeSpan.Zero));
            sequence.AddAction(penDevice.CreatePointerMove(inkCanvas, A.X, A.Y, TimeSpan.Zero));
            sequence.AddAction(penDevice.CreatePointerUp(PointerButton.PenContact));
            session.PerformActions(new List<ActionSequence> { sequence });

            // Fill the rectangle ABCD at the middle of the crosshair position (Point E)
            brushesPane.FindElementByAccessibilityId("FillBucket").Click();

            ActionSequence fillSequence = new ActionSequence(penDevice, 0);
            fillSequence.AddAction(penDevice.CreatePointerMove(inkCanvas, E.X, E.Y, TimeSpan.Zero));
            fillSequence.AddAction(penDevice.CreatePointerDown(PointerButton.PenContact));
            fillSequence.AddAction(penDevice.CreatePointerUp(PointerButton.PenContact));
            session.PerformActions(new List<ActionSequence> { fillSequence });

            // Erase by pressing PenEraser button along Point E X-Axis and Y-Axis to make the crosshair
            ActionSequence eraseSequence = new ActionSequence(penDevice, 0);
            eraseSequence.AddAction(penDevice.CreatePointerMove(inkCanvas, A.X - 5, E.Y, TimeSpan.Zero));
            eraseSequence.AddAction(penDevice.CreatePointerDown(PointerButton.PenEraser));
            eraseSequence.AddAction(penDevice.CreatePointerMove(inkCanvas, B.X + 5, E.Y, TimeSpan.FromSeconds(.5)));
            eraseSequence.AddAction(penDevice.CreatePointerUp(PointerButton.PenEraser));
            eraseSequence.AddAction(penDevice.CreatePointerMove(inkCanvas, E.X, C.Y, TimeSpan.Zero));
            eraseSequence.AddAction(penDevice.CreatePointerDown(PointerButton.PenEraser));
            eraseSequence.AddAction(penDevice.CreatePointerMove(inkCanvas, E.X, B.Y, TimeSpan.FromSeconds(.5)));
            eraseSequence.AddAction(penDevice.CreatePointerUp(PointerButton.PenEraser));
            session.PerformActions(new List<ActionSequence> { eraseSequence });

            // Verify that the drawing operations took place
            Assert.IsTrue(undoButton.Displayed);
            Assert.IsTrue(undoButton.Enabled);
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
        public void SetupBrushesPane()
        {
            // Select the Brushes toolbox to have the Brushes Pane sidebar displayed
            session.FindElementByAccessibilityId("Toolbox").FindElementByAccessibilityId("TopBar_ArtTools").Click();
            brushesPane = session.FindElementByAccessibilityId("SidebarWrapper");

            // Set eraser thickness to eraser width in pixel
            brushesPane.FindElementByAccessibilityId("Eraser3d").Click();
            if (brushesPane.FindElementByAccessibilityId("BrushSize").Text != eraserWidth)
            {
                brushesPane.FindElementByAccessibilityId("BrushSize").SendKeys(Keys.Control + "a" + Keys.Control);
                brushesPane.FindElementByAccessibilityId("BrushSize").SendKeys(eraserWidth + Keys.Enter);
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }

            // Ensure that the Pixel Pen is selected
            brushesPane.FindElementByAccessibilityId("PixelPencil3d").Click();

            // Locate the drawing surface
            inkCanvas = session.FindElementByAccessibilityId("InteractorFocusWrapper");

            // Locate the Undo button
            undoButton = session.FindElementByAccessibilityId("UndoIcon");
            Assert.IsTrue(undoButton.Displayed);
            Assert.IsFalse(undoButton.Enabled);
        }
    }
}

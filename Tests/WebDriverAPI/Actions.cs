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
using OpenQA.Selenium.Appium.Interactions;
using OpenQA.Selenium.Interactions;

// Define an alias to OpenQA.Selenium.Appium.Interactions.PointerInputDevice to hide
// inherited OpenQA.Selenium.Interactions.PointerInputDevice that causes ambiguity.
// In the future, all functions of OpenQA.Selenium.Appium.Interactions should be moved
// up to OpenQA.Selenium.Interactions and this alias can simply be removed.
using PointerInputDevice = OpenQA.Selenium.Appium.Interactions.PointerInputDevice;

namespace WebDriverAPI
{
    [TestClass]
    public class Actions : AlarmClockBase
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
        public void ActionsError_BadPointerButton_PointerDown()
        {
            try
            {
                // Perform pen down action using a bad button value
                PointerInputDevice penDevice = new PointerInputDevice(PointerKind.Pen);
                ActionSequence sequence = new ActionSequence(penDevice, 0);
                sequence.AddAction(penDevice.CreatePointerDown((PointerButton)(-1)));
                session.PerformActions(new List<ActionSequence> { sequence });
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.ActionsArgumentButton, exception.Message);
            }
        }

        [TestMethod]
        public void ActionsError_BadPointerButton_PointerUp()
        {
            try
            {
                // Perform pen up action using a bad button value
                PointerInputDevice penDevice = new PointerInputDevice(PointerKind.Pen);
                ActionSequence sequence = new ActionSequence(penDevice, 0);
                sequence.AddAction(penDevice.CreatePointerUp((PointerButton)(-1)));
                session.PerformActions(new List<ActionSequence> { sequence });
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.ActionsArgumentButton, exception.Message);
            }
        }

        [TestMethod]
        public void ActionsError_BadPointerMoveDuration()
        {
            try
            {
                // Perform pen move action using a bad duration value
                PointerInputDevice penDevice = new PointerInputDevice(PointerKind.Pen);
                ActionSequence sequence = new ActionSequence(penDevice, 0);
                sequence.AddAction(penDevice.CreatePointerMove(CoordinateOrigin.Viewport, 0, 0, TimeSpan.FromMilliseconds(-1)));
                session.PerformActions(new List<ActionSequence> { sequence });
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.ActionsArgumentDuration, exception.Message);
            }
        }

        [TestMethod]
        public void ActionsError_BadPointerOrigin()
        {
            try
            {
                // Perform pen move action using a bad origin value
                PointerInputDevice penDevice = new PointerInputDevice(PointerKind.Pen);
                ActionSequence sequence = new ActionSequence(penDevice, 0);
                sequence.AddAction(penDevice.CreatePointerMove((CoordinateOrigin)(-1), 0, 0, TimeSpan.Zero));
                session.PerformActions(new List<ActionSequence> { sequence });
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.ActionsArgumentOrigin, exception.Message);
            }
        }

        [TestMethod]
        public void ActionsError_BadPointerPen_Pressure()
        {
            try
            {
                // Perform pen move action using a bad pointer pressure parameter value
                PointerInputDevice penDevice = new PointerInputDevice(PointerKind.Pen);
                ActionSequence sequence = new ActionSequence(penDevice, 0);
                sequence.AddAction(penDevice.CreatePointerDown(PointerButton.PenContact, new PenInfo { Pressure = 2f }));
                session.PerformActions(new List<ActionSequence> { sequence });
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.ActionsArgumentParameterPressure, exception.Message);
            }
        }

        [TestMethod]
        public void ActionsError_BadPointerPen_TiltX()
        {
            try
            {
                // Perform pen move action using a bad pointer tilt x parameter value
                PointerInputDevice penDevice = new PointerInputDevice(PointerKind.Pen);
                ActionSequence sequence = new ActionSequence(penDevice, 0);
                sequence.AddAction(penDevice.CreatePointerDown(PointerButton.PenContact, new PenInfo { TiltX = 100 }));
                session.PerformActions(new List<ActionSequence> { sequence });
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.ActionsArgumentParameterTiltX, exception.Message);
            }
        }

        [TestMethod]
        public void ActionsError_BadPointerPen_TiltY()
        {
            try
            {
                // Perform pen move action using a bad pointer tilt y parameter value
                PointerInputDevice penDevice = new PointerInputDevice(PointerKind.Pen);
                ActionSequence sequence = new ActionSequence(penDevice, 0);
                sequence.AddAction(penDevice.CreatePointerDown(PointerButton.PenContact, new PenInfo { TiltY = 100 }));
                session.PerformActions(new List<ActionSequence> { sequence });
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.ActionsArgumentParameterTiltY, exception.Message);
            }
        }

        [TestMethod]
        public void ActionsError_BadPointerPen_Twist()
        {
            try
            {
                // Perform pen move action using a bad pointer twist parameter value
                PointerInputDevice penDevice = new PointerInputDevice(PointerKind.Pen);
                ActionSequence sequence = new ActionSequence(penDevice, 0);
                sequence.AddAction(penDevice.CreatePointerDown(PointerButton.PenContact, new PenInfo { Twist = -1 }));
                session.PerformActions(new List<ActionSequence> { sequence });
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.ActionsArgumentParameterTwist, exception.Message);
            }
        }

        [TestMethod]
        public void ActionsError_BadPointerTouch_Height()
        {
            try
            {
                // Perform pen move action using a bad pointer height parameter value
                PointerInputDevice touchDevice = new PointerInputDevice(PointerKind.Touch);
                ActionSequence sequence = new ActionSequence(touchDevice, 0);
                sequence.AddAction(touchDevice.CreatePointerDown(PointerButton.TouchContact, new TouchInfo { Height = -1f }));
                session.PerformActions(new List<ActionSequence> { sequence });
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.ActionsArgumentParameterHeight, exception.Message);
            }
        }

        [TestMethod]
        public void ActionsError_BadPointerTouch_Height_MissingWidth()
        {
            try
            {
                // Perform pen move action using a pointer height parameter value without providing the width
                PointerInputDevice touchDevice = new PointerInputDevice(PointerKind.Touch);
                ActionSequence sequence = new ActionSequence(touchDevice, 0);
                sequence.AddAction(touchDevice.CreatePointerDown(PointerButton.TouchContact, new TouchInfo { Height = 1f }));
                session.PerformActions(new List<ActionSequence> { sequence });
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.ActionsArgumentParameterMissingWidthOrHeight, exception.Message);
            }
        }

        [TestMethod]
        public void ActionsError_BadPointerTouch_Pressure()
        {
            try
            {
                // Perform pen move action using a bad pointer pressure parameter value
                PointerInputDevice touchDevice = new PointerInputDevice(PointerKind.Touch);
                ActionSequence sequence = new ActionSequence(touchDevice, 0);
                sequence.AddAction(touchDevice.CreatePointerDown(PointerButton.TouchContact, new TouchInfo { Pressure = 2f }));
                session.PerformActions(new List<ActionSequence> { sequence });
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.ActionsArgumentParameterPressure, exception.Message);
            }
        }

        [TestMethod]
        public void ActionsError_BadPointerTouch_Twist()
        {
            try
            {
                // Perform pen move action using a bad pointer twist parameter value
                PointerInputDevice touchDevice = new PointerInputDevice(PointerKind.Touch);
                ActionSequence sequence = new ActionSequence(touchDevice, 0);
                sequence.AddAction(touchDevice.CreatePointerDown(PointerButton.TouchContact, new TouchInfo { Twist = -1 }));
                session.PerformActions(new List<ActionSequence> { sequence });
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.ActionsArgumentParameterTwist, exception.Message);
            }
        }

        [TestMethod]
        public void ActionsError_BadPointerTouch_Width()
        {
            try
            {
                // Perform pen move action using a bad pointer width parameter value
                PointerInputDevice touchDevice = new PointerInputDevice(PointerKind.Touch);
                ActionSequence sequence = new ActionSequence(touchDevice, 0);
                sequence.AddAction(touchDevice.CreatePointerDown(PointerButton.TouchContact, new TouchInfo { Width = -1f }));
                session.PerformActions(new List<ActionSequence> { sequence });
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.ActionsArgumentParameterWidth, exception.Message);
            }
        }

        [TestMethod]
        public void ActionsError_BadPointerTouch_Width_MissingHeight()
        {
            try
            {
                // Perform pen move action using a pointer width parameter value without providing the height
                PointerInputDevice touchDevice = new PointerInputDevice(PointerKind.Touch);
                ActionSequence sequence = new ActionSequence(touchDevice, 0);
                sequence.AddAction(touchDevice.CreatePointerDown(PointerButton.TouchContact, new TouchInfo { Width = 1f }));
                session.PerformActions(new List<ActionSequence> { sequence });
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.ActionsArgumentParameterMissingWidthOrHeight, exception.Message);
            }
        }

        [TestMethod]
        public void ActionsError_MultiplePen()
        {
            try
            {
                // Perform pen move action on stale element
                PointerInputDevice penDevice1 = new PointerInputDevice(PointerKind.Pen);
                PointerInputDevice penDevice2 = new PointerInputDevice(PointerKind.Pen);
                ActionSequence sequence1 = new ActionSequence(penDevice1, 0);
                ActionSequence sequence2 = new ActionSequence(penDevice2, 0);
                sequence1.AddAction(penDevice1.CreatePointerDown(PointerButton.PenContact));
                sequence2.AddAction(penDevice2.CreatePointerDown(PointerButton.PenContact));
                session.PerformActions(new List<ActionSequence> { sequence1, sequence2 });
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.ActionsUnimplementedMultiPen, exception.Message);
            }
        }

        [TestMethod]
        public void ActionsError_NoSuchElement()
        {
            try
            {
                // Perform pen move action on stale element
                PointerInputDevice penDevice = new PointerInputDevice(PointerKind.Pen);
                ActionSequence sequence = new ActionSequence(penDevice, 0);
                sequence.AddAction(penDevice.CreatePointerMove(Utility.GetOrphanedElement(), 0, 0, TimeSpan.Zero));
                session.PerformActions(new List<ActionSequence> { sequence });
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.IsTrue(exception.Message.EndsWith(ErrorStrings.ActionsNoSuchElement));
            }
        }

        [TestMethod]
        public void ActionsError_NoSuchWindow()
        {
            try
            {
                // Perform pen move action on session that is no longer open
                PointerInputDevice penDevice = new PointerInputDevice(PointerKind.Pen);
                ActionSequence sequence = new ActionSequence(penDevice, 0);
                sequence.AddAction(penDevice.CreatePointerMove(Utility.GetOrphanedElement(), 0, 0, TimeSpan.Zero));
                Utility.GetOrphanedSession().PerformActions(new List<ActionSequence> { sequence });
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }

        [TestMethod]
        public void ActionsError_NullElement()
        {
            try
            {
                // Perform pen move action on null element
                PointerInputDevice penDevice = new PointerInputDevice(PointerKind.Pen);
                ActionSequence sequence = new ActionSequence(penDevice, 0);
                sequence.AddAction(penDevice.CreatePointerMove(null, 0, 0, TimeSpan.Zero));
                session.PerformActions(new List<ActionSequence> { sequence });
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.IsTrue(exception.Message.EndsWith(ErrorStrings.ActionsNullElement));
            }
        }

        [TestMethod]
        public void ActionsError_StaleElement()
        {
            try
            {
                // Perform pen move action on stale element
                PointerInputDevice penDevice = new PointerInputDevice(PointerKind.Pen);
                ActionSequence sequence = new ActionSequence(penDevice, 0);
                sequence.AddAction(penDevice.CreatePointerMove(GetStaleElement(), 0, 0, TimeSpan.Zero));
                session.PerformActions(new List<ActionSequence> { sequence });
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.IsTrue(exception.Message.EndsWith(ErrorStrings.ActionsStaleElementReference));
            }
        }

        [TestMethod]
        public void ActionsError_UnimplementedPointerType()
        {
            try
            {
                // Perform move action using unimplemented pointer type such as mouse
                PointerInputDevice penDevice = new PointerInputDevice(PointerKind.Mouse);
                ActionSequence sequence = new ActionSequence(penDevice, 0);
                sequence.AddAction(penDevice.CreatePointerMove(alarmTabElement, 0, 0, TimeSpan.Zero));
                session.PerformActions(new List<ActionSequence> { sequence });
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.IsTrue(exception.Message.StartsWith(ErrorStrings.ActionsUnimplementedPointerType));
            }
        }

        [TestMethod]
        public void ActionsError_UnsupportedPointerType()
        {
            try
            {
                // Perform move action using unsupported pointer type
                PointerInputDevice penDevice = new PointerInputDevice((PointerKind)(-1));
                ActionSequence sequence = new ActionSequence(penDevice, 0);
                sequence.AddAction(penDevice.CreatePointerMove(alarmTabElement, 0, 0, TimeSpan.Zero));
                session.PerformActions(new List<ActionSequence> { sequence });
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException) { }
        }
    }
}

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
using System;

namespace WebDriverAPI
{
    [TestClass]
    public class ElementLocationInView : CalculatorBase
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
        public void GetElementLocationInView()
        {
            WindowsElement num5Button = session.FindElementByAccessibilityId("num5Button");
            WindowsElement num7Button = session.FindElementByAccessibilityId("num7Button");
            WindowsElement num8Button = session.FindElementByAccessibilityId("num8Button");
            Assert.IsNotNull(num5Button);
            Assert.IsNotNull(num7Button);
            Assert.IsNotNull(num8Button);

            // Num 8 is in the same column with Num 5 and is in the same row with Num 7
            Assert.AreEqual(num8Button.LocationOnScreenOnceScrolledIntoView.X, num5Button.LocationOnScreenOnceScrolledIntoView.X);
            Assert.AreEqual(num8Button.LocationOnScreenOnceScrolledIntoView.Y, num7Button.LocationOnScreenOnceScrolledIntoView.Y);

            // Num 8 is on the right of Num 7 and on top of Num 5 (Y increases from top to bottom)
            Assert.IsTrue(num8Button.LocationOnScreenOnceScrolledIntoView.X > num7Button.LocationOnScreenOnceScrolledIntoView.X);
            Assert.IsTrue(num8Button.LocationOnScreenOnceScrolledIntoView.Y < num5Button.LocationOnScreenOnceScrolledIntoView.Y);
        }

        [TestMethod]
        public void GetElementLocationInViewError_NoSuchWindow()
        {
            try
            {
                var locationInView = Utility.GetOrphanedElement().LocationOnScreenOnceScrolledIntoView;
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }

        [TestMethod]
        public void GetElementLocationInViewError_StaleElement()
        {
            try
            {
                var locationInView = GetStaleElement().LocationOnScreenOnceScrolledIntoView;
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.StaleElementReference, exception.Message);
            }
        }
    }
}

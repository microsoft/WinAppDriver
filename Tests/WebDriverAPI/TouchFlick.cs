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
using OpenQA.Selenium;
using System;
using System.Threading;

namespace WebDriverAPI
{
    [TestClass]
    public class TouchFlick : EdgeBase
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
        public void TouchFlick_Arbitrary()
        {
            // Navigate to Edge about:flags page
            session.FindElementByAccessibilityId("addressEditBox").SendKeys(Keys.Alt + 'd' + Keys.Alt + CommonTestSettings.EdgeAboutFlagsURL + Keys.Enter);
            Thread.Sleep(TimeSpan.FromSeconds(1));

            // Use the reset all button on Edge about:flags page as a reference element
            var resetAllFlagsButton = session.FindElementByAccessibilityId("ResetAllFlags");
            Assert.IsNotNull(resetAllFlagsButton);
            Assert.IsTrue(resetAllFlagsButton.Displayed);

            // Perform flick up touch action to scroll the page down hiding the Homepage link element from the view
            // Good value typically goes around 160 - 200 pixels with diminishing delta on the bigger values
            touchScreen.Flick(0, 180);
            Thread.Sleep(TimeSpan.FromSeconds(3));
            Assert.IsFalse(resetAllFlagsButton.Displayed);

            // Perform flick down touch action to scroll the page up restoring the button element into the view
            touchScreen.Flick(0, -360);
            Thread.Sleep(TimeSpan.FromSeconds(3));
            Assert.IsTrue(resetAllFlagsButton.Displayed);
        }
    }
}
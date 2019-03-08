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
    public class ElementName : CalculatorBase
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
        public void GetElementTagName()
        {
            WindowsElement header = session.FindCalculatorTitleByAccessibilityId();
            Assert.AreEqual("ControlType.Text", header.TagName);

            WindowsElement navButton;
            try
            {
                // Current version of Calculator application
                navButton = session.FindElementByAccessibilityId("TogglePaneButton");
            }
            catch
            {
                // Previous version of Calculator application
                navButton = session.FindElementByAccessibilityId("NavButton");
            }
            Assert.AreEqual("ControlType.Button", navButton.TagName);
        }

        [TestMethod]
        public void GetElementTagNameError_NoSuchWindow()
        {
            try
            {
                var tagName = Utility.GetOrphanedElement().TagName;
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }

        [TestMethod]
        public void GetElementTagNameError_StaleElement()
        {
            try
            {
                var tagName = GetStaleElement().TagName;
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.StaleElementReference, exception.Message);
            }
        }
    }
}

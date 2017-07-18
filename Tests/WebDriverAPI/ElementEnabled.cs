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
    public class ElementEnabled : CalculatorBase
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
        public void GetElementEnabledState()
        {
            WindowsElement storeMemoryButton = session.FindElementByAccessibilityId("memButton");
            Assert.IsNotNull(storeMemoryButton);
            WindowsElement clearMemoryButton = session.FindElementByAccessibilityId("ClearMemoryButton");
            Assert.IsNotNull(clearMemoryButton);
            Assert.IsTrue(storeMemoryButton.Enabled);

            // Clear memory to disable clearMemoryButton (button could initially be already disabled)
            clearMemoryButton.Click();
            Assert.IsFalse(clearMemoryButton.Enabled);

            // Store memory to enable clearMemoryButton
            storeMemoryButton.Click();
            Assert.IsTrue(clearMemoryButton.Enabled);

            // Clear memory again to re-disable clearMemoryButton
            clearMemoryButton.Click();
            Assert.IsFalse(clearMemoryButton.Enabled);
        }

        [TestMethod]
        public void GetElementEnabledStateError_NoSuchWindow()
        {
            try
            {
                var enabled = Utility.GetOrphanedElement().Enabled;
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }

        [TestMethod]
        public void GetElementEnabledStateError_StaleElement()
        {
            try
            {
                var enabled = GetStaleElement().Enabled;
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.StaleElementReference, exception.Message);
            }
        }
    }
}

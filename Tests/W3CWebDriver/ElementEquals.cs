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

namespace W3CWebDriver
{
    [TestClass]
    public class ElementEquals : CalculatorBase
    {
        private static WindowsElement referenceElement = null;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);
            referenceElement = session.FindElementByAccessibilityId("plusButton");
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            referenceElement = null;
            TearDown();
        }

        [TestMethod]
        public void CompareNonEqualElements()
        {
            WindowsElement header = session.FindElementByAccessibilityId("Header");
            Assert.IsNotNull(header);
            Assert.IsFalse(header.Equals(referenceElement));
            Assert.IsFalse(referenceElement.Equals(header));
            Assert.AreNotEqual(header, referenceElement);
            Assert.AreNotEqual(referenceElement, header);
        }

        [TestMethod]
        public void ErrorCompareElementsNoSuchElement()
        {
            try
            {
                // The orphaned element Id is not valid as it does not originate from the current session
                var comparison = referenceElement.Equals(Utility.GetOrphanedElement());
                Assert.Fail("Exception should have been thrown");
            }
            catch (System.InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchElement, exception.Message);
            }
        }

        [TestMethod]
        public void ErrorCompareElementsNoSuchWindow()
        {
            try
            {
                // The orphaned element is no longer valid as the window it originated from is closed
                var comparison = Utility.GetOrphanedElement().Equals(referenceElement);
                Assert.Fail("Exception should have been thrown");
            }
            catch (System.InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }

        [TestMethod]
        public void ErrorCompareElementsStaleElement()
        {
            try
            {
                Assert.AreNotEqual(GetStaleElement(), referenceElement);
                Assert.Fail("Exception should have been thrown");
            }
            catch (System.InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.StaleElementReference, exception.Message);
            }
        }

        [TestMethod]
        public void ErrorCompareElementsStaleElementInverted()
        {
            WindowsElement staleElement = GetStaleElement();

            try
            {
                Assert.AreNotEqual(referenceElement, staleElement);
                Assert.Fail("Exception should have been thrown");
            }
            catch (System.InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.StaleElementReference, exception.Message);
            }
        }
    }
}

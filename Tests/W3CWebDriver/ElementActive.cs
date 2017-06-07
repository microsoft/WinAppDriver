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
    public class ElementActive : CalculatorBase
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
        public void ErrorGetActiveElementNoSuchWindow()
        {
            WindowsDriver<WindowsElement> newSession = CreateNewCalculatorSession();
            newSession.Close();

            try
            {
                WindowsElement activeElement = newSession.SwitchTo().ActiveElement() as WindowsElement;
                Assert.Fail("Exception should have been thrown");
            }
            catch (System.InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }

            newSession.Quit();
        }

        [TestMethod]
        public void GetActiveElement()
        {
            WindowsElement activeElement = session.SwitchTo().ActiveElement() as WindowsElement;
            Assert.IsNotNull(activeElement);
        }

        [TestMethod]
        public void GetActiveElementFocusableElement()
        {
            WindowsElement num8Button = session.FindElementByAccessibilityId("num8Button");
            num8Button.Click();
            WindowsElement activeElement = session.SwitchTo().ActiveElement() as WindowsElement;
            Assert.IsNotNull(activeElement);
            Assert.AreEqual(num8Button, activeElement);

            WindowsElement plusButton = session.FindElementByAccessibilityId("plusButton");
            plusButton.Click();
            WindowsElement activeElementAfter = session.SwitchTo().ActiveElement() as WindowsElement;
            Assert.IsNotNull(activeElementAfter);
            Assert.AreEqual(plusButton, activeElementAfter);
            Assert.AreNotEqual(activeElement, activeElementAfter);
        }

        [TestMethod]
        public void GetActiveElementNonFocusableElement()
        {
            WindowsElement header = session.FindElementByAccessibilityId("Header");
            header.Click();
            WindowsElement activeElement = session.SwitchTo().ActiveElement() as WindowsElement;
            Assert.IsNotNull(activeElement);
            Assert.AreNotEqual(header, activeElement);

            WindowsElement clearMemoryButton = session.FindElementByAccessibilityId("ClearMemoryButton");
            clearMemoryButton.Click();
            WindowsElement activeElementAfter = session.SwitchTo().ActiveElement() as WindowsElement;
            Assert.IsNotNull(activeElementAfter);
            Assert.AreNotEqual(clearMemoryButton, activeElementAfter);
            Assert.AreEqual(activeElement, activeElementAfter);
        }
    }
}

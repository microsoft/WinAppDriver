//******************************************************************************
//
// Copyright (c) 2019 Microsoft Corporation. All rights reserved.
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

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AbsoluteXPath
{
    [TestClass]
    public class SimpleAttributeExpression
    {
        DesktopSession desktopSession = new DesktopSession();

        [TestMethod]
        public void InvalidAttributeValue()
        {
            try
            {
                // [@Name=\"St^*~art\"]
                var xpath = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Pane[@ClassName=\"Shell_TrayWnd\"][@Name=\"Taskbar\"]/Button[@ClassName=\"Start\"][@Name=\"St^*~art\"]";
                desktopSession.DesktopSessionElement.FindElementByXPath(xpath);
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(WebDriverAPI.ErrorStrings.NoSuchElement, exception.Message);
            }
        }

        [TestMethod]
        public void NoAttributeValue()
        {
            // [@Name]
            var xpath = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Pane[@ClassName=\"Shell_TrayWnd\"][@Name=\"Taskbar\"]/Button[@ClassName=\"Start\"][@Name]";
            Assert.IsNotNull(desktopSession.DesktopSessionElement.FindElementByXPath(xpath));
        }

        [TestMethod]
        public void InvalidAttributeName()
        {
            try
            {
                // [@ClassNameInvalid=\"Shell_TrayWnd\"]
                var xpath = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Pane[@ClassNameInvalid=\"Shell_TrayWnd\"][@Name=\"Taskbar\"]/Button[@ClassName=\"Start\"][@Name=\"Start\"]";
                desktopSession.DesktopSessionElement.FindElementByXPath(xpath);
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(WebDriverAPI.ErrorStrings.NoSuchElement, exception.Message);
            }
        }

        [TestMethod]
        public void InvalidTagName()
        {
            try
            {
                // InvalidPane
                var xpath = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/InvalidPane[@ClassName=\"Shell_TrayWnd\"][@Name=\"Taskbar\"]/Button[@ClassName=\"Start\"][@Name=\"Start\"]";
                desktopSession.DesktopSessionElement.FindElementByXPath(xpath);
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(WebDriverAPI.ErrorStrings.NoSuchElement, exception.Message);
            }
        }

        [TestMethod]
        public void NameContainsStart()
        {
            var xpath = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Pane[@ClassName=\"Shell_TrayWnd\"][@Name=\"Taskbar\"]/Button[contains(@Name,\"Start\")]";
            Assert.IsNotNull(desktopSession.DesktopSessionElement.FindElementByXPath(xpath));
        }

        [TestMethod]
        public void ClassNameStartsWithStart()
        {
            var xpath = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Pane[starts-with(@ClassName,\"Shell_TrayWnd\")][@Name=\"Taskbar\"]/Button[contains(@Name,\"Start\")]";
            Assert.IsNotNull(desktopSession.DesktopSessionElement.FindElementByXPath(xpath));
        }

        [TestMethod]
        public void AttributesInSingleQuote()
        {
            var xpath = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Pane[starts-with(@ClassName,'Shell_TrayWnd')][@Name='Taskbar']/Button[contains(@Name,'Start')]";
            Assert.IsNotNull(desktopSession.DesktopSessionElement.FindElementByXPath(xpath));
        }

        [TestMethod]
        public void ButtonByPosition()
        {
            string xpath = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Pane[@ClassName=\"Shell_TrayWnd\"][@Name=\"Taskbar\"]/Button[position()=1]";
            Assert.IsNotNull(desktopSession.DesktopSessionElement.FindElementByXPath(xpath));
        }
    }
}

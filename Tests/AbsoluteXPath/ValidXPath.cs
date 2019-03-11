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
    public class ValidXPath
    {
        static DesktopSession desktopSession = new DesktopSession();

        [TestMethod]
        public void StarAsTagName()
        {
            string xpath = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Pane[@Name=\"Taskbar\"][@ClassName=\"Shell_TrayWnd\"]/*[@Name=\"Start\"]";
            Assert.IsNotNull(desktopSession.DesktopSessionElement.FindElementByXPath(xpath));
        }

        [TestMethod]
        public void TwoAncestors()
        {
            string xpath = "/*/*/*[@Name=\"Start\"]";
            Assert.IsNotNull(desktopSession.DesktopSessionElement.FindElementByXPath(xpath));
        }

        [TestMethod]
        public void SelectAllElements()
        {
            string xpath = "//*[@Name=\"Start\"]";
            Assert.IsNotNull(desktopSession.DesktopSessionElement.FindElementByXPath(xpath));
        }

        [TestMethod]
        public void DoubleSlash()
        {
            var xpath = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]//Button[@Name=\"Start\"]";
            Assert.IsNotNull(desktopSession.DesktopSessionElement.FindElementByXPath(xpath));
        }

        [TestMethod]
        public void IdAttributeName()
        {
            try
            {
                // Id attribute is not expected
                var xpath = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Pane[@Name=\"Taskbar\"][@ClassName=\"Shell_TrayWnd\"]/Button[@Name=\"Start\"][@Id=\"Start\"]";
                Assert.IsNull(desktopSession.DesktopSessionElement.FindElementByXPath(xpath));
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(WebDriverAPI.ErrorStrings.NoSuchElement, exception.Message);
            }
        }

        [TestMethod]
        public void andInAttributes()
        {
            var xpath = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Pane[@ClassName=\"Shell_TrayWnd\"][@Name=\"Taskbar\"]/Button[@ClassName=\"Start\" and @Name=\"Start\"]";
            Assert.IsNotNull(desktopSession.DesktopSessionElement.FindElementByXPath(xpath));
        }

        [TestMethod]
        public void orInAttributes()
        {
            string xpath = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Pane[@ClassName=\"Shell_TrayWnd\"][@Name=\"Taskbar\"]/Button[@ClassName=\"Start\" or @Name=\"Start\"]";
            Assert.IsNotNull(desktopSession.DesktopSessionElement.FindElementByXPath(xpath));
        }

        [TestMethod]
        public void ButtonByIndex()
        {
            string xpath = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Pane[@ClassName=\"Shell_TrayWnd\"][@Name=\"Taskbar\"]/Button[1]";
            Assert.IsNotNull(desktopSession.DesktopSessionElement.FindElementByXPath(xpath));
        }
    }
}

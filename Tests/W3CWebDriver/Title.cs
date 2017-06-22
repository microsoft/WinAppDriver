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

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;

namespace W3CWebDriver
{
    [TestClass]
    public class Title
    {
        private WindowsDriver<WindowsElement> session = null;

        [TestMethod]
        public void GetTitleClassicApp()
        {
            session = Utility.CreateNewSession(CommonTestSettings.NotepadAppId);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);
            Assert.AreEqual("Untitled - Notepad", session.Title);
            session.Quit();
            session = null;
        }

        [TestMethod]
        public void GetTitleDesktop()
        {
            session = Utility.CreateNewSession(CommonTestSettings.DesktopAppId);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);
            Assert.IsTrue(session.Title.StartsWith("Desktop"));
            session.Quit();
            session = null;
        }

        [TestMethod]
        public void GetTitleModernApp()
        {
            session = Utility.CreateNewSession(CommonTestSettings.CalculatorAppId);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);
            Assert.AreEqual("Calculator", session.Title);
            session.Quit();
            session = null;
        }

        [TestMethod]
        public void ErrorGetTitleNoSuchWindow()
        {
            try
            {
                var title = Utility.GetOrphanedSession().Title;
                Assert.Fail("Exception should have been thrown");
            }
            catch (System.InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }
    }
}

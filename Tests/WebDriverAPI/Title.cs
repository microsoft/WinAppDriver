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
using OpenQA.Selenium.Appium.Windows;
using System;

namespace WebDriverAPI
{
    [TestClass]
    public class Title
    {
        private WindowsDriver<WindowsElement> session = null;

        [TestCleanup]
        public void TestCleanup()
        {
            if (session != null)
            {
                session.Quit();
                session = null;
            }
        }

        [TestMethod]
        public void GetTitle_ClassicApp()
        {
            session = Utility.CreateNewSession(CommonTestSettings.NotepadAppId);
            Assert.IsNotNull(session);
            Assert.AreEqual("Untitled - Notepad", session.Title);
        }

        [TestMethod]
        public void GetTitle_Desktop()
        {
            session = Utility.CreateNewSession(CommonTestSettings.DesktopAppId);
            Assert.IsNotNull(session);
            Assert.IsTrue(session.Title.StartsWith("Desktop"));
        }

        [TestMethod]
        public void GetTitle_ModernApp()
        {
            session = Utility.CreateNewSession(CommonTestSettings.CalculatorAppId);
            Assert.IsNotNull(session);
            Assert.IsTrue(session.Title.Contains("Calculator"));
        }

        [TestMethod]
        public void GetTitleError_NoSuchWindow()
        {
            try
            {
                var title = Utility.GetOrphanedSession().Title;
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }
    }
}

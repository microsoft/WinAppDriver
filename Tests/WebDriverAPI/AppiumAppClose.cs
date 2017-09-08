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
using System.Threading;

namespace WebDriverAPI
{
    [TestClass]
    public class AppiumAppClose
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

        private void CloseApplication(string applicationId)
        {
            session = Utility.CreateNewSession(applicationId);
            Assert.IsNotNull(session.SessionId);
            Assert.AreNotEqual(string.Empty, session.Title);
            Assert.AreNotEqual(string.Empty, session.CurrentWindowHandle);
            Assert.IsTrue(session.WindowHandles.Count > 0);

            session.CloseApp();

            Assert.IsNotNull(session.SessionId);
            Assert.AreEqual(0, session.WindowHandles.Count);

            try
            {
                session.CloseApp(); // Attempt to close already closed app
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }

        [TestMethod]
        public void Close_ClassicApp()
        {
            CloseApplication(CommonTestSettings.NotepadAppId);
        }

        [TestMethod]
        public void Close_ModernApp()
        {
            CloseApplication(CommonTestSettings.CalculatorAppId);
        }

        [TestMethod]
        public void Close_SystemApp()
        {
            CloseApplication(CommonTestSettings.ExplorerAppId);
        }
    }
}

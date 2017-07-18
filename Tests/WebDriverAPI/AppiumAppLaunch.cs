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

namespace WebDriverAPI
{
    [TestClass]
    public class AppiumAppLaunch
    {
        [TestMethod]
        public void Launch_ClassicApp()
        {
            WindowsDriver<WindowsElement> session = Utility.CreateNewSession(CommonTestSettings.NotepadAppId);
            Assert.IsNotNull(session.SessionId);
            var originalTitle = session.Title;
            var originalLaunchedWindowHandle = session.CurrentWindowHandle;

            // Close notepad application session created above without quitting
            session.Close();
            Assert.IsNotNull(session.SessionId);

            // Re-launch notepad application in the same session using the same capabilities configuration
            session.LaunchApp();
            Assert.AreEqual(originalTitle, session.Title);
            Assert.AreNotEqual(originalLaunchedWindowHandle, session.CurrentWindowHandle);
            session.Quit();
        }

        [TestMethod]
        public void Launch_ModernApp()
        {
            WindowsDriver<WindowsElement> session = Utility.CreateNewSession(CommonTestSettings.CalculatorAppId);
            Assert.IsNotNull(session.SessionId);
            var originalTitle = session.Title;
            var originalWindowHandlesCount = session.WindowHandles.Count;
            var originalLaunchedWindowHandle = session.CurrentWindowHandle;

            // Re-launch calculator application in the same session using the same capabilities configuration
            // This will create a new calculator window and point the current active session to it
            session.LaunchApp();
            Assert.AreEqual(originalTitle, session.Title);
            Assert.AreEqual(originalWindowHandlesCount + 1, session.WindowHandles.Count);
            Assert.AreNotEqual(originalLaunchedWindowHandle, session.CurrentWindowHandle);

            session.Close();
            session.SwitchTo().Window(originalLaunchedWindowHandle);
            session.Close();
            session.Quit();
        }

        [TestMethod]
        public void Launch_SystemApp()
        {
            WindowsDriver<WindowsElement> session = Utility.CreateNewSession(CommonTestSettings.ExplorerAppId);
            Assert.IsNotNull(session.SessionId);
            var originalTitle = session.Title;
            var originalWindowHandlesCount = session.WindowHandles.Count;
            var originalLaunchedWindowHandle = session.CurrentWindowHandle;

            // Re-launch Windows Explorer in the same session using the same capabilities configuration
            // This will create a new explorer window and point the current active session to it
            session.LaunchApp();
            Assert.AreEqual(originalTitle, session.Title);
            Assert.AreEqual(originalWindowHandlesCount + 1, session.WindowHandles.Count);
            Assert.AreNotEqual(originalLaunchedWindowHandle, session.CurrentWindowHandle);

            session.Close();
            session.SwitchTo().Window(originalLaunchedWindowHandle);
            session.Close();
            session.Quit();
        }
    }
}

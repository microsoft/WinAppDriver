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
    public class AppiumAppLaunch
    {
        [TestMethod]
        public void LaunchClassicApp()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.NotepadAppId);
            WindowsDriver<WindowsElement> session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);

            var originalTitle = session.Title;
            var originalLaunchedWindowHandle = session.CurrentWindowHandle;
            session.Close();
            Assert.IsNotNull(session.SessionId);

            session.LaunchApp();
            Assert.AreEqual(originalTitle, session.Title);
            Assert.AreNotEqual(originalLaunchedWindowHandle, session.CurrentWindowHandle);
            session.Quit();
        }

        [TestMethod]
        public void LaunchModernApp()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.CalculatorAppId);
            WindowsDriver<WindowsElement> session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);

            var originalTitle = session.Title;
            var originalWindowHandlesCount = session.WindowHandles.Count;
            var originalLaunchedWindowHandle = session.CurrentWindowHandle;
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
        public void LaunchSystemApp()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.ExplorerAppId);
            WindowsDriver<WindowsElement> session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);

            var originalTitle = session.Title;
            var originalWindowHandlesCount = session.WindowHandles.Count;
            var originalLaunchedWindowHandle = session.CurrentWindowHandle;
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

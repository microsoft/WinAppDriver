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
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Remote;

namespace W3CWebDriver
{
    [TestClass]
    public class AppiumAppClose
    {
        private void CloseApplication(string applicationId)
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", applicationId);
            IOSDriver<IOSElement> session = new IOSDriver<IOSElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);

            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);
            Assert.AreNotEqual(String.Empty, session.Title);
            Assert.AreNotEqual(String.Empty, session.CurrentWindowHandle);
            var originalWindowHandlesCount = session.WindowHandles.Count;

            session.CloseApp();

            System.Threading.Thread.Sleep(3000); // Sleep for 3 second
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);
            Assert.AreEqual(originalWindowHandlesCount - 1, session.WindowHandles.Count);

            try
            {
                session.CloseApp(); // Attempt to close already closed app
                Assert.Fail("Exception should have been thrown");
            }
            catch (System.InvalidOperationException exception)
            {
                Assert.AreEqual("Currently selected window has been closed", exception.Message);
            }

            session.Quit();
        }

        [TestMethod]
        public void CloseClassicApp()
        {
            CloseApplication(CommonTestSettings.NotepadAppId);
        }

        [TestMethod]
        public void CloseModernApp()
        {
            CloseApplication(CommonTestSettings.CalculatorAppId);
        }

        [TestMethod]
        public void CloseSystemApp()
        {
            CloseApplication(CommonTestSettings.ExplorerAppId);
        }
    }
}

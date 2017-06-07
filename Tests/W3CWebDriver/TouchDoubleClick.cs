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
using OpenQA.Selenium.Remote;
using System;

namespace W3CWebDriver
{
    [TestClass]
    public class TouchDoubleClick : EdgeBase
    {
        private static WindowsDriver<WindowsElement> calculatorSession;
        private static RemoteTouchScreen calculatorTouchScreen;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            // Cleanup, close, and delete calculator session in case the cleanup steps were interrupted by exception
            calculatorTouchScreen = null;

            if (calculatorSession != null)
            {
                calculatorSession.Quit();
                calculatorSession = null;
            }

            TearDown();
        }

        [TestMethod]
        public void DoubleTap()
        {
            // Launch calculator for this specific test case
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.CalculatorAppId);
            calculatorSession = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(calculatorSession);
            Assert.IsNotNull(calculatorSession.SessionId);

            // Save application window original size and position
            var originalSize = calculatorSession.Manage().Window.Size;
            var originalPosition = calculatorSession.Manage().Window.Position;

            // Get maximized window size
            calculatorSession.Manage().Window.Maximize();
            var maximizedSize = calculatorSession.Manage().Window.Size;
            Assert.IsNotNull(maximizedSize);

            // Shrink the window size by 100 pixels each side from maximized size to ensure size changes when maximized
            int offset = 100;
            calculatorSession.Manage().Window.Size = new System.Drawing.Size(maximizedSize.Width - offset, maximizedSize.Height - offset);
            calculatorSession.Manage().Window.Position = new System.Drawing.Point(offset, offset);
            System.Threading.Thread.Sleep(1000); // Sleep for 1 second
            var currentWindowSize = calculatorSession.Manage().Window.Size;
            Assert.IsNotNull(currentWindowSize);
            Assert.AreNotEqual(maximizedSize.Width, currentWindowSize.Width);
            Assert.AreNotEqual(maximizedSize.Height, currentWindowSize.Height);

            // Perform double tap touch on the title bar to maximize calculator window
            calculatorTouchScreen = new RemoteTouchScreen(calculatorSession);
            Assert.IsNotNull(calculatorTouchScreen);
            calculatorTouchScreen.DoubleTap(calculatorSession.FindElementByAccessibilityId("AppNameTitle").Coordinates);
            System.Threading.Thread.Sleep(1000); // Sleep for 1 second
            currentWindowSize = calculatorSession.Manage().Window.Size;
            Assert.IsNotNull(currentWindowSize);
            Assert.AreEqual(maximizedSize.Width, currentWindowSize.Width);
            Assert.AreEqual(maximizedSize.Height, currentWindowSize.Height);

            // Restore application window original size and position
            calculatorSession.Manage().Window.Size = originalSize;
            calculatorSession.Manage().Window.Position = originalPosition;
            System.Threading.Thread.Sleep(1000); // Sleep for 1 second

            // Close the calculator application and delete the session after cleaning up
            calculatorTouchScreen = null;
            calculatorSession.Quit();
            calculatorSession = null;
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorTouchClosedWindow()
        {
            // Open a new window, retrieve an element, and close the window to get an orphaned element
            var orphanedElement = GetOrphanedElement(session);
            Assert.IsNotNull(orphanedElement);

            // Perform double tap touch on the orphaned element
            touchScreen.DoubleTap(orphanedElement.Coordinates);
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Net.WebException))]
        public void ErrorTouchInvalidElement()
        {
            ErrorTouchInvalidElement("doubleclick");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorTouchStaleElement()
        {
            // Navigate to a webpage, save a reference to an element, and navigate away to get a stale element
            var staleElement = GetStaleElement(session);
            Assert.IsNotNull(staleElement);

            // Perform double tap touch on stale element
            touchScreen.DoubleTap(staleElement.Coordinates);
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Net.WebException))]
        public void ErrorTouchInvalidArguments()
        {
            ErrorTouchInvalidArguments("doubleclick");
        }
    }
}

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
using OpenQA.Selenium.Remote;
using System;
using System.Threading;

namespace NotepadCalculatorTest
{
    public class CalculatorSession : IDisposable
    {
        // Note: append /wd/hub to the URL if you're directing the test at Appium
        private const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        private const string CalculatorAppId = "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App";

        public WindowsDriver<WindowsElement> Session { get; private set; }
        public WindowsElement CalculatorHeader { get; private set; }
        public WindowsElement CalculatorResult { get; private set; }

        public CalculatorSession()
        {
            // Create a new session to bring up an instance of the Calculator application
            // Note: Multiple calculator windows (instances) share the same process Id
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CalculatorAppId);
            appCapabilities.SetCapability("deviceName", "WindowsPC");
            this.Session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(Session);

            // Set implicit timeout to 1.5 seconds to make element search to retry every 500 ms for at most three times
            Session.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1.5);

            // Identify calculator mode by locating the calculator header
            try
            {
                CalculatorHeader = Session.FindElementByAccessibilityId("Header");
            }
            catch
            {
                CalculatorHeader = Session.FindElementByAccessibilityId("ContentPresenter");
            }

            // Ensure that calculator is in standard mode
            if (!CalculatorHeader.Text.Equals("Standard", StringComparison.OrdinalIgnoreCase))
            {
                Session.FindElementByAccessibilityId("TogglePaneButton").Click();
                Thread.Sleep(TimeSpan.FromSeconds(1));
                var splitViewPane = Session.FindElementByClassName("SplitViewPane");
                splitViewPane.FindElementByName("Standard Calculator").Click();
                Thread.Sleep(TimeSpan.FromSeconds(1));
                Assert.IsTrue(CalculatorHeader.Text.Equals("Standard", StringComparison.OrdinalIgnoreCase));
            }

            // Locate the CalculatorResult element
            CalculatorResult = Session.FindElementByAccessibilityId("CalculatorResults");
        }

        public void Dispose()
        {
            // Close the application and delete the session
            if (Session != null)
            {
                Session.Quit();
                Session = null;
                CalculatorHeader = null;
                CalculatorResult = null;
            }
        }
    }
}

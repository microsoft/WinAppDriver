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
using System.Threading;

namespace CortanaTest
{
    [TestClass]
    public class BingSearch
    {
        private const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";

        private static WindowsDriver<WindowsElement> cortanaSession;
        private static WindowsDriver<WindowsElement> desktopSession;
        private static WindowsElement searchBox;

        [TestMethod]
        public void LocalSearch()
        {
            // Type "add" in Cortana search box
            searchBox.SendKeys("add");
            Thread.Sleep(TimeSpan.FromSeconds(1));
            var bingPane = cortanaSession.FindElementByName("Bing");
            Assert.IsNotNull(bingPane);

            // Verify that a shortcut to "System settings Add or remove programs" is shown as a search result
            var bingResult = bingPane.FindElementByXPath("//DataItem[starts-with(@Name, \"Add or remove\")]");
            Assert.IsNotNull(bingResult);
            Assert.IsTrue(bingResult.Text.EndsWith("System settings"));
        }

        [TestMethod]
        public void WebSearchCalculator()
        {
            // Type a math sentence in Cortana search box
            searchBox.SendKeys("What is eight times eleven");
            Thread.Sleep(TimeSpan.FromSeconds(1));
            var bingPane = cortanaSession.FindElementByName("Bing");
            Assert.IsNotNull(bingPane);

            // Verify that a Calculator result is shown as one of the search results
            var bingResult = bingPane.FindElementByXPath("//*[contains(@Name, \"= 88\")]");
            Assert.IsNotNull(bingResult);
            Assert.IsTrue(bingResult.Text.Contains("8"));
            Assert.IsTrue(bingResult.Text.Contains("11"));
        }

        [TestMethod]
        public void WebSearchStockName()
        {
            // Type a Microsoft stock name in Cortana search box
            searchBox.SendKeys("msft");
            Thread.Sleep(TimeSpan.FromSeconds(1));
            var bingPane = cortanaSession.FindElementByName("Bing");
            Assert.IsNotNull(bingPane);

            // Verify that a Microsoft stock in NASDAQ is shown as one of the search results
            var bingResult = bingPane.FindElementByXPath("//*[contains(@Name, \"Microsoft\")]");
            Assert.IsNotNull(bingResult);
            Assert.IsTrue(bingResult.Text.Contains("NASDAQ"));
            Assert.IsTrue(bingResult.Text.Contains("MSFT"));
        }

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            // Create a session for Desktop
            DesiredCapabilities desktopCapabilities = new DesiredCapabilities();
            desktopCapabilities.SetCapability("app", "Root");
            desktopSession = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), desktopCapabilities);
            Assert.IsNotNull(desktopSession);

            // Launch Cortana Window using Windows Key + S keyboard shortcut to allow session creation to find it
            desktopSession.Keyboard.SendKeys(OpenQA.Selenium.Keys.Meta + "s" + OpenQA.Selenium.Keys.Meta);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            WindowsElement CortanaWindow = desktopSession.FindElementByName("Cortana");
            string CortanaTopLevelWindowHandle = CortanaTopLevelWindowHandle = (int.Parse(CortanaWindow.GetAttribute("NativeWindowHandle"))).ToString("x");

            // Create session for already running Cortana
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("appTopLevelWindow", CortanaTopLevelWindowHandle);
            cortanaSession = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(cortanaSession);

            // Set implicit timeout to 5 seconds to make element search to retry every 500 ms for at most ten times
            cortanaSession.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
        }

        [ClassCleanup]
        public static void TearDown()
        {
            searchBox = null;

            if (cortanaSession != null)
            {
                // Send escape key to close Cortana window
                cortanaSession.Keyboard.SendKeys(OpenQA.Selenium.Keys.Escape + OpenQA.Selenium.Keys.Escape);
                cortanaSession.Quit();
                cortanaSession = null;
            }

            if (desktopSession != null)
            {
                desktopSession.Quit();
                desktopSession = null;
            }
        }

        [TestInitialize]
        public void ClearSearchBox()
        {
            try
            {
                // Locate Cortana search box to ensure it is still displayed on the screen
                searchBox = cortanaSession.FindElementByAccessibilityId("SearchTextBox");
                searchBox.Clear();
            }
            catch
            {
                // Use Windows Key + S to relaunch Cortana window if it is not displayed
                desktopSession.Keyboard.SendKeys(OpenQA.Selenium.Keys.Meta + "s" + OpenQA.Selenium.Keys.Meta);
                Thread.Sleep(TimeSpan.FromSeconds(1));
                searchBox = cortanaSession.FindElementByAccessibilityId("SearchTextBox");
            }

            Assert.IsNotNull(searchBox);
            Thread.Sleep(TimeSpan.FromSeconds(0.5));
        }
    }
}

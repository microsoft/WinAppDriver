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

namespace CalculatorTest
{
    public class CalculatorTestBase
    {
        // Note: append /wd/hub to the URL if you're directing the test at Appium
        protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";

        protected static WindowsElement CalculatorResult;
        protected static WindowsDriver<WindowsElement> CalculatorSession;
        protected static string OriginalCalculatorMode;

        public static void BaseSetup(TestContext context)
        {
            if (CalculatorSession == null)
            { 
                // Launch the calculator app
                DesiredCapabilities appCapabilities = new DesiredCapabilities();
                appCapabilities.SetCapability("app", "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
                appCapabilities.SetCapability("deviceName", "WindowsPC");
                CalculatorSession = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
                Assert.IsNotNull(CalculatorSession);
                CalculatorSession.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(4));

                // Make sure we're in standard mode
                CalculatorSession.FindElementByAccessibilityId("NavButton").Click();
                OriginalCalculatorMode = CalculatorSession.FindElementByAccessibilityId("FlyoutNav").Text;
                CalculatorSession.FindElementByName("Standard Calculator").Click();

                // Use series of operation to locate the calculator result text element as a workaround
                CalculatorSession.FindElementByAccessibilityId("clearButton").Click();
                CalculatorSession.FindElementByAccessibilityId("num7Button").Click();
                CalculatorResult = CalculatorSession.FindElementByAccessibilityId("CalculatorResults");
            }

            Assert.IsNotNull(CalculatorResult);
        }

        public static void BaseTearDown()
        {
            if (CalculatorSession != null)
            { 
                // Restore original mode before closing down
                CalculatorSession.FindElementByAccessibilityId("NavButton").Click();
                CalculatorSession.FindElementByName("Standard Calculator").Click();

                CalculatorResult = null;
                CalculatorSession.Dispose();
                CalculatorSession = null;
            }
        }

        protected string _GetCalculatorResultText()
        {
            // trim extra text and whitespace off of the display value
            return CalculatorResult.Text.Replace("Display is", "").TrimEnd(' ').TrimStart(' ');
        }
    }
}

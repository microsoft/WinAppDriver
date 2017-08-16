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
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Configuration;

namespace CalculatorTest
{
    public class CalculatorTestBase
    {
        // Note: append /wd/hub to the URL if you're directing the test at Appium
        protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";

        protected static WindowsElement CalculatorResult;
        protected static WindowsDriver<WindowsElement> CalculatorSession;
        protected static string OriginalCalculatorMode;
        public TestContext TestContext { get; set; }

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

        /// <param name="filePath">path where file will be written</param>
        /// <param name="testMethodName">TestMethodName will be passed in</param>
        /// <param name="fileName">the file name to be saved with datetime: controlText_hh-mm-ss-tt.png</param>
        protected void _GetScreenshot(WindowsDriver<WindowsElement> session, string testMethodName, string controlText = "")
        {
            // Take a screenshot
            Screenshot ss = session.GetScreenshot();
            // Set the filePath to the name of the test method with datetime: testmethodname-yyyy-MM-dd_hh-mm
            string filePath = string.Format(ConfigurationManager.AppSettings["ScreenShotPath"].ToString()
                               + testMethodName+ "-{0:yyyy-MM-dd_hh-mm}", DateTime.Now);
            if (filePath != "" && controlText != "")
            {
                if (!System.IO.Directory.Exists(filePath))
                {
                    //Create the directory
                    System.IO.Directory.CreateDirectory(filePath);
                }
                // Set the filename to the name of the controlText with datetime: hh-mm-ss_controltext.png
                string newFileName = string.Format("{0:hh-mm-ss-ffff}", DateTime.Now)+"_"+controlText+".png";
                string file = string.Format("{0}\\{1}", filePath, newFileName);

                ss.SaveAsFile(file, ImageFormat.Png);
            }
        }
    }
}

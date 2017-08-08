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
using System.Linq;

namespace Input
{
    /// <summary>
    /// Standard durations in milliseconds for timing events
    /// </summary>
    public static class TouchDuration
    {
        public const int Immediate = 0;
        public const int Short = 100;
        public const int Long = 1000;
    }

    /// <summary>
    /// Standard distances in pixels for dragging, swiping, etc.
    /// </summary>
    public static class TouchDistance
    {
        public const int Short = 100;
        public const int Long = 250;
    }

    /// <summary>
    /// Standard speeds in pixels per second
    /// </summary>
    public static class TouchSpeed
    {
        public const int Slow = 100;
    }

    public class TestBase
    {
        // Note: append /wd/hub to the URL if you're directing the test at Appium
        protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";

        protected static WindowsElement AppResult;
        protected static WindowsDriver<WindowsElement> AppSession;
        protected static RemoteTouchScreen TouchScreen;

        public static void BaseSetup(TestContext context)
        {
            if (AppSession == null)
            {
                // Launch the test app
                DesiredCapabilities appCapabilities = new DesiredCapabilities();
                appCapabilities.SetCapability("app", "WinAppDriver.Input_xh1ske9axcpv8!App");
                appCapabilities.SetCapability("deviceName", "WindowsPC");
                AppSession = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
                Assert.IsNotNull(AppSession);
                AppSession.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(4);

                AppResult = AppSession.FindElementByAccessibilityId("ResultText");
                TouchScreen = new RemoteTouchScreen(AppSession);

                // Maximize the test window
                AppSession.Manage().Window.Maximize();
            }

            Assert.IsNotNull(AppResult);
            Assert.IsNotNull(TouchScreen);
        }

        public static void BaseTearDown()
        {
            // Cleanup the AppResult WindowsElement object if initialized
            AppResult = null;

            // Cleanup RemoteTouchScreen object if initialized
            TouchScreen = null;

            // Close the application and delete the session
            if (AppSession != null)
            {
                try
                {
                    AppSession.Close();
                    
                    // This should throw if the window is closed successfully
                    var currentHandle = AppSession.CurrentWindowHandle;
                }
                catch { }
                
                AppSession.Quit();
                AppSession = null;
            }
        }

        protected string _GetResultText()
        {
            return AppResult.Text;
        }
        protected string[] _GetResultStrings()
        {
            return AppResult.Text.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        }

        protected string _GetFirstResultString()
        {
            return _GetResultStrings().FirstOrDefault();
        }

        protected string _GetLastResultString()
        {
            return _GetResultStrings().LastOrDefault();
        }
    }
}

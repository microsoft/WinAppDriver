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

namespace UWPControls
{
    public class UWPControlsBase
    {
        private const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        private const string AppUIBasicAppId = "WinAppDriver.AppUIBasics_xh1ske9axcpv8!App";
        protected static WindowsDriver<WindowsElement> session = null;

        public static void Setup(TestContext context)
        {
            // Launch the App UI Basic app
            // Ensure AppUIBasics app has been installed in the device
            // Below are steps to install the AppUIBasics. These steps only need to be executed once
            // 1. Open ApplicationUnderTests\AppUIBasics\AppUIBasics.sln solution in Visual Studio
            // 2. Select the correct configuration for the device (e.g. x86) and Run the application
            // 3. The application will then be installed. You can safely close the AppUIBasics app & project
            if (session == null)
            {
                DesiredCapabilities appCapabilities = new DesiredCapabilities();
                appCapabilities.SetCapability("app", AppUIBasicAppId);
                session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
                Assert.IsNotNull(session);
                Assert.IsNotNull(session.SessionId);

                // Dismiss the disclaimer window that may pop up on the very first application launch
                try
                {
                    session.FindElementByName("Disclaimer").FindElementByName("Accept").Click();
                }
                catch { }

                session.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(5));
            }
        }

        public static void TearDown()
        {
            if (session != null)
            {
                session.Quit();
                session = null;
            }
        }

        // Helper function to use the SplitViewPane to navigate to a control test page
        protected static void NavigateTo(string ControlsGroup, string ControlName)
        {
            Assert.IsNotNull(session);
            session.FindElementByAccessibilityId("splitViewToggle").Click();
            System.Threading.Thread.Sleep(1000);

            var splitViewPane = session.FindElementByClassName("SplitViewPane");
            splitViewPane.FindElementByName(ControlsGroup).Click();
            splitViewPane.FindElementByName(ControlName).Click();
            System.Threading.Thread.Sleep(1000);
        }
    }
}
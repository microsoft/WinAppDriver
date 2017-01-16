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
        private const string AppUIBasicAppId = "a5bd3fa2-e27f-49e9-9041-47c97e903ecc_8wekyb3d8bbwe!App";
        protected static WindowsDriver<WindowsElement> session = null;

        protected virtual void LoadScenarioView()
        {
            Assert.IsTrue(true);
        }

        public static void Setup(TestContext context)
        {
            // Launch the App UI Basic app
            // Ensure AppUIBasics app has been installed in the device
            // Below are steps to install the AppUIBasics. These steps only need to be executed once
            // 1. Open ApplicationUnderTests\AppUIBasics\AppUIBasics.sln solution in Visual Studio
            // 2. Select the correct configuration for the device (e.g. x86) and Run the application
            // 3. The application will then be installed. You can safely close the AppUIBasics app & project
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", AppUIBasicAppId);
            session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);
        }

        public static void TearDown()
        {
            if (session != null)
            {
                session.Quit();
                session = null;
            }
        }

        [TestInitialize]
        public void TestInit()
        {
            Assert.IsNotNull(session);
            Assert.AreEqual("App UI Basics CS Sample", session.Title);
            LoadScenarioView();
        }
    }
}
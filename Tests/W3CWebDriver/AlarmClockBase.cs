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
    public class AlarmClockBase
    {
        protected static IOSDriver<IOSElement> session;
        protected IOSElement alarmTabElement;

        public static void Setup(TestContext context)
        {
            // Cleanup leftover objects from previous test if exists
            TearDown();

            // Launch Alarm Clock
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.AlarmClockAppId);
            session = new IOSDriver<IOSElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);
        }

        public static void TearDown()
        {
            // Close the application and delete the session
            if (session != null)
            {
                session.Quit();
                session = null;
            }
        }

        [TestInitialize]
        public void TestInit()
        { 
            // Attempt to go back to the main page in case Alarm & Clock app is started in EditAlarm view
            try
            {
                alarmTabElement = session.FindElementByAccessibilityId("AlarmPivotItem");
            }
            catch
            {
                session.Navigate().Back();
                alarmTabElement = session.FindElementByAccessibilityId("AlarmPivotItem");
            }

            Assert.IsNotNull(alarmTabElement);
            alarmTabElement.Click();
        }
    }
}
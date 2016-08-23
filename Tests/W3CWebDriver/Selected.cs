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
    public class Selected
    {
        protected static IOSDriver<IOSElement> session;
        protected static IOSElement alarmTabElement;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.AlarmClockAppId);
            session = new IOSDriver<IOSElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);

            alarmTabElement = session.FindElementByAccessibilityId("AlarmPivotItem");
            Assert.IsNotNull(alarmTabElement);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            if (session != null)
            {
                session.Quit();
                session = null;
            }
        }

        [TestMethod]
        public void FindSelectedElement()
        {
            IOSElement elementWorldClock = session.FindElementByAccessibilityId("WorldClockPivotItem");
            IOSElement elementAlarmClock = session.FindElementByAccessibilityId("AlarmPivotItem");

            elementWorldClock.Click();
            Assert.IsTrue(elementWorldClock.Selected);
            Assert.IsFalse(elementAlarmClock.Selected);
            elementAlarmClock.Click();
            Assert.IsFalse(elementWorldClock.Selected);
            Assert.IsTrue(elementAlarmClock.Selected);
        }
    }
}

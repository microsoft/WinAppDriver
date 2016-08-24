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
using System.Net;
using System.Drawing;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.iOS;

namespace W3CWebDriver
{
    [TestClass]
    public class Screenshot : AlarmClockBase
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            ClassInit(context);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            ClassClean();
        }

        [TestMethod]
        public void GetAlarmPivotItemScreenshot()
        {
            IOSElement element = session.FindElementByAccessibilityId("AlarmPivotItem");
            var screenshot = element.GetScreenshot();
            using (MemoryStream msScreenshot = new MemoryStream(screenshot.AsByteArray))
            {
                Image screenshotImage = Image.FromStream(msScreenshot);
                Assert.IsTrue(screenshotImage.Height > 0);
                Assert.IsTrue(screenshotImage.Width > 0);
            }
        }

        [TestMethod]
        public void GetWorldClockScreenshot()
        {
            IOSElement element = session.FindElementByAccessibilityId("WorldClockPivotItem");
            var screenshot = element.GetScreenshot();
            using (MemoryStream msScreenshot = new MemoryStream(screenshot.AsByteArray))
            {
                Image screenshotImage = Image.FromStream(msScreenshot);
                Assert.IsTrue(screenshotImage.Height > 0);
                Assert.IsTrue(screenshotImage.Width > 0);
            }
        }

        [TestMethod]
        public void GetAddAlarmScreenshot()
        {
            IOSElement element = session.FindElementByAccessibilityId("AddAlarmButton");
            var screenshot = element.GetScreenshot();
            using (MemoryStream msScreenshot = new MemoryStream(screenshot.AsByteArray))
            {
                Image screenshotImage = Image.FromStream(msScreenshot);
                Assert.IsTrue(screenshotImage.Height > 0);
                Assert.IsTrue(screenshotImage.Width > 0);
            }
        }

        [TestMethod]
        public void GetAlarmListScreenshot()
        {
            IOSElement element = session.FindElementByAccessibilityId("AlarmListView");
            var screenshot = element.GetScreenshot();
            using (MemoryStream msScreenshot = new MemoryStream(screenshot.AsByteArray))
            {
                Image screenshotImage = Image.FromStream(msScreenshot);
                Assert.IsTrue(screenshotImage.Height > 0);
                Assert.IsTrue(screenshotImage.Width > 0);
            }
        }

        [TestMethod]
        public void GetScreenshot()
        {
            var screenshot = session.GetScreenshot();
            using (MemoryStream msScreenshot = new MemoryStream(screenshot.AsByteArray))
            {
                Image screenshotImage = Image.FromStream(msScreenshot);
                Assert.IsTrue(screenshotImage.Height > 0);
                Assert.IsTrue(screenshotImage.Width > 0);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ErrorGetClosedWindowScreenshot()
        {
            session.Close();
            session.GetScreenshot();
            Assert.Fail("Exception should have been thrown because there is no such window");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Net.WebException))]
        public void ErrorGetInvalidElementScreenshot()
        {
            var request = WebRequest.Create(CommonTestSettings.WindowsApplicationDriverUrl + "/session/" + session.SessionId + "/element/invalidElement/screenshot");
            request.Method = "GET";
            WebResponse response = request.GetResponse();
            Assert.Fail("Exception should have been thrown because element is invalid");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ErrorGetStaleElementScreenshot()
        {
            session.FindElementByAccessibilityId("StopwatchPivotItem").Click();
            IOSElement element = session.FindElementByAccessibilityId("StopwatchPlayPauseButton");
            var screenshot1 = element.GetScreenshot();
            using (MemoryStream msScreenshot1 = new MemoryStream(screenshot1.AsByteArray))
            {
                Image screenshotImage1 = Image.FromStream(msScreenshot1);
                Assert.IsTrue(screenshotImage1.Height > 0);
                Assert.IsTrue(screenshotImage1.Width > 0);
            }

            session.FindElementByAccessibilityId("WorldClockPivotItem").Click();
            System.Threading.Thread.Sleep(1000); // Sleep for 1 second to allow screen to switch fully

            var screenshot = element.GetScreenshot();
            Assert.Fail("Exception should have been thrown because element is stale");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ErrorGetClosedSessionElementScreenshot()
        {
            session.FindElementByAccessibilityId("StopwatchPivotItem").Click();
            IOSElement element = session.FindElementByAccessibilityId("StopwatchPlayPauseButton");
            session.Close();
            try
            {
                var screenshot1 = element.GetScreenshot();

            }
            catch(Exception ex)
            {
                var ty = ex.GetType();
            }
            Assert.Fail("Exception should have been thrown because there is no such window");
        }
    }
}

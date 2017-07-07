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
using System.Drawing;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;

namespace W3CWebDriver
{
    [TestClass]
    public class Screenshot : AlarmClockBase
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TearDown();
        }

        [TestMethod]
        public void GetElementScreenshot()
        {
            WindowsElement element = session.FindElementByAccessibilityId("AddAlarmButton");
            var screenshot = element.GetScreenshot();
            using (MemoryStream msScreenshot = new MemoryStream(screenshot.AsByteArray))
            {
                Image screenshotImage = Image.FromStream(msScreenshot);
                Assert.IsTrue(screenshotImage.Height > 0);
                Assert.IsTrue(screenshotImage.Width > 0);
            }
        }

        [TestMethod]
        public void GetElementScreenshotError_NoSuchWindow()
        {
            try
            {
                var screenshot = Utility.GetOrphanedElement().GetScreenshot();
                Assert.Fail("Exception should have been thrown because there is no such window");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }

        [TestMethod]
        public void GetElementScreenshotError_StaleElement()
        {
            try
            {
                var screenshot = GetStaleElement().GetScreenshot();
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.StaleElementReference, exception.Message);
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
        public void GetScreenshotError_NoSuchWindow()
        {
            try
            {
                Utility.GetOrphanedSession().GetScreenshot();
                Assert.Fail("Exception should have been thrown because there is no such window");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }
    }
}

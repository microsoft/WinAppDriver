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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

namespace WebDriverAPI
{
    [TestClass]
    public class TouchLongClick : AlarmClockBase
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
        public void TouchLongTap()
        {
            // Create a new test alarm
            string alarmName = "LongTapTest";
            DeletePreviouslyCreatedAlarmEntry(alarmName);
            AddAlarmEntry(alarmName);
            Thread.Sleep(TimeSpan.FromSeconds(3));

            var alarmEntries = session.FindElementsByXPath($"//ListItem[starts-with(@Name, \"{alarmName}\")]");
            Assert.IsNotNull(alarmEntries);
            Assert.AreEqual(1, alarmEntries.Count);

            // Open a the context menu on the alarm entry using long tap (press and hold) action and click delete
            touchScreen.LongPress(alarmEntries[0].Coordinates);
            Thread.Sleep(TimeSpan.FromSeconds(3));
            session.FindElementByName("Delete").Click();
            Thread.Sleep(TimeSpan.FromSeconds(3));

            alarmEntries = session.FindElementsByXPath($"//ListItem[starts-with(@Name, \"{alarmName}\")]");
            Assert.IsNotNull(alarmEntries);
            Assert.AreEqual(0, alarmEntries.Count);
        }

        [TestMethod]
        public void TouchLongTapError_StaleElement()
        {
            try
            {
                // Perform long press touch on stale element
                touchScreen.LongPress(GetStaleElement().Coordinates);
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.StaleElementReference, exception.Message);
            }
        }

    }
}

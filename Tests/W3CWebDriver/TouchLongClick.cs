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

namespace W3CWebDriver
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
        public void LongTap()
        {
            // Create a new test alarm
            string alarmName = "LongTapTest";
            DeletePreviouslyCreatedAlarmEntry(alarmName);
            AddAlarmEntry(alarmName);
            System.Threading.Thread.Sleep(3000); // Sleep for 3 second

            var alarmEntries = session.FindElementsByXPath(string.Format("//ListItem[starts-with(@Name, \"{0}\")]", alarmName));
            Assert.IsNotNull(alarmEntries);
            Assert.AreEqual(1, alarmEntries.Count);

            // Open a the context menu on the alarm entry using long tap (press and hold) action and click delete
            touchScreen.LongPress(alarmEntries[0].Coordinates);
            System.Threading.Thread.Sleep(3000); // Sleep for 3 seconds
            session.FindElementByName("Delete").Click();
            System.Threading.Thread.Sleep(3000); // Sleep for 3 second

            alarmEntries = session.FindElementsByXPath(string.Format("//ListItem[starts-with(@Name, \"{0}\")]", alarmName));
            Assert.IsNotNull(alarmEntries);
            Assert.AreEqual(0, alarmEntries.Count);
        }
    }
}

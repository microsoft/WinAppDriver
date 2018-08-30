//******************************************************************************
//
// Copyright (c) 2018 Microsoft Corporation. All rights reserved.
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
using System.Threading;
using System;

namespace AlarmClockTest
{
    [TestClass]
    public class ScenarioTimer : AlarmClockSession
    {
        private const string NewTimerName = "Sample Test Timer";

        [TestMethod]
        public void AddTimer()
        {
            session.FindElementByAccessibilityId("AddTimerButton").Click();
            session.FindElementByAccessibilityId("HourLoopingSelector").FindElementByName("5").Click();
            session.FindElementByAccessibilityId("TimerNameTextBox").Clear();
            session.FindElementByAccessibilityId("TimerNameTextBox").SendKeys(NewTimerName);
            session.FindElementByAccessibilityId("TimerStartButton").Click();

            Assert.IsNotNull(session.FindElementByAccessibilityId("TimerListView"));
            var timers = session.FindElementByAccessibilityId("TimerListView").FindElementsByClassName("ListViewItem");
            Assert.IsTrue(timers.Count > 0);
            var timer = timers[timers.Count - 1];
            Assert.IsTrue(timer.FindElementByAccessibilityId("TimerResetButton").Enabled);
            Assert.AreEqual(NewTimerName, timer.FindElementByAccessibilityId("TimerNameText").Text);

            timer.SendKeys(OpenQA.Selenium.Keys.Delete + OpenQA.Selenium.Keys.Enter);
        }

        //* Pre-baked action
        //session.FindElementByAccessibilityId("AddTimerButton").Click();
        //session.FindElementByAccessibilityId("HourLoopingSelector").FindElementByName("5").Click();
        //session.FindElementByAccessibilityId("TimerNameTextBox").Clear();
        //session.FindElementByAccessibilityId("TimerNameTextBox").SendKeys(NewTimerName);
        //session.FindElementByAccessibilityId("TimerStartButton").Click();

        //Assert.IsNotNull(session.FindElementByAccessibilityId("TimerListView"));
        //var timers = session.FindElementByAccessibilityId("TimerListView").FindElementsByClassName("ListViewItem");
        //Assert.IsTrue(timers.Count > 0);
        //var timer = timers[timers.Count - 1];
        //Assert.IsTrue(timer.FindElementByAccessibilityId("TimerResetButton").Enabled);
        //Assert.AreEqual(NewTimerName, timer.FindElementByAccessibilityId("TimerNameText").Text);

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            // Try to delete any timer entry that may have been created
            while (true)
            {
                try
                {
                    var timerEntry = session.FindElementByXPath($"//ListItem[starts-with(@Name, \"{NewTimerName}\")]");
                    session.Mouse.ContextClick(timerEntry.Coordinates);
                    session.FindElementByName("Delete").Click();
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
                catch
                {
                    break;
                }
            }

            TearDown();
        }

        [TestInitialize]
        public override void TestInit()
        {
            // Invoke base class test initialization to ensure that the app is in the main page
            base.TestInit();

            // Navigate to Timer tab
            session.FindElementByAccessibilityId("TimerPivotItem").Click();
        }
    }
}

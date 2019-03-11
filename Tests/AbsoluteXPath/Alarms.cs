//******************************************************************************
//
// Copyright (c) 2019 Microsoft Corporation. All rights reserved.
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
using OpenQA.Selenium;

namespace AbsoluteXPath
{
    [TestClass]
    public class Alarms
    {
        [TestMethod]
        public void TestMethod1()
        {
            DesktopSession desktopSession = new DesktopSession();
            TimeSpan.FromSeconds(2);

            bool bSuccess = false;

            try
            {
                // LeftClick on Button "Type here to search" at (59,39)
                Console.WriteLine("LeftClick on Button \"Type here to search\" at (59,39)");
                string xpath_LeftClickButtonTypehereto_59_39 = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Pane[@ClassName=\"Shell_TrayWnd\"][@Name=\"Taskbar\"]/Pane[@ClassName=\"TrayDummySearchControl\"]/Button[@ClassName=\"Button\"][@Name=\"Type here to search\"]";
                var winElem_LeftClickButtonTypehereto_59_39 = desktopSession.FindElementByAbsoluteXPath(xpath_LeftClickButtonTypehereto_59_39);
                if (winElem_LeftClickButtonTypehereto_59_39 != null)
                {
                    winElem_LeftClickButtonTypehereto_59_39.Click();
                }
                else
                {
                    Console.WriteLine($"Failed to find element using xpath: {xpath_LeftClickButtonTypehereto_59_39}");
                    return;
                }


                // KeyboardInput VirtualKeys=""alarms"Keys.Space + Keys.SpaceKeys.Shift + "7" + Keys.ShiftKeys.Space + Keys.Space"clock"" CapsLock=False NumLock=False ScrollLock=False
                Console.WriteLine("KeyboardInput VirtualKeys=\"\"alarms\"Keys.Space + Keys.SpaceKeys.Shift + \"7\" + Keys.ShiftKeys.Space + Keys.Space\"clock\"\" CapsLock=False NumLock=False ScrollLock=False");
                System.Threading.Thread.Sleep(1000);
                winElem_LeftClickButtonTypehereto_59_39.SendKeys("alarms");
                winElem_LeftClickButtonTypehereto_59_39.SendKeys(Keys.Space + Keys.Space);
                winElem_LeftClickButtonTypehereto_59_39.SendKeys(Keys.Shift + "7" + Keys.Shift);
                winElem_LeftClickButtonTypehereto_59_39.SendKeys(Keys.Space + Keys.Space);
                winElem_LeftClickButtonTypehereto_59_39.SendKeys("clock");


                // LeftClick on ListItem "Alarms &amp; Clock, Trusted Microsoft Store app, Press right to swit" at (126,13)
                Console.WriteLine("LeftClick on ListItem \"Alarms &amp; Clock, Trusted Microsoft Store app, Press right to swit\" at (126,13)");
                string xpath_LeftClickListItemAlarmsCloc_126_13 = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Window[@ClassName=\"Windows.UI.Core.CoreWindow\"][@Name=\"Cortana\"]/Pane[@ClassName=\"WebView\"][@Name=\"Bing\"]/Pane[@Name=\"Bing\"]/List[@Name=\"Results\"][@AutomationId=\"suggestionsList\"]/ListItem[starts-with(@Name,\"Alarms &amp; Clock, Trusted Microsoft Store app, Press right to swit\")][starts-with(@AutomationId,\"id_\")]";
                var winElem_LeftClickListItemAlarmsCloc_126_13 = desktopSession.FindElementByAbsoluteXPath(xpath_LeftClickListItemAlarmsCloc_126_13);
                if (winElem_LeftClickListItemAlarmsCloc_126_13 != null)
                {
                    winElem_LeftClickListItemAlarmsCloc_126_13.Click();
                }
                else
                {
                    Console.WriteLine($"Failed to find element using xpath: {xpath_LeftClickListItemAlarmsCloc_126_13}");
                    return;
                }


                // LeftClick on ListItem "Alarm" at (56,17)
                Console.WriteLine("LeftClick on ListItem \"Alarm\" at (56,17)");
                string xpath_LeftClickListItemAlarm_56_17 = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Window[@ClassName=\"ApplicationFrameWindow\"][@Name=\"Alarms &amp; Clock\"]/Window[@ClassName=\"Windows.UI.Core.CoreWindow\"][@Name=\"Alarms &amp; Clock\"]/List[@AutomationId=\"TopNavMenuItemsHost\"]/ListItem[@Name=\"Alarm\"][@AutomationId=\"AlarmButton\"]";
                var winElem_LeftClickListItemAlarm_56_17 = desktopSession.FindElementByAbsoluteXPath(xpath_LeftClickListItemAlarm_56_17);
                if (winElem_LeftClickListItemAlarm_56_17 != null)
                {
                    winElem_LeftClickListItemAlarm_56_17.Click();
                }
                else
                {
                    Console.WriteLine($"Failed to find element using xpath: {xpath_LeftClickListItemAlarm_56_17}");
                    return;
                }


                // LeftClick on ListItem "Clock" at (54,10)
                Console.WriteLine("LeftClick on ListItem \"Clock\" at (54,10)");
                string xpath_LeftClickListItemClock_54_10 = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Window[@ClassName=\"ApplicationFrameWindow\"][@Name=\"Alarms &amp; Clock\"]/Window[@ClassName=\"Windows.UI.Core.CoreWindow\"][@Name=\"Alarms &amp; Clock\"]/List[@AutomationId=\"TopNavMenuItemsHost\"]/ListItem[@Name=\"Clock\"][@AutomationId=\"ClockButton\"]";
                var winElem_LeftClickListItemClock_54_10 = desktopSession.FindElementByAbsoluteXPath(xpath_LeftClickListItemClock_54_10);
                if (winElem_LeftClickListItemClock_54_10 != null)
                {
                    winElem_LeftClickListItemClock_54_10.Click();
                }
                else
                {
                    Console.WriteLine($"Failed to find element using xpath: {xpath_LeftClickListItemClock_54_10}");
                    return;
                }


                // LeftClick on ListItem "Timer" at (56,21)
                Console.WriteLine("LeftClick on ListItem \"Timer\" at (56,21)");
                string xpath_LeftClickListItemTimer_56_21 = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Window[@ClassName=\"ApplicationFrameWindow\"][@Name=\"Alarms &amp; Clock\"]/Window[@ClassName=\"Windows.UI.Core.CoreWindow\"][@Name=\"Alarms &amp; Clock\"]/List[@AutomationId=\"TopNavMenuItemsHost\"]/ListItem[@Name=\"Timer\"][@AutomationId=\"TimerButton\"]";
                var winElem_LeftClickListItemTimer_56_21 = desktopSession.FindElementByAbsoluteXPath(xpath_LeftClickListItemTimer_56_21);
                if (winElem_LeftClickListItemTimer_56_21 != null)
                {
                    winElem_LeftClickListItemTimer_56_21.Click();
                }
                else
                {
                    Console.WriteLine($"Failed to find element using xpath: {xpath_LeftClickListItemTimer_56_21}");
                    return;
                }


                // LeftClick on ListItem "Stopwatch" at (71,24)
                Console.WriteLine("LeftClick on ListItem \"Stopwatch\" at (71,24)");
                string xpath_LeftClickListItemStopwatch_71_24 = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Window[@ClassName=\"ApplicationFrameWindow\"][@Name=\"Alarms &amp; Clock\"]/Window[@ClassName=\"Windows.UI.Core.CoreWindow\"][@Name=\"Alarms &amp; Clock\"]/List[@AutomationId=\"TopNavMenuItemsHost\"]/ListItem[@Name=\"Stopwatch\"][@AutomationId=\"StopwatchButton\"]";
                var winElem_LeftClickListItemStopwatch_71_24 = desktopSession.FindElementByAbsoluteXPath(xpath_LeftClickListItemStopwatch_71_24);
                if (winElem_LeftClickListItemStopwatch_71_24 != null)
                {
                    winElem_LeftClickListItemStopwatch_71_24.Click();
                }
                else
                {
                    Console.WriteLine($"Failed to find element using xpath: {xpath_LeftClickListItemStopwatch_71_24}");
                    return;
                }


                // LeftClick on Button "Reset" at (16,23)
                Console.WriteLine("LeftClick on Button \"Reset\" at (16,23)");
                string xpath_LeftClickButtonReset_16_23 = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Window[@ClassName=\"ApplicationFrameWindow\"][@Name=\"Alarms &amp; Clock\"]/Window[@ClassName=\"Windows.UI.Core.CoreWindow\"][@Name=\"Alarms &amp; Clock\"]/Button[@Name=\"Reset\"][@AutomationId=\"StopWatchResetButton\"]";
                var winElem_LeftClickButtonReset_16_23 = desktopSession.FindElementByAbsoluteXPath(xpath_LeftClickButtonReset_16_23);
                if (winElem_LeftClickButtonReset_16_23 != null)
                {
                    winElem_LeftClickButtonReset_16_23.Click();
                }
                else
                {
                    Console.WriteLine($"Failed to find element using xpath: {xpath_LeftClickButtonReset_16_23}");
                    return;
                }


                // LeftClick on Button "Start" at (40,25)
                Console.WriteLine("LeftClick on Button \"Start\" at (40,25)");
                string xpath_LeftClickButtonStart_40_25 = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Window[@ClassName=\"ApplicationFrameWindow\"][@Name=\"Alarms &amp; Clock\"]/Window[@ClassName=\"Windows.UI.Core.CoreWindow\"][@Name=\"Alarms &amp; Clock\"]/Button[@Name=\"Start\"][@AutomationId=\"StopwatchPlayPauseButton\"]";
                var winElem_LeftClickButtonStart_40_25 = desktopSession.FindElementByAbsoluteXPath(xpath_LeftClickButtonStart_40_25);
                if (winElem_LeftClickButtonStart_40_25 != null)
                {
                    winElem_LeftClickButtonStart_40_25.Click();
                }
                else
                {
                    Console.WriteLine($"Failed to find element using xpath: {xpath_LeftClickButtonStart_40_25}");
                    return;
                }


                // LeftClick on Button "Pause" at (34,28)
                Console.WriteLine("LeftClick on Button \"Pause\" at (34,28)");
                string xpath_LeftClickButtonPause_34_28 = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Window[@ClassName=\"ApplicationFrameWindow\"][@Name=\"Alarms &amp; Clock\"]/Window[@ClassName=\"Windows.UI.Core.CoreWindow\"][@Name=\"Alarms &amp; Clock\"]/Button[@Name=\"Pause\"][@AutomationId=\"StopwatchPlayPauseButton\"]";
                var winElem_LeftClickButtonPause_34_28 = desktopSession.FindElementByAbsoluteXPath(xpath_LeftClickButtonPause_34_28);
                if (winElem_LeftClickButtonPause_34_28 != null)
                {
                    winElem_LeftClickButtonPause_34_28.Click();
                }
                else
                {
                    Console.WriteLine($"Failed to find element using xpath: {xpath_LeftClickButtonPause_34_28}");
                    return;
                }


                // LeftClick on Button "Reset" at (25,25)
                Console.WriteLine("LeftClick on Button \"Reset\" at (25,25)");
                string xpath_LeftClickButtonReset_25_25 = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Window[@ClassName=\"ApplicationFrameWindow\"][@Name=\"Alarms &amp; Clock\"]/Window[@ClassName=\"Windows.UI.Core.CoreWindow\"][@Name=\"Alarms &amp; Clock\"]/Button[@Name=\"Reset\"][@AutomationId=\"StopWatchResetButton\"]";
                var winElem_LeftClickButtonReset_25_25 = desktopSession.FindElementByAbsoluteXPath(xpath_LeftClickButtonReset_25_25);
                if (winElem_LeftClickButtonReset_25_25 != null)
                {
                    winElem_LeftClickButtonReset_25_25.Click();
                }
                else
                {
                    Console.WriteLine($"Failed to find element using xpath: {xpath_LeftClickButtonReset_25_25}");
                    return;
                }


                // LeftClick on Button "Close Alarms &amp; Clock" at (19,22)
                Console.WriteLine("LeftClick on Button \"Close Alarms &amp; Clock\" at (19,22)");
                string xpath_LeftClickButtonCloseAlarm_19_22 = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Window[@ClassName=\"ApplicationFrameWindow\"][@Name=\"Alarms &amp; Clock\"]/Window[@Name=\"Alarms &amp; Clock\"][@AutomationId=\"TitleBar\"]/Button[@Name=\"Close Alarms &amp; Clock\"][@AutomationId=\"Close\"]";
                var winElem_LeftClickButtonCloseAlarm_19_22 = desktopSession.FindElementByAbsoluteXPath(xpath_LeftClickButtonCloseAlarm_19_22);
                if (winElem_LeftClickButtonCloseAlarm_19_22 != null)
                {
                    winElem_LeftClickButtonCloseAlarm_19_22.Click();
                }
                else
                {
                    Console.WriteLine($"Failed to find element using xpath: {xpath_LeftClickButtonCloseAlarm_19_22}");
                    return;
                }

                bSuccess = true;
            }
            finally
            {
                Assert.AreEqual(bSuccess, true);
            }
        }
    }
}

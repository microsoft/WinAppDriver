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

namespace AbsoluteXPath
{
    [TestClass]
    public class Calculator
    {
        [TestMethod]
        public void TestMethod1()
        {
            DesktopSession desktopSession = new DesktopSession();
            TimeSpan.FromSeconds(2);

            bool bSuccess = false;

            try
            {
                // LeftClick on Button "Type here to search" at (95,20)
                Console.WriteLine("LeftClick on Button \"Type here to search\" at (95,20)");
                string xpath_LeftClickButtonTypehereto_95_20 = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Pane[@ClassName=\"Shell_TrayWnd\"][@Name=\"Taskbar\"]/Pane[@ClassName=\"TrayDummySearchControl\"]/Button[@ClassName=\"Button\"][@Name=\"Type here to search\"]";
                var winElem_LeftClickButtonTypehereto_95_20 = desktopSession.FindElementByAbsoluteXPath(xpath_LeftClickButtonTypehereto_95_20);
                if (winElem_LeftClickButtonTypehereto_95_20 != null)
                {
                    winElem_LeftClickButtonTypehereto_95_20.Click();
                }
                else
                {
                    Console.WriteLine($"Failed to find element using xpath: {xpath_LeftClickButtonTypehereto_95_20}");
                    return;
                }


                // KeyboardInput VirtualKeys=""calculator"" CapsLock=False NumLock=False ScrollLock=False
                Console.WriteLine("KeyboardInput VirtualKeys=\"\"calculator\"\" CapsLock=False NumLock=False ScrollLock=False");
                System.Threading.Thread.Sleep(1000);
                winElem_LeftClickButtonTypehereto_95_20.SendKeys("calculator");

                // LeftClick on ListItem "Calculator, Trusted Microsoft Store app, Press right to switch p" at (113,27)
                Console.WriteLine("LeftClick on ListItem \"Calculator, Trusted Microsoft Store app, Press right to switch p\" at (113,27)");
                string xpath_LeftClickListItemCalculator_113_27 = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Window[@ClassName=\"Windows.UI.Core.CoreWindow\"][@Name=\"Cortana\"]/Pane[@ClassName=\"WebView\"][@Name=\"Bing\"]/Pane[@Name=\"Bing\"]/List[@Name=\"Results\"][@AutomationId=\"suggestionsList\"]/ListItem[starts-with(@Name,\"Calculator, Trusted Microsoft Store app, Press right to switch p\")][starts-with(@AutomationId,\"id_\")]";
                var winElem_LeftClickListItemCalculator_113_27 = desktopSession.FindElementByAbsoluteXPath(xpath_LeftClickListItemCalculator_113_27);
                if (winElem_LeftClickListItemCalculator_113_27 != null)
                {
                    winElem_LeftClickListItemCalculator_113_27.Click();
                }
                else
                {
                    Console.WriteLine($"Failed to find element using xpath: {xpath_LeftClickListItemCalculator_113_27}");
                    return;
                }


                // LeftClick on Button "One" at (41,16)
                Console.WriteLine("LeftClick on Button \"One\" at (41,16)");
                string xpath_LeftClickButtonOne_41_16 = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Window[@ClassName=\"ApplicationFrameWindow\"][@Name=\"Calculator\"]/Window[@ClassName=\"Windows.UI.Core.CoreWindow\"][@Name=\"Calculator\"]/Group[@ClassName=\"LandmarkTarget\"]/Group[@Name=\"Number pad\"][@AutomationId=\"NumberPad\"]/Button[@Name=\"One\"][@AutomationId=\"num1Button\"]";
                var winElem_LeftClickButtonOne_41_16 = desktopSession.FindElementByAbsoluteXPath(xpath_LeftClickButtonOne_41_16);
                if (winElem_LeftClickButtonOne_41_16 != null)
                {
                    winElem_LeftClickButtonOne_41_16.Click();
                }
                else
                {
                    Console.WriteLine($"Failed to find element using xpath: {xpath_LeftClickButtonOne_41_16}");
                    return;
                }


                // LeftClick on Button "Plus" at (52,24)
                Console.WriteLine("LeftClick on Button \"Plus\" at (52,24)");
                string xpath_LeftClickButtonPlus_52_24 = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Window[@ClassName=\"ApplicationFrameWindow\"][@Name=\"Calculator\"]/Window[@ClassName=\"Windows.UI.Core.CoreWindow\"][@Name=\"Calculator\"]/Group[@ClassName=\"LandmarkTarget\"]/Group[@ClassName=\"NamedContainerAutomationPeer\"][@Name=\"Standard operators\"]/Button[@Name=\"Plus\"][@AutomationId=\"plusButton\"]";
                var winElem_LeftClickButtonPlus_52_24 = desktopSession.FindElementByAbsoluteXPath(xpath_LeftClickButtonPlus_52_24);
                if (winElem_LeftClickButtonPlus_52_24 != null)
                {
                    winElem_LeftClickButtonPlus_52_24.Click();
                }
                else
                {
                    Console.WriteLine($"Failed to find element using xpath: {xpath_LeftClickButtonPlus_52_24}");
                    return;
                }


                // LeftClick on Button "Two" at (58,28)
                Console.WriteLine("LeftClick on Button \"Two\" at (58,28)");
                string xpath_LeftClickButtonTwo_58_28 = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Window[@ClassName=\"ApplicationFrameWindow\"][@Name=\"Calculator\"]/Window[@ClassName=\"Windows.UI.Core.CoreWindow\"][@Name=\"Calculator\"]/Group[@ClassName=\"LandmarkTarget\"]/Group[@Name=\"Number pad\"][@AutomationId=\"NumberPad\"]/Button[@Name=\"Two\"][@AutomationId=\"num2Button\"]";
                var winElem_LeftClickButtonTwo_58_28 = desktopSession.FindElementByAbsoluteXPath(xpath_LeftClickButtonTwo_58_28);
                if (winElem_LeftClickButtonTwo_58_28 != null)
                {
                    winElem_LeftClickButtonTwo_58_28.Click();
                }
                else
                {
                    Console.WriteLine($"Failed to find element using xpath: {xpath_LeftClickButtonTwo_58_28}");
                    return;
                }


                // LeftClick on Button "Equals" at (56,27)
                Console.WriteLine("LeftClick on Button \"Equals\" at (56,27)");
                string xpath_LeftClickButtonEquals_56_27 = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Window[@ClassName=\"ApplicationFrameWindow\"][@Name=\"Calculator\"]/Window[@ClassName=\"Windows.UI.Core.CoreWindow\"][@Name=\"Calculator\"]/Group[@ClassName=\"LandmarkTarget\"]/Group[@ClassName=\"NamedContainerAutomationPeer\"][@Name=\"Standard operators\"]/Button[@Name=\"Equals\"][@AutomationId=\"equalButton\"]";
                var winElem_LeftClickButtonEquals_56_27 = desktopSession.FindElementByAbsoluteXPath(xpath_LeftClickButtonEquals_56_27);
                if (winElem_LeftClickButtonEquals_56_27 != null)
                {
                    winElem_LeftClickButtonEquals_56_27.Click();
                }
                else
                {
                    Console.WriteLine($"Failed to find element using xpath: {xpath_LeftClickButtonEquals_56_27}");
                    return;
                }


                // LeftClick on Text "Display is 3" at (472,2)
                Console.WriteLine("LeftClick on Text \"Display is 3\" at (472,2)");
                string xpath_LeftClickTextDisplayis3_472_2 = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Window[@ClassName=\"ApplicationFrameWindow\"][@Name=\"Calculator\"]/Window[@ClassName=\"Windows.UI.Core.CoreWindow\"][@Name=\"Calculator\"]/Group[@ClassName=\"LandmarkTarget\"]/Text[@Name=\"Display is 3\"][@AutomationId=\"CalculatorResults\"]";
                var winElem_LeftClickTextDisplayis3_472_2 = desktopSession.FindElementByAbsoluteXPath(xpath_LeftClickTextDisplayis3_472_2);
                if (winElem_LeftClickTextDisplayis3_472_2 != null)
                {
                    winElem_LeftClickTextDisplayis3_472_2.Click();
                }
                else
                {
                    Console.WriteLine($"Failed to find element using xpath: {xpath_LeftClickTextDisplayis3_472_2}");
                    return;
                }


                // LeftClick on Button "Close Calculator" at (24,15)
                Console.WriteLine("LeftClick on Button \"Close Calculator\" at (24,15)");
                string xpath_LeftClickButtonCloseCalcu_24_15 = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Window[@ClassName=\"ApplicationFrameWindow\"][@Name=\"Calculator\"]/Window[@Name=\"Calculator\"][@AutomationId=\"TitleBar\"]/Button[@Name=\"Close Calculator\"][@AutomationId=\"Close\"]";
                var winElem_LeftClickButtonCloseCalcu_24_15 = desktopSession.FindElementByAbsoluteXPath(xpath_LeftClickButtonCloseCalcu_24_15);
                if (winElem_LeftClickButtonCloseCalcu_24_15 != null)
                {
                    winElem_LeftClickButtonCloseCalcu_24_15.Click();
                }
                else
                {
                    Console.WriteLine($"Failed to find element using xpath: {xpath_LeftClickButtonCloseCalcu_24_15}");
                    return;
                }


                // test complete
                bSuccess = true;
            }
            finally
            {
                Assert.AreEqual(bSuccess, true);
            }
        }
    }
}

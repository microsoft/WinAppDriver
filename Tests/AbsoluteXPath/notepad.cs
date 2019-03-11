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
    public class Notepad
    {
        [TestMethod]
        public void TestMethod1()
        {
            DesktopSession desktopSession = new DesktopSession();
            TimeSpan.FromSeconds(2);

            bool bTestSuccess = false;

            try
            {
                // LeftClick on Button "Type here to search" at (85,17)
                Console.WriteLine("LeftClick on Button \"Type here to search\" at (85,17)");
                string xpath_LeftClickButtonTypehereto_85_17 = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Pane[@ClassName=\"Shell_TrayWnd\"][@Name=\"Taskbar\"]/Pane[@ClassName=\"TrayDummySearchControl\"]/Button[@ClassName=\"Button\"][@Name=\"Type here to search\"]";
                var winElem_LeftClickButtonTypehereto_85_17 = desktopSession.FindElementByAbsoluteXPath(xpath_LeftClickButtonTypehereto_85_17);
                if (winElem_LeftClickButtonTypehereto_85_17 != null)
                {
                    winElem_LeftClickButtonTypehereto_85_17.Click();
                }
                else
                {
                    Console.WriteLine($"Failed to find element using xpath: {xpath_LeftClickButtonTypehereto_85_17}");
                    return;
                }


                // KeyboardInput VirtualKeys=""notepad"" CapsLock=False NumLock=False ScrollLock=False
                Console.WriteLine("KeyboardInput VirtualKeys=\"\"notepad\"\" CapsLock=False NumLock=False ScrollLock=False");
                System.Threading.Thread.Sleep(1000);
                winElem_LeftClickButtonTypehereto_85_17.SendKeys("notepad");


                // LeftClick on ListItem "Notepad, Desktop app, Press right to switch preview" at (46,32)
                Console.WriteLine("LeftClick on ListItem \"Notepad, Desktop app, Press right to switch preview\" at (46,32)");
                string xpath_LeftClickListItemNotepadDes_46_32 = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Window[@ClassName=\"Windows.UI.Core.CoreWindow\"][@Name=\"Cortana\"]/Pane[@ClassName=\"WebView\"][@Name=\"Bing\"]/Pane[@Name=\"Bing\"]/List[@Name=\"Results\"][@AutomationId=\"suggestionsList\"]/ListItem[@Name=\"Notepad, Desktop app, Press right to switch preview\"][starts-with(@AutomationId,\"id_\")]";
                var winElem_LeftClickListItemNotepadDes_46_32 = desktopSession.FindElementByAbsoluteXPath(xpath_LeftClickListItemNotepadDes_46_32);
                if (winElem_LeftClickListItemNotepadDes_46_32 != null)
                {
                    winElem_LeftClickListItemNotepadDes_46_32.Click();
                }
                else
                {
                    Console.WriteLine($"Failed to find element using xpath: {xpath_LeftClickListItemNotepadDes_46_32}");
                    return;
                }


                // LeftClick on Document "Text Editor" at (143,95)
                Console.WriteLine("LeftClick on Document \"Text Editor\" at (143,95)");
                string xpath_LeftClickDocumentTextEditor_143_95 = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Window[@ClassName=\"Notepad\"][contains(@Name,\"Untitled\")]/Document[@ClassName=\"Edit\"][@Name=\"Text Editor\"]";
                var winElem_LeftClickDocumentTextEditor_143_95 = desktopSession.FindElementByAbsoluteXPath(xpath_LeftClickDocumentTextEditor_143_95);
                if (winElem_LeftClickDocumentTextEditor_143_95 != null)
                {
                    winElem_LeftClickDocumentTextEditor_143_95.Click();
                }
                else
                {
                    Console.WriteLine($"Failed to find element using xpath: {xpath_LeftClickDocumentTextEditor_143_95}");
                    return;
                }


                // KeyboardInput VirtualKeys=""test"" CapsLock=False NumLock=False ScrollLock=False
                Console.WriteLine("KeyboardInput VirtualKeys=\"\"test\"\" CapsLock=False NumLock=False ScrollLock=False");
                System.Threading.Thread.Sleep(100);
                winElem_LeftClickDocumentTextEditor_143_95.SendKeys("test");

                Console.WriteLine("KeyboardInput VirtualKeys=\"Alt + F\" CapsLock=False NumLock=False ScrollLock=False");
                System.Threading.Thread.Sleep(1000);
                winElem_LeftClickDocumentTextEditor_143_95.SendKeys(Keys.Alt + "F" + Keys.Alt);

                // LeftClick on MenuItem "Save As..." at (62,17)
                Console.WriteLine("LeftClick on MenuItem \"Save As...\" at (62,17)");
                string xpath_LeftClickMenuItemSaveAs_62_17 = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Window[@ClassName=\"Notepad\"][contains(@Name,\"Untitled\")]/Menu[@ClassName=\"#32768\"][@Name=\"File\"]/MenuItem[@Name=\"Save As...\"]";
                var winElem_LeftClickMenuItemSaveAs_62_17 = desktopSession.FindElementByAbsoluteXPath(xpath_LeftClickMenuItemSaveAs_62_17);
                if (winElem_LeftClickMenuItemSaveAs_62_17 != null)
                {
                    winElem_LeftClickMenuItemSaveAs_62_17.Click();
                }
                else
                {
                    Console.WriteLine($"Failed to find element using xpath: {xpath_LeftClickMenuItemSaveAs_62_17}");
                    return;
                }


                // LeftClick on Edit "File name:" at (147,11)
                Console.WriteLine("LeftClick on Edit \"File name:\" at (147,11)");
                string xpath_LeftClickEditFilename_147_11 = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Window[@ClassName=\"Notepad\"][contains(@Name,\"Untitled\")]/Window[@ClassName=\"#32770\"][@Name=\"Save As\"]/Pane[@ClassName=\"DUIViewWndClassName\"]/ComboBox[@Name=\"File name:\"][@AutomationId=\"FileNameControlHost\"]/Edit[@ClassName=\"Edit\"][@Name=\"File name:\"]";
                var winElem_LeftClickEditFilename_147_11 = desktopSession.FindElementByAbsoluteXPath(xpath_LeftClickEditFilename_147_11);
                if (winElem_LeftClickEditFilename_147_11 != null)
                {
                    winElem_LeftClickEditFilename_147_11.Click();
                }
                else
                {
                    Console.WriteLine($"Failed to find element using xpath: {xpath_LeftClickEditFilename_147_11}");
                    return;
                }


                // KeyboardInput VirtualKeys="Keys.Backspace + Keys.BackspaceKeys.Backspace + Keys.BackspaceKeys.Backspace + Keys.BackspaceKeys.Backspace + Keys.BackspaceKeys.Backspace + Keys.Backspace"winappdrivertest.txt"" CapsLock=False NumLock=False ScrollLock=False
                Console.WriteLine("KeyboardInput VirtualKeys=\"Keys.Backspace + Keys.BackspaceKeys.Backspace + Keys.BackspaceKeys.Backspace + Keys.BackspaceKeys.Backspace + Keys.BackspaceKeys.Backspace + Keys.Backspace\"winappdrivertest.txt\"\" CapsLock=False NumLock=False ScrollLock=False");
                System.Threading.Thread.Sleep(100);
                winElem_LeftClickEditFilename_147_11.SendKeys(Keys.Backspace + Keys.Backspace);
                winElem_LeftClickEditFilename_147_11.SendKeys(Keys.Backspace + Keys.Backspace);
                winElem_LeftClickEditFilename_147_11.SendKeys(Keys.Backspace + Keys.Backspace);
                winElem_LeftClickEditFilename_147_11.SendKeys(Keys.Backspace + Keys.Backspace);
                winElem_LeftClickEditFilename_147_11.SendKeys(Keys.Backspace + Keys.Backspace);
                winElem_LeftClickEditFilename_147_11.SendKeys("1winappdrivertest.txt");


                // LeftClick on Button "Save" at (22,14)
                Console.WriteLine("LeftClick on Button \"Save\" at (22,14)");
                string xpath_LeftClickButtonSave_22_14 = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Window[@ClassName=\"Notepad\"][contains(@Name,\"Untitled\")]/Window[@ClassName=\"#32770\"][@Name=\"Save As\"]/Button[@ClassName=\"Button\"][@Name=\"Save\"]";
                var winElem_LeftClickButtonSave_22_14 = desktopSession.FindElementByAbsoluteXPath(xpath_LeftClickButtonSave_22_14);
                if (winElem_LeftClickButtonSave_22_14 != null)
                {
                    winElem_LeftClickButtonSave_22_14.Click();
                }
                else
                {
                    Console.WriteLine($"Failed to find element using xpath: {xpath_LeftClickButtonSave_22_14}");
                    return;
                }

                Console.WriteLine("KeyboardInput VirtualKeys=\"Alt + F\" CapsLock=False NumLock=False ScrollLock=False");
                System.Threading.Thread.Sleep(1000);
                winElem_LeftClickDocumentTextEditor_143_95.SendKeys(Keys.Alt + "F" + Keys.Alt);

                // LeftClick on MenuItem "Open..." at (44,11)
                Console.WriteLine("LeftClick on MenuItem \"Open...\" at (44,11)");
                string xpath_LeftClickMenuItemOpen_44_11 = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Window[@ClassName=\"Notepad\"][contains(@Name,\"1winappdrivertest\")]/Menu[@ClassName=\"#32768\"][@Name=\"File\"]/MenuItem[@Name=\"Open...\"]";
                var winElem_LeftClickMenuItemOpen_44_11 = desktopSession.FindElementByAbsoluteXPath(xpath_LeftClickMenuItemOpen_44_11);
                if (winElem_LeftClickMenuItemOpen_44_11 != null)
                {
                    winElem_LeftClickMenuItemOpen_44_11.Click();
                }
                else
                {
                    Console.WriteLine($"Failed to find element using xpath: {xpath_LeftClickMenuItemOpen_44_11}");
                    return;
                }


                // RightClick on Edit "Name" at (9,8)
                Console.WriteLine("RightClick on Edit \"Name\" at (9,8)");
                string xpath_RightClickEditName_9_8 = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Window[@ClassName=\"Notepad\"][contains(@Name,\"1winappdrivertest\")]/Window[@ClassName=\"#32770\"][@Name=\"Open\"]/Pane[@ClassName=\"DUIViewWndClassName\"]/Pane[@Name=\"Shell Folder View\"][@AutomationId=\"listview\"]/List[@ClassName=\"UIItemsView\"][@Name=\"Items View\"]/ListItem[@ClassName=\"UIItem\"][@Name=\"1winappdrivertest.txt\"]/Edit[@Name=\"Name\"][@AutomationId=\"System.ItemNameDisplay\"]";
                var winElem_RightClickEditName_9_8 = desktopSession.FindElementByAbsoluteXPath(xpath_RightClickEditName_9_8);
                if (winElem_RightClickEditName_9_8 != null)
                {
                    winElem_RightClickEditName_9_8.Click();
                    desktopSession.DesktopSessionElement.Mouse.MouseMove(winElem_RightClickEditName_9_8.Coordinates);
                    desktopSession.DesktopSessionElement.Mouse.ContextClick(null);
                }
                else
                {
                    Console.WriteLine($"Failed to find element using xpath: {xpath_RightClickEditName_9_8}");
                    return;
                }


                // LeftClick on MenuItem "Delete" at (71,5)
                Console.WriteLine("LeftClick on MenuItem \"Delete\" at (71,5)");
                string xpath_LeftClickMenuItemDelete_71_5 = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Menu[@ClassName=\"#32768\"][@Name=\"Context\"]/MenuItem[@Name=\"Delete\"]";
                var winElem_LeftClickMenuItemDelete_71_5 = desktopSession.FindElementByAbsoluteXPath(xpath_LeftClickMenuItemDelete_71_5);
                if (winElem_LeftClickMenuItemDelete_71_5 != null)
                {
                    winElem_LeftClickMenuItemDelete_71_5.Click();
                }
                else
                {
                    Console.WriteLine($"Failed to find element using xpath: {xpath_LeftClickMenuItemDelete_71_5}");
                    return;
                }


                // LeftClick on Button "Cancel" at (24,13)
                Console.WriteLine("LeftClick on Button \"Cancel\" at (24,13)");
                string xpath_LeftClickButtonCancel_24_13 = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Window[@ClassName=\"Notepad\"][contains(@Name,\"1winappdrivertest\")]/Window[@ClassName=\"#32770\"][@Name=\"Open\"]/Button[@ClassName=\"Button\"][@Name=\"Cancel\"]";
                var winElem_LeftClickButtonCancel_24_13 = desktopSession.FindElementByAbsoluteXPath(xpath_LeftClickButtonCancel_24_13);
                if (winElem_LeftClickButtonCancel_24_13 != null)
                {
                    winElem_LeftClickButtonCancel_24_13.Click();
                }
                else
                {
                    Console.WriteLine($"Failed to find element using xpath: {xpath_LeftClickButtonCancel_24_13}");
                    return;
                }


                // LeftClick on Button "Close" at (28,15)
                Console.WriteLine("LeftClick on Button \"Close\" at (28,15)");
                string xpath_LeftClickButtonClose_28_15 = "/Pane[@ClassName=\"#32769\"][@Name=\"Desktop 1\"]/Window[@ClassName=\"Notepad\"][contains(@Name,\"1winappdrivertest\")]/TitleBar[@AutomationId=\"TitleBar\"]/Button[@Name=\"Close\"]";
                var winElem_LeftClickButtonClose_28_15 = desktopSession.FindElementByAbsoluteXPath(xpath_LeftClickButtonClose_28_15);
                if (winElem_LeftClickButtonClose_28_15 != null)
                {
                    winElem_LeftClickButtonClose_28_15.Click();
                }
                else
                {
                    Console.WriteLine($"Failed to find element using xpath: {xpath_LeftClickButtonClose_28_15}");
                    return;
                }

                bTestSuccess = true;
            }
            finally
            {
                Assert.AreEqual(bTestSuccess, true);
            }
        }
    }
}

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace WinAppDriverUIRecorder
{
    public enum UiTaskName : UInt32
    {
        UnknownUiTask = 3000,
        KeyboardInput,
        LeftClick,
        RightClick,
        LeftDblClick,
        Drag,
        DragStop,
        MouseWheel,
        MouseHover,
        Inspect
    }

    public class CharPair
    {
        public CharPair(char c1, char c2)
        {
            char1 = c1;
            char2 = c2;
        }

        public char char1;
        public char char2;
    }

    class ConstVariables
    {
        public const int DragDeltaOffset = 12345678;

        public const string DELTAX = "DELTAX";
        public const string DELTAY = "DELTAY";
        public const string WHEEL = "WHEEL";

        public const string ClassName = "ClassName";
        public const string Name = "Name";
        public const string AutomationId = "AutomationId";
        public const string UiTask = "UiTask";
        public const string Pos = "Pos";

        public static Dictionary<string, string> s_Vk2String = new Dictionary<string, string>();
        public static Dictionary<int, CharPair> s_Vk2CharPair = new Dictionary<int, CharPair>();

        public static UiTaskName FromStringTaskName(string strTask)
        {
            foreach (UiTaskName val in Enum.GetValues(typeof(UiTaskName)))
            {
                if (strTask == val.ToString())
                {
                    return val;
                }
            }

            return UiTaskName.UnknownUiTask;
        }

        public static void InitVk2StringMap()
        {
            s_Vk2String.Add(VirtualKeys.VK_NUMPAD0.ToString(), "NumberPad0");
            s_Vk2String.Add(VirtualKeys.VK_NUMPAD1.ToString(), "NumberPad1");
            s_Vk2String.Add(VirtualKeys.VK_NUMPAD2.ToString(), "NumberPad2");
            s_Vk2String.Add(VirtualKeys.VK_NUMPAD3.ToString(), "NumberPad3");
            s_Vk2String.Add(VirtualKeys.VK_NUMPAD4.ToString(), "NumberPad4");
            s_Vk2String.Add(VirtualKeys.VK_NUMPAD5.ToString(), "NumberPad5");
            s_Vk2String.Add(VirtualKeys.VK_NUMPAD6.ToString(), "NumberPad6");
            s_Vk2String.Add(VirtualKeys.VK_NUMPAD7.ToString(), "NumberPad7");
            s_Vk2String.Add(VirtualKeys.VK_NUMPAD8.ToString(), "NumberPad8");
            s_Vk2String.Add(VirtualKeys.VK_NUMPAD9.ToString(), "NumberPad9");
            s_Vk2String.Add(VirtualKeys.VK_MULTIPLY.ToString(), "Multiply");
            s_Vk2String.Add(VirtualKeys.VK_ADD.ToString(), "Add");
            s_Vk2String.Add(VirtualKeys.VK_SEPARATOR.ToString(), "Separator");
            s_Vk2String.Add(VirtualKeys.VK_OEM_NEC_EQUAL.ToString(), "Equal");
            s_Vk2String.Add(VirtualKeys.VK_SUBTRACT.ToString(), "Subtract");
            s_Vk2String.Add(VirtualKeys.VK_DIVIDE.ToString(), "Divide");
            s_Vk2String.Add(VirtualKeys.VK_F1.ToString(), "F1");
            s_Vk2String.Add(VirtualKeys.VK_F2.ToString(), "F2");
            s_Vk2String.Add(VirtualKeys.VK_F3.ToString(), "F3");
            s_Vk2String.Add(VirtualKeys.VK_F4.ToString(), "F4");
            s_Vk2String.Add(VirtualKeys.VK_F5.ToString(), "F5");
            s_Vk2String.Add(VirtualKeys.VK_F6.ToString(), "F6");
            s_Vk2String.Add(VirtualKeys.VK_F7.ToString(), "F7");
            s_Vk2String.Add(VirtualKeys.VK_F8.ToString(), "F8");
            s_Vk2String.Add(VirtualKeys.VK_F9.ToString(), "F9");
            s_Vk2String.Add(VirtualKeys.VK_F10.ToString(), "F10");
            s_Vk2String.Add(VirtualKeys.VK_F11.ToString(), "F11");
            s_Vk2String.Add(VirtualKeys.VK_F12.ToString(), "F12");
            s_Vk2String.Add(VirtualKeys.VK_DECIMAL.ToString(), "Decimal");
            //s_Vk2String.Add(VirtualKeys., "Meta");
            s_Vk2String.Add(VirtualKeys.VK_OEM_1.ToString(), "Semicolon");
            s_Vk2String.Add(VirtualKeys.VK_INSERT.ToString(), "Insert");
            s_Vk2String.Add(VirtualKeys.VK_CANCEL.ToString(), "Cancel");
            s_Vk2String.Add(VirtualKeys.VK_HELP.ToString(), "Help");
            s_Vk2String.Add(VirtualKeys.VK_BACK.ToString(), "Backspace");
            s_Vk2String.Add(VirtualKeys.VK_TAB.ToString(), "Tab");
            s_Vk2String.Add(VirtualKeys.VK_CLEAR.ToString(), "Clear");
            s_Vk2String.Add(VirtualKeys.VK_RETURN.ToString(), "Return");
            //s_Vk2String.Add(VirtualKeys.VK_RETURN.ToString(), "Enter");
            s_Vk2String.Add(VirtualKeys.VK_LSHIFT.ToString(), "Shift");
            s_Vk2String.Add(VirtualKeys.VK_RSHIFT.ToString(), "RightShift");
            s_Vk2String.Add(VirtualKeys.VK_LCONTROL.ToString(), "Control");
            s_Vk2String.Add(VirtualKeys.VK_RCONTROL.ToString(), "RightControl");
            s_Vk2String.Add(VirtualKeys.VK_LMENU.ToString(), "Alt");
            s_Vk2String.Add(VirtualKeys.VK_RMENU.ToString(), "RightAlt");

            s_Vk2String.Add(VirtualKeys.VK_DELETE.ToString(), "Delete");
            s_Vk2String.Add(VirtualKeys.VK_PAUSE.ToString(), "Pause");
            s_Vk2String.Add(VirtualKeys.VK_SPACE.ToString(), "Space");
            s_Vk2String.Add(VirtualKeys.VK_PRIOR.ToString(), "PageUp");
            s_Vk2String.Add(VirtualKeys.VK_NEXT.ToString(), "PageDown");
            s_Vk2String.Add(VirtualKeys.VK_END.ToString(), "End");
            s_Vk2String.Add(VirtualKeys.VK_HOME.ToString(), "Home");
            s_Vk2String.Add(VirtualKeys.VK_LEFT.ToString(), "Left");
            //s_Vk2String.Add(VirtualKeys.VK_LEFT.ToString(), "ArrowLeft");
            s_Vk2String.Add(VirtualKeys.VK_UP.ToString(), "Up");
            //s_Vk2String.Add(VirtualKeys.VK_UP.ToString(), "ArrowUp");
            s_Vk2String.Add(VirtualKeys.VK_RIGHT.ToString(), "Right");
            //s_Vk2String.Add(VirtualKeys.VK_RIGHT.ToString(), "ArrowRight");
            s_Vk2String.Add(VirtualKeys.VK_DOWN.ToString(), "Down");
            //s_Vk2String.Add(VirtualKeys.VK_DOWN.ToString(), "ArrowDown");
            s_Vk2String.Add(VirtualKeys.VK_ESCAPE.ToString(), "Escape");
            s_Vk2String.Add(VirtualKeys.VK_LWIN.ToString(), "Command");
            s_Vk2String.Add(VirtualKeys.VK_RWIN.ToString(), "Command");

            s_Vk2CharPair.Add(0x30, new CharPair('0', ')'));
            s_Vk2CharPair.Add(0x31, new CharPair('1', '!'));
            s_Vk2CharPair.Add(0x32, new CharPair('2', '@'));
            s_Vk2CharPair.Add(0x33, new CharPair('3', '#'));
            s_Vk2CharPair.Add(0x34, new CharPair('4', '$'));
            s_Vk2CharPair.Add(0x35, new CharPair('5', '%'));
            s_Vk2CharPair.Add(0x36, new CharPair('6', '^'));
            s_Vk2CharPair.Add(0x37, new CharPair('7', '&'));
            s_Vk2CharPair.Add(0x38, new CharPair('8', '*'));
            s_Vk2CharPair.Add(0x39, new CharPair('9', '('));
            s_Vk2CharPair.Add((int)VirtualKeys.VK_OEM_3, new CharPair('`', '~'));
            s_Vk2CharPair.Add((int)VirtualKeys.VK_OEM_MINUS, new CharPair('-', '_'));
            s_Vk2CharPair.Add((int)VirtualKeys.VK_OEM_PLUS, new CharPair('=', '+'));
            s_Vk2CharPair.Add((int)VirtualKeys.VK_OEM_4, new CharPair('[', '{'));
            s_Vk2CharPair.Add((int)VirtualKeys.VK_OEM_6, new CharPair(']', '}'));
            s_Vk2CharPair.Add((int)VirtualKeys.VK_OEM_5, new CharPair('\\', '|'));
            s_Vk2CharPair.Add((int)VirtualKeys.VK_OEM_1, new CharPair(';', ':'));
            s_Vk2CharPair.Add((int)VirtualKeys.VK_OEM_7, new CharPair(',', '<'));
            s_Vk2CharPair.Add((int)VirtualKeys.VK_OEM_PERIOD, new CharPair('.', '>'));
            s_Vk2CharPair.Add((int)VirtualKeys.VK_OEM_2, new CharPair('/', '?'));

            s_Vk2CharPair.Add(0x41, new CharPair('a', 'A'));
            s_Vk2CharPair.Add(0x42, new CharPair('b', 'B'));
            s_Vk2CharPair.Add(0x43, new CharPair('c', 'C'));
            s_Vk2CharPair.Add(0x44, new CharPair('d', 'D'));
            s_Vk2CharPair.Add(0x45, new CharPair('e', 'E'));
            s_Vk2CharPair.Add(0x46, new CharPair('f', 'F'));
            s_Vk2CharPair.Add(0x47, new CharPair('g', 'G'));
            s_Vk2CharPair.Add(0x48, new CharPair('h', 'H'));
            s_Vk2CharPair.Add(0x49, new CharPair('i', 'I'));
            s_Vk2CharPair.Add(0x4A, new CharPair('j', 'J'));
            s_Vk2CharPair.Add(0x4B, new CharPair('k', 'K'));
            s_Vk2CharPair.Add(0x4C, new CharPair('l', 'L'));
            s_Vk2CharPair.Add(0x4D, new CharPair('m', 'M'));
            s_Vk2CharPair.Add(0x4E, new CharPair('n', 'N'));
            s_Vk2CharPair.Add(0x4F, new CharPair('o', 'O'));
            s_Vk2CharPair.Add(0x50, new CharPair('p', 'P'));
            s_Vk2CharPair.Add(0x51, new CharPair('q', 'Q'));
            s_Vk2CharPair.Add(0x52, new CharPair('r', 'R'));
            s_Vk2CharPair.Add(0x53, new CharPair('s', 'S'));
            s_Vk2CharPair.Add(0x54, new CharPair('t', 'T'));
            s_Vk2CharPair.Add(0x55, new CharPair('u', 'U'));
            s_Vk2CharPair.Add(0x56, new CharPair('v', 'V'));
            s_Vk2CharPair.Add(0x57, new CharPair('w', 'W'));
            s_Vk2CharPair.Add(0x58, new CharPair('x', 'X'));
            s_Vk2CharPair.Add(0x59, new CharPair('y', 'Y'));
            s_Vk2CharPair.Add(0x5A, new CharPair('z', 'Z'));
        }

        public static string Vk2string(string vkString)
        {
            if (s_Vk2String.ContainsKey(vkString))
            {
                return s_Vk2String[vkString];
            }
            else
            {
                return vkString;
            }
        }

        public static char Vk2char(int vk, bool bShiftDown)
        {
            if (s_Vk2CharPair.ContainsKey(vk))
            {
                CharPair chaiPaire = s_Vk2CharPair[vk];
                return bShiftDown ? chaiPaire.char2 : chaiPaire.char1;
            }

            return (char)0;
        }
    }

    class GenerateXPath
    {
        const string sNameValue = "[@{0}=\\\"{1}\\\"]";
        const string sNameStartsWithValue = "[starts-with(@{0},\\\"{1}\\\")]";

        public static string GenerateXPathToUiElement(string strXmlNodes)
        {
            XmlDocument xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.LoadXml(strXmlNodes);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

            string strRet = "";
            foreach (XmlElement uiTask in xmlDocument.GetElementsByTagName(ConstVariables.UiTask))
            {
                strRet += GetXPathFromUiTaskNode(uiTask);
            }

            if (strRet.Length > 2)
            {
                return strRet.Substring(0, strRet.Length - 2) + "\n"; // remove last ,
            }
            else
            {
                return strRet;
            }
        }

        public static string XmlEncode(string strData)
        {
            strData = strData.Replace("&", "&amp;");
            strData = strData.Replace("\"", "&quot;");
            strData = strData.Replace("\'", "&apos;");
            strData = strData.Replace("<", "&lt;");
            strData = strData.Replace(">", "&gt;");
            return strData;
        }

        static string CheckAndFixNoneStaticValue(string strValue)
        {
            const string strGUIDPtn = @"[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}";

            if (strValue.StartsWith("HwndWrapper[") || strValue.StartsWith("starts-with:HwndWrapper["))
            {
                strValue = "starts-with:HwndWrapper";
            }
            else if (strValue.StartsWith("ATL:") || strValue.StartsWith("starts-with:ATL:"))
            {
                strValue = "starts-with:ATL";
            }
            else if (strValue.StartsWith("WindowsForms10.") || strValue.StartsWith("starts-with:WindowsForms10."))
            {
                strValue = "starts-with:WindowsForms10";
            }
            else if (System.Text.RegularExpressions.Regex.Match(strValue, strGUIDPtn).Success)
            {
                strValue = "";
            }

            return strValue;
        }

        static string GetXPathFromUiTaskNode(XmlElement uiTaskNode)
        {
            if (uiTaskNode == null || uiTaskNode.ChildNodes.Count < 1)
            {
                return "";
            }

            string tag, ClassName, Name, AutomationId, Pos;
            string xPath = "";

            for (int i = 0; i < uiTaskNode.ChildNodes.Count; i++)
            {
                XmlNode childNode = uiTaskNode.ChildNodes[i];

                tag = childNode.Name != "Unknown" ? childNode.Name : "*";
                AutomationId = XmlEncode(childNode.Attributes[ConstVariables.AutomationId].Value);
                AutomationId = CheckAndFixNoneStaticValue(AutomationId);

                Name = XmlEncode(childNode.Attributes[ConstVariables.Name].Value);

                ClassName = childNode.Attributes[ConstVariables.ClassName].Value;
                ClassName = CheckAndFixNoneStaticValue(ClassName);
                Pos = childNode.Attributes[ConstVariables.Pos].Value;

                xPath += $"/{tag}";
                int nPos = xPath.Length;

                // AutomationId (like UIs on Cortana search result list) created at runtime may end with digits
                if (!string.IsNullOrEmpty(AutomationId) && !AutomationId.StartsWith("starts-with:"))
                {
                    string patAutoIdEndsWithDigits = @"^([^\d]*)[\d]+$";
                    System.Text.RegularExpressions.Regex regAutoId = new System.Text.RegularExpressions.Regex(patAutoIdEndsWithDigits, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    if (regAutoId != null)
                    {
                        System.Text.RegularExpressions.Match matchAutoId = regAutoId.Match(AutomationId);
                        if (matchAutoId.Success && matchAutoId.Groups.Count > 1)
                        {
                            if (matchAutoId.Groups[1].Length > 0)
                            {
                                AutomationId = "starts-with:" + matchAutoId.Groups[1].ToString();
                            }
                            else
                            {
                                AutomationId = null;
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(AutomationId))
                {
                    if (AutomationId.StartsWith("starts-with:"))
                    {
                        AutomationId = AutomationId.Remove(0, "starts-with:".Length);
                        xPath += string.Format(sNameStartsWithValue, ConstVariables.AutomationId, AutomationId);
                    }
                    else
                    {
                        xPath += string.Format(sNameValue, ConstVariables.AutomationId, AutomationId);
                    }
                }

                if (!string.IsNullOrEmpty(Name))
                {
                    if (Name.StartsWith("starts-with:"))
                    {
                        Name = Name.Remove(0, "starts-with:".Length);
                        xPath += string.Format(sNameStartsWithValue, ConstVariables.Name, Name);
                    }
                    else
                    {
                        xPath += string.Format(sNameValue, ConstVariables.Name, Name);
                    }
                }

                if (!string.IsNullOrEmpty(ClassName) && string.IsNullOrEmpty(AutomationId))
                {
                    if (ClassName.StartsWith("starts-with:"))
                    {
                        ClassName = ClassName.Remove(0, "starts-with:".Length);
                        xPath += string.Format(sNameStartsWithValue, ConstVariables.ClassName, ClassName);
                    }
                    else
                    {
                        xPath += string.Format(sNameValue, ConstVariables.ClassName, ClassName);
                    }
                }

                if (!string.IsNullOrEmpty(Pos) && string.IsNullOrEmpty(AutomationId) && string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(ClassName))
                {
                    //TODO: verify
                    xPath = xPath.Insert(nPos, $"[position()={Pos}]");
                }
            }

            return "\"" + xPath + "\",\n";
        }
    }
}

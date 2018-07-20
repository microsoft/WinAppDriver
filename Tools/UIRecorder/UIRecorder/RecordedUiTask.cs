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
    public class RecordedUiTask
    {
        string _strXmlNode = null;
        string _strXPath = null;
        string _strXPathSessionRoot = null;
        string _strDescription = null;
        UiTaskName _uiTaskName = UiTaskName.UnknownUiTask;
        string _strLeft = null;
        string _strTop = null;
        string _strBase64Text = null;
        bool _bCapsLock = false;
        bool _bNumLock = false;
        bool _bScrollLock = false;
        int _nDeltaX = 0;
        int _nDeltaY = 0;
        public int _tickCount;

        public RecordedUiTask(string strXmlNode, int nTick, UiTaskName taskName)
        {
            _uiTaskName = taskName;
            _strXmlNode = strXmlNode;
            _tickCount = nTick == 0 ? Environment.TickCount : nTick;
        }

        public string GetXPath()
        {
            if (_strXPath == null)
            {
                _strXPath = GenerateXPath.GenerateXPathToUiElement(_strXmlNode).Trim();
            }

            return _strXPath;
        }

        public void SetXPath(string xpath)
        {
            _strXPath = xpath;
        }

        public string GetRootSessionXPath()
        {
            return _strXPathSessionRoot ?? "";
        }

        public string GetXPathExcludingSessinoRoot()
        {
            if (_strXPathSessionRoot != null && _strXPathSessionRoot.Length > 1)
            {
                return "\"" + _strXPath.Substring(_strXPathSessionRoot.Length - 1);
            }
            else
            {
                return "";
            }
        }

        public void SetRootSessionXPath(string xpath)
        {
            _strXPathSessionRoot = xpath;
        }

        public string GetXml()
        {
            return _strXmlNode;
        }

        public UiTaskName GetTask()
        {
            return _uiTaskName;
        }

        public string GetLeft()
        {
            return _strLeft;
        }

        public string GetTop()
        {
            return _strTop;
        }

        public int GetDeltaX()
        {
            return _nDeltaX;
        }

        public int GetDeltaY()
        {
            return _nDeltaY;
        }

        public int GetTickCount()
        {
            return _tickCount;
        }

        public string GetBase64Text()
        {
            return _strBase64Text;
        }

        public bool GetCapsLock()
        {
            return _bCapsLock;
        }

        public bool GetNumLock()
        {
            return _bNumLock;
        }
        public bool GetScrollLock()
        {
            return _bScrollLock;
        }

        public string GetDescription()
        {
            if (_strDescription == null)
            {
                XmlDocument xmlDocument = new XmlDocument();
                try
                {
                    xmlDocument.LoadXml(_strXmlNode);
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }

                XmlNodeList uiTasks = xmlDocument.GetElementsByTagName("UiTask");
                if (uiTasks.Count == 1)
                {
                    string strTask = uiTasks[0].Attributes["task"].Value;

                    _uiTaskName = ConstVariables.FromStringTaskName(strTask);

                    if (_uiTaskName == UiTaskName.KeyboardInput)
                    {
                        _strBase64Text = uiTasks[0].Attributes["base64String"].Value;

                        var varCapLock = uiTasks[0].Attributes["CapsLock"].Value;
                        if (varCapLock != null)
                        {
                            _bCapsLock = Convert.ToBoolean(varCapLock);
                        }

                        var varNumLock = uiTasks[0].Attributes["NumLock"].Value;
                        if (varNumLock != null)
                        {
                            _bNumLock = Convert.ToBoolean(varNumLock);
                        }

                        var varScrollLock = uiTasks[0].Attributes["ScrollLock"].Value;
                        if (varScrollLock != null)
                        {
                            _bScrollLock = Convert.ToBoolean(varScrollLock);
                        }

                        var keyboardTaskDescription = GenerateCSCode.GetDecodedKeyboardInput(_strBase64Text, _bCapsLock, _bNumLock, _bScrollLock);
                        StringBuilder sb = new StringBuilder();
                        foreach (var strLine in keyboardTaskDescription)
                        {
                            sb.Append(strLine);
                        }

                        _strDescription = $"{_uiTaskName} VirtualKeys=\"{sb.ToString()}\" CapsLock={_bCapsLock} NumLock={_bNumLock} ScrollLock={_bScrollLock}";
                    }
                    else
                    {
                        var vleft = uiTasks[0].Attributes["x"];
                        _strLeft = vleft != null ? vleft.Value : "";

                        var vtop = uiTasks[0].Attributes["y"];
                        _strTop = vtop != null ? vtop.Value : "";

                        string name = uiTasks[0].LastChild.Attributes[ConstVariables.Name].Value;
                        if (string.IsNullOrEmpty(name))
                        {
                            name = uiTasks[0].LastChild.Attributes[ConstVariables.AutomationId].Value;
                            if (string.IsNullOrEmpty(name))
                            {
                                name = uiTasks[0].Name;
                            }
                        }

                        if (_uiTaskName == UiTaskName.Drag || _uiTaskName == UiTaskName.MouseWheel)
                        {
                            _strDescription = $"{_uiTaskName} on \"{name}\" at ({_strLeft},{_strTop}) drag ({_nDeltaX},{_nDeltaY})";
                        }
                        else
                        {
                            _strDescription = $"{_uiTaskName} on \"{name}\" at ({_strLeft},{_strTop})";
                        }
                    }
                }
            }

            return _strDescription;
        }

        public override string ToString()
        {
            if (_strDescription == null)
            {
                _strDescription = GetDescription();
            }
            return _strDescription;
        }

        public string GetCSCode(int nOrder, int nOrderFocused)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("// " + GetDescription());

            string consoleWriteLine = "Console.WriteLine(\"" + GetDescription().Replace("\"", "\\\"") + "\");";
            sb.AppendLine(consoleWriteLine);

            if (!string.IsNullOrEmpty(GetRootSessionXPath()))
            {
                sb.AppendLine($"// xpath excluding session root: {GetXPathExcludingSessinoRoot()}");
            }

            if (GetTask() == UiTaskName.LeftClick)
            {
                sb.AppendLine(GenerateCSCode.LeftClick(this, nOrder));
            }
            else if (GetTask() == UiTaskName.RightClick)
            {
                sb.AppendLine(GenerateCSCode.RightClick(this, nOrder));
            }
            else if (GetTask() == UiTaskName.LeftDblClick)
            {
                sb.AppendLine(GenerateCSCode.DoubleClick(this, nOrder));
            }
            else if (GetTask() == UiTaskName.MouseWheel)
            {
                sb.AppendLine(GenerateCSCode.Wheel(this, nOrder));
            }
            else if (GetTask() == UiTaskName.Drag)
            {
                sb.AppendLine(GenerateCSCode.Drag(this, nOrder));
            }
            else if (GetTask() == UiTaskName.MouseHover)
            {
                sb.AppendLine(GenerateCSCode.MouseHover(this, nOrder));
            }
            else if (GetTask() == UiTaskName.KeyboardInput)
            {
                sb.AppendLine(GenerateCSCode.SendKeys(this, nOrder, nOrderFocused));
            }
            else
            {
                sb.AppendLine(GenerateCSCode.FindByXPath(this, nOrder));
            }

            return sb.ToString();
        }

        static string ReplaceLastWith(string strScript, string strOld, string strNew)
        {
            int nPos = strScript.LastIndexOf(strOld);
            if (nPos < 0)
            {
                return strScript;
            }

            return strScript.Remove(nPos, strOld.Length).Insert(nPos, strNew);
        }

        public void ChangeClickToDoubleClick()
        {
            _strXmlNode = ReplaceLastWith(_strXmlNode, UiTaskName.LeftClick.ToString(), UiTaskName.LeftDblClick.ToString());
            _uiTaskName = UiTaskName.LeftDblClick;
            _strDescription = null;
        }

        public void DragComplete(int deltaX, int deltaY)
        {
            _nDeltaX = deltaX;
            _nDeltaY = deltaY;

            string strDeltaX = deltaX.ToString();
            string strDeltaY = deltaY.ToString();

            string strUpdated = ReplaceLastWith(_strXmlNode, ConstVariables.DELTAX, strDeltaX);
            _strXmlNode = ReplaceLastWith(strUpdated, ConstVariables.DELTAY, strDeltaY);
            _uiTaskName = UiTaskName.Drag;
            _strDescription = null;
        }

        public void UpdateWheelData(int delta)
        {
            string wdata = ConstVariables.WHEEL;
            int nPos1 = _strXmlNode.IndexOf("wheel=\"") + "wheel=\"".Length;
            int nPos2 = _strXmlNode.IndexOf("\">", nPos1);
            if (nPos2 > nPos1)
            {
                wdata = _strXmlNode.Substring(nPos1, nPos2 - nPos1);
                int nCurData = 0;
                if (int.TryParse(wdata, out nCurData))
                {
                    delta += nCurData;
                }
            }

            _nDeltaX += 1;
            _nDeltaY = delta;

            string strdelta = $"wheel=\"{delta}\"";
            wdata = $"wheel=\"{wdata}\"";
            _strXmlNode = _strXmlNode.Replace(wdata, strdelta);
            _uiTaskName = UiTaskName.MouseWheel;
            _strDescription = null;
        }
    }

    class GenerateCSCode
    {
        public static string LeftClick(RecordedUiTask uiTask, int nOrder)
        {
            return $"string xp{nOrder} = {uiTask.GetXPath()};\n" +
                $"var winElem{nOrder} = MyDesktopSession.FindElementByXPath(xp{nOrder});\n" +
                $"if (winElem{nOrder} != null)\n" +
                "{\n" +
                $"    winElem{nOrder}.Click();\n" +
                "}\n" +
                "else\n" +
                "{\n" +
                "    Console.WriteLine($\"Failed to find element {xp" + $"{nOrder}" + "}\");\n" +
                "    return;\n" +
                "}\n";
        }

        public static string DoubleClick(RecordedUiTask uiTask, int nOrder)
        {
            return $"string xp{nOrder} = {uiTask.GetXPath()};\n" +
                $"var winElem{nOrder} = MyDesktopSession.FindElementByXPath(xp{nOrder});\n" +
                $"if (winElem{nOrder} != null)\n" +
                "{\n" +
                $"    MyDesktopSession.DesktopSession.Mouse.MouseMove(winElem{nOrder}.Coordinates);\n" +
                $"    MyDesktopSession.DesktopSession.Mouse.DoubleClick(null);\n" +
                "}\n" +
                "else\n" +
                "{\n" +
                "    Console.WriteLine($\"Failed to find element {xp" + $"{nOrder}" + "}\");\n" +
                "    return;\n" +
                "}\n";
        }

        public static string RightClick(RecordedUiTask uiTask, int nOrder)
        {
            return $"string xp{nOrder} = {uiTask.GetXPath()};\n" +
                $"var winElem{nOrder} = MyDesktopSession.FindElementByXPath(xp{nOrder});\n" +
                $"if (winElem{nOrder} != null)\n" +
                "{\n" +
                $"    MyDesktopSession.DesktopSession.Mouse.MouseMove(winElem{nOrder}.Coordinates);\n" +
                "    MyDesktopSession.DesktopSession.Mouse.ContextClick(null);\n" +
                "}\n" +
                "else\n" +
                "{\n" +
                "    Console.WriteLine($\"Failed to find element {xp" + $"{nOrder}" + "}\");\n" +
                "    return;\n" +
                "}\n";
        }

        public static string FindByXPath(RecordedUiTask uiTask, int nOrder)
        {
            return $"string xp{nOrder} = {uiTask.GetXPath()};\n" +
                $"var winElem{nOrder} = MyDesktopSession.FindElementByXPath(xp{nOrder});\n" +
                $"if (winElem{nOrder} != null)\n" +
                "{\n" +
                $"   //TODO: Add UI task at ({uiTask.GetLeft()},{uiTask.GetTop()}) on winElem{nOrder}\n" +
                "}\n" +
                "else\n" +
                "{\n" +
                "    Console.WriteLine($\"Failed to find element {xp" + $"{nOrder}" + "}\");\n" +
                "    return;\n" +
                "}\n";
        }

        public static string Drag(RecordedUiTask uiTask, int nOrder)
        {
            return $"string xp{nOrder} = {uiTask.GetXPath()};\n" +
                $"var winElem{nOrder} = MyDesktopSession.FindElementByXPath(xp{nOrder});\n" +
                $"if (winElem{nOrder} != null)\n" +
                "{\n" +
                $"   //TODO: Drag from ({uiTask.GetLeft()},{uiTask.GetTop()}) on winElem{nOrder} by ({uiTask.GetDeltaX()},{uiTask.GetDeltaY()})\n" +
                "}\n" +
                "else\n" +
                "{\n" +
                "    Console.WriteLine($\"Failed to find element {xp" + $"{nOrder}" + "}\");\n" +
                "    return;\n" +
                "}\n";
        }

        public static string Wheel(RecordedUiTask uiTask, int nOrder)
        {
            return $"string xp{nOrder} = {uiTask.GetXPath()};\n" +
                $"var winElem{nOrder} = MyDesktopSession.FindElementByXPath(xp{nOrder});\n" +
                $"if (winElem{nOrder} != null)\n" +
                "{\n" +
                $"   //TODO: Wheel at ({uiTask.GetLeft()},{uiTask.GetTop()}) on winElem{nOrder}, Count:{uiTask.GetDeltaX()}, Total Amount:{uiTask.GetDeltaY()}\n" +
                "}\n" +
                "else\n" +
                "{\n" +
                "    Console.WriteLine($\"Failed to find element {xp" + $"{nOrder}" + "}\");\n" +
                "    return;\n" +
                "}\n";
        }

        public static string MouseHover(RecordedUiTask uiTask, int nOrder)
        {
            return $"string xp{nOrder} = {uiTask.GetXPath()};\n" +
                $"var winElem{nOrder} = MyDesktopSession.FindElementByXPath(xp{nOrder});\n" +
                $"if (winElem{nOrder} != null)\n" +
                "{\n" +
                $"   //TODO: Hover at ({uiTask.GetLeft()},{uiTask.GetTop()}) on winElem{nOrder}\n" +
                "}\n" +
                "else\n" +
                "{\n" +
                "    Console.WriteLine($\"Failed to find element {xp" + $"{nOrder}" + "}\");\n" +
                "    return;\n" +
                "}\n";
        }

        public static List<string> GetDecodedKeyboardInput(string strBase64, bool bCapsLock, bool bNumLock, bool ScrollLock)
        {
            byte[] data = Convert.FromBase64String(strBase64);

            int i = 0;
            bool shift = false;

            StringBuilder sb = new StringBuilder();
            List<string> lines = new List<string>();

            while (i < data.Length / 2)
            {
                int n1 = i * 2;
                int n2 = i * 2 + 1;
                i++;

                bool bIsKeyDown = data[n1] == 0;
                VirtualKeys vk = (VirtualKeys)data[n2];

                if (vk == VirtualKeys.VK_SHIFT || vk == VirtualKeys.VK_LSHIFT || vk == VirtualKeys.VK_RSHIFT)
                {
                    shift = bIsKeyDown;
                    continue;
                }

                if (vk == VirtualKeys.VK_CAPITAL)
                {
                    if (bIsKeyDown)
                    {
                        bCapsLock = !bCapsLock;
                    }
                    continue;
                }

                char ch = ConstVariables.Vk2char((int)vk, shift || bCapsLock);

                // Create lines like winElem1.SendKeys(Keys.Control+ "a" + Keys.Control);
                bool bOutputVK = false;

                if (vk == VirtualKeys.VK_CONTROL ||
                    vk == VirtualKeys.VK_LCONTROL ||
                    vk == VirtualKeys.VK_RCONTROL ||
                    vk == VirtualKeys.VK_MENU ||
                    vk == VirtualKeys.VK_LMENU ||
                    vk == VirtualKeys.VK_RMENU ||
                    vk == VirtualKeys.VK_LWIN ||
                    vk == VirtualKeys.VK_RWIN)
                {
                    bOutputVK = true;
                }

                if (ch != 0)
                {
                    if (bIsKeyDown)
                    {
                        sb.Append(ch);
                    }
                }
                else
                {
                    if (bIsKeyDown && sb.Length > 0)
                    {
                        lines.Add("\"" + sb.ToString() + "\"");
                        sb.Clear();
                    }

                    string vkStr = vk.ToString();
                    string vkSendKey = ConstVariables.Vk2string(vkStr);

                    if (bOutputVK)
                    {
                        if (bIsKeyDown)
                        {
                            sb.Append("Keys." + vkSendKey + "+ \"");
                        }
                        else
                        {
                            lines.Add(sb.ToString() + "\" + Keys." + vkSendKey);
                            sb.Clear();
                        }
                    }
                    else
                    {
                        if (bIsKeyDown)
                        {
                            lines.Add("Keys." + vkSendKey);
                        }
                    }
                }
            }

            if (sb.Length > 0)
            {
                lines.Add("\"" + sb.ToString() + "\"");
            }

            return lines;
        }

        public static string SendKeys(RecordedUiTask uiTask, int nOrder, int nOrderFocused)
        {
            List<string> lines = GetDecodedKeyboardInput(uiTask.GetBase64Text(), uiTask.GetCapsLock(), uiTask.GetNumLock(), uiTask.GetScrollLock());

            StringBuilder sb = new StringBuilder();
            foreach (string line in lines)
            {
                sb.AppendLine($"winElem{nOrderFocused}.SendKeys({line});");
            }

            return sb.ToString();
        }
    }
}

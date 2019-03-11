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
using System.Text;

namespace WinAppDriverUIRecorder
{
    public class RecordedUiTask
    {
        public static List<RecordedUiTask> s_listRecordedUi = new List<RecordedUiTask>();
        public static object s_lockRecordedUi = new object();

        private UiTreeNode _uiTreeNode;
        private string _strPath = null;
        private string _strDescription = null;
        private List<string> _pathNodes;

        public RecordedUiTask(List<string> pathNodes, EnumUiTaskName taskName)
        {
            UiTaskName = taskName;
            _pathNodes = pathNodes;
        }

        public RecordedUiTask(EnumUiTaskName taskName, string base64Text, bool bCapLock, bool scrollLock, bool numLock)
        {
            UiTaskName = taskName;
            Base64Text = base64Text;
            CapsLock = bCapLock;
            NumLock = numLock;
            ScrollLock = scrollLock;
        }

        public string GetXPath(bool bExcludeSessionRootPath)
        {
            if (string.IsNullOrEmpty(this._strPath))
            {
                this._strPath = GenerateXPath.GenerateXPathToUiElement(this, _pathNodes, ref _uiTreeNode).Trim();
            }

            if (string.IsNullOrEmpty(this._strPath))
            {
                return string.Empty;
            }

            string xPathRet = this._strPath;
            if (bExcludeSessionRootPath == true && string.IsNullOrEmpty(MainWindow.s_mainWin.RootSessionPath) == false)
            {
                int nPos = xPathRet.IndexOf(MainWindow.s_mainWin.RootSessionPath);
                if (nPos >= 0)
                {
                    xPathRet = "\"" + xPathRet.Substring(nPos + MainWindow.s_mainWin.RootSessionPath.Length);
                }
            }

            return xPathRet;
        }

        public string UpdateXPath(string xpath)
        {
            this._strPath = xpath;
            return this._strPath;
        }

        public UiTreeNode GetUiTreeNode()
        {
            return _uiTreeNode;
        }

        public string Base64Text { get; set; }

        public EnumUiTaskName UiTaskName { get; set; }

        public string Tag { get; set; }

        public string Name { get; set; }

        public string Left { get; set; }

        public string Top { get; set; }

        public string LeftLocal { get; set; }

        public string TopLocal { get; set; }

        public int DeltaX { get; set; }

        public int DeltaY { get; set; }

        public bool CapsLock { get; set; }

        public bool NumLock { get; set; }

        public bool ScrollLock { get; set; }

        public string VariableName
        {
            get
            {
                string shortName = "";
                if (string.IsNullOrEmpty(Name) == false)
                {
                    shortName = GenerateXPath.XmlDecode(Name);
                    const string namePattern = @"\w+";
                    var regNameValue = new System.Text.RegularExpressions.Regex(namePattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    System.Text.RegularExpressions.Match matchNameValue = regNameValue.Match(shortName);

                    if (matchNameValue.Success)
                    {
                        shortName = "";
                        while (matchNameValue.Success)
                        {
                            shortName += matchNameValue.Value.ToString();
                            matchNameValue = matchNameValue.NextMatch();
                        }
                    }
                    else
                    {
                        //Not expected to get here
                        shortName = GenerateXPath.XmlDecode(Name).Replace(" ", "").Trim();
                        string[] temp = shortName.Split("',.\"".ToCharArray());
                        if (temp != null && temp.Length > 0)
                        {
                            shortName = temp[0];
                        }
                    }

                    if (shortName.Length > 10)
                    {
                        shortName = shortName.Substring(0, 10);
                    }
                }

                return UiTaskName.ToString() + Tag + shortName + $"_{LeftLocal}_{TopLocal}";
            }
        }

        public string Description
        {
            get
            {
                try
                {
                    if (this._strDescription == null)
                    {
                        if (this.UiTaskName == EnumUiTaskName.KeyboardInput)
                        {
                            var keyboardTaskDescription = GenerateCSCode.GetDecodedKeyboardInput(this.Base64Text, this.CapsLock, this.NumLock, this.ScrollLock);
                            StringBuilder sb = new StringBuilder();
                            foreach (var strLine in keyboardTaskDescription)
                            {
                                sb.Append(strLine);
                            }

                            this._strDescription = $"{this.UiTaskName} VirtualKeys=\"{sb.ToString()}\" CapsLock={this.CapsLock} NumLock={this.NumLock} ScrollLock={this.ScrollLock}";
                        }
                        else
                        {
                            // set LeftLocal and TopLocal from _pathNodes.Last()
                            if (this.UiTaskName == EnumUiTaskName.Drag || this.UiTaskName == EnumUiTaskName.MouseWheel)
                            {
                                this._strDescription = $"{this.UiTaskName} on {Tag} \"{this.Name}\" at ({this.LeftLocal},{this.TopLocal}) drag ({this.DeltaX},{this.DeltaY})";
                            }
                            else
                            {
                                this._strDescription = $"{this.UiTaskName} on {Tag} \"{this.Name}\" at ({this.LeftLocal},{this.TopLocal})";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    AppInsights.LogException("Description", ex.Message);
                }

                if (string.IsNullOrEmpty(this._strDescription))
                {
                    this._strDescription = string.Empty;
                }
                return this._strDescription;
            }
            //set is not defined
        }

        public override string ToString()
        {
            return Description;
        }

        public void AppendKeyboardInput(string textToAppend)
        {
            byte[] data1 = Convert.FromBase64String(this.Base64Text);
            byte[] data2 = Convert.FromBase64String(textToAppend);
            byte[] data = new byte[data1.Length + data2.Length];
            data1.CopyTo(data, 0);
            data2.CopyTo(data, data1.Length);
            this.Base64Text = Convert.ToBase64String(data);

            var keyboardTaskDescription = GenerateCSCode.GetDecodedKeyboardInput(this.Base64Text, this.CapsLock, this.NumLock, this.ScrollLock);
            StringBuilder sb = new StringBuilder();
            foreach (var strLine in keyboardTaskDescription)
            {
                sb.Append(strLine);
            }

            this._strDescription = $"{this.UiTaskName} VirtualKeys=\"{sb.ToString()}\" CapsLock={this.CapsLock} NumLock={this.NumLock} ScrollLock={this.ScrollLock}";
        }

        public string GetCSCode(string focusedElemName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("// " + this.Description);

            string consoleWriteLine = "Console.WriteLine(\"" + this.Description.Replace("\"", "\\\"") + "\");";
            sb.AppendLine(consoleWriteLine);

            if (this.UiTaskName == EnumUiTaskName.LeftClick)
            {
                sb.AppendLine(GenerateCSCode.LeftClick(this, VariableName));
            }
            else if (this.UiTaskName == EnumUiTaskName.RightClick)
            {
                sb.AppendLine(GenerateCSCode.RightClick(this, VariableName));
            }
            else if (this.UiTaskName == EnumUiTaskName.LeftDblClick)
            {
                sb.AppendLine(GenerateCSCode.DoubleClick(this, VariableName));
            }
            else if (this.UiTaskName == EnumUiTaskName.MouseWheel)
            {
                sb.AppendLine(GenerateCSCode.Wheel(this, VariableName));
            }
            else if (this.UiTaskName == EnumUiTaskName.KeyboardInput)
            {
                sb.AppendLine(GenerateCSCode.SendKeys(this, focusedElemName));
            }

            return sb.ToString();
        }

        public void ChangeClickToDoubleClick()
        {
            this.UiTaskName = EnumUiTaskName.LeftDblClick;
            this._strDescription = null;
        }

        public void DragComplete(int deltaX, int deltaY)
        {
            this.DeltaX = deltaX;
            this.DeltaY = deltaY;
            this.UiTaskName = EnumUiTaskName.Drag;
            this._strDescription = null;
        }

        public void UpdateWheelData(int delta)
        {
            this.DeltaX += 1;
            this.DeltaY += delta;
            this._strDescription = null;
        }
    }

    class GenerateCSCode
    {
        public static string GetCodeBlock(RecordedUiTask uiTask, string elemName, string uiActionLine)
        {
            var xpath = "xpath_" + elemName;
            elemName = "winElem_" + elemName;

            string codeBlock = $"string {xpath} = {uiTask.GetXPath(true)};\n" +
                $"var {elemName} = desktopSession.FindElementByAbsoluteXPath({xpath});\n" +
                $"if ({elemName} != null)\n" +
                "{\n" +
                "CODEBLOCK" +
                "}\n" +
                "else\n" +
                "{\n" +
                "    Console.WriteLine($\"Failed to find element using xpath: {" + $"{xpath}" + "}\");\n" +
                "    return;\n" +
                "}\n";

            return codeBlock.Replace("CODEBLOCK", uiActionLine);
        }

        public static string LeftClick(RecordedUiTask uiTask, string elemName)
        {
            string codeLine = $"    winElem_{elemName}.Click();\n";
            return GetCodeBlock(uiTask, elemName, codeLine);
        }

        public static string DoubleClick(RecordedUiTask uiTask, string elemName)
        {
            string codeLine = $"    desktopSession.DesktopSessionElement.Mouse.MouseMove(winElem_{elemName}.Coordinates);\n" +
                            $"    desktopSession.DesktopSessionElement.Mouse.DoubleClick(null);\n";

            return GetCodeBlock(uiTask, elemName, codeLine);
        }

        public static string RightClick(RecordedUiTask uiTask, string elemName)
        {
            string codeLine = $"    desktopSession.DesktopSessionElement.Mouse.MouseMove(winElem_{elemName}.Coordinates);\n" +
                            $"    desktopSession.DesktopSessionElement.Mouse.ContextClick(null);\n";

            return GetCodeBlock(uiTask, elemName, codeLine);
        }

        public static string Wheel(RecordedUiTask uiTask, string elemName)
        {
            string codeLine = $"   //TODO: Wheel at ({uiTask.Left},{uiTask.Top}) on winElem_{elemName}, Count:{uiTask.DeltaX}, Total Amount:{uiTask.DeltaY}\n";
            return GetCodeBlock(uiTask, elemName, codeLine);
        }

        public static List<string> GetDecodedKeyboardInput(string strBase64, bool bCapsLock, bool bNumLock, bool bScrollLock)
        {
            byte[] data = Convert.FromBase64String(strBase64);

            int i = 0;
            bool shift = false;

            StringBuilder sb = new StringBuilder();
            List<string> lines = new List<string>();

            int nCtrlKeyDown = 0;

            while (i < data.Length / 2)
            {
                int n1 = i * 2;
                int n2 = i * 2 + 1;
                i++;

                bool bIsKeyDown = data[n1] == 0;
                VirtualKeys vk = (VirtualKeys)data[n2];

                char ch = ConstVariables.Vk2char((int)vk, shift || bCapsLock);

                if (bIsKeyDown) //Keydown
                {
                    if (char.IsControl(ch))
                    {
                        nCtrlKeyDown++;

                        if (nCtrlKeyDown == 1 && sb.Length > 0)
                        {
                            lines.Add("\"" + sb.ToString() + "\"");
                            sb.Clear();
                        }

                        string vkStr = vk.ToString();
                        string vkSendKey = ConstVariables.Vk2string(vkStr);
                        if (nCtrlKeyDown == 1)
                            sb.Append("Keys." + vkSendKey);
                        else
                            sb.Append(" + Keys." + vkSendKey);
                    }
                    else if (ch != 0)
                    {
                        string strToAppend = ch.ToString();
                        if (ch == '\\')
                        {
                            strToAppend += "\\";
                        }

                        if (nCtrlKeyDown > 0)
                        {
                            sb.Append(" + \"" + strToAppend + "\"");
                        }
                        else
                        {
                            sb.Append(strToAppend);
                        }
                    }
                }
                else //Keyup
                {
                    if (char.IsControl(ch))
                    {
                        nCtrlKeyDown--;

                        string vkStr = vk.ToString();
                        string vkSendKey = ConstVariables.Vk2string(vkStr);

                        if (nCtrlKeyDown == 0)
                        {
                            lines.Add(sb.ToString() + " + Keys." + vkSendKey);
                            sb.Clear();
                        }
                        else
                        {
                            sb.Append(" + Keys." + vkSendKey);
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

        public static string SendKeys(RecordedUiTask uiTask, string focusedElemeName)
        {
            List<string> lines = GetDecodedKeyboardInput(uiTask.Base64Text, uiTask.CapsLock, uiTask.NumLock, uiTask.ScrollLock);

            StringBuilder sb = new StringBuilder();

            focusedElemeName = "winElem_" + focusedElemeName;

            sb.AppendLine($"System.Threading.Thread.Sleep(100);");
            foreach (string line in lines)
            {
                sb.AppendLine($"{focusedElemeName}.SendKeys({line});");
            }

            return sb.ToString();
        }
    }
}

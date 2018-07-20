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
    class XmlNodePathRecorder
    {
        static object lockUiPath = new object();
        static string strUiToRootXmlNodes = null;
        static string strUiToRootXmlNodesInspect = null;
        static string strBase64KeyboardInput = null;
        static bool bCapsLock = false;
        static bool bNumLock = false;
        static bool bScrollLock = false;

        public static void SetBase64KeyboardInput(string strMsgText, bool bCapKeyOn, bool bNumKeyOn, bool bScrollKeyOn)
        {
            lock (lockUiPath)
            {
                strBase64KeyboardInput = strMsgText;
                bCapsLock = bCapKeyOn;
                bNumLock = bNumKeyOn;
                bScrollLock = bScrollKeyOn;
            }
        }

        public static void SetUiToRootXmlNodes(ref string strMsgText, bool bIsInspect = false)
        {
            lock (lockUiPath)
            {
                if (bIsInspect == false)
                {
                    strUiToRootXmlNodes = strMsgText;
                    strMsgText = null;
                }
                else
                {
                    strUiToRootXmlNodesInspect = strMsgText;
                }
            }
        }

        static bool GetRelativeLeftTop(string strNode, out string left, out string top)
        {
            left = null;
            top = null;

            if (string.IsNullOrEmpty(strNode))
            {
                return false;
            }

            string patXY = "x=\"(.+)\"\\sy=\"(.+)\"\\swidth";
            System.Text.RegularExpressions.Regex regXY = new System.Text.RegularExpressions.Regex(patXY, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (regXY != null)
            {
                System.Text.RegularExpressions.Match matchXY = regXY.Match(strNode);
                if (matchXY.Success && matchXY.Groups.Count > 2)
                {
                    left = matchXY.Groups[1].Value;
                    top = matchXY.Groups[2].Value;
                    return true;
                }
            }

            return false;
        }

        static List<string> GetRootToLeafNodes(string strLeafToRoot, out string left, out string top)
        {
            left = null;
            top = null; ;

            if (string.IsNullOrEmpty(strLeafToRoot))
            {
                return null;
            }

            string patLeftTop = "\\((\\d+)\\s(\\d+)\\)\\s";
            System.Text.RegularExpressions.Regex regLeftTop = new System.Text.RegularExpressions.Regex(patLeftTop, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (regLeftTop == null)
            {
                return null;
            }

            System.Text.RegularExpressions.Match matchLeftTop = regLeftTop.Match(strLeafToRoot);
            if (matchLeftTop.Success && matchLeftTop.Groups.Count > 2)
            {
                left = matchLeftTop.Groups[1].ToString();
                top = matchLeftTop.Groups[2].ToString();
                strLeafToRoot = strLeafToRoot.Remove(0, matchLeftTop.Value.ToString().Length);
            }
            else
            {
                return null;
            }

            List<string> listRet = new List<string>();
            string patNode = "<.+>";
            System.Text.RegularExpressions.Regex regNode = new System.Text.RegularExpressions.Regex(patNode, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (regNode != null && string.IsNullOrEmpty(strLeafToRoot) == false)
            {
                System.Text.RegularExpressions.Match matchNode = regNode.Match(strLeafToRoot);
                while (matchNode.Success)
                {
                    listRet.Add(matchNode.Value);

                    matchNode = matchNode.NextMatch();
                }
            }

            return listRet;
        }

        static string CreateUiTaskXmlNodeString(List<string> nodesRootToLeaf, string mouseAction, string left, string top)
        {
            if (nodesRootToLeaf == null || nodesRootToLeaf.Count == 0)
            {
                return null;
            }

            StringBuilder sb = new StringBuilder();
            string lastLine = nodesRootToLeaf.First();

            for (int i = nodesRootToLeaf.Count - 1; i >= 0; i--)
            {
                sb.AppendLine(nodesRootToLeaf[i]);
            }

            string lastLeft, lastTop;
            GetRelativeLeftTop(lastLine, out lastLeft, out lastTop);

            int nLeft = Convert.ToInt32(left) - Convert.ToInt32(lastLeft);
            int nTop = Convert.ToInt32(top) - Convert.ToInt32(lastTop);

            if (mouseAction == UiTaskName.Drag.ToString())
            {
                sb.Insert(0, string.Format("<UiTask task=\"{0}\" x=\"{1}\" y=\"{2}\" deltaX=\"{3}\" deltaY=\"{4}\">\n", mouseAction, nLeft, nTop, ConstVariables.DELTAX, ConstVariables.DELTAY));
            }
            else if (mouseAction == UiTaskName.MouseWheel.ToString())
            {
                sb.Insert(0, string.Format("<UiTask task=\"{0}\" x=\"{1}\" y=\"{2}\" wheel=\"{3}\">\n", mouseAction, nLeft, nTop, ConstVariables.WHEEL));
            }
            else
            {
                sb.Insert(0, string.Format("<UiTask task=\"{0}\" x=\"{1}\" y=\"{2}\">\n", mouseAction, nLeft, nTop));
            }

            sb.AppendLine("</UiTask>");
            sb.AppendLine();

            return sb.ToString();
        }

        public static IntPtr ProcessMessage(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            handled = false;
            string left, top;

            if (msg == (int)UiTaskName.Inspect)
            {
                var nodesRootToLeaf = GetRootToLeafNodes(strUiToRootXmlNodesInspect, out left, out top);
                if (nodesRootToLeaf != null)
                {
                    string strXmlNodes = CreateUiTaskXmlNodeString(nodesRootToLeaf, ((UiTaskName)msg).ToString(), left, top);
                    if (string.IsNullOrEmpty(strXmlNodes) == false)
                    {
                        MainWindow.AddRecordedUi(new RecordedUiTask(strXmlNodes, 0, (UiTaskName)msg));
                    }
                }
                return IntPtr.Zero;
            }

            if (MainWindow.IsRecording() == false)
            {
                return IntPtr.Zero;
            }

            lock (lockUiPath)
            {
                if (msg == (int)UiTaskName.KeyboardInput)
                {
                    var keyboardTaskDescription = GenerateCSCode.GetDecodedKeyboardInput(strBase64KeyboardInput, bCapsLock, bNumLock, bScrollLock);
                    StringBuilder sb = new StringBuilder();
                    foreach (var strLine in keyboardTaskDescription)
                    {
                        sb.Append(GenerateXPath.XmlEncode(strLine));
                    }

                    var keyboarTask = new RecordedUiTask($"<UiTask task=\"{UiTaskName.KeyboardInput}\" VirtualKeys=\"{sb.ToString()}\" base64String=\"{strBase64KeyboardInput}\" CapsLock=\"{bCapsLock}\" NumLock=\"{bNumLock}\" ScrollLock=\"{bScrollLock}\"/>\n", (int)wParam, (UiTaskName)msg);
                    MainWindow.AddRecordedUi(keyboarTask);
                }
                else if ((int)UiTaskName.LeftClick <= msg && msg <= (int)UiTaskName.MouseHover)
                {
                    RecordedUiTask lastRecordedUi = null;
                    if (MainWindow.s_listRecordedUi.Count > 0)
                    {
                        lastRecordedUi = MainWindow.s_listRecordedUi.Last();
                    }

                    string strXmlNodes = null;
                    List<string> nodesRootToLeaf = GetRootToLeafNodes(strUiToRootXmlNodes, out left, out top);
                    if (nodesRootToLeaf != null && nodesRootToLeaf.Count > 1)
                    {
                        strXmlNodes = CreateUiTaskXmlNodeString(nodesRootToLeaf, ((UiTaskName)msg).ToString(), left, top);
                    }

                    bool bAddNewTask = true;

                    // Completing last UI
                    if (msg == (int)UiTaskName.LeftDblClick && lastRecordedUi != null)
                    {
                        lastRecordedUi.ChangeClickToDoubleClick();
                        bAddNewTask = false;
                    }
                    else if (msg == (int)UiTaskName.DragStop && lastRecordedUi != null)
                    {
                        int deltaX = wParam.ToInt32() - ConstVariables.DragDeltaOffset;
                        int deltaY = lParam.ToInt32() - ConstVariables.DragDeltaOffset;
                        lastRecordedUi.DragComplete(deltaX, deltaY);
                        bAddNewTask = false;
                    }
                    else if (msg == (int)UiTaskName.MouseWheel)
                    {
                        int delta = wParam.ToInt32() - ConstVariables.DragDeltaOffset;

                        if (strXmlNodes != null)
                        {
                            if (lastRecordedUi == null || lastRecordedUi.GetTask() != UiTaskName.MouseWheel)
                            {
                                lastRecordedUi = new RecordedUiTask(strXmlNodes, 0, (UiTaskName)msg);
                                MainWindow.AddRecordedUi(lastRecordedUi);
                            }
                        }

                        if (lastRecordedUi != null && lastRecordedUi.GetTask() == UiTaskName.MouseWheel)
                        {
                            lastRecordedUi.UpdateWheelData(delta);
                        }

                        bAddNewTask = false;
                    }

                    // Adding newly recorded UI
                    if (bAddNewTask)
                    {
                        MainWindow.AddRecordedUi(new RecordedUiTask(strXmlNodes, 0, (UiTaskName)msg));
                    }
                    else
                    {
                        MainWindow.UpdateLastUi(lastRecordedUi);
                    }
                }

                return IntPtr.Zero;
            }
        }
    }
}

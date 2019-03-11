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

using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinAppDriverUIRecorder
{
    class XmlNodePathRecorder
    {
        static List<string> GetRootToLeafNodes(string strLeafToRoot)
        {
            if (string.IsNullOrEmpty(strLeafToRoot))
            {
                AppInsights.LogException("GetRootToLeafNodes", "strLeafToRoot is null");
                return null;
            }

            List<string> listRet = new List<string>();
            string patNode = "/[^\n]+\n";
            System.Text.RegularExpressions.Regex regNode = new System.Text.RegularExpressions.Regex(patNode, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (regNode != null && string.IsNullOrEmpty(strLeafToRoot) == false)
            {
                System.Text.RegularExpressions.Match matchNode = regNode.Match(strLeafToRoot);
                while (matchNode.Success)
                {
                    listRet.Insert(0, matchNode.Value.Substring(0, matchNode.Value.Length - 1));
                    matchNode = matchNode.NextMatch();
                }
            }

            return listRet;
        }

        public static void AddKeyboardInputTask(ref string strBase64, bool bCapsLock, bool bNumLock, bool bScrollLock)
        {
            if (string.IsNullOrEmpty(strBase64))
            {
                AppInsights.LogException("AddKeyboardInputTask", "strBase64 is null");
                return;
            }

            var keyboardTaskDescription = GenerateCSCode.GetDecodedKeyboardInput(strBase64, bCapsLock, bNumLock, bScrollLock);

            StringBuilder sb = new StringBuilder();
            foreach (var strLine in keyboardTaskDescription)
            {
                sb.Append(GenerateXPath.XmlEncode(strLine));
            }

            RecordedUiTask lastRecordedUi = null;
            lock (RecordedUiTask.s_lockRecordedUi)
            {
                if (RecordedUiTask.s_listRecordedUi.Count > 0)
                {
                    lastRecordedUi = RecordedUiTask.s_listRecordedUi.Last();
                }
            }

            if (lastRecordedUi != null && lastRecordedUi.UiTaskName == EnumUiTaskName.KeyboardInput)
            {
                lastRecordedUi.AppendKeyboardInput(strBase64);
            }
            else
            {
                var keyboarTask = new RecordedUiTask(EnumUiTaskName.KeyboardInput, strBase64, bCapsLock, bScrollLock, bNumLock);
                MainWindow.AddRecordedUi(keyboarTask);
            }

            strBase64 = null;
        }

        public static void HandleUiEvent(ref string strXml, EnumUiTaskName uiTaskName, int deltaX, int deltaY)
        {
            List<string> nodesRootToLeaf = null;

            if (uiTaskName == EnumUiTaskName.Inspect)
            {
                nodesRootToLeaf = GetRootToLeafNodes(strXml);
                if (nodesRootToLeaf != null && nodesRootToLeaf.Count > 0)
                {
                    MainWindow.AddInspectUi(new RecordedUiTask(nodesRootToLeaf, uiTaskName));
                }
                return;
            }

            if (uiTaskName != EnumUiTaskName.KeyboardInput)
            {
                nodesRootToLeaf = GetRootToLeafNodes(strXml);
                strXml = null;
            }

            RecordedUiTask lastRecordedUi = null;
            lock (RecordedUiTask.s_lockRecordedUi)
            {
                if (RecordedUiTask.s_listRecordedUi.Count > 0)
                {
                    lastRecordedUi = RecordedUiTask.s_listRecordedUi.Last();
                }
            }

            bool bAddNewTask = true;

            // Completing last UI
            if (uiTaskName == EnumUiTaskName.LeftDblClick && lastRecordedUi != null)
            {
                lastRecordedUi.ChangeClickToDoubleClick();
                bAddNewTask = false;
            }
            else if (uiTaskName == EnumUiTaskName.MouseWheel)
            {
                if (lastRecordedUi == null || lastRecordedUi.UiTaskName != EnumUiTaskName.MouseWheel)
                {
                    if (nodesRootToLeaf != null && nodesRootToLeaf.Count > 0)
                    {
                        lastRecordedUi = new RecordedUiTask(nodesRootToLeaf, uiTaskName);
                        MainWindow.AddRecordedUi(lastRecordedUi);
                    }
                }

                if (lastRecordedUi != null && lastRecordedUi.UiTaskName == EnumUiTaskName.MouseWheel)
                {
                    lastRecordedUi.UpdateWheelData(deltaX);
                }

                bAddNewTask = false;
            }

            if (bAddNewTask)
            {
                if (nodesRootToLeaf != null && nodesRootToLeaf.Count > 0)
                {
                    MainWindow.AddRecordedUi(new RecordedUiTask(nodesRootToLeaf, uiTaskName));
                }
            }
            else if (lastRecordedUi != null)
            {
                //MouseWheel, DoubleClick
                MainWindow.UpdateLastUi(lastRecordedUi);
            }

            NativeMethods.PostMessage(MainWindow.s_windowHandle, (int)MainWindow.UiThreadTask.ActionAdded, 0, 0);

            AppInsights.LogEvent("HandleUiEvent", uiTaskName.ToString());
        }
    }
}
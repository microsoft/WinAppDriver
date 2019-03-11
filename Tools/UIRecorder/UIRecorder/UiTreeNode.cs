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
using System.Collections.ObjectModel;
using System.Linq;

namespace WinAppDriverUIRecorder
{
    public class PropNameValue
    {
        public PropNameValue(string name, string value)
        {
            PropName = name;
            PropValue = value;
        }

        public string PropName { set; get; }
        public string PropValue { set; get; }
    }

    public class UiTreeNode
    {
        public static List<UiTreeNode> s_uiTreeNodes = new List<UiTreeNode>();

        public enum CompareMethod { Equal, StartsWith, Contains };

        public UiTreeNode(UiTreeNode pNode)
        {
            Parent = pNode;
            this.Items = new List<UiTreeNode>();
        }

        public UiTreeNode Parent { get; set; }
        public string Tag { get; set; }
        public string Title { get; set; }
        public string ClassName { get; set; }
        public string Name { get; set; }
        public string AutomationId { get; set; }
        public string Position { get; set; }
        public string Left { get; set; }
        public string Top { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string RuntimeId { get; set; }
        public RecordedUiTask UiTask { get; set; }
        public List<UiTreeNode> Items { get; set; }
        public CompareMethod NameCompareMethod { get; set; }
        public CompareMethod ClassNameCompareMethod { get; set; }
        public CompareMethod AutomationIdCompareMethod { get; set; }

        public string NodePath { get; set; }

        public ObservableCollection<PropNameValue> LoadPropertyData()
        {
            var listPropValue = new ObservableCollection<PropNameValue>();
            listPropValue.Add(new PropNameValue("Tag", Tag));
            listPropValue.Add(new PropNameValue("ClassName", ClassName));
            listPropValue.Add(new PropNameValue("Name", Name));
            listPropValue.Add(new PropNameValue("AutomationId", AutomationId));
            listPropValue.Add(new PropNameValue("Position", (Position ?? "").ToString()));
            listPropValue.Add(new PropNameValue("Left", (Left ?? "").ToString()));
            listPropValue.Add(new PropNameValue("Top", (Top ?? "").ToString()));
            listPropValue.Add(new PropNameValue("Width", (Width ?? "").ToString()));
            listPropValue.Add(new PropNameValue("Height", (Height ?? "").ToString()));
            listPropValue.Add(new PropNameValue("RuntimeId", RuntimeId));
            listPropValue.Add(new PropNameValue("UiTask", UiTask != null ? UiTask.ToString() : ""));
            return listPropValue;
        }

        public static bool CompareRuntimeId(UiTreeNode nodeLeft, UiTreeNode nodeRight)
        {
            if (nodeLeft == null || nodeRight == null)
            {
                return false;
            }
            else
            {
                return nodeLeft.RuntimeId == nodeRight.RuntimeId;
            }
        }

        static UiTreeNode FindInCachedUiTree(UiTreeNode node, List<UiTreeNode> cachedNode)
        {
            foreach (var snode in cachedNode)
            {
                if (CompareRuntimeId(node, snode))
                {
                    return snode;
                }
            }

            return null;
        }

        public static void AddToUiTree(RecordedUiTask recordedUi)
        {
            if (recordedUi.GetUiTreeNode() == null)
            {
                if (string.IsNullOrEmpty(recordedUi.GetXPath(false)))
                {
                    return;
                }
            }

            var uiNode = recordedUi.GetUiTreeNode();

            if (uiNode == null || uiNode.Items.Count == 0)
            {
                //Log error
                return;
            }

            // Empty root
            if (s_uiTreeNodes.Count == 0)
            {
                s_uiTreeNodes.Add(uiNode);
                return;
            }

            // Add uiNode to s_uiNode
            UiTreeNode node = uiNode;
            UiTreeNode nodeCached = null;
            List<UiTreeNode> listCached = s_uiTreeNodes;

            while (node != null)
            {
                var nodeAddTo = FindInCachedUiTree(node, listCached);

                if (node.Items.Count > 0 && nodeAddTo != null)
                {
                    node = node.Items.First();
                    listCached = nodeAddTo.Items;
                    nodeCached = nodeAddTo;
                }
                else
                {
                    if (nodeCached != null)
                    {
                        node.Parent = nodeCached;
                        nodeCached.Items.Add(node);
                    }
                    break;
                }
            }
        }

        static UiTreeNode FindUiTaskNode(UiTreeNode node, RecordedUiTask uiTaskNode)
        {
            if (node.UiTask == uiTaskNode)
            {
                return node;
            }

            for (int i = 0; i < node.Items.Count; i++)
            {
                var ret = FindUiTaskNode(node.Items[i], uiTaskNode);
                if (ret != null)
                {
                    return ret;
                }
            }

            return null;
        }

        public static void RemoveUiTreeNode(RecordedUiTask uiTaskNode)
        {
            if (s_uiTreeNodes.Count == 0)
            {
                return;
            }

            UiTreeNode uiNode = FindUiTaskNode(s_uiTreeNodes.First(), uiTaskNode);
            if (uiNode == null)
            {
                s_uiTreeNodes.Clear();
                return;
            }

            UiTreeNode uiParent = uiNode.Parent;
            if (uiParent == null)
            {
                s_uiTreeNodes.Clear();
                return;
            }

            while (uiParent != null)
            {
                uiNode.UiTask = null;

                if (uiNode.Items.Count == 0)
                {
                    uiParent.Items.Remove(uiNode);
                }

                if (uiParent.Items.Count > 0)
                {
                    break;
                }
                else
                {
                    uiNode = uiParent;
                    uiParent = uiNode.Parent;
                }
            }

            if (uiParent == null && (s_uiTreeNodes.Count == 1 && s_uiTreeNodes.First().Items.Count == 0))
            {
                s_uiTreeNodes.Clear();
            }
        }
    }
}

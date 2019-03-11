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
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace WinAppDriverUIRecorder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow s_mainWin = null;
        public static IntPtr s_windowHandle;
        RecordedUiTask selectedTreeViewItem;
        static RecordedUiTask s_inspectUi;
        bool bPauseMouseKeyboardCurrent = false;
        System.Timers.Timer timer = null;

        public enum UiThreadTask { UnknownUiThreadTask = 3000, RemoveInspectNode, UpdateTreeView, Active, XPathReady, ActionAdded, PauseRecording, Init };
        public enum UiSelectChangedSource { ComboBoxSelected, TreeViewItemSelected, SelectChangeFinished };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            s_windowHandle = new System.Windows.Interop.WindowInteropHelper(Application.Current.MainWindow).Handle;
            System.Windows.Interop.HwndSource.FromHwnd(s_windowHandle).AddHook(RunOnUiThread);

            s_mainWin = this;
            this.comboBoxRecordedUi.ItemsSource = RecordedUiTask.s_listRecordedUi;
            this.treeUiPath.ItemsSource = UiTreeNode.s_uiTreeNodes;

            ConstVariables.InitVk2StringMap();

            AppInsights.LogEvent("Window_Loaded");
        }

        public static IntPtr RunOnUiThread(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            handled = false;

            if (s_mainWin.IsRecording == true)
            {
                if (msg == (int)UiThreadTask.Active)
                {
                    s_mainWin.gridTextRecording.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0x40, 0x40));
                    s_mainWin.textBoxRecording.Text = "Active";
                    s_mainWin.gridTextRecording.UpdateLayout();
                }
                else if (msg == (int)UiThreadTask.XPathReady)
                {
                    s_mainWin.gridTextRecording.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x40, 0x40, 0xff));
                    s_mainWin.textBoxRecording.Text = "XPath Ready";
                    s_mainWin.gridTextRecording.UpdateLayout();
                }
                else if (msg == (int)UiThreadTask.ActionAdded)
                {
                    s_mainWin.gridTextRecording.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x40, 0xff, 0x40));
                    s_mainWin.textBoxRecording.Text = "UI Action Added";
                    s_mainWin.gridTextRecording.UpdateLayout();
                }
                else if (msg == (int)UiThreadTask.PauseRecording)
                {
                    s_mainWin.gridTextRecording.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x80, 0x80, 0x80));
                    s_mainWin.gridTextRecording.UpdateLayout();
                }
            }

            if (msg == (int)UiThreadTask.RemoveInspectNode)
            {
                s_mainWin.DeleteRecordedUiTask(s_inspectUi);
            }
            else if (msg == (int)UiThreadTask.UpdateTreeView)
            {
                if (s_mainWin.SelectChangedSource == UiSelectChangedSource.SelectChangeFinished)
                {
                    lock (RecordedUiTask.s_lockRecordedUi)
                    {
                        if (RecordedUiTask.s_listRecordedUi.Count > 0)
                        {
                            // reset ItemsSource to null and s_listRecordedUi will force combobox value to update
                            s_mainWin.comboBoxRecordedUi.ItemsSource = null;
                            s_mainWin.comboBoxRecordedUi.ItemsSource = RecordedUiTask.s_listRecordedUi;
                            s_mainWin.comboBoxRecordedUi.SelectedIndex = RecordedUiTask.s_listRecordedUi.Count - 1;
                            s_mainWin.comboBoxRecordedUi.Items.Refresh();

                            s_mainWin.treeUiPath.Items.Refresh();
                        }
                    }
                }
            }

            return IntPtr.Zero;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            MouseKeyboardEventHandler.UnInit();
            MouseKeyboardHook.StopHook();
            s_mainWin = null;

            System.Windows.Interop.HwndSource.FromHwnd(s_windowHandle).RemoveHook(RunOnUiThread);

            AppInsights.LogEvent("Window_Closed");
        }

        public string RootSessionPath { get; set; }

        public bool IsRecording { get; set; }

        public UiSelectChangedSource SelectChangedSource { get; set; }

        private void btnRecord_Click(object sender, RoutedEventArgs e)
        {
            SelectChangedSource = UiSelectChangedSource.SelectChangeFinished;

            if (IsRecording == false)
            {
                MouseKeyboardEventHandler.Init();

                btnRecord.Content = "Pause";
                textBoxRecording.Text = "Recording";
                gridTextRecording.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0, 0));

                IsRecording = true;
                AppInsights.LogEvent("Record");
            }
            else
            {
                IsRecording = false;

                MouseKeyboardEventHandler.UnInit();

                btnRecord.Content = "Record";
                textBoxRecording.Text = "";
                gridTextRecording.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xFF, 0xFF));

                AddInspectUi(null); // remove last Inspect node
                AppInsights.LogEvent("Pause");
            }
        }

        public static void UpdateLastUi(RecordedUiTask uiTask)
        {
            NativeMethods.PostMessage(MainWindow.s_windowHandle, (int)UiThreadTask.UpdateTreeView, 0, 0);
        }

        public static void AddInspectUi(RecordedUiTask uiTask)
        {
            s_inspectUi = null;
            lock (RecordedUiTask.s_lockRecordedUi)
            {
                foreach (var recordedUi in RecordedUiTask.s_listRecordedUi)
                {
                    if (recordedUi.UiTaskName == EnumUiTaskName.Inspect)
                    {
                        s_inspectUi = recordedUi;
                        break;
                    }
                }

                if (uiTask != null)
                {
                    RecordedUiTask.s_listRecordedUi.Add(uiTask);

                    if (uiTask.UiTaskName != EnumUiTaskName.KeyboardInput)
                    {
                        UiTreeNode.AddToUiTree(uiTask);
                    }
                }
            }

            NativeMethods.PostMessage(MainWindow.s_windowHandle, (int)UiThreadTask.RemoveInspectNode, 0, 0);
            NativeMethods.PostMessage(MainWindow.s_windowHandle, (int)UiThreadTask.UpdateTreeView, 0, 0);
            if (uiTask != null)
            {
                NativeMethods.PostMessage(MainWindow.s_windowHandle, (int)UiThreadTask.XPathReady, 0, 0);
            }
        }

        public static void AddRecordedUi(RecordedUiTask uiTask)
        {
            lock (RecordedUiTask.s_lockRecordedUi)
            {
                RecordedUiTask.s_listRecordedUi.Add(uiTask);

                if (uiTask.UiTaskName != EnumUiTaskName.KeyboardInput)
                {
                    UiTreeNode.AddToUiTree(uiTask);
                }
            }

            NativeMethods.PostMessage(MainWindow.s_windowHandle, (int)UiThreadTask.UpdateTreeView, 0, 0);
        }

        private void comboBoxRecordedUi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectChangedSource == UiSelectChangedSource.ComboBoxSelected)
            {
                return;
            }

            var selectedItem = (comboBoxRecordedUi.SelectedItem as RecordedUiTask);
            if (selectedItem == null)
            {
                return;
            }

            if (SelectChangedSource != UiSelectChangedSource.TreeViewItemSelected)
                SelectChangedSource = UiSelectChangedSource.ComboBoxSelected;

            // C# code 
            var focusedElementName = "PreviousLeftClickElement";
            if (selectedItem.UiTaskName == EnumUiTaskName.KeyboardInput)
            {
                lock (RecordedUiTask.s_lockRecordedUi)
                {
                    foreach (var uiTask in RecordedUiTask.s_listRecordedUi)
                    {
                        if (uiTask.UiTaskName == EnumUiTaskName.LeftClick)
                        {
                            focusedElementName = uiTask.VariableName;
                        }

                        if (uiTask == selectedItem)
                        {
                            break;
                        }
                    }
                }
            }

            textBoxCode.Text = selectedItem.GetCSCode(focusedElementName);

            if (selectedItem.UiTaskName != EnumUiTaskName.KeyboardInput)
            {
                // xpath
                TextRange tr = new TextRange(rtbXPath.Document.ContentStart, rtbXPath.Document.ContentEnd);
                tr.Text = selectedItem.GetXPath(false);

                // TreeView
                HighlightPath(0, true/*highlight session path*/);

                if (treeUiPath.Items != null && treeUiPath.Items.Count > 0)
                {
                    var tvi = (TreeViewItem)treeUiPath.ItemContainerGenerator.ContainerFromItem(treeUiPath.Items[0]);
                    if (tvi != null)
                    {
                        tvi = ExpandSelectedTreeNode(UiTreeNode.s_uiTreeNodes.First(), tvi, selectedItem);
                        if (tvi != null)
                        {
                            // select to highlight leaf tree view node
                            tvi.IsSelected = true;
                            tvi.BringIntoView();
                        }
                    }
                }
            }

            if (SelectChangedSource != UiSelectChangedSource.TreeViewItemSelected)
                SelectChangedSource = UiSelectChangedSource.SelectChangeFinished;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            lock (RecordedUiTask.s_lockRecordedUi)
            {
                RecordedUiTask.s_listRecordedUi.Clear();
                UiTreeNode.s_uiTreeNodes.Clear();
            }

            this.comboBoxRecordedUi.Items.Refresh();
            treeUiPath.Items.Refresh();

            TextRange tr = new TextRange(rtbXPath.Document.ContentStart, rtbXPath.Document.ContentEnd);
            tr.Text = "";
            textBoxCode.Text = "";
            RootSessionPath = null;

            AppInsights.LogEvent("btnClear_Click");
        }

        private void btnWinAppDriverCode_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            lock (RecordedUiTask.s_lockRecordedUi)
            {
                if (RecordedUiTask.s_listRecordedUi.Count == 0)
                {
                    return;
                }

                // Stop recording
                if (btnRecord.IsChecked == true)
                {
                    btnRecord_Click(null, null);
                }

                string focusedLeftElementName = "";
                foreach (var uiTask in RecordedUiTask.s_listRecordedUi)
                {
                    if (uiTask.UiTaskName != EnumUiTaskName.Inspect)
                    {
                        if (uiTask.UiTaskName == EnumUiTaskName.LeftClick)
                        {
                            focusedLeftElementName = uiTask.VariableName;
                        }

                        sb.AppendLine(uiTask.GetCSCode(focusedLeftElementName));
                    }
                }
            }

            if (sb.Length > 0)
            {
                Clipboard.SetText(sb.ToString());

                this.Dispatcher.Invoke(new Action(() =>
                {
                    this.toolTipText.Text = "WinAppDriver client code copied to clipboard";
                    ((ToolTip)btnWinAppDriverCode.ToolTip).IsOpen = true;

                    timer = new System.Timers.Timer(2000) { Enabled = true, AutoReset = false };
                    timer.Elapsed += ResetToolTip;
                    timer.Start();
                }), System.Windows.Threading.DispatcherPriority.ContextIdle);
            }

            AppInsights.LogEvent("Recorded UI Count", $"{RecordedUiTask.s_listRecordedUi.Count}");
        }

        private void ResetToolTip(Object source, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();
            timer.Elapsed -= ResetToolTip;
            timer = null;

            this.Dispatcher.Invoke(new Action(() =>
            {
                ((ToolTip)btnWinAppDriverCode.ToolTip).IsOpen = false;
                toolTipText.Text = "Generate and copy C# code to Clipboard";
            }), System.Windows.Threading.DispatcherPriority.ContextIdle);
        }

        private void MenuCopyFullPath(object sender, RoutedEventArgs e)
        {
            var tr = new TextRange(rtbXPath.Document.ContentStart, rtbXPath.Document.ContentEnd);
            if (tr != null)
            {
                Clipboard.SetText(tr.Text);
            }

            AppInsights.LogEvent("Copy full xpath");
        }

        private void MenuCopyPathWithoutSessionPath(object sender, RoutedEventArgs e)
        {
            var tr = new TextRange(rtbXPath.Document.ContentStart, rtbXPath.Document.ContentEnd);
            if (tr != null && RootSessionPath != null)
            {
                int nPos = tr.Text.IndexOf(RootSessionPath);
                if (nPos > -1)
                {
                    string xpath = "\"" + tr.Text.Substring(nPos + RootSessionPath.Length);
                    Clipboard.SetText(xpath);
                }
                else
                {
                    Clipboard.SetText(tr.Text);
                }
            }

            AppInsights.LogEvent("Copy relative xpath");
        }

        private void MenuClearSessionPath(object sender, RoutedEventArgs e)
        {
            RootSessionPath = null;
            HighlightPath(0, true);
        }

        private void DeleteRecordedUiTask(RecordedUiTask uiTask)
        {
            if (uiTask == null)
                return;

            lock (RecordedUiTask.s_lockRecordedUi)
            {
                if (uiTask.UiTaskName != EnumUiTaskName.KeyboardInput)
                {
                    UiTreeNode.RemoveUiTreeNode(uiTask);
                }

                RecordedUiTask.s_listRecordedUi.Remove(uiTask);

                if (RecordedUiTask.s_listRecordedUi.Count > 0)
                {
                    comboBoxRecordedUi.SelectedIndex = 0;
                    comboBoxRecordedUi.SelectedValue = RecordedUiTask.s_listRecordedUi[comboBoxRecordedUi.SelectedIndex];
                }
                else
                {
                    TextRange tr = new TextRange(rtbXPath.Document.ContentStart, rtbXPath.Document.ContentEnd);
                    tr.Text = "";
                    textBoxCode.Text = "";
                }

                comboBoxRecordedUi.Items.Refresh();
                treeUiPath.Items.Refresh();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxRecordedUi.Items == null
                || comboBoxRecordedUi.Items.Count == 0
                || comboBoxRecordedUi.SelectedItem == null)
            {
                return;
            }

            var uiTask = (RecordedUiTask)comboBoxRecordedUi.SelectedItem;
            if (uiTask == null)
            {
                return;
            }

            DeleteRecordedUiTask(uiTask);

            AppInsights.LogEvent("btnDelete_Click");
        }

        private string RemoveFirstNode(string xpath)
        {
            bool bTagStart = false;
            int ncount = 0;
            int nBracketCount = 0;

            foreach (char c in xpath)
            {
                if (c == '[')
                {
                    nBracketCount++;
                }
                else if (c == ']')
                {
                    nBracketCount--;
                }
                else if (c == '/' && nBracketCount == 0)
                {
                    if (bTagStart == false)
                    {
                        bTagStart = true;
                    }
                    else
                    {
                        break;
                    }
                }

                ncount++;
            }

            return xpath.Substring(ncount);
        }

        private string HighlightPath(int nNodeCount, bool bHighlight)
        {
            var tr = new TextRange(rtbXPath.Document.ContentStart, rtbXPath.Document.ContentEnd);

            string pathText = tr.Text;
            if (string.IsNullOrEmpty(pathText))
            {
                return null;
            }

            string xpath = pathText;
            int nIndex1 = 0;
            int nIndex2 = 0;

            for (int node = 0; node < nNodeCount; node++)
            {
                int Len1 = xpath.Length;
                xpath = RemoveFirstNode(xpath);

                nIndex1 = nIndex2 + 1;
                nIndex2 += (Len1 - xpath.Length);
            }

            if (nIndex2 < nIndex1)
            {
                return null;
            }

            if (bHighlight == true)
            {
                tr.ClearAllProperties();

                if (RootSessionPath != null && pathText.Contains(RootSessionPath) == true)
                {
                    var posRoot1 = rtbXPath.Document.ContentStart.GetPositionAtOffset(1, LogicalDirection.Forward);
                    var posRoot2 = rtbXPath.Document.ContentStart.GetPositionAtOffset(RootSessionPath.Length + 2, LogicalDirection.Forward);
                    var trRoot = new TextRange(posRoot1, posRoot2);
                    trRoot.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.LightGray);
                }

                if (nNodeCount > 0)
                {
                    var pos1 = rtbXPath.Document.ContentStart.GetPositionAtOffset(nIndex1 + 1, LogicalDirection.Forward);
                    var pos2 = rtbXPath.Document.ContentStart.GetPositionAtOffset(nIndex2 + 2, LogicalDirection.Forward);
                    tr = new TextRange(pos1, pos2);
                    tr.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.LightBlue);
                }
            }

            return pathText.Substring(nIndex1, nIndex2 - nIndex1); //excluding leading quote
        }

        private RecordedUiTask GetExpandedLeafNode(TreeViewItem tvi, UiTreeNode node, RecordedUiTask recordedUiTask)
        {
            if (tvi == null || node == null)
            {
                return null;
            }

            if (recordedUiTask != null)
            {
                if (node.UiTask == recordedUiTask)
                {
                    return node.UiTask;
                }
            }
            else if (node.Items.Count == 0)
            {
                return node.UiTask;
            }

            foreach (var c in node.Items)
            {
                var subContainer = (TreeViewItem)tvi.ItemContainerGenerator.ContainerFromItem(c);
                if (subContainer != null)
                {
                    if (subContainer.IsExpanded == true)
                    {
                        var retTemp = GetExpandedLeafNode(subContainer, c, recordedUiTask);
                        if (retTemp != null)
                        {
                            return retTemp;
                        }
                    }
                }
            }

            return null;
        }

        private TreeViewItem UiTreeNode2TreeViewItem(TreeViewItem tvi, UiTreeNode node, UiTreeNode nodeTarget)
        {
            if (tvi == null || node == null)
            {
                return null;
            }

            if (node == nodeTarget)
            {
                return tvi;
            }

            foreach (var c in node.Items)
            {
                var subContainer = (TreeViewItem)tvi.ItemContainerGenerator.ContainerFromItem(c);
                var ret = UiTreeNode2TreeViewItem(subContainer, c, nodeTarget);
                if (ret != null)
                {
                    return ret;
                }
            }

            return null;
        }

        private void treeUiPath_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (SelectChangedSource == UiSelectChangedSource.TreeViewItemSelected)
            {
                return;
            }

            var selectedItem = treeUiPath.SelectedItem as UiTreeNode;
            if (selectedItem == null)
            {
                return;
            }

            if (SelectChangedSource != UiSelectChangedSource.ComboBoxSelected)
                SelectChangedSource = UiSelectChangedSource.TreeViewItemSelected;

            RecordedUiTask leafUiTask = selectedItem.UiTask;

            if (leafUiTask == null)
            {
                if (treeUiPath.Items != null && treeUiPath.Items.Count > 0)
                {
                    var x = (TreeViewItem)treeUiPath.ItemContainerGenerator.ContainerFromItem(treeUiPath.Items[0]);
                    if (x != null)
                    {
                        TreeViewItem tvi = UiTreeNode2TreeViewItem(x, UiTreeNode.s_uiTreeNodes.First(), selectedItem);
                        if (tvi != null)
                        {
                            // sync xpath with last selected tree view leaf if a parent node is selected 
                            // and the selected parent has more than 1 expanded leaf children
                            leafUiTask = GetExpandedLeafNode(tvi, selectedItem, selectedTreeViewItem);
                            {
                                leafUiTask = GetExpandedLeafNode(tvi, selectedItem, null);
                                selectedTreeViewItem = leafUiTask;
                            }
                        }
                    }
                }
            }

            if (leafUiTask == null)
            {
                if (SelectChangedSource != UiSelectChangedSource.ComboBoxSelected)
                    SelectChangedSource = UiSelectChangedSource.SelectChangeFinished;

                return;
            }
            else
            {
                selectedTreeViewItem = leafUiTask;
            }

            if (leafUiTask != comboBoxRecordedUi.SelectedItem)
            {
                comboBoxRecordedUi.SelectedItem = leafUiTask;
            }

            int childCount = selectedItem.Items.Count;
            if (childCount > 0)
            {
                treeUiPath.ContextMenu = treeUiPath.Resources["UiNode"] as System.Windows.Controls.ContextMenu;
            }
            else
            {
                treeUiPath.ContextMenu = treeUiPath.Resources["LeafNode"] as System.Windows.Controls.ContextMenu;
            }

            int nNodeCount = 1;
            var pParent = selectedItem.Parent;
            while (pParent != null)
            {
                nNodeCount++;
                pParent = pParent.Parent;
            }

            HighlightPath(nNodeCount, true);

            if (SelectChangedSource != UiSelectChangedSource.ComboBoxSelected)
                SelectChangedSource = UiSelectChangedSource.SelectChangeFinished;

            AppInsights.LogEvent("treeUiPath_SelectedItemChanged");
        }

        private void SetSessionRoot_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = treeUiPath.SelectedItem as UiTreeNode;
            if (selectedItem == null)
                return;

            int nNodeCount = 1;
            var pParent = selectedItem.Parent;
            while (pParent != null)
            {
                nNodeCount++;
                pParent = pParent.Parent;
            }

            string sessionRootPath = HighlightPath(nNodeCount, false);
            var textPath = new TextRange(rtbXPath.Document.ContentStart, rtbXPath.Document.ContentEnd).Text;
            var pos1 = rtbXPath.Document.ContentStart.GetPositionAtOffset(1, LogicalDirection.Forward);
            int nPos2 = textPath.IndexOf(sessionRootPath) + sessionRootPath.Length;
            RootSessionPath = textPath.Substring(0, nPos2);
            var pos2 = rtbXPath.Document.ContentStart.GetPositionAtOffset(nPos2 + 2, LogicalDirection.Forward);
            var textRange = new TextRange(pos1, pos2);
            textRange.ClearAllProperties();
            textRange.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.LightGray);

            AppInsights.LogEvent("SetSessionRoot_Click");
        }

        private TreeViewItem ExpandSelectedTreeNode(UiTreeNode uiTreeNode, TreeViewItem tvi, RecordedUiTask recordedUiTask)
        {
            if (tvi == null || uiTreeNode == null)
            {
                return null;
            }

            if (uiTreeNode.Items.Count == 0)
            {
                if (uiTreeNode.UiTask == recordedUiTask)
                {
                    tvi.IsSelected = true;
                    return tvi;
                }
                else
                {
                    return null;
                }
            }

            // Collapse all
            foreach (var c in uiTreeNode.Items)
            {
                var subContainer = (TreeViewItem)tvi.ItemContainerGenerator.ContainerFromItem(c);
                if (subContainer != null)
                {
                    subContainer.IsExpanded = false;
                }
            }

            foreach (var c in uiTreeNode.Items)
            {
                var subContainer = (TreeViewItem)tvi.ItemContainerGenerator.ContainerFromItem(c);
                var ret = ExpandSelectedTreeNode(c, subContainer, recordedUiTask);
                if (ret != null)
                {
                    subContainer.IsExpanded = true;
                    return ret;
                }
            }

            return null;
        }

        private void Highlight_Click(object sender, RoutedEventArgs e)
        {
            AppInsights.LogEvent("Highlight_Click");

            var selectedItem = treeUiPath.SelectedItem as UiTreeNode;
            if (selectedItem == null)
            {
                return;
            }

            RECT rect = new RECT();
            NativeMethods.HighlightCachedUI(selectedItem.RuntimeId, ref rect);
        }

        private void EditAttribute_Click(object sender, RoutedEventArgs e)
        {
            AppInsights.LogEvent("EditAttribute_Click");

            var uiTreeNode = treeUiPath.SelectedItem as UiTreeNode;
            if (uiTreeNode == null)
            {
                return;
            }

            RecordedUiTask leafUiTask = uiTreeNode.UiTask;

            if (leafUiTask == null)
            {
                if (treeUiPath.Items != null && treeUiPath.Items.Count > 0)
                {
                    var x = (TreeViewItem)treeUiPath.ItemContainerGenerator.ContainerFromItem(treeUiPath.Items[0]);
                    if (x != null)
                    {
                        TreeViewItem tvi = UiTreeNode2TreeViewItem(x, UiTreeNode.s_uiTreeNodes.First(), uiTreeNode);
                        if (tvi != null)
                        {
                            leafUiTask = GetExpandedLeafNode(tvi, uiTreeNode, selectedTreeViewItem);
                            if (leafUiTask == null)
                            {
                                leafUiTask = GetExpandedLeafNode(tvi, uiTreeNode, null);
                                selectedTreeViewItem = leafUiTask;
                            }
                        }
                    }
                }
            }

            if (leafUiTask == null)
            {
                return;
            }

            string oldNodePath = uiTreeNode.NodePath;

            var winEditAttr = new WindowEditNodeAttribute(uiTreeNode);
            if (true == winEditAttr.ShowDialog())
            {
                var xpath = leafUiTask.GetXPath(false).Replace(oldNodePath, uiTreeNode.NodePath);
                var tr = new TextRange(rtbXPath.Document.ContentStart, rtbXPath.Document.ContentEnd);
                tr.Text = leafUiTask.UpdateXPath(xpath);
            }
        }

        private void comboBoxRecordedUi_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(comboBoxRecordedUi.Text))
                comboBoxRecordedUi_ToolTip.Text = comboBoxRecordedUi.Text;
        }

        private void rtbXPath_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var tr = new TextRange(rtbXPath.Document.ContentStart, rtbXPath.Document.ContentEnd);
            if (!string.IsNullOrWhiteSpace(tr.Text))
                rtbXPath_ToolTip.Text = tr.Text;
        }

        private void textBoxCode_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBoxCode.Text))
                textBoxCode_ToolTip.Text = textBoxCode.Text;
        }

        private void Window_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            bPauseMouseKeyboardCurrent = MouseKeyboardHook.s_bPauseMouseKeyboard;

            if (bPauseMouseKeyboardCurrent == false)
            {
                MouseKeyboardHook.s_bPauseMouseKeyboard = true;
                NativeMethods.PostMessage(MainWindow.s_windowHandle, (int)MainWindow.UiThreadTask.PauseRecording, 0, 0);
            }
        }

        private void Window_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            MouseKeyboardHook.s_bPauseMouseKeyboard = bPauseMouseKeyboardCurrent;
        }

        private void ComboBoxRecordedUi_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            AppInsights.LogEvent("ComboBoxRecordedUi_PreviewMouseLeftButtonDown");
        }
    }
}
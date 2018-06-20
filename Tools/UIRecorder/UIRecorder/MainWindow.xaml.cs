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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WinAppDriverUIRecorder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const double tabCodeHeight = 250;
        public static IntPtr windowHandle;
        static MainWindow s_mainWin;
        public static List<RecordedUiTask> s_listRecordedUi = new List<RecordedUiTask>();
        System.Timers.Timer timer = null;
        const string strStopRecording = "Stop Recording";

        public MainWindow()
        {
            InitializeComponent();
        }

        public double TabCodeHeight
        {
            get { return tabCodeHeight; }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gridXPath.RowDefinitions[2].Height = new GridLength(TabCodeHeight);
            windowHandle = new System.Windows.Interop.WindowInteropHelper(Application.Current.MainWindow).Handle;
            System.Windows.Interop.HwndSource.FromHwnd(windowHandle).AddHook(XmlNodePathRecorder.ProcessMessage);

            s_mainWin = this;
            this.comboBoxRecordedUi.ItemsSource = s_listRecordedUi;

            ConstVariables.InitVk2StringMap();

            timer = new System.Timers.Timer(100) { Enabled = true, AutoReset = false };
            timer.Elapsed += InitMouseKeyboard;
            timer.Start();
        }

        private void InitMouseKeyboard(Object source, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop();
            timer.Elapsed -= InitMouseKeyboard;
            timer = null;

            this.Dispatcher.Invoke(new Action(() =>
            {
                MouseKeyboardEventHandler.Init();
            }), System.Windows.Threading.DispatcherPriority.Normal);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            MouseKeyboardEventHandler.UnInit();
            MouseKeyboardHook.StopHook();

            System.Windows.Interop.HwndSource.FromHwnd(windowHandle).RemoveHook(XmlNodePathRecorder.ProcessMessage);
            s_mainWin = null;
        }

        private void btnRecord_Click(object sender, RoutedEventArgs e)
        {
            // Stop recording
            if (btnRecord.Content.ToString() == strStopRecording)
            {
                btnRecord.Content = "Record";
                this.btnClear.IsEnabled = true;
            }
            else // Start recording
            {
                btnRecord.Content = strStopRecording;
                this.btnClear.IsEnabled = false;
            }

            btnRecord.Focusable = false;
        }

        public static bool IsRecording()
        {
            return s_mainWin.btnRecord.Content.ToString() == strStopRecording;
        }

        public static void UpdateLastUi(RecordedUiTask uiTask)
        {
            s_mainWin.Dispatcher.Invoke(new Action(() =>
            {
                TextRange tr = new TextRange(s_mainWin.rtbXPath.Document.ContentStart, s_mainWin.rtbXPath.Document.ContentEnd);
                tr.Text = uiTask.GetXPath();

                s_mainWin.textBoxXml.Text = uiTask.GetXml();

                s_mainWin.textBoxCode.Text = "";
            }), System.Windows.Threading.DispatcherPriority.ContextIdle);
        }

        public static void AddRecordedUi(RecordedUiTask uiTask)
        {
            if (uiTask.GetTask() != UiTaskName.Inspect)
            {
                MainWindow.s_listRecordedUi.Add(uiTask);
                s_mainWin.btnClear.IsEnabled = s_listRecordedUi.Count > 0;
                s_mainWin.btnWinAppDriverCode.IsEnabled = s_listRecordedUi.Count > 0;
            }

            s_mainWin.Dispatcher.Invoke(new Action(() =>
            {
                if (uiTask.GetTask() != UiTaskName.Inspect)
                {
                    s_mainWin.comboBoxRecordedUi.ItemsSource = null;
                    s_mainWin.comboBoxRecordedUi.ItemsSource = MainWindow.s_listRecordedUi;
                    s_mainWin.comboBoxRecordedUi.SelectedIndex = MainWindow.s_listRecordedUi.Count - 1;
                }
                else
                {
                    UpdateLastUi(uiTask);
                }

            }), System.Windows.Threading.DispatcherPriority.ContextIdle);
        }

        private void comboBoxRecordedUi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selUiTask = (comboBoxRecordedUi.SelectedItem as RecordedUiTask);
            if (selUiTask != null)
            {
                TextRange tr = new TextRange(rtbXPath.Document.ContentStart, rtbXPath.Document.ContentEnd);
                tr.Text = selUiTask.GetXPath();

                textBoxXml.Text = selUiTask.GetXml();

                textBoxCode.Text = selUiTask.GetCSCode(comboBoxRecordedUi.SelectedIndex, comboBoxRecordedUi.SelectedIndex);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            s_listRecordedUi.Clear();

            this.comboBoxRecordedUi.Items.Refresh();

            TextRange tr = new TextRange(rtbXPath.Document.ContentStart, rtbXPath.Document.ContentEnd);
            tr.Text = "";
            textBoxXml.Text = "";
            textBoxCode.Text = "";

            btnClear.IsEnabled = s_listRecordedUi.Count > 0;
            btnWinAppDriverCode.IsEnabled = s_listRecordedUi.Count > 0;
        }

        private void btnWinAppDriverCode_Click(object sender, RoutedEventArgs e)
        {
            // Stop recording
            if (btnRecord.Content.ToString() == strStopRecording)
            {
                btnRecord_Click(null, null);
            }

            StringBuilder sb = new StringBuilder();
            int nOrder = 0;
            int nOrderFocused = 0;
            foreach (var uiTask in s_listRecordedUi)
            {
                nOrder++;
                if (uiTask.GetTask() == UiTaskName.LeftClick)
                {
                    nOrderFocused = nOrder;
                }

                sb.AppendLine(uiTask.GetCSCode(nOrder, nOrderFocused));
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

        private void HideTabXmlCode(bool bHide)
        {
            if (bHide == true)
            {
                if (gridXPath.RowDefinitions[2].ActualHeight < TabCodeHeight + 1)
                {
                    bHide = false;
                }
            }

            if (bHide == false)
            {
                gridXPath.RowDefinitions[2].Height = new GridLength(gridXPath.ActualHeight * 0.667);
            }
            else
            {
                gridXPath.RowDefinitions[2].Height = new GridLength(TabCodeHeight);
            }
        }

        private void TabItemNodes_Click(object sender, RoutedEventArgs e)
        {
            HideTabXmlCode(this.tabControl.SelectedIndex == 0);
        }

        private void TabItemCode_Click(object sender, RoutedEventArgs e)
        {
            HideTabXmlCode(this.tabControl.SelectedIndex == 1);
        }

        private void MenuCopy(object sender, RoutedEventArgs e)
        {
            var tr = new TextRange(rtbXPath.Document.ContentStart, rtbXPath.Document.ContentEnd);
            if (tr != null)
            {
                Clipboard.SetText(tr.Text);
            }
        }

        string StringFromRichTextBox(RichTextBox rtb)
        {
            TextRange tr = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            return tr.Text;
        }

        private void btnClear_Loaded(object sender, RoutedEventArgs e)
        {
            btnClear.IsEnabled = s_listRecordedUi.Count > 0;
        }

        private void btnWinAppDriverCode_Loaded(object sender, RoutedEventArgs e)
        {
            btnWinAppDriverCode.IsEnabled = s_listRecordedUi.Count > 0;
        }
    }
}

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
using System.Windows;
using System.Windows.Controls;

namespace WinAppDriverUIRecorder
{
    /// <summary>
    /// Interaction logic for WindowEditNodeAttribute.xaml
    /// </summary>
    public partial class WindowEditNodeAttribute : Window
    {
        UiTreeNode uiTreeNode;
        static List<string> listAttrCompareMethod = new List<string>();
        string xpathNode;
        bool? bAddPositionAtribute = null;

        public WindowEditNodeAttribute(UiTreeNode uiNode)
        {
            InitializeComponent();
            uiTreeNode = uiNode;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (uiTreeNode == null)
            {
                return;
            }

            if (listAttrCompareMethod.Count == 0)
            {
                // order must be same as  public enum CompareMethod {Equal, StartsWith, Contains};
                listAttrCompareMethod.Add("=");
                listAttrCompareMethod.Add("starts-with");
                listAttrCompareMethod.Add("contains");
            }

            ReadUiTreeNodeValue();

            this.dgNameValue.ItemsSource = uiTreeNode.LoadPropertyData();
        }

        private void ReadUiTreeNodeValue()
        {
            this.textBoxName.Text = uiTreeNode.Name;
            this.textBoxAutoId.Text = uiTreeNode.AutomationId;
            this.textBoxClassName.Text = uiTreeNode.ClassName;

            this.comboBoxName.IsEnabled = !string.IsNullOrEmpty(uiTreeNode.Name);
            this.textBoxName.IsEnabled = !string.IsNullOrEmpty(uiTreeNode.Name);
            if (this.comboBoxName.IsEnabled)
            {
                this.comboBoxName.ItemsSource = listAttrCompareMethod;
                this.comboBoxName.SelectedIndex = (int)uiTreeNode.NameCompareMethod;
            }

            this.comboboxClass.IsEnabled = !string.IsNullOrEmpty(uiTreeNode.ClassName);
            this.textBoxClassName.IsEnabled = !string.IsNullOrEmpty(uiTreeNode.ClassName);
            if (this.comboboxClass.IsEnabled)
            {
                this.comboboxClass.ItemsSource = listAttrCompareMethod;
                this.comboboxClass.SelectedIndex = (int)uiTreeNode.ClassNameCompareMethod;
            }

            this.comboboxAutoId.IsEnabled = !string.IsNullOrEmpty(uiTreeNode.AutomationId);
            this.textBoxAutoId.IsEnabled = !string.IsNullOrEmpty(uiTreeNode.AutomationId);
            if (this.comboboxAutoId.IsEnabled)
            {
                this.comboboxAutoId.ItemsSource = listAttrCompareMethod;
                this.comboboxAutoId.SelectedIndex = (int)uiTreeNode.AutomationIdCompareMethod;
            }

            if (string.IsNullOrEmpty(uiTreeNode.Position) == false)
            {
                this.textBoxPosition.Text = uiTreeNode.Position;
            }

            xpathNode = uiTreeNode.NodePath;
            this.textBoxNode.Text = xpathNode;
        }


        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            UpdateUiTreeNodeValue();
            DialogResult = true;
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void UpdateUiTreeNodeValue()
        {
            if (this.comboBoxName.SelectedValue != null)
            {
                uiTreeNode.Name = this.textBoxName.Text;

                uiTreeNode.NameCompareMethod = UiTreeNode.CompareMethod.Equal;
                if (this.comboBoxName.SelectedValue.ToString() == "starts-with")
                {
                    uiTreeNode.NameCompareMethod = UiTreeNode.CompareMethod.StartsWith;
                }
                else if (this.comboBoxName.SelectedValue.ToString() == "contains")
                {
                    uiTreeNode.NameCompareMethod = UiTreeNode.CompareMethod.Contains;
                }
            }

            if (this.comboboxClass.SelectedValue != null)
            {
                uiTreeNode.ClassName = this.textBoxClassName.Text;

                uiTreeNode.ClassNameCompareMethod = UiTreeNode.CompareMethod.Equal;
                if (this.comboboxClass.SelectedValue.ToString() == "starts-with")
                {
                    uiTreeNode.ClassNameCompareMethod = UiTreeNode.CompareMethod.StartsWith;
                }
                else if (this.comboboxClass.SelectedValue.ToString() == "contains")
                {
                    uiTreeNode.ClassNameCompareMethod = UiTreeNode.CompareMethod.Contains;
                }
            }

            if (this.comboboxAutoId.SelectedValue != null)
            {
                uiTreeNode.AutomationId = this.textBoxAutoId.Text;

                uiTreeNode.AutomationIdCompareMethod = UiTreeNode.CompareMethod.Equal;
                if (this.comboboxAutoId.SelectedValue.ToString() == "starts-with")
                {
                    uiTreeNode.AutomationIdCompareMethod = UiTreeNode.CompareMethod.StartsWith;
                }
                else if (this.comboboxAutoId.SelectedValue.ToString() == "contains")
                {
                    uiTreeNode.AutomationIdCompareMethod = UiTreeNode.CompareMethod.Contains;
                }
            }

            uiTreeNode.NodePath = UpdateNodeXPath();
        }

        private string UpdateNodeXPath()
        {
            xpathNode = $"/{uiTreeNode.Tag}";

            if (!string.IsNullOrEmpty(uiTreeNode.Name) && comboBoxName.SelectedValue != null)
            {
                var nameComboValue = comboBoxName.SelectedValue.ToString();

                if (nameComboValue == "=")
                    xpathNode += $"[@Name=\\\"{this.textBoxName.Text}\\\"]";
                else
                    xpathNode += $"[{nameComboValue}(@Name,\\\"{this.textBoxName.Text}\\\")]";
            }

            if (!string.IsNullOrEmpty(uiTreeNode.ClassName) && comboboxClass.SelectedValue != null)
            {
                var classComboValue = comboboxClass.SelectedValue.ToString();

                if (classComboValue == "=")
                    xpathNode += $"[@ClassName=\\\"{textBoxClassName.Text}\\\"]";
                else
                    xpathNode += $"[{classComboValue}(@ClassName,\\\"{textBoxClassName.Text}\\\")]";
            }

            if (!string.IsNullOrEmpty(uiTreeNode.AutomationId) && comboboxAutoId.SelectedValue != null)
            {
                var autoIdComboValue = comboboxAutoId.SelectedValue.ToString();

                if (autoIdComboValue == "=")
                    xpathNode += $"[@AutomationId=\\\"{textBoxAutoId.Text}\\\"]";
                else
                    xpathNode += $"[{autoIdComboValue}(@AutomationId,\\\"{textBoxAutoId.Text}\\\")]";
            }

            if (!string.IsNullOrEmpty(uiTreeNode.Position) && textBoxPosition.Text.Trim().Length > 0)
            {
                if (bAddPositionAtribute == true)
                {
                    var positionValue = textBoxPosition.Text.Trim();
                    xpathNode += $"[position()={positionValue}]";
                }
            }

            return xpathNode;
        }

        private void comboBoxName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.textBoxNode.Text = UpdateNodeXPath();
        }

        private void comboboxClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.textBoxNode.Text = UpdateNodeXPath();
        }

        private void comboboxAutoId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.textBoxNode.Text = UpdateNodeXPath();
        }

        private void textBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.textBoxNode.Text = UpdateNodeXPath();
        }

        private void textBoxClassName_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.textBoxNode.Text = UpdateNodeXPath();
        }

        private void textBoxAutoId_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.textBoxNode.Text = UpdateNodeXPath();
        }

        private void textBoxPosition_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBoxPosition.Text.Trim().Length == 0)
                return;

            foreach (var c in textBoxPosition.Text)
            {
                if (char.IsDigit(c) == false)
                {
                    return;
                }
            }

            this.textBoxNode.Text = UpdateNodeXPath();
        }

        private void checkboxAddPosition_Checked(object sender, RoutedEventArgs e)
        {
            bAddPositionAtribute = checkboxAddPosition.IsChecked;
            textBoxPosition_TextChanged(null, null);
        }
    }
}

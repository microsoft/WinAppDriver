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

#pragma once

typedef struct
{
    UINT id;
    PCWSTR pszName;
} ControlId2NameEntry;

// Following lookup table is built using declarations like "UIA_ButtonControlTypeId" defined in UIAutomationClient.h
#define CONTROL_TYPE(x) { UIA_##x##ControlTypeId, L#x }
ControlId2NameEntry const gc_controlTypesTable[] =
{
    CONTROL_TYPE(Button),
    CONTROL_TYPE(Calendar),
    CONTROL_TYPE(CheckBox),
    CONTROL_TYPE(ComboBox),
    CONTROL_TYPE(Edit),
    CONTROL_TYPE(Hyperlink),
    CONTROL_TYPE(Image),
    CONTROL_TYPE(ListItem),
    CONTROL_TYPE(List),
    CONTROL_TYPE(Menu),
    CONTROL_TYPE(MenuBar),
    CONTROL_TYPE(MenuItem),
    CONTROL_TYPE(ProgressBar),
    CONTROL_TYPE(RadioButton),
    CONTROL_TYPE(ScrollBar),
    CONTROL_TYPE(Slider),
    CONTROL_TYPE(Spinner),
    CONTROL_TYPE(StatusBar),
    CONTROL_TYPE(Tab),
    CONTROL_TYPE(TabItem),
    CONTROL_TYPE(Text),
    CONTROL_TYPE(ToolBar),
    CONTROL_TYPE(ToolTip),
    CONTROL_TYPE(Tree),
    CONTROL_TYPE(TreeItem),
    CONTROL_TYPE(Custom),
    CONTROL_TYPE(Group),
    CONTROL_TYPE(Thumb),
    CONTROL_TYPE(DataGrid),
    CONTROL_TYPE(DataItem),
    CONTROL_TYPE(Document),
    CONTROL_TYPE(SplitButton),
    CONTROL_TYPE(Window),
    CONTROL_TYPE(Pane),
    CONTROL_TYPE(Header),
    CONTROL_TYPE(HeaderItem),
    CONTROL_TYPE(Table),
    CONTROL_TYPE(TitleBar),
    CONTROL_TYPE(Separator),
    CONTROL_TYPE(SemanticZoom),
    CONTROL_TYPE(AppBar)
};
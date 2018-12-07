# UWPControls

UWPControls is a collection of test scenarios that covers basic interactions with [Universal Windows Platform controls (UI elements)](https://docs.microsoft.com/en-us/windows/uwp/controls-and-patterns/). Use this as a reference on how to interact with certain UWP controls you have in your application.

The test scenarios are written against [AppUIBasics](../../ApplicationUnderTests/AppUIBasics) application that contains all UWP basic controls. This application needs to be installed once in the machine you are running your test. Follow the instruction on the application [README](../../ApplicationUnderTests/AppUIBasics/README.md).


## Requirements

- Windows 10 PC with the latest Windows 10 version (Version 1607 or later)
- Microsoft Visual Studio 2015 or later
- [AppUIBasics](../../ApplicationUnderTests/AppUIBasics) application


## Getting Started

1. Open `UWPControls.sln` in Visual Studio
2. Select **Test** > **Windows** > **Test Explorer**
3. Select **Run All** on the test pane or through menu **Test** > **Run** > **All Tests**

Note: The target application <em>AppUIBasics</em> will need to have been built and deployed [using the instructions here](https://github.com/Microsoft/WinAppDriver/tree/master/ApplicationUnderTests/AppUIBasics) prior to running the tests.

> Once the project is successfully built, you can use the **TestExplorer** to pick and choose the test scenario(s) to run


## Controls

- [Button      ](./Button.cs)
- [CheckBox    ](./CheckBox.cs)
- [ComboBox    ](./ComboBox.cs)
- [DatePicker  ](./DatePicker.cs)
- [ProgressBar ](./ProgressBar.cs)
- [RadioButton ](./RadioButton.cs)
- [Slider      ](./Slider.cs)
- [TextBlock   ](./TextBlock.cs)
- [TextBox     ](./TextBox.cs)
- [ToggleButton](./ToggleButton.cs)
- [ToggleSwitch](./ToggleSwitch.cs)


## Adding/Updating Test Scenario

Please follow the guidelines below to maintain test reliability and reduce test execution time:
1. Provide a complete set of interactions (if applicable) for each new control
2. Aim for simple and reliable scenario using the least amount of test steps
3. Reuse existing application session when possible to reduce unnecessary application re-launching

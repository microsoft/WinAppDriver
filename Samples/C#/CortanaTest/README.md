# CortanaTest

CortanaTest is a sample test project that runs and validates basic UI scenarios on Windows 10 built-in **Cortana** application. This sample demonstrates the use of [Desktop Session](../../../README.md#creating-a-desktop-session) and [creation of a new session from a top level window](../../../README.md#attaching-to-an-existing-app-window). In this sample, the **Desktop Session** is used to launch and locate **Cortana** window which the **Cortana Session** will attach to.

This test project highlights the following Windows Application Driver feature.
- Attaching to an Existing App Window
- Creating a Desktop Session
- Sending system keyboard shortcut such as Windows Key + S
- Sending keyboard input to a rich edit box
- Finding element in Edge based WebView


## Requirements

- Windows 10 PC with the latest Windows 10 version (Version 1607 or later)
- Microsoft Visual Studio 2015 or later


## Getting Started

1. Open `CortanaTest.sln` in Visual Studio
2. Select **Test** > **Windows** > **Test Explorer**
3. Select **Run All** on the test pane or through menu **Test** > **Run** > **All Tests**

> Once the project is successfully built, you can use the **TestExplorer** to pick and choose the test scenario(s) to run


## Adding/Updating Test Scenario

Please follow the guidelines below to maintain test reliability and conciseness:
1. Test all changes against all supported version of Windows 10 built-in **Cortana** app
2. Maintain simplicity and only add tests that provide additional value to the sample

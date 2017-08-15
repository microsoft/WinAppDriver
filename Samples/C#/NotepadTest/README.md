# NotepadTest

NotepadTest is a sample test project that runs and validates basic UI scenarios on Windows 10 built-in **Notepad** application. This sample depicts a typical test project that is written for a classic windows application built using Win32, DirectUI, WPF, etc. You can use this as a template for writing your test project.

In addition to the **Notepad** application primary session, this sample uses **Windows Explorer** as a secondary session to verify file creation and help clean up file artifact.

This test project highlights some common interactions below that can be put together to perform and verify various UI scenario on a classic app.
- Creating a classic windows app session
- Interacting with menu item
- Sending keyboard input to a text box
- Sending keyboard shortcut
- Using secondary session


## Requirements

- Windows 10 PC with the latest Windows 10 version (Version 1607 or later)
- Microsoft Visual Studio 2015 or later


## Getting Started

1. Open `NotepadTest.sln` in Visual Studio
2. Select **Test** > **Windows** > **Test Explorer**
3. Select **Run All** on the test pane or through menu **Test** > **Run** > **All Tests**

> Once the project is successfully built, you can use the **TestExplorer** to pick and choose the test scenario(s) to run


## Adding/Updating Test Scenario

Please follow the guidelines below to maintain test reliability and conciseness:
1. Maintain original state after test runs and keep clean state in between tests when possible
2. Only add tests that provide additional value to the sample

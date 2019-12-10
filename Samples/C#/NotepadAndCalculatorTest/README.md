# NotepadCalculatorTest

NotepadCalculatorTest is a sample test project that runs and validates basic UI scenario on Windows 10  using 2 built-in application, **Calculator** And **Notepad**. 

This test project highlights the following basic interactions to demonstrate how UI testing using Windows Application Driver work.
- Switching between session while working with 2 applications in single session
- Switching between session while working with 2 applications in multi session

## Requirements

- Windows 10 PC with the latest Windows 10 version (Version 1809 or later)
- Microsoft Visual Studio 2017 or later


## Getting Started

1. [Run](../../../README.md#installing-and-running-windows-application-driver) `WinAppDriver.exe` on the test device
2. Open `NotepadCalculatorTest.sln` in Visual Studio
3. Select **Build** > **Rebuild Solution**
4. Select **Test** > **Windows** > **Test Explorer**
5. Select **Run All** on the test pane or through menu **Test** > **Run** > **All Tests**

> Once the project is successfully built, you can use the **TestExplorer** to pick and choose the test scenario(s) to run

> If Visual Studio fail to discover and run the test scenarios:
> 1. Select **Tools** > **Options...** > **Test**
> 2. Under *Active Solution*, uncheck *For improved performance, only use test adapters in test assembly folder or as specified in runsettings file*

## Adding/Updating Test Scenario

Please follow the guidelines below to maintain test reliability and conciseness:
1. Test all changes against all supported version of Windows 10 built-in **Calculator** and **Notepad** app
2. Maintain simplicity and only add tests that provide additional value to the sample

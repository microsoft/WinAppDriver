# AbsoluteXPath

AbsoluteXPath is a sample test project that demonstrates how UIRecorder generated xpaths are used to find target elements. You can use this as a template for writing your test project.

This test project highlights some common interactions below that can be put together to perform and verify various UI scenario on a modern app.
- Sending click action to an element
- Sending keyboard input to a text box

## Requirements

- Windows 10 PC with the latest Windows 10 version (Version 1607 or later)
- Microsoft Visual Studio 2015 or later

## Getting Started

1. Open `AlarmClockTest.sln` in Visual Studio
2. Select **Test** > **Windows** > **Test Explorer**
3. Select **Run All** on the test pane or through menu **Test** > **Run** > **All Tests**

> Once the project is successfully built, you can use the **TestExplorer** to pick and choose the test scenario(s) to run


## Adding/Updating Test Scenario

Please follow the guidelines below to maintain test reliability and conciseness:
1. Group test methods based on the UI view page such as Alarm, Stopwatch, Timer, etc.
2. Maintain original state after test runs and keep clean state in between tests when possible
3. Only add tests that provide additional value to the sample

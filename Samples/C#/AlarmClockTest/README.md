# AlarmClockTest

AlarmClockTest is a sample test project that runs and validates basic UI scenarios on Windows 10 built-in **Alarms & Clock** application. This sample depicts a typical test project that is written for a modern Universal Windows Platform application. You can use this as a template for writing your test project.

This test project highlights some common interactions below that can be put together to perform and verify various UI scenario on a modern app.
- Creating a modern UWP app session
- Navigating between tabs (pivot items)
- Sending click action to an element
- Sending keyboard input to a text box
- Sending touch action
- Sending touch gesture
- Selecting entry in a list or picker
- Finding element using name, accessibility id, or XPath
- Finding multiple elements
- Retrieving element value
- Retrieving various element state such as displayed, enabled, and selected


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

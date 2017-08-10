# CalculatorTest

CalculatorTest is a sample test project that runs and validates basic UI scenarios on Windows 10 built-in **Calculator** application. This sample is created as the most basic test project to quickly try out Windows Application Driver.

This test project highlights the following basic interactions to demonstrate how UI testing using Windows Application Driver work.
- Creating a modern UWP app session
- Finding element using name
- Finding element using accessibility id
- Finding element using XPath
- Sending click action to an element
- Retrieving element value
- Navigating using SplitViewPane


## Requirements

- Windows 10 PC with the latest Windows 10 version (Version 1607 or later)
- Microsoft Visual Studio 2015 or later


## Getting Started

1. Open `CalculatorTest.sln` in Visual Studio
2. Select **Test** > **Windows** > **Test Explorer**
3. Select **Run All** on the test pane or through menu **Test** > **Run** > **All Tests**

> Once the project is successfully built, you can use the **TestExplorer** to pick and choose the test scenario(s) to run


## Adding/Updating Test Scenario

Please follow the guidelines below to maintain test reliability and conciseness:
1. Test all changes against all supported version of Windows 10 built-in **Calculator** app
2. Maintain simplicity and only add tests that provide additional value to the sample

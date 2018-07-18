# Paint3DTest

Paint3DTest is a sample test project that demonstrates how to perform pen and multi-touch interactions through the actions API using the Windows 10 built-in **Paint 3D** application.

This test project highlights some Actions API interactions below.
- Drawing and erasing using pen strokes
- Performing pinch and zoom multi-touch gestures


## Requirements

- Windows 10 PC with the latest Windows 10 version (Version 1607 or later)
- Microsoft Visual Studio 2017 or later


## Getting Started

1. [Run](../../../README.md#installing-and-running-windows-application-driver) `WinAppDriver.exe` on the test device
1. Open `Paint3DTest.sln` in Visual Studio
2. Select **Test** > **Windows** > **Test Explorer**
3. Select **Run All** on the test pane or through menu **Test** > **Run** > **All Tests**

> Once the project is successfully built, you can use the **TestExplorer** to pick and choose the test scenario(s) to run


## Including proper appium-dotnet-driver NuGet package

Pen and multi-touch input support in Windows Application Driver is accessed using W3C WebDriver Actions API. Currently this feature is only supported
through a temporary `WinAppDriver.Preview.Appium.WebDriver` instead of the official `Appium.WebDriver` NuGet package. This sample test
is using this temporary NuGet package as shown in [packages.config](./packages.config).

To make use all the pen features through the library in the NuGet package above, use the `OpenQA.Selenium.Appium.Interactions.PointerInputDevice`
instead of the `OpenQA.Selenium.Interactions.PointerInputDevice`.


## Adding/Updating Test Scenario

Please follow the guidelines below to maintain test reliability and conciseness:
1. Group test methods based on the relevant scenarios such as **Draw** and **Zoom**
2. Maintain original state after test runs and keep clean state in between tests when possible
3. Only add tests that provide additional value to the sample

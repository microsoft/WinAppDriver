# WebDriverAPI

WebDriverAPI is a collection of test scenarios that covers every WebDriver command supported by Windows Application Driver. Use this as a reference on how to invoke each command in the test script. While these tests are written in C#, corresponding commands in other Selenium supported language bindings should map to the same API endpoint.

The test scenarios are written against Windows 10 built-in apps such as **Calculator**, **Alarms & Clock**, **Notepad**, **File Explorer**, and **Microsoft Edge Browser**. Therefore these tests can simply be run on Windows 10 PC running Windows Application Driver.


## Requirements

- Windows 10 PC with the latest Windows 10 version (Version 1607 or later)
- Microsoft Visual Studio 2015 or later


## Getting Started

1. Open `WebDriverAPI.sln` in Visual Studio
2. Select **Test** > **Windows** > **Test Explorer**
3. Select **Run All** on the test pane or through menu **Test** > **Run** > **All Tests**

> Once the project is successfully built, you can use the **TestExplorer** to pick and choose the test scenario(s) to run


## Command Summary

Please refer to [here](/./././Docs/SupportedAPIs.md) for an updated list of supported WinAppDriver commands. 

## Command Reference

These tests are written to verify each API endpoint behavior and error values as specified in [JSON Wire Protocol](https://github.com/SeleniumHQ/selenium/wiki/JsonWireProtocol) document.


## Adding/Updating Test Scenario

Please follow the guidelines below to maintain test reliability and reduce test execution time:
1. Avoid duplicating test scenario that uses the same code path
2. Aim for simple and reliable scenario using the least amount of test steps
3. Write test against Windows 10 built-in apps only to avoid requiring extra installation
4. Reuse existing application session when possible to reduce unnecessary application re-launching
5. Group test methods accordingly by naming them with the existing convention. E.g.
   - `ClickElement`
   - `ClickElementError_NoSuchWindow`
   - `ClickElementError_StaleElement`

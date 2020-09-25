# CalculatorTest

CalculatorTest is a sample test project that runs and validates basic UI scenarios on Windows 10 built-in **Calculator** application. This sample is created as the most basic test project to quickly try out Windows Application Driver using the [RobotFramework-Zoomba Library](https://github.com/Accruent/robotframework-zoomba).

This test project highlights the following basic interactions to demonstrate how you can write UI tests in RobotFramework which run with the Windows Application Driver.
- Creating a modern UWP app session
- Finding element using name, accessibility id, class
- Sending click action to an element
- Inputting text to an element
- Retrieving element value
- Mousing over elements through ids and offsets
- Context(right) clicking elements
- Sending specific keys to an application

Additional keywords can be found in the [library documentation](https://accruent.github.io/robotframework-zoomba/DesktopLibraryDocumentation.html).

## Requirements

- Windows 10 PC with the latest Windows 10 version (Version 1607 or later)
- Python v3.6 or higher
- Use Python package manager (PIP) to install the following dependencies:
    - pip install robotframework-zoomba --upgrade
- WinAppDriver running on the default port 4723
  - No longer necessary if using the 'Driver Setup' and 'Driver Teardown' keywords in the example
- Recommend an IDE for editing/debugging a Robotframework project such as Visual Studio Code or Pycharm

## Getting Started

1. Open a Command Prompt and browse to this sample directory.
2. Type **robot calculatortest.robot**

## Adding/Updating Test Scenario

Please follow the guidelines below to maintain test reliability and conciseness:
1. Test all changes against all supported version of Windows 10 built-in **Calculator** app
2. Maintain simplicity and only add tests that provide additional value to the sample

# CalculatorTest

CalculatorTest is a sample test project that runs and validates basic UI scenarios on Windows 10 built-in **Calculator** application. This sample is created as the most basic test project to quickly try out Windows Application Driver.

This test project highlights the following basic interactions to demonstrate how you can write UI tests in Python which run with Windows Application Driver.
- Creating a modern UWP app session
- Finding element using name
- Finding element using accessibility id
- Sending click action to an element
- Retrieving element value

## Requirements

- Windows 10 PC with the latest Windows 10 version (Version 1607 or later)
- Python v2.7 or higher
- Use Python package manager (PIP) to install the following dependencies
    - Appium-Python-Client (tested version 0.24)
    - selenium (tested version 3.5.0)
    - setuptools (tested version 28.8.0)
- Recommend an IDE for editing/debuggin a Python project such as Visual Studio Code 

## Getting Started

1. Open a Command Prompt and run WinAppDriver.exe (in C:\Program Files (86)\Windows Application Driver)
1. Open another Command Prompt and browse to this Sample directory
2. type **python calculatortest.py**

## Adding/Updating Test Scenario

Please follow the guidelines below to maintain test reliability and conciseness:
1. Test all changes against all supported version of Windows 10 built-in **Calculator** app
2. Maintain simplicity and only add tests that provide additional value to the sample

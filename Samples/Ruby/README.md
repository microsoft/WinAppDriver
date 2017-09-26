# Calculator Example

This a sample test project that runs and validates basic UI scenarios on Windows 10 built-in **Calculator** application. This sample is created as the most basic test project to quickly try out Windows Application Driver.

This test project highlights the following basic interactions to demonstrate how UI testing using Windows Application Driver work.
- Creating a modern UWP app session
- Finding element using name
- Finding element using accessibility id
- Finding element using XPath <= FIXME
- Sending click action to an element
- Retrieving element value


## Requirements

- Windows 10 PC with the latest Windows 10 version (Version 1607 or later)
- Ruby for Windows
- The 'selenium-webdriver' ruby gem
- The 'appium_lib' ruby gem
- The 'cucumber' ruby gem
- A good code editor (E.g. Visual Studio Code)


## Getting Started

### Start WinAppDriver Server

1. Open a command prompt
2. Change directory into Windows Application Driver installation folder
2. Run the WinAppDriver server with WebDriver path

`cd C:\Program Files (x86)\Windows Application Driver`
`winappdriver.exe 127.0.0.1 4723/wd/hub`

### Run tests

1. Open a command prompt
2. Change directory into WinAppDriver\Samples\Ruby
3. Run the script to start the tests:

`cucumber`

Run with log output in HTML format:

`cucumber --tags @standard --format html --out test_report.html --format pretty`


## Adding/Updating Test Scenario

Please follow the guidelines below to maintain test reliability and conciseness:
1. Test all changes against all supported version of Windows 10 built-in **Calculator** app
2. Maintain simplicity and only add tests that provide additional value to the sample

# CalculatorTest

CalculatorTest is a sample test project that runs and validates basic UI scenarios on Windows 10 built-in **Calculator** application using JavaScript. This sample is created as the most basic test project to quickly try out Windows Application Driver.

This test project highlights the following basic interactions to demonstrate how UI testing using Windows Application Driver work.
- Creating a modern UWP app session
- Finding element using accessibility id
- Sending click action to an element
- Retrieving element value
- Use WebDriverIO
- Test: WebDriverIO <-> Appium <-> WinAppDriver
- Test: WebDriverIO <-> WinAppDriver

## Requirements

- Windows 10 PC with the latest Windows 10 version (Version 1809 or later)
- nodejs and yarn

## Dependencies
* Install nodejs, make sure a recent version of [Node.js](https://nodejs.org) is installed. [Chocolatey](https://chocolatey.org/) is the recommended installation method. But you can also install Node directly from [NodeJs](https://nodejs.org/en/download/).  To use chocolately, from an elevated Command Prompt, run:
  ```
  choco install nodejs
  ```
* Install [Yarn](https://yarnpkg.com/en/docs/install) (*optional* if you use npm command directly)

## Getting Started
 Install dependencies from the package.json file
  ```
  yarn install
  ```

## Run the test
Appium is optional. There are two typical configuration
1. test <-> Appium <-> WinAppDriver
2. test <-> WinAppDriver

### Test: WebDriverIO <-> Appium <-> WinAppDriver
1. Start appium

```
yarn run appium
```

And the output is like this:
```
warning package.json: No license field
$ appium
[Appium] Welcome to Appium v1.14.1
[Appium] Appium REST http interface listener started on 0.0.0.0:4723

```
2. Launch the test
```
yarn run testappium
```

### Test: WebDriverIO <-> WinAppDriver
1. [Install](../../../README.md#installing-and-running-windows-application-driver) `WinAppDriver.exe` on the test device

2. Start WinAppDriver and listen on 4723/wd/hub
```
cd C:\Program Files (x86)\Windows Application Driver
c:
WinAppDriver.exe 127.0.0.1 4723/wd/hub
```
And the output is like this:
```
Windows Application Driver listening for requests at: http://127.0.0.1:4723/wd/hub
Press ENTER to exit.
```

3. Launch the test
```
yarn run test
```

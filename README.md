## Windows Application Driver
Windows Application Driver (WinAppDriver) is a service to support Selenium-like UI Test Automation on Windows Applications. This service supports testing **Universal Windows Platform (UWP)**, **Windows Forms (WinForms)**, **Windows Presentation Foundation (WPF)**, and **Classic Windows (Win32)** apps on **Windows 10 PCs**. 

### Install & Run WinAppDriver
1. Download Windows Application Driver installer from <https://github.com/Microsoft/WinAppDriver/releases>
2. Run the installer on a Windows 10 machine where your application under test is installed and will be tested
3. Enable [Developer Mode](https://docs.microsoft.com/en-us/windows/uwp/get-started/enable-your-device-for-development) in Windows settings
4. Run `WinAppDriver.exe` from the installation directory (E.g. `C:\Program Files (x86)\Windows Application Driver`)

Windows Application Driver will then be running on the test machine listening to requests on the default IP address and port (`127.0.0.1:4723`). You can then run any of our [Tests](/Tests/) or [Samples](/Samples). `WinAppDriver.exe` can be configured to listen to a different IP address and port as follows:

```
WinAppDriver.exe 4727
WinAppDriver.exe 10.0.0.10 4725
WinAppDriver.exe 10.0.0.10 4723/wd/hub
```

> **Note**: You must run `WinAppDriver.exe` as **administrator** to listen to a different IP address and port.

### Write an Automation Script
Now that you've successfully installed WinAppDriver, you can get started with [authoring your first automation script](./Docs/AuthoringTestScripts.md)! 

### Supported APIs

See [here](./Docs/SupportedAPIs.md) for a list of supported APIs by WinAppDriver. API support may differ from Appium and other counterparts.

## FAQ & Documentation
Additional documentation on WinAppDriver and related topics can be found under [/Docs/](./Docs/), such as the following:
   - [Frequently Asked Questions](./Docs/FAQ.md) 
     - [General Development & Best Practices](./Docs/FAQ.md#general-development--best-practices) 
     - [Using with Appium](./Docs/UsingAppium.md)
   - [Running WinAppDriver in CI (with Azure Pipelines)](./Docs/CI_AzureDevOps.md) 
   - [Using UI Recorder](./Docs/UsingUIRecorder.md)
   - [Authoring Test Scripts](./Docs/AuthoringTestScripts.md)
   - [Using the Selenium Grid](./Docs/SeleniumGrid.md) 
   - [Running On a Remote Machine](./Docs/RunningOnRemoteMachine.md)

## Repository Content
This repository includes the following content:
* [Samples](https://github.com/Microsoft/WinAppDriver/tree/master/Samples) - used to showcase various commands and operations such as opening applications, finding elements, clicking elements, typing keystrokes, reading texts, etc; and can be run against built-in Windows 10 applications such as **Alarms & Clock**, **Calculator**, and **Notepad**. 
* [Tests](https://github.com/Microsoft/WinAppDriver/tree/master/Tests) - used to verify the functionality of **Windows Application Driver** itself. The tests cover each API endpoints extensively and also against all basic UI control scenario, and demonstrate how to invoke certain command in C#. In addition, they show how to interact with some more complex UI elements such as **DatePicker**, **SplitViewPane**, **Slider**, etc.
* [UI Recorder](https://github.com/microsoft/WinAppDriver/tree/master/Tools/UIRecorder) - standalone tool that aims to provide users a simpler way of creating automaton scripts by recording UI events performed by the user and generating XPath queries and C# code on the fly. Read more about it on our [Wiki](https://github.com/Microsoft/WinAppDriver/wiki/WinAppDriver-UI-Recorder). 
* [Docs](./Docs/) - subdirectory hosting WinAppDriver related documentation. 

## Vote on New Features
Add your feature request in [issues](../../issues/) or :+1: (+1) existing issues labeled as **Enhancement**

# Windows Application Driver

Windows Application Driver is a service to support Selenium-like UI Test Automation on Windows Applications. This service supports testing **Universal Windows Platform (UWP)**, **Windows Presentation Foundation (WPF)**, **Windows Forms (WinForms)** and **Classic Windows (Win32)** apps on **Windows 10 PCs**. Windows Application Driver complies to the [JSON Wire Protocol](https://github.com/SeleniumHQ/selenium/wiki/JsonWireProtocol) standard and some application management functionalities defined by **Appium**. If you've been looking for better support for using [Appium](http://appium.io) to test Windows Applications, then this service is for you!

This Github project provides:
- Documentation
- Samples
- Issue tracking
- Functional tests
- Sample applications under test

**Videos about WinAppDriver**

- <https://channel9.msdn.com/events/Connect/2016/202> - 8 minutes overview with demos
- <https://channel9.msdn.com/events/Build/2016/Panel-Engineering-Quality> - Session with Jonathan Lipps
- <https://channel9.msdn.com/events/Build/2016/P499> - Longer discussion
- <https://www.youtube.com/watch?v=XAJVpvaEchY> - C# demo with calculator sample walkthrough


## Getting Started

### Frequently Asked Questions 

For answers to common questions and/or best practices in using WinAppDriver, please refer to our updated [FAQ page](https://github.com/Microsoft/WinAppDriver/wiki/Frequently-Asked-Questions). 

### System Requirements

- Windows 10 PC
- Any Appium test runner (Samples and Tests in this repository use Microsoft Visual Studio as the test runner)

### Installing and Running Windows Application Driver

1. Download Windows Application Driver installer from <https://github.com/Microsoft/WinAppDriver/releases>
2. Run the installer on a Windows 10 machine where your application under test is installed and will be tested
3. Run `WinAppDriver.exe` from the installation directory (E.g. `C:\Program Files (x86)\Windows Application Driver`)

Windows Application Driver will then be running on the test machine listening to requests on the default IP address and port (`127.0.0.1:4723`). You can then run any of our [Tests](/Tests/) or [Samples](/Samples). `WinAppDriver.exe` can be configured to listen to a different IP address and port as follows:

```
WinAppDriver.exe 4727
WinAppDriver.exe 10.0.0.10 4725
WinAppDriver.exe 10.0.0.10 4723/wd/hub
```

> **Note**: You must run `WinAppDriver.exe` as **administrator** to listen to a different IP address and port.


## Samples

This repository includes some [samples](/Samples/) that can be run against built-in Windows 10 applications such as **Alarms & Clock**, **Calculator**, and **Notepad**. These samples showcase various commands and operations such as opening applications, finding elements, clicking elements, typing keystrokes, reading texts, etc.


## Tests

This repository also includes some [tests](/Tests/) that are used to verify the functionality of **Windows Application Driver** itself. These tests cover each API endpoints extensively and also against all basic UI control scenario. As a result, these tests are excellent sources to see how to invoke certain command in C# test scripts. In addition, they show how to interact with some more complex UI elements such as **DatePicker**, **SplitViewPane**, **Slider**, etc.


## Authoring Your Own Test Script

You can choose any programming language or tools supported by Appium/Selenium to write your test scripts. In the example below, we will author the test script in C# using Microsoft Visual Studio.

### Creating a Test Project

1. Open **Microsoft Visual Studio 2015** or **Microsoft Visual Studio 2017**
   > **Note**: in Visual Studio 2017 make sure you have the optional **.NET desktop development** workload installed
2. Create the test project and solution. I.e. Select **New Project > Templates > Visual C# > Test > Unit Test Project**
3. Once created, select **Project > Manage NuGet Packages... > Browse** and search for **Appium.WebDriver**
4. Install the **Appium.WebDriver** NuGet packages for the test project
5. Start writing your test (see sample code under [samples](/Samples/))

### Testing a Universal Windows Platform Application

To test a UWP app, simply specify the **Application Id** for the application you want to test in the **app** capabilities entry when you are creating a session. You can also specify launching arguments if your application supports them through **appArguments** capability. Below is an example of creating a test session for Windows **Alarms & Clock** app written in C#:

```c#
// Launch the Alarms & Clock app
DesiredCapabilities appCapabilities = new DesiredCapabilities();
appCapabilities.SetCapability("app", "Microsoft.WindowsAlarms_8wekyb3d8bbwe!App");
AlarmClockSession = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), appCapabilities);

// Use the session to control the app
AlarmClockSession.FindElementByAccessibilityId("AddAlarmButton").Click();
AlarmClockSession.FindElementByAccessibilityId("AlarmNameTextBox").Clear();
```

> You can find the **Application Id** of your application in the generated `AppX\vs.appxrecipe` file under `RegisteredUserModeAppID` node. E.g. `c24c8163-548e-4b84-a466-530178fc0580_scyf5npe3hv32!App`

### Testing a Classic Windows Application

To test a classic Windows app, specify the **full executable path** for the app under test in the **app** capabilities entry when creating a new session. Similar with modern (UWP) app, you can specify launching arguments through **appArguments** capability. But unlike modern apps, you can also specify the app working directory for a classic app through "appWorkingDir" capability. Below is an example of creating a test session for the **Notepad** app that opens `MyTestFile.txt` in `C:\MyTestFolder\`.

```c#
// Launch Notepad
DesiredCapabilities appCapabilities = new DesiredCapabilities();
appCapabilities.SetCapability("app", @"C:\Windows\System32\notepad.exe");
appCapabilities.SetCapability("appArguments", @"MyTestFile.txt");
appCapabilities.SetCapability("appWorkingDir", @"C:\MyTestFolder\");
NotepadSession = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), appCapabilities);

// Use the session to control the app
NotepadSession.FindElementByClassName("Edit").SendKeys("This is some text");
```

## Supported Capabilities

Below are the capabilities that can be used to create Windows Application Driver session.

| Capabilities       	| Descriptions                                          	| Example                                               	|
|--------------------	|-------------------------------------------------------	|-------------------------------------------------------	|
| app                	| Application identifier or executable full path        	| Microsoft.MicrosoftEdge_8wekyb3d8bbwe!MicrosoftEdge   	|
| appArguments       	| Application launch arguments                          	| https://github.com/Microsoft/WinAppDriver             	|
| appTopLevelWindow  	| Existing application top level window to attach to    	| `0xB822E2`                                            	|
| appWorkingDir      	| Application working directory (Classic apps only)     	| `C:\Temp`                                             	|
| platformName       	| Target platform name                                  	| Windows                                               	|
| platformVersion    	| Target platform version                               	| 1.0                                                   	|


## Supported APIs

| HTTP   	| Path                                              	|
|--------	|---------------------------------------------------	|
| GET    	| /status                                           	|
| POST   	| /session                                          	|
| GET    	| /sessions                                         	|
| DELETE 	| /session/:sessionId                               	|
| POST   	| /session/:sessionId/actions (v1.1 RC)         	|
| POST   	| /session/:sessionId/appium/app/launch             	|
| POST   	| /session/:sessionId/appium/app/close              	|
| POST   	| /session/:sessionId/back                          	|
| POST   	| /session/:sessionId/buttondown                    	|
| POST   	| /session/:sessionId/buttonup                      	|
| POST   	| /session/:sessionId/click                         	|
| POST   	| /session/:sessionId/doubleclick                   	|
| POST   	| /session/:sessionId/element                       	|
| POST   	| /session/:sessionId/elements                      	|
| POST   	| /session/:sessionId/element/active                	|
| GET    	| /session/:sessionId/element/:id/attribute/:name   	|
| POST   	| /session/:sessionId/element/:id/clear             	|
| POST   	| /session/:sessionId/element/:id/click             	|
| GET    	| /session/:sessionId/element/:id/displayed         	|
| GET    	| /session/:sessionId/element/:id/element           	|
| GET    	| /session/:sessionId/element/:id/elements          	|
| GET    	| /session/:sessionId/element/:id/enabled           	|
| GET    	| /session/:sessionId/element/:id/equals            	|
| GET    	| /session/:sessionId/element/:id/location          	|
| GET    	| /session/:sessionId/element/:id/location_in_view  	|
| GET    	| /session/:sessionId/element/:id/name              	|
| GET    	| /session/:sessionId/element/:id/screenshot        	|
| GET    	| /session/:sessionId/element/:id/selected          	|
| GET    	| /session/:sessionId/element/:id/size              	|
| GET    	| /session/:sessionId/element/:id/text              	|
| POST   	| /session/:sessionId/element/:id/value             	|
| POST   	| /session/:sessionId/forward                       	|
| POST   	| /session/:sessionId/keys                          	|
| GET    	| /session/:sessionId/location                      	|
| POST   	| /session/:sessionId/moveto                        	|
| GET    	| /session/:sessionId/orientation                   	|
| GET    	| /session/:sessionId/screenshot                    	|
| GET    	| /session/:sessionId/source                        	|
| POST   	| /session/:sessionId/timeouts                      	|
| POST   	| /session/:sessionId/timeouts/implicit_wait        	|
| GET    	| /session/:sessionId/title                         	|
| POST   	| /session/:sessionId/touch/click                   	|
| POST   	| /session/:sessionId/touch/doubleclick             	|
| POST   	| /session/:sessionId/touch/down                    	|
| POST   	| /session/:sessionId/touch/flick                   	|
| POST   	| /session/:sessionId/touch/longclick               	|
| POST   	| /session/:sessionId/touch/move                    	|
| POST   	| /session/:sessionId/touch/multi/perform           	|
| POST   	| /session/:sessionId/touch/scroll                  	|
| POST   	| /session/:sessionId/touch/up                      	|
| GET    	| /session/:sessionId/window                        	|
| DELETE 	| /session/:sessionId/window                        	|
| POST   	| /session/:sessionId/window                        	|
| GET    	| /session/:sessionId/window/handles                	|
| POST   	| /session/:sessionId/window/maximize               	|
| POST   	| /session/:sessionId/window/size                   	|
| GET    	| /session/:sessionId/window/size                   	|
| POST   	| /session/:sessionId/window/:windowHandle/size     	|
| GET    	| /session/:sessionId/window/:windowHandle/size     	|
| POST   	| /session/:sessionId/window/:windowHandle/position 	|
| GET    	| /session/:sessionId/window/:windowHandle/position 	|
| POST   	| /session/:sessionId/window/:windowHandle/maximize 	|
| GET    	| /session/:sessionId/window_handle                 	|
| GET    	| /session/:sessionId/window_handles                	|


## Supported Locators to Find UI Elements

Windows Application Driver supports various locators to find UI element in the application session. The table below shows all supported locator strategies with their corresponding UI element attributes shown in **inspect.exe**.

| Client API                   	| Locator Strategy 	| Matched Attribute in inspect.exe       	| Example      	|
|------------------------------	|------------------	|----------------------------------------	|--------------	|
| FindElementByAccessibilityId 	| accessibility id 	| AutomationId                           	| AppNameTitle 	|
| FindElementByClassName       	| class name       	| ClassName                              	| TextBlock    	|
| FindElementById              	| id               	| RuntimeId (decimal)                    	| 42.333896.3.1	|
| FindElementByName            	| name             	| Name                                   	| Calculator   	|
| FindElementByTagName         	| tag name         	| LocalizedControlType (upper camel case)	| Text         	|
| FindElementByXPath           	| xpath            	| Any                                    	| //Button[0]  	|


## Inspecting UI Elements

The latest Microsoft Visual Studio version by default includes the Windows SDK with a great tool to inspect the application you are testing. This tool allows you to see every UI element/node that you can query using Windows Application Driver. This **inspect.exe** tool can be found under the Windows SDK folder which is typically `C:\Program Files (x86)\Windows Kits\10\bin\x86`

More detailed documentation on Inspect is available on MSDN <https://msdn.microsoft.com/library/windows/desktop/dd318521(v=vs.85).aspx>.


## Using Appium

Windows Application Driver is integrated with Appium, meaning if you use Appium as part of the test runner then it will launch `WinAppDriver.exe` and proxy the requests for you.

### Important Notes
1. Appium will install **Windows Application Driver** for you on Windows if you don't already have it.  Every release of Appium is linked to a specific release of WinAppDriver and will not proxy to a different version of WinAppDriver. The easiest way to manage this is to let Appium install WinAppDriver for you.
2. To create multiple sessions with one Appium server you need Appium 1.6.4 or newer
3. When pointing a test at Appium you need to include `/wd/hub` on the server URI. E.g. `http://127.0.0.1:4723/wd/hub`

For more details visit the Appium documentation: <https://appium.io/docs/en/drivers/windows/>


## Vote on New Features
Add your feature request in [issues](../../issues/) or :+1: (+1) existing issues labeled as **Enhancement**

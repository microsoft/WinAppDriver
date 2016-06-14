# Windows Application Driver (Beta)

Windows Application Driver is a service to support UI Test Automation of Windows Applications.  The service design subscribes to the Mobile JSON Wire Protocol standard.  If you've been looking for better support for using <a href="http://appium.io">Appium</a> to test Windows Applications then this service is for you!

This Github project provides
- documentation
- samples
- issue tracking

**Videos about WinAppDriver**<br/>
https://channel9.msdn.com/events/Build/2016/Panel-Engineering-Quality (With Jonathan Lipps!)<br/>
https://channel9.msdn.com/events/Build/2016/P499 (Includes demos)<br/>

## Vote on New Features
Go to https://wpdev.uservoice.com/forums/110705-universal-windows-platform and enter requests under the **UI Testing** category.

## Getting Started
1. Download Windows Application Driver Installer here: http://download.microsoft.com/download/6/8/7/687DEE85-E907-4A95-8035-8BC969B9EA95/WindowsApplicationDriver.msi
2. Run the Installer on the machine where you will run your test in (the application under test should also be installed on this machine)
3. Browse to the Windows Application Driver installation directory and run `WinAppDriver.exe`

When running `WinAppDriver.exe` a console window is opened which logs the JSON Wire Protocol HTTP requests

> Default listening address is 127.0.0.1:4723.  You can configure `WinAppDriver.exe` to listen to a different IP address and port if you run it as administrator.

## C# Samples
1. see Samples/C# in this github project.  Open one of the test solutions with Visual Studio 2015.  For example, pull and open `CalculatorTest.sln` under [CalculatorTest](https://github.com/Microsoft/WinAppDriver/tree/master/Samples/C%23/CalculatorTest)
2. In Visual Studio 2015 with the test solution open build the test and select **Test > Run > All Tests**
 
## Java Samples
1. see Samples/Java in this github project.  Open the sample folder as an existing project in a Java IDE such as IntelliJ. For example: [CalculatorTest](https://github.com/Microsoft/WinAppDriver/tree/master/Samples/Java/CalculatorTest)
2. In the Java IDE build and run the test

## Features
Windows Application Driver supports testing **Universal Windows Platform (UWP)** and **Classic Windows (Win32)** apps on **Windows 10 PC**

## Currently Supported API's

| HTTP   	| Path                                              	|
|--------	|---------------------------------------------------	|
| GET    	| /status                                           	|
| POST   	| /session                                          	|
| DELETE 	| /session/:sessionId                               	|
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
| GET    	| /session/:sessionId/element/:id/location          	|
| GET    	| /session/:sessionId/element/:id/location_in_view  	|
| GET    	| /session/:sessionId/element/:id/name              	|
| GET    	| /session/:sessionId/element/:id/screenshot        	|
| GET    	| /session/:sessionId/element/:id/selected          	|
| GET    	| /session/:sessionId/element/:id/size              	|
| GET    	| /session/:sessionId/element/:id/text              	|
| POST   	| /session/:sessionId/element/:id/value             	|
| POST   	| /session/:sessionId/moveto                        	|
| POST   	| /session/:sessionId/timeouts                      	|
| POST   	| /session/:sessionId/timeouts/implicit_wait        	|
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



## Creating Your Own Test Script
You can choose any programming language or tools supported by Appium/Selenium to write your test scripts. In the example below, we will author the test script in C# using Microsoft Visual Studio 2015.

### Create Test Project
1. Open **Microsoft Visual Studio 2015**
2. Create the test project and solution. I.e. select **New Project > Templates > Visual C# > Test > Unit Test Project**
3. Once created, select **Project > Manage NuGet Packages... > Browse** and search for **Appium.WebDriver**
4. Install the **Appium.WebDriver** NuGet packages for the test project
5. Starts writing your test (see sample code under [samples](https://github.com/Microsoft/WinAppDriver/tree/master/Samples))

### Universal Windows Platform App Testing

To test a UWP app, you can use any Selenium supported language and simply specify the **Application Id** for the app under test in the **app** capabilities entry. Below is an example of creating a test session for Windows **Alarms & Clock** app written in C#:

```c#
// Launch the AlarmClock app
DesiredCapabilities appCapabilities = new DesiredCapabilities();
appCapabilities.SetCapability("app", "Microsoft.WindowsAlarms_8wekyb3d8bbwe!App");
AlarmClockSession = new IOSDriver<IOSElement>(new Uri("http://127.0.0.1:4723"), appCapabilities);

// Control the AlarmClock app
AlarmClockSession.FindElementByAccessibilityId("AddAlarmButton").Click();
AlarmClockSession.FindElementByAccessibilityId("AlarmNameTextBox").Clear();
```

> When testing the application you authored yourself, you can find the **Application Id** in the generetated `AppX\vs.appxrecipe` file under `RegisteredUserNmodeAppID` node. E.g. ```c24c8163-548e-4b84-a466-530178fc0580_scyf5npe3hv32!App```

### Classic Windows App Testing

To test a classic Windows app, you can also use any Selenium supported language and specify the **full executable path** for the app under test in the **app** capabilities entry. Below is an example of creating a test session for Windows **Notepad** app:

```c#
// Launch Notepad
DesiredCapabilities appCapabilities = new DesiredCapabilities();
appCapabilities.SetCapability("app", @"C:\Windows\System32\notepad.exe");
NotepadSession = new IOSDriver<IOSElement>(new Uri("http://127.0.0.1:4723"), appCapabilities);

// Control the AlarmClock app
NotepadSession.FindElementByClassName("Edit").SendKeys("This is some text");
```

### Inspecting UI Elements

Microsoft Visual Studio 2015 by default includes Windows SDK that provides great tool to inspect the application you are testing. This tool allows you to see every UI element/node that you can query using Windows Application Driver. This **inspect.exe** tool can be found under the Windows SDK folder such as `C:\Program Files (x86)\Windows Kits\10\bin\x86`

| Locator Strategy 	| Matched Attribute 	|
|------------------	|-------------------	|
| accessibility id 	| AutomationId      	|
| class name       	| ClassName         	|
| name             	| Name              	|

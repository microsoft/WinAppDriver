---
layout: default
title: Features
---

# Features

Windows Application Driver supports testing **Universal Windows Platofrm (UWP)** and **Classic Windows (Win32)** apps on Windows 10 PC.

## Universal Windows Platform App

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


## Classic Windows App

To test a classic Windows app, you can also use any Selenium supported language and specify the **full executable path** for the app under test in the **app** capabilities entry. Below is an example of creating a test session for Windows **Notepad** app:

```c#
// Launch Notepad
DesiredCapabilities appCapabilities = new DesiredCapabilities();
appCapabilities.SetCapability("app", @"C:\Windows\System32\notepad.exe");
NotepadSession = new IOSDriver<IOSElement>(new Uri("http://127.0.0.1:4723"), appCapabilities);

// Control the AlarmClock app
NotepadSession.FindElementByClassName("Edit").SendKeys("This is some text");
```

## Supported API

| HTTP  | Path                                          	|
|------	|--------------------------------------------------	|
| GET  	| /status                                          	|
| POST 	| /session                                         	|
| POST 	| /session/:sessionId/buttondown                   	|
| POST 	| /session/:sessionId/buttonup                     	|
| POST 	| /session/:sessionId/click                        	|
| POST 	| /session/:sessionId/doubleclick                  	|
| POST 	| /session/:sessionId/element                      	|
| POST 	| /session/:sessionId/elements                     	|
| POST 	| /session/:sessionId/element/active               	|
| GET  	| /session/:sessionId/element/:id/attribute/:name  	|
| POST 	| /session/:sessionId/element/:id/clear            	|
| POST 	| /session/:sessionId/element/:id/click            	|
| GET  	| /session/:sessionId/element/:id/displayed        	|
| GET  	| /session/:sessionId/element/:id/element          	|
| GET  	| /session/:sessionId/element/:id/elements         	|
| GET  	| /session/:sessionId/element/:id/enabled          	|
| GET  	| /session/:sessionId/element/:id/location         	|
| GET  	| /session/:sessionId/element/:id/location_in_view 	|
| GET  	| /session/:sessionId/element/:id/name             	|
| GET  	| /session/:sessionId/element/:id/screenshot       	|
| GET  	| /session/:sessionId/element/:id/selected         	|
| GET  	| /session/:sessionId/element/:id/size             	|
| GET  	| /session/:sessionId/element/:id/text             	|
| POST 	| /session/:sessionId/element/:id/value            	|
| POST 	| /session/:sessionId/moveto                       	|
| POST 	| /session/:sessionId/timeouts                     	|
| POST 	| /session/:sessionId/timeouts/implicit_wait       	|

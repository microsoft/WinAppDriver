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
## Inspecting UI Elements

The latest Microsoft Visual Studio version by default includes the Windows SDK with a great tool to inspect the application you are testing. This tool allows you to see every UI element/node that you can query using Windows Application Driver. This **inspect.exe** tool can be found under the Windows SDK folder which is typically `C:\Program Files (x86)\Windows Kits\10\bin\x86`

More detailed documentation on Inspect is available on MSDN <https://msdn.microsoft.com/library/windows/desktop/dd318521(v=vs.85).aspx>.
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

## Supported Capabilities

Below are the capabilities that can be used to create Windows Application Driver session.

| Capabilities       	| Descriptions                                          	| Example                                               	|
|--------------------	|-------------------------------------------------------	|-------------------------------------------------------	|
| app                	| Application identifier or executable full path        	| Microsoft.MicrosoftEdge_8wekyb3d8bbwe!MicrosoftEdge   	|
| appArguments       	| Application launch arguments                          	| https://github.com/Microsoft/WinAppDriver             	|
| appTopLevelWindow  	| Existing application top level window to attach to    	| `0xB822E2`                                            	|
| appWorkingDir      	| Application working directory (Classic apps only)     	| `C:\Temp`                                             	|
| platformName       	| Target platform name                                  	| Windows                                               	|
| platformVersion    	| Target platform version                               	| 1.0                    
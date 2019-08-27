# Frequently Asked Questions 

## **Getting Started**
#### What are the system requirements for WinAppDriver? 
WinAppDriver is supported on machines running **Windows 10** (Home and Pro) and **Windows Server 2016**.
 
#### What type of applications are supported by WinAppDriver? 
Supports virtually all Windows 10 applications, including **UWP**, **WPF**, **WinForms**, and legacy **Win32**. 

#### How does WinAppDriver relate to Appium? 
Though WinAppDriver can run as a standalone, it can also serve as a plugin for Appium. If the Appium server is already running,  any oncoming requests for Windows 10 app automation will launch WinAppDriver.exe and proxy the requests.

As such, users can leverage WinAppDriver into scenarios or workflows that have already been integrated with Appium. Additional notes on Appium integration can be found on our [readme](https://github.com/Microsoft/WinAppDriver/tree/v1.0#using-appium).

#### Is WinAppDriver open-source?

The WinAppDriver code is **not** currently open-source.  Samples, tests, and related tools are open-source.  We’re investigating the possibility of open-sourcing the WinAppDriver code.
 
#### How can I contribute to WinAppDriver? 
Though WinAppDriver is currently closed source, there are still many ways for users to contribute. If you have any feedback or suggestions to improve WinAppDriver, always feel free to reach out to us at on our [GitHub board](https://github.com/Microsoft/WinAppDriver/issues) or on our [Windows Developer Feedback ](https://wpdev.uservoice.com/forums/110705-dev-platform?category_id=161358) portal. 

Another great way to contribute is by creating your own samples and submitting a pull request to our [samples repository](https://github.com/Microsoft/WinAppDriver/tree/master/Samples). 
 
#### How can I try out WinAppDriver functionality? 
You can download the latest version of WinAppDriver from our GitHub [release page](https://github.com/Microsoft/WinAppDriver/releases), or through Appium's installer. 
 
The best way to get started is to jump straight in to one of the [samples](https://github.com/Microsoft/WinAppDriver/tree/master/Samples) and start playing around. We recommend the  [Calculator Test](https://github.com/Microsoft/WinAppDriver/tree/master/Samples/C%23/CalculatorTest), as it runs through the following competencies: 
- Creating a modern UWP app session
- Finding element using name
- Finding element using accessibility id
- Finding element using XPath
- Sending click action to an element
- Retrieving element value
- Navigating using SplitViewPane
#### What is "inspect.exe"? 
Inspect (Inspect.exe) is a Windows-based tool that enables you select any UI element and view the element's accessibility data. You can read more about Inspect on its [Windows Dev Center](https://msdn.microsoft.com/en-us/library/windows/desktop/dd318521(v=vs.85).aspx) page. 
#### How does "inspect.exe" relate to WinAppDriver? 
Inspect allows users to find and locate element inside an application window. Once a desired element is located and in-focus by Inspect, users can take note of its attribute data, and ultimately refer back to it in testing scripts for WinAppDriver to interact against. 
For example, in the [Calaculator Test](https://github.com/Microsoft/WinAppDriver/tree/master/Samples/C%23/CalculatorTest) sample:
```c++
session.FindElementByXPath("//Button[@AutomationId=\"equalButton\"]").Click();
```
The AutomationID of the "=" can easily be found through using Inspect with the Calculator application, and applied to your WinAppDriver script. 


## **General Development & Best Practices** ##


### When and how to create a Desktop Session

One test session typically corresponds to one app top level window. As long as you have your session alive, you can send input interactions and navigate the app elements tree. On a Windows 10 PC however, an app could trigger external changes such as toast notifications, app tiles, etc. In addition, some apps also respond to external events that can be triggered through the start menu or other sources. Windows Application Driver supports all these scenarios by exposing the entire desktop through a **Root** session that can be created as shown below.

```c#
DesiredCapabilities appCapabilities = new DesiredCapabilities();
appCapabilities.SetCapability("app", "Root");
DesktopSession = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), appCapabilities);

// Use the session to control the desktop
DesktopSession.Keyboard.PressKey(OpenQA.Selenium.Keys.Command + "a" + OpenQA.Selenium.Keys.Command);
```

### When and how to attach to an existing App Window

In some cases, you may want to test applications that are not launched in a conventional way like shown above. For instance, the Cortana application is always running and will not launch a UI window until triggered through **Start Menu** or a keyboard shortcut. In this case, you can create a new session in Windows Application Driver by providing the application top level window handle as a hex string (E.g. `0xB822E2`). This window handle can be retrieved from various methods including the **Desktop Session** mentioned above. This mechanism can also be used for applications that have unusually long startup times. Below is an example of creating a test session for the **Cortana** app after launching the UI using a keyboard shortcut and locating the window using the **Desktop Session**.

```c#
DesktopSession.Keyboard.SendKeys(Keys.Meta + "s" + Keys.Meta);

var CortanaWindow = DesktopSession.FindElementByName("Cortana");
var CortanaTopLevelWindowHandle = CortanaWindow.GetAttribute("NativeWindowHandle");
CortanaTopLevelWindowHandle = (int.Parse(CortanaTopLevelWindowHandle)).ToString("x"); // Convert to Hex

// Create session by attaching to Cortana top level window
DesiredCapabilities appCapabilities = new DesiredCapabilities();
appCapabilities.SetCapability("appTopLevelWindow", CortanaTopLevelWindowHandle);
CortanaSession = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);

// Use the session to control Cortana
CortanaSession.FindElementByAccessibilityId("SearchTextBox").SendKeys("add");
```
### What to do when an application has a splash screen or WinAppDriver fails to recognize the correct window?
This could be due to a variety of reasons. Assuming application capabilities have been declared correctly, one possible reason for WinAppDriver failing to recognize the application window could be due to the application having splash screens. WinAppDriver can often mistake a splash screen as the main UI window. As a result,  as soon as the splash screen vanishes, further operations will result in an error. “NoSuchWindowException” is a common response that happens as soon as the splash screen times out. 
**WinAppDriver Logs**:
```sh
{"status":23,"value":{"error":"no such window","message":"Currently selected window has been closed"}}
```
**Common Error Codes:**
```java
org.openqa.selenium.NoSuchWindowException: Currently selected window has been closed (WARNING: The server did not provide any stacktrace information)
```
**Examples of this Issue:**
105, 213

***Resolution***

You can use a fixed delay. Below is the snippet of the workaround found in issue-ticket 213 that will work if the application under test enforce single instance and will simply bring up already launched instance when being re-launched.
```c#
DesiredCapabilities appCapabilities = new DesiredCapabilities();
appCapabilities.SetCapability("app", YourAppId);
WindowsDriver<WindowsElement> session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
Assert.IsNotNull(session);
// Wait for 5 seconds or however long it is needed for the right window to appear/for the splash screen to be dismissed
Thread.Sleep(TimeSpan.FromSeconds(5));
// When the application uses pre-launched existing instance, re-launching the application simply update 
// the current application window to whatever current main window belonging to the same application 
// process id
session.LaunchApp();
```
You can use SwitchTo() API to switch to the right main window once the correct main window is displayed. Below is a sample on how to switch window if your session is pointing to a wrong window such as the splash screen:
```c#
DesiredCapabilities appCapabilities = new DesiredCapabilities();
appCapabilities.SetCapability("app", YourAppId);
WindowsDriver<WindowsElement> session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
Assert.IsNotNull(session);
// Identify the current window handle. You can check through inspect.exe which window this is.
var currentWindowHandle = session.CurrentWindowHandle;
// Wait for 5 seconds or however long it is needed for the right window to appear/for the splash screen to be dismissed
Thread.Sleep(TimeSpan.FromSeconds(5));
// Return all window handles associated with this process/application.
// At this point hopefully you have one to pick from. Otherwise you can
// simply iterate through them to identify the one you want.
var allWindowHandles = session.WindowHandles;
// Assuming you only have only one window entry in allWindowHandles and it is in fact the correct one,
// switch the session to that window as follows. You can repeat this logic with any top window with the same
// process id (any entry of allWindowHandles)
session.SwitchTo().Window(allWindowHandles[0]);
```
### How to adjust the command timeout (the default duration of each command before a test fails)
To change the command timeout, you can call on an overridden constructor when creating a session. Below is an example in C# using AppiumDotNetDriver binding that specifies a 10-minute command timeout:
```c#
WindowsDriver<WindowsElement> session = new WindowsDriver<WindowsElement>(new Uri("http://127.0.0.1:4723"), appCapabilities, TimeSpan.FromMinutes(10));
```
### How to handle dynamically generated content  (e.g. interacting with the Scrollbar)
The Alarm Clock Test serves as a good reference for this, and demonstrates two possible scenarios for handling elements in a list.

**Scenario 1 - element pre-generated**  
As per the spec, we implicitly scroll elements within the view when they are selected--however this relies on the UI element being generated and accessible from the start.
The Alarm Scenario test serves as a good reference for this. From the code snippet here:
```C#
minuteSelector.FindElementByName("55").Click();`	
```
The AlarmClock test automatically scrolls to the '55' element and selects it - as can be seen when the alarm GUI is set to 3:55 when the test is running.

**Scenario 2 - element dynamically generated**
There exists the possibility of cases where the element UI is not generated until certain conditions are met (e.g. a list has to scroll to it's location first before element is generated). This will require automating in the steps to fulfill those requirements first before the element can be interacted with.

The Stopwatch Scenario serves as a good example for handling dynamically generated elements. From the code snippet here:
```c#
//the test scrolls to the location of the element before taking action on the desired element.
touchScreen.Scroll(lapListView.Coordinates, 0, -50); 
````
### Running on a Remote Machine

Windows Application Driver can run remotely on any Windows 10 machine with `WinAppDriver.exe` installed and running. This *test machine* can then serve any JSON wire protocol commands coming from the *test runner* remotely through the network. Below are the steps to the one-time setup for the *test machine* to receive inbound requests:

1. On the *test machine* you want to run the test application on, open up **Windows Firewall with Advanced Security**
   - Select **Inbound Rules** -> **New Rule...**
   - **Rule Type** -> **Port**
   - Select **TCP**
   - Choose specific local port (4723 is WinAppDriver standard)
   - **Action** -> **Allow the connection**
   - **Profile** -> select all
   - **Name** -> optional, choose name for rule (e.g. WinAppDriver remote).
   
   Below command when run in admin command prompt gives same result
   ```shell
   netsh advfirewall firewall add rule name="WinAppDriver remote" dir=in action=allow protocol=TCP localport=4723
   ```
   
2. Run `ipconfig.exe` to determine your machine's local IP address
   > **Note**: Setting `*` as the IP address command line option will cause it to bind to all bound IP addresses on the machine
3. Run `WinAppDriver.exe 10.X.X.10 4723/wd/hub` as **administrator** with command line arguments as seen above specifying local IP and port
4. On the *test runner* machine where the runner and scripts are, update the test script to point to the IP of the remote *test machine* 

Sample Java Example:
```c#
DesiredCapabilities capabilities = new DesiredCapabilities();
capabilities.setCapability("app", "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
CalculatorSession = (WindowsDriver)(new WindowsDriver(new URL("http://10.X.X.52:4723/wd/hub"), capabilities));
CalculatorSession.manage().timeouts().implicitlyWait(2, TimeUnit.SECONDS);
CalculatorResult = CalculatorSession.findElementByAccessibilityId("CalculatorResults");
 ```

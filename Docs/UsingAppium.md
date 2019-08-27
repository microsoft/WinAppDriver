## You can use WinAppDriver by itself or with Appium
When considering which approach will work best for you it helps to know more about Appium, the goals of the WinAppDriver project, and some considerations when setting up your test workflow.

###  Appium
* [Appium](http://appium.io/) is the industry leading test automation framework for mobile applications
* Is [open source](https://github.com/appium)
* Supports the WebDriver protocol used by Selenium
* Approaches testing with a focus on mobile apps

### WinAppDriver 
* Created to bring Windows app testing to Appium
* Can run standalone and does not require using Appium

### Regardless of what you choose, you'll be using exact same WinAppDriver
* The exact same build of WinAppDriver is driving tests for Appium (The Appium installer installs WinAppDriver for you on Windows Desktop).  This is not a special build of WinAppDriver - it is the same WinAppDriver you use without Appium.
* When Appium receives test commands for Windows, it passes the commands along to WinAppDriver

## Here are some tips to help decide which route is best for you

### Differences in the flow of information
* When using Appium in the mobile or web world:<br/>
Test Runner >> Appium Server >> mobile/web app

* When using Appium and testing on Windows:<br/>
Test Runner >> Appium Server >> WinAppDriver >> Windows application

* When testing Windows apps and not using Appium we just take out the Appium Server:<br/>
Test Runner >> WinAppDriver >> Windows application

### The Appium default server path is different from WinAppDriver
Appium: http://127.0.0.1:4723/wd/hub<br/>
WinAppDriver: http://127.0.0.1:4723<br/>

For consistency you can tell WinAppDriver to listen to /wd/hub by passing startup arguments:<br/>
`winappdriver.exe 127.0.0.1 4723/wd/hub`

### Appium provides the [Selenium Grid](https://github.com/Microsoft/WinAppDriver/wiki/Working-with-the-Selenium-Grid)
If you're looking for multi device management, take a look at the Selenium Grid and plan on using Appium to test your Windows devices.  Appium supports the Selenium Grid with Windows devices.

### Appium tests iOS, Android and MacOS apps
If you're running tests for multiple devices, platforms, including browser tests, you should consider using Appium.  Appium can make it easier to manage tests across multiple devices.

### If you're just testing Windows Apps
If you're just testing Windows Apps it's fine to not include Appium

### It's easy to switch back and forth
Don't feel like this is a big decision you can't later change - you can experiment, try with and without Appium, and change later if your needs change.  Switching between WinAppDriver standalone and using Appium can be near 0 cost, with the bulk of the work dealing with the "wd/hub" difference.

### Additional Reading
Scott Hanselman's blog post [here](https://www.hanselman.com/blog/WinAppDriverTestAnyAppWithAppiumsSeleniumlikeTestsOnWindows.aspx)

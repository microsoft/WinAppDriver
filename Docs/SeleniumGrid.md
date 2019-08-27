# Using Windows devices with the [Selenium Grid](https://www.seleniumhq.org/docs/07_selenium_grid)
Below is an example on how to enable registering Windows devices with Appium and the Selenium Grid.  Additional info is provided on the [Appium documentation](https://appium.io/docs/en/advanced-concepts/grid/)

## Setup a Windows test machine as a test Node
1. Install Java Development Kit
1. Add to the Environment Variables JAVA_HOME and set it to the base path of your installation, for example "C:\Program Files\Java\jdk1.8.0_121"
1. Add to the Path environment variable the folder where "Jar.exe" is located, for example, you'd add "C:\Program Files\Java\jdk1.8.0_121\bin" to the path
1. Download latest node and npm tools MSI (version >= 6.0) and install it. 
1. Verify that the npm and nodejs paths are in your PATH environment variable. If not, add them manually to the PATH, should be: %APPDATA%\npm;C:\Program Files\nodejs\
1. Open a Command Prompt in Administrative mode 
1. Run the command "npm install appium"
This will install Appium from NPM

## Connecting to a Selenium Grid
### Run the Selenium Hub
1. Download latest Selenium Jar
1. Launch a command line
1. Run "java -jar selenium-server-standalone-3.0.1.jar -role hub -port 4444"

### Node machine setup
1. Edit a JSon configuration file for Appium to let Appium know where the Selenium grid is located. See [here](http://qaautomationworld.blogspot.in/2014/11/appium-remote-execution-grid-execution.html) for more information. Look at the rows in **bold**, those indicate where the Selenium hub is located. The JSon looks something like this
```
	{
	  "capabilities":
	      [
	        {
	          "browserName": "",
	          "version":"",
	          "maxInstances": 3,
	          "platform":"WINDOWS"
	        }
	      ],
	  "configuration":
	  {
		"cleanUpCycle":2000, 
		"timeout":30000, 
		"proxy": "org.openqa.grid.selenium.proxy.DefaultRemoteProxy",
		"url":"http://127.0.0.1:4723/wd/hub",
		"host":"127.0.0.1",
		"port":4723, 
		"maxSession":3,
		"register":true,
		"registerCycle":10000,
		**"hub": "127.0.0.1:4444/grid/register",
		"hubPort":4444,
		"hubHost":"127.0.0.1",**
		"nodeTimeout":120, 
		"nodePolling":2000
	  }
	}
```
2. Launch a Command line
1. Run this, assuming AppiumNode.json is the config file you created
`node appium --nodeconfig AppiumNode.json -p 4723`



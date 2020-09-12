### Running on a Remote Machine

Windows Application Driver can run remotely on any Windows 10 machine with `WinAppDriver.exe` installed and running. This *test machine* can then serve any JSON wire protocol commands coming from the *test runner* remotely through the network. Below are the steps to the one-time setup for the *test machine* to receive inbound requests:

1. On the *remote test machine* you want to run the test application on, open up **Windows Firewall with Advanced Security**
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
   
2. In the *remote test machine*, run `ipconfig.exe` to determine the machine's local IP address
   > **Note**: Setting `*` as the IP address command line option will cause it to bind to all bound IP addresses on the machine
3. In the *remote test machine*, run `WinAppDriver.exe 10.X.X.10 4723/wd/hub` as **administrator** with command line arguments as seen above, changing the IP and Port to the specified IP and Port set in Steep 1.
4. On the *test runner* machine where the runner and scripts are, update the test script to point to the IP of the remote *test machine* 

Sample Java Example:
```c#
DesiredCapabilities capabilities = new DesiredCapabilities();
capabilities.setCapability("app", "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
CalculatorSession = (WindowsDriver)(new WindowsDriver(new URL("http://10.X.X.10:4723/wd/hub"), capabilities));
CalculatorSession.manage().timeouts().implicitlyWait(2, TimeUnit.SECONDS);
CalculatorResult = CalculatorSession.findElementByAccessibilityId("CalculatorResults");
 ```

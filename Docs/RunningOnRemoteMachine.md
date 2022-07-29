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

5. WinAppDriver requires the WinAppDriver server to be listening in order to perform the requisite UI interactions. While Appium generally takes care of this requirement on the local machine, running on a server/remote machine requires a few extra steps as WinAppDriver requires an interactive desktop session. Therefore, we recommend the following (community-contributed) setup steps on the remote machine:
   1. Setup a batch file to terminate any old instances of WinAppDriver
      - **Name**: `kill_winappdriver.cmd`
      - **Contents**: `taskkill /im WinAppDriver.exe /f`
   2. Setup a batch file to start WinAppDriver
      - **Name** : `LaunchWAD.cmd`
      - **Contents** : `cmd start /K "C:/Program Files (x86)/Windows Application Driver/WinAppDriver.exe" 10.x.xx.xx 4723/wd/hub`
      - **Note**: The IP address above (10.x.xx.xx) should be replaced with the local network IP address of the server.
   3. Setup a batch file to log off the remote session without disconnecting: 
      - **Name** : `logout-rdp.cmd`
      - **Contents**: `for /f "skip=1 tokens=3" %%s in ('query user %USERNAME%') do (%windir%\System32\tscon.exe %%s /dest:console C:\Install\QRes.exe /x 1920 /y 1080)`
      - **Notes**
         - [Qres](http://qres.sourceforge.net/) is a remote VNC tool that can change the resolution of the target machine to match the local machine
         - When using Remote Desktop to connect to a remote computer, closing Remote Desktop locks out the computer and displays the login screen. While the machine is locked, the interactive session is paused. Any currently running or scheduled GUI tests will fail.
         - To avoid this, we use the `tscon` utility to disconnect from Remote Desktop. `tscon` returns control to the original local session on the remote computer, bypassing the logon screen. All programs on the remote computer continue running normally, including GUI tests. `logout-rdp.cmd` should be used exclusively to logout from the remote machine, and the admin user should not logout/disconnect manually. You may configure the resolution of the remote machine in the above batch file.
   4. Set up two *Scheduled Tasks* on the target machine to terminate WinAppDriver (script from step 5.1) and to start WinAppDriver (script from step 5.2) using the earlier scripts as the target programs.
      - Ideally, the Triggers should be *Daily* and *Startup* so that your Continuous Integration build agents will always have an instance of WinAppDriver running.
      - These Scheduled Tasks should be set up to run with highest privileges on the machine (as WinAppDriver requires Admin privileges.)
   5. Some remote machines or server instances can have a screen lock policy, preventing WinAppDriver from interacting with GUI elements. This can be handled via either updating the policy or by preventing sleep with this small VBScript snippet:

```vbscript
Dim objResult
Set objShell = WScript.CreateObject("WScript.Shell")    
Do While True
  objResult = objShell.sendkeys("{NUMLOCK}{NUMLOCK}")
  Wscript.Sleep (6000)
Loop
```

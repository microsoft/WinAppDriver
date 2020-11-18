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
 5.WinAppDriver requires the Winappdriver server to be listening on the local host or IP address in order to perform the requisite UI interactions. While Appium generally takes care of this requirement on the local , for running on a server/remote machine, we need to perform a few extra steps as Winappdriver requires a GUI Output and the Winappdriver server running in an interactive shell. 
Therefore we need to follow the below steps on the remote machine for the Windows GUI tests to run in an autonomous fashion :
- **5.1**.	Setup Batch file to kill any old instances of WinAppDriver - This will be used in the KILLWAD scheduled task later 
-	*Name* : kill_winappdriver.bat
-	*Contents* : `taskkill /im WinAppDriver.exe /f`
- **5.2**.	Setup Batch file to start WinAppdriver  - This will be used in the StartWAD scheduled task later 
-	*Name* : LaunchWAD.bat
-	*Contents* : `cmd start /K "C:/Program Files (x86)/Windows Application Driver/WinAppDriver.exe" 10.x.xx.xx 4723/wd/hub`
-	*Note*: The IP address above (10.x.xx.xx) should be replaced with the local IP address of the server/remote machine
- **5.3**.	Setup Batch file to logout (without disconnecting) from the remote machine : 
-	*Name* : logout-rdp.bat
-	*Contents*: `for /f "skip=1 tokens=3" %%s in ('query user %USERNAME%') do (%windir%\System32\tscon.exe %%s /dest:console C:\Install\QRes.exe /x 1920 /y 1080)`
-	*Note*: When using Remote Desktop to connect to a remote computer, closing Remote Desktop locks out the computer and displays the login screen. In the locked mode, the computer does not have GUI, so any currently running or scheduled GUI tests will fail.
To avoid problems with GUI tests, we use the tscon utility to disconnect from Remote Desktop. tscon returns the control to the original local session on the remote computer, bypassing the logon screen. All programs on the remote computer continue running normally, including GUI tests. Therefore logout-rdp.bat should be exclusively used to logout from the remote machine and the admin user should not logout/disconnect manually from the remote . Also, the resolution is passed as a parameter in the above batch file as 1920x1080
- **5.4** . Setup *Scheduled Tasks* on the target machine to kill Winappdriver (as per the BAT file in 5.1) and to start Winappdriver (as per the BAT file in 5.2) as the target programs. Ideally the Triggers should be *Daily* and *Startup*  , so that the scripts running via the Test runner (Ex: JENKINS) , always have an instance of Winappdriver running on the server.  These Scheduled Tasks should be setup to run with highest privileges on the machine (as Winappdriver requires to be run with Admin rights)
- **5.5** : Some remote machines or server instances can have a screen lock policy, preventing Winappdriver from interacting with GUI elements. This can be handled via either updating the policy or running a small script as follows :
`Dim objResult
Set objShell = WScript.CreateObject("WScript.Shell")    
Do While True
  objResult = objShell.sendkeys("{NUMLOCK}{NUMLOCK}")
  Wscript.Sleep (6000)
Loop`

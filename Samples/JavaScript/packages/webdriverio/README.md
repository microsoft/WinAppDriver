## Example with webdriverio
Make sure [Dependencies](../../README.md)

### By launching WinAppDriver automatically
1. Launch the test
```
yarn run testappium
```

### By launching WinAppDriver manually
1. Start WinAppDriver and listen on 4723/wd/hub
```
cd C:\Program Files (x86)\Windows Application Driver
c:
WinAppDriver.exe 127.0.0.1 4723/wd/hub
```
And the output is like this:
```
Windows Application Driver listening for requests at: http://127.0.0.1:4723/wd/hub
Press ENTER to exit.
```

or
```
yarn run appium
```
2. Launch the test
```
yarn run test
```
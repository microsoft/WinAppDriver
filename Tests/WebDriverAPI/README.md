# WebDriverAPI

WebDriverAPI is a collection of test scenarios that covers every WebDriver command supported by Windows Application Driver. Use this as a reference on how to invoke each command in the test script. While these tests are written in C#, corresponding commands in other Selenium supported language bindings should map to the same API endpoint.

The test scenarios are written against Windows 10 built-in apps such as **Calculator**, **Alarms & Clock**, **Notepad**, **File Explorer**, and **Microsoft Edge Browser**. Therefore these tests can simply be run on Windows 10 PC running Windows Application Driver.


## Requirements

- Windows 10 PC with the latest Windows 10 version (Version 1607 or later)
- Microsoft Visual Studio 2015 or later


## Getting Started

1. Open `WebDriverAPI.sln` in Visual Studio
2. Select **Test** > **Windows** > **Test Explorer**
3. Select **Run All** on the test pane or through menu **Test** > **Run** > **All Tests**

> Once the project is successfully built, you can use the **TestExplorer** to pick and choose the test scenario(s) to run


## Command Summary

| HTTP   	| Path                                                                              	|
|--------	|-----------------------------------------------------------------------------------	|
| GET    	| [/status                                           ](./Status.cs)                 	|
| POST   	| [/session                                          ](./Session.cs)                	|
| GET    	| [/sessions                                         ](./Sessions.cs)               	|
| DELETE 	| [/session/:sessionId                               ](./Session.cs)                	|
| POST   	| [/session/:sessionId/appium/app/launch             ](./AppiumAppClose.cs)         	|
| POST   	| [/session/:sessionId/appium/app/close              ](./AppiumAppLaunch.cs)        	|
| POST   	| [/session/:sessionId/back                          ](./Back.cs)                   	|
| POST   	| [/session/:sessionId/buttondown                    ](./Mouse.cs)                  	|
| POST   	| [/session/:sessionId/buttonup                      ](./Mouse.cs)                  	|
| POST   	| [/session/:sessionId/click                         ](./Mouse.cs)                  	|
| POST   	| [/session/:sessionId/doubleclick                   ](./Mouse.cs)                  	|
| POST   	| [/session/:sessionId/element                       ](./Element.cs)                	|
| POST   	| [/session/:sessionId/elements                      ](./Elements.cs)               	|
| POST   	| [/session/:sessionId/element/active                ](./ElementActive.cs)          	|
| GET    	| [/session/:sessionId/element/:id/attribute/:name   ](./ElementAttribute.cs)       	|
| POST   	| [/session/:sessionId/element/:id/clear             ](./ElementClear.cs)           	|
| POST   	| [/session/:sessionId/element/:id/click             ](./ElementClick.cs)           	|
| GET    	| [/session/:sessionId/element/:id/displayed         ](./ElementDisplayed.cs)       	|
| GET    	| [/session/:sessionId/element/:id/element           ](./ElementElement.cs)         	|
| GET    	| [/session/:sessionId/element/:id/elements          ](./ElementElements.cs)        	|
| GET    	| [/session/:sessionId/element/:id/enabled           ](./ElementEnabled.cs)         	|
| GET    	| [/session/:sessionId/element/:id/equals            ](./ElementEquals.cs)          	|
| GET    	| [/session/:sessionId/element/:id/location          ](./ElementLocation.cs)        	|
| GET    	| [/session/:sessionId/element/:id/location_in_view  ](./ElementLocationInView.cs)  	|
| GET    	| [/session/:sessionId/element/:id/name              ](./ElementName.cs)            	|
| GET    	| [/session/:sessionId/element/:id/screenshot        ](./Screenshot.cs)             	|
| GET    	| [/session/:sessionId/element/:id/selected          ](./ElementSelected.cs)        	|
| GET    	| [/session/:sessionId/element/:id/size              ](./ElementSize.cs)            	|
| GET    	| [/session/:sessionId/element/:id/text              ](./ElementText.cs)            	|
| POST   	| [/session/:sessionId/element/:id/value             ](./ElementSendKeys.cs)        	|
| POST   	| [/session/:sessionId/forward                       ](./Forward.cs)                	|
| POST   	| [/session/:sessionId/keys                          ](./SendKeys.cs)               	|
| GET    	| [/session/:sessionId/location                      ](./Location.cs)               	|
| POST   	| [/session/:sessionId/moveto                        ](./Mouse.cs)                  	|
| GET    	| [/session/:sessionId/orientation                   ](./Orientation.cs)            	|
| GET    	| [/session/:sessionId/screenshot                    ](./Screenshot.cs)             	|
| GET    	| [/session/:sessionId/source                        ](./Source.cs)                 	|
| POST   	| [/session/:sessionId/timeouts                      ](./Timeouts.cs)               	|
| GET    	| [/session/:sessionId/title                         ](./Title.cs)                  	|
| POST   	| [/session/:sessionId/touch/click                   ](./TouchClick.cs)             	|
| POST   	| [/session/:sessionId/touch/doubleclick             ](./TouchDoubleClick.cs)       	|
| POST   	| [/session/:sessionId/touch/down                    ](./TouchDownMoveUp.cs)        	|
| POST   	| [/session/:sessionId/touch/flick                   ](./TouchFlick.cs)             	|
| POST   	| [/session/:sessionId/touch/longclick               ](./TouchLongClick.cs)         	|
| POST   	| [/session/:sessionId/touch/move                    ](./TouchDownMoveUp.cs)        	|
| POST   	| [/session/:sessionId/touch/scroll                  ](./TouchScroll.cs)            	|
| POST   	| [/session/:sessionId/touch/up                      ](./TouchDownMoveUp.cs)        	|
| DELETE 	| [/session/:sessionId/window                        ](./Window.cs)                 	|
| POST   	| [/session/:sessionId/window                        ](./Window.cs)                 	|
| POST   	| [/session/:sessionId/window/maximize               ](./Window.cs)                 	|
| POST   	| [/session/:sessionId/window/size                   ](./Window.cs)                 	|
| GET    	| [/session/:sessionId/window/size                   ](./Window.cs)                 	|
| POST   	| [/session/:sessionId/window/:windowHandle/size     ](./Window.cs)                 	|
| GET    	| [/session/:sessionId/window/:windowHandle/size     ](./Window.cs)                 	|
| POST   	| [/session/:sessionId/window/:windowHandle/position ](./Window.cs)                 	|
| GET    	| [/session/:sessionId/window/:windowHandle/position ](./Window.cs)                 	|
| POST   	| [/session/:sessionId/window/:windowHandle/maximize ](./Window.cs)                 	|
| GET    	| [/session/:sessionId/window_handle                 ](./Window.cs)                 	|
| GET    	| [/session/:sessionId/window_handles                ](./Window.cs)                 	|


## Command Reference

These tests are written to verify each API endpoint behavior and error values as specified in [JSON Wire Protocol](https://github.com/SeleniumHQ/selenium/wiki/JsonWireProtocol) document.


## Adding/Updating Test Scenario

Please follow the guidelines below to maintain test reliability and reduce test execution time:
1. Avoid duplicating test scenario that uses the same code path
2. Aim for simple and reliable scenario using the least amount of test steps
3. Write test against Windows 10 built-in apps only to avoid requiring extra installation
4. Reuse existing application session when possible to reduce unnecessary application re-launching
5. Group test methods accordingly by naming them with the existing convention. E.g.
   - `ClickElement`
   - `ClickElementError_NoSuchWindow`
   - `ClickElementError_StaleElement`
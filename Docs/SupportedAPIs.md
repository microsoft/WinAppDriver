## Command Summary

The following is a list of APIs supported by WinAppDriver:

| HTTP   	| Path                                                                              	|
|--------	|-----------------------------------------------------------------------------------	|
| GET    	| [/status                                           ](./Tests/Status.cs)                 	|
| POST   	| [/session                                          ](./Tests/Session.cs)                	|
| GET    	| [/sessions                                         ](./Tests/Sessions.cs)               	|
| DELETE 	| [/session/:sessionId                               ](./Tests/Session.cs)                	|
| POST   	| [/session/:sessionId/appium/app/launch             ](./Tests/AppiumAppClose.cs)         	|
| POST   	| [/session/:sessionId/appium/app/close              ](./Tests/AppiumAppLaunch.cs)        	|
| POST   	| [/session/:sessionId/back                          ](./Tests/Back.cs)                   	|
| POST   	| [/session/:sessionId/buttondown                    ](./Tests/Mouse.cs)                  	|
| POST   	| [/session/:sessionId/buttonup                      ](./Tests/Mouse.cs)                  	|
| POST   	| [/session/:sessionId/click                         ](./Tests/Mouse.cs)                  	|
| POST   	| [/session/:sessionId/doubleclick                   ](./Tests/Mouse.cs)                  	|
| POST   	| [/session/:sessionId/element                       ](./Tests/Element.cs)                	|
| POST   	| [/session/:sessionId/elements                      ](./Tests/Elements.cs)               	|
| POST   	| [/session/:sessionId/element/active                ](./Tests/ElementActive.cs)          	|
| GET    	| [/session/:sessionId/element/:id/attribute/:name   ](./Tests/ElementAttribute.cs)       	|
| POST   	| [/session/:sessionId/element/:id/clear             ](./Tests/ElementClear.cs)           	|
| POST   	| [/session/:sessionId/element/:id/click             ](./Tests/ElementClick.cs)           	|
| GET    	| [/session/:sessionId/element/:id/displayed         ](./Tests/ElementDisplayed.cs)       	|
| GET    	| [/session/:sessionId/element/:id/element           ](./Tests/ElementElement.cs)         	|
| GET    	| [/session/:sessionId/element/:id/elements          ](./Tests/ElementElements.cs)        	|
| GET    	| [/session/:sessionId/element/:id/enabled           ](./Tests/ElementEnabled.cs)         	|
| GET    	| [/session/:sessionId/element/:id/equals            ](./Tests/ElementEquals.cs)          	|
| GET    	| [/session/:sessionId/element/:id/location          ](./Tests/ElementLocation.cs)        	|
| GET    	| [/session/:sessionId/element/:id/location_in_view  ](./Tests/ElementLocationInView.cs)  	|
| GET    	| [/session/:sessionId/element/:id/name              ](./Tests/ElementName.cs)            	|
| GET    	| [/session/:sessionId/element/:id/screenshot        ](./Tests/Screenshot.cs)             	|
| GET    	| [/session/:sessionId/element/:id/selected          ](./Tests/ElementSelected.cs)        	|
| GET    	| [/session/:sessionId/element/:id/size              ](./Tests/ElementSize.cs)            	|
| GET    	| [/session/:sessionId/element/:id/text              ](./Tests/ElementText.cs)            	|
| POST   	| [/session/:sessionId/element/:id/value             ](./Tests/ElementSendKeys.cs)        	|
| POST   	| [/session/:sessionId/forward                       ](./Tests/Forward.cs)                	|
| POST   	| [/session/:sessionId/keys                          ](./Tests/SendKeys.cs)               	|
| GET    	| [/session/:sessionId/location                      ](./Tests/Location.cs)               	|
| POST   	| [/session/:sessionId/moveto                        ](./Tests/Mouse.cs)                  	|
| GET    	| [/session/:sessionId/orientation                   ](./Tests/Orientation.cs)            	|
| GET    	| [/session/:sessionId/screenshot                    ](./Tests/Screenshot.cs)             	|
| GET    	| [/session/:sessionId/source                        ](./Tests/Source.cs)                 	|
| POST   	| [/session/:sessionId/timeouts                      ](./Tests/Timeouts.cs)               	|
| GET    	| [/session/:sessionId/title                         ](./Tests/Title.cs)                  	|
| POST   	| [/session/:sessionId/touch/click                   ](./Tests/TouchClick.cs)             	|
| POST   	| [/session/:sessionId/touch/doubleclick             ](./Tests/TouchDoubleClick.cs)       	|
| POST   	| [/session/:sessionId/touch/down                    ](./Tests/TouchDownMoveUp.cs)        	|
| POST   	| [/session/:sessionId/touch/flick                   ](./Tests/TouchFlick.cs)             	|
| POST   	| [/session/:sessionId/touch/longclick               ](./Tests/TouchLongClick.cs)         	|
| POST   	| [/session/:sessionId/touch/move                    ](./Tests/TouchDownMoveUp.cs)        	|
| POST   	| [/session/:sessionId/touch/scroll                  ](./Tests/TouchScroll.cs)            	|
| POST   	| [/session/:sessionId/touch/up                      ](./Tests/TouchDownMoveUp.cs)        	|
| DELETE 	| [/session/:sessionId/window                        ](./Tests/Window.cs)                 	|
| POST   	| [/session/:sessionId/window                        ](./Tests/Window.cs)                 	|
| POST   	| [/session/:sessionId/window/maximize               ](./Tests/Window.cs)                 	|
| POST   	| [/session/:sessionId/window/size                   ](./Tests/Window.cs)                 	|
| GET    	| [/session/:sessionId/window/size                   ](./Tests/Window.cs)                 	|
| POST   	| [/session/:sessionId/window/:windowHandle/size     ](./Tests/Window.cs)                 	|
| GET    	| [/session/:sessionId/window/:windowHandle/size     ](./Tests/Window.cs)                 	|
| POST   	| [/session/:sessionId/window/:windowHandle/position ](./Tests/Window.cs)                 	|
| GET    	| [/session/:sessionId/window/:windowHandle/position ](./Tests/Window.cs)                 	|
| POST   	| [/session/:sessionId/window/:windowHandle/maximize ](./Tests/Window.cs)                 	|
| GET    	| [/session/:sessionId/window_handle                 ](./Tests/Window.cs)                 	|
| GET    	| [/session/:sessionId/window_handles                ](./Tests/Window.cs)                 	|

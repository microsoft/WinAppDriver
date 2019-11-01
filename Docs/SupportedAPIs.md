## Command Summary

The following is a list of APIs supported by WinAppDriver:

| HTTP   	| Path                                                                              	|
|--------	|-----------------------------------------------------------------------------------	|
| GET    	| [/status                                           ](./../Tests/WebDriverAPI/Status.cs)                 	|
| POST   	| [/session                                          ](./../Tests/WebDriverAPI/Session.cs)                	|
| GET    	| [/sessions                                         ](./../Tests/WebDriverAPI/Sessions.cs)               	|
| DELETE 	| [/session/:sessionId                               ](./../Tests/WebDriverAPI/Session.cs)                	|
| POST   	| [/session/:sessionId/appium/app/launch             ](./../Tests/WebDriverAPI/AppiumAppClose.cs)         	|
| POST   	| [/session/:sessionId/appium/app/close              ](./../Tests/WebDriverAPI/AppiumAppLaunch.cs)        	|
| POST   	| [/session/:sessionId/back                          ](./../Tests/WebDriverAPI/Back.cs)                   	|
| POST   	| [/session/:sessionId/buttondown                    ](./../Tests/WebDriverAPI/Mouse.cs)                  	|
| POST   	| [/session/:sessionId/buttonup                      ](./../Tests/WebDriverAPI/Mouse.cs)                  	|
| POST   	| [/session/:sessionId/click                         ](./../Tests/WebDriverAPI/Mouse.cs)                  	|
| POST   	| [/session/:sessionId/doubleclick                   ](./../Tests/WebDriverAPI/Mouse.cs)                  	|
| POST   	| [/session/:sessionId/element                       ](./../Tests/WebDriverAPI/Element.cs)                	|
| POST   	| [/session/:sessionId/elements                      ](./../Tests/WebDriverAPI/Elements.cs)               	|
| POST   	| [/session/:sessionId/element/active                ](./../Tests/WebDriverAPI/ElementActive.cs)          	|
| GET    	| [/session/:sessionId/element/:id/attribute/:name   ](./../Tests/WebDriverAPI/ElementAttribute.cs)       	|
| POST   	| [/session/:sessionId/element/:id/clear             ](./../Tests/WebDriverAPI/ElementClear.cs)           	|
| POST   	| [/session/:sessionId/element/:id/click             ](./../Tests/WebDriverAPI/ElementClick.cs)           	|
| GET    	| [/session/:sessionId/element/:id/displayed         ](./../Tests/WebDriverAPI/ElementDisplayed.cs)       	|
| GET    	| [/session/:sessionId/element/:id/element           ](./../Tests/WebDriverAPI/ElementElement.cs)         	|
| GET    	| [/session/:sessionId/element/:id/elements          ](./../Tests/WebDriverAPI/ElementElements.cs)        	|
| GET    	| [/session/:sessionId/element/:id/enabled           ](./../Tests/WebDriverAPI/ElementEnabled.cs)         	|
| GET    	| [/session/:sessionId/element/:id/equals            ](./../Tests/WebDriverAPI/ElementEquals.cs)          	|
| GET    	| [/session/:sessionId/element/:id/location          ](./../Tests/WebDriverAPI/ElementLocation.cs)        	|
| GET    	| [/session/:sessionId/element/:id/location_in_view  ](./../Tests/WebDriverAPI/ElementLocationInView.cs)  	|
| GET    	| [/session/:sessionId/element/:id/name              ](./../Tests/WebDriverAPI/ElementName.cs)            	|
| GET    	| [/session/:sessionId/element/:id/screenshot        ](./../Tests/WebDriverAPI/Screenshot.cs)             	|
| GET    	| [/session/:sessionId/element/:id/selected          ](./../Tests/WebDriverAPI/ElementSelected.cs)        	|
| GET    	| [/session/:sessionId/element/:id/size              ](./../Tests/WebDriverAPI/ElementSize.cs)            	|
| GET    	| [/session/:sessionId/element/:id/text              ](./../Tests/WebDriverAPI/ElementText.cs)            	|
| POST   	| [/session/:sessionId/element/:id/value             ](./../Tests/WebDriverAPI/ElementSendKeys.cs)        	|
| POST   	| [/session/:sessionId/forward                       ](./../Tests/WebDriverAPI/Forward.cs)                	|
| POST   	| [/session/:sessionId/keys                          ](./../Tests/WebDriverAPI/SendKeys.cs)               	|
| GET    	| [/session/:sessionId/location                      ](./../Tests/WebDriverAPI/Location.cs)               	|
| POST   	| [/session/:sessionId/moveto                        ](./../Tests/WebDriverAPI/Mouse.cs)                  	|
| GET    	| [/session/:sessionId/orientation                   ](./../Tests/WebDriverAPI/Orientation.cs)            	|
| GET    	| [/session/:sessionId/screenshot                    ](./../Tests/WebDriverAPI/Screenshot.cs)             	|
| GET    	| [/session/:sessionId/source                        ](./../Tests/WebDriverAPI/Source.cs)                 	|
| POST   	| [/session/:sessionId/timeouts                      ](./../Tests/WebDriverAPI/Timeouts.cs)               	|
| GET    	| [/session/:sessionId/title                         ](./../Tests/WebDriverAPI/Title.cs)                  	|
| POST   	| [/session/:sessionId/touch/click                   ](./../Tests/WebDriverAPI/TouchClick.cs)             	|
| POST   	| [/session/:sessionId/touch/doubleclick             ](./../Tests/WebDriverAPI/TouchDoubleClick.cs)       	|
| POST   	| [/session/:sessionId/touch/down                    ](./../Tests/WebDriverAPI/TouchDownMoveUp.cs)        	|
| POST   	| [/session/:sessionId/touch/flick                   ](./../Tests/WebDriverAPI/TouchFlick.cs)             	|
| POST   	| [/session/:sessionId/touch/longclick               ](./../Tests/WebDriverAPI/TouchLongClick.cs)         	|
| POST   	| [/session/:sessionId/touch/move                    ](./../Tests/WebDriverAPI/TouchDownMoveUp.cs)        	|
| POST   	| [/session/:sessionId/touch/scroll                  ](./../Tests/WebDriverAPI/TouchScroll.cs)            	|
| POST   	| [/session/:sessionId/touch/up                      ](./../Tests/WebDriverAPI/TouchDownMoveUp.cs)        	|
| DELETE 	| [/session/:sessionId/window                        ](./../Tests/WebDriverAPI/Window.cs)                 	|
| POST   	| [/session/:sessionId/window                        ](./../Tests/WebDriverAPI/Window.cs)                 	|
| POST   	| [/session/:sessionId/window/maximize               ](./../Tests/WebDriverAPI/Window.cs)                 	|
| POST   	| [/session/:sessionId/window/size                   ](./../Tests/WebDriverAPI/Window.cs)                 	|
| GET    	| [/session/:sessionId/window/size                   ](./../Tests/WebDriverAPI/Window.cs)                 	|
| POST   	| [/session/:sessionId/window/:windowHandle/size     ](./../Tests/WebDriverAPI/Window.cs)                 	|
| GET    	| [/session/:sessionId/window/:windowHandle/size     ](./../Tests/WebDriverAPI/Window.cs)                 	|
| POST   	| [/session/:sessionId/window/:windowHandle/position ](./../Tests/WebDriverAPI/Window.cs)                 	|
| GET    	| [/session/:sessionId/window/:windowHandle/position ](./../Tests/WebDriverAPI/Window.cs)                 	|
| POST   	| [/session/:sessionId/window/:windowHandle/maximize ](./../Tests/WebDriverAPI/Window.cs)                 	|
| GET    	| [/session/:sessionId/window_handle                 ](./../Tests/WebDriverAPI/Window.cs)                 	|
| GET    	| [/session/:sessionId/window_handles                ](./../Tests/WebDriverAPI/Window.cs)                 	|

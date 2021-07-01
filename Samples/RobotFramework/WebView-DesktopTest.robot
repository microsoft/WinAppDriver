*** Settings ***
Documentation     Zoomba GUI/Desktop Library Tests using a WebView application
Library           Zoomba.DesktopLibrary
Library           Zoomba.GUILibrary   plugins=Zoomba.Helpers.EdgePlugin
Suite Setup       Driver Setup
Test Setup        Test Setup
Test Teardown     Run Keywords    Close All Applications   AND   Close All Browsers
Suite Teardown    Driver Teardown
Force Tags        Windows

*** Variables ***
${REMOTE_URL}           http://127.0.0.1:4723
# This app link will be the location of your compiled WebView application, using this demo app here:
# https://docs.microsoft.com/en-us/microsoft-edge/webview2/how-to/webdriver
${WebView_APP}          C:/Git/WebView2Samples/SampleApps/WebView2APISample/Debug/Win32/WebView2APISample.exe

*** Keywords ***
Test Setup
    # Get edge options from plugin and set required variables for webview
    ${options}=   Get Edge Options
    ${options.use_chromium}=    Set Variable   True
    ${options.use_webview}=    Set Variable   True
    ${options.binary_location}=    Set Variable   ${WebView_APP}
    # Open Browser with options
    Open Browser    https://www.github.com     Edge   options=${options}

*** Test Cases ***
Test WebView App
    # Click an element in the web page (GUILibrary Keyword)
    Zoomba.GUILibrary.Wait For And Click Element    //header/div[1]/div[2]/nav[1]/ul[1]/li[2]/a[1]
    # Connect to your WebView application's app frame (DesktopLibrary Keyword)
    Switch Application By Locator    ${REMOTE_URL}   class=WEBVIEW2APISAMPLE
    # Clicking a button outside the web view, in this case the 'Back' button (DesktopLibrary Keyword)
    Zoomba.DesktopLibrary.Wait For And Click Element    name=Back
    # Click an element in the web page, just to demonstate that we can still issue commands there as well
    Zoomba.GUILibrary.Wait For And Click Element    //header/div[1]/div[2]/nav[1]/ul[1]/li[2]/a[1]
    Sleep   3s   # Just to slow down the teardown for demo

*** Settings ***
Documentation     Zoomba Desktop Library Tests. See https://accruent.github.io/robotframework-zoomba/DesktopLibraryDocumentation.html for a full list of keywords.
Library           Zoomba.DesktopLibrary
Suite Setup       Start App
Test Setup        Launch Application
Test Teardown     Quit Application
Suite Teardown    Driver Teardown
Force Tags        Windows

*** Variables ***
${REMOTE_URL}           http://127.0.0.1:4723
${APP}                  Microsoft.WindowsCalculator_8wekyb3d8bbwe!App
${Notepad}              C:/Windows/System32/notepad.exe

*** Keywords ***
Start App
    [Documentation]     Sets up the application for quick launching through 'Launch Application' and starts the winappdriver
    Driver Setup
    Open Application    ${REMOTE_URL}     platformName=Windows    deviceName=Windows   app=${APP}    alias=Main
    Maximize Window
    Quit Application

*** Test Cases ***
Wait For And Click Element By Id Keyword Test
    Wait For And Click Element       accessibility_id=num2Button
    Wait Until Element Contains       accessibility_id=CalculatorResults      2

Wait For And Click Element By Xpath Keyword Test
    Wait For And Click Element       xpath=//Button[@Name="Two"]
    Wait Until Element Contains       accessibility_id=CalculatorResults      2

Wait For And Click Element By Name Keyword Test
    Wait For And Click Element       name=Two
    Wait Until Element Contains       accessibility_id=CalculatorResults      2

Wait For And Click Element By Class Keyword Test
    Wait For And Click Element       class=Button

Wait For And Input Text By Id Keyword Test
    Wait For And Input Text        accessibility_id=CalculatorResults       12345
    Wait Until Element Contains       accessibility_id=CalculatorResults       12,345

Wait For And Input Text By Name Keyword Test
    Wait For And Input Text        name=Display is 0       12345
    Wait Until Element Contains       accessibility_id=CalculatorResults       12,345

Wait For And Long Press Keyword Test
    Wait For And Long Press       accessibility_id=num2Button
    Wait Until Element Contains       accessibility_id=CalculatorResults      2

Wait For And Input Password Keyword Test
    Wait For And Input Password        accessibility_id=CalculatorResults       12345
    Wait Until Element Contains       accessibility_id=CalculatorResults      12,345
    Wait Until Element Does Not Contain   accessibility_id=CalculatorResults      0

Wait Until Element is Enabled / Disabled Keyword Test
    Wait Until Element Is Enabled       accessibility_id=MemPlus
    Wait Until Element Is Disabled       accessibility_id=MemRecall

Click Webelement Test
    ${element}    Get Webelement    name=One
    Click Element    ${element}

Mouse Over Element Keyword Test
    Mouse Over Element     name=Two

Mouse Over And Click Element Keyword Test
    # This test will fail unless run at 1080p
    Mouse Over And Click Element     name=Two
    Mouse Over And Click Element     name=Two     x_offset=400   y_offset=100
    Wait Until Element Contains       accessibility_id=CalculatorResults       23

Mouse Over And Context Click Element Keyword Test
    Mouse Over And Context Click Element     name=Two

Mouse Over And Double Click Element Keyword Test
    Mouse Over And Click Element     name=Two    double_click=True

Wait For And Mouse Over And Click Element Keyword Test
    Wait For And Mouse Over And Click Element     name=Two

Mouse Over by Offset Keyword Test
    Mouse Over Element     name=Three
    Mouse Over By Offset    100    -200

Click A Point Keyword Test
    Mouse Over Element     name=Three
    Click A Point
    Click A Point     100    -200
    Click A Point     double_click=True

Context Click A Point Keyword Test
    Mouse Over Element     name=Three
    Context Click A Point
    Context Click A Point     100    -200

Send Keys Keyword Test
    Send Keys    24     \ue025     2      \ue007
    Page Should Contain Text    26

Send Keys with Modifier (Alt + 2)
    Send Keys               \ue00A    2    \ue00A
    Wait Until Page Contains Element          Name=Scientific Calculator mode
    Send Keys               \ue00A    1    \ue00A
    Wait Until Page Contains Element          Name=Standard Calculator mode

Send Keys To Element Keyword Test
    Send Keys To Element   name=Display is 0    24     \ue025     2      \ue007
    Page Should Contain Text    26

Save Selenium Screenshot Test
    ${file1}=                       Save Appium Screenshot
    ${file2}=                       Save Appium Screenshot
    Should Not Be Equal             ${file1}  ${file2}
    Should Match Regexp             ${file1}                    appium-screenshot-\\d{10}.\\d{0,8}-\\d.png

Xpath Should Match X Times
    Xpath Should Match X Times     //Group[@Name="Number pad"][@AutomationId="NumberPad"]/Button     11

Select Element From Combobox Test
    Select Element From ComboBox      accessibility_id=TogglePaneButton         accessibility_id=Speed
    Wait Until Page Contains Element  accessibility_id=TogglePaneButton
    Select Element From ComboBox      accessibility_id=Units1         name=Knots
    Wait Until Page Contains Element  accessibility_id=TogglePaneButton
    Select Element From ComboBox      accessibility_id=TogglePaneButton         accessibility_id=Standard

Select Elements From Menu Test
    Select Elements From Menu        name=Two      name=Three      name=Four
    Wait Until Element Contains       accessibility_id=CalculatorResults       234

Drag And Drop By Touch Tests
    drag and drop by touch      accessibility_id=AppName       name=Five
    drag and drop by touch offset      accessibility_id=AppName     100    100
    Maximize Window

Tap Tests
    Tap    name=Five
    Wait For And Tap     name=Five
    Double Tap    name=Five
    Wait For And Double Tap     name=Five
    Wait Until Element Contains       accessibility_id=CalculatorResults      555,555

Flick Tests
    Select Element From ComboBox      accessibility_id=TogglePaneButton         accessibility_id=Graphing
    Wait Until Page Contains Element  accessibility_id=TogglePaneButton
    Flick      100    100
    Wait For And Click Element        accessibility_id=TogglePaneButton
    Flick From Element                accessibility_id=Standard       0      -100    10
    Wait For And Flick From Element   accessibility_id=Standard       0      -100    10
    Wait Until Page Contains Element  accessibility_id=Standard
    Wait For And Click Element        accessibility_id=Standard

Switch Application By Name or Locator
    # Select Window by Class Name
    Open Application    ${REMOTE_URL}     platformName=Windows    deviceName=Windows   app=${Notepad}
    Switch Application    Main
    Switch Application By Locator    ${REMOTE_URL}     class=Notepad
    Wait For And Input Text          Name=Text Editor      test
    Wait Until Element Contains      Name=Text Editor      test
    Quit Application
    Wait For And Click Element       Name=Don't Save
    # Select Window by Xpath
    Open Application    ${REMOTE_URL}     platformName=Windows    deviceName=Windows   app=${Notepad}
    Switch Application    Main
    Switch Application By Locator    ${REMOTE_URL}     //Window[contains(@Name, "Notepad")]
    Wait For And Input Text          Name=Text Editor      test
    Wait Until Element Contains      Name=Text Editor      test
    Quit Application
    Wait For And Click Element       Name=Don't Save
    # Select Window by Name
    Open Application    ${REMOTE_URL}     platformName=Windows    deviceName=Windows   app=${Notepad}
    Switch Application    Main
    Switch Application By Name       ${REMOTE_URL}     Untitled - Notepad
    Wait For And Input Text          Name=Text Editor      test
    Wait Until Element Contains      Name=Text Editor      test
    Quit Application
    Wait For And Click Element       Name=Don't Save
    # Select Window by Partial Name
    Open Application    ${REMOTE_URL}     platformName=Windows    deviceName=Windows   app=${Notepad}
    Switch Application    Main
    Switch Application By Name       ${REMOTE_URL}     Notepad    exact_match=False
    Wait For And Input Text          Name=Text Editor      test
    Wait Until Element Contains      Name=Text Editor      test
    Quit Application
    Wait For And Click Element       Name=Don't Save
    # Switch back to the main window to make sure it gets closed
    Switch Application    Main

Switch Application Multiple Desktop
    # Open Application creates a new desktop session for this isnatnce, useful when running tests on multiple computers
    Open Application    ${REMOTE_URL}     platformName=Windows    deviceName=Windows   app=${Notepad}     alias=Notepad   desktop_alias=Desktop2
    Switch Application    Desktop
    Switch Application    Desktop2
    Switch Application    Notepad     desktop_alias=Desktop
    Quit Application
    # Switch back to the main window to make sure it gets closed
    Switch Application    Main

Switch To Desktop Test
    [Setup]      Driver Setup
    Switch Application      Desktop
    Wait For And Click Element           name=Start
    Wait For And Click Element           name=Start

*** Settings ***
Documentation   Calulator test using Zoomba Desktop Library. Requires Appium Server running on port 4723.
Library         Zoomba.DesktopLibrary
Suite Setup     App Suite Setup
Test Setup      Launch Application
Test Teardown   Quit Application
Suite Teardown    Close All Applications
Force Tags        Windows

*** Variables ***
${REMOTE_URL}           http://localhost:4723/wd/hub
${APP}                  Microsoft.WindowsCalculator_8wekyb3d8bbwe!App

*** Keywords ***
App Suite Setup
    Open Application        ${REMOTE_URL}     platformName=Windows    deviceName=Windows   app=${APP}
    Maximize Window
    Quit Application

*** Test Cases ***
Wait For And Click Element By Id Keyword Test
    Wait For And Click Element       accessibility_id=num2Button
    Wait Until Page Contains      2

Wait For And Click Element By Name Keyword Test
    Wait For And Click Element       name=Two
    Wait Until Page Contains      2

Wait For And Click Element By Class Keyword Test
    Wait For And Click Element       class=Button

Wait For And Input Text By Id Keyword Test
    Wait For And Input Text        accessibility_id=CalculatorResults       12345
    Wait Until Page Contains       12,345

Wait For And Input Text By Name Keyword Test
    Wait For And Input Text        name=Display is 0       12345
    Wait Until Page Contains       12,345

Wait For And Long Press Keyword Test
    Wait For And Long Press       accessibility_id=num2Button
    Wait Until Page Contains      2

Wait For And Input Password Keyword Test
    Wait For And Input Password        accessibility_id=CalculatorResults       12345
    Wait Until Element Contains       accessibility_id=CalculatorResults      12,345
    Wait Until Element Does Not Contain   accessibility_id=CalculatorResults      0

Wait Until Element is Enabled / Disabled Keyword Test
    Wait Until Element Is Enabled       accessibility_id=MemPlus
    Wait Until Element Is Disabled       accessibility_id=MemRecall

Mouse Over Element/Text Keyword Test
    Mouse Over Element     name=Two
    Mouse Over Text      Memory
    Mouse Over Text      Memory    True

Mouse Over And Click Element/Text Keyword Test
    Mouse Over And Click Element     name=Two
    Mouse Over And Click Element     name=Two     x_offset=400   y_offset=100
    Mouse Over And Click Text      Memory
    Mouse Over And Click Text      Memory    True

Mouse Over And Context Click Element/Text Keyword Test
    Mouse Over And Context Click Element     name=Two
    Mouse Over And Context Click Text      Memory
    Mouse Over And Context Click Text      Memory    True

Mouse Over And Double Click Element/Text Keyword Test
    Mouse Over And Click Element     name=Two    double_click=True
    Mouse Over And Click Text      Memory    double_click=True
    Mouse Over And Click Text      Memory    True    True

Wait For And Mouse Over And Click Element/Text Keyword Test
    Wait For And Mouse Over And Click Element     name=Two
    Wait For And Mouse Over And Click Text      Memory
    Wait For And Mouse Over And Click Text      Memory    True

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

Send Keys To Element Keyword Test
    Send Keys To Element   name=Display is 0    24     \ue025     2      \ue007
    Page Should Contain Text    26

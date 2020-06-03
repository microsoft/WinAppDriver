*** Settings ***
Documentation   Calulator test using Zoomba Desktop Library. Requires Appium/WinAppDriver running on port 4723.
Library         Zoomba.DesktopLibrary
Suite Setup     App Suite Setup
Test Setup      Launch Application
Test Teardown   Quit Application
Suite Teardown    Close All Applications
Force Tags        Windows

*** Variables ***
#${REMOTE_URL}           http://localhost:4723/wd/hub     #If using Appium
${REMOTE_URL}           http://localhost:4723             #If Using WinAppDriver directly (suggested)
${APP}                  Microsoft.WindowsCalculator_8wekyb3d8bbwe!App

*** Keywords ***
App Suite Setup
    Open Application        ${REMOTE_URL}     platformName=Windows    deviceName=Windows   app=${APP}
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

Mouse Over Element Keyword Test
    Mouse Over Element     name=Two

Mouse Over And Click Element Keyword Test
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

Send Keys with Modifier (Ctrl + v)
    Send Keys               \ue009    v    \ue009
    Wait Until Element Contains       accessibility_id=CalculatorResults    Display is Invalid input

Send Keys To Element Keyword Test
    Send Keys To Element   name=Display is 0    24     \ue025     2      \ue007
    Page Should Contain Text    26

Save Selenium Screenshot Test
    ${file1}=                       Save Appium Screenshot
    ${file2}=                       Save Appium Screenshot
    Should Not Be Equal             ${file1}  ${file2}
    Should Match Regexp             ${file1}                    appium-screenshot-\\d{10}.\\d{0,8}-\\d.png

Select Element From Combobox Test
    Select Element From ComboBox      accessibility_id=TogglePaneButton         accessibility_id=Speed
    Select Element From ComboBox      accessibility_id=Units1         name=Knots
    Select Element From ComboBox      accessibility_id=TogglePaneButton         accessibility_id=Standard

Switch To Desktop Test
    Close Application
    Switch Application      Desktop
    Click Element           name=Start

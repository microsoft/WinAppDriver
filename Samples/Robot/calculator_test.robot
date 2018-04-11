*** Settings ***
Library          AppiumLibrary
Library          CalculatorKeywordsLibrary.py
Library          robot/libraries/OperatingSystem.py
Test Setup       Open App

*** Test Cases ***
Test Print Key
    [Tags]    Calculator test
    [Documentation]    *Purpose: test just press button*
    ...
    ...     *Actions*:
    ...     - Check is the calculator started
    ...     - Press button Seven
    ...
    ...     *Result*:
    ...     - Result is 8
    ...
    Assert Calculator Is Open
    Click Calculator Button    Seven
    ${result} =    Get Result
    AssertEquals    ${result}    7
    [Teardown]    Quit App

Test Addition
    [Tags]    Calculator test
    [Documentation]    *Purpose: test to Addition*
    ...
    ...     *Actions*:
    ...     - Check is the calculator started
    ...     - Press button One
    ...     - Press button Plus
    ...     - Press button Seven
    ...     - Press button Equals
    ...
    ...     *Result*:
    ...     - Result is 8
    ...
    Assert Calculator Is Open
    Click Calculator Button    One
    Click Calculator Button    Plus
    Click Calculator Button    Seven
    Click Calculator Button    Equals
    ${result} =    Get Result
    AssertEquals    ${result}    8
    [Teardown]    Quit App

Test Multiply
    [Tags]    Calculator test
    [Documentation]    *Purpose: test to Multiply*
    ...
    ...     *Actions*:
    ...     - Check is the calculator started
    ...     - Press button Four
    ...     - Press button Multiply by
    ...     - Press button Six
    ...     - Press button Equals
    ...
    ...     *Result*:
    ...     - Result is 24
    ...
    Assert Calculator Is Open
    Click Calculator Button    Four
    Click Calculator Button    Multiply by
    Click Calculator Button    Six
    Click Calculator Button    Equals
    ${result} =    Get Result
    AssertEquals    ${result}    24
    [Teardown]    Quit App

Test Square
    [Tags]    Calculator test
    [Documentation]    *Purpose: test Square*
    ...
    ...     *Actions*:
    ...     - Check is the calculator started
    ...     - Press button Four
    ...     - Press button Square
    ...
    ...     *Result*:
    ...     - Result is 2
    ...
    Assert Calculator Is Open
    Click Calculator Button    Four
    Click Calculator Button    Square root
    ${result} =    Get Result
    AssertEquals    ${result}    2
    [Teardown]    Quit App

*** Keywords ***
Assert Calculator Is Open
    ${result} =    Is calc displayed
    AssertTrue    ${result}
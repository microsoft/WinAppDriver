# ******************************************************************************
#
# Copyright (c) 2016 Microsoft Corporation. All rights reserved.
#
# This code is licensed under the MIT License (MIT).
#
# THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
# IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
# FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
# AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
# LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
# OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
# THE SOFTWARE.
#
#******************************************************************************

# Recommend checking  out more Ruby examples for Appium, including setup instructions here
# https://github.com/appium/sample-code/tree/master/sample-code/examples/ruby

# TODO: Once we have better Appium integration update this test to use the Appium gem
require 'selenium-webdriver'

$CalculatorSession
$CalculatorResult

def caps 
    {
        platformName: "WINDOWS", platform: "WINDOWS", deviceName: "mydevice", app: "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App" 
    }
end

def assert &block
    raise AssertionError unless yield
end

def setup
    $CalculatorSession = Selenium::WebDriver.for(:remote, :url => "http://127.0.0.1:4723/", :desired_capabilities => caps )
    
    $CalculatorSession.find_elements(:name, "Clear")[0].click;
    $CalculatorSession.find_elements(:name, "Seven")[0].click;
    $CalculatorResult = $CalculatorSession.find_elements(:name, "Display is  7 ")[0];
    assert{$CalculatorResult != nil}
    $CalculatorSession.find_elements(:name, "Clear")[0].click;
end

def addition
    $CalculatorSession.find_elements(:name, "One")[0].click;
    $CalculatorSession.find_elements(:name, "Plus")[0].click;
    $CalculatorSession.find_elements(:name, "Seven")[0].click;
    $CalculatorSession.find_elements(:name, "Equals")[0].click;
    assert{$CalculatorResult.text == "Display is  8 "};
end

def combination
    $CalculatorSession.find_elements(:name, "Seven")[0].click;
    $CalculatorSession.find_elements(:name, "Multiply by")[0].click;
    $CalculatorSession.find_elements(:name, "Nine")[0].click;
    $CalculatorSession.find_elements(:name, "Plus")[0].click;
    $CalculatorSession.find_elements(:name, "One")[0].click;
    $CalculatorSession.find_elements(:name, "Equals")[0].click;
    $CalculatorSession.find_elements(:name, "Divide by")[0].click;
    $CalculatorSession.find_elements(:name, "Eight")[0].click;
    $CalculatorSession.find_elements(:name, "Equals")[0].click;
    assert{$CalculatorResult.text == "Display is  8 "};
end

def division
    $CalculatorSession.find_elements(:name, "Eight")[0].click;
    $CalculatorSession.find_elements(:name, "Eight")[0].click;
    $CalculatorSession.find_elements(:name, "Divide by")[0].click;
    $CalculatorSession.find_elements(:name, "One")[0].click;
    $CalculatorSession.find_elements(:name, "One")[0].click;
    $CalculatorSession.find_elements(:name, "Equals")[0].click;
    assert{$CalculatorResult.text == "Display is  8 "};
end

def teardown
    $CalculatorSession.quit
end

# run through the tests
setup
addition
combination
division
teardown

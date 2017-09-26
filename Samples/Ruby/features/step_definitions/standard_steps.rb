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

require 'selenium-webdriver'
require 'rubygems'
require 'appium_lib'

# $CalculatorSession is a Selenium::WebDriver instance defined in setup_steps.rb

When /^I see "([^\"]*)"$/ do |text|

   result = $CalculatorSession.find_elements(:name, text)[0]
   unless result != nil
       fail("'#{text}' was not found.")
   end
end

When /^I wait to see "([^\"]*)"$/ do |text|

    wait = Selenium::WebDriver::Wait.new :timeout => 30
    wait.until { $CalculatorSession.find_elements(:name, text)[0] != nil }
end

When /^I press "([^\"]*)"$/ do |text|

    $CalculatorSession.find_element(:name, text).click()
end

When /^I press "([^\"]*)" by id$/ do |control_id|

    $CalculatorSession.find_element(:accessibility_id, control_id).click()
end

# FIXME:
# {"using":"xpath","value":"//Button[@Name='Nine']"}
# HTTP/1.1 404 Not Found
# Content-Length: 139
# Content-Type: application/json
# {"status":7,"value":{"error":"no such element","message":"An element could not be located on the page using the given search parameters."}}
When /^I press "([^\"]*)" by xpath$/ do |x_path_name|

    $CalculatorSession.find_element(:xpath, "//Button[@Name='#{x_path_name}']").click()
end

# FIXME:
# {"using":"xpath","value":"//Button[@AutomationId=\"num9button\"]"}
# HTTP/1.1 404 Not Found
# Content-Length: 139
# Content-Type: application/json
# {"status":7,"value":{"error":"no such element","message":"An element could not be located on the page using the given search parameters."}}
When /^I press "([^\"]*)" by automation$/ do |automation_id|

    $CalculatorSession.find_element(:xpath, "//Button[@AutomationId=\"#{automation_id}\"]").click()
end

When /^I see the result is "([^\"]*)"$/ do |expected|

    actual = return_results().sub! 'Display is', ''
    actual = actual.strip()

    unless actual.eql? expected
        fail("Expected '#{expected}' but result is '#{actual}'.")
    end
end

def return_results()
    return $CalculatorSession.find_element(:accessibility_id, "CalculatorResults").text
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

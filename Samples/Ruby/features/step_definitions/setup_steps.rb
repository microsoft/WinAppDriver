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

$CalculatorSession
$CalculatorResult

Given /^I start the application$/ do
    setup()
end

Given /^I quit the application$/ do
    teardown()
end

def assert &block
    raise AssertionError unless yield
end

def setup()

    opts =
    {
        caps:
        {
            platformName: "WINDOWS",
            platform: "WINDOWS",
            deviceName: "WindowsPC",
            app: 'Microsoft.WindowsCalculator_8wekyb3d8bbwe!App'
        },
        appium_lib:
        {
            wait_timeout: 30,
            wait_interval: 0.5
        }
    }

    # run winappdriver.exe 127.0.0.1 4723/wd/hub
    $CalculatorSession = Appium::Driver.new(opts, false).start_driver

    wait = Selenium::WebDriver::Wait.new :timeout => 20
    wait.until { $CalculatorSession.find_elements(:name, "Clear")[0] != nil }

    # clear if already running
    $CalculatorSession.find_element(:name, "Clear").click()
end

def teardown()
    $CalculatorSession.quit
end

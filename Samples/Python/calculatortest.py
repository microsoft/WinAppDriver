#******************************************************************************
#
# Copyright (c) 2016 Microsoft Corporation. All rights reserved.
#
# This code is licensed under the MIT License (MIT).
#
# THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
# IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
# FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
# AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
# // LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
# OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
# THE SOFTWARE.
#
#******************************************************************************


import unittest
from appium import webdriver
from appium.options.windows import WindowsOptions
from appium.webdriver.common.appiumby import AppiumBy


class SimpleCalculatorTests(unittest.TestCase):
    @classmethod
    def setUpClass(self):
        # set up appium
        desired_caps = {"app": "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"}
        self.driver = webdriver.Remote(
            command_executor='http://127.0.0.1:4723',
            options=WindowsOptions().load_capabilities(desired_caps))

    @classmethod
    def tearDownClass(self):
        self.driver.quit()

    def getresults(self):
        displaytext = self.driver.find_element(AppiumBy.ACCESSIBILITY_ID, "CalculatorResults").text
        displaytext = displaytext.strip("Display is ")
        displaytext = displaytext.rstrip(' ')
        displaytext = displaytext.lstrip(' ')
        return displaytext

    def test_initialize(self):
        self.driver.find_element(AppiumBy.NAME, "Clear").click()
        self.driver.find_element(AppiumBy.NAME, "Seven").click()
        self.assertEqual(self.getresults(), "7")
        self.driver.find_element(AppiumBy.NAME, "Clear").click()

    def test_addition(self):
        self.driver.find_element(AppiumBy.NAME, "One").click()
        self.driver.find_element(AppiumBy.NAME, "Plus").click()
        self.driver.find_element(AppiumBy.NAME, "Seven").click()
        self.driver.find_element(AppiumBy.NAME, "Equals").click()
        self.assertEqual(self.getresults(), "8")

    def test_combination(self):
        self.driver.find_element(AppiumBy.NAME, "Seven").click()
        self.driver.find_element(AppiumBy.NAME, "Multiply by").click()
        self.driver.find_element(AppiumBy.NAME, "Nine").click()
        self.driver.find_element(AppiumBy.NAME, "Plus").click()
        self.driver.find_element(AppiumBy.NAME, "One").click()
        self.driver.find_element(AppiumBy.NAME, "Equals").click()
        self.driver.find_element(AppiumBy.NAME, "Divide by").click()
        self.driver.find_element(AppiumBy.NAME, "Eight").click()
        self.driver.find_element(AppiumBy.NAME, "Equals").click()
        self.assertEqual(self.getresults(), "8")

    def test_division(self):
        self.driver.find_element(AppiumBy.NAME, "Eight").click()
        self.driver.find_element(AppiumBy.NAME, "Eight").click()
        self.driver.find_element(AppiumBy.NAME, "Divide by").click()
        self.driver.find_element(AppiumBy.NAME, "One").click()
        self.driver.find_element(AppiumBy.NAME, "One").click()
        self.driver.find_element(AppiumBy.NAME, "Equals").click()
        self.assertEqual(self.getresults(), "8")

    def test_multiplication(self):
        self.driver.find_element(AppiumBy.NAME, "Nine").click()
        self.driver.find_element(AppiumBy.NAME, "Multiply by").click()
        self.driver.find_element(AppiumBy.NAME, "Nine").click()
        self.driver.find_element(AppiumBy.NAME, "Equals").click()
        self.assertEqual(self.getresults(), "81")

    def test_subtraction(self):
        self.driver.find_element(AppiumBy.NAME, "Nine").click()
        self.driver.find_element(AppiumBy.NAME, "Minus").click()
        self.driver.find_element(AppiumBy.NAME, "One").click()
        self.driver.find_element(AppiumBy.NAME, "Equals").click()
        self.assertEqual(self.getresults(), "8")


if __name__ == '__main__':
    suite = unittest.TestLoader().loadTestsFromTestCase(SimpleCalculatorTests)
    unittest.TextTestRunner(verbosity=2).run(suite)

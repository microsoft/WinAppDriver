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
# // LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
# OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
# THE SOFTWARE.
#
# ******************************************************************************


import re
import unittest

from appium import webdriver


class SimpleCalculatorTests(unittest.TestCase):

    @classmethod
    def setUpClass(self):
        # set up appium
        desired_caps = {}
        desired_caps["app"] = "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"
        self.driver = webdriver.Remote(
            command_executor='http://127.0.0.1:4723',
            desired_capabilities=desired_caps)

    @classmethod
    def tearDownClass(self):
        self.driver.quit()

    def getresults(self):
        displaytext = self.driver.find_element_by_accessibility_id(
            "CalculatorResults").text
        return re.sub('[^0123456789]', '', displaytext)

    def test_initialize(self):
        self.driver.find_element_by_accessibility_id("clearButton").click()
        self.driver.find_element_by_accessibility_id("num7Button").click()
        self.assertEqual(self.getresults(), "7")
        self.driver.find_element_by_accessibility_id("clearButton").click()

    def test_addition(self):
        self.driver.find_element_by_accessibility_id("num1Button").click()
        self.driver.find_element_by_accessibility_id("plusButton").click()
        self.driver.find_element_by_accessibility_id("num7Button").click()
        self.driver.find_element_by_accessibility_id("equalButton").click()
        self.assertEqual(self.getresults(), "8")

    def test_combination(self):
        self.driver.find_element_by_accessibility_id("num7Button").click()
        self.driver.find_element_by_accessibility_id("multiplyButton").click()
        self.driver.find_element_by_accessibility_id("num9Button").click()
        self.driver.find_element_by_accessibility_id("plusButton").click()
        self.driver.find_element_by_accessibility_id("num1Button").click()
        self.driver.find_element_by_accessibility_id("equalButton").click()
        self.driver.find_element_by_accessibility_id("divideButton").click()
        self.driver.find_element_by_accessibility_id("num8Button").click()
        self.driver.find_element_by_accessibility_id("equalButton").click()
        self.assertEqual(self.getresults(), "8")

    def test_division(self):
        self.driver.find_element_by_accessibility_id("num8Button").click()
        self.driver.find_element_by_accessibility_id("num8Button").click()
        self.driver.find_element_by_accessibility_id("divideButton").click()
        self.driver.find_element_by_accessibility_id("num1Button").click()
        self.driver.find_element_by_accessibility_id("num1Button").click()
        self.driver.find_element_by_accessibility_id("equalButton").click()
        self.assertEqual(self.getresults(), "8")

    def test_multiplication(self):
        self.driver.find_element_by_accessibility_id("num9Button").click()
        self.driver.find_element_by_accessibility_id("multiplyButton").click()
        self.driver.find_element_by_accessibility_id("num9Button").click()
        self.driver.find_element_by_accessibility_id("equalButton").click()
        self.assertEqual(self.getresults(), "81")

    def test_subtraction(self):
        self.driver.find_element_by_accessibility_id("num9Button").click()
        self.driver.find_element_by_accessibility_id("minusButton").click()
        self.driver.find_element_by_accessibility_id("num1Button").click()
        self.driver.find_element_by_accessibility_id("equalButton").click()
        self.assertEqual(self.getresults(), "8")


if __name__ == '__main__':
    suite = unittest.TestLoader().loadTestsFromTestCase(SimpleCalculatorTests)
    unittest.TextTestRunner(verbosity=2).run(suite)

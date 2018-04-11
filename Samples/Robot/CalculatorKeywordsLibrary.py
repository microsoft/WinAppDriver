from appium import webdriver
from robot.api.deco import keyword
import unittest


class App:

    def __init__(self):
        desired_caps = {}
        desired_caps["app"] = "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App"
        self.__driver = webdriver.Remote(
            command_executor='http://127.0.0.1:4723',
            desired_capabilities=desired_caps)

    def setup(self):
        return self.__driver


class CalculatorKeywordsLibrary(unittest.TestCase):
    @keyword("Open App")
    def open_app(self):
        self.driver = App().setup()
        return self.driver

    @keyword("Get Result")
    def getresults(self):
        displaytext = self.driver.find_element_by_accessibility_id("CalculatorResults").text
        displaytext = displaytext.strip("Display is ")
        displaytext = displaytext.rstrip(' ')
        displaytext = displaytext.lstrip(' ')
        return displaytext

    @keyword("Click Calculator Button")
    def click_calculator_button(self, str):
        self.driver.find_element_by_name(str).click()

    @keyword("Quit App")
    def quit_app(self):
        self.driver.quit()

    @keyword("Is calc displayed")
    def is_calculator_open(self):
        return self.driver.find_element_by_name("Calculator").is_displayed()

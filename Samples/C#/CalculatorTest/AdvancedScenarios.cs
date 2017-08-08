//******************************************************************************
//
// Copyright (c) 2016 Microsoft Corporation. All rights reserved.
//
// This code is licensed under the MIT License (MIT).
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//******************************************************************************

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Remote;

namespace CalculatorTest
{
    [TestClass]
    public class AdvancedScenarios
    {
        protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        protected static RemoteWebDriver CalculatorSession;
        protected static RemoteWebElement CalculatorResult;
        protected static string OriginalCalculatorMode;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            // Launch the calculator app
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App");
            CalculatorSession = new RemoteWebDriver(new Uri(WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(CalculatorSession);
            CalculatorSession.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(4));

            // Make sure we're in standard mode
            CalculatorSession.FindElementByXPath("//Button[starts-with(@Name, \"Menu\")]").Click();
            OriginalCalculatorMode = CalculatorSession.FindElementByXPath("//List[@AutomationId=\"FlyoutNav\"]//ListItem[@IsSelected=\"True\"]").Text;
            CalculatorSession.FindElementByXPath("//ListItem[@Name=\"Standard Calculator\"]").Click();

            // Use series of operation to locate the calculator result text element as a workaround
            // We currently cannot query element by automationId without using modified appium dot net driver
            // TODO: Use a proper appium/webdriver nuget package that allow us to query based on automationId
            CalculatorSession.FindElementByXPath("//Button[@Name=\"Clear\"]").Click();
            CalculatorSession.FindElementByXPath("//Button[@Name=\"Seven\"]").Click();
            CalculatorResult = CalculatorSession.FindElementByName("Display is 7") as RemoteWebElement;
            Assert.IsNotNull(CalculatorResult);
        }

        [ClassCleanup]
        public static void TearDown()
        {
            // Restore original mode before closing down
            CalculatorSession.FindElementByXPath("//Button[starts-with(@Name, \"Menu\")]").Click();
            CalculatorSession.FindElementByXPath("//ListItem[@Name=\"" + OriginalCalculatorMode + "\"]").Click();

            CalculatorResult = null;
            CalculatorSession.Dispose();
            CalculatorSession = null;
        }

        [TestInitialize]
        public void Clear()
        {
            CalculatorSession.FindElementByName("Clear").Click();
            Assert.AreEqual("Display is 0", CalculatorResult.Text);
        }

        [TestMethod]
        [DataRow("One",     "Plus",     "One",      "Equals",    "2")]
        [DataRow("One",     "Plus",     "Two",      "Equals",    "3")]
        [DataRow("One",     "Plus",     "Three",    "Equals",    "4")]
        [DataRow("One",     "Plus",     "Four",     "Equals",    "5")]
        [DataRow("One",     "Plus",     "Five",     "Equals",    "6")]
        [DataRow("One",     "Plus",     "Six",      "Equals",    "7")]
        [DataRow("One",     "Plus",     "Seven",    "Equals",    "8")]
        [DataRow("One",     "Plus",     "Eight",    "Equals",    "9")]
        [DataRow("One",     "Plus",     "Nine",     "Equals",    "10")]

        [DataRow("Nine",    "Plus",     "Two",      "Equals",    "11")]
        [DataRow("Eight",   "Plus",     "Four",     "Equals",    "12")]
        [DataRow("Seven",   "Plus",     "Six",      "Equals",    "13")]
        [DataRow("Six",     "Plus",     "Eight",    "Equals",    "14")]
        [DataRow("Six",     "Plus",     "Nine",     "Equals",    "15")]
        [DataRow("Seven",   "Plus",     "Nine",     "Equals",    "16")]
        [DataRow("Nine",    "Plus",     "Eight",    "Equals",    "17")]
        [DataRow("Nine",    "Plus",     "Nine",     "Equals",    "18")]


        public void Addition_Advanced(string input1,string operation,string input2, string equals, string assertion)
        {
            CalculatorSession.FindElementByName(input1).Click();
            CalculatorSession.FindElementByName(operation).Click();
            CalculatorSession.FindElementByName(input2).Click();
            CalculatorSession.FindElementByName(equals).Click();
            Assert.AreEqual("Display is " + assertion, CalculatorResult.Text);
        }

        [TestMethod]
        [DataRow("Seven",   "Multiply by",  "Nine",     "Plus", "One",  "Equals",   "64 ",  "Divide by",    "Eight",   "Equals",   "8")]
        [DataRow("Nine",    "Multiply by",  "One",      "Plus", "One",  "Equals",   "10 ",  "Divide by",    "Five",    "Equals",   "2")]
        [DataRow("Eight",   "Multiply by",  "Two",      "Plus", "Two",  "Equals",   "18 ",  "Divide by",    "Four",    "Equals",   "4.5")]
        [DataRow("Seven",   "Multiply by",  "Three",    "Plus", "Four",  "Equals",  "25 ",  "Divide by",    "Four",    "Equals",   "6.25")]
        [DataRow("Six",     "Multiply by",  "Four",     "Plus", "One",  "Equals",   "25 ",  "Divide by",    "Two",     "Equals",   "12.5")]

        public void Combination_Advanced(string input1,string operation1,string input2,string operation2,string input3,string equals1,string assertion1,
                                string operation3,string input4,string equals2,string assertion2)
        {
            CalculatorSession.FindElementByXPath("//Button[@Name=\""+input1+"\"]").Click();
            CalculatorSession.FindElementByXPath("//Button[@Name=\""+operation1+"\"]").Click();
            CalculatorSession.FindElementByXPath("//Button[@Name=\""+ input2+ "\"]").Click();
            CalculatorSession.FindElementByXPath("//Button[@Name=\"" + operation2 + "\"]").Click();
            CalculatorSession.FindElementByXPath("//Button[@Name=\"" + input3 + "\"]").Click();
            CalculatorSession.FindElementByXPath("//Button[@Name=\"" + equals1 + "\"]").Click();
            Assert.AreEqual("Display is "+assertion1, CalculatorResult.Text);
            CalculatorSession.FindElementByXPath("//Button[@Name=\"" + operation3 + "\"]").Click();
            CalculatorSession.FindElementByXPath("//Button[@Name=\"" + input4 + "\"]").Click();
            CalculatorSession.FindElementByXPath("//Button[@Name=\"" + equals2 + "\"]").Click();
            Assert.AreEqual("Display is " + assertion2, CalculatorResult.Text);
        }

        [TestMethod]
        [DataRow("Eight",   "Eight",    "Divide by",    "One", "One",  "Equals",   "8")]
        [DataRow("Six",     "Four",     "Divide by",    "One", "Zero", "Equals",   "6.4")]
        [DataRow("Five",    "Five",     "Divide by",    "One", "Zero", "Equals",   "5.5")]
        public void Division_Advanced(string input1, string input2, string operation, string input3, string input4, string equals, string assertion)
        {
            CalculatorSession.FindElementByName(input1).Click();
            CalculatorSession.FindElementByName(input2).Click();
            CalculatorSession.FindElementByName(operation).Click();
            CalculatorSession.FindElementByName(input3).Click();
            CalculatorSession.FindElementByName(input4).Click();
            CalculatorSession.FindElementByName(equals).Click();
            Assert.AreEqual("Display is "+assertion, CalculatorResult.Text);
        }

        [TestMethod]
        [DataRow("Nine",   "Multiply by", "Nine", "Equals", "81")]
        [DataRow("Eight",  "Multiply by", "Nine", "Equals", "72")]
        [DataRow("Seven",  "Multiply by", "Nine", "Equals", "63")]
        [DataRow("Six",    "Multiply by", "Nine", "Equals", "54")]
        [DataRow("Five",   "Multiply by", "Nine", "Equals", "45")]

        public void Multiplication_Advanced(string input1, string operation, string input2, string equals, string assertion)
        {
            CalculatorSession.FindElementByName(input1).Click();
            CalculatorSession.FindElementByName(operation).Click();
            CalculatorSession.FindElementByName(input2).Click();
            CalculatorSession.FindElementByName(equals).Click();
            Assert.AreEqual("Display is " + assertion, CalculatorResult.Text);
        }

        [TestMethod]
        [DataRow("Nine",  "Minus", "Seven", "Equals", "2")]
        [DataRow("Eight", "Minus", "Six", "Equals", "2")]
        [DataRow("Seven", "Minus", "One", "Equals", "6")]
        [DataRow("Six",   "Minus", "Three", "Equals", "3")]
        [DataRow("Five",  "Minus", "Four", "Equals", "1")]
        public void Subtraction_Advanced(string input1, string operation, string input2, string equals, string assertion)
        {
            CalculatorSession.FindElementByName(input1).Click();
            CalculatorSession.FindElementByName(operation).Click();
            CalculatorSession.FindElementByName(input2).Click();
            CalculatorSession.FindElementByName(equals).Click();
            Assert.AreEqual("Display is "+ assertion, CalculatorResult.Text);
        }
    }
}

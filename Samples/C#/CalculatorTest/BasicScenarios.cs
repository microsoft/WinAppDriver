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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CalculatorTest
{
    [TestClass]
    public class BasicScenarios : CalculatorTestBase
    {
        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            BaseSetup(context);
        }

        [ClassCleanup]
        public static void TearDown()
        {
            BaseTearDown();
        }

        [TestInitialize]
        public void Clear()
        {
            CalculatorSession.FindElementByName("Clear").Click();
            Assert.AreEqual("0", _GetCalculatorResultText());
        }

        [TestMethod]
        public void Addition()
        {
            CalculatorSession.FindElementByName("One").Click();
            CalculatorSession.FindElementByName("Plus").Click();
            CalculatorSession.FindElementByName("Seven").Click();
            CalculatorSession.FindElementByName("Equals").Click();
            Assert.AreEqual("8", _GetCalculatorResultText());
        }

        [TestMethod]
        public void Combination()
        {
            CalculatorSession.FindElementByAccessibilityId("num7Button").Click();
            CalculatorSession.FindElementByAccessibilityId("multiplyButton").Click();
            CalculatorSession.FindElementByAccessibilityId("num9Button").Click();
            CalculatorSession.FindElementByAccessibilityId("plusButton").Click();
            CalculatorSession.FindElementByAccessibilityId("num1Button").Click();
            CalculatorSession.FindElementByAccessibilityId("equalButton").Click();
            CalculatorSession.FindElementByAccessibilityId("divideButton").Click();
            CalculatorSession.FindElementByAccessibilityId("num8Button").Click();
            CalculatorSession.FindElementByAccessibilityId("equalButton").Click();
            Assert.AreEqual("8", _GetCalculatorResultText());
        }

        [TestMethod]
        public void Division()
        {
            CalculatorSession.FindElementByName("Eight").Click();
            CalculatorSession.FindElementByName("Eight").Click();
            CalculatorSession.FindElementByName("Divide by").Click();
            CalculatorSession.FindElementByName("One").Click();
            CalculatorSession.FindElementByName("One").Click();
            CalculatorSession.FindElementByName("Equals").Click();
            Assert.AreEqual("8", _GetCalculatorResultText());
        }

        [TestMethod]
        public void Multiplication()
        {
            CalculatorSession.FindElementByName("Nine").Click();
            CalculatorSession.FindElementByName("Multiply by").Click();
            CalculatorSession.FindElementByName("Nine").Click();
            CalculatorSession.FindElementByName("Equals").Click();
            Assert.AreEqual("81", _GetCalculatorResultText());
        }

        [TestMethod]
        public void Subtraction()
        {
            CalculatorSession.FindElementByName("Nine").Click();
            CalculatorSession.FindElementByName("Minus").Click();
            CalculatorSession.FindElementByName("One").Click();
            CalculatorSession.FindElementByName("Equals").Click();
            Assert.AreEqual("8", _GetCalculatorResultText());
        }
    }
}

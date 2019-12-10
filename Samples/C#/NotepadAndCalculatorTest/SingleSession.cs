//******************************************************************************
//
// Copyright (c) 2017 Microsoft Corporation. All rights reserved.
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
using OpenQA.Selenium.Appium.Windows;
using System.Threading;
using System;
using System.Runtime.CompilerServices;

namespace NotepadCalculatorTest
{
    [TestClass]
    public class SingleSession
    {
        private static NotepadSession notepadSession;
        private static CalculatorSession calculatorSession;
     
        [TestMethod]
        [DataRow("One",   "Plus",      "Seven")]
        [DataRow("Nine",  "Minus",     "One")]
        [DataRow("Eight", "Divide by", "Eight")]
        public void Templatized(string input1, string operation, string input2)
        {
            // we (re) start with our notepad session
            notepadSession.Session.SwitchTo();
            notepadSession.NotepadMainEditBox.SendKeys($"{input1} {operation} {input2} = ");

            // now let's switch to calculator
            calculatorSession.Session.SwitchTo();

            // Run sequence of button presses specified above and validate the results
            calculatorSession.Session.FindElementByName(input1).Click();
            calculatorSession.Session.FindElementByName(operation).Click();
            calculatorSession.Session.FindElementByName(input2).Click();
            calculatorSession.Session.FindElementByName("Equals").Click();

            // and back to notepad for the result 
            notepadSession.Session.SwitchTo();
            notepadSession.NotepadMainEditBox.SendKeys($"{GetCalculatorResultText()}\n");
        }

        [TestInitialize]
        public void TestInitialize()
        {
            notepadSession.Cleanup();
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext) 
        {
            notepadSession = new NotepadSession();
            calculatorSession = new CalculatorSession();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            calculatorSession.Dispose();
            notepadSession.Dispose();
        }

        private string GetCalculatorResultText()
        {
            return calculatorSession.CalculatorResult.Text.Replace("Display is", string.Empty).Trim();
        }
    }
}

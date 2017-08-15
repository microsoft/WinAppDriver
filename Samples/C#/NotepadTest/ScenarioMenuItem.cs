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

namespace NotepadTest
{
    [TestClass]
    public class ScenarioMenuItem : NotepadSession
    {
        [TestMethod]
        public void MenuItemEdit()
        {
            // Select Edit -> Time/Date to get Time/Date from Notepad
            Assert.AreEqual(string.Empty, editBox.Text);
            session.FindElementByName("Edit").Click();
            session.FindElementByXPath($"//MenuItem[starts-with(@Name, \"Time/Date\")]").Click();
            string timeDateString = editBox.Text;
            Assert.AreNotEqual(string.Empty, timeDateString);

            // Select all text, copy, and paste it twice using MenuItem Edit -> Select All, Copy, and Paste
            session.FindElementByName("Edit").Click();
            session.FindElementByXPath($"//MenuItem[starts-with(@Name, \"Select All\")]").Click();
            session.FindElementByName("Edit").Click();
            session.FindElementByXPath($"//MenuItem[starts-with(@Name, \"Copy\")]").Click();
            session.FindElementByName("Edit").Click();
            session.FindElementByXPath($"//MenuItem[starts-with(@Name, \"Paste\")]").Click();
            session.FindElementByName("Edit").Click();
            session.FindElementByXPath($"//MenuItem[starts-with(@Name, \"Paste\")]").Click();

            // Verify that the Time/Date string is duplicated
            Assert.AreEqual(timeDateString + timeDateString, editBox.Text);
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TearDown();
        }
    }
}

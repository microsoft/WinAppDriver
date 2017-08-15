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
using OpenQA.Selenium;
using System.Threading;
using System;

namespace NotepadTest
{
    [TestClass]
    public class ScenarioEditor : NotepadSession
    {
        [TestMethod]
        public void EditorEnterText()
        {
            // Type mixed text and apply shift modifier to 7890_ to generate corresponding symbols
            Thread.Sleep(TimeSpan.FromSeconds(2));
            editBox.SendKeys("abcdeABCDE 12345" + Keys.Shift + "7890-" + Keys.Shift + @"!@#$%");
            Assert.AreEqual(@"abcdeABCDE 12345&*()_!@#$%", editBox.Text);
        }

        [TestMethod]
        public void EditorKeyboardShortcut()
        {
            // Type a known text sequence, select, copy, and paste it three times
            editBox.SendKeys("789");
            editBox.SendKeys(Keys.Control + "a" + Keys.Control); // Select all using Ctrl + A keyboard shortcut
            editBox.SendKeys(Keys.Control + "c" + Keys.Control); // Copy using Ctrl + C keyboard shortcut
            editBox.SendKeys(Keys.Control + "vvv" + Keys.Control); // Paste 3 times using Ctrl + V keyboard shortcut
            Assert.AreEqual("789789789", editBox.Text);
        }

        [TestMethod]
        public void EditorNonPrintableShortcutKey()
        {
            // Press F5 to get Time/Date from Notepad
            Assert.AreEqual(string.Empty, editBox.Text);
            editBox.SendKeys(Keys.F5);
            Assert.AreNotEqual(string.Empty, editBox.Text);
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

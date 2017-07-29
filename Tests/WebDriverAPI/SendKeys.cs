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
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium;
using System;

namespace WebDriverAPI
{
    [TestClass]
    public class SendKeys
    {
        protected static WindowsDriver<WindowsElement> session;
        protected static WindowsElement editBox;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            session = Utility.CreateNewSession(CommonTestSettings.NotepadAppId);
            Assert.IsNotNull(session);

            editBox = session.FindElementByClassName("Edit");
            Assert.IsNotNull(editBox);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            if (session != null)
            {
                ClearEditBox();
                session.Quit();
                session = null;
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            ClearEditBox();
        }

        public static void ClearEditBox()
        {
            // Select all text and delete using keyboard shortcut Ctrl + A and Delete
            editBox.Click();
            session.Keyboard.PressKey(Keys.Control + "a" + Keys.Control);
            session.Keyboard.PressKey(Keys.Delete);
            Assert.AreEqual(string.Empty, editBox.Text);
        }

        [TestMethod]
        public void SendKeys_Alphabet()
        {
            session.Keyboard.PressKey("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ");
            Assert.AreEqual("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", editBox.Text);
        }

        [TestMethod]
        public void SendKeys_EmptySequence()
        {
            session.Keyboard.PressKey(string.Empty);
            Assert.AreEqual(string.Empty, editBox.Text);
        }

        [TestMethod]
        public void SendKeys_ModifierAlt()
        {
            session.Keyboard.PressKey(Keys.Alt + "ED" + Keys.Alt); // Insert Time/Date
            Assert.IsTrue(editBox.Text.Length > 0);
        }

        [TestMethod]
        public void SendKeys_ModifierControl()
        {
            session.Keyboard.PressKey("789");
            session.Keyboard.PressKey(Keys.Control + "a" + Keys.Control); // Select all
            session.Keyboard.PressKey(Keys.Control + "c" + Keys.Control); // Copy
            session.Keyboard.PressKey(Keys.Control + "vvv" + Keys.Control); // Paste 3 times
            Assert.AreEqual("789789789", editBox.Text);
        }

        [TestMethod]
        public void SendKeys_ModifierExplicitRelease()
        {
            // Keys persist all modifier between API call and requires ecplicit modifier release
            session.Keyboard.PressKey(Keys.Shift + "abcwxyz1237890"); // Shift modifier is still pressed
            session.Keyboard.PressKey("abcwxyz1237890" + Keys.Shift);
            session.Keyboard.PressKey("abcwxyz1237890");
            Assert.AreEqual("ABCWXYZ!@#&*()ABCWXYZ!@#&*()abcwxyz1237890", editBox.Text);
        }

        [TestMethod]
        public void SendKeys_ModifierShift()
        {
            session.Keyboard.PressKey(Keys.Shift + "abcdefghijklmnopqrstuvwxyz\n1234567890\t`-=[]\\;',./" + Keys.Shift);
            Assert.AreEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZ\r\n!@#$%^&*()\t~_+{}|:\"<>?", editBox.Text); // Assumes 101 keys US Keyboard layout
        }

        [TestMethod]
        public void SendKeys_ModifierWindowsKey()
        {
            WindowsDriver<WindowsElement> desktopSession = Utility.CreateNewSession(CommonTestSettings.DesktopAppId);
            Assert.IsNotNull(desktopSession);

            // Launch action center using Window Keys + A
            session.Keyboard.PressKey(Keys.Command + "a" + Keys.Command);
            WindowsElement actionCenterElement = null;

            // Before Windows 10 Anniversary and Creators Update Action Center name had lower case c for "center"
            try
            {
                actionCenterElement = desktopSession.FindElementByName("Action Center");
            }
            catch
            {
                actionCenterElement = desktopSession.FindElementByName("Action center");
            }

            Assert.IsNotNull(actionCenterElement);

            // Dismiss action center and cleanup the temporary session
            actionCenterElement.SendKeys(Keys.Escape);
            editBox.Click();
            desktopSession.Quit();
        }

        [TestMethod]
        public void SendKeys_NonPrintableKeys()
        {
            session.Keyboard.PressKey("9");
            session.Keyboard.PressKey(Keys.Home + "8");
            session.Keyboard.PressKey(Keys.Home + "7");
            session.Keyboard.PressKey(Keys.Home + Keys.Tab);
            session.Keyboard.PressKey(Keys.Home + Keys.Enter);
            session.Keyboard.PressKey(Keys.Up + Keys.Tab + "78");
            session.Keyboard.PressKey(Keys.Home + Keys.Enter);
            session.Keyboard.PressKey(Keys.Up + Keys.Tab + "7");
            Assert.AreEqual("\t7\r\n\t78\r\n\t789", editBox.Text);
        }

        [TestMethod]
        public void SendKeys_Number()
        {
            session.Keyboard.PressKey("0123456789");
            Assert.AreEqual("0123456789", editBox.Text);
        }

        [TestMethod]
        public void SendKeys_SymbolsEscapedCharacter()
        {
            // Line endings such as \r or \n are replaced with \r\n
            // form feeds (\f) or vertical tab feeds (\v) are removed
            session.Keyboard.PressKey("\a\b\f\n\r\t\v\'\"\\\r\n");
            Assert.AreEqual("\r\n\r\n\t\'\"\\\r\n\r\n", editBox.Text);
        }

        [TestMethod]
        public void SendKeys_SymbolsKeys()
        {
            session.Keyboard.PressKey("`-=[]\\;',./~!@#$%^&*()_+{}|:\"<>?");
            Assert.AreEqual("`-=[]\\;',./~!@#$%^&*()_+{}|:\"<>?", editBox.Text);
        }

        [TestMethod]
        public void SendKeysError_NoSuchWindow()
        {
            try
            {
                Utility.GetOrphanedSession().Keyboard.PressKey("keys");
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }
    }
}

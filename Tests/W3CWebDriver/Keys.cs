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
using OpenQA.Selenium.Appium.Windows;

namespace W3CWebDriver
{
    [TestClass]
    public class Keys
    {
        protected static WindowsDriver<WindowsElement> session;
        protected static WindowsElement editBox;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            session = Utility.CreateNewSession(CommonTestSettings.NotepadAppId);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);

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
            session.Keyboard.PressKey(OpenQA.Selenium.Keys.Control + "a" + OpenQA.Selenium.Keys.Control);
            session.Keyboard.PressKey(OpenQA.Selenium.Keys.Delete);
            Assert.AreEqual(String.Empty, editBox.Text);
        }

        [TestMethod]
        public void SendAlphabetLowerCase()
        {
            session.Keyboard.PressKey("abcdefghijklmnopqrstuvwxyz");
            Assert.AreEqual("abcdefghijklmnopqrstuvwxyz", editBox.Text);
        }

        [TestMethod]
        public void SendAlphabetUpperCase()
        {
            session.Keyboard.PressKey("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            Assert.AreEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZ", editBox.Text);
        }

        [TestMethod]
        public void SendEmptySequence()
        {
            session.Keyboard.PressKey(String.Empty);
            Assert.AreEqual(String.Empty, editBox.Text);
        }

        [TestMethod]
        public void SendModifierAlt()
        {
            session.Keyboard.PressKey(OpenQA.Selenium.Keys.Alt + "ED" + OpenQA.Selenium.Keys.Alt); // Insert Time/Date
            Assert.IsTrue(editBox.Text.Length > 0);
        }

        [TestMethod]
        public void SendModifierControl()
        {
            session.Keyboard.PressKey("789");
            session.Keyboard.PressKey(OpenQA.Selenium.Keys.Control + "a" + OpenQA.Selenium.Keys.Control); // Select all
            session.Keyboard.PressKey(OpenQA.Selenium.Keys.Control + "c" + OpenQA.Selenium.Keys.Control); // Copy
            session.Keyboard.PressKey(OpenQA.Selenium.Keys.Control + "vvv" + OpenQA.Selenium.Keys.Control); // Paste 3 times
            Assert.AreEqual("789789789", editBox.Text);
        }

        [TestMethod]
        public void SendModifierExplicitRelease()
        {
            // Keys persist all modifier between API call and requires ecplicit modifier release
            session.Keyboard.PressKey(OpenQA.Selenium.Keys.Shift + "abcdefghijklmnopqrstuvwxyz1234567890"); // Shift modifier is still pressed
            session.Keyboard.PressKey("abcdefghijklmnopqrstuvwxyz1234567890" + OpenQA.Selenium.Keys.Shift);
            session.Keyboard.PressKey("abcdefghijklmnopqrstuvwxyz1234567890");
            Assert.AreEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%^&*()ABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%^&*()abcdefghijklmnopqrstuvwxyz1234567890", editBox.Text);
        }

        [TestMethod]
        public void SendModifierShift()
        {
            session.Keyboard.PressKey(OpenQA.Selenium.Keys.Shift + "abcdefghijklmnopqrstuvwxyz\n1234567890\t`-=[]\\;',./" + OpenQA.Selenium.Keys.Shift);
            Assert.AreEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZ\r\n!@#$%^&*()\t~_+{}|:\"<>?", editBox.Text); // Assumes 101 keys US Keyboard layout
        }

        [TestMethod]
        public void SendNonPrintableKeys()
        {
            session.Keyboard.PressKey("9");
            session.Keyboard.PressKey(OpenQA.Selenium.Keys.Home + "8");
            session.Keyboard.PressKey(OpenQA.Selenium.Keys.Home + "7");
            session.Keyboard.PressKey(OpenQA.Selenium.Keys.Home + OpenQA.Selenium.Keys.Tab);
            session.Keyboard.PressKey(OpenQA.Selenium.Keys.Home + OpenQA.Selenium.Keys.Enter);
            session.Keyboard.PressKey(OpenQA.Selenium.Keys.Up + OpenQA.Selenium.Keys.Tab + "78");
            session.Keyboard.PressKey(OpenQA.Selenium.Keys.Home + OpenQA.Selenium.Keys.Enter);
            session.Keyboard.PressKey(OpenQA.Selenium.Keys.Up + OpenQA.Selenium.Keys.Tab + "7");
            Assert.AreEqual("\t7\r\n\t78\r\n\t789", editBox.Text);
        }

        [TestMethod]
        public void SendNumber()
        {
            session.Keyboard.PressKey("0123456789");
            Assert.AreEqual("0123456789", editBox.Text);
        }

        [TestMethod]
        public void SendSymbolsEscapedCharacter()
        {
            // Line endings such as \r or \n are replaced with \r\n
            // form feeds (\f) or vertical tab feeds (\v) are removed
            session.Keyboard.PressKey("\a\b\f\n\r\t\v\'\"\\\r\n");
            Assert.AreEqual("\r\n\r\n\t\'\"\\\r\n\r\n", editBox.Text);
        }

        [TestMethod]
        public void SendSymbolsKeys()
        {
            session.Keyboard.PressKey("`-=[]\\;',./~!@#$%^&*()_+{}|:\"<>?");
            Assert.AreEqual("`-=[]\\;',./~!@#$%^&*()_+{}|:\"<>?", editBox.Text);
        }

        [TestMethod]
        public void SendModifierWindowsKey()
        {
            WindowsDriver<WindowsElement> desktopSession = Utility.CreateNewSession(CommonTestSettings.DesktopAppId);
            Assert.IsNotNull(desktopSession);

            // Launch action center using Window Keys + A
            session.Keyboard.PressKey(OpenQA.Selenium.Keys.Command + "a" + OpenQA.Selenium.Keys.Command);
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
            actionCenterElement.SendKeys(OpenQA.Selenium.Keys.Escape);
            editBox.Click();
            desktopSession.Quit();
        }
    }
}

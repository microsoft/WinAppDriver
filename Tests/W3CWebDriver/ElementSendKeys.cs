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
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Remote;

namespace W3CWebDriver
{
    [TestClass]
    public class ElementSendKeys
    {
        protected static IOSDriver<IOSElement> session;
        protected static IOSElement editBox;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.NotepadAppId);
            session = new IOSDriver<IOSElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
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
            editBox.SendKeys(OpenQA.Selenium.Keys.Control + "a");
            editBox.SendKeys(OpenQA.Selenium.Keys.Delete);
            Assert.AreEqual(String.Empty, editBox.Text);
        }

        [TestMethod]
        public void SendAlphabetLowerCase()
        {
            editBox.SendKeys("abcdefghijklmnopqrstuvwxyz");
            Assert.AreEqual("abcdefghijklmnopqrstuvwxyz", editBox.Text);
        }

        [TestMethod]
        public void SendAlphabetUpperCase()
        {
            editBox.SendKeys("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            Assert.AreEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZ", editBox.Text);
        }

        [TestMethod]
        public void SendEmptySequence()
        {
            editBox.SendKeys(String.Empty);
            Assert.AreEqual(String.Empty, editBox.Text);
        }

        [TestMethod]
        public void SendModifierAlt()
        {
            editBox.SendKeys(OpenQA.Selenium.Keys.Alt + "ED" + OpenQA.Selenium.Keys.Alt); // Insert Time/Date
            Assert.IsTrue(editBox.Text.Length > 0);
        }

        [TestMethod]
        public void SendModifierControl()
        {
            editBox.SendKeys("789");
            editBox.SendKeys(OpenQA.Selenium.Keys.Control + "a" + OpenQA.Selenium.Keys.Control); // Select all
            editBox.SendKeys(OpenQA.Selenium.Keys.Control + "c" + OpenQA.Selenium.Keys.Control); // Copy
            editBox.SendKeys(OpenQA.Selenium.Keys.Control + "vvv" + OpenQA.Selenium.Keys.Control); // Paste 3 times
            Assert.AreEqual("789789789", editBox.Text);
        }

        [TestMethod]
        public void SendModifierImplicitRelease()
        {
            // SendKeys implicitly depress all modifier at the end of the sequence (every API call)
            editBox.SendKeys(OpenQA.Selenium.Keys.Shift + "abcdefghijklmnopqrstuvwxyz1234567890"); // Implicit shift release at the end of the sequence
            editBox.SendKeys("abcdefghijklmnopqrstuvwxyz1234567890" + OpenQA.Selenium.Keys.Shift); 
            editBox.SendKeys("abcdefghijklmnopqrstuvwxyz1234567890");
            Assert.AreEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%^&*()abcdefghijklmnopqrstuvwxyz1234567890abcdefghijklmnopqrstuvwxyz1234567890", editBox.Text);
        }

        [TestMethod]
        public void SendModifierShift()
        {
            editBox.SendKeys(OpenQA.Selenium.Keys.Shift + "abcdefghijklmnopqrstuvwxyz1234567890`-=[]\\;',./" + OpenQA.Selenium.Keys.Shift);
            Assert.AreEqual("ABCDEFGHIJKLMNOPQRSTUVWXYZ!@#$%^&*()~_+{}|:\"<>?", editBox.Text); // Assumes 101 keys US Keyboard layout
        }

        [TestMethod]
        public void SendNonPrintableKeys()
        {
            editBox.SendKeys("9");
            editBox.SendKeys(OpenQA.Selenium.Keys.Home + "8");
            editBox.SendKeys(OpenQA.Selenium.Keys.Home + "7");
            editBox.SendKeys(OpenQA.Selenium.Keys.Home + OpenQA.Selenium.Keys.Tab);
            editBox.SendKeys(OpenQA.Selenium.Keys.Home + OpenQA.Selenium.Keys.Enter);
            editBox.SendKeys(OpenQA.Selenium.Keys.Up + OpenQA.Selenium.Keys.Tab + "78");
            editBox.SendKeys(OpenQA.Selenium.Keys.Home + OpenQA.Selenium.Keys.Enter);
            editBox.SendKeys(OpenQA.Selenium.Keys.Up + OpenQA.Selenium.Keys.Tab + "7");
            Assert.AreEqual("\t7\r\n\t78\r\n\t789", editBox.Text);
        }

        [TestMethod]
        public void SendNumber()
        {
            editBox.SendKeys("0123456789");
            Assert.AreEqual("0123456789", editBox.Text);
        }

        [TestMethod]
        public void SendSymbolsEscapedCharacter()
        {
            // Line endings such as \r or \n are replaced with \r\n
            // form feeds (\f) or vertical tab feeds (\v) are removed
            editBox.SendKeys("\a\b\f\n\r\t\v\'\"\\\r\n");
            Assert.AreEqual("\r\n\r\n\t\'\"\\\r\n\r\n", editBox.Text);
        }

        [TestMethod]
        public void SendSymbolsKeys()
        {
            editBox.SendKeys("`-=[]\\;',./~!@#$%^&*()_+{}|:\"<>?");
            Assert.AreEqual("`-=[]\\;',./~!@#$%^&*()_+{}|:\"<>?", editBox.Text);
        }

        [TestMethod]
        public void SendModifierWindowsKey()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", "Root");
            IOSDriver<IOSElement> desktopSession = new IOSDriver<IOSElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(desktopSession);

            // Lauch action center using Window Keys + A
            editBox.SendKeys(OpenQA.Selenium.Keys.Command + "a" + OpenQA.Selenium.Keys.Command);
            var actionCenterElement = desktopSession.FindElementByName("Action Center");
            Assert.IsNotNull(actionCenterElement);

            desktopSession.Quit();
        }
    }
}

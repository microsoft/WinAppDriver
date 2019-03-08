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
using System.Threading;

namespace WebDriverAPI
{
    [TestClass]
    public class ElementSendKeys : AlarmClockBase
    {
        private static WindowsElement alarmNameTextBox;

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

        [TestInitialize]
        public override void TestInit()
        {
            // Open new alarm page if app is currently in different view
            try
            {
                alarmNameTextBox = session.FindElementByAccessibilityId("AlarmNameTextBox");
            }
            catch
            {
                base.TestInit();
                Thread.Sleep(TimeSpan.FromSeconds(1));
                session.FindElementByAccessibilityId("AddAlarmButton").Click();
                Thread.Sleep(TimeSpan.FromSeconds(1.5));
                alarmNameTextBox = session.FindElementByAccessibilityId("AlarmNameTextBox");
            }

            // Select all text and delete using keyboard shortcut Ctrl + A and Delete
            alarmNameTextBox.SendKeys(Keys.Control + "a");
            alarmNameTextBox.SendKeys(Keys.Delete);
            Assert.AreEqual(string.Empty, alarmNameTextBox.Text);
        }

        [TestMethod]
        public void SendKeysToElement_Alphabet()
        {
            alarmNameTextBox.SendKeys("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ");
            Assert.AreEqual("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", alarmNameTextBox.Text);
        }

        [TestMethod]
        public void SendKeysToElement_EmptySequence()
        {
            alarmNameTextBox.SendKeys(string.Empty);
            Assert.AreEqual(string.Empty, alarmNameTextBox.Text);
        }

        [TestMethod]
        public void SendKeysToElement_ModifierAlt()
        {
            alarmNameTextBox.SendKeys(Keys.Space);
            Assert.AreEqual("True", alarmNameTextBox.GetAttribute("HasKeyboardFocus"));
            alarmNameTextBox.SendKeys(Keys.Alt + Keys.Enter + Keys.Alt); // Alt + Enter moves the focus to the next element
            Assert.AreEqual("False", alarmNameTextBox.GetAttribute("HasKeyboardFocus"));
        }

        [TestMethod]
        public void SendKeysToElement_ModifierControl()
        {
            alarmNameTextBox.SendKeys("789");
            alarmNameTextBox.SendKeys(Keys.Control + "a" + Keys.Control); // Select all
            alarmNameTextBox.SendKeys(Keys.Control + "c" + Keys.Control); // Copy
            alarmNameTextBox.SendKeys(Keys.Control + "vvv" + Keys.Control); // Paste 3 times
            Assert.AreEqual("789789789", alarmNameTextBox.Text);
        }

        [TestMethod]
        public void SendKeysToElement_ModifierImplicitRelease()
        {
            // SendKeys implicitly depress all modifier at the end of the sequence (every API call)
            alarmNameTextBox.SendKeys(Keys.Shift + "abcwxyz1237890"); // Implicit shift release at the end of the sequence
            alarmNameTextBox.SendKeys("abcwxyz1237890" + Keys.Shift);
            alarmNameTextBox.SendKeys("abcwxyz1237890");
            Assert.AreEqual("ABCWXYZ!@#&*()abcwxyz1237890abcwxyz1237890", alarmNameTextBox.Text);
        }

        [TestMethod]
        public void SendKeysToElement_ModifierShift()
        {
            alarmNameTextBox.SendKeys(Keys.Shift + "abcwxyz1237890`-=[]\\;',./" + Keys.Shift);
            Assert.AreEqual("ABCWXYZ!@#&*()~_+{}|:\"<>?", alarmNameTextBox.Text); // Assumes 101 keys US Keyboard layout
        }

        [TestMethod]
        public void SendKeysToElement_NonPrintableKeys()
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));
            alarmNameTextBox.SendKeys("9");
            alarmNameTextBox.SendKeys(Keys.Home + "8");
            alarmNameTextBox.SendKeys(Keys.Left + "7");
            Assert.AreEqual("789", alarmNameTextBox.Text);
        }

        [TestMethod]
        public void SendKeysToElement_Number()
        {
            alarmNameTextBox.SendKeys("0123456789");
            Assert.AreEqual("0123456789", alarmNameTextBox.Text);
        }

        [TestMethod]
        public void SendKeysToElement_SymbolsKeys()
        {
            alarmNameTextBox.SendKeys("`-=[]\\;',./~!@#$%^&*()_+{}|:\"<>?");
            Assert.AreEqual("`-=[]\\;',./~!@#$%^&*()_+{}|:\"<>?", alarmNameTextBox.Text);
        }

        [TestMethod]
        public void SendKeysToElementError_ElementNotVisible()
        {
            base.TestInit();

            // Different Alarm & Clock application version uses different UI elements
            if (AlarmTabClassName == "ListViewItem")
            {
                // The latest Alarms & Clock application destroys the previous view instead of hiding it
            }
            else
            {
                // Navigate to Stopwatch tab and attempt to click on addAlarmButton that is no longer displayed
                WindowsElement addAlarmButton = session.FindElementByAccessibilityId("AddAlarmButton");
                session.FindElementByAccessibilityId(StopwatchTabAutomationId).Click();
                Thread.Sleep(TimeSpan.FromSeconds(1));
                Assert.IsFalse(addAlarmButton.Displayed);

                try
                {
                    addAlarmButton.SendKeys("keys");
                    Assert.Fail("Exception should have been thrown");
                }
                catch (InvalidOperationException exception)
                {
                    Assert.AreEqual(ErrorStrings.ElementNotVisible, exception.Message);
                }
            }
        }

        [TestMethod]
        public void SendKeysToElementError_NoSuchWindow()
        {
            try
            {
                Utility.GetOrphanedElement().SendKeys("keys");
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.NoSuchWindow, exception.Message);
            }
        }

        [TestMethod]
        public void SendKeysToElementError_StaleElement()
        {
            try
            {
                DismissAddAlarmPage();
                Thread.Sleep(TimeSpan.FromSeconds(1));
                alarmNameTextBox.SendKeys("keys");
                Assert.Fail("Exception should have been thrown");
            }
            catch (InvalidOperationException exception)
            {
                Assert.AreEqual(ErrorStrings.StaleElementReference, exception.Message);
            }
        }
    }
}

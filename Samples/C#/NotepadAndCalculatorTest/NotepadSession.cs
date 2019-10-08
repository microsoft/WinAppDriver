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
using OpenQA.Selenium.Remote;
using OpenQA.Selenium;
using System;

namespace NotepadCalculatorTest
{
    public class NotepadSession :IDisposable
    {
        private const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        private const string NotepadAppId = @"C:\Windows\System32\notepad.exe";

        public WindowsElement NotepadMainEditBox { get; private set; }

        public NotepadSession()
        {
        
                // Create a new session to launch Notepad application
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", NotepadAppId);
            this.Session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(Session);
            Assert.IsNotNull(Session.SessionId);

            // Verify that Notepad is started with untitled new file
            Assert.AreEqual("Untitled - Notepad", Session.Title);

            // Set implicit timeout to 1.5 seconds to make element search to retry every 500 ms for at most three times
            Session.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1.5);

            NotepadMainEditBox = Session.FindElementByClassName("Edit");
        }


        public void Cleanup()
        {
            // Select all text and delete to clear the edit box
            NotepadMainEditBox.SendKeys(Keys.Control + "a" + Keys.Control);
            NotepadMainEditBox.SendKeys(Keys.Delete);
            Assert.AreEqual(string.Empty, NotepadMainEditBox.Text);
        }

        public WindowsDriver<WindowsElement> Session { get; }
        
        public void Dispose()
        {
            // Close the application and delete the session
            if (Session != null)
            {
                NotepadMainEditBox = null;

                Session.Close();

                try
                {
                    // Dismiss Save dialog if it is blocking the exit
                    Session.FindElementByName("Don't Save").Click();
                }
                catch { }

                Session.Quit();
            }
        }
    }
}
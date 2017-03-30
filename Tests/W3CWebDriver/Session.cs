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
using System.IO;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;

namespace W3CWebDriver
{
    [TestClass]
    public class Session
    {
        private WindowsDriver<WindowsElement> session = null;

        [TestMethod]
        public void CreateSessionClassicApp()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.NotepadAppId);
            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);
            session.Quit();
            session = null;
        }

        [TestMethod]
        public void CreateSessionDesktop()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", "Root");
            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);
            session.Quit();
            session = null;
        }

        [TestMethod]
        public void CreateSessionModernApp()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.CalculatorAppId);
            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);
            session.Quit();
            session = null;
        }

        [TestMethod]
        public void CreateSessionSystemApp()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.ExplorerAppId);
            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);
            session.Quit();
            session = null;
        }

        [TestMethod]
        public void CreateSessionWithSupportedDesiredCapabilities()
        {
            DesiredCapabilities supportedCapabilities = new DesiredCapabilities();
            supportedCapabilities.SetCapability("app", CommonTestSettings.CalculatorAppId);
            supportedCapabilities.SetCapability("platformVersion", "10");
            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), supportedCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);
            session.Quit();
            session = null;
        }

        [TestMethod]
        public void CreateSessionWithUnsupportedRequiredCapabilities()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.CalculatorAppId);
            appCapabilities.SetCapability("anUnsupportedCapabilities", "Unsupported Capabilities Value");
            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);
            session.Quit();
            session = null;
        }

        [TestMethod]
        public void DeleteSessionClassicApp()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.NotepadAppId);
            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);

            session.Quit();
            Assert.IsNull(session.SessionId);
            session = null;
        }

        [TestMethod]
        public void DeleteSessionDesktop()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", "Root");
            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);

            session.Quit();
            Assert.IsNull(session.SessionId);
            session = null;
        }

        [TestMethod]
        public void DeleteSessionModernApp()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.CalculatorAppId);
            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);

            session.Quit();
            Assert.IsNull(session.SessionId);
            session = null;
        }

        [TestMethod]
        public void DeleteSessionSystemApp()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.ExplorerAppId);
            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);

            session.Quit();
            Assert.IsNull(session.SessionId);
            session = null;
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorCreateSessionBadArgumentAppId()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", "BadAppId");
            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorCreateSessionBadArgumentAppIdClassicApp()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", @"C:\Windows\System32\BadAppId.exe");
            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorCreateSessionBadArgumentAppIdModernApp()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", "Microsoft.BadAppId!App");
            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorCreateSessionMissingArgumentAppId()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities(); // Leave capabilities empty
            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Net.WebException))]
        public void ErrorInvalidSessionId()
        {
            HttpWebResponse response = WebRequest.Create(CommonTestSettings.WindowsApplicationDriverUrl + "/session/" + "INVALID-SESSION-ID").GetResponse() as HttpWebResponse;
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ErrorStaleSessionId()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.AlarmClockAppId);

            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);

            string applicationTitle = session.Title;
            Assert.IsNotNull(applicationTitle);
            session.Quit();
            applicationTitle = session.Title;
            Assert.Fail("Exception should have been thrown");
        }

        [TestMethod]
        public void GetSessionCapabilities()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.AlarmClockAppId);

            session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);

            using (HttpWebResponse response = WebRequest.Create(CommonTestSettings.WindowsApplicationDriverUrl + "/session/" + session.SessionId).GetResponse() as HttpWebResponse)
            {
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                Assert.AreEqual("{\"sessionId\":\"" + session.SessionId + "\",\"status\":0,\"value\":{\"app\":\"Microsoft.WindowsAlarms_8wekyb3d8bbwe!App\",\"platformName\":\"Windows\"}}", responseString);
            }

            session.Quit();
            session = null;
        }

        [TestMethod]
        public void MultipleSessions()
        {
            WindowsDriver<WindowsElement> session1 = null;
            WindowsDriver<WindowsElement> session2 = null;

            try
            {
                DesiredCapabilities appCapabilities = new DesiredCapabilities();
                appCapabilities.SetCapability("app", CommonTestSettings.NotepadAppId);

                session1 = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
                Assert.IsNotNull(session1);
                Assert.IsNotNull(session1.SessionId);

                session2 = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
                Assert.IsNotNull(session2);
                Assert.IsNotNull(session2.SessionId);

                Assert.AreNotEqual(session1.SessionId, session2.SessionId);
                Assert.AreNotEqual(session1.CurrentWindowHandle, session2.CurrentWindowHandle);
                session1.Quit();
                session1 = null;
                session2.Quit();
                session2 = null;
            }
            finally
            {
                if (session1 != null)
                {
                    session1.Quit();
                }
                if (session2 != null)
                {
                    session2.Quit();
                }
            }
        }

        [TestMethod]
        public void MultipleSessionsSingleInstanceApplication()
        {
            WindowsDriver<WindowsElement> session1 = null;
            WindowsDriver<WindowsElement> session2 = null;

            try
            {
                DesiredCapabilities appCapabilities = new DesiredCapabilities();
                appCapabilities.SetCapability("app", CommonTestSettings.AlarmClockAppId);

                session1 = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
                Assert.IsNotNull(session1);
                Assert.IsNotNull(session1.SessionId);

                session2 = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
                Assert.IsNotNull(session2);
                Assert.IsNotNull(session2.SessionId);

                Assert.AreNotEqual(session1.SessionId, session2.SessionId);
                session1.Quit();
                session1 = null;
                session2.Quit();
                session2 = null;
            }
            finally
            {
                if (session1 != null)
                {
                    session1.Quit();
                }
                if (session2 != null)
                {
                    session2.Quit();
                }
            }

        }
    }
}

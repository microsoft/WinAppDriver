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

using System.IO;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium.Appium.Windows;

namespace WebDriverAPI
{
    [TestClass]
    public class Sessions
    {
        [TestMethod]
        public void GetSessions_CurrentList()
        {
            using (HttpWebResponse response = WebRequest.Create(CommonTestSettings.WindowsApplicationDriverUrl + "/sessions").GetResponse() as HttpWebResponse)
            {
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                JObject responseObject = JObject.Parse(responseString);
                Assert.AreEqual(0, (int)responseObject["status"]);

                JArray capabilitiesArray = (JArray)responseObject["value"];
                Assert.IsNotNull(capabilitiesArray);
                Assert.IsTrue(capabilitiesArray.Count >= 0);
            }
        }

        [TestMethod]
        public void GetSessions_SingleEntry()
        {
            WindowsDriver<WindowsElement> session = Utility.CreateNewSession(CommonTestSettings.AlarmClockAppId);
            Assert.IsNotNull(session);

            using (HttpWebResponse response = WebRequest.Create(CommonTestSettings.WindowsApplicationDriverUrl + "/sessions").GetResponse() as HttpWebResponse)
            {
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                JObject responseObject = JObject.Parse(responseString);
                Assert.AreEqual(0, (int)responseObject["status"]);

                JArray capabilitiesArray = (JArray)responseObject["value"];
                Assert.IsTrue(capabilitiesArray.Count >= 1);

                // Verify that the newly created session is on the list
                JToken newSessionEntry = null;
                foreach (var entry in capabilitiesArray.Children())
                {
                    if (entry["id"].ToString() == session.SessionId.ToString())
                    {
                        newSessionEntry = entry;
                        break;
                    }
                }

                Assert.IsNotNull(newSessionEntry);
                Assert.AreEqual(CommonTestSettings.AlarmClockAppId, newSessionEntry["capabilities"]["app"].ToString());
            }

            session.Quit();
        }

        [TestMethod]
        public void GetSessions_MultipleEntry()
        {
            WindowsDriver<WindowsElement> calculatorSession = Utility.CreateNewSession(CommonTestSettings.CalculatorAppId);
            Assert.IsNotNull(calculatorSession);

            WindowsDriver<WindowsElement> notepadSession = Utility.CreateNewSession(CommonTestSettings.NotepadAppId);
            Assert.IsNotNull(notepadSession);

            int openSessionsCount = 0;

            using (HttpWebResponse response = WebRequest.Create(CommonTestSettings.WindowsApplicationDriverUrl + "/sessions").GetResponse() as HttpWebResponse)
            {
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                JObject responseObject = JObject.Parse(responseString);
                Assert.AreEqual(0, (int)responseObject["status"]);

                // There needs to be at least 2 open sessions after we create the 2 session above
                JArray capabilitiesArray = (JArray)responseObject["value"];
                openSessionsCount = capabilitiesArray.Count;
                Assert.IsTrue(openSessionsCount >= 2);

                // Verify that both calculator and notepad sessions are created
                JToken calculatorSessionEntry = null;
                JToken notepadSessionEntry = null;
                foreach (var entry in capabilitiesArray.Children())
                {
                    if (entry["id"].ToString() == calculatorSession.SessionId.ToString())
                    {
                        calculatorSessionEntry = entry;
                    }
                    else if (entry["id"].ToString() == notepadSession.SessionId.ToString())
                    {
                        notepadSessionEntry = entry;
                    }

                    if (calculatorSessionEntry != null && notepadSessionEntry != null)
                    {
                        break;
                    }
                }

                Assert.IsNotNull(calculatorSessionEntry);
                Assert.AreEqual(CommonTestSettings.CalculatorAppId, calculatorSessionEntry["capabilities"]["app"].ToString());
                Assert.IsNotNull(notepadSessionEntry);
                Assert.AreEqual(CommonTestSettings.NotepadAppId, notepadSessionEntry["capabilities"]["app"].ToString());
            }

            // Close the newly created sessions
            calculatorSession.Quit();
            notepadSession.Quit();

            using (HttpWebResponse response = WebRequest.Create(CommonTestSettings.WindowsApplicationDriverUrl + "/sessions").GetResponse() as HttpWebResponse)
            {
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                JObject responseObject = JObject.Parse(responseString);
                Assert.AreEqual(0, (int)responseObject["status"]);

                // There needs to be 2 less sessions after we closed the 2 sessions above
                JArray capabilitiesArray = (JArray)responseObject["value"];
                Assert.AreEqual(openSessionsCount - 2, capabilitiesArray.Count);
            }
        }
    }
}

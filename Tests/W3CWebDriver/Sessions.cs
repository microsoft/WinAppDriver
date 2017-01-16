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
using Newtonsoft.Json.Linq;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;

namespace W3CWebDriver
{
    [TestClass]
    public class Sessions
    {
        [TestInitialize]
        public void CloseAllActiveSessions()
        {
            // Close all active sessions in the driver
            using (HttpWebResponse response = WebRequest.Create(CommonTestSettings.WindowsApplicationDriverUrl + "/sessions").GetResponse() as HttpWebResponse)
            {
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                JObject responseObject = JObject.Parse(responseString);
                Assert.AreEqual(0, (int)responseObject["status"]);

                JArray capabilitiesArray = (JArray)responseObject["value"];
                foreach (var entry in capabilitiesArray)
                {
                    var request = WebRequest.Create(CommonTestSettings.WindowsApplicationDriverUrl + "/session/" + entry["id"].ToString());
                    request.Method = "DELETE";
                    request.GetResponse();
                }
            }
        }

        [TestMethod]
        public void GetSessionsEmptyList()
        {
            using (HttpWebResponse response = WebRequest.Create(CommonTestSettings.WindowsApplicationDriverUrl + "/sessions").GetResponse() as HttpWebResponse)
            {
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                Assert.AreEqual("{\"status\":0,\"value\":[]}", responseString);
            }
        }

        [TestMethod]
        public void GetSessionsSingleEntry()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.AlarmClockAppId);
            WindowsDriver<WindowsElement> session = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);

            using (HttpWebResponse response = WebRequest.Create(CommonTestSettings.WindowsApplicationDriverUrl + "/sessions").GetResponse() as HttpWebResponse)
            {
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                JObject responseObject = JObject.Parse(responseString);
                Assert.AreEqual(0, (int)responseObject["status"]);

                JArray capabilitiesArray = (JArray)responseObject["value"];
                Assert.AreEqual(1, capabilitiesArray.Count);

                JToken firstEntry = capabilitiesArray[0];
                Assert.AreEqual(session.SessionId.ToString(), firstEntry["id"].ToString());
                Assert.AreEqual(CommonTestSettings.AlarmClockAppId, firstEntry["capabilities"]["app"].ToString());
            }

            session.Quit();
        }

        [TestMethod]
        public void GetSessionsMultipleEntry()
        {
            DesiredCapabilities alarmAppCapabilities = new DesiredCapabilities();
            alarmAppCapabilities.SetCapability("app", CommonTestSettings.AlarmClockAppId);
            WindowsDriver<WindowsElement> alarmSession = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), alarmAppCapabilities);
            Assert.IsNotNull(alarmSession);

            DesiredCapabilities notepadAppCapabilities = new DesiredCapabilities();
            notepadAppCapabilities.SetCapability("app", CommonTestSettings.NotepadAppId);
            WindowsDriver<WindowsElement> notepadSession = new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), notepadAppCapabilities);
            Assert.IsNotNull(notepadSession);

            using (HttpWebResponse response = WebRequest.Create(CommonTestSettings.WindowsApplicationDriverUrl + "/sessions").GetResponse() as HttpWebResponse)
            {
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                JObject responseObject = JObject.Parse(responseString);
                Assert.AreEqual(0, (int)responseObject["status"]);

                JArray capabilitiesArray = (JArray)responseObject["value"];
                Assert.AreEqual(2, capabilitiesArray.Count);

                foreach (var entry in capabilitiesArray.Children())
                {
                    if (entry["id"].ToString() == alarmSession.SessionId.ToString())
                    {
                        Assert.AreEqual(CommonTestSettings.AlarmClockAppId, entry["capabilities"]["app"].ToString());
                    }
                    else if (entry["id"].ToString() == notepadSession.SessionId.ToString())
                    {
                        Assert.AreEqual(CommonTestSettings.NotepadAppId, entry["capabilities"]["app"].ToString());
                    }
                    else
                    {
                        Assert.Fail("This session entry is unexpected");
                    }
                }
            }

            alarmSession.Quit();
            notepadSession.Quit();
        }
    }
}

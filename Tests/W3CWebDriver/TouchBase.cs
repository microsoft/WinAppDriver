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
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Remote;
using Newtonsoft.Json.Linq;

namespace W3CWebDriver
{
    public class TouchBase
    {
        protected static IOSDriver<IOSElement> session;

        [TestInitialize]
        public void TestInit()
        {
            // Save application window original size and position
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.EdgeAppId);
            session = new IOSDriver<IOSElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);
        }

        [TestCleanup]
        public void TestClean()
        {
            if (session != null)
            {
                session.Quit();
                session = null;
            }
        }

        protected static HttpWebResponse SendTouchPost(String touchType, JObject requestObject)
        {
            var request = WebRequest.Create(CommonTestSettings.WindowsApplicationDriverUrl + "/session/" + session.SessionId + "/touch/" + touchType);
            request.Method = "POST";
            request.ContentType = "application/json";

            String postData = requestObject.ToString();
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            return request.GetResponse() as HttpWebResponse;
        }

        public void ErrorTouchClosedWindow(string touchType)
        {
            JObject enterRequestObject = new JObject();
            enterRequestObject["element"] = session.FindElementByAccessibilityId("m_newTabPageGoButton").GetAttribute("elementId");
            session.Close();
            HttpWebResponse response = SendTouchPost(touchType, enterRequestObject);
            Assert.Fail("Exception should have been thrown because there is no such window");
        }
        
        public void ErrorTouchInvalidElement(string touchType)
        {
            JObject enterRequestObject = new JObject();
            enterRequestObject["element"] = "InvalidElementId";
            HttpWebResponse response = SendTouchPost(touchType, enterRequestObject);
            Assert.Fail("Exception should have been thrown because there is no such element");
        }
        
        public void ErrorTouchStaleElement(string touchType)
        {
            Assert.IsNotNull(session.SessionId);
            var title = session.Title;
            
            GoToGitHub();

            // Make sure the page you went to is not the page you started on
            Assert.AreNotEqual(title, session.Title);

            // Create a request to touch Microsoft link
            JObject microsoftClickRequest = new JObject();
            microsoftClickRequest["element"] = session.FindElementByName("Microsoft").GetAttribute("elementId");

            // Navigate back to the original page
            JObject backRequestObject = new JObject();
            backRequestObject["element"] = session.FindElementByName("Back").GetAttribute("elementId");

            HttpWebResponse response2 = SendTouchPost("click", backRequestObject);
            Assert.IsNotNull(response2);

            System.Threading.Thread.Sleep(1000);

            // Make sure you are on the original page
            Assert.AreEqual(title, session.Title);

            // Try to touch the microsoft button
            HttpWebResponse staleResponse = SendTouchPost(touchType, microsoftClickRequest);
        }
        
        public void ErrorTouchInvalidArguments(string touchType)
        {
            JObject enterRequestObject = new JObject();
            HttpWebResponse response = SendTouchPost(touchType, enterRequestObject);
            session.Close();
            Assert.Fail("Exception should have been thrown because there are insufficient arguments");
        }

        protected void GoToGitHub()
        {
            IOSElement addressEditBox = session.FindElementByAccessibilityId("addressEditBox");
            Assert.IsNotNull(addressEditBox);

            addressEditBox.SendKeys("https://github.com/Microsoft/WinAppDriver");

            JObject enterRequestObject = new JObject();
            enterRequestObject["element"] = session.FindElementByAccessibilityId("m_newTabPageGoButton").GetAttribute("elementId");
            HttpWebResponse response = SendTouchPost("click", enterRequestObject);
            Assert.IsNotNull(response);

            System.Threading.Thread.Sleep(3000);
        }
    }
}
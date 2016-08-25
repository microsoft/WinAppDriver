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
using Newtonsoft.Json.Linq;
using System.Net;

namespace W3CWebDriver
{
    [TestClass]
    public class TouchDoubleClick : TouchBase
    {

        [TestMethod]
        public void TouchDoubleClickBackButton()
        {
            Assert.IsNotNull(session.SessionId);
            var firstTitle = session.Title;

            // Navigate forward a page
            GoToGitHub();

            // Make sure the page you went to is not the page you started on
            var secondTitle = session.Title;
            Assert.AreNotEqual(firstTitle, secondTitle);

            // Navigate forward another page
            JObject microsoftClickRequest = new JObject();
            microsoftClickRequest["element"] = session.FindElementByName("Microsoft").GetAttribute("elementId");
            HttpWebResponse bingSearchResponse = SendTouchPost("click", microsoftClickRequest);
            Assert.IsNotNull(bingSearchResponse);

            System.Threading.Thread.Sleep(1000);

            // Make sure the page you went to is not either of the previous pages
            var finalTitle = session.Title;
            Assert.AreNotEqual(firstTitle, finalTitle);
            Assert.AreNotEqual(secondTitle, finalTitle);

            JObject doubleClickRequestObject = new JObject();
            doubleClickRequestObject["element"] = session.FindElementByName("Back").GetAttribute("elementId");

            HttpWebResponse response2 = SendTouchPost("doubleclick", doubleClickRequestObject);
            Assert.IsNotNull(response2);

            System.Threading.Thread.Sleep(1000);

            Assert.AreEqual(firstTitle, session.Title);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Net.WebException))]
        public void ErrorTouchDoubleClickClosedWindow()
        {
            ErrorTouchClosedWindow("doubleclick");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Net.WebException))]
        public void ErrorTouchDoubleClickInvalidElement()
        {
            ErrorTouchInvalidElement("doubleclick");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Net.WebException))]
        public void ErrorTouchDoubleClickStaleElement()
        {
            ErrorTouchStaleElement("doubleclick");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Net.WebException))]
        public void ErrorTouchDoubleClickInvalidArguments()
        {
            ErrorTouchInvalidArguments("doubleclick");
        }
    }
}

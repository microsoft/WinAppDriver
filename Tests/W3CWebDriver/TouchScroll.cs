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
    public class TouchScroll : TouchBase
    {
        [TestMethod]
        public void TouchScrollWithoutElement()
        {
            Assert.IsNotNull(session.SessionId);

            GoToGitHub();

            JObject requestObject = new JObject();
            requestObject["xoffset"] = 0;
            requestObject["yoffset"] = 100;

            HttpWebResponse response2 = SendTouchPost("scroll", requestObject);
            Assert.IsNotNull(response2);

            System.Threading.Thread.Sleep(1000);
        }

        [TestMethod]
        public void TouchScrollOnElement()
        {
            Assert.IsNotNull(session.SessionId);

            GoToGitHub();

            JObject newTabRequestObject = new JObject();
            newTabRequestObject["element"] = session.FindElementByAccessibilityId("AddTabButton").GetAttribute("elementId");

            JObject scrollRequestObject = new JObject();
            scrollRequestObject["xoffset"] = 0;
            scrollRequestObject["yoffset"] = -100;
            scrollRequestObject["element"] = session.FindElementByName("Explore").GetAttribute("elementId");

            HttpWebResponse response2 = SendTouchPost("scroll", scrollRequestObject);
            Assert.IsNotNull(response2);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Net.WebException))]
        public void ErrorTouchScrollClosedWindow()
        {
            ErrorTouchClosedWindow("scroll");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Net.WebException))]
        public void ErrorTouchScrollInvalidElement()
        {
            ErrorTouchInvalidElement("scroll");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Net.WebException))]
        public void ErrorTouchScrollStaleElement()
        {
            ErrorTouchStaleElement("scroll");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Net.WebException))]
        public void ErrorTouchScrollInvalidArguments()
        {
            ErrorTouchInvalidArguments("scroll");
        }
    }
}

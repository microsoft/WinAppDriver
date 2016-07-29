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
    public class TouchFlick : TouchBase
    {
        [TestMethod]
        public void TouchFlickWithoutElement()
        {
            Assert.IsNotNull(session.SessionId);

            GoToGitHub();

            JObject requestObject = new JObject();
            requestObject["xspeed"] = 0;
            requestObject["yspeed"] = 1000;

            HttpWebResponse response2 = SendTouchPost("flick", requestObject);
            Assert.IsNotNull(response2);

            System.Threading.Thread.Sleep(1000);
        }

        [TestMethod]
        public void TouchFlickOnElement()
        {
            Assert.IsNotNull(session.SessionId);

            GoToGitHub();

            JObject flickRequestObject = new JObject();
            flickRequestObject["xoffset"] = 0;
            flickRequestObject["yoffset"] = -100;
            flickRequestObject["speed"] = 1;
            flickRequestObject["element"] = session.FindElementByName("Explore").GetAttribute("elementId");

            HttpWebResponse response2 = SendTouchPost("flick", flickRequestObject);
            Assert.IsNotNull(response2);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Net.WebException))]
        public void ErrorTouchFlickClosedWindow()
        {
            ErrorTouchClosedWindow("flick");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Net.WebException))]
        public void ErrorTouchFlickInvalidElement()
        {
            ErrorTouchInvalidElement("flick");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Net.WebException))]
        public void ErrorTouchFlickStaleElement()
        {
            ErrorTouchStaleElement("flick");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Net.WebException))]
        public void ErrorTouchFlickInvalidArguments()
        {
            ErrorTouchInvalidArguments("flick");
        }
    }
}

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
    public class TouchClick : TouchBase
    {

        [TestMethod]
        public void TouchSingleClick()
        {
            Assert.IsNotNull(session.SessionId);
            var title = session.Title;

            GoToGitHub();

            // Make sure the page you went to is not the page you started on
            Assert.AreNotEqual(title, session.Title);

            // Navigate back to the original page
            JObject backRequestObject = new JObject();
            backRequestObject["element"] = session.FindElementByName("Back").GetAttribute("elementId");

            HttpWebResponse response2 = SendTouchPost("click", backRequestObject);
            Assert.IsNotNull(response2);

            System.Threading.Thread.Sleep(1000);

            // Make sure you are on the original page
            Assert.AreEqual(title, session.Title);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Net.WebException))]
        public void ErrorTouchClickClosedWindow()
        {
            ErrorTouchClosedWindow("click");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Net.WebException))]
        public void ErrorTouchClickInvalidElement()
        {
            ErrorTouchInvalidElement("click");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Net.WebException))]
        public void ErrorTouchClickStaleElement()
        {
            ErrorTouchStaleElement("click");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Net.WebException))]
        public void ErrorTouchClickInvalidArguments()
        {
            ErrorTouchInvalidArguments("click");
        }
    }
}

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
using OpenQA.Selenium.Appium.iOS;
using System.Net;

namespace W3CWebDriver
{
    [TestClass]
    public class TouchLongClick : TouchBase
    {
        [TestMethod]
        public void TouchLongClickNavigationBar()
        {
            Assert.IsNotNull(session.SessionId);

            GoToGitHub();

            JObject requestObject = new JObject();
            IOSElement addressEditBox = session.FindElementByAccessibilityId("addressEditBox");
            requestObject["element"] = addressEditBox.GetAttribute("elementId");

            HttpWebResponse response2 = SendTouchPost("longclick", requestObject);
            Assert.IsNotNull(response2);

            System.Threading.Thread.Sleep(1000);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Net.WebException))]
        public void ErrorTouchLongClickClosedWindow()
        {
            ErrorTouchClosedWindow("longclick");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Net.WebException))]
        public void ErrorTouchLongClickInvalidElement()
        {
            ErrorTouchInvalidElement("longclick");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Net.WebException))]
        public void ErrorTouchLongClickStaleElement()
        {
            ErrorTouchStaleElement("longclick");
        }

        [TestMethod]
        [ExpectedException(typeof(System.Net.WebException))]
        public void ErrorTouchLongClickInvalidArguments()
        {
            ErrorTouchInvalidArguments("longclick");
        }
    }
}

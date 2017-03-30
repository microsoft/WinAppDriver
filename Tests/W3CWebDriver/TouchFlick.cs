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
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TearDown();
        }

        [TestMethod]
        public void ArbitraryFlick()
        {
            // Navigate to GitHub
            session.FindElementByAccessibilityId("addressEditBox").SendKeys(CommonTestSettings.GitHubUrl + OpenQA.Selenium.Keys.Enter);
            System.Threading.Thread.Sleep(2000); // Sleep for 2 seconds

            // Use Homepage link in GitHub page as a reference element
            var gitHubHomePageLink = session.FindElementByName("Homepage");
            Assert.IsNotNull(gitHubHomePageLink);
            Assert.IsTrue(gitHubHomePageLink.Displayed);

            // Perform flick up touch action to scroll the page down hiding the Homepage link element from the view
            // Good value typically goes around 160 - 200 pixels with diminishing delta on the bigger values
            touchScreen.Flick(0, 180);
            System.Threading.Thread.Sleep(3000); // Sleep for 3 seconds
            Assert.IsFalse(gitHubHomePageLink.Displayed);

            // Perform flick down touch action to scroll the page up restoring the Homepage link element into the view
            touchScreen.Flick(0, -360);
            System.Threading.Thread.Sleep(3000); // Sleep for 3 seconds
            Assert.IsTrue(gitHubHomePageLink.Displayed);
        }
    }
}

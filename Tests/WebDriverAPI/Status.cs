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

namespace WebDriverAPI
{
    [TestClass]
    public class Status
    {
        [TestMethod]
        public void GetStatus()
        {
            var request = WebRequest.Create(CommonTestSettings.WindowsApplicationDriverUrl + "/status/");
            request.Method = "GET";
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                JObject responseObject = JObject.Parse(responseString);

                JToken buildToken = responseObject["build"];
                Assert.IsNotNull(buildToken);
                Assert.IsNotNull(buildToken["version"]);
                Assert.IsNotNull(buildToken["revision"]);
                Assert.IsNotNull(buildToken["time"]);

                JToken osToken = responseObject["os"];
                Assert.IsNotNull(osToken);
                Assert.IsNotNull(osToken["arch"]);
                Assert.AreEqual(osToken["name"], "windows");
                Assert.IsNotNull(osToken["version"]);
            }
        }
    }
}

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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Remote;

namespace BaseConverterUITest
{
    [TestClass]
    public class UnitTest1
    {
        protected const string AppDriverUrl = "http://127.0.0.1:4723";
        protected static RemoteWebDriver TestAppSession;
        protected static RemoteWebElement TestAppResult;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", "67017b09-d193-4676-a8a5-9d474e2f2c74_1gb0zh1xa1sqe!App");
            TestAppSession = new RemoteWebDriver(new Uri(AppDriverUrl), appCapabilities);

            Assert.IsNotNull(TestAppSession);
        }
        [TestMethod]
        public void ConvertBaseTest()
        {
            var boxes = TestAppSession.FindElementsByClassName("TextBox");
            for (int i = 0; i < boxes.Count; i++)
            {
                var item = boxes[i];
                if (i == 0)
                {
                    item.SendKeys("16");
                }
                else if (i == 1)
                {
                    item.SendKeys("2");
                }
                else
                {
                    //do nothing
                }
            }

            var button = TestAppSession.FindElementByXPath("//Button[@Name=\"Convert!\"]");
            button.Click();

            /*Verify the conversion happened successfully by locating the converted item on the page
            *In this case, converting 16 from base 10 to base 2 should yield 10,000 as the result*/
            TestAppResult = TestAppSession.FindElementByName("10000") as RemoteWebElement;
            Assert.IsNotNull(TestAppResult);

            TestAppSession.Dispose();
        }
    }
}

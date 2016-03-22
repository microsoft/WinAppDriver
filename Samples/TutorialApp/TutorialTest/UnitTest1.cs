using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Remote;

namespace TutorialTest
{
    [TestClass]
    public class UnitTest1
    {
        protected const string AppDriverUrl = "http://127.0.0.1:9999";
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

            var button = TestAppSession.FindElementByClassName("Button");
            button.Click();

            /*Verify the conversion happened successfully by locating the converted item on the page
            *In this case, converting 16 from base 10 to base 2 should yield 10,000 as the result*/
            TestAppResult = TestAppSession.FindElementByName("10000") as RemoteWebElement;
            Assert.IsNotNull(TestAppResult);

            TestAppSession.Dispose();
        }
    }
}

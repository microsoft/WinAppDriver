using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.iOS;

namespace W3CWebDriver
{
    public class TestBase
    {
        protected IOSDriver<IOSElement> session = null;

        [TestCleanup]
        public void TestCleanup()
        {
            if (session != null)
            {
                // There are cases where we throw an exception and end the test
                // after we have quit the session.
                try
                {
                    session.Quit();
                }
                catch { }
                session = null;
            }
        }
    }
}

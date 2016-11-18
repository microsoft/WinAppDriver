using System;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Appium.iOS;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WPFNotepadTest
{
    [TestClass]
    public class Scenario
    {
        protected const string AppDriverUrl = "http://127.0.0.1:4723";
        protected const string WpfNotepadAppId = @"C:\dev\wpfnotepad.exe"; //copy and build binary from source https://codeoverload.wordpress.com/2010/05/01/wpf-notepad/   
        protected const string ExplorerAppId = @"wpfnotepad.exe";
        protected const string TargetSaveLocation = @"%TEMP%\";
        protected static IOSDriver<IOSElement> WpfnotepadSession;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            // Launch Word Classic Windows Application
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", WpfNotepadAppId);
            WpfnotepadSession = new IOSDriver<IOSElement>(new Uri(AppDriverUrl), appCapabilities);
            Assert.IsNotNull(WpfnotepadSession);
        }
        [TestMethod]
        public void BasicScenario()
        {
            WpfnotepadSession.FindElementById("txtDocument").Click();
            WpfnotepadSession.FindElementById("txtDocument").SendKeys("Hello World");
            WpfnotepadSession.Quit();
        }
    }
}

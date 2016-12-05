using System;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Appium.iOS;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;

namespace WordTest
{
    [TestClass]
    public class Scenario
    {
        protected const string AppDriverUrl = "http://127.0.0.1:4723";
        protected const string TextValue = "This is an automated test on Microsoft Office 2016 Word!";
        protected const string WordAppId = @"C:\Program Files (x86)\Microsoft Office\root\Office16\WINWORD.EXE";
        //protected const string WordAppId = @"C:\Program Files(x86)\admindirectory\wpfnotepad.exe";        
        protected const string ExplorerAppId = @"C:\Windows\System32\explorer.exe";
        protected const string TestFileName = "NotepadTestOutputFile.txt";
        protected const string TargetSaveLocation = @"%TEMP%\";
        protected static IOSDriver<IOSElement> WordSession;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            // Launch Word Classic Windows Application
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", WordAppId);
            WordSession = new IOSDriver<IOSElement>(new Uri(AppDriverUrl), appCapabilities);
            Assert.IsNotNull(WordSession);
            Thread.Sleep(30000);
        }
        [TestMethod]
        public void BasicScenario()
        {
            WordSession.FindElementByName("Blank document").Click();
        }
    }
}

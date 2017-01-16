using System;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Appium.Windows;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WordTest
{
    [TestClass]
    public class Scenario
    {
        protected const string AppDriverUrl = "http://127.0.0.1:4723";
        protected const string TextValue = "This is an automated test on Classic Windows Application!";
        protected const string WordAppId = @"C:\Program Files (x86)\Microsoft Office\root\Office16\WINWORD.EXE";
        protected const string ExplorerAppId = @"C:\Windows\System32\explorer.exe";
        protected const string TestFileName = "NotepadTestOutputFile.txt";
        protected const string TargetSaveLocation = @"%TEMP%\";
        protected static WindowsDriver<WindowsElement> WordSession;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            // Launch Word Classic Windows Application
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", WordAppId);
            WordSession = new WindowsDriver<WindowsElement>(new Uri(AppDriverUrl), appCapabilities);
            Assert.IsNotNull(WordSession);
        }
        [TestMethod]
        public void BasicScenario()
        {
            WordSession.FindElementByName("Blank document").Click();
        }
    }
}

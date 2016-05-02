using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium;
using System.IO;

namespace CalculatorTest
{
    [TestClass]
    public class PortedOpenCalculatorTest
    {
        // Calculator Apps activation name.
        private static string CalculatorActivationName = "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App";
        private static string DesktopCalculatorLocalStateDirectory = Environment.GetEnvironmentVariable("USERPROFILE") + @"\AppData\Local\Packages\Microsoft.WindowsCalculator_8wekyb3d8bbwe\LocalState";
        private static string TestDataPath = Environment.GetEnvironmentVariable("SYSTEMDRIVE") + @"\data\test\bin";
        private static string SRPFileName = "SRPData.xml";

        private static string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        private static List<RemoteWebDriver> CalculatorSession;

        [AssemblyInitialize]
        [TestProperty("CoreClrProfile", "TestNetv2.0")]
        public static void AssemblySetup(TestContext tc)
        {
            // Disable the Store Rating Promotion Dialog from appearing
            DisableSRPDialog();
        }

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            CalculatorSession = new List<RemoteWebDriver>();
        }

        [ClassCleanup]
        public static void TearDown()
        {
            while (CalculatorSession.Count > 0)
            {
                CalculatorSession.Last().Dispose();
                CalculatorSession.RemoveAt(CalculatorSession.Count - 1);
            }
        }

        [TestInitialize]
        public void LaunchCalculatorApp()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CalculatorActivationName);

            CalculatorSession.Add(new RemoteWebDriver(new Uri(WindowsApplicationDriverUrl), appCapabilities));
            Assert.IsNotNull(CalculatorSession.Last());
            CalculatorSession.Last().Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));
        }

        [TestMethod]
        [TestProperty("RunAs", "User")]
        public void LaunchTest()
        {
            LaunchCalculatorApp();
            LaunchCalculatorApp();
            CloseCalculatorApp();
            CloseCalculatorApp();

            for (var i = 0; i < 2; i++)
            {
                LaunchCalculatorApp();
                CloseCalculatorApp();
            }
            // leave app launched, it is cleaned up in TestCleanup - other tests assume that it's running
            LaunchCalculatorApp();
        }

        [TestMethod]
        [TestProperty("RunAs", "User")]
        public void DoMathOperation()
        {
            RemoteWebDriver session = CalculatorSession.Last();

            IWebElement num5ButtonUiObject = session.FindElementByName("Five");
            if (num5ButtonUiObject == null)
            {
                throw new InvalidOperationException("Cannot find num 5 button");
            }

            IWebElement multiplyButtonUiObject = session.FindElementByName("Multiply by");
            if (multiplyButtonUiObject == null)
            {
                throw new InvalidOperationException("Cannot find multiply button");
            }

            IWebElement equalButtonUiObject = session.FindElementByName("Equals");
            if (equalButtonUiObject == null)
            {
                throw new InvalidOperationException("Cannot find equal button");
            }

            // Clear the results in case we open an exisiting instance of calculator.
            IWebElement clearButtonUiObject = session.FindElementByName("Clear entry");
            if (clearButtonUiObject == null)
            {
                throw new InvalidOperationException("Cannot find clear button");
            }

            clearButtonUiObject.Click();

            // Do 55 * 5
// XXXYD: slowing it down so we can watch the events
            num5ButtonUiObject.Click();
            System.Threading.Thread.Sleep(1000);

            num5ButtonUiObject.Click();
            System.Threading.Thread.Sleep(1000);

            multiplyButtonUiObject.Click();
            System.Threading.Thread.Sleep(1000);

            num5ButtonUiObject.Click();
            System.Threading.Thread.Sleep(1000);

            equalButtonUiObject.Click();
            System.Threading.Thread.Sleep(5000);


            /* XXXYD - when we use the Appium .net package we'll be able to query by Automation ID, then 
                        we can find elements properly.  For now using display name, which for the results box changes

                                    IWebElement calculatorResult = CalculatorSession.Last().FindElementByName("CalculatorResults");
                                    if (calculatorResult == null)
                                    {
                                        throw new InvalidOperationException("Cannot find calculator result text block");
                                    }
            */

            // The automation name of the calculator text is in the followin string format.
            // Please note the spaces between text.
            // This logic will break languages other than english.
            var expectedResult = String.Format(" {0} ", 55 * 5);

            // We don't have access to the resources and the calculator puts the result in a UIA block that
            // says "Display is <space>number<space>". We just want to make sure the number is correct so that
            // this test runs on multiple languages. Normally you would have access to the resources if needed or just
            // present the proper data in the UIA tree

//XXXYD: Not sure what the equivalent of Verify is as I don't know exactly what Verify is doing :)
//            Verify.IsTrue(calculatorResult.Name.Contains(expectedResult), "Validate calculator did math right!");
        }

        [TestCleanup]
        public void CloseCalculatorApp()
        {
            CalculatorSession.Last().Dispose();
            CalculatorSession.RemoveAt(CalculatorSession.Count - 1);

//XXXYD: TODO, Support looking for Calculator window after we close the session
/*
                        if (this.calculatorWindow != null)
                        {
                            UAPApp.CloseWindow(this.calculatorWindow);
                            this.calculatorWindow = null;
                        }

                        UIObject element;
                        UIObject.Root.Descendants.TryFind(this.calculatorUiCondition, out element);
                        Verify.IsNull(element, "Verify app is no longer visible");
*/
        }

        private static void DisableSRPDialog()
        {
            string calcSRPDataFilePath;
            string sourceSRPDataFilePath = Path.Combine(TestDataPath, SRPFileName);

            calcSRPDataFilePath = Path.Combine(DesktopCalculatorLocalStateDirectory, SRPFileName);

/* XXXYD Removing mobile for now

            if (!Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily.Equals("Windows.Mobile", StringComparison.OrdinalIgnoreCase))
            {
                calcSRPDataFilePath = Path.Combine(DesktopCalculatorLocalStateDirectory, SRPFileName);
            }
            else
            {
                calcSRPDataFilePath = Path.Combine(MobileCalculatorLocalStateDirectory, SRPFileName);
            }
*/

            if (File.Exists(calcSRPDataFilePath))
            {
                File.Delete(calcSRPDataFilePath);
            }

            File.Copy(sourceSRPDataFilePath, calcSRPDataFilePath);
        }
    }
}

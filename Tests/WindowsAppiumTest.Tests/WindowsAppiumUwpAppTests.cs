using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Threading;
using System.Xml.Linq;
using System.Xml.XPath;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Appium.MultiTouch;
using OpenQA.Selenium.Remote;

namespace WindowsAppiumTest.Tests
{
    [TestFixture]
    public class WindowsAppiumUwpAppTests
    {
        protected const string AppiumUrl = "http://127.0.0.1:4723";

        protected static WindowsDriver<WindowsElement> _driver;
        protected static string AppUIDName = "c24c8163-548e-4b84-a466-530178fc0580_scyf5npe3hv32!App";
        protected const string AppxPath = "//*[@Name='WindowsAppiumTest.UwpApp'][@ClassName='ApplicationFrameWindow']";

        protected static readonly TimeSpan WaitTimeout = TimeSpan.FromSeconds(5);

        protected virtual string DeviceArchitecture => "x64";
            
        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            var capabilities = new DesiredCapabilities();

            // TODO: Deploy the app via Appium.  
            capabilities.SetCapability("app", AppUIDName);

            _driver = new WindowsDriver<WindowsElement>(new Uri(AppiumUrl), capabilities);

            bool bIsInstalled = _driver.IsAppInstalled("WindowsAppiumTest.UwpApp");
            if (!bIsInstalled)
            {
                // TODO: install the app here, or throw an error
                //_driver.InstallApp(AppxPath);
            }

            _driver.LaunchApp();
        }

        [SetUp]
        public void SetUp()
        {
            _driver.Manage().Timeouts().ImplicitlyWait(WaitTimeout);

            // TODO: Hide keybaord if needed
            //_driver.HideKeyboard();

            GoHomeIfNeeded();
        }

        #region JSWP Tests

        /// <summary>
        /// Tests '/status'
        /// </summary>
        [Test]
        [Explicit] // Status is not yet provided in correct format (and/or this test needs to be changed)
        public void StatusTest()
        {
            var request = WebRequest.Create(AppiumUrl + "/status");
            using (var response = request.GetResponse() as HttpWebResponse)
            {
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.IsTrue(response.ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase));
                dynamic result = JObject.Parse(new StreamReader(response.GetResponseStream()).ReadToEnd());
                Assert.IsFalse(string.IsNullOrWhiteSpace((string)result.sessionId));
                Assert.AreEqual(0, (int)result.status);
                Assert.AreEqual("0.1.0.0", (string)result.value.build.version); // TODO: update version number
                Assert.AreEqual(DeviceArchitecture, (string)result.value.os.arch);
                Assert.AreEqual("windows", (string)result.value.os.name);
                Assert.AreEqual("Microsoft Windows NT 6.2.9200.0", (string)result.value.os.version);
            }
        }

        /// <summary>
        /// Tests '/sessions' and '/session/:sessionId'
        /// </summary>
        [Test]
        [Explicit] // Get Sessions not yet supported
        public void SessionsTest()
        {
            const string platformVersion = "10";
            const bool webStorageEnabled = false; // TODO: should this be true for web sessions? If so, add test
            const bool javascriptEnabled = false; // TODO: should this be true for web sessions? If so, add test
            const bool locationContextEnabled = false; // TODO: should this be true for web sessions? If so, add test
            const string deviceName = ""; // TODO: set the device name
            const string appUnderTest = ""; // TODO: set the name of the app under test

            string sessionId;

            using (var response = WebRequest.Create(AppiumUrl + "/sessions").GetResponse() as HttpWebResponse)
            {
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.IsTrue(response.ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase));

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                // responseString should be something like this...
                // { "status":0,"value":[{"id":"d5ee7b01-296d-428f-bd1c-2a706a7fe6b1","capabilities":{"platform":"WINDOWS","browserName":"","platformVersion":"10","webStorageEnabled":false,"takesScreenshot":true,"javascriptEnabled":false,"databaseEnabled":false,"networkConnectionEnabled":true,"locationContextEnabled":false,"warnings":{},"desired":{"browserName":"","appium-version":"1.0","platformVersion":"10","deviceName":"donatello","app":"http://appurl","platformName":"Windows"},"appium-version":"1.0","deviceName":"","app":"http://appurl","platformName":"Windows"}}],"sessionId":"d5ee7b01-296d-428f-bd1c-2a706a7fe6b1"}

                dynamic result = JObject.Parse(responseString);
                Assert.IsFalse(string.IsNullOrWhiteSpace((string)result.sessionId));
                sessionId = result.sessionId;
                Assert.AreEqual(0, (int)result.status);
                Assert.AreEqual(1, result.value.Count);
                var sessionDetails = result.value.First;
                Assert.AreEqual((string)result.sessionId, (string)sessionDetails.id);
                var capabilities = sessionDetails.capabilities;
                Assert.AreEqual("WINDOWS", (string)capabilities.platform);
                Assert.AreEqual("", (string)capabilities.browserName);
                Assert.AreEqual(platformVersion, (string)capabilities.platformVersion);
                Assert.AreEqual(webStorageEnabled, (bool)capabilities.webStorageEnabled);
                Assert.AreEqual(true, (bool)capabilities.takesScreenshot);
                Assert.AreEqual(javascriptEnabled, (bool)capabilities.javascriptEnabled);
                Assert.AreEqual(false, (bool)capabilities.databaseEnabled); 
                Assert.AreEqual(true, (bool)capabilities.networkConnectionEnabled); 
                Assert.AreEqual(locationContextEnabled, (bool)capabilities.locationContextEnabled);
                Assert.AreEqual("1.0", (string)capabilities.GetValue("appium-version")); 
                Assert.AreEqual(deviceName, (string)capabilities.deviceName); 
                Assert.AreEqual(appUnderTest, (string)capabilities.app);
                Assert.AreEqual("Windows", (string)capabilities.platformName); 
            }

            using (var response = WebRequest.Create(AppiumUrl + "/session/" + sessionId).GetResponse() as HttpWebResponse)
            {
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.IsTrue(response.ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase));

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                // responseString should be something like this...
                // { "status":0,"value":{"id":"d5ee7b01-296d-428f-bd1c-2a706a7fe6b1","capabilities":{"platform":"WINDOWS","browserName":"","platformVersion":"10","webStorageEnabled":false,"takesScreenshot":true,"javascriptEnabled":false,"databaseEnabled":false,"networkConnectionEnabled":true,"locationContextEnabled":false,"warnings":{},"desired":{"browserName":"","appium-version":"1.0","platformVersion":"10","deviceName":"donatello","app":"http://appurl","platformName":"Windows"},"appium-version":"1.0","deviceName":"","app":"http://appurl","platformName":"Windows"}},"sessionId":"d5ee7b01-296d-428f-bd1c-2a706a7fe6b1"}

                dynamic result = JObject.Parse(responseString);
                Assert.IsFalse(string.IsNullOrWhiteSpace((string)result.sessionId));
                Assert.AreEqual(0, (int)result.status);
                var capabilities = result.value.capabilities;
                Assert.AreEqual("WINDOWS", (string)capabilities.platform);
                Assert.AreEqual("", (string)capabilities.browserName);
                Assert.AreEqual(platformVersion, (string)capabilities.platformVersion);
                Assert.AreEqual(webStorageEnabled, (bool)capabilities.webStorageEnabled);
                Assert.AreEqual(true, (bool)capabilities.takesScreenshot);
                Assert.AreEqual(javascriptEnabled, (bool)capabilities.javascriptEnabled);
                Assert.AreEqual(false, (bool)capabilities.databaseEnabled);
                Assert.AreEqual(true, (bool)capabilities.networkConnectionEnabled);
                Assert.AreEqual(locationContextEnabled, (bool)capabilities.locationContextEnabled);
                Assert.AreEqual("1.0", (string)capabilities.GetValue("appium-version"));
                Assert.AreEqual(deviceName, (string)capabilities.deviceName); 
                Assert.AreEqual(appUnderTest, (string)capabilities.app);
            }

            _driver.Dispose();

            using (var response = WebRequest.Create(AppiumUrl + "/sessions").GetResponse() as HttpWebResponse)
            {
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                Assert.IsTrue(response.ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase));

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                // responseString should be something like this...
                // { "status":0,"value":[],"sessionId":"d5ee7b01-296d-428f-bd1c-2a706a7fe6b1"}

                dynamic result = JObject.Parse(responseString);
                Assert.IsFalse(string.IsNullOrWhiteSpace((string)result.sessionId));
                Assert.AreEqual(0, (int)result.status);
                Assert.AreEqual(0, result.value.Count);
            }
        }

        // TODO: Test '/session/:sessionId/timeouts' if applicable (possibly when we support WebViews) 
        // TODO: '/session/:sessionId/async_script' if applicable (possibly when we support WebViews)

        /// <summary>
        /// Tests '/session/:sessionId/timeouts/implicit_wait'
        /// </summary>
        [Test]
        [Explicit] // Wait timeout is not currently being used in element queries
        public void ImplicitWaitTimeoutTest()
        {
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(1));

            var started = DateTime.UtcNow;

            try
            {
                FindElementById("no-such-automation-id");
            }
            catch (NoSuchElementException)
            {
            }

            Assert.IsTrue(DateTime.UtcNow > started + TimeSpan.FromSeconds(1));
            Assert.IsTrue(DateTime.UtcNow < started + TimeSpan.FromSeconds(2));

            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(2));

            started = DateTime.UtcNow;

            try
            {
                FindElementById("no-such-automation-id");
            }
            catch (NoSuchElementException)
            {
            }

            Assert.IsTrue(DateTime.UtcNow > started + TimeSpan.FromSeconds(2));
            Assert.IsTrue(DateTime.UtcNow < started + TimeSpan.FromSeconds(3));

            _driver.Manage().Timeouts().ImplicitlyWait(WaitTimeout);
        }

        // TODO: '/session/:sessionId/window_handle'
        // TODO: '/session/:sessionId/window_handles'

        /// <summary>
        /// Tests '/session/:sessionId/url'
        /// </summary>
        [Test]
        [Explicit] // Url is not currently supported
        public void SessionUrlTest()
        {
            Assert.AreEqual("TODO", _driver.Url); // TODO: native url

            // TODO: switch to WebView and Assert _driver.Url value
            // TODO: test updating the value of _driver.Url if appropriate
        }

        // TODO: '/session/:sessionId/forward' (probably only when we have WebView support)
        // TODO: '/session/:sessionId/back' (probably only when we have WebView support)
        // TODO: '/session/:sessionId/refresh' (probably only when we have WebView support)
        // TODO: '/session/:sessionId/execute' (probably only when we have WebView support)
        // TODO: '/session/:sessionId/execute_async' (probably only when we have WebView support)

        /// <summary>
        /// Tests '/session/:sessionId/screenshot'
        /// </summary>
        [Test]
        public void ScreenshotTest()
        {
            var screenshotName = "screenshot.png";

            var screenshotFile = new FileInfo(screenshotName);

            if (screenshotFile.Exists)
                screenshotFile.Delete();

            screenshotFile = new FileInfo(screenshotName);

            Assert.IsFalse(screenshotFile.Exists);

            var screenshot = _driver.GetScreenshot();
            screenshot.SaveAsFile(screenshotFile.FullName, ImageFormat.Png);

            screenshotFile = new FileInfo(screenshotName);

            Assert.IsTrue(screenshotFile.Exists);

            var screenshotImage = Image.FromFile(screenshotFile.FullName);

            Assert.IsTrue(screenshotImage.Width > 0);
        }

        /// <summary>
        /// Tests:
        ///   '/session/:sessionid/ime/available_engines'
        ///   '/session/:sessionid/ime/activated'
        ///   '/session/:sessionid/ime/active_engine'
        ///   '/session/:sessionid/ime/deactivate'
        ///   '/session/:sessionid/ime/activate'
        /// </summary>
        [Test]
        [Explicit] // IME is not currently supported
        public void ImeTest()
        {
            Assert.IsNotEmpty(_driver.GetIMEAvailableEngines());

            Assert.IsFalse(_driver.IsIMEActive());

            TapElementById("TextEntryButton");

            TapElementById("TextBox1");

            Assert.IsTrue(_driver.IsIMEActive());

            var activeIme = _driver.GetIMEActiveEngine();

            _driver.DeactiveIMEEngine();

            Assert.IsFalse(_driver.IsIMEActive());

            _driver.ActivateIMEEngine(activeIme);

            Assert.IsTrue(_driver.IsIMEActive());
        }

        // TODO: '/session/:sessionid/frame' (probably only when we have WebView support)
        // TODO: '/session/:sessionid/frame/parent' (probably only when we have WebView support)
        // TODO: '/session/:sessionid/window'

        /// <summary>
        /// Tests '/session/:sessionId/window/:windowHandle/size'
        /// </summary>
        [Test]
        [Explicit] // Windows Size is not currently supported
        public void WindowSizeTest()
        {
            var size = _driver.Manage().Window.Size;
            Assert.IsTrue(size.Height > 0);
            Assert.IsTrue(size.Width > 0);
            Assert.IsTrue(size.Height < 256000);
            Assert.IsTrue(size.Width > 256000);

            // TODO: test setting _driver.Manage().Window.Size - it should probably throw an exception
        }

        /// <summary>
        /// Tests '/session/:sessionId/window/:windowHandle/position'
        /// </summary>
        [Test]
        [Explicit] // Windows Position is not currently supported
        public void WindowPositionTest()
        {
            var position = _driver.Manage().Window.Position;
            Assert.IsTrue(position.X >= 0);
            Assert.IsTrue(position.Y >= 0);
            Assert.IsTrue(position.X < 256000);
            Assert.IsTrue(position.Y > 256000);

            // TODO: test setting _driver.Manage().Window.Position 
        }

        // TODO: '/session/:sessionId/window/:windowHandle/maximise'
        // TODO: '/session/:sessionid/cookie' (probably only when we have WebView support)
        // TODO: '/session/:sessionid/cookie/:name' (probably only when we have WebView support)

        /// <summary>
        /// Tests '/session/:sessionId/source'
        /// </summary>
        [Test]
        [Explicit] // Source is not currently supported
        public void SourceTest()
        {
            var source = _driver.PageSource;

            var xDoc = XDocument.Parse(source);

            Assert.IsNotNull(xDoc.XPathEvaluate("//*[@Name='WindowsAppiumTest.UwpApp']"));

            // TODO: check various elements and attributes
        }

        // TODO: '/session/:sessionId/title' (probably only when we have WebView support)

        /// <summary>
        /// Tests '/session/:sessionId/element' with 'class name' strategy
        /// </summary>
        [Test]
        public void FindElementByClassNameTest()
        {
            Assert.IsNotNull(_driver.FindElementByClassName("TextBlock"));
        }

        /// <summary>
        /// Tests '/session/:sessionId/element' with 'class name' strategy
        /// </summary>
        [Test]
        [ExpectedException(typeof(NoSuchElementException))]
        public void FindNonExistantElementByClassNameTest()
        {
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.Zero);

            _driver.FindElementByClassName("Not-a-valid-class-name");
        }

        /// <summary>
        /// Tests '/session/:sessionId/element' with 'css selector' strategy
        /// </summary>
        [Explicit] // Currently CSS selector is not supported
        [Test]
        public void FindElementByCssSelectorTest()
        {
            // TODO: Consider what should be implemented here - I think Appium only supports CSS selectors with WebViews.
            Assert.IsNotNull(_driver.FindElementByCssSelector("TODO"));
        }

        /// <summary>
        /// Tests '/session/:sessionId/element' with 'css selector' strategy
        /// </summary>
        [Explicit] // Currently CSS selector is not supported
        [Test]
        [ExpectedException(typeof(NoSuchElementException))]
        public void FindNonExistantElementByCssSelectorTest()
        {
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.Zero);

            // TODO: Consider what should be implemented here - I think Appium only supports CSS selectors with WebViews.
            _driver.FindElementByCssSelector("Not-a-valid-css-selecto");
        }

        /// <summary>
        /// Tests '/session/:sessionId/element' with 'id' strategy
        /// </summary>
        [Test]
        public void FindElementByIdTest()
        {
            Assert.IsNotNull(FindElementById("PageTitle"));
        }

        /// <summary>
        /// Tests '/session/:sessionId/element' with 'id' strategy
        /// </summary>
        [Test]
        [ExpectedException(typeof(NoSuchElementException))]
        public void FindNonExistantElementByIdTest()
        {
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.Zero);

            FindElementById("Not-a-valid-id");
        }

        /// <summary>
        /// Tests '/session/:sessionId/element' with 'name' strategy
        /// </summary>
        [Test]
        public void FindElementByNameTest()
        {
            Assert.IsNotNull(_driver.FindElementByName("WindowsAppiumTest.UwpApp"));
        }

        /// <summary>
        /// Tests '/session/:sessionId/element' with 'name' strategy
        /// </summary>
        [Test]
        [ExpectedException(typeof(NoSuchElementException))]
        public void FindNonExistantElementByNameTest()
        {
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.Zero);

            _driver.FindElementByName("Not-a-valid-name");
        }

        /// <summary>
        /// Tests '/session/:sessionId/element' with 'tag name' strategy
        /// </summary>
        [Explicit] // Currently TagName is not supported
        [Test]
        public void FindElementByTagName()
        {
            Assert.IsNotNull(_driver.FindElementByTagName("ControlType.Text")); 
        }

        /// <summary>
        /// Tests '/session/:sessionId/element' with 'tag name' strategy
        /// </summary>
        [Explicit] // Currently TagName is not supported
        [Test]
        [ExpectedException(typeof(NoSuchElementException))]
        public void FindNonExistantElementByTagName()
        {
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.Zero);

            _driver.FindElementByTagName("Not-a-valid-tag-name"); 
        }

        /// <summary>
        /// Tests '/session/:sessionId/element' with 'xpath' strategy
        /// </summary>
        [Explicit] // Currently Xpath is not supported
        [Test]
        public void FindElementByXpath()
        {
            Assert.IsNotNull(_driver.FindElementByXPath($"//*[@AutomationId='{QualifiedAutomationId("PageTitle")}']"));
        }

        /// <summary>
        /// Tests '/session/:sessionId/element' with 'xpath' strategy
        /// </summary>
        [Explicit] // Currently Xpath is not supported
        [Test]
        [ExpectedException(typeof(NoSuchElementException))]
        public void FindNonExistantElementByXpath()
        {
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.Zero);

            _driver.FindElementByXPath("//*[@AutomationId='Not-a-valid-id']");
        }

        /// <summary>
        /// Tests '/session/:sessionId/element' with 'xpath' strategy
        /// </summary>
        [Explicit] // Currently Xpath is not supported
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void FindElementByInvalidXpath()
        {
            _driver.FindElementByXPath("Not-a-valid-xpath-string[");
        }

        /// <summary>
        /// Tests '/session/:sessionId/elements' with 'class name' strategy
        /// </summary>
        [Test]
        public void FindElementsByClassNameTest()
        {
            Assert.IsNotEmpty(_driver.FindElementsByClassName("TextBlock"));
        }

        /// <summary>
        /// Tests '/session/:sessionId/elements' with 'css selector' strategy
        /// </summary>
        [Explicit] // Currently CSS is not supported
        [Test]
        public void FindElementsByCssSelectorTest()
        {
            // TODO: Consider what should be implemented here - I think Appium only supports CSS selectors with WebViews.
        }

        /// <summary>
        /// Tests '/session/:sessionId/elements' with 'id' strategy
        /// </summary>
        [Test]
        public void FindElementsByIdTest()
        {
            Assert.IsNotEmpty(_driver.FindElementsByAccessibilityId(QualifiedAutomationId("PageTitle")));
        }

        /// <summary>
        /// Tests '/session/:sessionId/elements' with 'name' strategy
        /// </summary>
        [Test]
        public void FindElementsByNameTest()
        {
            Assert.IsNotEmpty(_driver.FindElementsByName("WindowsAppiumTest.UwpApp"));
        }

        /// <summary>
        /// Tests '/session/:sessionId/elements' with 'tag name' strategy
        /// </summary>
        [Explicit] // Currently TagName is not supported
        [Test]
        public void FindElementsByTagName()
        {
            Assert.IsNotEmpty(_driver.FindElementsByTagName("ControlType.Text"));
        }

        /// <summary>
        /// Tests '/session/:sessionId/elements' with 'xpath' strategy
        /// </summary>
        [Explicit] // Currently Xpath is not supported
        [Test]
        public void FindElementsByXpath()
        {
            Assert.IsNotEmpty(
                _driver.FindElementsByXPath($"//*[@AutomationId='{QualifiedAutomationId("PageTitle")}']"));
        }

        /// <summary>
        /// Tests '/session/:sessionId/elements' with 'xpath' strategy
        /// </summary>
        [Explicit] // Currently Xpath is not supported
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void FindElementsByInvalidXpath()
        {
            _driver.FindElementsByXPath("Not-a-valid-xpath-string[");
        }

        /// <summary>
        /// Tests '/session/:sessionId/element/active'
        /// </summary>
        [Test]
        public void ActiveElementTest()
        {
            TapElementById("TextEntryButton");

            var textBox = FindElementById("TextBox1");
            textBox.Tap(1, 100);

            const string activeElementText = "this is the active element";

            textBox.SendKeys(activeElementText);

            Assert.AreEqual(activeElementText, _driver.SwitchTo().ActiveElement().Text);
        }

        /// <summary>
        /// Tests '/session/:sessionId/element/:id/element' with 'class name' strategy
        /// </summary>
        [Test]
        public void FindChildElementByClassNameTest()
        {
            Assert.IsNotNull(AppElement().FindElementByClassName("TextBlock"));
        }

        /// <summary>
        /// Tests '/session/:sessionId/element/:id/element/:id/element' with 'class name' strategy
        /// </summary>
        [Test]
        [ExpectedException(typeof(NoSuchElementException))]
        public void FindNonExistantChildElementByClassNameTest()
        {
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.Zero);

            AppElement().FindElementByClassName("Not-a-valid-class-name");
        }

        /// <summary>
        /// Tests '/session/:sessionId/element/:id/element' with 'css selector' strategy
        /// </summary>
        [Explicit] // Currently CSS selector is not supported
        // we can do this before having to solve xpath query
        [Test]
        public void FindChildElementByCssSelectorTest()
        {
            // TODO: Consider what should be implemented here - I think Appium only supports CSS selectors with WebViews.
            Assert.IsNotNull(AppElement().FindElementByCssSelector("TODO"));
        }

        /// <summary>
        /// Tests '/session/:sessionId/element/:id/element' with 'css selector' strategy
        /// </summary>
        [Explicit] // Currently CSS selector is not supported
        [Test]
        [ExpectedException(typeof(NoSuchElementException))]
        public void FindNonExistantChildElementByCssSelectorTest()
        {
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.Zero);

            // TODO: Consider what should be implemented here - I think Appium only supports CSS selectors with WebViews.
            AppElement().FindElementByCssSelector("Not-a-valid-css-selecto");
        }

        /// <summary>
        /// Tests '/session/:sessionId/element/:id/element' with 'id' strategy
        /// </summary>
        [Test]
        public void FindChildElementByIdTest()
        {
            Assert.IsNotNull(AppElement().FindElementByAccessibilityId(QualifiedAutomationId("PageTitle")));
        }

        /// <summary>
        /// Tests '/session/:sessionId/element/:id/element' with 'id' strategy
        /// </summary>
        [Test]
        [ExpectedException(typeof(NoSuchElementException))]
        public void FindNonExistantChildElementByIdTest()
        {
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.Zero);

            AppElement().FindElementByAccessibilityId("Not-a-valid-id");
        }

        /// <summary>
        /// Tests '/session/:sessionId/element/:id/element' with 'name' strategy
        /// </summary>
        [Test]
        public void FindChildElementByNameTest()
        {
            Assert.IsNotNull(AppElement().FindElementByName("home"));
        }

        /// <summary>
        /// Tests '/session/:sessionId/element/:id/element' with 'name' strategy
        /// </summary>
        [Test]
        [ExpectedException(typeof(NoSuchElementException))]
        public void FindNonExistantChileElementByNameTest()
        {
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.Zero);

            AppElement().FindElementByName("Not-a-valid-name");
        }

        /// <summary>
        /// Tests '/session/:sessionId/element/:id/element' with 'tag name' strategy
        /// </summary>
        [Explicit] // Currently TagName is not supported
        [Test]
        public void FindChildElementByTagName()
        {
            Assert.IsNotNull(AppElement().FindElementByTagName("ControlType.Text"));
        }

        /// <summary>
        /// Tests '/session/:sessionId/element/:id/element' with 'tag name' strategy
        /// </summary>
        [Explicit] // Currently TagName is not supported
        [Test]
        [ExpectedException(typeof(NoSuchElementException))]
        public void FindNonExistantChildElementByTagName()
        {
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.Zero);

            AppElement().FindElementByTagName("Not-a-valid-tag-name");
        }

        /// <summary>
        /// Tests '/session/:sessionId/element/:id/element' with 'xpath' strategy
        /// </summary>
        [Explicit] // Currently Xpath is not supported
        [Test]
        public void FindChildElementByXpath()
        {
            Assert.IsNotNull(AppElement().FindElementByXPath($"//*[@AutomationId='{QualifiedAutomationId("PageTitle")}']"));
        }

        /// <summary>
        /// Tests '/session/:sessionId/element/:id/element' with 'xpath' strategy
        /// </summary>
        [Explicit] // Currently Xpath is not supported
        [Test]
        [ExpectedException(typeof(NoSuchElementException))]
        public void FindNonExistantChildElementByXpath()
        {
            _driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.Zero);

            AppElement().FindElementByXPath("//*[@AutomationId='Not-a-valid-id']");
        }

        /// <summary>
        /// Tests '/session/:sessionId/element/:id/element' with 'xpath' strategy
        /// </summary>
        [Explicit] // Currently xpath is not supported
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void FindChildElementByInvalidXpath()
        {
            AppElement().FindElementByXPath("Not-a-valid-xpath-string[");
        }

        /// Tests '/session/:sessionId/element/:id/elements' with 'class name' strategy
        /// </summary>
        [Test]
        public void FindChildElementsByClassNameTest()
        {
            Assert.IsNotEmpty(AppElement().FindElementsByClassName("TextBlock"));
        }

        /// <summary>
        /// Tests '/session/:sessionId/element/:id/elements' with 'css selector' strategy
        /// </summary>
        [Explicit] // Currently CSS selector is not supported
        [Test]
        public void FindChildElementsByCssSelectorTest()
        {
            // TODO: Consider what should be implemented here - I think Appium only supports CSS selectors with WebViews.
        }

        /// <summary>
        /// Tests '/session/:sessionId/element/:id/elements' with 'id' strategy
        /// </summary>
        [Test]
        public void FindChildElementsByIdTest()
        {
            Assert.IsNotEmpty(AppElement().FindElementsByAccessibilityId(QualifiedAutomationId("PageTitle")));
        }

        /// <summary>
        /// Tests '/session/:sessionId/element/:id/elements' with 'name' strategy
        /// </summary>
        [Test]
        public void FindChildElementsByNameTest()
        {
            Assert.IsNotEmpty(AppElement().FindElementsByName("home"));
        }

        /// <summary>
        /// Tests '/session/:sessionId/element/:id/elements' with 'tag name' strategy
        /// </summary>
        [Explicit] // Currently TagName is not supported
        [Test]
        public void FindChildElementsByTagName()
        {
            Assert.IsNotEmpty(AppElement().FindElementsByTagName("ControlType.Text"));
        }

        /// <summary>
        /// Tests '/session/:sessionId/element/:id/elements' with 'xpath' strategy
        /// </summary>
        [Explicit] // Currently xpath is not supported
        [Test]
        public void FindChildElementsByXpath()
        {
            Assert.IsNotEmpty(
                AppElement().FindElementsByXPath($"//*[@AutomationId='{QualifiedAutomationId("PageTitle")}']"));
        }

        /// <summary>
        /// Tests '/session/:sessionId/element/:id/elements' with 'xpath' strategy
        /// </summary>
        [Explicit] // Currently xpath is not supported
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void FindChildElementsByInvalidXpath()
        {
            AppElement().FindElementsByXPath("Not-a-valid-xpath-string[");
        }

        // TODO: '/session/:sessionId/element/:id'

        /// <summary>
        /// Tests '/session/:sessionId/element/:id/click'
        /// </summary>
        [Test]
        public void ClickElementTest()
        {
            FindElementById("GesturesButton").Click();

            Assert.AreEqual("gestures", FindElementById("PageTitle").Text);
        }

        /// <summary>
        /// Tests '/session/:sessionId/element/:id/click'
        /// </summary>
        [Test]
        [Explicit] // ElementNotVisibleException is not currently thrown
        [ExpectedException(typeof(ElementNotVisibleException))]
        public void ClickOffScreenElementTest()
        {
            FindElementById("OffScreenButton").Click();
        }

        // TODO: '/session/:sessionId/element/:id/submit' (probably only when we have WebView support)

        /// <summary>
        /// Tests '/session/:sessionId/element/:id/text'
        /// </summary>
        [Test]
        public void ElementTextTest()
        {
            // TODO: Should buttons have text values?  If not, modify this test to check a TextBox
            Assert.AreEqual("Gestures", FindElementById("GesturesButton").Text);
        }

        /// <summary>
        /// Tests '/session/:sessionId/element/:id/value'
        /// </summary>
        [Test]
        public void SendKeysToElementTest()
        {
            TapElementById("TextEntryButton");

            FindElementById("TextBox3").SendKeys("hello");

            Assert.AreEqual("hello", FindElementById("TheOutput").Text);

            // TODO: Test every key and applicable non-keys: https://code.google.com/p/selenium/wiki/JsonWireProtocol#/session/:sessionId/element/:id/value
        }



        /// <summary>
        /// Tests '/session/:sessionId/keys'
        /// </summary>
        [Test]
        [Explicit] // Send Keys to active element is not currently supported
        public void SendKeysToActiveElementTest()
        {
            TapElementById("TextEntryButton");

            TapElementById("TextBox3");
                
            _driver.Keyboard.SendKeys("hello");

            Assert.AreEqual("hello", FindElementById("TheOutput").Text);

            // TODO: Test every key and applicable non-keys: https://code.google.com/p/selenium/wiki/JsonWireProtocol#/session/:sessionId/element/:id/value
        }

        /// <summary>
        /// Tests '/session/:sessionId/element/:id/name'
        /// </summary>
        [Test]
        public void ElementNameTest()
        {
            Assert.AreEqual("ControlType.Button", FindElementById("GesturesButton").TagName);
        }

        /// <summary>
        /// Tests '/session/:sessionId/element/:id/clear'
        /// </summary>
        [Test]
        public void ElementClearTextTest()
        {
            TapElementById("TextEntryButton");

            var textBox1 = FindElementById("TextBox1");
            textBox1.SendKeys("hello\n");

            // ask for element again to trigger getting the new text
            Assert.AreEqual("hello", textBox1.Text);

            textBox1.Clear();

            Assert.AreEqual("", textBox1.Text);
        }

        /// <summary>
        /// Tests '/session/:sessionId/element/:id/selected'
        /// </summary>
        [Explicit] // Toggle button state is not currently supported
        [Test]
        public void ElementSelectedTest()
        {
            TapElementById("ControlsButton");

            Assert.AreEqual(false, FindElementById("OffToggleButton").Selected);
            Assert.AreEqual(true, FindElementById("OnToggleButton").Selected);
        }

        /// <summary>
        /// Tests '/session/:sessionId/element/:id/enabled'
        /// </summary>
        [Test]
        public void ElementEnabledTest()
        {
            TapElementById("ControlsButton");

            Assert.AreEqual(false, FindElementById("DisabledButton").Enabled);
            Assert.AreEqual(true, FindElementById("OnToggleButton").Enabled);
        }

        /// <summary>
        /// Tests '/session/:sessionId/element/:id/attribute/:name'
        /// </summary>
        [Test]
        public void ElementAttributeTest()
        {
            Assert.AreEqual(
                "Controls Button Help Text",
                FindElementById("ControlsButton").GetAttribute("HelpText"));
        }

        /// <summary>
        /// Tests '/session/:sessionId/element/:id/attribute/:name'
        /// </summary>
        [Test]
        [Explicit] // Currently returns string.Empty instead of null
        public void ElementNonExistantAttributeTest()
        {
            Assert.AreEqual(
                null,
                FindElementById("ControlsButton").GetAttribute("No-such-attribute-name"));
        }

        /// <summary>
        /// Tests '/session/:sessionId/element/:id/equals/:other'
        /// </summary>
        [Test]
        public void ElementEqualsTest()
        {
            var firstButton = _driver.FindElementByClassName("Button");

             var idButton = FindElementById("ControlsButton");

             Assert.IsTrue(idButton.Equals(firstButton));
        }

        /// <summary>
        /// Tests '/session/:sessionId/element/:id/equals/:other'
        /// </summary>
        [Test]
        [Explicit] // Equals is not currently supported
        public void ElementNotEqualsTest()
        {
            var firstButton = _driver.FindElementByClassName("Button");

             var idButton = FindElementById("GesturesButton");

             Assert.IsFalse(idButton.Equals(firstButton));
        }

        /// <summary>
        /// Tests '/session/:sessionId/element/:id/displayed'
        /// </summary>
        [Test]
        public void ElementDisplayedTest()
        {
            Assert.IsTrue(FindElementById("GesturesButton").Displayed);
            Assert.IsFalse(FindElementById("OffScreenButton").Displayed);
        }

        /// <summary>
        /// Tests '/session/:sessionId/element/:id/location'
        /// </summary>
        [Test]
        public void ElementLocationTest()
        {
            var controlsButtonLocation = FindElementById("ControlsButton").Location;
            var gesturesButtonLocation = FindElementById("GesturesButton").Location;

            Assert.AreEqual(controlsButtonLocation.X, gesturesButtonLocation.X);
            Assert.Less(controlsButtonLocation.Y, gesturesButtonLocation.Y);
            Assert.IsTrue(controlsButtonLocation.Y < gesturesButtonLocation.Y + 60);
        }

        /// <summary>
        /// Tests '/session/:sessionId/element/:id/location_in_view'
        /// </summary>
        [Test]
        public void ElementLocationInViewTest()
        {
            var controlsButtonLocation = FindElementById("ControlsButton").LocationOnScreenOnceScrolledIntoView;
            var gesturesButtonLocation = FindElementById("GesturesButton").LocationOnScreenOnceScrolledIntoView;

            Assert.AreEqual(controlsButtonLocation.X, gesturesButtonLocation.X);
            Assert.Less(controlsButtonLocation.Y, gesturesButtonLocation.Y);
            Assert.IsTrue(controlsButtonLocation.Y < gesturesButtonLocation.Y + 60);
        }   

        /// <summary>
        /// Tests '/session/:sessionId/element/:id/size'
        /// </summary>
        [Test]
        public void ElementSizeTest()
        {
            var controlsButtonSize = FindElementById("ControlsButton").Size;

            Assert.Greater(controlsButtonSize.Width, 0);
            Assert.Greater(controlsButtonSize.Height, 0);
            Assert.Less(controlsButtonSize.Width, 256000);
            Assert.Less(controlsButtonSize.Height, 100);
        }

        // TODO: '/session/:sessionId/element/:id/css/:propertyName' (probably only when we have WebView support)

        /// <summary>
        /// Tests '/session/:sessionId/orientation'
        /// </summary>
        [Test]
        [Explicit] // Orientation is not currently supported
        public void OrientationTest()
        {
            if (_driver.Orientation == ScreenOrientation.Landscape)
            { 
                _driver.Orientation = ScreenOrientation.Portrait;
                Assert.AreEqual(ScreenOrientation.Portrait, _driver.Orientation);
                _driver.Orientation = ScreenOrientation.Landscape;
                Assert.AreEqual(ScreenOrientation.Landscape, _driver.Orientation);
            }
            else
            {
                _driver.Orientation = ScreenOrientation.Landscape;
                Assert.AreEqual(ScreenOrientation.Landscape, _driver.Orientation);
                _driver.Orientation = ScreenOrientation.Portrait;
                Assert.AreEqual(ScreenOrientation.Portrait, _driver.Orientation);
            }
        }

        // TODO: '/session/:sessionId/alert_text' (probably only when we have WebView support)
        // TODO: '/session/:sessionId/accept_alert' (probably only when we have WebView support)
        // TODO: '/session/:sessionId/dismiss_alert' (probably only when we have WebView support)

        /// <summary>
        /// Tests '/session/:sessionId/moveto', '/session/:sessionId/click' and '/session/:sessionId/doubleclick'
        /// </summary>
        [Test]
        [Explicit] // Clicks not currently positioned as expected
        public void MouseClickTest()
        {
            const int xChrome = 4;
            const int yChrome = 38;

            TapElementById("GesturesButton");

            _driver.Mouse.Click(AppElement().Coordinates);

            var gestureCount = FindElementById("GestureCount");
            Assert.AreEqual("1", gestureCount.Text);

            var output = FindElementById("Output");

            Assert.IsTrue(output.Text.StartsWith("Click:", StringComparison.Ordinal));

            var position = output.Text.Substring(6).Split(',');

            var appSize = AppElement().Size;

            var appCenterX = Math.Round(appSize.Width/2.0) - xChrome;
            var appCenterY = Math.Round(appSize.Height/2.0) - yChrome;

            var xClick = appCenterX.ToString();
            var yClick = appCenterY.ToString();

            Assert.AreEqual(xClick, position[0]);
            Assert.AreEqual(yClick, position[1]);

            _driver.Mouse.MouseMove(null, 10, 10);
            _driver.Mouse.Click(null);

            Assert.AreEqual("2", gestureCount.Text);

            Assert.IsTrue(output.Text.StartsWith("Click:", StringComparison.Ordinal));

            position = output.Text.Substring(6).Split(',');

            xClick = (appCenterX + 10).ToString();
            yClick = (appCenterY + 10).ToString();

            Assert.AreEqual(xClick, position[0]);
            Assert.AreEqual(yClick, position[1]);

            _driver.Mouse.MouseMove(AppElement().Coordinates, 20, 20);
            _driver.Mouse.Click(null);

            Assert.AreEqual("3", gestureCount.Text);

            Assert.IsTrue(output.Text.StartsWith("Click:", StringComparison.Ordinal));

            position = output.Text.Substring(6).Split(',');

            xClick = (appCenterX + 20).ToString();
            yClick = (appCenterY + 20).ToString();

            Assert.AreEqual(xClick, position[0]);
            Assert.AreEqual(yClick, position[1]);

            _driver.Mouse.ContextClick(AppElement().Coordinates);

            Assert.AreEqual("4", gestureCount.Text);

            Assert.IsTrue(output.Text.StartsWith("RightClick:", StringComparison.Ordinal));

            position = output.Text.Substring(11).Split(',');

            xClick = appCenterX.ToString();
            yClick = appCenterY.ToString();

            Assert.AreEqual(xClick, position[0]);
            Assert.AreEqual(yClick, position[1]);

            _driver.Mouse.DoubleClick(AppElement().Coordinates);

            Assert.AreEqual("5", gestureCount.Text);

            Assert.IsTrue(output.Text.StartsWith("DoubleClick:", StringComparison.Ordinal));

            position = output.Text.Substring(12).Split(',');

            Assert.AreEqual(xClick, position[0]);
            Assert.AreEqual(yClick, position[1]);

            // TODO: Test middle button click.  It seems the dotnet Appium client doesn't support this.
        }

        /// <summary>
        /// Tests '/session/:sessionId/buttondown' and '/session/:sessionId/buttonup'
        /// </summary>
        [Test]
        [Explicit] // Mouse down/move/up not currently supported
        public void MouseDragTest()
        {
            TapElementById("GesturesButton");

            _driver.Mouse.MouseMove(AppElement().Coordinates, 10, 10);
            _driver.Mouse.MouseDown(null);
            _driver.Mouse.MouseMove(AppElement().Coordinates, 100, 100);
            _driver.Mouse.MouseUp(null);

            var gestureCount = FindElementById("GestureCount");
            Assert.AreEqual("1", gestureCount.Text);

            var output = FindElementById("Output");

            Assert.IsTrue(output.Text.StartsWith("Flick", StringComparison.Ordinal));

            // TODO: Validate gesture

            _driver.Mouse.MouseDown(FindElementById("PageTitle").Coordinates);
            _driver.Mouse.MouseUp(AppElement().Coordinates);

            Assert.AreEqual("2", gestureCount.Text);

            Assert.IsTrue(output.Text.StartsWith("Flick", StringComparison.Ordinal));

            // TODO: Validate gesture

            // TODO: Test middle and right button drags.  It seems the dotnet Appium client doesn't support this.
        }

        /// <summary>
        /// Tests '/session/:sessionId/touch/click' and '/session/:sessionId/touch/doubleclick'
        /// </summary>
        [Test]
        [Explicit] // Touch not currently supported
        public void TouchClickTest()
        {
            TapElementById("GesturesButton");

            var touchScreen = new RemoteTouchScreen(_driver);
            
            touchScreen.SingleTap(AppElement().Coordinates);

            var gestureCount = FindElementById("GestureCount");

            Assert.AreEqual("1", gestureCount.Text);

            var output = FindElementById("Output").Text;

            Assert.IsTrue(output.StartsWith("Tap:"));

            var position = output.Substring(4).Split(',');

            var appSize = AppElement().Size;

            var appCenterX = Math.Round(appSize.Width/2.0);
            var appCenterY = Math.Round(appSize.Height/2.0);

            Assert.AreEqual(appCenterX.ToString(), position[0]);
            Assert.AreEqual(appCenterY.ToString(), position[1]);

            touchScreen.DoubleTap(AppElement().Coordinates);

            Assert.AreEqual("2", gestureCount.Text);

            output = FindElementById("Output").Text;

            Assert.IsTrue(output.StartsWith("DoubleTap:"));

            position = output.Substring(10).Split(',');

            Assert.AreEqual(appCenterX.ToString(), position[0]);
            Assert.AreEqual(appCenterY.ToString(), position[1]);

            touchScreen.LongPress(AppElement().Coordinates);

            Assert.AreEqual("3", gestureCount.Text);
            
            output = FindElementById("Output").Text;

            Assert.IsTrue(output.StartsWith("Hold:"));

            position = output.Substring(5).Split(',');

            Assert.AreEqual(appCenterX.ToString(), position[0]);
            Assert.AreEqual(appCenterY.ToString(), position[1]);
        }

        /// <summary>
        /// Tests '/session/:sessionId/touch/longclick'
        /// </summary>
        [Test]
        [Explicit] // Long Click not currently supported
        public void LongClickTest()
        {
            TapElementById("GesturesButton");

            var touchScreen = new RemoteTouchScreen(_driver);
            touchScreen.LongPress(AppElement().Coordinates);

            Thread.Sleep(1000); // Give the gesture time to complete

            var gestureCount = FindElementById("GestureCount");

            Assert.AreEqual("1", gestureCount.Text);

            var output = FindElementById("Output").Text;

            Assert.IsTrue(output.StartsWith("Hold:"));
        }

        /// <summary>
        /// Tests '/session/:sessionId/touch/down', '/session/:sessionId/touch/move' and '/session/:sessionId/touch/up'
        /// </summary>
        [Test]
        [Explicit] // Touch not currently supported
        public void TouchDragTest()
        {
            TapElementById("GesturesButton");

            var touchScreen = new RemoteTouchScreen(_driver);

            var topLeft = AppElement().Location;
             
            touchScreen.Down(topLeft.X + 100, topLeft.Y + 100);
            touchScreen.Move(topLeft.X + 110, topLeft.Y + 110);
            touchScreen.Move(topLeft.X + 120, topLeft.Y + 120);
            touchScreen.Move(topLeft.X + 130, topLeft.Y + 130);
            touchScreen.Move(topLeft.X + 140, topLeft.Y + 140);
            touchScreen.Move(topLeft.X + 150, topLeft.Y + 150);
            touchScreen.Move(topLeft.X + 160, topLeft.Y + 160);
            touchScreen.Up(topLeft.X + 170, topLeft.Y + 170);

            Thread.Sleep(1000); // Give the gesture time to complete

            var gestureCount = FindElementById("GestureCount");

            Assert.AreEqual("1", gestureCount.Text);

            var output = FindElementById("Output").Text;

            Assert.IsTrue(output.StartsWith("Flick "));

            Assert.IsTrue(output.Contains("Angle:45 "));

            Assert.IsTrue(output.Contains("Inertial:True "));
        }

        /// <summary>
        /// Tests '/session/:sessionId/touch/scroll'
        /// </summary>
        [Test]
        [Explicit] // Touch not currently supported
        public void TouchScrollTest()
        {
            TapElementById("GesturesButton");

            var touchScreen = new RemoteTouchScreen(_driver);
            
            touchScreen.Scroll(AppElement().Coordinates, 100, 100);

            Thread.Sleep(1000); // Give the gesture time to complete

            var gestureCount = FindElementById("GestureCount");

            Assert.AreEqual("1", gestureCount.Text);

            var output = FindElementById("Output").Text;

            Assert.IsTrue(output.StartsWith("Flick "));

            Assert.IsTrue(output.Contains("Angle:45 "));

            Assert.IsTrue(output.Contains("Inertial:False "));

            touchScreen.Scroll(-100, -100);

            Thread.Sleep(1000); // Give the gesture time to complete

            Assert.AreEqual("2", gestureCount.Text);

            output = FindElementById("Output").Text;

            Assert.IsTrue(output.StartsWith("Flick "));

            Assert.IsTrue(output.Contains("Angle:225 "));

            Assert.IsTrue(output.Contains("Inertial:False "));
        }

        /// <summary>
        /// Tests '/session/:sessionId/touch/flick' with 'element' parameter
        /// </summary>
        [Test]
        [Explicit] // Touch not currently supported
        public void TouchFlickWithElementTest()
        {
            TapElementById("GesturesButton");

            var touchScreen = new RemoteTouchScreen(_driver);

            touchScreen.Flick(AppElement().Coordinates, 200, 200, 1000);

            Thread.Sleep(1000); // Give the gesture time to complete

            var gestureCount = FindElementById("GestureCount");

            Assert.AreEqual("1", gestureCount.Text);

            var output = FindElementById("Output").Text;

            Assert.IsTrue(output.StartsWith("Flick "));
            Assert.IsTrue(output.Contains("Angle:45 "));
            Assert.IsTrue(output.Contains("Inertial:True "));

            var indexOfAmount = output.IndexOf("Amount:") + 7;
            var amount1 = output.Substring(indexOfAmount, output.IndexOf(" Velocity:") - indexOfAmount).Split(',');
            var amount1X = Convert.ToDouble(amount1[0]);
            var amount1Y = Convert.ToDouble(amount1[1]);

            var velocity1 = output.Substring(output.IndexOf("Velocity:") + 9).Split(',');
            var velocity1X = Convert.ToDouble(velocity1[0]);
            var velocity1Y = Convert.ToDouble(velocity1[1]);

            touchScreen.Flick(AppElement().Coordinates, 100, 100, 1000);

            Thread.Sleep(1000); // Give the gesture time to complete

            Assert.AreEqual("2", gestureCount.Text);

            output = FindElementById("Output").Text;

            Assert.IsTrue(output.StartsWith("Flick "));
            Assert.IsTrue(output.Contains("Angle:45 "));
            Assert.IsTrue(output.Contains("Inertial:True "));

            indexOfAmount = output.IndexOf("Amount:") + 7;
            var amount2 = output.Substring(indexOfAmount, output.IndexOf(" Velocity:") - indexOfAmount).Split(',');
            var amount2X = Convert.ToDouble(amount2[0]);
            var amount2Y = Convert.ToDouble(amount2[1]);

            var velocity2 = output.Substring(output.IndexOf("Velocity:") + 9).Split(',');
            var velocity2X = Convert.ToDouble(velocity2[0]);
            var velocity2Y = Convert.ToDouble(velocity2[1]);

            Assert.Greater(amount1X, amount2X * 1.8);
            Assert.Greater(amount1Y, amount2Y * 1.8);
            Assert.Less(amount1X, amount2X * 2.2);
            Assert.Less(amount1Y, amount2Y * 2.2);

            Assert.Greater(velocity1X * 1.2, velocity2X);
            Assert.Greater(velocity1Y * 1.2, velocity2Y);
            Assert.Less(velocity1X * 0.8, velocity2X);
            Assert.Less(velocity1Y * 0.8, velocity2Y);

            touchScreen.Flick(AppElement().Coordinates, 100, 100, 2000);

            Thread.Sleep(1000); // Give the gesture time to complete

            Assert.AreEqual("3", gestureCount.Text);

            output = FindElementById("Output").Text;

            Assert.IsTrue(output.StartsWith("Flick "));
            Assert.IsTrue(output.Contains("Angle:45 "));
            Assert.IsTrue(output.Contains("Inertial:True "));

            indexOfAmount = output.IndexOf("Amount:") + 7;
            var amount3 = output.Substring(indexOfAmount, output.IndexOf(" Velocity:") - indexOfAmount).Split(',');
            var amount3X = Convert.ToDouble(amount3[0]);
            var amount3Y = Convert.ToDouble(amount3[1]);
        
            var velocity3 = output.Substring(output.IndexOf("Velocity:") + 9).Split(',');
            var velocity3X = Convert.ToDouble(velocity3[0]);
            var velocity3Y = Convert.ToDouble(velocity3[1]);

            Assert.Greater(amount2X * 1.2, amount3X);
            Assert.Greater(amount2Y * 1.2, amount3Y);
            Assert.Less(amount2X * 0.8, amount3X);
            Assert.Less(amount2Y * 0.8, amount3Y);

            Assert.Greater(velocity3X, velocity2X * 1.8);
            Assert.Greater(velocity3Y, velocity2Y * 1.8);
            Assert.Less(velocity3X, velocity2X * 2.2);
            Assert.Less(velocity3Y, velocity2Y * 2.2);
        }

        /// <summary>
        /// Tests 'session/:sessionId/touch/flick' without 'element' parameter
        /// </summary>
        [Test]
        [Explicit] // Touch not currently supported
        public void TouchFlickWithoutElementTest()
        {
            TapElementById("GesturesButton");

            var touchScreen = new RemoteTouchScreen(_driver);

            touchScreen.Flick(200, 200);

            Thread.Sleep(1000); // Give the gesture time to complete

            var gestureCount = FindElementById("GestureCount");

            Assert.AreEqual("1", gestureCount.Text);

            var output = FindElementById("Output").Text;

            Assert.IsTrue(output.StartsWith("Flick "));
            Assert.IsTrue(output.Contains("Angle:45 "));
            Assert.IsTrue(output.Contains("Inertial:True "));

            var indexOfAmount = output.IndexOf("Amount:") + 7;
            var amount1 = output.Substring(indexOfAmount, output.IndexOf(" Velocity:") - indexOfAmount).Split(',');
            var amount1X = Convert.ToDouble(amount1[0]);
            var amount1Y = Convert.ToDouble(amount1[1]);

            var velocity1 = output.Substring(output.IndexOf("Velocity:") + 9).Split(',');
            var velocity1X = Convert.ToDouble(velocity1[0]);
            var velocity1Y = Convert.ToDouble(velocity1[1]);

            touchScreen.Flick(100, 100);

            Thread.Sleep(1000); // Give the gesture time to complete

            Assert.AreEqual("2", gestureCount.Text);

            output = FindElementById("Output").Text;

            Assert.IsTrue(output.StartsWith("Flick "));
            Assert.IsTrue(output.Contains("Angle:45 "));
            Assert.IsTrue(output.Contains("Inertial:True "));

            indexOfAmount = output.IndexOf("Amount:") + 7;
            var amount2 = output.Substring(indexOfAmount, output.IndexOf(" Velocity:") - indexOfAmount).Split(',');
            var amount2X = Convert.ToDouble(amount2[0]);
            var amount2Y = Convert.ToDouble(amount2[1]);

            var velocity2 = output.Substring(output.IndexOf("Velocity:") + 9).Split(',');
            var velocity2X = Convert.ToDouble(velocity2[0]);
            var velocity2Y = Convert.ToDouble(velocity2[1]);

            Assert.Greater(amount1X / 2, amount2X * 0.8);
            Assert.Greater(amount1Y / 2, amount2Y * 0.8);
            Assert.Less(amount1X / 2, amount2X * 1.2);
            Assert.Less(amount1Y / 2, amount2Y * 1.2);

            Assert.Greater(velocity1X / 2, velocity2X * 0.8);
            Assert.Greater(velocity1Y / 2, velocity2Y * 0.8);
            Assert.Less(velocity1X / 2, velocity2X * 1.2);
            Assert.Less(velocity1Y / 2, velocity2Y * 1.2);
        }

        /// <summary>
        /// Tests '/session/:sessionId/location'
        /// </summary>
        [Test]
        [Explicit] // Location not currently supported
        public void LocationTest()
        {
            var originalLocation = _driver.Location;

            var mountEverest = new Location
            {
                Altitude = 8495,
                Latitude = 27.986065,
                Longitude = 86.922623
            };

            _driver.Location = mountEverest;

            var newLocation = _driver.Location;

            Assert.AreEqual(mountEverest.Altitude, newLocation.Altitude);
            Assert.AreEqual(mountEverest.Latitude, newLocation.Latitude);
            Assert.AreEqual(mountEverest.Longitude, newLocation.Longitude);

            _driver.Location = originalLocation;
        }

        // TODO: '/session/:sessionId/local_storage' (not implemented by appium)
        // TODO: '/session/:sessionId/local_storage/key/:key' (not implemented by appium)
        // TODO: '/session/:sessionId/local_storage/size' (not implemented by appium)

        // TODO: '/session/:sessionId/session_storage' (not implemented by appium)
        // TODO: '/session/:sessionId/session_storage/key/:key' (not implemented by appium)
        // TODO: '/session/:sessionId/session_storage/size' (not implemented by appium)

        // TODO: '/session/:sessionId/log' (need to add support to dotnet appium client)
        // TODO: '/session/:sessionId/log/types' (need to add support to dotnet appium client)

        // TODO '/session/:sessionId/application_cache/status' (not implemented by appium)

        #endregion JSWP Tests

        #region Appium Extensions Tests

        // TODO: '/session/:sessionId/context'
        // TODO: '/session/:sessionId/contexts'
        // TODO: '/session/:sessionId/element/:elementId/pageIndex' (need to add support to dotnet appium client)
        // TODO: '/session/:sessionId/network_connection' (need to add support to the dotnet appium client in WindowsDriver, similar to AndroidDriver)
        // TODO: '/welcome'
        // TODO: '/produce_error'
        // TODO: '/crash'
        // TODO: '/test/guinea-pig'
        // TODO: '/session/:sessionId/appium/device/shake' (need to add support to the dotnet appium client in WindowsDriver, similar to IOSDriver)
        // TODO: '/session/:sessionId/appium/device/lock'
        // TODO: '/session/:sessionId/appium/device/unlock' (need to add support to the dotnet appium clinet)
        // TODO: '/session/:sessionId/appium/device/is_locked' (need to add support to the dotnet appium clinet)

        /// <summary>
        /// Test '/session/:sessionId/appium/device/press_keycode'
        /// </summary>
        [Test]
        [Explicit] // Key Codes not currently supported
        public void PressKeyCodeEvent()
        {
            // TODO: use a valid keycode and check that it was handled correctly
            _driver.PressKeyCode(5);    
        }

        /// <summary>
        /// Test '/session/:sessionId/appium/device/long_press_keycode'
        /// </summary>
        [Test]
        [Explicit] // Key Codes not currently supported
        public void LongPressKeyCodeEvent()
        {
            // TODO: use a valid keycode and check that it was handled correctly
            _driver.LongPressKeyCode(5);
        }

        /// <summary>
        /// Tests '/session/:sessionId/appium/device/rotate'
        /// </summary>
        [Test]
        [Explicit] // Rotate not currently supported
        public void RotateTest()
        {
            TapElementById("GesturesButton");

            _driver.Rotate(new Dictionary<string,int>()
            {
                { "x",  100 },
                { "y",  100 },
                { "radius", 50 },
                { "rotation", 30 },
                { "touchCount", 2 },
                { "duration", 2 },
            });

            var gestureCount = FindElementById("GestureCount");

            Assert.AreEqual("1", gestureCount.Text);

            var output = FindElementById("Output").Text;

            Assert.IsTrue(output.StartsWith("{'Rotation':"));

            // TODO: further checks to confirm the rotation was performed correctly

            AppElement().Rotate(new Dictionary<string,int>()
            {
                { "x",  100 },
                { "y",  100 },
                { "radius", 50 },
                { "rotation", 30 },
                { "touchCount", 2 },
                { "duration", 2 },
            });

            Assert.AreEqual("2", gestureCount.Text);

            output = FindElementById("Output").Text;

            Assert.IsTrue(output.StartsWith("{'Rotation':"));

            // TODO: further checks to confirm the rotation was performed correctly

            // TODO: consider performing rotations with different parameters
        }

        // TODO: '/session/:sessionId/appium/device/current_activity'

        /// <summary>
        /// Tests:
        ///   '/session/:sessionId/appium/device/install_app'
        ///   '/session/:sessionId/appium/device/remove_app'
        ///   '/session/:sessionId/appium/device/app_installed'
        /// </summary>
        [Test]
        [Explicit] // Install not currently supported
        public void InstallAppTests()
        {
            const string appPath = ""; // TODO: set app path
            const string appId = ""; // TODO: set app id

            Assert.IsFalse(_driver.IsAppInstalled(appId));
            _driver.InstallApp(appPath);
            Assert.IsTrue(_driver.IsAppInstalled(appId));
            _driver.RemoveApp(appId);
            Assert.IsFalse(_driver.IsAppInstalled(appId));
        }

        // TODO: '/session/:sessionId/element/:id/replace_value' (needs support adding to dotnet appium client)

        /// <summary>
        /// Tests '/session/:sessionId/appium/device/hide_keyboard'
        /// </summary>
        [Explicit]
        [Test]
        public void DismissKeyboardTest()
        {
            TapElementById("TextEntryButton");

            Assert.IsTrue(IsKeyboardVisible());

            _driver.HideKeyboard();

            Assert.IsFalse(IsKeyboardVisible());
        }

        /// <summary>
        /// Tests '/session/:sessionId/appium/device/hide_keyboard'
        /// </summary>
        [Explicit]
        [Test]
        [ExpectedException(
            typeof(WebDriverException), // TODO: It may be a sub-class of WebDriverException which should be thrown
            ExpectedMessage="Soft keyboard not present, cannot hide keyboard"
            )]
        public void DismissNonExistantKeyboardTest()
        {
            _driver.HideKeyboard();
        }

        /// <summary>
        /// Tests '/session/:sessionId/appium/device/push_file' and '/session/:sessionId/appium/device/pull_file'
        /// </summary>
        [Test]
        [Explicit] // File transfer not currently supported
        public void PushPullFileTest()
        {
            const string textFileContents = "Hi\r\nI'm a text file\r\n\r\n";
            const string textFilePath = "my_text_file.txt";

            var base64TextFile = Convert.ToBase64String(Encoding.UTF8.GetBytes(textFileContents));

            //_driver.PushFile(textFileName, base64TextFile); // TODO: add support to dotnet appium client, it probably needs iOS support adding as appium supports pushfile for simulators

            Assert.AreEqual(textFileContents, Encoding.UTF8.GetString(_driver.PullFile(textFilePath)));

            // TODO: additional checks with different file paths
        }

        /// <summary>
        /// Test '/session/:sessionId/appium/device/pull_folder'
        /// </summary>
        [Test]
        [Explicit] // File transfer not currently supported
        public void PullFolder()
        {
            const string folderPath = ""; // TODO: set folder path

            var zip = new ZipArchive(new MemoryStream(_driver.PullFolder(folderPath)));

            Assert.IsNotEmpty(zip.Entries);
        }

        // TODO: '/session/:sessionId/appium/device/toggle_airplane_mode' (needs to be added to dotnet appium client)
        // TODO: '/session/:sessionId/appium/device/toggle_data' (needs to be added to dotnet appium client)
        // TODO: '/session/:sessionId/appium/device/toggle_wifi' (needs to be added to dotnet appium client)
        // TODO: '/session/:sessionId/appium/device/toggle_location_services' (needs to be added to dotnet appium client)
        // TODO: '/session/:sessionId/appium/device/open_notifications' (needs to be added to dotnet appium client)
        // TODO: '/session/:sessionId/appium/device/start_activity' (needs to be added to dotnet appium client)

        /// <summary>
        /// Tests '/session/:sessionId/appium/app/launch' and '/session/:sessionId/appium/app/close'
        /// </summary>
        [Test]
        [Explicit] // CloseApp not currently supported
        public void AppLaunchTest()
        {
            TapElementById("TextEntryButton");
            FindElementById("TextBox1");

            _driver.CloseApp();

            var buttonVisible = true;
            var textBoxVisible = true;

            try
            {
                FindElementById("TextEntryButton");
            }
            catch (NoSuchElementException) // TODO: specific exception
            {
                // TODO: check exception message
                buttonVisible = false;
            }

            try
            {
                FindElementById("TextBox1");
            }
            catch (NoSuchElementException) // TODO: specific exception
            {
                // TODO: check exception message
                textBoxVisible = false;
            }

            Assert.IsFalse(buttonVisible);
            Assert.IsFalse(textBoxVisible);

            _driver.LaunchApp();

            FindElementById("TextEntryButton");
        }

        /// <summary>
        /// Tests '/session/:sessionId/appium/app/reset'
        /// </summary>
        [Test]
        [Explicit] // ResetApp currently not supported
        public void AppResetTest()
        {
            TapElementById("TextEntryButton");
            FindElementById("TextBox1");

            _driver.ResetApp();

            FindElementById("TextEntryButton");
        }

        /// <summary>
        /// Test '/session/:sessionId/appium/app/background'
        /// </summary>
        [Test]
        [Explicit] // BackgroundApp currently not supported
        public void AppBackgroundTest()
        {
            TapElementById("TextEntryButton");
            FindElementById("TextBox1");

            _driver.BackgroundApp(3);

            var buttonVisible = true;
            var textBoxVisible = true;

            try
            {
                FindElementById("TextEntryButton");
            }
            catch (NoSuchElementException) // TODO: specific exception
            {
                // TODO: check exception message
                buttonVisible = false;
            }

            try
            {
                FindElementById("TextBox1");
            }
            catch (NoSuchElementException) // TODO: specific exception
            {
                // TODO: check exception message
                textBoxVisible = false;
            }

            Assert.IsFalse(buttonVisible);
            Assert.IsFalse(textBoxVisible);

            Thread.Sleep(3500);

            FindElementById("TextBox1");
        }

        // TODO: '/session/:sessionId/appium/app/end_test_coverage' (pretty sure this is only relevant for Android)

        /// <summary>
        /// Tests '/session/:sessionId/appium/app/strings' 
        /// </summary>
        [Test]
        [Explicit] // Test incomplete
        public void StringsTest()
        {
            // TODO: need to fix GetAppStrings in appium dotnet client
            var defaultStrings = _driver.GetAppStrings(); // TODO: need to add strings to test app
            var germanStrings = _driver.GetAppStrings("de"); // TODO: need to add german strings to test app

            // TODO: tests to validate default and german strings
        }

        // TODO: '/session/:sessionId/appium/settings' (don't think this is relevant to WinAppDriver)

        /// <summary>
        /// Tests '/session/:sessionId/touch/perform' with a Tap TouchAction
        /// </summary>
        [Test]
        [Explicit] // Tap element not currently supported
        public void TapElementTest()
        {
            TapElementById("GesturesButton");

            Assert.AreEqual("gestures", FindElementById("PageTitle").Text);

            TapElement(AppElement());

            Assert.AreEqual("Tap", FindElementById("Output").Text);
        }

        // TODO: '/session/:sessionId/touch/multi/perform'

        #endregion Appium Extensions Tests

        #region Misc tests

        [Test]
        public void BackButtonTest()
        {
            TapElementById("GesturesButton");

            Assert.AreEqual("gestures", FindElementById("PageTitle").Text);

            Back();

            Assert.AreEqual("home", FindElementById("PageTitle").Text);
        }

        #endregion Misc tests

        #region private methods

        void Back()
        {
            // TODO: need a way of locating the back button for:
            //    desktop modern apps with a back button in the title bar
            //    desktop modern apps in tablet mode
            //    phone devices

            var backButton = FindElementById("BackButton"); // This is a temporary button within the app.
            // _driver.FindElementByXPath("/*[@ClassName='Shell_TrayWnd']/*[@Name='Back']");

            if (backButton != null)
            {
                new TouchAction(_driver).Tap(backButton).Perform();
            }

            // TODO: Replace the code above with this if possible
            //_driver.KeyEvent(WindowsKeyCode.Back);
        }

        string QualifiedAutomationId(string automationId)
        {
            return $"WindowsAppiumTest.UwpApp.{automationId}";
        }

        WindowsElement FindElementById(string automationId)
        {
            return _driver.FindElementByAccessibilityId(QualifiedAutomationId(automationId));
        }

        void TapElement(WindowsElement element)
        {
            new TouchAction(_driver).Tap(element).Perform();
        }

        void TapElementById(string automationId)
        {
            TapElement(FindElementById(automationId));
        }

        void GoHomeIfNeeded()
        {
            for (var i = 0; i < 5; i++)
            {
                if (FindElementById("PageTitle").Text == "home")
                {
                    break;
                }

                Back();
                Thread.Sleep(1000);
            }
        }

        bool IsKeyboardVisible()
        {
            // TODO: fix this!
            return false;

            // TODO: What is the best way of determining keyboard visibility?  Can we (ab)use /session/session:Id/ime/activated?
            // return _driver.FindElementsByName("Ctrl").Any();
        }

        WindowsElement AppElement()
        {
            return _driver.FindElementByName("WindowsAppiumTest.UwpApp");
        }

        #endregion private methods
    }
}

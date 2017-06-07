//******************************************************************************
//
// Copyright (c) 2017 Microsoft Corporation. All rights reserved.
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
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;

namespace W3CWebDriver
{
    public class CalculatorBase
    {
        protected static WindowsDriver<WindowsElement> session;

        public static void Setup(TestContext context)
        {
            // Cleanup leftover objects from previous test if exists
            TearDown();

            // Launch Calculator
            session = CreateNewCalculatorSession();
            Assert.IsNotNull(session);
            Assert.IsNotNull(session.SessionId);

            // Ensure that calculator is in standard mode
            session.FindElementByAccessibilityId("NavButton").Click();
            System.Threading.Thread.Sleep(1000);
            var splitViewPane = session.FindElementByClassName("SplitViewPane");
            splitViewPane.FindElementByName("Standard Calculator").Click();
            System.Threading.Thread.Sleep(1000);
            var header = session.FindElementByAccessibilityId("Header");
            Assert.AreEqual("Standard", header.Text);
        }

        public static void TearDown()
        {
            // Close the application and delete the session
            if (session != null)
            {
                session.Quit();
                session = null;
            }
        }

        protected static WindowsDriver<WindowsElement> CreateNewCalculatorSession()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", CommonTestSettings.CalculatorAppId);
            return new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
        }

        protected static WindowsElement GetForeignElement()
        {
            // Launch a new calculator instance, locate an element, and close the session to get an element that is not valid in another session
            WindowsDriver<WindowsElement> newSession = CreateNewCalculatorSession();
            WindowsElement foreignElement = newSession.FindElementByAccessibilityId("Header");
            newSession.Quit();
            return foreignElement;
        }

        protected static WindowsElement GetOrphanedElement(WindowsDriver<WindowsElement> remoteSession)
        {
            // Use existing calculator session and close the window to get an orphaned element
            WindowsElement orphanedElement = remoteSession.FindElementByAccessibilityId("Header");
            remoteSession.Close();
            return orphanedElement;
        }

        protected static WindowsElement GetStaleElement()
        {
            session.FindElementByAccessibilityId("ClearMemoryButton").Click();
            session.FindElementByAccessibilityId("clearButton").Click();
            session.FindElementByAccessibilityId("memButton").Click();

            try
            {
                // Locate the Memory pivot item tab that is displayed in expanded mode
                session.FindElementByAccessibilityId("MemoryLabel").Click();
            }
            catch
            {
                // Open the memory flyout when the calculator is in compact mode
                session.FindElementByAccessibilityId("MemoryButton").Click();
            }

            System.Threading.Thread.Sleep(1000);
            WindowsElement staleElement = session.FindElementByAccessibilityId("MemoryListView").FindElementByName("0") as WindowsElement;
            session.FindElementByAccessibilityId("ClearMemory").Click();
            System.Threading.Thread.Sleep(1000);
            return staleElement;
        }
    }
}
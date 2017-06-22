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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;

namespace W3CWebDriver
{
    public class CalculatorBase
    {
        protected static WindowsDriver<WindowsElement> session;
        private static WindowsElement header;

        public static void Setup(TestContext context)
        {
            // Launch Calculator if it is not yet launched
            if (session == null)
            {
                session = Utility.CreateNewSession(CommonTestSettings.CalculatorAppId);
                Assert.IsNotNull(session);
                Assert.IsNotNull(session.SessionId);
                header = session.FindElementByAccessibilityId("Header");
                Assert.IsNotNull(header);
            }

            // Set focus on the calculator window
            header.Click();

            // Ensure that calculator is in standard mode
            if (!header.Text.Equals("Standard"))
            {
                session.FindElementByAccessibilityId("NavButton").Click();
                System.Threading.Thread.Sleep(1000);
                var splitViewPane = session.FindElementByClassName("SplitViewPane");
                splitViewPane.FindElementByName("Standard Calculator").Click();
                System.Threading.Thread.Sleep(1000);
                Assert.AreEqual("Standard", header.Text);
            }
        }

        public static void TearDown()
        {
            header = null;

            // Close the application and delete the session
            if (session != null)
            {
                session.Quit();
                session = null;
            }
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
            header.Click(); // Dismiss memory flyout that could be displayed if calculator is in compact mode
            System.Threading.Thread.Sleep(1000);
            return staleElement;
        }
    }
}
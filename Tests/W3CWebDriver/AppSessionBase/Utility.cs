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

using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using System;

namespace W3CWebDriver
{
    public class Utility
    {
        private static WindowsDriver<WindowsElement> orphanedSession;
        private static WindowsElement orphanedElement;

        ~Utility()
        {
            CleanupOrphanedSession();
        }

        public static WindowsDriver<WindowsElement> CreateNewSession(string appId)
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", appId);
            return new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
        }

        public static WindowsElement GetOrphanedElement()
        {
            // Re-initialize orphaned session and element if they are compromised
            if (orphanedSession == null || orphanedElement == null)
            {
                InitializeOrphanedSession();
            }

            return orphanedElement;
        }

        public static WindowsDriver<WindowsElement> GetOrphanedSession()
        {
            // Re-initialize orphaned session and element if they are compromised
            if (orphanedSession == null || orphanedElement == null)
            {
                InitializeOrphanedSession();
            }

            return orphanedSession;
        }

        private static void CleanupOrphanedSession()
        {
            orphanedElement = null;

            // Cleanup after the session if exists
            if (orphanedSession != null)
            {
                orphanedSession.Quit();
                orphanedSession = null;
            }
        }

        private static void InitializeOrphanedSession()
        {
            // Create new calculator session and close the window to get an orphaned element
            CleanupOrphanedSession();
            orphanedSession = CreateNewSession(CommonTestSettings.CalculatorAppId);
            orphanedElement = orphanedSession.FindElementByAccessibilityId("Header");
            orphanedSession.Close();
        }
    }
}
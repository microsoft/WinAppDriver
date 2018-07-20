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

namespace WebDriverAPI
{
    public class Utility
    {
        private static WindowsDriver<WindowsElement> orphanedSession;
        private static WindowsElement orphanedElement;
        private static string orphanedWindowHandle;

        ~Utility()
        {
            CleanupOrphanedSession();
        }

        public static WindowsDriver<WindowsElement> CreateNewSession(string appId, string argument = null)
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", appId);

            if (argument != null)
            {
                appCapabilities.SetCapability("appArguments", argument);
            }

            return new WindowsDriver<WindowsElement>(new Uri(CommonTestSettings.WindowsApplicationDriverUrl), appCapabilities);
        }

        public static bool CurrentWindowIsAlive(WindowsDriver<WindowsElement> remoteSession)
        {
            bool windowIsAlive = false;

            if (remoteSession != null)
            {
                try
                {
                    windowIsAlive = !string.IsNullOrEmpty(remoteSession.CurrentWindowHandle) && remoteSession.CurrentWindowHandle != "0";
                    windowIsAlive = true;
                }
                catch { }
            }

            return windowIsAlive;
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
            if (orphanedSession == null || orphanedElement == null || string.IsNullOrEmpty(orphanedWindowHandle))
            {
                InitializeOrphanedSession();
            }

            return orphanedSession;
        }

        public static string GetOrphanedWindowHandle()
        {
            // Re-initialize orphaned session and element if they are compromised
            if (orphanedSession == null || orphanedElement == null || string.IsNullOrEmpty(orphanedWindowHandle))
            {
                InitializeOrphanedSession();
            }

            return orphanedWindowHandle;
        }

        private static void CleanupOrphanedSession()
        {
            orphanedWindowHandle = null;
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
            orphanedElement = orphanedSession.FindCalculatorTitleByAccessibilityId();
            orphanedWindowHandle = orphanedSession.CurrentWindowHandle;
            orphanedSession.Close();
        }
    }
}
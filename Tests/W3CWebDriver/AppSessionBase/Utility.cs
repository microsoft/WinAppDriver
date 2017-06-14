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
    public class Utility
    {
        private static WindowsDriver<WindowsElement> orphanedSession;
        private static WindowsElement orphanedElement;

        public static WindowsElement GetOrphanedElement()
        {
            // Create new calculator session and close the window to get an orphaned element
            if (orphanedSession == null || orphanedElement == null)
            {
                if (orphanedSession == null)
                {
                    orphanedSession = CalculatorBase.CreateNewCalculatorSession();
                }

                Assert.IsNotNull(orphanedSession);
                orphanedElement = orphanedSession.FindElementByAccessibilityId("Header");
                orphanedSession.Close();
            }

            return orphanedElement;
        }

        ~Utility()
        {
            orphanedElement = null;

            // Cleanup after the session if exists
            if (orphanedSession != null)
            {
                orphanedSession.Quit();
                orphanedSession = null;
            }
        }
    }
}
//******************************************************************************
//
// Copyright (c) 2019 Microsoft Corporation. All rights reserved.
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
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;

namespace AbsoluteXPath
{
    public class DesktopSession
    {
        private const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723/";
        WindowsDriver<WindowsElement> desktopSession;

        public DesktopSession()
        {
            DesiredCapabilities appCapabilities = new DesiredCapabilities();
            appCapabilities.SetCapability("app", "Root");
            appCapabilities.SetCapability("deviceName", "WindowsPC");
            desktopSession = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
        }

        ~DesktopSession()
        {
            desktopSession.Quit();
        }

        public WindowsDriver<WindowsElement> DesktopSessionElement
        {
            get { return desktopSession; }
        }

        public WindowsElement FindElementByAbsoluteXPath(string xPath, int nTryCount = 15)
        {
            WindowsElement uiTarget = null;

            while (nTryCount-- > 0)
            {
                try
                {
                    uiTarget = desktopSession.FindElementByXPath(xPath);
                }
                catch
                {
                }

                if (uiTarget != null)
                {
                    break;
                }
                else
                {
                    System.Threading.Thread.Sleep(2000);
                }
            }

            return uiTarget;
        }
    }
}

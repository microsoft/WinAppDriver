//******************************************************************************
//
// Copyright (c) 2018 Microsoft Corporation. All rights reserved.
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium;

namespace UIXPathLib
{
    public class MyDesktopSession
    {
        private const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723/";
        public static WindowsDriver<WindowsElement> DesktopSession;

        public static void Setup()
        {
            if (DesktopSession == null)
            {
                DesiredCapabilities appCapabilities = new DesiredCapabilities();
                appCapabilities.SetCapability("app", "Root");
                appCapabilities.SetCapability("deviceName", "WindowsPC");
                DesktopSession = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
            }
        }

        public static void TearDown()
        {
            if (DesktopSession != null)
            {
                DesktopSession.Quit();
                DesktopSession = null;
            }
        }

        public static WindowsElement FindElementByXPath(string xPath)
        {
            WindowsElement uiTarget = null;
            int nTryCount = 10;

            while (nTryCount-- > 0)
            {
                try
                {
                    uiTarget = MyDesktopSession.DesktopSession.FindElementByXPath(xPath);
                }
                catch
                {
                }

                if (uiTarget != null)
                {
                    System.Threading.Thread.Sleep(500); // default delay
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

    class Program
    {
        static void Main(string[] args)
        {
            MyDesktopSession.Setup();
            System.Threading.Thread.Sleep(2000);

            //Paste generated code here


            MyDesktopSession.TearDown();
        }
    }
}

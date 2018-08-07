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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using System;

namespace StickyNotesTest
{
    public class StickyNotesSession
    {
        // Note: append /wd/hub to the URL if you're directing the test at Appium
        protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        private const string StickyNotesAppId = @"Microsoft.MicrosoftStickyNotes_8wekyb3d8bbwe!App";

        protected static WindowsDriver<WindowsElement> session;

        public static void Setup(TestContext context)
        {
            // Launch StickyNotes application if it is not yet launched
            if (session == null)
            {
                try
                {
                    // Create a new session to launch or bring up Sticky Notes application
                    // Note: All sticky note windows are parented to Modern_Sticky_Top_Window pane
                    DesiredCapabilities appCapabilities = new DesiredCapabilities();
                    appCapabilities.SetCapability("app", StickyNotesAppId);
                    appCapabilities.SetCapability("deviceName", "WindowsPC");
                    session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
                }
                catch
                {
                    // When Sticky Notes application was previously launched, the creation above may fail.
                    // In such failure, simply look for the Modern_Sticky_Top_Window pane using the Desktop
                    // session and create a new session based on the located top window pane.
                    DesiredCapabilities desktopCapabilities = new DesiredCapabilities();
                    desktopCapabilities.SetCapability("app", "Root");
                    desktopCapabilities.SetCapability("deviceName", "WindowsPC");
                    var desktopSession = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), desktopCapabilities);

                    var StickyNotesTopLevelWindow = desktopSession.FindElementByClassName("Modern_Sticky_Top_Window");
                    var StickyNotesTopLevelWindowHandle = StickyNotesTopLevelWindow.GetAttribute("NativeWindowHandle");
                    StickyNotesTopLevelWindowHandle = (int.Parse(StickyNotesTopLevelWindowHandle)).ToString("x"); // Convert to Hex

                    DesiredCapabilities appCapabilities = new DesiredCapabilities();
                    appCapabilities.SetCapability("appTopLevelWindow", StickyNotesTopLevelWindowHandle);
                    appCapabilities.SetCapability("deviceName", "WindowsPC");
                    session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
                }
                Assert.IsNotNull(session);

                // Set implicit timeout to 1.5 seconds to make element search to retry every 500 ms for at most three times
                session.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1.5);
            }
        }

        public static void TearDown()
        {
            // Close the application and delete the session
            if (session != null)
            {
                try
                {
                    // Sticky Notes applciation can be closed by explicitly closing any of the opened Sticky Notes window.
                    // Create a new session based on any of opened Sticky Notes window and close it to close the application.
                    var openedStickyNotes = session.FindElementsByClassName("ApplicationFrameWindow");
                    if (openedStickyNotes.Count > 0)
                    {
                        var newStickyNoteWindowHandle = openedStickyNotes[0].GetAttribute("NativeWindowHandle");
                        newStickyNoteWindowHandle = (int.Parse(newStickyNoteWindowHandle)).ToString("x"); // Convert to Hex

                        DesiredCapabilities appCapabilities = new DesiredCapabilities();
                        appCapabilities.SetCapability("appTopLevelWindow", newStickyNoteWindowHandle);
                        appCapabilities.SetCapability("deviceName", "WindowsPC");
                        var stickyNoteSession = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
                        stickyNoteSession.Close();
                    }
                }
                catch { }

                session.Quit();
                session = null;
            }
        }
    }
}

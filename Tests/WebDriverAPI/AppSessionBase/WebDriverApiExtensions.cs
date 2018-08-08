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
using System;

namespace WebDriverAPI
{
    static class WebDriverApiExtensions
    {
        internal static WindowsElement FindCalculatorTitleByAccessibilityId(this WindowsDriver<WindowsElement> session)
        {
            WindowsElement element;
            try
            {
                element = session.FindElementByAccessibilityId("AppName");
            }
            catch (InvalidOperationException)
            {
                element = session.FindElementByAccessibilityId("AppNameTitle");
            }
            return element;
        }

        internal static void DismissAlarmDialogIfThere(this WindowsDriver<WindowsElement> session)
        {
            try
            {
                session.FindElementByAccessibilityId("SecondaryButton").Click();
            }
            catch (InvalidOperationException)
            {
            }
        }
    }
}

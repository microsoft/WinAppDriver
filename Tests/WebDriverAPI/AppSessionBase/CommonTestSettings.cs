//******************************************************************************
//
// Copyright (c) 2016 Microsoft Corporation. All rights reserved.
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

namespace WebDriverAPI
{
    public class CommonTestSettings
    {
        public const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";

        public const string AlarmClockAppId = "Microsoft.WindowsAlarms_8wekyb3d8bbwe!App";
        public const string CalculatorAppId = "Microsoft.WindowsCalculator_8wekyb3d8bbwe!App";
        public const string DesktopAppId = "Root";
        public const string EdgeAppId = "Microsoft.MicrosoftEdge_8wekyb3d8bbwe!MicrosoftEdge";
        public const string ExplorerAppId = @"C:\Windows\System32\explorer.exe";
        public const string NotepadAppId = @"C:\Windows\System32\notepad.exe";

        public const string EdgeAboutBlankURL = "about:blank";
        public const string EdgeAboutFlagsURL = "about:flags";
        public const string EdgeAboutTabsURL = "about:tabs";

        public const string TestFileName = @"TestFile";
        public const string TestFolderLocation = @"%TEMP%";
    }

    public class ErrorStrings
    {
        public const string ElementNotVisible = "An element command could not be completed because the element is not pointer- or keyboard interactable.";
        public const string NoSuchElement = "An element could not be located on the page using the given search parameters.";
        public const string NoSuchWindow = "Currently selected window has been closed";
        public const string StaleElementReference = "An element command failed because the referenced element is no longer attached to the DOM.";
        public const string UnimplementedCommandLocator = "Unexpected error. Unimplemented Command: {0} locator strategy is not supported";
        public const string UnimplementedCommandTimeoutType = "Unexpected error. Unimplemented Command: {0} timeout type is not supported";
        public const string XPathLookupError = "Invalid XPath expression: {0} (XPathLookupError)";

        public const string ActionsNoSuchElement = "specified in the Actions origin is unknown or does not exist";
        public const string ActionsNullElement = "element is not an Object that represents a web element";
        public const string ActionsStaleElementReference = "specified in the Actions origin is no longer valid";
        public const string ActionsUnimplementedPointerType = "Currently only pen and touch pointer input source types are supported";
        public const string ActionsUnimplementedMultiPen = "Currently only a single (non-concurrent) pen input is supported";

        public const string ActionsArgumentButton = "\"button\" in a pointer action JSON payload is undefined or is not an Integer greater than or equal to 0";
        public const string ActionsArgumentDuration = "\"duration\" in a pointer action JSON payload is not an Integer greater than or equal to 0";
        public const string ActionsArgumentOrigin = "\"origin\" in a action JSON payload is not equal to \"viewport\" or \"pointer\" and element is not an Object that represents a web element";
        public const string ActionsArgumentParameterHeight = "\"height\" attribute is not a floating point value greater or equal to 1";
        public const string ActionsArgumentParameterWidth = "\"width\" attribute is not a floating point value greater or equal to 1";
        public const string ActionsArgumentParameterMissingWidthOrHeight = "\"width\" and \"height\" attributes need to be specified together";
        public const string ActionsArgumentParameterPressure = "\"pressure\" attribute is not a floating point value between 0 and 1";
        public const string ActionsArgumentParameterTiltX = "\"tiltX\" attribute is not an integer value between -90 and 90";
        public const string ActionsArgumentParameterTiltY = "\"tiltY\" attribute is not an integer value between -90 and 90";
        public const string ActionsArgumentParameterTwist = "\"twist\" attribute is not an integer value between 0 and 359";
    }
}

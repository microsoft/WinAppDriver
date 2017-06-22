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

namespace W3CWebDriver
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
        public const string MicrosoftUrl = "www.microsoft.com";
        public const string GitHubUrl = "https://github.com/Microsoft/WinAppDriver";
        public const string TestFileName = "TestFile.txt";
        public const string TestFolderLocation = "%TEMP%";
    }

    public class ErrorStrings
    {
        public const string ElementNotVisible = "An element command could not be completed because the element is not pointer- or keyboard interactable.";
        public const string NoSuchElement = "An element could not be located on the page using the given search parameters.";
        public const string NoSuchWindow = "Currently selected window has been closed";
        public const string StaleElementReference = "An element command failed because the referenced element is no longer attached to the DOM.";
    }
}

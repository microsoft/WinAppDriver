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
using System.Text;
using System.Runtime.InteropServices;

namespace WinAppDriverUIRecorder
{
    class NativeMethods
    {
        public const int BUFFERSIZE = 4096 * 4; // must be same as BUFFERSIZE defined in UiTreeWalk.h

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetProcessDPIAware();

        [DllImport("User32.Dll")]
        public static extern bool PostMessage(IntPtr hWnd, uint msg, uint wParam, uint lParam);

        [DllImport("user32.dll")]
        public static extern IntPtr WindowFromPoint(int x, int y);

        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();

        [DllImport("user32.dll")]
        public static extern bool GetPhysicalCursorPos(out System.Drawing.Point point);

        [DllImport("user32.dll")]
        public static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("USER32.dll")]
        public static extern short GetKeyState(int nVirtKey);

        [DllImport("UIXPathLib.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern uint GetKeyboardReading(uint wParam, uint nParam, ref int vKeyCode, ref int scanCode);

        [DllImport("UIXPathLib.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern uint SetWindowsHookExNative(HookProc hProc, uint nHookId, uint nThreadId);

        [DllImport("UIXPathLib.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern IntPtr UninitializeHook(uint hId);

        [DllImport("UIXPathLib.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        public static extern int GetUiXPath(int left, int top, StringBuilder s, int nMaxCount);

        [DllImport("UIXPathLib.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern void InitUiTreeWalk();

        [DllImport("UIXPathLib.dll", CallingConvention = CallingConvention.Winapi)]
        public static extern void UnInitUiTreeWalk();

        [DllImport("UIXPathLib.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Unicode)]
        public static extern int HighlightCachedUI(string strRuntimeId, ref RECT rect);
    }
}

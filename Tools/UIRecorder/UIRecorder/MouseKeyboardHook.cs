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
using System.Runtime.InteropServices;

namespace WinAppDriverUIRecorder
{
    [StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;
    }

    public delegate IntPtr HookProc(int nCode, IntPtr wParam, IntPtr lParam);

    public enum KeyboardEvents
    {
        /// <include file='ManagedHooks.xml' path='Docs/General/Empty/*'/>
        KeyDown = 0x0100,
        /// <include file='ManagedHooks.xml' path='Docs/General/Empty/*'/>
        KeyUp = 0x0101,
        /// <include file='ManagedHooks.xml' path='Docs/General/Empty/*'/>
        SystemKeyDown = 0x0104,
        /// <include file='ManagedHooks.xml' path='Docs/General/Empty/*'/>
        SystemKeyUp = 0x0105
    }

    // https://www.pinvoke.net/default.aspx/user32.GetKeyState + A-Z
    public enum VirtualKeys : int
    {
        VK_LBUTTON = 0x01,
        VK_RBUTTON = 0x02,
        VK_CANCEL = 0x03,
        VK_MBUTTON = 0x04,
        //
        VK_XBUTTON1 = 0x05,
        VK_XBUTTON2 = 0x06,
        //
        VK_BACK = 0x08,
        VK_TAB = 0x09,
        //
        VK_CLEAR = 0x0C,
        VK_RETURN = 0x0D,
        //
        VK_SHIFT = 0x10,
        VK_CONTROL = 0x11,
        VK_MENU = 0x12,
        VK_PAUSE = 0x13,
        VK_CAPITAL = 0x14,
        //
        VK_KANA = 0x15,
        VK_HANGEUL = 0x15,  /* old name - should be here for compatibility */
        VK_HANGUL = 0x15,
        VK_JUNJA = 0x17,
        VK_FINAL = 0x18,
        VK_HANJA = 0x19,
        VK_KANJI = 0x19,
        //
        VK_ESCAPE = 0x1B,
        //
        VK_CONVERT = 0x1C,
        VK_NONCONVERT = 0x1D,
        VK_ACCEPT = 0x1E,
        VK_MODECHANGE = 0x1F,
        //
        VK_SPACE = 0x20,
        VK_PRIOR = 0x21,
        VK_NEXT = 0x22,
        VK_END = 0x23,
        VK_HOME = 0x24,
        VK_LEFT = 0x25,
        VK_UP = 0x26,
        VK_RIGHT = 0x27,
        VK_DOWN = 0x28,
        VK_SELECT = 0x29,
        VK_PRINT = 0x2A,
        VK_EXECUTE = 0x2B,
        VK_SNAPSHOT = 0x2C,
        VK_INSERT = 0x2D,
        VK_DELETE = 0x2E,
        VK_HELP = 0x2F,

        A = 0x41,
        B = 0x42,
        C = 0x43,
        D = 0x44,
        E = 0x45,
        F = 0x46,
        G = 0x47,
        H = 0x48,
        I = 0x49,
        J = 0x4A,
        K = 0x4B,
        L = 0x4C,
        M = 0x4D,
        N = 0x4E,
        O = 0x4F,
        P = 0x50,
        Q = 0x51,
        R = 0x52,
        S = 0x53,
        T = 0x54,
        U = 0x55,
        V = 0x56,
        W = 0x57,
        X = 0x58,
        Y = 0x59,
        Z = 0x5A,

        //
        VK_LWIN = 0x5B,
        VK_RWIN = 0x5C,
        VK_APPS = 0x5D,
        //
        VK_SLEEP = 0x5F,
        //
        VK_NUMPAD0 = 0x60,
        VK_NUMPAD1 = 0x61,
        VK_NUMPAD2 = 0x62,
        VK_NUMPAD3 = 0x63,
        VK_NUMPAD4 = 0x64,
        VK_NUMPAD5 = 0x65,
        VK_NUMPAD6 = 0x66,
        VK_NUMPAD7 = 0x67,
        VK_NUMPAD8 = 0x68,
        VK_NUMPAD9 = 0x69,
        VK_MULTIPLY = 0x6A,
        VK_ADD = 0x6B,
        VK_SEPARATOR = 0x6C,
        VK_SUBTRACT = 0x6D,
        VK_DECIMAL = 0x6E,
        VK_DIVIDE = 0x6F,
        VK_F1 = 0x70,
        VK_F2 = 0x71,
        VK_F3 = 0x72,
        VK_F4 = 0x73,
        VK_F5 = 0x74,
        VK_F6 = 0x75,
        VK_F7 = 0x76,
        VK_F8 = 0x77,
        VK_F9 = 0x78,
        VK_F10 = 0x79,
        VK_F11 = 0x7A,
        VK_F12 = 0x7B,
        VK_F13 = 0x7C,
        VK_F14 = 0x7D,
        VK_F15 = 0x7E,
        VK_F16 = 0x7F,
        VK_F17 = 0x80,
        VK_F18 = 0x81,
        VK_F19 = 0x82,
        VK_F20 = 0x83,
        VK_F21 = 0x84,
        VK_F22 = 0x85,
        VK_F23 = 0x86,
        VK_F24 = 0x87,
        //
        VK_NUMLOCK = 0x90,
        VK_SCROLL = 0x91,
        //
        VK_OEM_NEC_EQUAL = 0x92,   // '=' key on numpad
                                   //
        VK_OEM_FJ_JISHO = 0x92,   // 'Dictionary' key
        VK_OEM_FJ_MASSHOU = 0x93,   // 'Unregister word' key
        VK_OEM_FJ_TOUROKU = 0x94,   // 'Register word' key
        VK_OEM_FJ_LOYA = 0x95,   // 'Left OYAYUBI' key
        VK_OEM_FJ_ROYA = 0x96,   // 'Right OYAYUBI' key
                                 //
        VK_LSHIFT = 0xA0,
        VK_RSHIFT = 0xA1,
        VK_LCONTROL = 0xA2,
        VK_RCONTROL = 0xA3,
        VK_LMENU = 0xA4,
        VK_RMENU = 0xA5,
        //
        VK_BROWSER_BACK = 0xA6,
        VK_BROWSER_FORWARD = 0xA7,
        VK_BROWSER_REFRESH = 0xA8,
        VK_BROWSER_STOP = 0xA9,
        VK_BROWSER_SEARCH = 0xAA,
        VK_BROWSER_FAVORITES = 0xAB,
        VK_BROWSER_HOME = 0xAC,
        //
        VK_VOLUME_MUTE = 0xAD,
        VK_VOLUME_DOWN = 0xAE,
        VK_VOLUME_UP = 0xAF,
        VK_MEDIA_NEXT_TRACK = 0xB0,
        VK_MEDIA_PREV_TRACK = 0xB1,
        VK_MEDIA_STOP = 0xB2,
        VK_MEDIA_PLAY_PAUSE = 0xB3,
        VK_LAUNCH_MAIL = 0xB4,
        VK_LAUNCH_MEDIA_SELECT = 0xB5,
        VK_LAUNCH_APP1 = 0xB6,
        VK_LAUNCH_APP2 = 0xB7,
        //
        VK_OEM_1 = 0xBA,   // ';:' for US
        VK_OEM_PLUS = 0xBB,   // '+' any country
        VK_OEM_COMMA = 0xBC,   // ',' any country
        VK_OEM_MINUS = 0xBD,   // '-' any country
        VK_OEM_PERIOD = 0xBE,   // '.' any country
        VK_OEM_2 = 0xBF,   // '/?' for US
        VK_OEM_3 = 0xC0,   // '`~' for US
                           //
        VK_OEM_4 = 0xDB,  //  '[{' for US
        VK_OEM_5 = 0xDC,  //  '\|' for US
        VK_OEM_6 = 0xDD,  //  ']}' for US
        VK_OEM_7 = 0xDE,  //  ''"' for US
        VK_OEM_8 = 0xDF,
        //
        VK_OEM_AX = 0xE1,  //  'AX' key on Japanese AX kbd
        VK_OEM_102 = 0xE2,  //  "<>" or "\|" on RT 102-key kbd.
        VK_ICO_HELP = 0xE3,  //  Help key on ICO
        VK_ICO_00 = 0xE4,  //  00 key on ICO
                           //
        VK_PROCESSKEY = 0xE5,
        //
        VK_ICO_CLEAR = 0xE6,
        //
        VK_PACKET = 0xE7,
        //
        VK_OEM_RESET = 0xE9,
        VK_OEM_JUMP = 0xEA,
        VK_OEM_PA1 = 0xEB,
        VK_OEM_PA2 = 0xEC,
        VK_OEM_PA3 = 0xED,
        VK_OEM_WSCTRL = 0xEE,
        VK_OEM_CUSEL = 0xEF,
        VK_OEM_ATTN = 0xF0,
        VK_OEM_FINISH = 0xF1,
        VK_OEM_COPY = 0xF2,
        VK_OEM_AUTO = 0xF3,
        VK_OEM_ENLW = 0xF4,
        VK_OEM_BACKTAB = 0xF5,
        //
        VK_ATTN = 0xF6,
        VK_CRSEL = 0xF7,
        VK_EXSEL = 0xF8,
        VK_EREOF = 0xF9,
        VK_PLAY = 0xFA,
        VK_ZOOM = 0xFB,
        VK_NONAME = 0xFC,
        VK_PA1 = 0xFD,
        VK_OEM_CLEAR = 0xFE
    }

    class MouseKeyboardHook
    {
        //Declare MouseHookProcedure as a HookProc type.
        static HookProc MouseHookProcedure;
        static HookProc KeyboardHookProcedure;

        //Declare the hook handle as an int.
        static uint s_hHookMouse = 0;
        static uint s_hHookKeyboard = 0;

        public static bool s_bPauseMouseKeyboard = false;

        // WinUser.h
        public const int WM_MOUSEFIRST = 0x0200;
        public const int WM_MOUSEMOVE = 0x0200;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONUP = 0x0202;
        public const int WM_LBUTTONDBLCLK = 0x0203;
        public const int WM_RBUTTONDOWN = 0x0204;
        public const int WM_RBUTTONUP = 0x0205;
        public const int WM_RBUTTONDBLCLK = 0x0206;
        public const int WM_MBUTTONDOWN = 0x0207;
        public const int WM_MBUTTONUP = 0x0208;
        public const int WM_MBUTTONDBLCLK = 0x0209;
        public const int WM_MOUSEWHEEL = 0x020A;
        public const int WM_XBUTTONDOWN = 0x020B;
        public const int WM_XBUTTONUP = 0x020C;
        public const int WM_XBUTTONDBLCLK = 0x020D;

        //Declare the mouse hook constant.
        public const uint WH_JOURNALRECORD = 0;
        public const uint WH_JOURNALPLAYBACK = 1;
        public const uint WH_KEYBOARD = 2;
        public const uint WH_GETMESSAGE = 3;
        public const uint WH_CALLWNDPROC = 4;
        public const uint WH_CBT = 5;
        public const uint WH_SYSMSGFILTER = 6;
        public const uint WH_MOUSE = 7;
        public const uint WH_HARDWARE = 8;
        public const uint WH_DEBUG = 9;
        public const uint WH_SHELL = 10;
        public const uint WH_FOREGROUNDIDLE = 11;
        public const uint WH_CALLWNDPROCRET = 12;
        public const uint WH_KEYBOARD_LL = 13;
        public const uint WH_MOUSE_LL = 14;

        [StructLayout(LayoutKind.Sequential)]
        public class POINT
        {
            public int X;
            public int Y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public uint dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class KBDLLHOOKSTRUCT
        {
            public uint vkCode;
            public uint scanCode;
            public uint flags;
            public uint time;
            public uint dwExtraInfo;
        }

        private static void StartMouseHook()
        {
            StopMouseHook();

            MouseHookProcedure = new HookProc(MouseHookProc);
            s_hHookMouse = (uint)NativeMethods.SetWindowsHookExNative(MouseHookProcedure, (uint)WH_MOUSE_LL, NativeMethods.GetCurrentThreadId());
        }

        private static void StopMouseHook()
        {
            if (s_hHookMouse != 0)
            {
                NativeMethods.UninitializeHook(s_hHookMouse);
                s_hHookMouse = 0;
            }

            MouseHookProcedure = null;
        }

        private static void StartKeyboardHook()
        {
            StopKeyboardHook();

            KeyboardHookProcedure = new HookProc(KeyboardHookProc);
            s_hHookKeyboard = (uint)NativeMethods.SetWindowsHookExNative(KeyboardHookProcedure, (uint)WH_KEYBOARD_LL, 0);
        }

        private static void StopKeyboardHook()
        {
            if (s_hHookKeyboard != 0)
            {
                NativeMethods.UninitializeHook(s_hHookKeyboard);
                s_hHookKeyboard = 0;
            }

            KeyboardHookProcedure = null;
        }

        public static bool StartHook()
        {
            s_bPauseMouseKeyboard = false;

            MouseKeyboardHook.StartMouseHook();
            MouseKeyboardHook.StartKeyboardHook();

            return (s_hHookKeyboard != 0 && s_hHookMouse != 0);
        }

        public static void StopHook()
        {
            MouseKeyboardHook.StopMouseHook();
            MouseKeyboardHook.StopKeyboardHook();
        }

        public static IntPtr KeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0 || MouseKeyboardEventHandler.s_threadRecorder == null || MainWindow.s_mainWin.IsRecording == false)
            {
                return NativeMethods.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
            }

            KeyboardEvents kEvent = (KeyboardEvents)wParam.ToInt32();
            KBDLLHOOKSTRUCT kbd = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));

            int scanCode = (int)kbd.scanCode;
            VirtualKeys vk = (VirtualKeys)kbd.vkCode;

            if (vk == VirtualKeys.VK_PAUSE && kEvent == KeyboardEvents.KeyDown)
            {
                s_bPauseMouseKeyboard = !s_bPauseMouseKeyboard;

                if (s_bPauseMouseKeyboard == true)
                    NativeMethods.PostMessage(MainWindow.s_windowHandle, (int)MainWindow.UiThreadTask.PauseRecording, 0, 0);
                else
                    NativeMethods.PostMessage(MainWindow.s_windowHandle, (int)MainWindow.UiThreadTask.Active, 0, 0);
            }
          
            if(s_bPauseMouseKeyboard == false)
            {
                MouseKeyboardEventHandler.RecordKey(kEvent, vk, scanCode);
            }

            return NativeMethods.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        public static IntPtr MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0 || MouseKeyboardEventHandler.s_threadRecorder == null || s_bPauseMouseKeyboard == true)
            {
                return NativeMethods.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
            }

            MSLLHOOKSTRUCT mhs = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
            int left = mhs.pt.X;
            int top = mhs.pt.Y;

            //skip if cursor is on this app
            IntPtr hwnd = NativeMethods.WindowFromPoint(left, top);
            if (hwnd == MainWindow.s_windowHandle)
            {
                MouseKeyboardEventHandler.ResetRecordTimer();
                return NativeMethods.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
            }

            switch (wParam.ToInt32())
            {
                case WM_MOUSEMOVE:
                    MouseKeyboardEventHandler.MouseMove(left, top);
                    break;

                case WM_MOUSEWHEEL:
                    short zDelta = (short)(mhs.mouseData >> 16);
                    MouseKeyboardEventHandler.MouseWheel(left, top, zDelta);
                    break;

                case WM_LBUTTONDOWN:
                    MouseKeyboardEventHandler.LeftMouseDown(left, top);
                    break;

                case WM_LBUTTONUP:
                    MouseKeyboardEventHandler.LeftMouseUp(left, top);
                    break;

                case WM_RBUTTONDOWN:
                    MouseKeyboardEventHandler.RightMouseDown(left, top);
                    break;

                case WM_RBUTTONUP:
                    MouseKeyboardEventHandler.RightMouseUp(left, top);
                    break;
            }

            return NativeMethods.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }
    }
}

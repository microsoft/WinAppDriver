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
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Automation;
using System.Timers;
using System.Threading;

namespace WinAppDriverUIRecorder
{
    public enum MouseState
    {
        LeftMouseUp,
        LeftMouseDown,
        LeftMouseDrag
    }

    class MouseKeyboardEventHandler
    {
        // keyboard events
        public static List<byte> s_listRecordedKeycode = new List<byte>();
        public static int s_keyboardInputTick;
        static VirtualKeys s_lastKeyCode = VirtualKeys.VK_RETURN;
        static bool s_lastKeyDown = false;

        // mosue events
        static int tickLeftUp = 0;
        static MouseState mouseState = MouseState.LeftMouseUp;
        static System.Drawing.Point ptCursorDown = new System.Drawing.Point(0, 0);
        static System.Drawing.Point ptCursorUp = new System.Drawing.Point(0, 0);
        static System.Drawing.Point ptCursorMove = new System.Drawing.Point(0, 0);

        // recording thread
        static Thread s_threadRecorder = null;
        static ManualResetEvent s_eventQuitRecording = new ManualResetEvent(false);
        static ManualResetEvent s_eventRecordNow = new ManualResetEvent(false);
        static System.Timers.Timer s_timerFromPoint = null;

        static System.Drawing.Point ptUiWalking = new System.Drawing.Point(0, 0);
        const int nDoubleClickDelta = 750;
        const int nDelayRecord = 125;
        const int nMinDist = 15;
        static int nHoverCount = 0;
        const int nHoverDelay = 5000;

        static string s_strXmlNodes = null;
        static object s_lockUiPath = new object();
        public static bool s_bWinUiPlayer = false;
        static int s_nPathHash = 0;

        static bool s_bCapsLock = false;
        static bool s_bNumLock = false;
        static bool s_bScrollLock = false;

        public static void Init()
        {
            MouseKeyboardHook.StartHook();

            s_strXmlNodes = null;
            s_listRecordedKeycode.Clear();

            s_eventQuitRecording.Reset();
            s_eventRecordNow.Reset();

            s_threadRecorder = new Thread(RecorderThread);
            s_threadRecorder.Start();

            if (s_timerFromPoint == null)
            {
                s_timerFromPoint = new System.Timers.Timer(nDelayRecord) { Enabled = true, AutoReset = false };
                s_timerFromPoint.Elapsed += OnTimedEvent;
            }
        }

        public static void UnInit()
        {
            if (s_timerFromPoint != null)
            {
                s_timerFromPoint.Stop();
                s_timerFromPoint.Elapsed -= OnTimedEvent;
                s_timerFromPoint.Close();
                s_timerFromPoint = null;
            }

            MouseKeyboardHook.StopHook();

            s_eventRecordNow.Reset();

            s_eventQuitRecording.Reset();
            if (s_threadRecorder != null)
            {
                s_threadRecorder.Join(1000);
                s_threadRecorder = null;
            }
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            s_eventRecordNow.Set();
        }

        public static void ResetRecordTimer()
        {
            s_timerFromPoint.Stop();
            s_timerFromPoint.Start();
        }

        private static void RecorderThread()
        {
            StringBuilder sb = new StringBuilder(4096);
            System.Drawing.Point pt = new System.Drawing.Point(0, 0);

            while (true)
            {
                bool bStartWalk = s_eventRecordNow.WaitOne(nDelayRecord);

                if (s_eventQuitRecording.WaitOne(0))
                {
                    break;
                }

                NativeMethods.GetPhysicalCursorPos(out pt);
                int dist = Math.Abs(pt.X - ptUiWalking.X) + Math.Abs(pt.Y - ptUiWalking.Y);

                if (bStartWalk)
                {
                    // check if cursor has moved
                    if (dist > nMinDist)
                    {
                        ptUiWalking.X = pt.X;
                        ptUiWalking.Y = pt.Y;

                        NativeMethods.GetUiXPath(ptUiWalking.X, ptUiWalking.Y, sb, sb.Capacity);

                        if (sb.Length > 0)
                        {
                            string strXmlNode = sb.ToString();
                            int nHash = strXmlNode.GetHashCode();
                            if (nHash != s_nPathHash)
                            {
                                lock (s_lockUiPath)
                                {
                                    MouseKeyboardEventHandler.s_strXmlNodes = $"({pt.X} {pt.Y}) " + strXmlNode;
                                }
                                s_nPathHash = nHash;
                                nHoverCount = 0;

                                if (mouseState == MouseState.LeftMouseUp)
                                {
                                    XmlNodePathRecorder.SetUiToRootXmlNodes(ref s_strXmlNodes, true);
                                    NativeMethods.PostMessage(MainWindow.windowHandle, (uint)UiTaskName.Inspect, 0, 0);
                                }
                            }
                        }
                    }

                    s_eventRecordNow.Reset();
                }
            }
        }

        public static void EnsureXPathReady(int nWaitTime = 1000)
        {
            // Get xpath, if it is empty, without waiting for timer to fire
            if (string.IsNullOrEmpty(s_strXmlNodes))
            {
                s_eventRecordNow.Set();

                while (s_eventRecordNow.WaitOne(0) && nWaitTime > 0)
                {
                    System.Threading.Thread.Sleep(10);
                    nWaitTime -= 10;
                }
            }
        }

        public static void MouseMove(int left, int top)
        {
            ResetRecordTimer();

            int dist = Math.Abs(left - ptUiWalking.X) + Math.Abs(top - ptUiWalking.Y);

            int moveDelta = Math.Abs(left - ptCursorMove.X) + Math.Abs(top - ptCursorMove.Y);
            ptCursorMove.X = left;
            ptCursorMove.Y = top;

            if (mouseState == MouseState.LeftMouseUp)
            {
                if (dist > nMinDist)
                {
                    if ((Environment.TickCount - s_keyboardInputTick) > 2000)
                    {
                        PublishKeyboardInput();
                    }
                }

                if (moveDelta < 2 && string.IsNullOrEmpty(s_strXmlNodes) == false)
                {
                    nHoverCount++;
                }

                if (nHoverCount > nMinDist)
                {
                    nHoverCount = 0;
                    XmlNodePathRecorder.SetUiToRootXmlNodes(ref s_strXmlNodes, false);
                    NativeMethods.PostMessage(MainWindow.windowHandle, (uint)UiTaskName.MouseHover, 0, 0);
                }
            }
            else if (mouseState == MouseState.LeftMouseDown) // drag start - down and up are at different points
            {
                if (dist > nMinDist)
                {
                    mouseState = MouseState.LeftMouseDrag;

                    PublishKeyboardInput();

                    if (s_strXmlNodes != null)
                    {
                        lock (s_lockUiPath)
                        {
                            XmlNodePathRecorder.SetUiToRootXmlNodes(ref s_strXmlNodes);
                            NativeMethods.PostMessage(MainWindow.windowHandle, (uint)UiTaskName.Drag, 0, 0);
                        }
                    }

                    ptCursorDown.X = left;
                    ptCursorDown.Y = top;
                }
            }
            else if (mouseState == MouseState.LeftMouseDrag)
            {
            }
            else
            {
                //log error
            }
        }

        public static void MouseWheel(int left, int top, short delta)
        {
            lock (MouseKeyboardEventHandler.s_lockUiPath)
            {
                if (s_strXmlNodes != null)
                {
                    XmlNodePathRecorder.SetUiToRootXmlNodes(ref s_strXmlNodes);
                }
            }

            NativeMethods.PostMessage(MainWindow.windowHandle, (uint)UiTaskName.MouseWheel, (uint)(ConstVariables.DragDeltaOffset + delta), 0);
            ResetRecordTimer();
        }

        public static void LeftMouseDown(int left, int top)
        {
            ptCursorDown.X = left;
            ptCursorDown.Y = top;

            if (mouseState == MouseState.LeftMouseUp)
            {
                mouseState = MouseState.LeftMouseDown;
            }
            else if (mouseState == MouseState.LeftMouseDown)
            {
                //should not get here
            }
            else if (mouseState == MouseState.LeftMouseDrag)
            {
                //should not get here
            }
            else
            {
                //log error
            }

            ResetRecordTimer();
        }

        public static void LeftMouseUp(int left, int top)
        {
            ptCursorUp.X = left;
            ptCursorUp.Y = top;

            int dist = Math.Abs(left - ptCursorDown.X) + Math.Abs(top - ptCursorDown.Y);

            if (dist <= nMinDist)
            {
                //Check if it is double click
                if ((Environment.TickCount - tickLeftUp) < nDoubleClickDelta)
                {
                    NativeMethods.PostMessage(MainWindow.windowHandle, (uint)UiTaskName.LeftDblClick, 0, 0);
                }
                else
                {
                    EnsureXPathReady();

                    // Create a keyboard task if keys are recorded 
                    PublishKeyboardInput();

                    // Create a click task
                    if (!string.IsNullOrEmpty(s_strXmlNodes))
                    {
                        lock (MouseKeyboardEventHandler.s_lockUiPath)
                        {
                            XmlNodePathRecorder.SetUiToRootXmlNodes(ref s_strXmlNodes);
                            NativeMethods.PostMessage(MainWindow.windowHandle, (uint)UiTaskName.LeftClick, 0, 0);
                        }
                    }
                }
            }
            else if (mouseState == MouseState.LeftMouseDrag) // drag stop - down and up are at different points
            {
                int dragDeltaX = ptCursorUp.X - ptCursorDown.X;
                int dragDeltaY = ptCursorUp.Y - ptCursorDown.Y;
                //Add DragDeltaOffset to dragDeltaX and dragDeltaY in case they are negative
                NativeMethods.PostMessage(MainWindow.windowHandle, (uint)UiTaskName.DragStop, (uint)(ConstVariables.DragDeltaOffset + dragDeltaX), (uint)(ConstVariables.DragDeltaOffset + dragDeltaY));
            }
            else
            {
                //log error
            }

            tickLeftUp = Environment.TickCount;

            mouseState = MouseState.LeftMouseUp;
        }

        public static void RightMouseDown(int left, int top)
        {
            ResetRecordTimer();
        }

        public static void RightMouseUp(int left, int top)
        {
            EnsureXPathReady();

            PublishKeyboardInput();

            if (s_strXmlNodes != null)
            {
                lock (MouseKeyboardEventHandler.s_lockUiPath)
                {
                    XmlNodePathRecorder.SetUiToRootXmlNodes(ref s_strXmlNodes);
                    NativeMethods.PostMessage(MainWindow.windowHandle, (uint)UiTaskName.RightClick, 0, 0);
                }
            }
        }

        static int PublishKeyboardInput()
        {
            if (s_listRecordedKeycode.Count == 0)
            {
                return 0;
            }

            string strBase64 = Convert.ToBase64String(s_listRecordedKeycode.ToArray());
            s_listRecordedKeycode.Clear();

            XmlNodePathRecorder.SetBase64KeyboardInput(strBase64, s_bCapsLock, s_bNumLock, s_bScrollLock);
            NativeMethods.PostMessage(MainWindow.windowHandle, (uint)UiTaskName.KeyboardInput, (uint)s_keyboardInputTick, 0);

            return strBase64.Length;
        }

        public static void RecordKey(KeyboardEvents keyEvent, VirtualKeys vKey, int scanCode)
        {
            ResetRecordTimer();

            bool bIsKeydown = keyEvent == KeyboardEvents.SystemKeyDown || keyEvent == KeyboardEvents.KeyDown;

            // return if same key and up/down state
            if (s_lastKeyCode == vKey)
            {
                if (s_lastKeyDown == bIsKeydown)
                {
                    return;
                }
            }

            s_lastKeyDown = bIsKeydown;
            s_lastKeyCode = vKey;

            if (s_listRecordedKeycode.Count == 0)
            {
                s_keyboardInputTick = Environment.TickCount;
                s_bCapsLock = (NativeMethods.GetKeyState((int)VirtualKeys.VK_CAPITAL) & 0x0001) != 0;
                s_bNumLock = (NativeMethods.GetKeyState((int)VirtualKeys.VK_NUMLOCK) & 0x0001) != 0;
                s_bScrollLock = (NativeMethods.GetKeyState((int)VirtualKeys.VK_SCROLL) & 0x0001) != 0;
            }

            // record down/up state
            if (bIsKeydown)
                s_listRecordedKeycode.Add((byte)0);
            else
                s_listRecordedKeycode.Add((byte)1);

            // store acutal key code
            s_listRecordedKeycode.Add((byte)vKey);

            if (bIsKeydown == false)
            {
                if (vKey == VirtualKeys.VK_RETURN
                    || vKey == VirtualKeys.VK_ESCAPE
                    || (VirtualKeys.VK_F1 <= vKey && vKey <= VirtualKeys.VK_F24))
                {
                    PublishKeyboardInput();
                    return;
                }
            }
        }
    }
}
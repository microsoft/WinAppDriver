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
        // keyboard state
        static List<byte> s_listRecordedKeycode = new List<byte>();
        static int s_keyboardInputTick;
        static VirtualKeys s_lastKeyCode = VirtualKeys.VK_RETURN;
        static bool s_lastKeyDown = false;
        static bool s_bCapsLock = false;
        static bool s_bNumLock = false;
        static bool s_bScrollLock = false;

        // mosue events
        static int tickLeftUp = 0;
        static MouseState mouseState = MouseState.LeftMouseUp;
        static System.Drawing.Point ptCursorDown = new System.Drawing.Point(0, 0);
        static System.Drawing.Point ptCursorUp = new System.Drawing.Point(0, 0);

        // recording thread
        public static Thread s_threadRecorder = null;
        static ManualResetEvent s_eventQuitRecording = new ManualResetEvent(false);
        static ManualResetEvent s_eventRecordNow = new ManualResetEvent(false);
        static System.Timers.Timer s_timerFromPoint = null;

        static System.Drawing.Point ptUiWalking = new System.Drawing.Point(0, 0);
        const int nDoubleClickDelta = 750;
        const int nDelayRecord = 125;
        const int nMinDist = 15;
        static string s_strXmlNodes = null;
        static object s_lockUiPath = new object();
        static int s_nPathHash = 0;

        public static void Init()
        {
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

            MouseKeyboardHook.StartHook();
        }

        public static void UnInit()
        {
            MouseKeyboardHook.StopHook();

            if (s_timerFromPoint != null)
            {
                s_timerFromPoint.Stop();
                s_timerFromPoint.Elapsed -= OnTimedEvent;
                s_timerFromPoint.Close();
                s_timerFromPoint = null;
            }

            s_eventRecordNow.Reset();

            s_eventQuitRecording.Set();
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
            StringBuilder sb = new StringBuilder(NativeMethods.BUFFERSIZE);
            System.Drawing.Point pt = new System.Drawing.Point(0, 0);

            while (true)
            {
                bool bStartWalk = s_eventRecordNow.WaitOne(nDelayRecord);

                PublishKeyboardInput();

                if (s_eventQuitRecording.WaitOne(0))
                    break;

                NativeMethods.GetPhysicalCursorPos(out pt);
                int dist = Math.Abs(pt.X - ptUiWalking.X) + Math.Abs(pt.Y - ptUiWalking.Y);

                if (bStartWalk && MouseKeyboardHook.s_bPauseMouseKeyboard == false)
                {
                    // check if cursor has moved
                    if (dist > nMinDist)
                    {
                        NativeMethods.PostMessage(MainWindow.s_windowHandle, (int)MainWindow.UiThreadTask.Active, 0, 0);

                        ptUiWalking.X = pt.X;
                        ptUiWalking.Y = pt.Y;

                        var tick = Environment.TickCount;

                        NativeMethods.GetUiXPath(ptUiWalking.X, ptUiWalking.Y, sb, sb.Capacity);
                        AppInsights.LogMetric("GetUiXPathPerf", Environment.TickCount - tick);

                        if (MouseKeyboardHook.s_bPauseMouseKeyboard == true)
                            continue;

                        if (s_eventQuitRecording.WaitOne(0))
                            break;

                        string strXmlNode = sb.ToString();
                        if (!string.IsNullOrEmpty(strXmlNode))
                        {
                            int nHash = strXmlNode.GetHashCode();
                            if (nHash != s_nPathHash)
                            {
                                lock (s_lockUiPath)
                                {
                                    MouseKeyboardEventHandler.s_strXmlNodes = strXmlNode;

                                    s_nPathHash = nHash;
                                }

                                XmlNodePathRecorder.HandleUiEvent(ref s_strXmlNodes, EnumUiTaskName.Inspect, 0, 0);
                            }
                        }
                    }

                    s_eventRecordNow.Reset();
                }
            }
        }

        public static void MouseMove(int left, int top)
        {
            ResetRecordTimer();

        }

        public static void MouseWheel(int left, int top, short delta)
        {
            lock (MouseKeyboardEventHandler.s_lockUiPath)
            {
                XmlNodePathRecorder.HandleUiEvent(ref s_strXmlNodes, EnumUiTaskName.MouseWheel, (int)delta, 0);
                s_strXmlNodes = null;
            }
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
                if ((Environment.TickCount - tickLeftUp) < nDoubleClickDelta && (Environment.TickCount - tickLeftUp) > 1)
                {
                    lock (s_lockUiPath)
                    {
                        XmlNodePathRecorder.HandleUiEvent(ref s_strXmlNodes, EnumUiTaskName.LeftDblClick, 0, 0);
                    }
                }
                else
                {
                    // Create a click task
                    lock (s_lockUiPath)
                    {
                        if (!string.IsNullOrEmpty(s_strXmlNodes))
                        {
                            XmlNodePathRecorder.HandleUiEvent(ref s_strXmlNodes, EnumUiTaskName.LeftClick, 0, 0);
                        }
                    }

                }
            }
            else if (mouseState == MouseState.LeftMouseDrag) // drag stop - down and up are at different points
            {
                int dragDeltaX = ptCursorUp.X - ptCursorDown.X;
                int dragDeltaY = ptCursorUp.Y - ptCursorDown.Y;
                lock (s_lockUiPath)
                {
                    XmlNodePathRecorder.HandleUiEvent(ref s_strXmlNodes, EnumUiTaskName.DragStop, dragDeltaX, dragDeltaY);
                }
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
            lock (MouseKeyboardEventHandler.s_lockUiPath)
            {
                if (s_strXmlNodes != null)
                {
                    XmlNodePathRecorder.HandleUiEvent(ref s_strXmlNodes, EnumUiTaskName.RightClick, 0, 0);
                }
            }
        }

        static void PublishKeyboardInput()
        {
            string strBase64 = string.Empty;

            lock (s_lockUiPath)
            {
                if (s_listRecordedKeycode.Count == 0)
                {
                    return;
                }

                strBase64 = Convert.ToBase64String(s_listRecordedKeycode.ToArray());
                s_listRecordedKeycode.Clear();

                if (string.IsNullOrEmpty(strBase64) == false)
                {
                    XmlNodePathRecorder.AddKeyboardInputTask(ref strBase64, s_bCapsLock, s_bNumLock, s_bScrollLock);
                }
            }
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

            lock (s_lockUiPath)
            {
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
            }
        }
    }
}
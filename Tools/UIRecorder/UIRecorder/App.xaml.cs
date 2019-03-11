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
using System.Windows;
using Microsoft.ApplicationInsights;

namespace WinAppDriverUIRecorder
{
    public class AppInsights
    {
        static TelemetryClient tc = new TelemetryClient();

        public static void InitAppSights()
        {
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine;
            Microsoft.Win32.RegistryKey skey = key.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
            string bldStr = skey == null ? Environment.OSVersion.ToString() : $"{skey.GetValue("BuildLab")}";
            //Please follow the instruction on https://docs.microsoft.com/en-us/azure/azure-monitor/app/windows-desktop
            //to get your own Instrumentation Key
            Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Active.InstrumentationKey = "Your Instrumentation Key";

            tc.Context.User.Id = Environment.UserName;
            tc.Context.Session.Id = Guid.NewGuid().ToString();
            tc.Context.Device.OperatingSystem = bldStr;
        }

        public static void Flush()
        {
            if (tc != null)
                tc.Flush();
        }

        public static void LogEvent(string eventName, string eventData)
        {
            if (eventData != null)
            {
                var dict = new Dictionary<string, string>();
                dict.Add("EventData", eventData);
                tc.TrackEvent(eventName, dict);
            }
            else
            {
                tc.TrackEvent(eventName);
            }
        }

        public static void LogEvent(string eventName)
        {
            tc.TrackEvent(eventName);
        }

        public static void LogException(string metricName, string metricData)
        {
            var et = new Microsoft.ApplicationInsights.DataContracts.ExceptionTelemetry();
            et.Message = metricData;
            tc.TrackException(et);
        }

        public static void LogMetric(string metricName, int metricData)
        {
            tc.TrackMetric(new Microsoft.ApplicationInsights.DataContracts.MetricTelemetry(metricName, metricData));
        }
    }

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            NativeMethods.SetProcessDPIAware();
            AppInsights.InitAppSights();
            NativeMethods.InitUiTreeWalk();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            NativeMethods.UnInitUiTreeWalk();
            AppInsights.Flush();
            base.OnExit(e);
            Environment.Exit(0);
        }
    }
}

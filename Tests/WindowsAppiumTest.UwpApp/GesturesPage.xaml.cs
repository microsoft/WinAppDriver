using System;
using Windows.Devices.Input;
using Windows.UI.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WindowsAppiumTest.UwpApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GesturesPage : Page
    {
        bool isPinch;
        double scaleX, scaleY;
        double translateX, translateY;

        int _gestureCount;

        public GesturesPage()
        {
            InitializeComponent();
        }

        void IncrementGestureCount()
        {
            _gestureCount++;
            GestureCount.Text = _gestureCount.ToString();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                await Windows.UI.ViewManagement.StatusBar.GetForCurrentView().HideAsync();
            }
        }

        void OnDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            var position = e.GetPosition(LayoutRoot);
            var action = e.PointerDeviceType == PointerDeviceType.Touch ? "DoubleTap" : "DoubleClick";
            Output.Text = $"{action}:{Math.Round(position.X)},{Math.Round(position.Y)}";
        }

        void ContentRoot_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (e.PointerDeviceType == PointerDeviceType.Touch)
            {
                var position = e.GetPosition(LayoutRoot);
                Output.Text = $"Tap:{Math.Round(position.X)},{Math.Round(position.Y)}";
                IncrementGestureCount();
            }
        }

        void ContentRoot_OnHolding(object sender, HoldingRoutedEventArgs e)
        {
            if (e.HoldingState == HoldingState.Started)
            {
                Output.Text = "Holding";
            }
            else if (e.HoldingState == HoldingState.Canceled)
            {
                Output.Text = "Holding Cancelled";
            }
            else
            {
                var position = e.GetPosition(LayoutRoot);
                Output.Text = $"Hold:{Math.Round(position.X)},{Math.Round(position.Y)}";
                IncrementGestureCount();
            }
        }

        void OnManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            isPinch = false;
            translateX = translateY = 0;
        }

        void OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            var oldIsPinch = isPinch;
            isPinch = e.Delta.Scale != 1.0;

            if (oldIsPinch == false && isPinch == false)
            {
                this.OnDragDelta(sender, e);
            }
            else if (oldIsPinch == false && isPinch == true)
            {
                this.OnPinchStarted(sender, e);
            }
            else if (oldIsPinch == true && isPinch == true)
            {
                this.OnPinchDelta(sender, e);
            }
            else if (oldIsPinch == true && isPinch == false)
            {
                this.OnPinchCompleted(sender, e);
            }
        }

        void OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (e.Cumulative.Rotation != 0 || e.Cumulative.Scale != 1)
            {
                Output.Text =
                    $"{{'Rotation':{e.Cumulative.Rotation},'Scale':{e.Cumulative.Scale}," +
                    $"'Translation':[{e.Cumulative.Translation.X},{e.Cumulative.Translation.Y}]}}";

                IncrementGestureCount();
                return;
            }

            if (Math.Abs(translateX) + Math.Abs(translateY) > 0.1)
            {
                OnFlick(sender, e);
                IncrementGestureCount();
            }
        }

        void OnDragDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            translateX += e.Delta.Translation.X;
            translateY += e.Delta.Translation.Y;
        }

        void OnPinchStarted(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            scaleX = scaleY = 1.0;
        }

        void OnPinchDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            double angleDelta = e.Cumulative.Rotation;

            scaleX = e.Cumulative.Scale;
            scaleY = e.Cumulative.Scale;
            Output.Text = string.Format("Pinch A:{0} X:{1} Y:{2}", angleDelta, scaleX, scaleY);
        }

        void OnPinchCompleted(object sender, ManipulationDeltaRoutedEventArgs e)
        {
        }

        void OnFlick(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            var horizontalVelocity = e.Cumulative.Translation.X;
            var verticalVelocity = e.Cumulative.Translation.Y;

            Output.Text = string.Concat(
                $"Flick Orientation:{GetDirection(horizontalVelocity, verticalVelocity)} ",
                $"Angle:{Math.Round(GetAngle(horizontalVelocity, verticalVelocity))} ",
                $"Inertial:{e.IsInertial} ",
                $"Amount:{translateX},{translateY} ",
                $"Velocity:{horizontalVelocity},{verticalVelocity}");
        }

        Orientation GetDirection(double x, double y)
        {
            return Math.Abs(x) >= Math.Abs(y) ? Orientation.Horizontal : Orientation.Vertical;
        }

        double GetAngle(double x, double y)
        {
            var angle = Math.Atan2(y, x);

            if (angle < 0)
            {
                angle += 2 * Math.PI;
            }

            return angle * 180 / Math.PI;
        }

        private void Back_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        void OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType != PointerDeviceType.Touch)
            {
                var pointerPoint = e.GetCurrentPoint(LayoutRoot);
                var action = "Click";
                if (pointerPoint.Properties.IsRightButtonPressed)
                {
                    action = "RightClick";
                }
                else if (pointerPoint.Properties.IsMiddleButtonPressed)
                {
                    action = "MiddleClick";
                }
                Output.Text = $"{action}:{Math.Round(pointerPoint.Position.X)},{Math.Round(pointerPoint.Position.Y)}";
                IncrementGestureCount();
            }
        }
    }
}

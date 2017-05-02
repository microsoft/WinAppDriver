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

using System;
using Windows.Storage.Streams;
using Windows.UI.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Input
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();

            ConfigureDialRotation();
        }

        private void ConfigureDialRotation()
        {
            var iconRotate = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/Rotate.png"));
            var itemRotate = RadialControllerMenuItem.CreateFromIcon("Rotate", iconRotate);
            var controller = RadialController.CreateForCurrentView();

            controller.Menu.Items.Add(itemRotate);
            controller.RotationChanged += ControllerOnRotationChanged;

            var config = RadialControllerConfiguration.GetForCurrentView();
            config.SetDefaultMenuItems(new RadialControllerSystemMenuItemKind[] { });
        }

        private void Log(string text)
        {
            if (ResultText.Text.Length > 0)
            {
                ResultText.Text += Environment.NewLine;
            }

            ResultText.Text += text;
        }

        private void Touchable_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            Log("Tapped");
        }

        private void Touchable_OnDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            Log("DoubleTapped");
        }

        private void Touchable_OnRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            Log("RightTapped");
        }

        private void Touchable_OnHolding(object sender, HoldingRoutedEventArgs e)
        {
            Log("Holding");
        }

        private void Clear_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            ResultText.Text = string.Empty;
            Touchable.Opacity = 1;
            TouchableTransform.TranslateX = 0;
            TouchableTransform.TranslateY = 0;
            TouchableTransform.Rotation = 0;
            TouchableTransform.ScaleX = 1;
            TouchableTransform.ScaleY = 1;
        }

        private void Touchable_OnManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            Log("ManipulationStarted");
            Touchable.Opacity = 0.5;
        }

        private void Touchable_OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            // Do some math to prevent the Touchable from being moved outside of the designated container.
            var maxDistanceX = (ManipulationContainer.ActualWidth - Touchable.ActualWidth) / 2;
            var maxDistanceY = (ManipulationContainer.ActualHeight - Touchable.ActualHeight) / 2;
            var translationX = TouchableTransform.TranslateX + e.Delta.Translation.X;
            var translationY = TouchableTransform.TranslateY + e.Delta.Translation.Y;

            TouchableTransform.TranslateX = translationX > 0 
                ? Math.Min(maxDistanceX, translationX)
                : Math.Max(maxDistanceX * -1, translationX);
            TouchableTransform.TranslateY = translationY > 0
                ? Math.Min(maxDistanceY, translationY)
                : Math.Max(maxDistanceY * -1, translationY);
            
            // Apply other manipulation deltas
            TouchableTransform.Rotation += e.Delta.Rotation;
            TouchableTransform.ScaleX *= e.Delta.Scale;
            TouchableTransform.ScaleY *= e.Delta.Scale;
        }

        private void ControllerOnRotationChanged(RadialController sender, RadialControllerRotationChangedEventArgs e)
        {
            TouchableTransform.Rotation += e.RotationDeltaInDegrees;
        }

        private void Touchable_OnManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            Log("ManipulationCompleted");
            Touchable.Opacity = 1;
        }
    }
}

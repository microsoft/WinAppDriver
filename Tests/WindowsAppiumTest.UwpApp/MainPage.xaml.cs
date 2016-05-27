using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WindowsAppiumTest.UwpApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        void Controls_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ControlsPage));
        }

        void Gestures_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GesturesPage));
        }

        void TextEntry_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TextEntryPage));
        }
    }
}

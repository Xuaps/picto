using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace PictoUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Gallery : Page
    {
        public Gallery()
        {
            this.InitializeComponent();
        }

        private void Play(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            sentence.PlayAll();
        }
    }
}

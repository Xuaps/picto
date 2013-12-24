using System.Collections.Specialized;
using PictoUI.ViewModels;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace PictoUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PictoGallery : Page
    {
        public PictoGallery()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            (DataContext as PictoGalleryViewModel).Words.CollectionChanged += WordsCollectionChanged;

        }

        void WordsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(e.Action==NotifyCollectionChangedAction.Add)
                sentence.PlayLast();
        }

        private void Play(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            sentence.PlayAll();
        }
    }
}

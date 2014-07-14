using System;
using System.Threading.Tasks;
using PictoUI.ViewModels;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace PictoUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AdminPictos : Page
    {
        public AdminPictos()
        {
            this.InitializeComponent();
        }

        //popup
        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            HidePopup();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            HidePopup();
            (DataContext as AdminPictosViewModel).CategoryImage = null;
            (DataContext as AdminPictosViewModel).CategoryName = null;
            (DataContext as AdminPictosViewModel).CategoryBitmap = null;

            (DataContext as AdminPictosViewModel).PictoImage = null;
            (DataContext as AdminPictosViewModel).PictoName = null;
            (DataContext as AdminPictosViewModel).PictoSound = null;
            (DataContext as AdminPictosViewModel).PictoBitmap = null;
        }

        private void ShowPopup(Popup popup)
        {
            var currentWindow = Window.Current.CoreWindow;
            var content = ((FrameworkElement) popup.Child);

            rectBackgroundHide.Visibility = Windows.UI.Xaml.Visibility.Visible;
            content.Width = currentWindow.Bounds.Width;
            popup.VerticalOffset = (currentWindow.Bounds.Height / 2) - (content.Height / 2);
            popup.IsOpen = true;
        }

        //category
        private void AddCategory(object sender, RoutedEventArgs e)
        {
            ShowPopup(popAddCategory);
        }

        private async void SelectCategoryImage(object sender, RoutedEventArgs e)
        {
            var file = await SelectImage();
            (DataContext as AdminPictosViewModel).CategoryImage = file;
        }

        //picto
        private async void AddPicto(object sender, RoutedEventArgs e)
        {
            ShowPopup(popAddPicto);
        }

        private async void SelectPictoImage(object sender, RoutedEventArgs e)
        {
            var file = await SelectImage();
            (DataContext as AdminPictosViewModel).PictoImage = file;
        }

        private async void SelectPictoSound(object sender, RoutedEventArgs e)
        {
            var file = await SelectPictoSound();
            (DataContext as AdminPictosViewModel).PictoSound = file;
        }

        //commons
        private async Task<StorageFile> SelectPictoSound()
        {
            var openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SettingsIdentifier = "SoundPicker";
            openPicker.SuggestedStartLocation = PickerLocationId.MusicLibrary;
            openPicker.FileTypeFilter.Add(".mp3");
            openPicker.FileTypeFilter.Add(".wma");
            return await openPicker.PickSingleFileAsync();
        }

        private async Task<StorageFile> SelectImage()
        {
            var openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SettingsIdentifier = "ImagePicker";
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
           return await openPicker.PickSingleFileAsync();
        }

        private void HidePopup()
        {
            rectBackgroundHide.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            popAddCategory.IsOpen = false;
            popAddPicto.IsOpen = false;
        }
    }
}

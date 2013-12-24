using System;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace PictoUI.Services
{
    public interface INavigationService
    {
        event NavigatingCancelEventHandler Navigating;
        event NavigatedEventHandler Navigated;

        void InitializeFrame(Frame frame);
        void Navigate(Type source, object parameter = null);
        bool CanGoBack { get; }
        Type CurrentSourcePageType { get; }
        void GoBack();
        ICommand GoBackCommand { get; }
    }
}
using System;
using System.Windows.Input;
using PictoUI.Common;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace PictoUI.Services
{
    public class NavigationService : INavigationService
    {
        public event NavigatingCancelEventHandler Navigating = delegate { };
        public event NavigatedEventHandler Navigated = delegate { };
        private Frame _frame;

        public NavigationService()
        {
            GoBackCommand = new DelegateCommand(GoBack, () => CanGoBack);
        }

        public ICommand GoBackCommand { get; set; }

        public bool CanGoBack
        {
            get { return _frame.CanGoBack; }
        }

        public Type CurrentSourcePageType { get {return _frame.CurrentSourcePageType; } }

        public void InitializeFrame(Frame frame)
        {
            if (_frame != null)
            {
                _frame.Navigating -= Frame_Navigating;
                _frame.Navigated -= _frame_Navigated;
            }

            _frame = frame;
            _frame.Navigating += Frame_Navigating;
            _frame.Navigated += _frame_Navigated;
        }

        void _frame_Navigated(object sender, NavigationEventArgs e)
        {
            Navigated(sender, e);
        }

        void Frame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            
            Navigating(sender, e);
        }

        public void Navigate(Type source, object parameter = null)
        {
            if (_frame == null)
            {
                throw new InvalidOperationException("Frame has not been initialized.");
            }
            _frame.Navigate(source, parameter);
            ((DelegateCommandBase)GoBackCommand).RaiseCanExecuteChanged();
        }

        public void GoBack()
        {
            if (CanGoBack)
            {
                _frame.GoBack();
                ((DelegateCommandBase)GoBackCommand).RaiseCanExecuteChanged();
            }
        }
    }
}

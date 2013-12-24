using System.Collections.ObjectModel;
using System.Windows.Input;
using PictoUI.Common;
using PictoUI.Model;
using PictoUI.Services;

namespace PictoUI.ViewModels
{
    public class ShellViewModel:ViewModel
    {
        private INavigationService _navigationService;

        public ShellViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        
        public ICommand NavigateAdminPanel
        {
            get
            {
                return new DelegateCommand(() => _navigationService.Navigate(typeof (AdminPictos)));
            }
        }

        public ICommand NavigateHome
        {
            get
            {
                return new DelegateCommand(() =>
                                               {
                                                   if (_navigationService.CurrentSourcePageType == typeof(AdminPictos))
                                                       _navigationService.Navigate(typeof (Gallery));
                                               });
            }
        }
    }
}
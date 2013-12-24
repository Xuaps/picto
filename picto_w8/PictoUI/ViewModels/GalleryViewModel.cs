using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using PictoUI.Common;
using PictoUI.Dto;
using PictoUI.Model;
using PictoUI.Services;

namespace PictoUI.ViewModels
{
    public class GalleryViewModel : ViewModel
    {
        private INavigationService _navigationService;
        private IPictos _pictosCollection;

        public IEnumerable<Picto> Categories { get; private set; }
        public ObservableCollection<Picto> Words { get; set; }

        public GalleryViewModel(INavigationService navigationService, IPictos pictosCollection)
        {
            _navigationService = navigationService;
            _navigationService.Navigated += NavigationServiceNavigated;
            _pictosCollection = pictosCollection;

            LoadCategories();
        }

        void NavigationServiceNavigated(object sender, Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            var data = e.Parameter as HomeDto;
            if(data==null)
            {
                Words=new ObservableCollection<Picto>();
                return;
            }

            Words = data.Words;
        }

        async void LoadCategories()
        {
            var categories = await _pictosCollection.GetCategories();
            Categories = new ObservableCollection<Picto>(categories);
            OnPropertyChanged("Categories");
        }

        //Commands
        public ICommand NavigateToPictos
        {
            get
            {
                return
                    new DelegateCommand<string>(
                        (name) =>
                        _navigationService.Navigate(typeof (PictoGallery), new HomeDto{Category = name, Words = this.Words}));
            }
        }

        public ICommand DeleteLast
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    if (Words.Count > 0)
                        Words.RemoveAt(Words.Count - 1);
                });
            }
        }

        public ICommand DeleteAll
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    while (Words.Count > 0)
                        Words.Remove(Words[0]);
                });
            }
        }
    }
}   

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using PictoUI.Common;
using PictoUI.Dto;
using PictoUI.Model;
using PictoUI.Services;
using Windows.UI.Xaml.Navigation;

namespace PictoUI.ViewModels
{
    public class PictoGalleryViewModel:ViewModel
    {
        private readonly INavigationService _navigationService;
        private readonly IPictos _pictosCollection;

        public ObservableCollection<Picto> Words { get; set; }
        public IEnumerable<Picto> Pictos { get; private set; }
        public string CategoryName { get; set; }

        public PictoGalleryViewModel(INavigationService navigationService, IPictos pictosCollection)
        {
            _navigationService = navigationService;
            _navigationService.Navigated += NavigationServiceNavigated;
            _pictosCollection = pictosCollection;

            SelectPicto=new DelegateCommand<Picto>(AddWordToSentence);
        }

        public void AddWordToSentence(Picto selectedWord)
        {
            Words.Add(selectedWord);
        }

        private async void LoadPictos()
        {
            var category = await _pictosCollection.GetCategory(CategoryName);
            Pictos = category.Children;
            OnPropertyChanged("Pictos");
        }

        void NavigationServiceNavigated(object sender, NavigationEventArgs e)
        {
            var data = e.Parameter as HomeDto;
            if (data==null)
                return;

            CategoryName = data.Category;
            if(CategoryName!=null)
                LoadPictos();
            Words = data.Words;
        }

        //Commmands
        public ICommand SelectPicto { get; private set; }

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

        public  ICommand GoBack
        {
            get
            {
                return new DelegateCommand(()=> _navigationService.Navigate(typeof(Gallery), new HomeDto{Words = this.Words}));
            }
        }
    }
}

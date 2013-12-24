using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using PictoUI.Common;
using PictoUI.Model;
using PictoUI.Services;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace PictoUI.ViewModels
{
    public class AdminPictosViewModel:ViewModel
    {
        private readonly IPictos _pictosCollection;
        private Picto _selectedCategory;
        private Picto _selectedPicto;
        private string _categoryName;
        private StorageFile _categoryImage;
        private StorageFile _pictoSound;
        private StorageFile _pictoImage;
        private string _pictoName;
        private ICollection<Picto> _categories;
        private ResourceLoader _resourceLoader;
        private BitmapImage _pictoBitmap;

        public AdminPictosViewModel(IPictos pictosCollection)
        {
            _resourceLoader = new ResourceLoader();
            _pictosCollection = pictosCollection;
            LoadCollections();
            this.PropertyChanged += AdminPictosViewModel_PropertyChanged;
        }

        void AdminPictosViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if ((e.PropertyName == "CategoryName" || e.PropertyName == "CategoryImage") && e.PropertyName != "ErrorCategory" && e.PropertyName != "IsValidCategory")
            {

                this.OnPropertyChanged("ErrorCategory");
                this.OnPropertyChanged("IsValidCategory");
            }
            if ((e.PropertyName == "PictoName" || e.PropertyName != "PictoImage" || e.PropertyName == "PictoSound") && e.PropertyName != "ErrorPicto" && e.PropertyName != "IsValidPicto")
            {
                this.OnPropertyChanged("ErrorPicto");
                this.OnPropertyChanged("IsValidPicto");
                
            }
        }

        //category
        public string ErrorCategory
        {
            get
            {
                string error = string.Empty;

                string property = ValidateCategory("CategoryNameRequired");
                if (!string.IsNullOrEmpty(property))
                {
                    error += property + Environment.NewLine;
                }

                property = ValidateCategory("CategoryNameValid");
                if (!string.IsNullOrEmpty(property))
                {
                    error += property + Environment.NewLine;
                }

                property = ValidateCategory("CategoryNameUnique");
                if (!string.IsNullOrEmpty(property))
                {
                    error += property + Environment.NewLine;
                }

                property = ValidateCategory("CategoryImage");
                if (!string.IsNullOrEmpty(property))
                {
                    error += property + Environment.NewLine;
                }
                
                return error.Trim();
            }
        }

        public string ValidateCategory(string columnName)
        {
            switch (columnName)
            {
                case "CategoryNameRequired":
                    return string.IsNullOrEmpty(CategoryName) ?_resourceLoader.NameRequired : "";
                case "CategoryNameValid":
                    return CategoryName==null || new Regex(@"[\w -.]*").Match(CategoryName).Length==CategoryName.Length ? "" : _resourceLoader.InvalidName;
                case "CategoryNameUnique":
                    {
                        return  _pictosCollection.IsUnique(CategoryName)? "" : _resourceLoader.UniqueName;
                    }

                case "CategoryImage":
                    return CategoryImage==null ? _resourceLoader.ImageRequired : "";
                    
                default:
                    return "";
            }
        }

        public bool IsValidCategory
        {
            get
            {
                return string.IsNullOrEmpty(this.ErrorCategory);
            }
        }

        public string CategoryName
        {
            get { return _categoryName; }
            set { 
                _categoryName = value; 
                OnPropertyChanged("CategoryName");
            }
        }

        public StorageFile CategoryImage
        {
            get { return _categoryImage; }
            set
            {
                _categoryImage = value;
                ConvertToBitmapCategory(value);
                OnPropertyChanged("CategoryImage");
            }
        }

        private async Task ConvertToBitmapCategory(StorageFile sfile)
        {
            var stream = await sfile.OpenReadAsync();
            var bitmap= new BitmapImage();
            bitmap.SetSource(stream);
            CategoryBitmap = bitmap;
        }

        public BitmapImage CategoryBitmap
        {
            get { return _pictoBitmap; }
            set { 
                _pictoBitmap = value; 
                OnPropertyChanged("CategoryBitmap");
            }
        }

        public ICommand AddCategory
        {
            get { return new AsyncDelegateCommand(AddCategoryAsync); }
        }
        public ICommand DeleteCategory { get{return new AsyncDelegateCommand(DeleteCategoryAsync);} }

        private async Task DeleteCategoryAsync()
        {
            await _pictosCollection.DeleteCategory(SelectedCategory);
        }

        async Task AddCategoryAsync()
        {
            var picto = await _pictosCollection.SavePicto(null, CategoryName, CategoryImage,null);
            SelectedCategory = picto;

            CategoryName = "";
            CategoryImage = null;
            CategoryBitmap = null;
        }

        //picto
        public string ErrorPicto
        {
            get
            {
                string error = string.Empty;

                string property = ValidatePicto("PictoNameRequired");
                if (!string.IsNullOrEmpty(property))
                {
                    error += property + Environment.NewLine;
                }

                property = ValidatePicto("PictoNameValid");
                if (!string.IsNullOrEmpty(property))
                {
                    error += property + Environment.NewLine;
                }

                property = ValidatePicto("PictoNameUnique");
                if (!string.IsNullOrEmpty(property))
                {
                    error += property + Environment.NewLine;
                }

                property = ValidatePicto("PictoImage");
                if (!string.IsNullOrEmpty(property))
                {
                    error += property + Environment.NewLine;
                }

                property = ValidatePicto("PictoSound");
                if (!string.IsNullOrEmpty(property))
                {
                    error += property + Environment.NewLine;
                }

                return error.Trim();
            }
        }

        public string ValidatePicto(string columnName)
        {
            switch (columnName)
            {
                case "PictoNameRequired":
                    return string.IsNullOrEmpty(PictoName) ? _resourceLoader.NameRequired : "";
                case "PictoNameValid":
                    return PictoName == null || new Regex(@"[\w -.]*").Match(PictoName).Length == PictoName.Length ? "" : _resourceLoader.InvalidName;
                case "PictoNameUnique":
                    {
                        return _pictosCollection.IsUnique(CategoryName, PictoName) ? "" : _resourceLoader.UniqueName;
                    }

                case "PictoImage":
                    return PictoImage == null ? _resourceLoader.ImageRequired : "";

                case "PictoSound":
                    return PictoSound == null ? _resourceLoader.SoundRequired : "";

                default:
                    return "";
            }
        }

        public bool IsValidPicto
        {
            get
            {
                return string.IsNullOrEmpty(this.ErrorPicto);
            }
        }

        public StorageFile PictoSound
        {
            get { return _pictoSound; }
            set
            {
                _pictoSound = value;
                OnPropertyChanged("PictoSound");
            }
        }

        public StorageFile PictoImage
        {
            get { return _pictoImage; }
            set
            {
                _pictoImage = value;
                ConvertToBitmapPicto(value);
                OnPropertyChanged("PictoImage");
            }
        }

        private async Task ConvertToBitmapPicto(StorageFile sfile)
        {
            var stream = await sfile.OpenReadAsync();
            var bitmap = new BitmapImage();
            bitmap.SetSource(stream);
            PictoBitmap = bitmap;
        }

        public BitmapImage PictoBitmap
        {
            get { return _pictoBitmap; }
            set { 
                _pictoBitmap = value; 
                OnPropertyChanged("PictoBitmap");
            }
        }

        public string PictoName
        {
            get { return _pictoName; }
            set 
            { 
                _pictoName = value;
                OnPropertyChanged("PictoName");
            }
        }

        public ICommand AddPicto { get { return new AsyncDelegateCommand(AddPictoAsync); } }
        public ICommand DeletePicto { get { return new AsyncDelegateCommand(DeletePictoAsync); } }

        private async Task DeletePictoAsync()
        {
            await _pictosCollection.DeletePicto(SelectedCategory, SelectedPicto);
        }

        public async Task AddPictoAsync()
        {
            var picto = await _pictosCollection.SavePicto(SelectedCategory, PictoName,PictoImage, PictoSound);
            SelectedPicto = picto;

            PictoName = "";
            PictoImage = null;
            PictoSound = null;
            PictoBitmap = null;
        }
        //admin
        public ICollection<Picto> Categories
        {
            get { return _categories; }
            private set
            {
                _categories = value;
                OnPropertyChanged("Categories");
            }
        }

        public object IsAppBarOpen
        {
            get { return SelectedCategory!=null; }
        }

        public Picto SelectedPicto
        {
            get { return _selectedPicto; }
            set { _selectedPicto = value;
            OnPropertyChanged("IsAppBarOpen");
            OnPropertyChanged("SelectedPicto");
            }
        }

        public Picto SelectedCategory
        {
            get { return _selectedCategory; }
            set { 
                _selectedCategory = value;
                OnPropertyChanged("IsAppBarOpen");
                OnPropertyChanged("SelectedCategory");
            }
        }

        private async void LoadCollections()
        {
            Categories = await _pictosCollection.GetCategories();
        }
    }
}

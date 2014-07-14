using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroIoc;
using PictoUI.Messaging;
using PictoUI.Services;
using PictoUI.ViewModels;

namespace PictoUI.Common
{
    public class ViewModelLocator
    {
        private Lazy<IContainer> _container;
        public IContainer Container { get { return _container.Value; } }

        public ViewModelLocator()
        {
            _container = new Lazy<IContainer>(IoC.BuildContainer);
        }

        public GalleryViewModel GalleryViewModel
        {
            get { return Container.Resolve<GalleryViewModel>(); }
        }

        public ShellViewModel ShellViewModel
        {
            get
            {
                return Container.Resolve<ShellViewModel>();
            }
        }

        public IDialogService DialogService
        {
            get
            {
                return Container.Resolve<IDialogService>();
            }
        }

        public object PictoGalleryViewModel
        {
            get { return Container.Resolve<PictoGalleryViewModel>(); }
        }

        public INavigationService NavigationService
        {
            get { return Container.Resolve<INavigationService>(); }
        }

        public SuggestionsViewModel SuggestionsViewModel    
        {
            get { return Container.Resolve<SuggestionsViewModel>(); }
        }

        public AdminPictosViewModel AdminPictosViewModel
        {
            get { return Container.Resolve<AdminPictosViewModel>(); }
        }
    }
}

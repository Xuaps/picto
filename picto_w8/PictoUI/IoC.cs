using MetroIoc;
using PictoUI.Messaging;
using PictoUI.Model;
using PictoUI.Services;
using PictoUI.ViewModels;

namespace PictoUI
{
    public static class IoC
    {
        public static IContainer BuildContainer()
        {
            var container = new MetroContainer();
            container.RegisterInstance(container);
            container.RegisterInstance<IContainer>(container);

            container.Register<INavigationService, NavigationService>(lifecycle: new SingletonLifecycle());
            container.Register<IHub, MessageHub>(lifecycle: new SingletonLifecycle());
            container.Register<IDialogService, DialogService>();

            container.Register<IPictos, Pictos>();

            RegisterHandlers(container);

            container.Register<ShellViewModel>(lifecycle: new SingletonLifecycle());
            // I would prefer not to immediately resolve a singleton, but the MetroContainer doesn't support resolving from Method or provider alternatives.
            //container.RegisterInstance<IStatusService>(container.Resolve<ShellViewModel>());

            return container;
        }

        private static void RegisterHandlers(IContainer container)
        {
            // todo: Add Assembly Scanning here to auto-register

            /*
           container.Register<IHandler<ShareImageResultsMessage>, ShareImageResultsHandler>();
           container.Register<IHandler<ShareUriMessage>, ShareUriHandler>();

           container.Register<IHandler<ShowShareUiMessage>, ShowShareUiHandler>();
           container.Register<IHandler<UpdateTileMessage>, UpdateTileHandler>();
           container.Register<IHandler<UpdateTileImageCollectionMessage>, UpdateTileImageCollectionHandler>();
           container.Register<IHandler<ShowSettingsMessage>, ShowSettingsHandler>();
           container.Register<IAsyncHandler<SearchQuerySubmittedMessage>, SearchQuerySubmittedHandler>();
           container.Register<IAsyncHandler<SetLockScreenMessage>, SetLockScreenHandler>();
           container.Register<IAsyncHandler<SaveImageMessage>, SaveImageHandler>();
            */
        }
    }
}

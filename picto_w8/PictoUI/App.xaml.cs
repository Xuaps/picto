using System;
using System.Threading.Tasks;
using BugSense;
using BugSense.Core.Model;
using BugSense.Model;
using PictoUI.Common;
using PictoUI.Messaging;
using PictoUI.Model;
using PictoUI.Services;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;

// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace PictoUI
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        private Shell shell;
        public static CoreDispatcher Dispatcher { get; set; }

        public static ViewModelLocator ViewModelLocator
        {
            get { return (ViewModelLocator)Current.Resources["ViewModelLocator"]; }
        }

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            BugSenseHandler.Instance.InitAndStartSession(new ExceptionManager(Current), "w8c4a705");
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            //UnhandledException += OnUnhandledException;
        }

        public async void OnUnhandledException(Exception exception)
        {
            var dispatcher = App.Dispatcher;
            BugSenseHandler.Instance.LogException(exception, "Picto", exception.Message);
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, (DispatchedHandler)(async () =>
            {
#if DEBUG
                await ViewModelLocator.DialogService.ShowMessageAsync(exception.Message);
#else
                await ViewModelLocator.DialogService.ShowResourceMessageAsync("Exception_UnhandledException");
                Exit();
#endif
            }));
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            try
            {
                EnsureShell(args.PreviousExecutionState);
                var pictos = ViewModelLocator.Container.Resolve<IPictos>();
                await new InitialDataService(pictos).LoadInitialData();

                ViewModelLocator.NavigationService.Navigate(typeof(Gallery));
                shell.ProgressBar.Visibility=Visibility.Collapsed;
                //
            }
            catch (Exception ex)
            {
                // Due to an issues I've noted online: http://social.msdn.microsoft.com/Forums/en/winappswithcsharp/thread/bea154b0-08b0-4fdc-be31-058d9f5d1c4e
                // I am limiting the use of 'async void'  In a few rare occasions I use it
                // and manually route the exceptions to the UnhandledExceptionHandler
                ((App)App.Current).OnUnhandledException(ex);
            }
        }

        private  void EnsureShell(ApplicationExecutionState previousState)
        {
            if (previousState == ApplicationExecutionState.Running)
            {
                Window.Current.Activate();
                ViewModelLocator.NavigationService.InitializeFrame(shell.Frame);
                return;
            }

            /*
            if (previousState == ApplicationExecutionState.Terminated || previousState == ApplicationExecutionState.ClosedByUser)
            {
                var settings = ViewModelLocator.Container.Resolve<ApplicationSettings>();
                await settings.RestoreAsync();
            }
            */

            shell = new Shell();
            ViewModelLocator.NavigationService.InitializeFrame(shell.MainFrame);
            Window.Current.Content = shell;
            Window.Current.Activate();
            Dispatcher = Window.Current.CoreWindow.Dispatcher;

            /*
            MessagePumps.StartAll();

            ViewModelLocator.ShellViewModel.IsLoading = false;
             */
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}

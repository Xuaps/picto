using System;
using System.Collections.Specialized;
using PictoUI.Common;
using PictoUI.ViewModels;
using Windows.System;
using Windows.UI.ApplicationSettings;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace PictoUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Shell : Page
    {
        public Frame MainFrame
        {
            get { return Body; }
        }

        public ProgressBar ProgressBar
        {
            get { return progressBar; }
        }

        public Shell()
        {
            this.InitializeComponent();
            Loaded += Shell_Loaded;
        }

        void Shell_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            RegisterSettings();
            SizeChanged += Shell_SizeChanged;
        }

        void Shell_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            VisualStateManager.GoToState(this, ApplicationView.Value.ToString(), true);
            if(Body.Content is AdminPictos)
                Body.Navigate(typeof (Gallery));
        }

        private void RegisterSettings()
        {
            var resourceLoader = new ResourceLoader();
            SettingsPane.GetForCurrentView().CommandsRequested += (s, e) =>
            {
                // Política de privacidad
                SettingsCommand privacyPolicyCommand =
                    new SettingsCommand("PrivacityPolicy", resourceLoader.PrivacityPolicyTittle, async (h)=>
                    {
                        await Launcher.LaunchUriAsync(resourceLoader.PrivacityPolicyUri);                                                   
                    });
                e.Request.ApplicationCommands.Add(privacyPolicyCommand);

                // Contacta con nosotros
                SettingsCommand contactUsCommand =
                    new SettingsCommand("ContactUs", resourceLoader.ContactUsTittle, async (h) =>
                    {
                        await Launcher.LaunchUriAsync(resourceLoader.ContactUsUri);
                    });
                e.Request.ApplicationCommands.Add(contactUsCommand);
            /*

                var settingsCommand = new SettingsCommand("Preferences", "Preferences", (h) =>
                {
                    new MessageDialog("juas").ShowAsync();
                });

                e.Request.ApplicationCommands.Add(settingsCommand);
             */
            };
        }
  
        private void ApplyAdminStyles(object sender, RoutedEventArgs e)
        {
            HideAppBar();
        }
        
        private void ApplyHomeStyles(object sender, RoutedEventArgs e)
        {
            HideAppBar();
        }

        private void HideAppBar()
        {
            TopAppBar.IsOpen = false;
        }
    }
}

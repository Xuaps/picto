using System;
using System.Collections.Specialized;
using Windows.UI;
using Windows.UI.Xaml.Media;
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
                // Add an About command
                var about = new SettingsCommand("about", "About", (handler) =>
                {
                    var settings = new SettingsFlyout
                    {
                        Content = new About(),
                        HeaderBackground = new SolidColorBrush(Color.FromArgb(255,0,95,51)),
                        Background = new SolidColorBrush(Color.FromArgb(255, 255,255,255)),
                        Title = "About",
                    };

                    settings.Show();
                });
                e.Request.ApplicationCommands.Add(about);

                // Política de privacidad
                var privacyPolicyCommand =
                    new SettingsCommand("PrivacityPolicy", resourceLoader.PrivacityPolicyTittle, async (h)=>
                    {
                        await Launcher.LaunchUriAsync(resourceLoader.PrivacityPolicyUri);                                                   
                    });
                e.Request.ApplicationCommands.Add(privacyPolicyCommand);

                // Contacta con nosotros
                var contactUsCommand =
                    new SettingsCommand("ContactUs", resourceLoader.ContactUsTittle, async (h) =>
                    {
                        await Launcher.LaunchUriAsync(resourceLoader.ContactUsUri);
                    });
                e.Request.ApplicationCommands.Add(contactUsCommand);
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

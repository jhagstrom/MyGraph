using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;
using Windows.UI;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MyGraph
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string mailAddress;
        private string displayName = null;
        private TaskHelper taskHelper = new TaskHelper();
        public static ApplicationDataContainer settings = ApplicationData.Current.RoamingSettings;

        public MainPage()
        {
            this.InitializeComponent();
            var white = new SolidColorBrush(Colors.White);
            HamburgerButton.Foreground = white;
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            progressRing.Visibility = Visibility.Visible;
            if (await SignInCurrentUserAsync())
            {
                taskView.ItemsSource = await taskHelper.GetMyTasks();
            }
            else
            {
                //InfoText.Text = "nope";// ResourceLoader.GetForCurrentView().GetString("AuthenticationErrorMessage");
            }

            progressRing.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Signs in the current user.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SignInCurrentUserAsync()
        {
            var token = await AuthenticationHelper.GetTokenForUserAsync();

            if (token != null)
            {
                string userId = (string)settings.Values["userID"];
                mailAddress = (string)settings.Values["userEmail"];
                displayName = (string)settings.Values["userName"];
                return true;
            }
            else
            {
                return false;
            }

        }


        //private async void MailButton_Click(object sender, RoutedEventArgs e)
        //{
        //    _mailAddress = EmailAddressBox.Text;
        //    ProgressBar.Visibility = Visibility.Visible;
        //    MailStatus.Text = string.Empty;
        //    try
        //    {
        //        await _mailHelper.ComposeAndSendMailAsync("Sending out testmessage from Graph UWP App", ComposePersonalizedMail(_displayName), _mailAddress);
        //        MailStatus.Text = string.Format("Message sent!", _mailAddress);
        //    }
        //    catch (Exception exception)
        //    {
        //        MailStatus.Text = exception.Message;// ResourceLoader.GetForCurrentView().GetString("MailErrorMessage");
        //    }
        //    finally
        //    {
        //        ProgressBar.Visibility = Visibility.Collapsed;
        //    }

        //}

        // <summary>
        // Personalizes the email.
        // </summary>
        //public static string ComposePersonalizedMail(string userName)
        //{
        //    return String.Format("This mail was sent to you from an app", userName);
        //}

        //private void Disconnect_Click(object sender, RoutedEventArgs e)
        //{
        //    ProgressBar.Visibility = Visibility.Visible;
        //    AuthenticationHelper.SignOut();
        //    ProgressBar.Visibility = Visibility.Collapsed;
        //    MailButton.IsEnabled = false;
        //    EmailAddressBox.IsEnabled = false;
        //    InfoText.Text = "Connect"; // ResourceLoader.GetForCurrentView().GetString("ConnectPrompt");
        //    this._displayName = null;
        //    this._mailAddress = null;
        //}

        private void HamburgerButton_Click(object sender, RoutedEventArgs e) { MySplitView.IsPaneOpen = !MySplitView.IsPaneOpen; }
    }
}

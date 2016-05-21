using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WoChat.Models;
using WoChat.Storage;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WoChat.Views {
    public sealed partial class SettingsPage : Page {
        private SettingsPageUIViewModel UIVM = new SettingsPageUIViewModel();

        public SettingsPage() {
            InitializeComponent();
            SystemNavigationManager.GetForCurrentView().BackRequested += MyProfilePage_BackRequested;
        }

        private void MyProfilePage_BackRequested(object sender, BackRequestedEventArgs e) {
            var rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null && rootFrame.CanGoBack && e.Handled == false) {
                rootFrame.GoBack();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }
    }

    class SettingsPageUIViewModel : NotifyPropertyChangedBase {
        public int SendMessageKey {
            get {
                return SettingsHelper.LoadInt("send_message_key", 0);
            }
            set {
                SettingsHelper.Save("send_message_key", value);
                OnPropertyChanged();
            }
        }
    }
}

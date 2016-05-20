using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WoChat.Models;
using WoChat.ViewModels;

namespace WoChat.Views {
    public sealed partial class FriendsPage : Page {
        public ContactViewModel ContactVM {
            get {
                return contactVM;
            }
        }

        public FriendsPage() {
            InitializeComponent();
        }

        private ContactViewModel contactVM = App.AppVM.ContactVM;

        private void StartChatButton_Click(object sender, RoutedEventArgs e) {
            var rootFrame = Window.Current.Content as Frame;
            var mainPage = rootFrame.Content as MainPage;
            mainPage.StartChat((FriendsListView.SelectedItem as ContactModel).UserId, 0);
        }
    }
}

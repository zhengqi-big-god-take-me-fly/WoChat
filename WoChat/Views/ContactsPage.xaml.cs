using Windows.UI.Xaml.Controls;

namespace WoChat.Views {
    public sealed partial class ContactsPage : Page {
        public ContactsPage() {
            InitializeComponent();

            FriendsFrame.Navigate(typeof(FriendsPage));
            GroupsFrame.Navigate(typeof(GroupsPage));
            SystemsFrame.Navigate(typeof(SystemsPage));
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (e.AddedItems.Contains(ContactsPivot.Items[0])) {
                App.AppVM.ContactVM.Sync();
            }
        }
    }
}

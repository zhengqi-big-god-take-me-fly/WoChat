using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace WoChat.Views {
    public sealed partial class MainPage : Page {
        private MainPageUIViewModel MainPageUIVM = new MainPageUIViewModel();

        public MainPage() {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs nea) {
            MainPageUIVM.SelectedMenuItem = MainPageUIVM.MenuItems.Count > 0 ? MainPageUIVM.MenuItems[0] : null;
        }

        private void MainPaneMenuButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e) {
            MainSplitView.IsPaneOpen = !MainSplitView.IsPaneOpen;
        }

        private void MainPaneMenu_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            int selectedIndex = MainPageUIVM.MenuItems.IndexOf(MainPageUIVM.SelectedMenuItem);
            switch (selectedIndex) {
                case 0:
                    MainFrame.Navigate(typeof(ChatPage));
                    break;
                case 1:
                    MainFrame.Navigate(typeof(ContactsPage));
                    break;
            }
        }
    }

    public class MainPageUIViewModel {
        public MainPageUIViewModel() {
            // Create menu items
            MenuItems.Add(new MenuItem("", "Chat"));
            MenuItems.Add(new MenuItem("", "Contacts"));
        }
        public ObservableCollection<MenuItem> MenuItems {
            get {
                return menuItems;
            }
        }
        public MenuItem SelectedMenuItem {
            get {
                return selectedMenuItem;
            }
            set {
                selectedMenuItem = value;
            }
        }

        private ObservableCollection<MenuItem> menuItems = new ObservableCollection<MenuItem>();
        private MenuItem selectedMenuItem;
    }

    public class MenuItem : INotifyPropertyChanged {
        public MenuItem(string _icon = "", string _title = "") {
            Icon = _icon;
            Title = _title;
        }

        public string Icon {
            get {
                return icon;
            }
            set {
                icon = value;
                OnPropertyChanged();
            }
        }
        public string Title {
            get {
                return title;
            }
            set {
                title = value;
                OnPropertyChanged();
            }
        }

        private string icon;
        private string title;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

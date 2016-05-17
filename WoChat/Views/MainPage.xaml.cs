using Newtonsoft.Json;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WoChat.Commons.Utils;

namespace WoChat.Views {
    public sealed partial class MainPage : Page {
        private MainPageUIViewModel MainPageUIVM = new MainPageUIViewModel();
        private ChatSimpleInfo startChatParams = null;

        public MainPage() {
            InitializeComponent();
        }

        public void StartChat(string id, int type) {
            startChatParams = new ChatSimpleInfo();
            startChatParams.id = id;
            startChatParams.type = type;
            SplitViewMenu.SelectedIndex = 0;
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
                    MainFrame.Navigate(typeof(ChatPage), startChatParams != null ? JsonConvert.SerializeObject(startChatParams) : null);
                    startChatParams = null;
                    break;
                case 1:
                    MainFrame.Navigate(typeof(ContactsPage));
                    break;
                default:
                    break;
            }
            MainSplitView.IsPaneOpen = false;
        }
    }

    public class ChatSimpleInfo {
        public string id;
        public int type;
    }

    class MainPageUIViewModel {
        public MainPageUIViewModel() {
            // Create menu items
            MenuItems.Add(new MenuItem("", "聊天"));
            MenuItems.Add(new MenuItem("", "联系人"));
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
}

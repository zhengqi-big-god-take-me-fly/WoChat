using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WoChat.Utils;
using WoChat.Net;
using WoChat.ViewModels;

namespace WoChat.Views {
    public sealed partial class MainPage : Page {
        private MainPageUIViewModel MainPageUIVM = new MainPageUIViewModel();
        private LocalUserViewModel LocalUserVM = App.AppVM.LocalUserVM;
        private ChatSimpleInfo startChatParams = null;

        public MainPage() {
            InitializeComponent();
            App.PushSocket.OnMessageArrive += PushSocket_OnMessageArrive;
        }

        private async void PushSocket_OnMessageArrive(object sender, MessageArriveEventArgs e) {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                List<PushMessage> ms = e.Messages;
                MainPageUIVM.MenuItems[0].Unread += ms.Count;
                if (MainPageUIVM.SelectedMenuItem == MainPageUIVM.MenuItems[0]) {
                    MainPageUIVM.MenuItems[0].Unread = 0;
                }
                MainPageUIVM.MenuItems[0].IndicatorVisibility = MainPageUIVM.MenuItems[0].Unread > 0 ? Visibility.Visible : Visibility.Collapsed;
                // TODO: Send message receipt
            });
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

        private void MainPaneMenuButton_Click(object sender, RoutedEventArgs e) {
            MainSplitView.IsPaneOpen = !MainSplitView.IsPaneOpen;
        }

        private void MainPaneMenu_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            int selectedIndex = MainPageUIVM.MenuItems.IndexOf(MainPageUIVM.SelectedMenuItem);
            switch (selectedIndex) {
                case 0:
                    MainFrame.Navigate(typeof(ChatPage), startChatParams != null ? JsonConvert.SerializeObject(startChatParams) : null);
                    startChatParams = null;
                    MainPageUIVM.MenuItems[0].Unread = 0;
                    break;
                case 1:
                    MainFrame.Navigate(typeof(ContactsPage));
                    break;
                default:
                    break;
            }
            MainSplitView.IsPaneOpen = false;
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e) {
            App.AppVM.LocalUserVM.UserLogOut();
            App.PushSocket.Logout();
            var rootFrame = Window.Current.Content as Frame;
            rootFrame.Navigate(typeof(RegLogPage));
        }

        private void UserButton_Click(object sender, RoutedEventArgs e) {
            // TODO
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e) {
            // TODO
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

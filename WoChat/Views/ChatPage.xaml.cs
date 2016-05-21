using Newtonsoft.Json;
using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WoChat.Models;
using WoChat.Net;
using WoChat.ViewModels;

namespace WoChat.Views {
    public sealed partial class ChatPage : Page {
        private ChatViewModel ChatVM = App.AppVM.ChatVM;
        private UIViewModel UIVM = new UIViewModel();
        
        public ChatPage() {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            if (e.Parameter != null) {
                ChatSimpleInfo info = JsonConvert.DeserializeObject<ChatSimpleInfo>(e.Parameter as string);
                switch (info.type) {
                    case 0:
                        int s = -1;
                        for (int i = 0; i < ChatVM.Chats.Count; ++i) {
                            if (ChatVM.Chats[i].ReceiverId == info.id) {
                                s = i;
                                break;
                            }
                        }
                        if (s == -1) {
                            ChatVM.Chats.Insert(0, new ChatModel(info.id, 0, App.AppVM.ContactVM.FindUser(info.id).Nickname));
                            s = 0;
                        }
                        UIVM.SelectedIndex = s;
                        break;
                    default:
                        break;
                }
            }
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e) {
            if (SendTextBox.Text.Equals("")) return;
            var chat = ChatList.SelectedItem as ChatModel;
            switch (chat.Type) {
                case ChatModel.ChatType.User:
                    var result = await HTTP.PostUsers_Message(App.AppVM.LocalUserVM.JWT, App.AppVM.ContactVM.FindUser(chat.ReceiverId).Username, 0, SendTextBox.Text);
                    if (result.StatusCode == PostUsers_MessageResult.PostUsers_MessageStatusCode.Success) {
                        SendTextBox.Text = "";
                        chat.MessageList.Add(new MessageModel(result.content, result.time, 0, result.receiver, result.sender, result.sender));
                    }
                    break;
            }
        }
    }

    class UIViewModel : NotifyPropertyChangedBase {
        public int SelectedIndex {
            get {
                return selectedIndex;
            }
            set {
                selectedIndex = value;
                OnPropertyChanged();
            }
        }

        private int selectedIndex = -1;
    }
}

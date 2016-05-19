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
        
        public ChatPage() {
            InitializeComponent();
            App.PushSocket.OnMessageArrive += PushSocket_OnMessageArrive;
        }

        private async void PushSocket_OnMessageArrive(object sender, MessageArriveEventArgs e) {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                App.AppVM.ChatVM.PushSocket_OnMessageArrive(sender, e);
            });
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            if (e.Parameter != null) {
                ChatSimpleInfo info = JsonConvert.DeserializeObject<ChatSimpleInfo>(e.Parameter as string);
                switch (info.type) {
                    case 0:
                        // TODO: Check existence of chat
                        ChatVM.Chats.Insert(0, new ChatModel(info.id, 0, "DDDDDD"));
                        ChatList.SelectedIndex = 0;
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
}

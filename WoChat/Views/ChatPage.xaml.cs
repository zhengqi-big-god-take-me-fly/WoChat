using Newtonsoft.Json;
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
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            if (e.Parameter != null) {
                ChatSimpleInfo info = JsonConvert.DeserializeObject<ChatSimpleInfo>(e.Parameter as string);
                switch (info.type) {
                    case 0:
                        // TODO: Check existence of chat
                        ChatVM.Chats.Add(new ChatModel(info.id, 0, "DDDDDD"));
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
                    var result = await HTTP.PostUsers_Message(App.AppVM.LocalUserVM.JWT, chat.ReceiverId, 0, SendTextBox.Text);
                    if (result.StatusCode == PostUsers_MessageResult.PostUsers_MessageStatusCode.Success) SendTextBox.Text = "";
                    break;
            }
        }
    }
}

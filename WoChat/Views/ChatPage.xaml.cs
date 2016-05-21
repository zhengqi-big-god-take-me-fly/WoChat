using Newtonsoft.Json;
using System;
using System.Diagnostics;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using WoChat.Models;
using WoChat.Net;
using WoChat.Storage;
using WoChat.Utils;
using WoChat.ViewModels;

namespace WoChat.Views {
    public sealed partial class ChatPage : Page {
        private ChatViewModel ChatVM = App.AppVM.ChatVM;
        private LocalUserViewModel LocalUserVM = App.AppVM.LocalUserVM;
        private UIViewModel UIVM = new UIViewModel();
        private bool isCtrlDown = false;
        
        public ChatPage() {
            InitializeComponent();
            // For handling enter key down
            KeyEventHandler SendTextBoxKeyDownHandler = new KeyEventHandler(SendTextBox_KeyDown);
            SendTextBox.AddHandler(KeyDownEvent, SendTextBoxKeyDownHandler, true);
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

        private void SendButton_Click(object sender, RoutedEventArgs e) {
            sendMessage(/*SendTextBox.Text*/);
        }

        private async void AcceptInvitation_Click(object sender, RoutedEventArgs e) {
            Button self = sender as Button;
            if (self == null) return;
            string content = self.Tag as string;
            if (content == null) return;
            PutUsers_InvitationResult result = await HTTP.PutUsers_Invitation(App.AppVM.LocalUserVM.JWT, App.AppVM.LocalUserVM.LocalUser.Username, content);
            switch (result.StatusCode) {
                case PutUsers_InvitationResult.PutUsers_InvitationStatusCode.Success:
                    self.IsEnabled = false;
                    App.AppVM.ContactVM.Sync();
                    break;
                case PutUsers_InvitationResult.PutUsers_InvitationStatusCode.UnknownError:
                    NotificationHelper.ShowToast("未知错误，请稍候重试！");
                    break;
                default:
                    self.IsEnabled = false;
                    NotificationHelper.ShowToast("邀请已失效！");
                    break;
            }
        }

        private void SendTextBox_KeyUp(object sender, KeyRoutedEventArgs e) {
            if (e.Key == VirtualKey.Control) isCtrlDown = false;
        }

        private void SendTextBox_KeyDown(object sender, KeyRoutedEventArgs e) {
            if (e.Key == VirtualKey.Control) {
                isCtrlDown = true;
            } else if (e.Key == VirtualKey.Enter) {
                bool useEnter = SettingsHelper.LoadInt("send_message_key", 0) == 0;
                if ((useEnter && !isCtrlDown) || (!useEnter && isCtrlDown)) {
                    // Enter to send
                    if (SendTextBox.Text.EndsWith("\r\n")) {
                        SendTextBox.Text = SendTextBox.Text.Remove(SendTextBox.Text.LastIndexOf("\r\n"));
                    }
                    sendMessage(/*SendTextBox.Text.Remove(SendTextBox.Text.Length - 1)*/);
                    e.Handled = true;
                }
            }
        }

        private async void sendMessage(/*string msg*/) {
            if (SendTextBox.Text.Equals("")) return;
            var chat = ChatList.SelectedItem as ChatModel;
            switch (chat.Type) {
                case ChatModel.ChatType.User:
                    UIVM.IsSending = true;
                    var result = await HTTP.PostUsers_Message(App.AppVM.LocalUserVM.JWT, App.AppVM.ContactVM.FindUser(chat.ReceiverId).Username, 0, SendTextBox.Text);
                    UIVM.IsSending = false;
                    if (result.StatusCode == PostUsers_MessageResult.PostUsers_MessageStatusCode.Success) {
                        UIVM.Error = "";
                        SendTextBox.Text = "";
                        chat.MessageList.Add(new MessageModel(result.content, result.time, 0, result.receiver, result.sender));
                    } else {
                        UIVM.Error = "发送失败！";
                    }
                    break;
            }
        }

        private void MessageList_ContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args) {
            if (MessageList.Items.Count > 0) {
                MessageList.ScrollIntoView(MessageList.Items[MessageList.Items.Count - 1]);
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
        public bool IsSending {
            get {
                return isSending;
            }
            set {
                isSending = value;
                OnPropertyChanged();
            }
        }
        public string Error {
            get {
                return error;
            }
            set {
                error = value;
                OnPropertyChanged();
            }
        }

        private int selectedIndex = -1;
        private bool isSending = false;
        private string error = "";
    }
}

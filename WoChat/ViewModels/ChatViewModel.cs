using System.Collections.Generic;
using System.Collections.ObjectModel;
using WoChat.Commons.Models;
using WoChat.Models;
using WoChat.Net;

namespace WoChat.ViewModels {
    /// <summary>
    /// Retains references of data for binding, and provides methods to modify data.
    /// </summary>
    public class ChatViewModel : NotifyPropertyChangedBase {
        public ObservableCollection<ChatModel> Chats {
            get {
                return chats;
            }
            set {
                chats = value;
            }
        }

        public ChatViewModel() {
            //App.PushSocket.OnMessageArrive += PushSocket_OnMessageArrive;
        }

        public void PushSocket_OnMessageArrive(object sender, MessageArriveEventArgs e) {
            List<PushMessage> ms = e.Messages;
            foreach (var m in ms) {
                if (m.to_group) {
                    // TODO
                } else {
                    AppendMessage(m.sender, m.to_group, m.content, m.time);
                }
            }
        }

        private void AppendMessage(string id, bool toGroup, string content, long time) {
            // TODO: Use find method provided by database helper is more efficient.
            ChatModel cm = null;
            foreach (var c in Chats) {
                if (c.ReceiverId == id) {
                    cm = c;
                    break;
                }
            }
            if (cm == null) {
                cm = new ChatModel(id, toGroup ? 1 : 0);
                Chats.Add(cm);
            }
            cm.MessageList.Add(new MessageModel(content, time, toGroup ? 1 : 0, "", id, "DisplayName"));
        }

        public void Load() {
            isLoading = true;
            // TODO: Load from local storage.
            isLoading = false;
        }

        private ObservableCollection<ChatModel> chats = new ObservableCollection<ChatModel>();
        private bool isLoading = false;
    }
}

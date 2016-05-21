using System.Collections.Generic;
using System.Collections.ObjectModel;
using WoChat.Commons;
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
            App.PushSocket.OnMessageArrive += PushSocket_OnMessageArrive;
        }

        public void PushSocket_OnMessageArrive(object sender, MessageArriveEventArgs e) {
            List<PushMessage> ms = e.Messages;
            List<string> mr = new List<string>();
            foreach (var m in ms) {
                switch (m.type) {
                    case 0:
                        AppendMessage(m.sender, 0, m.content, m.time);
                        break;
                    case 1:
                        break;
                    case 2:
                        // TODO: Handle more types of system user
                        // Friend Invitation
                        AppendMessage(m.sender, 2, m.content, m.time);
                        break;
                    default:
                        break;
                }
                mr.Add(m._id);
            }
            App.PushSocket.MessagesRead(mr);
        }

        private void AppendMessage(string id, int type, string content, long time) {
            // TODO: Using find method provided by database helper is more efficient.
            ChatModel cm = null;
            foreach (var c in Chats) {
                if (c.ReceiverId == id) {
                    cm = c;
                    break;
                }
            }
            if (cm == null) {
                cm = new ChatModel(id, 0);
                Chats.Insert(0, cm);
            }
            cm.MessageList.Add(new MessageModel(content, time, type, "", id));
        }

        public void Load() {
            isLoading = true;
            Chats.Clear();
            // TODO: Load from local storage.
            isLoading = false;
        }

        private ObservableCollection<ChatModel> chats = new ObservableCollection<ChatModel>();
        private bool isLoading = false;
    }
}

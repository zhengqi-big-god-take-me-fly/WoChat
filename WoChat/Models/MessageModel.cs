namespace WoChat.Models {
    public class MessageModel : NotifyPropertyChangedBase {
        public string Content {
            get {
                return content;
            }
            set {
                content = value;
                OnPropertyChanged();
            }
        }

        public long Time {
            get {
                return time;
            }
            set {
                time = value;
                OnPropertyChanged();
            }
        }

        public MessageTypeEnum MessageType {
            get {
                return messageType;
            }
            set {
                messageType = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The chat this message belongs to.
        /// </summary>
        public string ChatId {
            get {
                return chatId;
            }
            set {
                chatId = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// The UserId of the sender.
        /// </summary>
        /// <remarks>
        /// If this is a group chat, SenderId should be the UserId of the group member, not the GroupId.
        /// </remarks>
        public string SenderId {
            get {
                return senderid;
            }
            private set {
                senderid = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// This should be the display name of the sender, such as the group nickname or remark name.
        /// Considering that getting real time nickname with SenderId may be asynchronous, this property
        /// provide real time access of nickname, It provides lastly used nickname stored in database.
        /// As result, you have to manually set it to latest when opening related chat.
        /// </summary>
        public string SenderDisplayName {
            get {
                return senderDisplayName;
            }
            set {
                senderDisplayName = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Convert Time to human-readable time string.
        /// </summary>
        /// <remarks>
        /// Not need to store in database.
        /// </remarks>
        public string HumanTime {
            get {
                return time.ToString();
            }
        }

        public enum MessageTypeEnum { Text };

        public MessageModel(string _content = "", long _time = 0, int _messageType = 0, string _chatId = "", string _senderId = "", string _senderDisplayName = "") {
            Content = _content;
            Time = _time;
            MessageType = (MessageTypeEnum)_messageType;
            ChatId = _chatId;
            SenderId = _senderId;
            SenderDisplayName = senderDisplayName;
        }

        private string content = "";
        private long time = 0;
        private MessageTypeEnum messageType = MessageTypeEnum.Text;
        private string chatId = "";
        private string senderid = "";
        private string senderDisplayName = "";
    }
}
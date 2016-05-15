namespace WoChat.Models {
    public class MessageModel {
        public string Content {
            get;
            set;
        }

        public int Time {
            get;
            set;
        }

        public MessageTypeEnum MessageType {
            get;
            set;
        }

        /// <summary>
        /// The chat this message belongs to.
        /// </summary>
        public string ChatId {
            get;
            set;
        }

        /// <summary>
        /// The UserId of the sender.
        /// </summary>
        /// <remarks>
        /// If this is a group chat, SenderId should be the UserId of the group member, not the GroupId.
        /// </remarks>
        public string SenderId {
            get;
            private set;
        }

        /// <summary>
        /// This should be the display name of the sender, such as the group nickname or remark name.
        /// Considering that getting real time nickname with SenderId may be asynchronous, this property
        /// provide real time access of nickname, It provides lastly used nickname stored in database.
        /// As result, you have to manually set it to latest when opening related chat.
        /// </summary>
        public string SenderDisplayName {
            get;
            set;
        }

        /// <summary>
        /// Convert Time to human-readable time string.
        /// </summary>
        /// <remarks>
        /// Not need to store in database.
        /// </remarks>
        public string HumanTime {
            get;
        }

        public enum MessageTypeEnum { Text };
    }
}
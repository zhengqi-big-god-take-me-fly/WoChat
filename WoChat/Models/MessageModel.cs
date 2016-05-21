using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using WoChat.Commons;
using WoChat.Utils;

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
                HumanTime = "";
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
            set {
                senderid = value;
                OnPropertyChanged();
                // Change other data that should be retrieved from other model
                // Sadly, this make this model tight coupling.
                switch (MessageType) {
                    case MessageTypeEnum.Text:
                        if (value.Equals(App.AppVM.LocalUserVM.LocalUser.UserId)) {
                            SenderModel = App.AppVM.LocalUserVM.LocalUser;
                        } else {
                            SenderModel = App.AppVM.ContactVM.FindUser(value);
                        }
                        break;
                    case MessageTypeEnum.FriendInvitation:
                        SenderModel = App.AppVM.SystemVM.FindUser(SystemIds.SystemIdFriendInvitation);
                        break;
                    default:
                        break;
                }
            }
        }

        public UserModel SenderModel {
            get {
                return senderModel;
            }
            set {
                senderModel = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// This should be the display name of the sender, such as the group nickname or remark name.
        /// Considering that getting real time nickname with SenderId may be asynchronous, this property
        /// provide real time access of nickname, It provides lastly used nickname stored in database.
        /// As result, you have to manually set it to latest when opening related chat.
        /// </summary>
        //public string SenderDisplayName {
        //    get {
        //        return senderDisplayName;
        //    }
        //    set {
        //        senderDisplayName = value;
        //        OnPropertyChanged();
        //    }
        //}

        /// <summary>
        /// This property is only relying on SenderId's change.
        /// </summary>
        //public ImageSource SenderAvatarSource {
        //    get {
        //        return senderAvatarSource;
        //    }
        //    set {
        //        senderAvatarSource = value;
        //        OnPropertyChanged();
        //    }
        //}

        /// <summary>
        /// Convert Time to human-readable time string.
        /// </summary>
        /// <remarks>
        /// Not need to store in database.
        /// </remarks>
        public string HumanTime {
            get {
                return TimesTool.UnixTimeStampToDateTime(time / 1000);
            }
            private set {
                OnPropertyChanged();
            }
        }

        public enum MessageTypeEnum { Text, Image, FriendInvitation };

        public MessageModel(string _content = "", long _time = 0, int _messageType = 0, string _chatId = "", string _senderId = "", string _senderDisplayName = "") {
            Content = _content;
            Time = _time;
            MessageType = (MessageTypeEnum)_messageType;
            ChatId = _chatId;
            SenderId = _senderId;
            //SenderDisplayName = senderDisplayName;
        }

        private string content = "";
        private long time = 0;
        private MessageTypeEnum messageType = MessageTypeEnum.Text;
        private string chatId = "";
        private string senderid = "";
        private UserModel senderModel = new UserModel();
        private string senderDisplayName = "";
        private ImageSource senderAvatarSource = new BitmapImage();
    }
}
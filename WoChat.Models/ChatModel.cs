using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WoChat.Models {
    /// <summary>
    /// This model stores a chat the local user participates in.
    /// You can implement this class however you want, but please keep the public properties available.
    /// </summary>
    /// <todo>
    /// Avatar releted property
    /// </todo>
    public class ChatModel : INotifyPropertyChanged {
        /// <summary>
        /// Context outside the model should be able to get ChatId in order to find specific chats.
        /// </summary>
        public string ChatId {
            get;
            private set;
        }

        /// <summary>
        /// Chater is local user.
        /// </summary>
        /// <remarks>
        /// @a20185
        /// Chater is always the local user, so it is NOT necessary to provide this property.
        /// However, it is free for you to store any field like chatee or others :D
        /// </remarks>
        //public UserModel Chater {
        //    get;
        //}

        /// <summary>
        /// Indicate chat type
        /// </summary>
        public ChatTypeEnum ChatType {
            get;
            private set;
        }

        /// <summary>
        /// Display name for a chat.
        /// Typically, it return the nickname of the user when ChatType is User.
        /// </summary>
        public string DisplayName {
            get;
            set;
        }

        /// <summary>
        /// The latest message of this chat, in plain text format. Used for displaying in chat ListViewItem.
        /// </summary>
        public string LatestMessageText {
            get;
            private set;
        }

        /// <summary>
        /// The time of the latest message of this chat, in human-readable format.
        /// </summary>
        public string LatestMessageTimeText {
            get;
            private set;
        }

        /// <summary>
        /// The messages of the chat.
        /// </summary>
        public ObservableCollection<MessageModel> MessageList {
            get;
            private set;
        }

        public enum ChatTypeEnum { User, Group, System };

        public void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}

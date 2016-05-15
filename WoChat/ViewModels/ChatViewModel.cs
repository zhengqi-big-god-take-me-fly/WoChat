using System.Collections.ObjectModel;
using WoChat.Models;
using WoChat.Utils;

namespace WoChat.ViewModels {
    /// <summary>
    /// Retains references of data for binding, and provides methods to modify data.
    /// </summary>
    public class ChatViewModel : NotifyPropertyChangedBase {
        public ObservableCollection<ChatModel> Chats {
            get;
        }
    }
}

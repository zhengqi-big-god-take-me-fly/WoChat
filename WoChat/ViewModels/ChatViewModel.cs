using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoChat.Models;

namespace WoChat.ViewModels {
    /// <summary>
    /// Retains references of data for binding, and provides methods to modify data.
    /// </summary>
    class ChatViewModel : INotifyPropertyChanged {
        public ObservableCollection<ChatModel> Chats {
            get;
        }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}

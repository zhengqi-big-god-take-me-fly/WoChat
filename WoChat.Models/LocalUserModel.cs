using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoChat.Models {
    public class LocalUserModel : UserModel {
        public ObservableCollection<ContactModel> Contacts {
            get;
            private set;
        }

        public ObservableCollection<ChatModel> Chats {
            get;
            private set;
        }
    }
}

using WoChat.Commons.Models;

namespace WoChat.ViewModels {
    /// <summary>
    /// An all-in-one ViewModel for data bindings and data modifications.
    /// </summary>
    public class AppViewModel : NotifyPropertyChangedBase {
        public LocalUserViewModel LocalUserVM {
            get {
                return localUserVM;
            }
        }

        public ChatViewModel ChatVM {
            get {
                return chatVM;
            }
        }

        public ContactViewModel ContactVM {
            get {
                return contactVM;
            }
        }

        public GroupViewModel GroupVM {
            get {
                return groupVM;
            }
        }

        /// <summary>
        /// Load all data in this ViewModel from local storage such as database
        /// </summary>
        public void Load() {
            LocalUserVM.Load();
            ChatVM.Load();
            ContactVM.Load();
            GroupVM.Load();
        }

        private LocalUserViewModel localUserVM = new LocalUserViewModel();
        private ChatViewModel chatVM = new ChatViewModel();
        private ContactViewModel contactVM = new ContactViewModel();
        private GroupViewModel groupVM = new GroupViewModel();
    }
}

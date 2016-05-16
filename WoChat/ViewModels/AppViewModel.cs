using System.ComponentModel;
using WoChat.Models;
using WoChat.Utils;

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

        public MixedContactViewModel MixedContactVM {
            get {
                return mixedContactVM;
            }
        }

        /// <summary>
        /// Load all data in this ViewModel from local storage such as database
        /// </summary>
        public void Load() {
            LocalUserVM.Load();
            ChatVM.Load();
            MixedContactVM.Load();
        }

        private LocalUserViewModel localUserVM = new LocalUserViewModel();
        private ChatViewModel chatVM = new ChatViewModel();
        private MixedContactViewModel mixedContactVM = new MixedContactViewModel();
    }
}

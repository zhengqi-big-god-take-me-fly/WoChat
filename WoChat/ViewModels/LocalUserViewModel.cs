using WoChat.Models;
using WoChat.Utils;

namespace WoChat.ViewModels {
    public class LocalUserViewModel : NotifyPropertyChangedBase {
        public LocalUserModel LocalUser {
            get {
                return localUser;
            }
        }

        public bool AlreadyLoggedIn {
            get {
                return alreadyLoggedIn;
            }
            set {
                alreadyLoggedIn = value;
                OnPropertyChanged();
                // TODO: Store to local settings
            }
        }

        public string JWT {
            get {
                return jwt;
            }
            set {
                jwt = value;
                OnPropertyChanged();
                // TODO: Store to local settings
            }
        }

        /// <summary>
        /// Store logged in user data
        /// </summary>
        /// <param name="j">JWT token</param>
        public void UserLogIn(string j) {
            AlreadyLoggedIn = true;
            JWT = j;
        }

        private LocalUserModel localUser = new LocalUserModel();
        private bool alreadyLoggedIn = false;
        private string jwt = "";
    }
}

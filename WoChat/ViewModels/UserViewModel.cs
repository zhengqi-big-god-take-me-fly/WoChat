using WoChat.Models;

namespace WoChat.ViewModels {
    /// <summary>
    /// For the user information of this client
    /// </summary>
    public class LocalUserViewModel {
        public UserModel LocalUser {
            get {
                return localUser;
            }
        }

        public string JWT {
            get {
                return jwt;
            }
            set {
                jwt = value;
            }
        }

        public bool IsLogin {
            get {
                return isLogin;
            }
            set {
                isLogin = value;
            }
        }

        /// <summary>
        /// Save all local user data to local storage
        /// </summary>
        public void Save() {
            //TODO: Save to local storage
        }

        /// <summary>
        /// Load all local user data from local storage
        /// </summary>
        public void Load() {
            //TODO: Load from local storage
        }

        private UserModel localUser;
        private string jwt;
        private bool isLogin;
    }
}

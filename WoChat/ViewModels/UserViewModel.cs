using WoChat.Models;

namespace WoChat.ViewModels {
    /// <summary>
    /// For the user information of this client
    /// </summary>
    public class UserViewModel {
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

        private UserModel localUser;
        private string jwt;
        private bool isLogin;
    }
}

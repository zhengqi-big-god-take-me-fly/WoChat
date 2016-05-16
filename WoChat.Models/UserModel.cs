using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WoChat.Models {
    /// <summary>
    /// Model for common user information.
    /// </summary>
    public class UserModel : NotifyPropertyChangedBase {
        public string UserId {
            get {
                return userId;
            }
            set {
                userId = value;
                OnPropertyChanged();
            }
        }

        public string Username {
            get {
                return username;
            }
            set {
                username = value;
                OnPropertyChanged();
            }
        }

        public string Nickname {
            get {
                return nickname;
            }
            set {
                nickname = value;
                OnPropertyChanged();
            }
        }

        private string userId = "";
        private string username = "";
        private string nickname = "";
    }
}

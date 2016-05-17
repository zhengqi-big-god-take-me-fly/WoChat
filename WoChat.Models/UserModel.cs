using WoChat.Commons.Models;

namespace WoChat.Models {
    /// <summary>
    /// Model for common user information.
    /// </summary>
    public class UserModel : NotifyPropertyChangedBase {
        public virtual string UserId {
            get {
                return userId;
            }
            set {
                userId = value;
                OnPropertyChanged();
            }
        }

        public virtual string Username {
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

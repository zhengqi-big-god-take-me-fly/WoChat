using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WoChat.Models {
    /// <summary>
    /// Model for common user information.
    /// </summary>
    public class UserModel : INotifyPropertyChanged {
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

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private string userId = "";
        private string username = "";
        private string nickname = "";
    }
}

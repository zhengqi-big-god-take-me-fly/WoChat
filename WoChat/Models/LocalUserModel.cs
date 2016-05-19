using WoChat.Storage;

namespace WoChat.Models {
    public class LocalUserModel : UserModel {
        public override string Username {
            get {
                return SettingsHelper.LoadString("username", "");
            }
            set {
                SettingsHelper.Save("username", value);
                OnPropertyChanged();
            }
        }

        public override string UserId {
            get {
                return SettingsHelper.LoadString("user_id", "");
            }
            set {
                SettingsHelper.Save("user_id", value);
                OnPropertyChanged();
            }
        }
    }
}

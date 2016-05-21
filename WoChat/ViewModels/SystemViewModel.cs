using System.Collections.ObjectModel;
using WoChat.Commons;
using WoChat.Models;

namespace WoChat.ViewModels {
    public class SystemViewModel : NotifyPropertyChangedBase {
        public ObservableCollection<SystemModel> Systems {
            get {
                return systems;
            }

            set {
                systems = value;
                OnPropertyChanged();
            }
        }

        public void Load() {
            Systems.Clear();
            // TODO: Load from db
            // Add system user "Friend invitation"
            Systems.Add(new SystemModel(SystemIds.SystemIdFriendInvitation, "WOCHAT_SYSTEM_FRIEND_INVITATION", "Friend invitation", 0, 0, "ms-appx:///Assets/WOCHAT_SYSTEM_FRIEND_INVITATION.png"));
        }

        public SystemModel FindUser(string id) {
            foreach (var s in Systems) {
                if (s.UserId == id) {
                    return s;
                }
            }
            return new SystemModel("", "", "", 0, 0, "ms-appx:///Assets/default.png");
        }

        private ObservableCollection<SystemModel> systems = new ObservableCollection<SystemModel>();
    }
}

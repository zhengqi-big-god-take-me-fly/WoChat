using WoChat.Commons.Models;

namespace WoChat.Commons.Utils {
    public class MenuItem : NotifyPropertyChangedBase {
        public MenuItem(string _icon = "", string _title = "") {
            Icon = _icon;
            Title = _title;
        }

        public string Icon {
            get {
                return icon;
            }
            set {
                icon = value;
                OnPropertyChanged();
            }
        }
        public string Title {
            get {
                return title;
            }
            set {
                title = value;
                OnPropertyChanged();
            }
        }

        private string icon = "";
        private string title = "";
    }
}

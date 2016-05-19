using Windows.UI.Xaml;
using WoChat.Commons.Models;

namespace WoChat.Commons.Utils {
    public class MenuItem : NotifyPropertyChangedBase {
        public MenuItem(string _icon = "", string _title = "", int _unread = 0, Visibility _visibility = Visibility.Collapsed) {
            Icon = _icon;
            Title = _title;
            Unread = _unread;
            indicatorVisibility = _visibility;
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

        public Visibility IndicatorVisibility {
            get {
                return indicatorVisibility;
            }
            set {
                indicatorVisibility = value;
                OnPropertyChanged();
            }
        }

        public int Unread {
            get {
                return unread;
            }
            set {
                unread = value;
                OnPropertyChanged();
            }
        }

        private string icon = "";
        private string title = "";
        private int unread = 0;
        private Visibility indicatorVisibility = Visibility.Collapsed;
    }
}

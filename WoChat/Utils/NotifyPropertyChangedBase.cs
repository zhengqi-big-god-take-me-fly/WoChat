using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WoChat.Utils {
    public class NotifyPropertyChangedBase : INotifyPropertyChanged {
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WoChat.Commons.Models {
    public class NotifyPropertyChangedBase : INotifyPropertyChanged {
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}

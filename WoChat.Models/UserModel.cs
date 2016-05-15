using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WoChat.Models {
    /// <summary>
    /// Model for common user information.
    /// </summary>
    public class UserModel : INotifyPropertyChanged {
        public string UserId {
            get;
            set;
        }

        public string Username {
            get;
            set;
        }

        public string Nickname {
            get;
            set;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };
    }
}

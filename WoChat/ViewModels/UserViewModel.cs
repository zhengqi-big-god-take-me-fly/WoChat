using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WoChat.Models;

namespace WoChat.ViewModels {
    /// <summary>
    /// For the user information of this client
    /// </summary>
    class UserViewModel {
        public UserModel LocalUser {
            get {
                return localUser;
            }
        }
        private UserModel localUser;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoChat.Models {
    public class ContactModel : UserModel {
        public string Remark {
            get;
            set;
        }

        public BlockLevelType BlockLevel {
            get;
            set;
        }

        /// <remarks>
        /// The order is the same as that from API document.
        /// </remarks>
        public enum BlockLevelType { Normal, Mute, Blocked, NotFriend };
    }
}

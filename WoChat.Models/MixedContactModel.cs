using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoChat.Models {
    public class MixedContactModel {
        public string DisplayName {
            get;
        }

        public ContactType Type {
            get;
        }

        public enum ContactType { User, Group, System };
    }
}

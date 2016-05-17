namespace WoChat.Models {
    public class MixedContactModel : NotifyPropertyChangedBase {
        public string DisplayName {
            get {
                return displayName;
            }
        }

        public ContactType Type {
            get {
                return type;
            }
        }

        public enum ContactType { User, Group, System };

        private string displayName = "";
        private ContactType type = ContactType.User;
    }
}

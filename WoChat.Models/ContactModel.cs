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

        public ContactModel(string _id = "", string _username = "", string _nickname = "", string _avatar = "", string _remark = "", int blt = 0) {
            UserId = _id;
            Username = _username;
            Nickname = _nickname;
            // TODO: Avatar
            Remark = _remark;
            BlockLevel = (BlockLevelType)blt;
        }

        /// <remarks>
        /// The order is the same as that from API document.
        /// </remarks>
        public enum BlockLevelType { Normal, Mute, Blocked, NotFriend };
    }
}

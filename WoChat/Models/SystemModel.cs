using System;
using Windows.UI.Xaml.Media.Imaging;

namespace WoChat.Models {
    public class SystemModel : UserModel {
        public SystemModel(string _id, string _username, string _nickname, int _gender, int _region, string _avatar) {
            UserId = _id;
            Username = _username;
            Nickname = _nickname;
            Gender = _gender;
            Region = _region;
            Avatar = _avatar;
        }

        protected override void updateAvatarSource() {
            AvatarSource = new BitmapImage(new Uri(Avatar));
        }
    }
}

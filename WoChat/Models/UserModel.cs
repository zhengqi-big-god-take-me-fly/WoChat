using System;
using System.Diagnostics;
using System.IO;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using WoChat.Net;

namespace WoChat.Models {
    /// <summary>
    /// Model for common user information.
    /// </summary>
    public class UserModel : NotifyPropertyChangedBase {
        public virtual string UserId {
            get {
                return userId;
            }
            set {
                userId = value;
                OnPropertyChanged();
            }
        }

        public virtual string Username {
            get {
                return username;
            }
            set {
                username = value;
                OnPropertyChanged();
            }
        }

        public string Nickname {
            get {
                return nickname;
            }
            set {
                nickname = value;
                OnPropertyChanged();
            }
        }

        public int Gender {
            get {
                return gender;
            }
            set {
                gender = value;
                OnPropertyChanged();
            }
        }

        public int Region {
            get {
                return region;
            }
            set {
                region = value;
                OnPropertyChanged();
            }
        }

        public string Avatar {
            get {
                return avatar;
            }
            set {
                avatar = value;
                OnPropertyChanged();
                updateAvatarSource();
            }
        }

        public ImageSource AvatarSource {
            get {
                return avatarSource;
            }
            private set {
                avatarSource = value;
                OnPropertyChanged();
            }
        }

        private async void updateAvatarSource() {
            if (Avatar == "") return;
            var localFolder = ApplicationData.Current.LocalFolder;
            string filename = Avatar.Substring(Avatar.LastIndexOfAny(new char[2] { '/', '\\' }) + 1);
            try {
                StorageFile avatarFile = await localFolder.GetFileAsync(filename);
                BitmapImage source = new BitmapImage();
                await source.SetSourceAsync(await avatarFile.OpenReadAsync());
                AvatarSource = source;
            } catch (FileNotFoundException e) {
                string avatarOriginal = Avatar;
                Avatar = "";
                IBuffer buffer = await HTTP.GetAvatar(avatarOriginal);
                StorageFile newAvatar = await localFolder.CreateFileAsync(filename);
                Stream write = await newAvatar.OpenStreamForWriteAsync();
                await write.AsOutputStream().WriteAsync(buffer);
                Avatar = avatarOriginal;
            } catch (Exception e) {
                Debug.WriteLine(e.Message);
            }
        }

        private string userId = "";
        private string username = "";
        private string nickname = "";
        private string avatar = "";
        private int gender = 0;
        private int region = 0;
        private ImageSource avatarSource = new BitmapImage();
    }
}

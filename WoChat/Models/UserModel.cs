using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
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

        public virtual string Avatar {
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
            protected set {
                avatarSource = value;
                OnPropertyChanged();
            }
        }

        protected virtual async void updateAvatarSource() {
            if (Avatar == "") return;
            var localFolder = ApplicationData.Current.LocalFolder;
            string filename = Avatar.Substring(Avatar.LastIndexOfAny(new char[2] { '/', '\\' }) + 1);
            // If the file is not exist, create one.
            StorageFile avatarFile;
            try {
                avatarFile = await localFolder.GetFileAsync(filename);
            } catch (FileNotFoundException e) {
                Debug.WriteLine(e.Message);
                avatarFile = await localFolder.CreateFileAsync(filename);
            } catch (Exception e) {
                Debug.WriteLine(e.Message);
                return;
            }
            // Download file from web
            IBuffer downloadedBuffer = await HTTP.GetAvatar(Avatar);
            try {
                // Store downloaded content into file
                Stream writeStream = await avatarFile.OpenStreamForWriteAsync();
                await writeStream.AsOutputStream().WriteAsync(downloadedBuffer);
                await writeStream.AsOutputStream().FlushAsync();
                writeStream.AsOutputStream().Dispose();
                // Read content and add it to source
                BitmapImage source = new BitmapImage();
                IRandomAccessStream ras = await avatarFile.OpenReadAsync();
                await source.SetSourceAsync(ras);
                AvatarSource = source;
            } catch (Exception e) {
                Debug.WriteLine(e.Message);
            }
        }

        public async Task RemoveAvatar() {
            if (Avatar == "") return;
            var localFolder = ApplicationData.Current.LocalFolder;
            string filename = Avatar.Substring(Avatar.LastIndexOfAny(new char[2] { '/', '\\' }) + 1);
            try {
                StorageFile avatarFile = await localFolder.GetFileAsync(filename);
                await avatarFile.DeleteAsync();
            } catch (Exception e) {
                Debug.WriteLine(e.Message);
            }
        }

        private string userId = "";
        private string username = "";
        private string nickname = "";
        protected string avatar = "";
        private int gender = 0;
        private int region = 0;
        protected ImageSource avatarSource = new BitmapImage(new Uri("ms-appx:///Assets/default.png"));
    }
}

using Newtonsoft.Json;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using WoChat.Models;
using WoChat.Net;
using WoChat.Storage;

namespace WoChat.ViewModels {
    public class LocalUserViewModel : NotifyPropertyChangedBase {
        public LocalUserModel LocalUser {
            get {
                return localUser;
            }
        }

        public bool AlreadyLoggedIn {
            get {
                return SettingsHelper.LoadBool("already_logged_in", false);
            }
            set {
                SettingsHelper.Save("already_logged_in", value);
                OnPropertyChanged();
            }
        }

        public string JWT {
            get {
                return SettingsHelper.LoadString("jwt", "");
            }
            set {
                SettingsHelper.Save("jwt", value);
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Store logged in user data
        /// </summary>
        /// <param name="j">JWT token</param>
        public void UserLogIn(string j) {
            AlreadyLoggedIn = true;
            JWT = j;
            // Decode
            string payloadJson = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, CryptographicBuffer.DecodeFromBase64String(JWT.Split('.')[1]));
            JWTPayload jp = JsonConvert.DeserializeObject<JWTPayload>(payloadJson);
            LocalUser.UserId = jp._id;
            LocalUser.Username = jp.username;
        }

        public void UserLogOut() {
            JWT = "";
            LocalUser.UserId = "";
            LocalUser.Username = "";
            AlreadyLoggedIn = false;
        }

        /// <summary>
        /// Load all data from local storage, such as database.
        /// All the old data in this ViewModel will be overwriten.
        /// </summary>
        public void Load() {
            // TODO
        }

        public async void Sync() {
            if (isSyncing) return;
            await WaitUntilLoaded();
            isSyncing = true;
            GetUsers_Result result = await HTTP.GetUsers_(LocalUser.Username);
            LocalUser.UserId = result._id;
            LocalUser.Username = result.username;
            LocalUser.Nickname = result.nickname;
            LocalUser.Avatar = result.avatar;
            LocalUser.Gender = result.gender;
            LocalUser.Region = result.region;
            isSyncing = false;
        }

#pragma warning disable CS1998
        public async Task WaitUntilLoaded() {
            while (isLoading) ;
        }
#pragma warning restore CS1998

        private LocalUserModel localUser = new LocalUserModel();
        private bool isLoading = false;
        private bool isSyncing = false;
    }

    class JWTPayload {
        public string _id = "";
        public string username = "";
    }
}

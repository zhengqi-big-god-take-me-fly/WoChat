using Newtonsoft.Json;
using Windows.Security.Cryptography;
using WoChat.Commons.Models;
using WoChat.Commons.Storage;
using WoChat.Models;

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

        /// <summary>
        /// Load all data from local storage, such as database.
        /// All the old data in this ViewModel will be overwriten.
        /// </summary>
        public void Load() {
            // TODO
        }

        private LocalUserModel localUser = new LocalUserModel();
        //private bool alreadyLoggedIn = false;
        //private string jwt = "";
    }

    class JWTPayload {
        public string _id = "";
        public string username = "";
    }
}

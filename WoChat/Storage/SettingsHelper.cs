using Windows.Storage;

namespace WoChat.Storage {
    public class SettingsHelper {
        public static void Save(string key, string value) {
            ApplicationData.Current.LocalSettings.Values[key] = value;
        }

        public static string Load(string key, string defaultValue) {
            return (string)ApplicationData.Current.LocalSettings.Values[key] ?? defaultValue;
        }
    }
}

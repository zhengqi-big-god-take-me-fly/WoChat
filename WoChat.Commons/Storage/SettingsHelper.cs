using Windows.Storage;

namespace WoChat.Commons.Storage {
    public class SettingsHelper {
        public static void Save(string key, string value) {
            ApplicationData.Current.LocalSettings.Values[key] = value;
        }

        public static string LoadString(string key, string defaultValue) {
            return (string)ApplicationData.Current.LocalSettings.Values[key] ?? defaultValue;
        }

        public static void Save(string key, bool value) {
            ApplicationData.Current.LocalSettings.Values[key] = value;
        }

        public static bool LoadBool(string key, bool defaultValue) {
            return (bool?)ApplicationData.Current.LocalSettings.Values[key] ?? defaultValue;
        }
    }
}

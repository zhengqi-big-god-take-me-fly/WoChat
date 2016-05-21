using Newtonsoft.Json;
using System;
using System.Text;
using Windows.Security.Cryptography;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace WoChat.Utils {
    public class ObjectToMenuItemConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            if (value != null)
                return (MenuItem)value;
            return value;
        }
    }

    public class IntToStringConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            return ((int)value).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            return int.Parse((string)value);
        }
    }

    /// <summary>
    /// Boolean to Visibility converter
    /// </summary>
    /// <seealso cref="https://github.com/jamesmcroft/WinUX-UWP-Toolkit/blob/master/Croft.Core/Croft.Core.UWP/Xaml/Converters/BooleanToVisibilityConverter.cs"/>
    public class BooleanToVisibilityConverter : IValueConverter {
        /// <summary>
        /// Converts a bool value to a Visibility value.
        /// </summary>
        /// <returns>
        /// Returns Visibility.Collapsed if false, else Visibility.Visible.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, string language) {
            var b = value as bool?;
            return b == null ? Visibility.Visible : (b.Value ? Visibility.Visible : Visibility.Collapsed);
        }

        /// <summary>
        /// Converts a Visibility value to a bool value.
        /// </summary>
        /// <returns>
        /// Returns true if Visiblility.Visible, else false.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            var v = value as Visibility?;
            return v == null ? (object)null : v.Value == Visibility.Visible;
        }
    }

    public class BooleanToVisibilityReverseConverter : IValueConverter {
        /// <summary>
        /// Converts a bool value to a Visibility value.
        /// </summary>
        /// <returns>
        /// Returns Visibility.Visible if false, else Visibility.Collapsed.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, string language) {
            var b = value as bool?;
            return b == null ? Visibility.Collapsed : (b.Value ? Visibility.Collapsed : Visibility.Visible);
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }

    public class EqualToVisibilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            return value.ToString().Equals(parameter as string) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }

    public class GenderMaleConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            int g = value as int? ?? 0;
            return g == 0 ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }

    public class GenderFemaleConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            int g = value as int? ?? 0;
            return g == 1 ? true : false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }

    public class FriendInvitationContentConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            string jwt = value as string;
            if (jwt == null) return "";
            try {
                jwt = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, CryptographicBuffer.DecodeFromBase64String(jwt.Split('.')[1]));
                InvitationInfo info = JsonConvert.DeserializeObject<InvitationInfo>(jwt);
                return new StringBuilder().Append("你收到了一条好友申请\n").Append(info.message).ToString();
            } catch (Exception e) {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }

    class SenderIdToMessageBackgroundConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            bool isSent = (value as string ?? "").Equals(App.AppVM.LocalUserVM.LocalUser.UserId);
            // TODO: Change to theme color
            return new SolidColorBrush(isSent == false ? Color.FromArgb(223, 255, 255, 255) : Color.FromArgb(223, 205, 114, 1));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }

    class SenderIdToMessageHorizontalAlignmentConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            bool isSent = (value as string ?? "").Equals(App.AppVM.LocalUserVM.LocalUser.UserId);
            return isSent ? HorizontalAlignment.Right : HorizontalAlignment.Left;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }

    class SenderIdToMessageAvatarVisibilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            bool isSent = (value as string ?? "").Equals(App.AppVM.LocalUserVM.LocalUser.UserId);
            bool isLeft = (parameter as string ?? "Left").Equals("Left");
            return (isSent && isLeft) || (!isSent && !isLeft) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }

    class SelectedIndexToVisibilityConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, string language) {
            int i = value as int? ?? -1;
            return i == -1 ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) {
            throw new NotImplementedException();
        }
    }

    public class InvitationInfo {
        public string sender;
        public string receiver;
        public string message;
        public long iat;
    }
}

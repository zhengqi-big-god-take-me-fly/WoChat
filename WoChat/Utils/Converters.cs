using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

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
}

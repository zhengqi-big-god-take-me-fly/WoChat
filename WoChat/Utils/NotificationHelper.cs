using System.Linq;
using Windows.UI.Notifications;

namespace WoChat.Utils {
    public class NotificationHelper {
        public static void ShowToast(string noti) {
            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            toastXml.GetElementsByTagName("text").First().AppendChild(toastXml.CreateTextNode(noti));
            //var toastNotification = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(new ToastNotification(toastXml));
        }
    }
}

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WoChat.Commons.Utils;
using WoChat.Models;
using WoChat.Net;

namespace WoChat.ViewModels {

    public class ContactViewModel {
        public ObservableCollection<ContactModel> Contacts {
            get {
                return contacts;
            }
        }

        public void Load() {
            // TODO: Load from db
        }

        public async void Sync() {
            await WaitUntilLoaded();
            GetUsers_ContactsResult r = await HTTP.GetUsers_Contacts(App.AppVM.LocalUserVM.JWT, App.AppVM.LocalUserVM.LocalUser.Username);
            switch (r.StatusCode) {
                case GetUsers_ContactsResult.GetUsers0ContactsStatusCode.Success:
                    Contacts.Clear();
                    foreach (var c in r.contacts) {
                        Contacts.Add(new ContactModel(c.contact._id, c.contact.username, c.contact.nickname, c.contact.avatar, c.remark, c.block_level));
                    }
                    break;
                case GetUsers_ContactsResult.GetUsers0ContactsStatusCode.InvalidToken:
                    NotificationHelper.ShowToast("无法验证您的身份，请尝试重新登录！");
                    break;
                default:
                    NotificationHelper.ShowToast("未知错误，无法更新联系人列表！");
                    break;
            }
        }

        private async Task WaitUntilLoaded() {
            while (isLoading) ;
        }

        private ObservableCollection<ContactModel> contacts = new ObservableCollection<ContactModel>();
        private bool isLoading = false;
    }
}

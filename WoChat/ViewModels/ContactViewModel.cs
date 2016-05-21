using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WoChat.Models;
using WoChat.Net;
using WoChat.Utils;

namespace WoChat.ViewModels {

    public class ContactViewModel {
        public ObservableCollection<ContactModel> Contacts {
            get {
                return contacts;
            }
        }

        public void Load() {
            Contacts.Clear();
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
                        // Update info. Should use one-to-one ChatId-ReceiverId pair to avoid loop finding.
                        foreach (var ch in App.AppVM.ChatVM.Chats) {
                            if (ch.ReceiverId.Equals(c.contact._id)) {
                                ch.RefreshReceiver();
                                break;
                            }
                        }
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

        public ContactModel FindUser(string id) {
            foreach (var c in Contacts) {
                if (c.UserId.Equals(id)) {
                    return c;
                }
            }
            return new ContactModel();
        }

#pragma warning disable CS1998
        private async Task WaitUntilLoaded() {
            while (isLoading) ;
        }
#pragma warning restore CS1998

        private ObservableCollection<ContactModel> contacts = new ObservableCollection<ContactModel>();
        private bool isLoading = false;
    }
}

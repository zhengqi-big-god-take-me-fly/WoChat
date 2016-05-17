using System.Collections.ObjectModel;
using System.Threading.Tasks;
using WoChat.Models;
using WoChat.Net;
using WoChat.Utils;

namespace WoChat.ViewModels {
    /// <summary>
    /// Retains references of data for binding, and provides methods to modify data.
    /// </summary>
    public class ChatViewModel : NotifyPropertyChangedBase {
        public ObservableCollection<ChatModel> Chats {
            get {
                return chats;
            }
            set {
                chats = value;
            }
        }

        public void Load() {
            isLoading = true;
            // TODO: To be removed. Only for debug
            ChatModel cm1 = new ChatModel();
            cm1.DisplayName = "AAA";
            chats.Add(cm1);
            ChatModel cm2 = new ChatModel();
            cm2.DisplayName = "BBB";
            chats.Add(cm2);
            isLoading = false;
        }

        /// <summary>
        /// Chat is stored locally so is no need to sync
        /// </summary>
        //public async void Sync() {
        //    await WaitUntilLoaded();
        //    HTTP.Get
        //}
        //
        //public async Task WaitUntilLoaded() {
        //    while (isLoading) ;
        //}

        private ObservableCollection<ChatModel> chats = new ObservableCollection<ChatModel>();
        private bool isLoading = false;
    }
}

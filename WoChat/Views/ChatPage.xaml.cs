using Windows.UI.Xaml.Controls;
using WoChat.ViewModels;

namespace WoChat.Views {
    public sealed partial class ChatPage : Page {
        private LocalUserViewModel LocalUserVM = App.LocalUserVM;

        public ChatPage() {
            this.InitializeComponent();
        }
    }
}

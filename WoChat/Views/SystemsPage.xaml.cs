using Windows.UI.Xaml.Controls;
using WoChat.ViewModels;

namespace WoChat.Views {
    public sealed partial class SystemsPage : Page {
        public SystemViewModel SystemVM {
            get {
                return systemVM;
            }
        }
        private SystemViewModel systemVM = App.AppVM.SystemVM;

        public SystemsPage() {
            InitializeComponent();
        }
    }
}

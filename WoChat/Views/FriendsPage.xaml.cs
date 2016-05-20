using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using WoChat.Models;
using WoChat.Net;
using WoChat.Utils;
using WoChat.ViewModels;

namespace WoChat.Views {
    public sealed partial class FriendsPage : Page {
        private FriendsPageUIViewModel UIVM = new FriendsPageUIViewModel();

        public ContactViewModel ContactVM {
            get {
                return contactVM;
            }
        }

        public FriendsPage() {
            InitializeComponent();
        }

        private ContactViewModel contactVM = App.AppVM.ContactVM;

        private void StartChatButton_Click(object sender, RoutedEventArgs e) {
            var rootFrame = Window.Current.Content as Frame;
            var mainPage = rootFrame.Content as MainPage;
            mainPage.StartChat((FriendsListView.SelectedItem as ContactModel).UserId, 0);
        }

        private async void AddFriend_Click(object sender, RoutedEventArgs e) {
            PostUsers_InvitationResult result = await HTTP.PostUsers_Invitation(App.AppVM.LocalUserVM.JWT, UIVM.SearchQuery, "交个朋友吧！");
            UIVM.SearchQuery = "";
            switch (result.StatusCode) {
                case PostUsers_InvitationResult.PostUsers_InvitationStatusCode.Success:
                    NotificationHelper.ShowToast("已发送邀请，请等待对方接受。");
                    break;
                case PostUsers_InvitationResult.PostUsers_InvitationStatusCode.NoThisUser:
                    NotificationHelper.ShowToast("该用户不存在！");
                    break;
                case PostUsers_InvitationResult.PostUsers_InvitationStatusCode.AlreadyContact:
                    NotificationHelper.ShowToast("你和TA已经是好友啦！");
                    break;
                default:
                    NotificationHelper.ShowToast("未知错误，请稍后重试！");
                    break;
            }
        }

        /// <summary>
        /// This event must be handled if want SearchQuery to be changed as soon as user input.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void SearchBox_TextChanging(TextBox sender, TextBoxTextChangingEventArgs args) {}
    }

    class FriendsPageUIViewModel : NotifyPropertyChangedBase {
        public string SearchQuery {
            get {
                return searchQuery;
            }
            set {
                searchQuery = value;
                OnPropertyChanged();
                ChatListVisibility = value.Trim().Equals("") ? Visibility.Visible : Visibility.Collapsed;
                SearchListVisibility = value.Trim().Equals("") ? Visibility.Collapsed : Visibility.Visible;
                AddButtonEnabled = value.Trim().Equals("") == false;
            }
        }
        public ObservableCollection<ContactModel> SearchList {
            get {
                return searchList;
            }
        }
        public Visibility ChatListVisibility {
            get {
                return chatListVisibility;
            }
            set {
                chatListVisibility = value;
                OnPropertyChanged();
            }
        }
        public Visibility SearchListVisibility {
            get {
                return searchListVisibility;
            }
            set {
                searchListVisibility = value;
                OnPropertyChanged();
            }
        }

        public bool AddButtonEnabled {
            get {
                return addButtonEnabled;
            }
            set {
                addButtonEnabled = value;
                OnPropertyChanged();
            }
        }

        private string searchQuery = "";
        private ObservableCollection<ContactModel> searchList = new ObservableCollection<ContactModel>();
        private Visibility chatListVisibility = Visibility.Visible;
        private Visibility searchListVisibility = Visibility.Collapsed;
        private bool addButtonEnabled = false;
    }
}

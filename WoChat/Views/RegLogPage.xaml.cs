using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using WoChat.Net;
using WoChat.ViewModels;

namespace WoChat.Views {
    public sealed partial class RegLogPage : Page {
        private RegLogPageUIViewModel RegLogPageUIVM = new RegLogPageUIViewModel();
        private LocalUserViewModel localUserViewModel = App.AppVM.LocalUserVM;

        public RegLogPage() {
            InitializeComponent();
            App.PushSocket.Dispatcher = Dispatcher;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e) {
            if (localUserViewModel.AlreadyLoggedIn) {
                UserAutoLoggedIn();
            }
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e) {
            if (RegLogPageUIVM.Password.Equals("") || RegLogPageUIVM.Username.Equals("")) {
                RegLogPageUIVM.HintText = "用户名或密码不能为空！";
                return;
            }
            RegLogPageUIVM.HintText = "正在注册，请稍候…";
            RegLogPageUIVM.IsLoading = true;
            PostUsersResult result = await HTTP.PostUsers(RegLogPageUIVM.Username, RegLogPageUIVM.Password);
            RegLogPageUIVM.IsLoading = false;
            switch (result.StatusCode) {
                case PostUsersResult.PostUsersStatusCode.Success:
                    RegLogPageUIVM.HintText = "";
                    PerformLogin();
                    break;
                case PostUsersResult.PostUsersStatusCode.InvalidParams:
                    RegLogPageUIVM.HintText = "用户名或密码不符合规范，请修改后重试！";
                    break;
                case PostUsersResult.PostUsersStatusCode.ExistingUser:
                    RegLogPageUIVM.HintText = "用户已存在，请登录！";
                    break;
                default:
                    RegLogPageUIVM.HintText = "未知错误，请稍后重试！";
                    break;
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e) {
            PerformLogin();
        }

        private async void PerformLogin() {
            if (RegLogPageUIVM.Password.Equals("") || RegLogPageUIVM.Username.Equals("")) {
                RegLogPageUIVM.HintText = "用户名或密码不能为空！";
                return;
            }
            RegLogPageUIVM.HintText = "正在登录，请稍候…";
            RegLogPageUIVM.IsLoading = true;
            PostAuthLoginResult result = await HTTP.PostAuthLogin(RegLogPageUIVM.Username, RegLogPageUIVM.Password);
            RegLogPageUIVM.IsLoading = false;
            switch (result.StatusCode) {
                case PostAuthLoginResult.PostAuthLoginStatusCode.Success:
                    RegLogPageUIVM.HintText = "";
                    localUserViewModel.UserLogIn(result.jwt);
                    UserAutoLoggedIn();
                    break;
                case PostAuthLoginResult.PostAuthLoginStatusCode.Failure:
                    RegLogPageUIVM.HintText = "用户名或密码错误，请重试！";
                    break;
                default:
                    RegLogPageUIVM.HintText = "未知错误，请稍后重试！";
                    break;
            }
        }

        private async void UserAutoLoggedIn() {
            RegLogPageUIVM.HintText = "正在登录，请稍候…";
            RegLogPageUIVM.IsLoading = true;
            App.PushSocket.Connect();
            App.AppVM.Load();
            App.AppVM.LocalUserVM.Sync();
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null) {
                await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                    rootFrame.Navigate(typeof(MainPage));
                });
            }
        }

        private void UsernameBox_KeyUp(object sender, KeyRoutedEventArgs e) {
            if (e.Key == Windows.System.VirtualKey.Enter) {
                PerformLogin();
            }
        }

        private void PasswordBox_KeyUp(object sender, KeyRoutedEventArgs e) {
            if (e.Key == Windows.System.VirtualKey.Enter) {
                PerformLogin();
            }
        }
    }

    class RegLogPageUIViewModel : INotifyPropertyChanged {
        public bool IsLoading {
            get {
                return isLoading;
            }
            set {
                isLoading = value;
                OnPropertyChanged();
            }
        }
        public bool AcceptInput {
            get {
                return !IsLoading;
            }
        }
        public string HintText {
            get {
                return hintText;
            }
            set {
                hintText = value;
                OnPropertyChanged();
            }
        }
        public string Username {
            get {
                return username;
            }
            set {
                username = value;
                OnPropertyChanged();
            }
        }
        public string Password {
            get {
                return password;
            }
            set {
                password = value;
                OnPropertyChanged();
            }
        }

        private bool isLoading = false;
        private string hintText = "";
        private string username = "";
        private string password = "";

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

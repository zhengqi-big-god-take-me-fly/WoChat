using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WoChat.Net;
using WoChat.ViewModels;

namespace WoChat.Views {
    public sealed partial class RegLogPage : Page {
        private RegLogPageUIViewModel RegLogPageUIVM = new RegLogPageUIViewModel();
        private LocalUserViewModel localUserViewModel = App.AppVM.LocalUserVM;
        //private AppViewModel AppVM = App.AppVM;
        //private LocalUserViewModel LocalUserVM = App.LocalUserVM;
        //private StubViewModel localUserVM = App.LocalUserVMOld;
        //private StubViewModel tester;
        //private bool isLogin;
        public RegLogPage() {
            InitializeComponent();
            if (localUserViewModel.AlreadyLoggedIn) {
                UserAutoLoggedIn();
            }
            //tester = new StubViewModel();
            //isLogin = false;
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
            if (RegLogPageUIVM.Password.Equals("") || RegLogPageUIVM.Username.Equals("")) {
                RegLogPageUIVM.HintText = "用户名或密码不能为空！";
                return;
            }
            PerformLogin();
        }

        private async void PerformLogin() {
            RegLogPageUIVM.HintText = "正在登录，请稍候…";
            RegLogPageUIVM.IsLoading = true;
            PostAuthLoginResult result = await HTTP.PostAuthLogin(RegLogPageUIVM.Username, RegLogPageUIVM.Password);
            RegLogPageUIVM.IsLoading = false;
            switch (result.StatusCode) {
                case PostAuthLoginResult.PostAuthLoginStatusCode.Success:
                    RegLogPageUIVM.HintText = "";
                    // TODO: Decode and store username and userId
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

        private void UserAutoLoggedIn() {
            RegLogPageUIVM.HintText = "正在登录，请稍候…";
            RegLogPageUIVM.IsLoading = true;
            App.PushSocket.Connect();
            //TODO: Retrieve new data from server

            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame != null) {
                rootFrame.Navigate(typeof(MainPage));
            }
        }

        /**
         *
         * -----------------------------------------------------------
         * ---------------------Updated 12th May----------------------
         * -------------------User Login Function---------------------
         * ---------------Return True if Login Success----------------
         * -------------Should Contact with Server(Future)------------
         * -----------------------Test Passed-------------------------
         * 
         */
        // public void userLogin(object sender, RoutedEventArgs e)
        //{
        //    if (userName == null || passWord == null || userName.Text == "" || passWord.Text == "")
        //    {
        //        var dd = new MessageDialog("请输入用户名或者密码!").ShowAsync();
        //    }
        //    else if (tester.login(userName.Text, passWord.Text))
        //    {
        //        isLogin = true;
        //        var c = tester.getCurrentUser();
        //        var b = new MessageDialog("登陆成功!\n用户名为：" + c.getName() + "\n呢称为：" + c.getNick() + "\nID为：" + c.getID() + "\n邮箱为：" + c.getInfo().email).ShowAsync();
        //    }
        //    else {
        //        var cc = new MessageDialog("用户名或者账号错误!").ShowAsync();
        //    }

        //}


        //public void userRegister(object sender, RoutedEventArgs e)
        //{
        //    // You should provide additional Features
        //    // Like Nicknames , Emails , Icon , Stylish or other Info
        //    string nickname = "Give a special Nickname";
        //    string emails = "WhyTheRegPageIs@SoStupid.ICanNotBear";
        //    string icon;
        //    string stylishSignature;
        //    if (tester.register(userName.Text, passWord.Text, nickname, emails))
        //    {
        //        var t = new MessageDialog("注册成功").ShowAsync();
        //    } else
        //    {
        //        var s = new MessageDialog("注册失败").ShowAsync();
        //    }
        //}
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

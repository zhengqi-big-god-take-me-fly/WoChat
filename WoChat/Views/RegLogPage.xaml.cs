using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WoChat.ViewModels;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace WoChat.Views {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class RegLogPage : Page {
        private StubViewModel tester;
        private bool isLogin;
        public RegLogPage() {
            this.InitializeComponent();
            tester = new StubViewModel();
            isLogin = false;
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
         public void userLogin(object sender, RoutedEventArgs e)
        {
            if (userName == null || passWord == null || userName.Text == "" || passWord.Text == "")
            {
                var dd = new MessageDialog("请输入用户名或者密码!").ShowAsync();
            }
            else if (tester.login(userName.Text, passWord.Text))
            {
                isLogin = true;
                var c = tester.getCurrentUser();
                var b = new MessageDialog("登陆成功!\n用户名为：" + c.getName() + "\n呢称为：" + c.getNick() + "\nID为：" + c.getID() + "\n邮箱为：" + c.getInfo().email).ShowAsync();
            }
            else {
                var cc = new MessageDialog("用户名或者账号错误!").ShowAsync();
            }

        }


        public void userRegister(object sender, RoutedEventArgs e)
        {
            // You should provide additional Features
            // Like Nicknames , Emails , Icon , Stylish or other Info
            string nickname = "Give a special Nickname";
            string emails = "WhyTheRegPageIs@SoStupid.ICanNotBear";
            string icon;
            string stylishSignature;
            if (tester.register(userName.Text, passWord.Text, nickname, emails))
            {
                var t = new MessageDialog("注册成功").ShowAsync();
            } else
            {
                var s = new MessageDialog("注册失败").ShowAsync();
            }
        }



    }
}

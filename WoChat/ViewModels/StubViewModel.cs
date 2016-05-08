using System;
using System.Collections.ObjectModel;
using WoChat.Models;

namespace WoChat.ViewModels {
    class StubViewModel {
        private UserModel currentUser;
        private bool isLogin;


        //本地化聊天数据和联系人/组别数据(只同步跟本用户有关的)
        private ObservableCollection<UserModel> friends;
        private ObservableCollection<GroupModel> groups;
        private ObservableCollection<ChatModel> chats;


        //聊天数据联系人、组别数据的getter
        public ObservableCollection<UserModel> getFriends()
        {
            return this.friends;
        }

        public ObservableCollection<GroupModel> getGroups()
        {
            return this.groups;
        }

        public ObservableCollection<ChatModel> getChats()
        {
            return this.chats;
        }


        public UserModel getCurrentUser()
        {
            return this.currentUser;
        }





        public bool fetchCurrentUser()
        {

            return true;
        }


        public StubViewModel()
        {
            this.currentUser = null;
            isLogin = false;
        }
    }
}

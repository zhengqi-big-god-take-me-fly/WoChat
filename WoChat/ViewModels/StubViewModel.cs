using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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



        // 数据的初始化构造器
        private bool initDatas()
        {
            if (!isLogin) return false;
            // 添加朋友
            List<string> friendList = currentUser.getFriends();
            for (int i = 0; i < friendList.Count; i++)
            {
                this.friends.Add(fetchFriend(friendList.ElementAt(i)));
            }
            // 添加群
            List<string> groupList = currentUser.getGroups();
            for (int i = 0; i < groupList.Count; i++)
            {
                this.groups.Add(fetchGroup(groupList.ElementAt(i)));
            }
            // 添加聊天系列
            List<string> chatList = currentUser.getChats();
            for (int i = 0; i < chatList.Count; i++)
            {
                this.chats.Add(fetchChat(chatList.ElementAt(i)));
            }
            return true;
        }


        public bool fetchCurrentUser()
        {
            return true;
        }


        private UserModel fetchFriend(string fid)
        {
            return DataModel.getFriend(fid);
        }
        private GroupModel fetchGroup(string gid)
        {
            return DataModel.getGroup(gid);
        }
        private ChatModel fetchChat(string cid)
        {
            return DataModel.getChat(cid);
        }

        public StubViewModel()
        {
            this.currentUser = null;
            isLogin = false;
        }
    }
}

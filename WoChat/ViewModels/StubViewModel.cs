using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WoChat.Models;

namespace WoChat.ViewModels {
    class StubViewModel {
        private UserModel currentUser;
        private bool isLogin;


        /**
         * Init the Models Providing to the Page Renderer
         * Type: ObservableCollection
         * All models have the function for Getters and Setters
         */
        private ObservableCollection<UserModel> friends;
        private ObservableCollection<GroupModel> groups;
        private ObservableCollection<ChatModel> chats;



        /**
         * The 3 datas that return to the Page Renderer
         * just Open the Api for the Xaml Page cs files
         */
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


        /**
         * Functions for getting current Users
         * if User has been Logged in , then we return the basic Information for the User
         * For safety Respect , User Model didn't provide open Api For pasword Getters.
         */
        public UserModel getCurrentUser()
        {
            return this.currentUser;
        }


        
        /**
         * 
         */
        public bool login(string uname , string password)
        {
            if (isLogin) return false;
            this.currentUser = DataModel.userLogin(uname, password);
            if (this.currentUser == null) return false;
            isLogin = true;
            /**
             * 
             */
            return initDatas();
        }


        /**
         * [_icon description]
         * @type {String}
         */
        public bool register(string name, string password, string _nick, string _email, string _icon = "default", string _style = "None Yet!")
        {
            return DataModel.userRegister(name, password, _nick, _email, _icon = "default", _style = "None Yet!");
        }


        /**
         * 
         */
        public bool logout()
        {
            if (!isLogin) return false;
            this.isLogin = false;
            this.currentUser = null;
            return true;
        }

        

        /**
         *
         *
         *
         *
         *
         *
         *
         *
         * 
         */
        private bool initDatas()
        {
            if (!isLogin) return false;
            /**
             * [friendList description]
             * @type {[type]}
             */
            List<string> friendList = currentUser.getFriends();
            for (int i = 0; i < friendList.Count; i++)
            {
                this.friends.Add(fetchFriend(friendList.ElementAt(i)));
            }
            /**
             * [groupList description]
             * @type {[type]}
             */
            List<string> groupList = currentUser.getGroups();
            for (int i = 0; i < groupList.Count; i++)
            {
                this.groups.Add(fetchGroup(groupList.ElementAt(i)));
            }
            /**
             * [chatList description]
             * @type {[type]}
             */
            List<string> chatList = currentUser.getChats();
            for (int i = 0; i < chatList.Count; i++)
            {
                this.chats.Add(fetchChat(chatList.ElementAt(i)));
            }
            return true;
        }

        

        /**
         *
         *
         * 
         */
        public bool addFriend(string fid)
        {
            if (!this.isLogin) return false;
            if (DataModel.addFriend(this.currentUser.getID() , fid)) {
                /**
                 * 
                 */
                syncUserFriend();
                syncUserChat();
                return true;
            }
            return false;
        }



        /**
         *
         *
         * 
         */
        public bool removeFriend(string fid)
        {
            if (!this.isLogin) return false;
            bool res = DataModel.removeFriend(this.currentUser.getID(), fid);
            if (res)
            {
                /**
                 * 
                 */
                syncUserFriend();
                syncUserChat();
            }
            return res;
        }


        /**
         *  Override
         * 
         */
        public List<string> searchGroup(string gname)
        {
            return DataModel.searchGroups(gname);
        }

        

        /**
         * 
         *
         *
         *
         *
         *
         * 
         * @type {String}
         */
        public bool joinOrCreateGroup(string gname , string gid = "NULL")
        {
            if (!this.isLogin) return false;
            // Safe for a RUBBISH value of gid;
            bool res = DataModel.addToGroupByID(this.currentUser.getID() , gid , gname);
            if (res)
            {
                syncUserGroup();
                syncUserChat();
            }
            return res;
        }



        /**
         *
         *
         *
         *
         * 
         */
        public bool exitGroup(string gid)
        {
            if (!this.isLogin) return false;
            bool res = DataModel.quitGroupByID(this.currentUser.getID() , gid);
            if (res)
            {
                syncUserGroup();
                syncUserChat();
            }
            return res;
        }



        /**
         *
         *
         *
         * 
         */
        private void syncUserFriend()
        {
            this.currentUser.setFriend(DataModel.getFriendIDs(this.currentUser.getID()));
        }
        private void syncUserGroup()
        {
            this.currentUser.setGroup(DataModel. getGroupIDs(this.currentUser.getID()));
        }
        private void syncUserChat()
        {
            this.currentUser.setChat(DataModel.getChatIDs(this.currentUser.getID()));
        }



        //// 数据的初始化构造器
        //private bool initDatas()
        //{
        //    if (!isLogin) return false;
        //    // 添加朋友
        //    List<string> friendList = currentUser.getFriends();
        //    for (int i = 0; i < friendList.Count; i++)
        //    {
        //        this.friends.Add(fetchFriend(friendList.ElementAt(i)));
        //    }
        //    // 添加群
        //    List<string> groupList = currentUser.getGroups();
        //    for (int i = 0; i < groupList.Count; i++)
        //    {
        //        this.groups.Add(fetchGroup(groupList.ElementAt(i)));
        //    }
        //    // 添加聊天系列
        //    List<string> chatList = currentUser.getChats();
        //    for (int i = 0; i < chatList.Count; i++)
        //    {
        //        this.chats.Add(fetchChat(chatList.ElementAt(i)));
        //    }
        //    return true;
        //}


        /**
         *
         * 
         */
        public bool fetchCurrentUser()
        {
            return true;
        }



        /**
         *
         * 
         */
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



        /**
         *
         *
         *
         *
         * 
         */
        public StubViewModel()
        {
            this.currentUser = null;
            isLogin = false;
            //在通讯录显示的（离线数据）
            this.groups = new ObservableCollection<GroupModel>();
            this.friends = new ObservableCollection<UserModel>();
            this.chats = new ObservableCollection<ChatModel>();

        }
    }
}

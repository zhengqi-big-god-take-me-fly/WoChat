﻿using System;
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
         * --------------------------------------------------------
         * ------------------Updated 12th May----------------------
         * --------------Check if a user is addable----------------
         * --------------------------------------------------------
         */
        public bool checkAddability(string userid)
        {
            //isLogin false -> not Logged in
            if (!isLogin) return false;
            //if adding oneself then we not permit this method
            if (currentUser.getID() == userid) return false;
            return DataModel.isUserExist(userid);
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
         * ---------------------Modified 12th May------------------
         * ----------Search A User By Name and Return ID-----------
         * ---------Return The First Users Match Username----------
         * -----If no such user found , then we simply return------
         * ----------------------"NOTFOUND"------------------------
         * --------------------------------------------------------
         */
         public string searchUserByName(string username)
        {
            return DataModel.getUserIDByName(username);
        }




        /**
         * ------------------Update 12th May-----------------
         * ---------If add friend Successfully , then-------- 
         * ----Sync The Observation List and User himself----
         * --------------------------------------------------
         */
        public bool addFriend(string fid)
        {
            if (!this.isLogin) return false;
            if (DataModel.addFriend(this.currentUser.getID() , fid)) {
                /**
                 * Sync User's Friend String List(Update User)
                 */
                syncUserFriend();
                syncUserChat();
                /**
                 * Sync Local Friend Observation Collection Set
                 */
                syncLocalFriend(this.getCurrentUser().getFriends().ElementAt(this.getCurrentUser().getFriends().Count - 1));
                syncLocalChat(this.getCurrentUser().getChats().ElementAt(this.getCurrentUser().getChats().Count - 1));
                return true;
            }
            return false;
        }

        private void syncLocalFriend(string newFriendID)
        {
            UserModel newFriend = DataModel.getFriendObjectById(newFriendID);
            this.friends.Add(newFriend);
        }
        private void syncLocalGroup(string newGroupID)
        {
            GroupModel newGroup = DataModel.getGroupObjectById(newGroupID);
            this.groups.Add(newGroup);
        }
        private void syncLocalChat(string newChatID)
        {
            ChatModel newChat = DataModel.getChatObjectById(newChatID);
            this.chats.Add(newChat);
        }




        // Should Be Modified
        public ChatModel getChatByFriend(string fid)
        {
            if (!isLogin) return null;
            // If we Can find At Local
            // Certainly should be found at local!

            for (int i = 0; i < chats.Count; i++)
            {
                if (this.chats.ElementAt(i).getChaterID() == this.currentUser.getID() && this.chats.ElementAt(i).getChateeID() == fid)
                {
                    return this.chats.ElementAt(i);
                } else if (this.chats.ElementAt(i).getChaterID() == fid && this.chats.ElementAt(i).getChateeID() == this.currentUser.getID())
                {
                    return this.chats.ElementAt(i);
                }
            }
            return null;
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
            DataModel.init();
            //在通讯录显示的（离线数据）
            this.groups = new ObservableCollection<GroupModel>();
            this.friends = new ObservableCollection<UserModel>();
            this.chats = new ObservableCollection<ChatModel>();

        }
    }
}

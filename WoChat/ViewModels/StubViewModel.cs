using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Popups;
using WoChat.Models;

namespace WoChat.ViewModels {
    public class StubViewModel {
        private UserModelOld currentUser;
        private bool isLogin;


        /**
         * Init the Models Providing to the Page Renderer
         * Type: ObservableCollection
         * All models have the function for Getters and Setters
         */
        //private ObservableCollection<UserModel> friends;
        //private ObservableCollection<GroupModel> groups;
        //private ObservableCollection<ChatModel> chats;

        public ObservableCollection<FriendViewModel> friends;
        public ObservableCollection<GroupViewModel> groups;
        public ObservableCollection<ChatViewModel> chats;



        /**
         * The 3 datas that return to the Page Renderer
         * just Open the Api for the Xaml Page cs files
         */
        public ObservableCollection<FriendViewModel> getFriends()
        {
            return this.friends;
        }
       
        public ObservableCollection<GroupViewModel> getGroups()
        {
            return this.groups;
        }

        public ObservableCollection<ChatViewModel> getChats()
        {
            return this.chats;
        }


        /**
         * Functions for getting current Users
         * if User has been Logged in , then we return the basic Information for the User
         * For safety Respect , User Model didn't provide open Api For pasword Getters.
         */
        public UserModelOld getCurrentUser()
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


        public List<UserModelOld> showTestDatabases()
        {
            List<UserModelOld> myModel = DataModel.readAndCreateUsers();
            List<GroupModelOld> myGroups = DataModel.readAndCreateGroups();
            List<ChatModelOld> myChats = DataModel.readAndCreateChats();

            UserModelOld tempUser;
            GroupModelOld tempGroup;
            ChatModelOld tc;




            //for (int i = 0; i < myModel.Count; i++) {
            //    tempUser = myModel.ElementAt(i);
            //    this.friends.Add(new FriendViewModel(tempUser.getID(), tempUser.getName(), tempUser.getIcon(), tempUser.getStyle(), tempUser.getInfo().email, ""));

            //}
            //for (int i = 0; i < myChats.Count; i++) {
            //    tc = myChats.ElementAt(i);
            //    List<MessageModel> myModel = tc.getChat();
            //    List<MessageViewModel> myView = new List<MessageViewModel();
            //    for (int ii = 0; ii < )
            //    this.chats.Add(new ChatViewModel(tc.getID() , tc.getChaterID() , tc.getChateeID() , tc.getChaterName() , tc.getChateeName() ,tc.getGroupChatFlag() , tc.));

            //}


            return myModel;
        } 



        /**
         * 
         */
        //public bool login(string uname , string password)
        //{
        //    //if (isLogin) return false;
        //    //this.currentUser = DataModel.userLogin(uname, password);
        //    //if (this.currentUser == null) return false;
        //    //isLogin = true;
        //    /**
        //     * 
        //     */
        //    return initDatas();
        //}


        /**
         * [_icon description]
         * @type {String}
         */
        public bool register(string name, string password, string _nick, string _email, string _icon = "default", string _style = "None Yet!")
        {
            return DataModel.userRegister(name, password, _nick, _email, _icon = "default", _style = "None Yet!");
        }


        /**
         * Log Out User
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
         * Fetch 3 Lists from server (Models)
         * And then update Local ObservableCollection
         * 
         */
        //private bool initDatas()
        //{
        //    if (!isLogin) return false;
        //    /**
        //     * [friendList description]
        //     * @type {[type]}
        //     */
        //    List<string> friendList = currentUser.getFriends();
        //    UserModelOld tempUserModel;
        //    string currentChatID;
        //    for (int i = 0; i < friendList.Count; i++)
        //    {
        //        tempUserModel = fetchFriend(friendList.ElementAt(i));
        //        currentChatID = getChatByFriend(tempUserModel.getID()).chatID;
        //        this.friends.Add(new FriendViewModel(tempUserModel.getID(), tempUserModel.getName(), tempUserModel.getInfo().icon , tempUserModel.getInfo().stylish , tempUserModel.getInfo().email , currentChatID));
        //    }
        //    /**
        //     * [groupList description]
        //     * @type {[type]}
        //     */
        //    List<string> groupList = currentUser.getGroups();
        //    GroupModelOld tempGroupModel;
        //    for (int i = 0; i < groupList.Count; i++)
        //    {
        //        tempGroupModel = fetchGroup(groupList.ElementAt(i));
        //        this.groups.Add(new GroupViewModel(tempGroupModel.getName() , tempGroupModel.getID() , tempGroupModel.getChatID() ,  tempGroupModel.getMembers().ElementAt(0) , tempGroupModel.getMembers()));
        //    }
        //    /**
        //     * [chatList description]
        //     * @type {[type]}
        //     */
        //    List<string> chatList = currentUser.getChats();
        //    ChatModelOld tempChatModel;
        //    List<MessageViewModel> tempMessage;
        //    List<MessageModelOld> msgModel;
        //    MessageModelOld helper;
        //    for (int i = 0; i < chatList.Count; i++)
        //    {
        //        tempChatModel = fetchChat(chatList.ElementAt(i));
        //        tempMessage = new List<MessageViewModel>();
        //        msgModel = tempChatModel.getChat();
        //        for (int ii = 0; ii < msgModel.Count; ii++)
        //        {
        //            helper = msgModel.ElementAt(ii);
        //            tempMessage.Add(new MessageViewModel(helper.getSenderID(), helper.getSender(), helper.getReceiverID(), helper.getReceiver(), helper.getContent(), helper.getTime()));
        //        }
        //        this.chats.Add(new ChatViewModel(tempChatModel.getID() , tempChatModel.getChaterID() , tempChatModel.getChateeID() , tempChatModel.getChaterName() , tempChatModel.getChateeName() ,tempChatModel.getGroupChatFlag() , tempMessage));
        //    }
        //    return true;
        //}


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
        //public bool addFriend(string fid)
        //{
        //    if (!this.isLogin) return false;
        //    if (DataModel.addFriend(this.currentUser.getID() , fid)) {
        //        /**
        //         * Sync User's Friend String List(Update User)
        //         */
        //        syncUserFriend();
        //        syncUserChat();
        //        /**
        //         * Sync Local Friend Observation Collection Set
        //         */
        //        syncLocalFriend(this.getCurrentUser().getFriends().ElementAt(this.getCurrentUser().getFriends().Count - 1));
        //        syncLocalChat(this.getCurrentUser().getChats().ElementAt(this.getCurrentUser().getChats().Count - 1));
        //        return true;
        //    }
        //    return false;
        //}

        //private void syncLocalFriend(string newFriendID)
        //{
        //    UserModelOld newFriend = DataModel.getFriendObjectById(newFriendID);
        //    FriendViewModel friend = new FriendViewModel(newFriendID, newFriend.getName(), newFriend.getIcon(), newFriend.getStyle() ,newFriend.getInfo().email,getChatByFriend(newFriendID).chatID);
        //    this.friends.Add(friend);
        //}
        //private void syncLocalGroup(string newGroupID)
        //{
        //    GroupModelOld newGroup = DataModel.getGroupObjectById(newGroupID);
        //    GroupViewModel group = new GroupViewModel(newGroup.getName(), newGroupID, newGroup.getChatID(), newGroup.getMembers().ElementAt(0), newGroup.getMembers());
        //    this.groups.Add(group);
        //}
        //private void syncLocalChat(string newChatID)
        //{
        //    ChatModelOld newChat = DataModel.getChatObjectById(newChatID);
        //    List<MessageViewModel> tempMessage = new List<MessageViewModel>();
        //    List<MessageModelOld> msgModel = newChat.getChat();
        //    MessageModelOld helper;
        //    for (int i = 0; i < msgModel.Count; i++)
        //    {
        //        helper = msgModel.ElementAt(i);
        //        tempMessage.Add(new MessageViewModel(helper.getSenderID() , helper.getSender() , helper.getReceiverID() , helper.getReceiver() , helper.getContent() , helper.getTime()));
        //    }
        //    ChatViewModel chat = new ChatViewModel(newChatID, newChat.getChaterID(), newChat.getChateeID(), newChat.getChaterName(), newChat.getChateeName(), newChat.getGroupChatFlag(), tempMessage);
        //    this.chats.Add(chat);
        //}




        // Should Be Modified
        //public ChatViewModel getChatByFriend(string fid)
        //{
        //    if (!isLogin) return null;
        //    // If we Can find At Local
        //    // Certainly should be found at local!
        //    // A chat will be removed only when we delete a friend
        //    for (int i = 0; i < chats.Count; i++)
        //    {
        //        if (this.chats.ElementAt(i).hostID == this.currentUser.getID() && this.chats.ElementAt(i).participantID == fid)
        //        {
        //            return this.chats.ElementAt(i);
        //        } else if (this.chats.ElementAt(i).hostID == fid && this.chats.ElementAt(i).participantID == this.currentUser.getID())
        //        {
        //            return this.chats.ElementAt(i);
        //        }
        //    }
        //    return null;
        //}





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
        private UserModelOld fetchFriend(string fid)
        {
            return DataModel.getFriend(fid);
        }
        private GroupModelOld fetchGroup(string gid)
        {
            return DataModel.getGroup(gid);
        }
        private ChatModelOld fetchChat(string cid)
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
            this.groups = new ObservableCollection<GroupViewModel>();
            this.friends = new ObservableCollection<FriendViewModel>();
            this.chats = new ObservableCollection<ChatViewModel>();

        }
    }
}

using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.UI.Popups;

namespace WoChat.Models
{

    /**
     * The Data Processing Models
     */
    class DataModel
    {

        /**
         * The "Database Tables" of Users , Groups and Chats
         * @type {List}
         */
        private static SQLiteConnection syncConnection;
        private static List<UserModelOld> users = new List<UserModelOld>();
        private static List<GroupModelOld> groups = new List<GroupModelOld>();
        private static List<ChatModelOld> chats = new List<ChatModelOld>();




        /**
         * We use all kinds of Static items,so we no need constructers
         */
        public DataModel() {

        }





        /**
         * Read From Database
         * Sync for Local UserDatas
         */
        private static bool loadLocalDatabases()
        {
            if (syncConnection == null)
            {
                syncConnection = new SQLiteConnection("wochat.db");
            }

            loadFriends();
            loadGroups();
            loadChats();
            loadRelations();
            return true;
        }




        //Database Load functions
        private static void loadFriends()
        {
            // Get a reference to the SQLite database
            //string sql = @"CREATE TABLE IF NOT EXISTS
            //               Users   (id  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, 
            //                        uid     VARCHAR( 140 ), 
            //                        name    VARCHAR( 140 ), 
            //                        pass    VARCHAR( 1024 ), 
            //                        nick    VARCHAR( 140 ), 
            //                        icon    VARCHAR( 140 ), 
            //                        style   VARCHAR( 140 ), 
            //                        email   VARCHAR( 140 ) 
            //                        );";
            string sql = "CREATE TABLE IF NOT EXISTS Users   (id  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, uid     VARCHAR( 140 ), name    VARCHAR( 140 ), pass    VARCHAR( 1024 ), nick    VARCHAR( 140 ), icon    VARCHAR( 140 ), style   VARCHAR( 140 ), email   VARCHAR( 140 ) );";
            using (var statement = syncConnection.Prepare(sql))
            {
                statement.Step();
            }

        }
        private static void loadGroups()
        {
            //string sql = @"CREATE TABLE IF NOT EXISTS
            //               Groups   (id     INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
            //                        gid     VARCHAR( 140 ),
            //                        gname   VARCHAR( 140 ),
            //                        admin   VARCHAR( 1024 ),
            //                        icon    VARCHAR( 140 ),
            //                        style   VARCHAR( 140 ),
            //                        chat    VARCHAR( 140 ）
            //                        );";
            string sql = "CREATE TABLE IF NOT EXISTS Groups   (id     INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, gid     VARCHAR( 140 ), gname   VARCHAR( 140 ), admin   VARCHAR( 1024 ), icon    VARCHAR( 140 ), style   VARCHAR( 140 ), chat    VARCHAR( 140 ));";
            using (var statement = syncConnection.Prepare(sql))
            {
                statement.Step();
            }
        }
        private static void loadChats()
        {
            //string sql = @"CREATE TABLE IF NOT EXISTS
            //               Chats   (id      INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
            //                        cid     VARCHAR( 140 ),
            //                        host    VARCHAR( 140 ),
            //                        part    VARCHAR( 140 ),
            //                        hostID  VARCHAR( 140 ),
            //                        partID  VARCHAR( 140 ),
            //                        message VARCHAR( 140 ),
            //                        isGroup VARCHAR( 140 )
            //                        );";
            string sql = "CREATE TABLE IF NOT EXISTS Chats   (id      INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, cid     VARCHAR( 140 ), host    VARCHAR( 140 ), part    VARCHAR( 140 ), hostID  VARCHAR( 140 ), partID  VARCHAR( 140 ), message VARCHAR( 140 ), isGroup VARCHAR( 140 ) );";
            using (var statement = syncConnection.Prepare(sql))
            {
                statement.Step();
            }
        }
        private static void loadRelations()
        {

            //string sqlFriend = @"CREATE TABLE IF NOT EXISTS
            //               FriendRelation   (id     INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
            //                                aid     VARCHAR( 140 ),
            //                                bid     VARCHAR( 140 ),
            //                                chat    VARCHAR( 140 ),
            //                        );";
            //string sqlGrouper = @"CREATE TABLE IF NOT EXISTS
            //               GroupRelation   (id      INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
            //                                gid     VARCHAR( 140 ),
            //                                pid     VARCHAR( 140 ),
            //                                chat    VARCHAR( 140 ),
            //                        );";
            //string sqlMessage = @"CREATE TABLE IF NOT EXISTS
            //               MsgRelation   (id      INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
            //                              mid     VARCHAR( 140 ),
            //                              cid     VARCHAR( 140 ),
            //                              sid     VARCHAR( 140 ),
            //                              rid     VARCHAR( 140 ),
            //                              message VARCHAR( 2048 ),
            //                              time    VARCHAR( 140 ),
            //                        );";

            string sqlFriend = "CREATE TABLE IF NOT EXISTS FriendRelation   (id     INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, aid     VARCHAR( 140 ), bid     VARCHAR( 140 ), chat    VARCHAR( 140 ));";
            string sqlGrouper = "CREATE TABLE IF NOT EXISTS GroupRelation   (id      INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, gid     VARCHAR( 140 ), pid     VARCHAR( 140 ), chat    VARCHAR( 140 ));";
            string sqlMessage = "CREATE TABLE IF NOT EXISTS MsgRelation   (id      INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, mid     VARCHAR( 140 ), cid     VARCHAR( 140 ), sid     VARCHAR( 140 ), rid     VARCHAR( 140 ), message VARCHAR( 2048 ), time    VARCHAR( 140 ));";
            using (var statementFriend = syncConnection.Prepare(sqlFriend)) {
                statementFriend.Step();
                using (var statementGrouper = syncConnection.Prepare(sqlGrouper)) {
                    statementGrouper.Step();
                    using (var statementMessage = syncConnection.Prepare(sqlMessage)) {
                        statementMessage.Step();
                    }
                }
            }
        }

        //public async static List<string> readAndProcessStringLists(string uid , string sqlSentence , int requirePosition)
        //{
        //    List<string> ret = new List<string>();
        //    using (var process = syncConnection.Prepare(sqlSentence))
        //    {
        //        process.Bind(1, uid);
        //        while (process.Step() == SQLiteResult.DONE)
        //        {
        //            ret.Add((string)process[requirePosition]);
        //        }
        //    }
        //    return ret;
        //}

        public static List<UserModelOld> readAndCreateUsers()
        {
            List<UserModelOld> userList = new List<UserModelOld>();
            List<string> friendList;
            List<string> groupList;
            List<string> chatList;

            UserModelOld temp;
            string usersql = "SELECT * FROM Users";
            string friendRelationSQL = "SELECT * FROM FriendRelation WHERE aid = ?";
            string friendRelationSQL2 = "SELECT * FROM FriendRelation WHERE bid = ?";
            string groupRelationSQL = "SELECT * FROM GroupRelation WHERE pid = ?";
            // ChatRelation Is Ignored Since we only need to add Chat IDs.(fetch from FriendRelation and GroupRelation)


            
            // Read From Database
            using (var getBareUser = syncConnection.Prepare(usersql))
            {
                while (getBareUser.Step() == SQLiteResult.ROW)
                {
                    //Initialize the 3 string Lists
                    friendList = new List<string>();
                    groupList = new List<string>();
                    chatList = new List<string>();

                    // Find his friend IDs(As host)
                    using (var getFriendIDStringList1 = syncConnection.Prepare(friendRelationSQL))
                    {
                        getFriendIDStringList1.Bind(1, (string)getBareUser[1]);
                        // Bind the User id as host
                        while (getFriendIDStringList1.Step() == SQLiteResult.ROW)
                        {
                            //Add participant id into Friend(id) List
                            friendList.Add((string)getFriendIDStringList1[2]);
                            //Add chat id into Chat(id) list
                            chatList.Add((string)getFriendIDStringList1[3]);
                        }
                        // returns the Relation if the relation contains him as host
                    }

                    using (var getFriendIDStringList2 = syncConnection.Prepare(friendRelationSQL2))
                    {
                        getFriendIDStringList2.Bind(1, (string)getBareUser[1]);
                        // Bind the User id as Participant
                        while (getFriendIDStringList2.Step() == SQLiteResult.ROW)
                        {
                            //Add host id into Friend(id) List
                            friendList.Add((string)getFriendIDStringList2[1]);
                            //Add chat id into Chat(id) list
                            chatList.Add((string)getFriendIDStringList2[3]);
                        }
                        // returns the Relation if the relation contains him as Participant
                    }

                    using (var getGroupList = syncConnection.Prepare(groupRelationSQL))
                    {
                        getGroupList.Bind(1, (string)getBareUser[1]);
                        while (getGroupList.Step() == SQLiteResult.ROW)
                        {
                            groupList.Add((string)getGroupList[1]);
                            chatList.Add((string)getGroupList[3]);
                        }
                    }

                    temp = new UserModelOld((string)getBareUser[2], (string)getBareUser[3], (string)getBareUser[4], (string)getBareUser[7], (string)getBareUser[5], (string)getBareUser[6]);
                    temp.resetID((string)getBareUser[1]);
                    temp.setFriend(friendList);
                    temp.setGroup(groupList);
                    temp.setChat(chatList);

                    userList.Add(temp);

                   // Here we get a bare user we still need to get his/her friend Lists
                }
            }
            return userList;
        }


        public static List<GroupModelOld> readAndCreateGroups()
        {
            List<GroupModelOld> groupList = new List<GroupModelOld>();
            List<string> memberList;
            GroupModelOld temp;
            string tmpcid;

            string sql = "SELECT * FROM Groups";
            string sqlGrouper = "SELECT * FROM GroupRelation where gid = ?";
            using (var getBareGroup = syncConnection.Prepare(sql))
            {
                while (getBareGroup.Step() == SQLiteResult.ROW)
                {
                    memberList = new List<string>();
                    tmpcid = "NULL";
                    // This Is the Group We want;
                    // Therefore we get the gid
                    using (var getGrouper = syncConnection.Prepare(sqlGrouper))
                    {
                        getGrouper.Bind(1, (string)getBareGroup[1]);
                        while (getGrouper.Step() == SQLiteResult.ROW)
                        {
                            memberList.Add((string)getGrouper[2]);
                            if (tmpcid == "NULL")
                            {
                                tmpcid = (string)getGrouper[3];
                            }
                        }
                    }
                    temp = new GroupModelOld((string)getBareGroup[3] , (string)getBareGroup[3] , (string)getBareGroup[2] , tmpcid , (string)getBareGroup[4] , (string)getBareGroup[5]);
                    temp.resetID((string)getBareGroup[1]);
                    temp.setMember(memberList);
                    groupList.Add(temp);
                }
            }
            return groupList;
        }


        public static List<ChatModelOld> readAndCreateChats()
        {
            List<ChatModelOld> chatList = new List<ChatModelOld>();
            ChatModelOld temp;
            List<MessageModelOld> messages;
            MessageModelOld singleMessage;
            string cid;


            string sql = "SELECT * FROM Chats";
            string sqlMessageList = "SELECT * From MsgRelation WHERE cid = ?";

            using (var getChat = syncConnection.Prepare(sql))
            {
                while (getChat.Step() == SQLiteResult.ROW)
                {
                    // Now we Got this Chat record;
                    cid = (string)getChat[1];
                    messages = new List<MessageModelOld>();
                    bool reference = (string)getChat[7] == "true" ? true : false;

                    using (var getMessages = syncConnection.Prepare(sqlMessageList))
                    {
                        getMessages.Bind(1, cid);
                        while (getMessages.Step() == SQLiteResult.ROW)
                        {
                            
                            // Now we can safelyCreate the Messages!
                            singleMessage = new MessageModelOld((string)getMessages[3], (string)getMessages[4], (string)getMessages[5], cid , reference);
                            singleMessage.setSendingTime((string)getMessages[6]);
                            // Add to MessageList
                            messages.Add(singleMessage);
                        }
                    }
                    // Now adding Completed , we can create ChatModel
                    temp = new ChatModelOld((string)getChat[2], (string)getChat[4], (string)getChat[3], (string)getChat[5], reference);
                    // Need To Modify ID!!!!!!
                    temp.resetID(cid);

                    temp.setChat(messages);
                    chatList.Add(temp);
                }
            }
            return chatList;
        }



        //ADD To DATABASE;
        private static bool addNewUserToDatabase(string uid , int index = -1)
        {
            if (index == -1) index = getUserIndexByID(uid);
            if (index == -1) return false;

            UserModelOld temp = users.ElementAt(index);
            string sql = "INSERT INTO Users (id , uid , name , pass , nick , icon , style , email) VALUES (? , ? , ? , ? , ? , ? , ? , ?)";
            try {
                using (var addUser = syncConnection.Prepare(sql))
                {
                    addUser.Bind(1, null);
                    addUser.Bind(2, temp.getID());
                    addUser.Bind(3, temp.getName());
                    addUser.Bind(4, temp.storePassword());
                    addUser.Bind(5, temp.getNick());
                    addUser.Bind(6, temp.getIcon());
                    addUser.Bind(7, temp.getStyle());
                    addUser.Bind(8, temp.getInfo().email);

                    // Ok We Store This Record!
                    addUser.Step();
                }

                return true;
            } catch(Exception ex)
            {
                return false;
            }
        }

        private static bool addNewGroupToDatabase(string gid, int index = -1)
        {
            if (index == -1) index = getGroupIndexByID(gid);
            if (index == -1) return false;
            // Start
            GroupModelOld gm = groups.ElementAt(index);
            string sql = "INSERT INTO Groups (id , gid , gname , admin , icon , style , chat) VALUES (? , ? , ? , ? , ? , ? , ?)";
            try
            {
                using (var addGroup = syncConnection.Prepare(sql))
                {
                    addGroup.Bind(1, null);
                    addGroup.Bind(2, gm.getID());
                    addGroup.Bind(3, gm.getName());
                    addGroup.Bind(4, gm.getAdmin());
                    addGroup.Bind(5, gm.getInfo().icon);
                    addGroup.Bind(6, gm.getInfo().stylish);
                    addGroup.Bind(7, gm.getChatID());
                    addGroup.Step();
                }
                return true;
            } catch (Exception e)
            {
                return false;
            }
        }


        private static bool addNewChatToDatabase(string cid, int index = -1)
        {
            if (index == -1) index = getChatIndexByID(cid);
            if (index == -1) return false;
            // Start
            ChatModelOld cm = chats.ElementAt(index);
            string sql = "INSERT INTO Chats (id , cid , host , part , hostID , partID , message , isGroup) VALUES (? , ? , ? , ? , ? , ? , ? , ?)";
            try
            {
                using (var addChat = syncConnection.Prepare(sql))
                {
                    addChat.Bind(1, null);
                    addChat.Bind(2, cm.getID());
                    addChat.Bind(3, cm.getChaterName());
                    addChat.Bind(4, cm.getChateeName());
                    addChat.Bind(5, cm.getChaterID());
                    addChat.Bind(6, cm.getChateeID());
                    addChat.Bind(7, cm.getGroupID());
                    addChat.Bind(8 , cm.getGroupChatFlag() == false ? "false" : "true");
                    addChat.Step();
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }



        // ADD RELATIONS
        // Must Provide MyChatID
        private static bool addNewFriendRelationToDataBase(string uid , string fid, string myChatID)
        {
            string sql = "INSERT INTO FriendRelation (id , aid , bid , chat) VALUES (? , ? , ? , ?)";
            try
            {
                using (var newFriend = syncConnection.Prepare(sql))
                {
                    newFriend.Bind(1, null);
                    newFriend.Bind(2, uid);
                    newFriend.Bind(3, fid);
                    newFriend.Bind(4, myChatID);
                    newFriend.Step();
                }
                return true;
            } catch(Exception e)
            {
                return false;
            }
        }

        private static bool addNewGroupRelationToDataBase(string uid, string gid, string groupChatID)
        {
            string sql = "INSERT INTO GroupRelation (id , gid , pid , chat) VALUES (? , ? , ? , ?)";
            try
            {
                using (var newGroup = syncConnection.Prepare(sql))
                {
                    newGroup.Bind(1, null);
                    newGroup.Bind(2, gid);
                    newGroup.Bind(3, uid);
                    newGroup.Bind(4, groupChatID);
                    newGroup.Step();
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private static bool addNewMsgRelationToDataBase(string uid, string fid, string chatid , string message , string time)
        {
            string sql = "INSERT INTO ChatRelation (id , mid , cid , sid , rid , message , time) VALUES (? , ? , ? , ? , ? , ? , ?)";
            try
            {
                using (var newMessage = syncConnection.Prepare(sql))
                {
                    newMessage.Bind(1, null);
                    newMessage.Bind(2, null);
                    newMessage.Bind(3, chatid);
                    newMessage.Bind(4, uid);
                    newMessage.Bind(5, fid);
                    newMessage.Bind(6, message);
                    newMessage.Bind(7, time);
                    newMessage.Step();
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }





        // Remove From Database
        
        //Cannot Remove A Certain User/Group Or Chat
        //Just Provide Removal of Relations
        private static bool removeFriendRelationFromDataBase(string uid, string fid)
        {
            string sql1 = "SELECT * FROM FriendRelation WHERE aid = ? AND bid = ?";
            string remove1 = "DELETE FROM FriendRelation WHERE chat = ?";
            string remove2 = "DELETE FROM Chats WHERE cid = ?";
            string chatids = "NULL";
            try
            {
                using (var removeFriend = syncConnection.Prepare(sql1))
                {
                    removeFriend.Bind(1, uid);
                    removeFriend.Bind(2, fid);
                    if (removeFriend.Step() != SQLiteResult.ROW)
                    {
                        removeFriend.Bind(1, fid);
                        removeFriend.Bind(2, uid);
                        if (removeFriend.Step() != SQLiteResult.ROW)
                        {
                            // Cannot find current Users
                            return false;
                        }
                        chatids = (string)removeFriend[3];
                    }
                    chatids = (string)removeFriend[3];

                    using (var deleteFriend = syncConnection.Prepare(remove1))
                    {
                        deleteFriend.Bind(1, chatids);
                        deleteFriend.Step();
                    }
                    using (var deleteChat = syncConnection.Prepare(remove2))
                    {
                        deleteChat.Bind(1, chatids);
                        deleteChat.Step();
                    }
                    return true;
                }
            } catch(Exception e)
            {
                return false;
            }
        }

        private static bool removeGroupRelationFromDatabase(string uid , string gid)
        {
            string remove1 = "DELETE FROM GroupRelation WHERE uid = ? AND gid = ?";
            try
            {
                using (var removeGroup = syncConnection.Prepare(remove1))
                {
                    removeGroup.Bind(1, uid);
                    removeGroup.Bind(2, gid);
                    removeGroup.Step();
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        // Modified To Database
        // Not Permotted to Update A Friend/Group/Chat At This Version




        // Query From Database(Not doing now Because we don't need)













        /**
         * Initialize of the "Databases"
         * Needed to Sync with Server
         */
        public static bool init()
        {
            /**
             * ---------------------Modified 12th May------------------
             * ---------Test Page , We Simply add a test User----------
             * --------------------------------------------------------
             */


            loadLocalDatabases();
            //UserModel testUser = new UserModel("a20185", encryptCreator("52013142"), "Souler" , "ou@souler.me");
            //UserModel testUser2 = new UserModel("tidyzq", encryptCreator("zqdashen"), "BigGod Qi Zheng", "tidyzq@tidyzq.com");
            //UserModel testUser3 = new UserModel("perqin", encryptCreator("changlaoshi"), "Teacher Aoi", "perqin@perqin.com");
            //users.Add(testUser);
            //users.Add(testUser2);
            //users.Add(testUser3);

            users = readAndCreateUsers();
            groups = readAndCreateGroups();
            chats = readAndCreateChats();



            /**
             * ---------------------Modified 12th May------------------
             * ---------Test Page , We Simply add a TestGroup----------
             * --------------------------------------------------------
             */
            //ChatModel godZheng = new ChatModel(testUser2.getID(), true);
            //chats.Add(godZheng);
            //GroupModel zhengqi_big_god_carries_me_fly = new GroupModel(testUser2.getID(), testUser2.getName(), "Fly", godZheng.getID());
            //groups.Add(zhengqi_big_god_carries_me_fly);

            return true;
        }



        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------NEEDS TO CHANGEBY HTTP MODULE !!!!--------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        /**
         * Login Module
         * return a Specified Model of User.
         * (To the Client)
         *
         * If no such user
         * Then return null
         */
        public static UserModelOld userLogin(string username, string password) {
            UserModelOld uml = null;
            int index = getUserIndexByName(username);
            if (index != -1 && users.ElementAt(index).comparePassword(encryptCreator(password))) {
                uml = users.ElementAt(index);

                // if not found the user from local datatbase , then we fetch the users from the remote
                // Server and then add to local databases

                addNewUserToDatabase(uml.getID(), index);

            }



            return uml;
        }


        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------NEEDS TO CHANGEBY HTTP MODULE !!!!--------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /**
         * User Register Module
         * return true if Create User Success.
         *
         * --------------Updated 11th May----------------------
         * We now use the Server(DataModel) For Encryption
         * Therefore encryptCreator before Create a user is needed
         * ----------------------------------------------------
         *
         * @type {String}
         */
        public static bool userRegister(string name, string password, string _nick, string _email, string _icon = "default", string _style = "None Yet!") {
            /**
             * Query First
             */
            if (searchForUser(name, _nick, _email) != "Pass!") return false;
            UserModelOld um = new UserModelOld(name, encryptCreator(password), _nick, _email, _icon, _style);
            /**
             * Append to user database
             */
            users.Add(um);

            // Add to database
            addNewUserToDatabase(um.getID(), users.Count - 1);
            return true;
        }



        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------NEEDS TO CHANGEBY HTTP MODULE !!!!--------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /**
         * --------------Updated 11th May----------------------
         * User Change Password
         * return true if Change Success
         *
         * We now use the Server(DataModel) For Encryption
         * Therefore encryptCreator before change a password is needed
         * ----------------------------------------------------
         *
         * @type {String}
         */
        public bool updatePassword(string username, string originalPassword, string newPassword) {
            int index = getUserIndexByName(username);
            if (index == -1) return false;
            else return users.ElementAt(index).changePassword(encryptCreator(originalPassword), encryptCreator(newPassword));
        }



        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------NEEDS TO CHANGEBY HTTP MODULE !!!!--------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /**
         * User Register Searcher
         * Judge if the User term is duplicate
         * return a warning Message
         */
        public static string searchForUser(string name, string _nick, string _email) {
            string warningMessage = "";
            for (int i = 0; i < users.Count; i++) {
                if (users.ElementAt(i).getName() == name) {
                    warningMessage += "\n该用户名已经被占用，无法使用。\n";
                }

                if (users.ElementAt(i).getInfo().email == _email) {
                    warningMessage += "\n该邮件已经被占用，无法使用。\n";
                }

                if (users.ElementAt(i).getInfo().nickname == _nick) {
                    warningMessage += "\n该呢称已经被占用，无法使用。\n";
                }
            }
            if (warningMessage == "") {
                warningMessage = "Pass!";
            }
            return warningMessage;
        }





        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------NEEDS TO CHANGEBY HTTP MODULE !!!!--------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //public UserModel fetchUserFromServer(string uname) {
        //    // Get user From server
        //    // And then Push In Local Array, and Save in Database
        //    // And then return
        //}
        //public GroupModel fetchGroupFromServer(string groupName) {
        //    //Get Group From Server
        //    // And Then Push in Local Array and save in database
        //    //ant then return
        //}
        //public GroupModel fetchChatFromServer(string ChatName) {
        //    //Get Chat From Server
        //    // And Then Push in Local Array and save in database
        //    //ant then return
        //}



        /**
         * --------------------------------------------------------
         * ------------------Updated 12th May----------------------
         * ----------Return User ID by Finding his name------------
         * --------------------------------------------------------
         */
        public static string getUserIDByName(string username)
        {
            int index = getUserIndexByName(username);
            if (index != -1)
            {
                return users.ElementAt(index).getID();
            }
            return "NOTFOUND";
            // Get a user from server
            //return fetchUserFromServer().getID();
        }
        public static string getGroupIDByName(string groupname)
        {
            int index = getGroupIndexByName(groupname);
            if (index != -1)
            {
                return users.ElementAt(index).getID();
            }
            return "NOTFOUND";
            // Get a Group From Server
            // return fetchGroupFromServer().getID();
        }


        /**
         * Index Getters for users
         * To get the Specified index from the database
         */
        private static int getUserIndexByID(string uid)
        {
            int ret = -1;
            for (int i = 0; i < users.Count; i++)
            {
                if (users.ElementAt(i).getID() == uid)
                {
                    ret = i;
                    break;
                }
            }
            return ret;
        }

        private static int getUserIndexByName(string uname)
        {
            int ret = -1;
            for (int i = 0; i < users.Count; i++)
            {
                if (users.ElementAt(i).getName() == uname)
                {
                    ret = i;
                    break;
                }
            }
            return ret;
        }




        /**
         * Index Getters for chats
         * To get the Specified index from the Database
         *
         * ByID: search by ChatID
         * ByHostName: search by chater's name (First Value)
         * ByParticipantName: search By chatee's name (First Value)
         * ByHostID: search by chater's ID(first Value)
         * ByParticipantID: search by chatee's ID (First Value)
         */
        private static int getChatIndexByID(string cid)
        {
            int ret = -1;
            for (int i = 0; i < chats.Count; i++)
            {
                if (chats.ElementAt(i).getID() == cid)
                {
                    ret = i;
                    break;
                }
            }
            return ret;
        }

        private static int getChatIndexByHostName(string host)
        {
            int ret = -1;
            for (int i = 0; i < chats.Count; i++)
            {
                if (chats.ElementAt(i).getChaterName() == host)
                {
                    ret = i;
                    break;
                }
            }
            return ret;
        }

        private static int getChatIndexByParticipantName(string partipant)
        {
            int ret = -1;
            for (int i = 0; i < chats.Count; i++)
            {
                if (chats.ElementAt(i).getChateeName() == partipant)
                {
                    ret = i;
                    break;
                }
            }
            return ret;
        }

        private static int getChatIndexByHostID(string hostID)
        {
            int ret = -1;
            for (int i = 0; i < chats.Count; i++)
            {
                if (chats.ElementAt(i).getChaterID() == hostID)
                {
                    ret = i;
                    break;
                }
            }
            return ret;
        }

        private static int getChatIndexByParticipantID(string partipantID)
        {
            int ret = -1;
            for (int i = 0; i < chats.Count; i++)
            {
                if (chats.ElementAt(i).getChateeID() == partipantID)
                {
                    ret = i;
                    break;
                }
            }
            return ret;
        }










        /**
        * --------------------------------------------------------
        * ------------------Updated 12th May----------------------
        * ----------Methods For Syncing Datas(New Datas)----------
        * --------------------------------------------------------
        */


        public static UserModelOld getFriendObjectById(string fid)
        {
            int index = getUserIndexByID(fid);
            if (index != -1)
            {
                return users.ElementAt(index);
            }
            return null;
        }
        public static GroupModelOld getGroupObjectById(string gid)
        {
            int index = getGroupIndexByID(gid);
            if (index != -1)
            {
                return groups.ElementAt(index);
            }
            return null;
        }
        public static ChatModelOld getChatObjectById(string cid)
        {
            int index = getChatIndexByID(cid);
            if (index != -1)
            {
                return chats.ElementAt(index);
            }
            return null;
        }



        /**
         * --------------------------------------------------------
         * ------------------Updated 12th May----------------------
         * ----------Determine if a certain user is exist----------
         * --------------------------------------------------------
         */
        public static bool isUserExist(string uid)
        {
            if (getUserIndexByID(uid) != -1) return true;
            //else Lookup in the remote server
            // And add to the local database
            //addNewUserToDatabase()
            //then return true;
            return true;
        }



        /**
         * Index Getters for a specified Chat
         * ByID: search by group id
         * ByName: search by group name(First Value)
         *
         */
        private static int getGroupIndexByID(string gid)
        {
            int ret = -1;
            for (int i = 0; i < groups.Count; i++)
            {
                if (groups.ElementAt(i).getID() == gid)
                {
                    ret = i;
                    break;
                }
            }
            return ret;
        }

        private static int getGroupIndexByName(string gname)
        {
            int ret = -1;
            for (int i = 0; i < groups.Count; i++)
            {
                if (groups.ElementAt(i).getName() == gname)
                {
                    ret = i;
                    break;
                }
            }
            return ret;
        }




        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------NEEDS TO CHANGEBY HTTP MODULE !!!!--------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        /**
         * Create a Group if the Group & GroupChat not Exist
         * params:
         *     uindex: user index of Group Founder
         *     gname: group name
         *     _gicon: the group icon
         *     _gstyle: the group's personal Signature
         *
         * return true if creation success
         * @type {String}
         */
        private static bool createGroup(int uindex, string gname, string _gicon = "default", string _gstyle = "None Yet!") {

            /**
             * Get UID and UNAME by user Index
             * @type {[type]}
             */
            string uid = users.ElementAt(uindex).getID();
            string uname = users.ElementAt(uindex).getName();

            /**
             * Create Chatmodel First
             * @type {ChatModel}
             */
            ChatModelOld cm = new ChatModelOld(uname, true);


            if (cm == null) return false;
            chats.Add(cm);

            // Add To Database
            addNewChatToDatabase(cm.getID(), getChatIndexByID(cm.getID()));



            /**
             * Create GroupModel
             * @type {GroupModel}
             */
            GroupModelOld gm = new GroupModelOld(uid, uname, gname, cm.getID(), _gicon, _gstyle);

            if (gm != null) {
                groups.Add(gm);

                //Add to database
                addNewGroupToDatabase(gm.getID(), getGroupIndexByID(gm.getID()));


                /**
                 * Append the new Group & GroupChat to the Current User(Founder).
                 */
                users.ElementAt(uindex).addGroup(gm.getID());


                // Add to Relation Database
                addNewGroupRelationToDataBase(users.ElementAt(uindex).getID(), gm.getID(), cm.getID());
                users.ElementAt(uindex).addChat(cm.getID());
                return true;
            } else return false;
        }





        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------NEEDS TO CHANGEBY HTTP MODULE !!!!--------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        /**
         * Join an existing group
         * params:
         *     uindex: user index
         *     gindex: the Specified Group he want to join
         *
         * return true if creating successfully.
         */
        private static bool joinGroupByIndex(int uindex, int gindex) {
            /**
             * Step 1: add the group id to the groupList of the current User
             */
            users.ElementAt(uindex).addGroup(groups.ElementAt(gindex).getID());
            /**
             * Step2: add the current user to the memberList of the object Group
             */
            groups.ElementAt(gindex).addMember(users.ElementAt(uindex).getID());
            /**
             * Step3: add the groupChat to the chatList of the Current User
             */
            users.ElementAt(uindex).addChat(groups.ElementAt(gindex).getChatID());

            //The simply add a Relation of (GroupRelation into Local Databases)
            addNewGroupRelationToDataBase(users.ElementAt(uindex).getID(), groups.ElementAt(gindex).getID(), groups.ElementAt(gindex).getChatID());
            return true;
        }



        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------NEEDS TO CHANGEBY HTTP MODULE !!!!--------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        /**
         * Create a specified chat for the host and parcipant
         * returns a string of creating process successful or not.
         * @type {Boolean}
         */
        public static string createChatForUser(string hostID, string participantID, bool isGroupChat = false) {
            if (getUserIndexByID(hostID) == -1 || getUserIndexByID(participantID) == -1) return "No User!";
            string hostName = users.ElementAt(getUserIndexByID(hostID)).getName();
            string participantName = users.ElementAt(getUserIndexByID(participantID)).getName();
            ChatModelOld cm = new ChatModelOld(hostName, hostID, participantName, participantID, isGroupChat);
            chats.Add(cm);
            // Append Chat to Database
            //addNewFriendRelationToDataBase(hostID, participantID, cm.getID());
            addNewChatToDatabase(cm.getID(), chats.Count - 1);
            return cm.getID();
        }



        /**
         * Getters of the User Database
         * Should be depreciated.
         *
         */
        public static List<UserModelOld> getUsers()
        {
            return users;
        }




        /**
         * Depreciated Methods
         * Return the Requested Method to the Users.
         *
         */
        public static List<string> getFriendIDs(string uid)
        {
            List<string> ret = null;
            int index = getUserIndexByID(uid);
            if (index != -1) ret = users.ElementAt(index).getFriends();
            return ret;
        }

        public static List<string> getGroupIDs(string uid)
        {
            List<string> ret = null;
            int index = getUserIndexByID(uid);
            if (index != -1) ret = users.ElementAt(index).getGroups();
            return ret;
        }


        public static List<string> getChatIDs(string uid)
        {
            List<string> ret = null;
            int index = getUserIndexByID(uid);
            if (index != -1) ret = users.ElementAt(index).getChats();
            return ret;
        }



        /**
         * Add Friend Method
         */
        public static bool addFriend(string id, string fid) {
            int index = getUserIndexByID(id);
            int findex = getUserIndexByID(fid);
            if (index != -1 && findex != -1) {
                /**
                 * Add a friend is Double Binding
                 */
                users.ElementAt(index).addFriend(fid);
                users.ElementAt(findex).addFriend(id);
                string cid = createChatForUser(id, fid);
                // Create relation
                addNewFriendRelationToDataBase(id, fid, cid);

                // Then we create a Chat for them
                return true;
            } else return false;
        }



        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------NEEDS TO CHANGEBY HTTP MODULE !!!!--------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        /**
         * Remove a friend Method
         */
        public static bool removeFriend(string id, string fid) {
            int index = getUserIndexByID(id);
            if (index != -1) {
                /**
                 * But Delete a friend is just Single-deletion
                 */
                users.ElementAt(index).deleteFriend(fid);
                // Sync Local Database
                return removeFriendRelationFromDataBase(id, fid);
            } else return false;
        }


        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------NO-NEEDS TO CHANGEBY HTTP MODULE !!!!----------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        /**
         * Sinple Add Chat for a user
         * params:
         *     id: the user(host) ID
         *     fid: the user(participant) ID
         *
         * return true if adding success.
         * @type {Boolean}
         */
        public bool addChatForUser(string id, string fid, bool isGroupChat = false) {
            /**
             * A chat should be create once.
             * But use many times
             * @type {[type]}
             */
            string cid = createChatForUser(id, fid, isGroupChat);

            int index = getUserIndexByID(id);
            int findex = getUserIndexByID(fid);
            //addChat to their listWa
            if (index != -1 && findex != -1) {
                users.ElementAt(index).addChat(cid);
                users.ElementAt(findex).addChat(cid);
                return true;
            } else return false;
        }





        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------NEEDS TO CHANGEBY HTTP MODULE !!!!--------------------------------------------------------------------------
        //---------------------------------------------------------------------------Wait and See To Modify!!--------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /**
         * Chat remove Method
         * params:
         *     id: user id,
         *     cid: the chat id
         *
         * return true if removal successful
         */
        public bool removeChatForUser(string id, string cid) {
            /**
             * Not remove the records(MessageList)
             * @type {[type]}
             */
            int index = getUserIndexByID(id);
            if (index != -1) {

                return users.ElementAt(index).deleteChat(cid);
            } else return false;
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------NEEDS TO CHANGEBY HTTP MODULE !!!!--------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /**
         * Search group from a given name
         * params:
         *     gname: the name you want to search
         *
         * return a List of corresponding group ids
         */
        public static List<string> searchGroups(string gname) {
            List<string> res = new List<string>();
            for (int i = 0; i < groups.Count; i++) {
                if (groups.ElementAt(i).getName() == gname) {
                    res.Add(groups.ElementAt(i).getID());
                }
            }
            // Append To local Array and then sync to local databases
            // Search From Server And we save the results into the database()
            return res;
        }



        /**
         * Join a specified group(Called by Client)
         * params:
         *     uid: user Id;
         *     gid: group id;
         *     gname: group name;
         *
         * return true if add Successfully
         */
        public static bool addToGroupByID(string uid, string gid, string gname) {
            int index = getUserIndexByID(uid);
            int gindex = getGroupIndexByID(gid);
            if (index != -1) {
                if (gindex != -1) return joinGroupByIndex(index, gindex);
                else return createGroup(index, gname);
            } else return false;
        }



        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------NEEDS TO CHANGEBY HTTP MODULE !!!!--------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        /**
         * Group Quit Method
         * Remember that the group will be explosed
         * if the owner exit the group
         *
         * params:
         *     uid: user id;
         *     gid: group id;
         *
         *
         * return true if exitation success
         */
        public static bool quitGroupByID(string uid, string gid) {
            int index = getUserIndexByID(uid);
            int gindex = getGroupIndexByID(gid);
            if (index == -1 || gindex == -1) return false;
            /**
             * Only permits quitting for the
             * chrrent group members
             */
            if (groups.ElementAt(gindex).hasMember(uid)) {
                if (groups.ElementAt(gindex).isAdmin(uid)) {
                    /**
                     * batch removal for the group members
                     */
                    int userIndex;
                    List<string> allMembers = groups.ElementAt(gindex).getMembers();
                    string chatID = groups.ElementAt(gindex).getChatID();
                    string groupID = groups.ElementAt(gindex).getID();
                    for (int i = 0; i < allMembers.Count; i++) {
                        userIndex = getUserIndexByID(allMembers.ElementAt(i));
                        users.ElementAt(userIndex).deleteChat(chatID);
                        users.ElementAt(userIndex).deleteGroup(groupID);
                        groups.ElementAt(gindex).deleteMember(allMembers.ElementAt(i));
                        // Remove a certain person from a specified group
                        removeGroupRelationFromDatabase(users.ElementAt(userIndex).getID(), groupID);
                    }

                    /**
                     * Finally release chatid and itself
                     */
                    chats.RemoveAt(getChatIndexByID(chatID));
                    groups.RemoveAt(gindex);
                    // Remove Group And Remove Chat will be realized in the future
                    return true;
                } else {
                    /**
                     * remove chat and group for the ordinary group members
                     */
                    users.ElementAt(index).deleteChat(groups.ElementAt(gindex).getChatID());
                    users.ElementAt(index).deleteGroup(groups.ElementAt(gindex).getID());
                    //Remove Relation from Database;
                    removeGroupRelationFromDatabase(users.ElementAt(index).getID(), groups.ElementAt(gindex).getID());
                    /**
                     * And simply dismiss the user from the group
                     */
                    groups.ElementAt(gindex).deleteMember(uid);
                    return true;
                }
            }
            return false;
        }



        /**
         * I don't remember what these functions for
         */
        public static UserModelOld getFriend(string id)
        {
            int index = getUserIndexByID(id);
            if (index != -1) return users.ElementAt(index);
            else return null;
        }
        public static UserModelOld getFriendByName(string name)
        {
            int index = getUserIndexByName(name);
            if (index != -1) return users.ElementAt(index);
            else return null;
        }

        public static GroupModelOld getGroup(string id)
        {
            int index = getGroupIndexByID(id);
            if (index != -1) return groups.ElementAt(index);
            else return null;
        }

        public static ChatModelOld getChat(string id)
        {
            int index = getChatIndexByID(id);
            if (index != -1) return chats.ElementAt(index);
            else return null;
        }


        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------NEEDS TO CHANGEBY HTTP MODULE !!!!--------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /**
         * Single one to one push
         * warning!!!
         * @type {String}
         */
        public static bool pushMessageToChat(string message, string chaterID, string chatID, string chateeID = "NULL") {
            int cindex = getChatIndexByID(chatID);
            int aindex = getUserIndexByID(chaterID);
            if (cindex == -1) {
                if (chateeID == "NULL" || getUserIndexByID(chateeID) == -1) return false;
                /**
                 * if chat not exists , then we create a new Chat
                 * @type {[type]}
                 */
                string cid = createChatForUser(chaterID, chateeID);
                //Sync this chat Relation To database
                addNewChatToDatabase(cid, getChatIndexByID(cid));

                users.ElementAt(aindex).addChat(cid);
                users.ElementAt(getUserIndexByID(chateeID)).addChat(cid);
                /**
                 * Append the Message
                 */
                // Add to new relation
                addNewMsgRelationToDataBase(chaterID, chateeID, cid, message, DateTimeOffset.Now.ToLocalTime().ToString());
                return chats.ElementAt(getChatIndexByID(cid)).pushMessage(message, chaterID, chateeID);
            } else {
                // Add a new Relation
                addNewMsgRelationToDataBase(chaterID, chateeID, chatID, message, DateTimeOffset.Now.ToLocalTime().ToString());

                return chats.ElementAt(cindex).pushMessage(message, chaterID, chateeID);
            }
        }


        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------NEEDS TO CHANGEBY HTTP MODULE !!!!--------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        /**
         * Push Message to an existing group
         */
        public static bool pushMessageToGroup(string message, string uid, string gid) {
            int gindex = getGroupIndexByID(gid);
            if (gindex == -1) return false;

            string cid = groups.ElementAt(gindex).getChatID();
            int cindex = getChatIndexByID(cid);
            if (cindex == -1) return false;

            // Append To Database
            addNewMsgRelationToDataBase(uid, gid, cid, message, DateTimeOffset.Now.ToLocalTime().ToString());
            return chats.ElementAt(cindex).pushMessage(message, uid, gid);
        }


        /**
         * No-use Creator
         */
        private static string encryptCreator(string encrypt)
        {
            string sha = HashAlgorithmNames.Sha512;
            HashAlgorithmProvider provider = HashAlgorithmProvider.OpenAlgorithm(sha);
            CryptographicHash hashme = provider.CreateHash();
            IBuffer origin = CryptographicBuffer.ConvertStringToBinary(encrypt, BinaryStringEncoding.Utf16BE);
            hashme.Append(origin);
            IBuffer result = hashme.GetValueAndReset();
            string hashcode = CryptographicBuffer.EncodeToBase64String(result);
            return hashcode;
        }
    }
}

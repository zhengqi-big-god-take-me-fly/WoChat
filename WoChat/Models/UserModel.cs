using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace WoChat.Models
{
    /**
     * Model of a user
     */
    class UserModel
    {
        /**
         * Intialize variables:
         *
         * uid: the user id
         * uname: the username
         * upassword: the user Password(Encrypted)
         */
        private string uid;
        private string uname;
        private string upassword;

        /**
         * uinfo: the user info (type: InfoModel)
         */
        private InfoModel uinfo;

        /**
         * A string list of a user's friends(ids)
         */
        private List<string> friends;

        /**
         * A string list of a user's group(ids)
         */
        private List<string> groups;

        /**
         * A string list of a user's chat(ids)
         */
        private List<string> chats;


        /**
         * setter of name password and info
         */
        public void setName(string newname)
        {
            this.uname = newname;
        }
        public void setPassword(string newpassword)
        {
            this.upassword = encryptCreator(newpassword);
        }
        public void setInfo(InfoModel info)
        {
            this.uinfo = info;
        }


        /**
         * getter of ID, Name, Password , Info
         */
        public string getID()
        {
            return uid;
        }
        public string getName()
        {
            return uname;
        }
        public string getPassword()
        {
            return upassword;
        }
        public InfoModel getInfo()
        {
            return uinfo;
        }

        /**
         * The getter of the String Lists.
         */
        public List<string> getFriends()
        {
            return this.friends;
        }

        public List<string> getGroups()
        {
            return this.groups;
        }

        public List<string> getChats()
        {
            return this.chats;
        }




        /**
         * Just Wait for Sync
         */
        //public string addChatModel(string participantName , string participantID,  bool isGroup = false)
        //{
        //    return DataModel.createChatForUser(this.uname, this.uid, participantName, participantID);
        //}


        /**
         * Add a Friend by id
         * this function adds id to the String list of friend ids
         * return true if no execption
         */
        public bool addFriend(string fid)
        {
            this.friends.Add(fid);
            return true;
        }

        /**
         * Delete a friend by id
         * This function removes id from the String list of friend ids
         * return true if removal successful.
         */
        public bool deleteFriend(string fid)
        {
            return this.friends.Remove(fid);
        }

        /**
         * Add a group by id
         * this function adds id to the String list of user group ids
         * return true if no execption
         */
        public bool addGroup(string gid)
        {
            this.groups.Add(gid);
            return true;
        }

        /**
         * Exit a group by id
         * This function removes id from the String list of user groups
         * return true if Exit successfully.
         */
        public bool deleteGroup(string gid)
        {
            return this.groups.Remove(gid);
        }

        /**
         * Add a chat by id
         * this function adds id to the String list of user groups
         * return true if no exception
         */
        public bool addChat(string cid)
        {
            this.chats.Add(cid);
            return true;
        }

        /**
         * Delete a chat by id
         * This function removes id from the String list of user chats
         * return true if Deletion successful
         */
        public bool deleteChat(string cid)
        {
            return this.chats.Remove(cid);
        }




        /**
         * Setters for the three Lists
         * Used for Synchronization
         */
        public bool setFriend(List<string> newFriends)
        {
            this.friends = newFriends;
            return true;
        }
        public bool setGroup(List<string> newGroups)
        {
            this.groups = newGroups;
            return true;
        }
        public bool setChat(List<string> newChat)
        {
            this.chats = newChat;
            return true;
        }



        /**
         * Here are useless methods.
         */
        public UserModel getFriendInfo(string friendID)
        {
            return DataModel.getFriend(friendID);
        }

        public GroupModel getGroupInfo(string groupID)
        {
            return DataModel.getGroup(groupID);
        }

        public ChatModel getChatInfo(string chatID)
        {
            return DataModel.getChat(chatID);
        }



        /**
         * Encrypter for the password
         * accepts the regular password and generate the encrypted ones
         * use SHA512 Algorithms
         * return : the encrypted password.
         */
        private string encryptCreator(string encrypt)
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

        /**
         * Constructor for User
         * Required name ,pw ,nickname , email, icon and style
         *
         */
        public UserModel(string name, string password , string _nick , string _email , string _icon = "default" , string _style = "None Yet!")
        {

            /**
             * Auto generate for User ID
             * [uid description]
             * @type {[type]}
             */
            this.uid = Guid.NewGuid().ToString();
            this.uname = name;
            /**
             * [upassword description]
             * Only storage Encrypted passwords!
             * @type {[type]}
             */
            this.upassword = encryptCreator(password);
            this.uinfo = new InfoModel(_nick, _email, _icon, _style);


            /**
             * Initialize for three String Lists
             * @type {List}
             */
            this.groups = new List<string>();
            this.friends = new List<string>();
            this.chats = new List<string>();
        }
    }
}

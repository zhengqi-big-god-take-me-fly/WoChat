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
    class DataModel
    {
        private static List<UserModel> users = new List<UserModel>();
        private static List<GroupModel> groups = new List<GroupModel>();
        private static List<ChatModel> chats = new List<ChatModel>();

        // 这里只是模拟服务器请求 ， 实际上数据应该存在服务器上。
        public DataModel() {
            //this.users = new List<UserModel>();
            //this.groups = new List<GroupModel>();
            //this.chats = new List<ChatModel>();
        }

        public static Boolean init()
        {
            // ALL get Methods.
            return true;
        }


        //登录模块
        public static UserModel userLogin(string username , string password)
        {
            UserModel uml = null;
            string pw = encryptCreator(password);
            for (int i = 0; i < users.Count; i++)
            {
                if (users.ElementAt(i).getName() == username)
                {
                    if (users.ElementAt(i).getPassword() == pw)
                    {
                        uml = users.ElementAt(i);
                        break;
                    }
                    break;
                }
            }
            return uml;
        }

        //注册模块
        public static Boolean userRegister(string name, string password, string _nick, string _email, string _icon = "default", string _style = "None Yet!")
        {
            //先查询
            if (searchForUser(name, _nick, _email) != "Pass!") return false;
            UserModel um = new UserModel(name, password, _nick, _email, _icon, _style);
            users.Add(um);
            return true;
        }

        //这三个条件都是unique
        public static string searchForUser(string name , string _nick , string _email)
        {
            string warningMessage = "";
            for (int i = 0; i < users.Count; i++)
            {
                if (users.ElementAt(i).getName() == name)
                {
                    warningMessage += "\n该用户名已经被占用，无法使用。\n";
                }

                if (users.ElementAt(i).getInfo().email == _email)
                {
                    warningMessage += "\n该邮件已经被占用，无法使用。\n";
                }

                if (users.ElementAt(i).getInfo().nickname == _nick)
                {
                    warningMessage += "\n该呢称已经被占用，无法使用。\n";
                }
            }
            if (warningMessage == "")
            {
                warningMessage = "Pass!";
            }
            return warningMessage;
        }



        public static string createChatForUser(string hostName, string hostID, string participantName, string participantID)
        {
            ChatModel cm = new ChatModel(hostName, hostID, participantName, participantID);
            chats.Add(cm);
            return cm.getID();
        }

        public static List<UserModel> getUsers()
        {
            return users;
        }
        public static List<string> getFriendIDs(string uid)
        {
            List<string> ret = null;
            for (int i = 0; i < users.Count; i++)
            {
                if (users.ElementAt(i).getID() == uid)
                {
                    ret = users.ElementAt(i).getFriends();
                }
            }
            return ret;
        }

        public static List<string> getGroupIDs(string uid)
        {
            List<string> ret = null;
            for (int i = 0; i < users.Count; i++)
            {
                if (users.ElementAt(i).getID() == uid)
                {
                    ret = users.ElementAt(i).getGroups();
                }
            }
            return ret;
        }


        public static List<string> getChatIDs(string uid)
        {
            List<string> ret = null;
            for (int i = 0; i < users.Count; i++)
            {
                if (users.ElementAt(i).getID() == uid)
                {
                    ret = users.ElementAt(i).getChats();
                }
            }
            return ret;
        }





        public static UserModel getFriend(string id)
        {
            UserModel ret = null;
            for (int i = 0; i < users.Count; i++)
            {
                if (users.ElementAt(i).getID() == id)
                {
                    ret = users.ElementAt(i);
                    break;
                }
            }
            return ret;
        }

        public static GroupModel getGroup(string id)
        {
            GroupModel ret = null;
            for (int i = 0; i < groups.Count; i++)
            {
                if (groups.ElementAt(i).getID() == id)
                {
                    ret = groups.ElementAt(i);
                    break;
                }
            }
            return ret;
        }

        public static ChatModel getChat(string id)
        {
            ChatModel ret = null;
            for (int i = 0; i < chats.Count; i++)
            {
                if (chats.ElementAt(i).getID() == id)
                {
                    ret = chats.ElementAt(i);
                    break;
                }
            }
            return ret;
        }


        public static Boolean pushMessageToChat(string message , string chaterID , Boolean isGroupChat = false)
        {
            if (!isGroupChat)
            {
                //不是群聊的话就只能是参与者或者发起者
                for (int i = 0; i < chats.Count; i++)
                {
                    if (chaterID == chats.ElementAt(i).getChaterID() || chaterID == chats.ElementAt(i).getChateeID())
                    {
                        chats.ElementAt(i).pushMessageLocal(message);
                    }
                }
                return true;
                //群聊则需要根据GroupId找到一个组
            } else
            {
                for (int i = 0; i < chats.Count; i++)
                {
                    if (chats.ElementAt(i).getGroupID() == chaterID)
                    {
                        chats.ElementAt(i).pushMessageLocal(message);
                        break;
                    }
                }
                return true;
            }
        }

        public static string lookUpForId(string name , string type , int identity = 0)
        {
            string o = "Not Found!";
            switch (type) {
                case "user":
                    for (int i = 0; i < users.Count; i++)
                    {
                        if (users.ElementAt(i).getName() == name)
                        {
                            return users.ElementAt(i).getID();
                        }
                    }
                    // Look up in userList;
                    break;
                case "group":
                    // Look up in groupList;
                    for (int i = 0; i < groups.Count; i++)
                    {
                        if (groups.ElementAt(i).getName() == name)
                        {
                            return groups.ElementAt(i).getID();
                        }
                    }
                    break;
                case "send":
                    for (int i = 0; i < chats.Count; i++)
                    {
                        // Send to personal users
                        if (identity == 0)
                        {
                        }
                    }
                    // Look up in messageList;
                    break;
                case "receive":
                    // Don't know what to append;
                    break;
                default:
                    break;
            }
            return o;

        }

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

        public static string lookUpForName(string name, string type, int identity = 0)
        {
            string o = "Not Found!";
            switch (type)
            {
                case "user":
                    for (int i = 0; i < users.Count; i++)
                    {
                        if (users.ElementAt(i).getID() == name)
                        {
                            return users.ElementAt(i).getName();
                        }
                    }
                    // Look up in userList;
                    break;
                case "group":
                    // Look up in groupList;
                    for (int i = 0; i < groups.Count; i++)
                    {
                        if (groups.ElementAt(i).getID() == name)
                        {
                            return groups.ElementAt(i).getName();
                        }
                    }
                    break;
                case "send":
                    for (int i = 0; i < chats.Count; i++)
                    {
                        // Send to personal users
                        if (identity == 0)
                        {
                        }
                    }
                    // Look up in messageList;
                    break;
                case "receive":
                    // Don't know what to append;
                    break;
                default:
                    break;
            }
            return o;

        }
    }
}

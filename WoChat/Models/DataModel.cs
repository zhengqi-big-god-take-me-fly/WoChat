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

        }

        public static bool init()
        {

            return true;
        }


        //登录模块
        public static UserModel userLogin(string username , string password)
        {
            UserModel uml = null;
            int index = getUserIndexByName(username);
            if (index != -1 && users.ElementAt(index).getPassword() == encryptCreator(password))
            {
                uml = users.ElementAt(index);
            }
            return uml;
        }

        //注册模块
        public static bool userRegister(string name, string password, string _nick, string _email, string _icon = "default", string _style = "None Yet!")
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

        public static bool addFriend(string id , string fid)
        {
            int index = getUserIndexByID(id);
            if (index != -1)
            {
                //users.ElementAt(index).addFriend(fid, 1);
                return users.ElementAt(index).addFriend(fid, 1); ;
            }
            else return false;
        }
        public static bool removeFriend(string id , string fid)
        {

        }



        public static UserModel getFriend(string id)
        {
            int index = getUserIndexByID(id);
            if (index != -1) return users.ElementAt(index);
            else return null;
        }
        public static UserModel getFriendByName(string name)
        {
            int index = getUserIndexByName(name);
            if (index != -1) return users.ElementAt(index);
            else return null;
        }

        public static GroupModel getGroup(string id)
        {
            int index = getGroupIndexByID(id);
            if (index != -1) return groups.ElementAt(index);
            else return null;
        }

        public static ChatModel getChat(string id)
        {
            int index = getChatIndexByID(id);
            if (index != -1) return chats.ElementAt(index);
            else return null;
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

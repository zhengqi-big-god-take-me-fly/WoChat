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

        private static bool createGroup(int uindex , string gname , string _gicon = "default", string _gstyle = "None Yet!")
        {
            string uid = users.ElementAt(uindex).getID();
            string uname = users.ElementAt(uindex).getName();
            ChatModel cm = new ChatModel(uname, "NULL", true);
            if (cm == null) return false;
            GroupModel gm = new GroupModel(uid, uname, gname, cm.getID() , _gicon, _gstyle);
            if (gm != null)
            {
                groups.Add(gm);
                //自己的group列表加入gid，自己的chat列表加入cid；
                return true;
            }
            else return false;
        }


        private static bool joinGroupByIndex(int uindex , int gindex)
        {
            //第一步 ， 自己group列表加入gid
            users.ElementAt(uindex).addGroup(groups.ElementAt(gindex).getID());
            //第二部 ， group的member里面加入uid
            groups.ElementAt(gindex).addMember(users.ElementAt(uindex).getID());
            //第三步，在自己的Chat列表中加入cid
            users.ElementAt(uindex).addChat(groups.ElementAt(gindex).getChatID());
            return true;
        }



        public static string createChatForUser(string hostID, string participantID , bool isGroupChat = false)
        {
            if (getUserIndexByID(hostID) == -1 || getUserIndexByID(participantID) == -1) return "No User!";
            string hostName = users.ElementAt(getUserIndexByID(hostID)).getName();
            string participantName = users.ElementAt(getUserIndexByID(participantID)).getName();
            ChatModel cm = new ChatModel(hostName , hostID, participantName ,participantID , isGroupChat);
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


        //正常套路：添加朋友
        public static bool addFriend(string id , string fid)
        {
            int index = getUserIndexByID(id);
            int findex = getUserIndexByID(fid);
            if (index != -1 && findex != -1)
            {
                users.ElementAt(index).addFriend(fid);
                users.ElementAt(findex).addFriend(id);
                return true;
            }
            else return false;
        }
        public static bool removeFriend(string id , string fid)
        {
            int index = getUserIndexByID(id);
            if (index != -1)
            {
                return users.ElementAt(index).deleteFriend(fid);
            }
            else return false;
        }

        //下面是添加聊天
        public bool addChatForUser(string id , string fid , bool isGroupChat = false)
        {
            //Double Binding
            string cid = createChatForUser(id, fid, isGroupChat);
            //createChatForUser(fid, id, isGroupChat);

            //Add into the users chatlist;
            int index = getUserIndexByID(id);
            int findex = getUserIndexByID(fid);
            //addChat to their list
            if (index != -1 && findex != -1)
            {
                users.ElementAt(index).addChat(cid);
                users.ElementAt(findex).addChat(cid);
                return true;
            }
            else return false;
        }

        public bool removeChatForUser(string id , string cid)
        {
            //不删除聊天记录
            int index = getUserIndexByID(id);
            if (index != -1) return users.ElementAt(index).deleteChat(cid);
            else return false;
        }


        public static List<string> searchGroups(string gname)
        {
            List<string> res = new List<string>();
            for (int i = 0; i < groups.Count; i++)
            {
                if (groups.ElementAt(i).getName() == gname)
                {
                    res.Add(groups.ElementAt(i).getID());
                }
            }
            return res;
        }

        //加入群
        public static bool addToGroupByID(string uid , string gid , string gname)
        {
            int index = getUserIndexByID(uid);
            int gindex = getGroupIndexByID(gid);
            if (index != -1)
            {
                if (gindex != -1) return joinGroupByIndex(index, gindex);
                else return createGroup(index, gname);
            }
            else return false;
        }
        //退出群，注意创建者退出将自动解散
        public static bool quitGroupByID(string uid , string gid)
        {
            int index = getUserIndexByID(uid);
            int gindex = getGroupIndexByID(gid);
            if (index == -1 || gindex == -1) return false;
            // 是组内成员才能退出
            if (groups.ElementAt(gindex).hasMember(uid))
            {
                if (groups.ElementAt(gindex).isAdmin(uid))
                {
                    //批量删除用户中的Chat和Group。
                    int userIndex;
                    List<string> allMembers = groups.ElementAt(gindex).getMembers();
                    string chatID = groups.ElementAt(gindex).getChatID();
                    string groupID = groups.ElementAt(gindex).getID();
                    for (int i = 0; i < allMembers.Count; i++)
                    {
                        userIndex = getUserIndexByID(allMembers.ElementAt(i));
                        users.ElementAt(userIndex).deleteChat(chatID);
                        users.ElementAt(userIndex).deleteGroup(groupID);
                        groups.ElementAt(gindex).deleteMember(allMembers.ElementAt(i));
                    }
                    //最后删除自己的chat和销毁自己。
                    chats.RemoveAt(getChatIndexByID(chatID));
                    groups.RemoveAt(gindex);
                } else
                {
                    // 只是普通成员 , 在自己的列表中删除Chat和Group
                    users.ElementAt(index).deleteChat(groups.ElementAt(gindex).getChatID());
                    users.ElementAt(index).deleteGroup(groups.ElementAt(gindex).getID());
                    // 然后从该群聊中退出（无视该用户已经发送的信息）
                    groups.ElementAt(gindex).deleteMember(uid);
                }
            }
            return false;
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

        //一对一类型信息
        public static bool pushMessageToChat(string message , string chaterID , string chatID , string chateeID = "NULL")
        {
            int cindex = getChatIndexByID(chatID);
            int aindex = getUserIndexByID(chaterID);
            if (cindex == -1)
            {
                if (chateeID == "NULL" || getUserIndexByID(chateeID) == -1) return false;
                //创建新的Chat
                string cid = createChatForUser(chaterID, chateeID);
                users.ElementAt(aindex).addChat(cid);
                users.ElementAt(getUserIndexByID(chateeID)).addChat(cid);
                //添加信息
                return chats.ElementAt(getChatIndexByID(cid)).pushMessage(message);
            } else
            {
                return chats.ElementAt(cindex).pushMessage(message);
            }
        }
        //群消息
        public static bool pushMessageToGroup(string message , string gid)
        {
            int gindex = getGroupIndexByID(gid);
            if (gindex == -1) return false;

            string cid = groups.ElementAt(gindex).getChatID();
            int cindex = getChatIndexByID(cid);
            if (cindex == -1) return false;
            return chats.ElementAt(cindex).pushMessage(message);
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

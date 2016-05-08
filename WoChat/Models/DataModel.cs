using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static string createChatForUser(string hostName, string hostID, string participantName, string participantID)
        {
            ChatModel cm = new ChatModel(hostName, hostID, participantName, participantID);
            chats.Add(cm);
            return cm.getID();
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

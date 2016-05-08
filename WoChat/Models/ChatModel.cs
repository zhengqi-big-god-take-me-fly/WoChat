using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WoChat.Models
{
    class ChatModel
    {
        private string chatid;
        private string chater, chatee;
        private string chaterId, chateeId;
        private string groupName;
        private string groupId;
        private Boolean isGroupChat;
        private List<string> chaterList;
        private List<MessageModel> messageList;

        public string getID()
        {
            return this.chatid;
        }

        public string getChaterName()
        {
            if (!this.isGroupChat)
            {
                return this.chater;
            } else
            {
                return "It's a Group Chat!";
            }
        }

        public string getChaterID()
        {
            if (!this.isGroupChat)
            {
                return this.chaterId;
            } else
            {
                return "It's a Group Chat!";
            }
        }

        public string getChateeName()
        {
            if (!isGroupChat)
            {
                return this.chatee;
            } else
            {
                return "It's a Group Chat!";
            }
        }

        public string getChateeID()
        {
            if (!isGroupChat)
            {
                return this.chateeId;
            } else
            {
                return "It's a Group Chat!";
            }
        }


        public string getGroupName()
        {
            if (!isGroupChat)
            {
                return "It's NOT a Group Chat!";
            } else
            {
                return this.groupName;
            }
        }

        public string getGroupID()
        {
            if (!isGroupChat)
            {
                return "It's NOT a Group Chat!";
            } else
            {
                return this.groupId;
            }
        }



        public string lookUpForId(string name)
        {
            // Implement;
            return null;

        }

        public List<MessageModel> getGroupChat()
        {
            if (this.isGroupChat == true)
            {
                return messageList;
            } else
            {
                return null;
            }
        }
        
        public List<MessageModel> getChat()
        {
            if (this.isGroupChat == false)
            {
                return messageList;
            } else
            {
                return null;
            }
        }





       public Boolean pushMessageLocal(string message)
        {
            if (this.messageList == null)
            {
                this.messageList = new List<MessageModel>();
            }
            // Start pushing process
            MessageModel newMessage = new MessageModel(this.chater, this.chatee, message);
            this.messageList.Add(newMessage);
            return true;
        }

        public Boolean pushMessageToServer(string message)
        {
            if (!this.isGroupChat)
            {
                DataModel.pushMessageToChat(message, this.chaterId, this.isGroupChat);
            } else
            {
                DataModel.pushMessageToChat(message, this.groupId, this.isGroupChat);
            }
            return true;
        }






        public ChatModel(string _chater , string _chatee = "NULL" , bool _isGroupChat = false)
        {
            this.chatid = Guid.NewGuid().ToString();
            this.chater = _chater;
            this.chatee = _chatee;
            this.isGroupChat = _isGroupChat;
            this.messageList = new List<MessageModel>();
        }
        

        public ChatModel(string _chater, string _chaterid , string _chatee, string _chateeid , bool _isGroupChat = false)
        {
            if (!_isGroupChat)
            {
                this.chatid = Guid.NewGuid().ToString();
                this.chater = _chater;
                this.chatee = _chatee;
                this.chaterId = _chaterid;
                this.chateeId = _chateeid;
                this.isGroupChat = _isGroupChat;
                this.messageList = new List<MessageModel>();
            } else
            {
                this.chatid = Guid.NewGuid().ToString();
                this.isGroupChat = _isGroupChat;
                this.messageList = new List<MessageModel>();
                this.chaterList = new List<string>();
                this.chaterList.Add(_chaterid);
            }
        }
    }
}

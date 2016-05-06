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
        private string chater, chatee;
        private string chaterId, chateeId;
        private string groupName;
        private string groupId;
        private Boolean isGroupChat;
        private List<MessageModel> messageList;

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

       public Boolean pushMessage(string message)
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

        public ChatModel(string _chater , string _chatee , Boolean _isGroupChat)
        {
            this.chater = _chater;
            this.chatee = _chatee;
            this.isGroupChat = _isGroupChat;
            this.messageList = new List<MessageModel>();
        }
    }
}

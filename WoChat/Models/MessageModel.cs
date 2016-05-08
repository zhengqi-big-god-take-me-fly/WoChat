using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoChat.Models
{
    class MessageModel
    {
        private string sender;
        private string senderId;
        private string receiver;
        private string receiverId;
        private bool isGroupMessage;
        private string content;
        private DateTimeOffset time;

        public string getSender()
        {
            return this.sender;
        }

        public string getReceiver()
        {
            return this.receiver;
        }

        public string getContent()
        {
            return this.content;
        }

        public DateTimeOffset getTime()
        {
            return this.time;
        }

        public bool getFlag()
        {
            return this.isGroupMessage;
        }

        public MessageModel(string _sender , string _receiver , string message , bool _isGroupMessage)
        {
            this.sender = _sender;
            this.senderId = DataModel.getFriendByName(_sender).getID();
            this.receiver = _receiver;
            this.receiverId = DataModel.getFriendByName(_receiver).getID();
            this.content = message;
            this.isGroupMessage = _isGroupMessage;
            this.time = DateTimeOffset.Now;
        }
    }
}

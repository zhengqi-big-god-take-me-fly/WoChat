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
        private Boolean isGroupMessage;
        private string content;
        private DateTimeOffset time;

        public MessageModel(string _sender , string _receiver , string message)
        {
            this.sender = _sender;
            this.senderId = DataModel.getFriendByName(_sender).getID();
            this.receiver = _receiver;
            this.receiverId = DataModel.getFriendByName(_receiver).getID();
            this.content = message;
            this.time = DateTimeOffset.Now;
        }
    }
}

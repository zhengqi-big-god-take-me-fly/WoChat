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
        private string content;
        private DateTimeOffset time;

        private string lookUpForId(string name)
        {
            // needs to be implemented;
            return null;
        }

        public MessageModel(string _sender , string _receiver , string message)
        {
            this.sender = _sender;
            this.senderId = lookUpForId(_sender);
            this.receiver = _receiver;
            this.receiverId = lookUpForId(_receiver);
            this.content = message;
            this.time = DateTimeOffset.Now;
        }
    }
}

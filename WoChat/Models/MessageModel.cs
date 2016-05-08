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

        private string lookUpForId(string name)
        {
            // needs to be implemented;
            return null;
        }

        public MessageModel(string _sender , string _receiver , string message)
        {
            this.sender = _sender;
            this.senderId = DataModel.lookUpForId(_sender , "user");
            this.receiver = _receiver;
            this.receiverId = DataModel.lookUpForId(_receiver , "user");
            this.content = message;
            this.time = DateTimeOffset.Now;
        }
    }
}

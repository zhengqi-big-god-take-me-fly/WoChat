using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoChat.Deprecated {
    public class MessageViewModel
    {
        public string senderID { get; set; }
        public string senderName { get; set; }
        public string receiverID { get; set; }
        public string receiverName { get; set; }
        public string content { get; set; }
        public string timeString { get; set; }

        public MessageViewModel(string sid , string sname , string rid , string rname , string body , string time)
        {
            this.senderID = sid;
            this.senderName = sname;
            this.receiverID = rid;
            this.receiverName = rname;
            this.content = body;
            this.timeString = time;
        }
    }
}

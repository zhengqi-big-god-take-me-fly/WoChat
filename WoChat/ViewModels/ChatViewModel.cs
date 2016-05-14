using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoChat.ViewModels
{
    public class ChatViewModel
    {
        public string chatID { get; set; }
        public string hostID { get; set; }
        public string participantID { get; set; }
        public string hostName { get; set; }
        public string participantName { get; set; }
        public bool isGroupChat { get; set; }
        public List<MessageViewModel> messageList { get; set; }
        //public string

        public ChatViewModel(string cid , string hid , string pid , string hname , string pname , bool isGroupChat , List<MessageViewModel> myMessageList)
        {
            this.chatID = cid;
            this.hostID = hid;
            this.participantID = pid;
            this.hostName = hname;
            this.participantName = pname;
            this.isGroupChat = isGroupChat;
            this.messageList = myMessageList;
        }

    }
}

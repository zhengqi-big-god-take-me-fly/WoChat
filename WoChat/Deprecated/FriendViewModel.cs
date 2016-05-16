using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoChat.Deprecated
{
    public class FriendViewModel
    {
        string friendID { get; set; }
        string friendName { get; set; }
        string friendIcon { get; set; }
        string friendStylish { get; set; }
        string friendEmail { get; set; }
        string friendChatID { get; set; }

        public FriendViewModel(string id , string name, string icon , string style , string email , string myChat)
        {
            this.friendID = id;
            this.friendName = name;
            this.friendIcon = icon;
            this.friendStylish = style;
            this.friendEmail = email;
            this.friendChatID = myChat;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoChat.Deprecated {
    public class GroupViewModel
    {
        public string groupName { get; set; }
        public string groupID { get; set; }
        public string chatID { get; set; }
        //public string groupFounder { get; set; }
        public string groupFounderID { get; set; }
        public List<string> participants { get; set; }
        
        public GroupViewModel(string name , string gid , string cid ,  string founderid , List<string> groupMembers)
        {
            this.groupName = name;
            this.groupID = gid;
            this.chatID = cid;
            //this.groupFounder = founder;
            this.groupFounderID = founderid;
            this.participants = groupMembers;
        }
    }
}

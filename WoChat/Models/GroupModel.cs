using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoChat.Models
{
    class GroupModel
    {
        private string gid;
        private string gname;
        private InfoModel ginfo;
        private List<string> members;
        private string groupChatID;
        private string admin;

        public string getID()
        {
            return gid;
        }

        public string getName()
        {
            return gname;
        }

        public InfoModel getInfo()
        {
            return ginfo;
        }

        public List<string> getMembers()
        {
            return members;
        }

        public string getChatID()
        {
            return groupChatID;
        }

        public bool addMember(string uid)
        {
            this.members.Add(uid);
            return true;
        }
        public bool deleteMember(string uid)
        {
            return this.members.Remove(uid);
        }
        public bool hasMember(string uid)
        {
            return this.members.Contains(uid);
        }
        public bool isAdmin(string uid)
        {
            return uid == this.admin;
        }

        public GroupModel(string uid , string uname , string _gname , string _ccid , string _gicon = "default", string _gstyle = "None Yet!")
        {
            this.admin = uid;
            this.gid = Guid.NewGuid().ToString();
            this.gname = _gname;
            this.ginfo = new InfoModel(_gname, "NOTPERSON", _gicon, _gstyle);
            this.members = new List<string>();
            members.Add(uid);
            this.groupChatID = _ccid;
        }




    }
}

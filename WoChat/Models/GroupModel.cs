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
        private ChatModel groupList;

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

        public ChatModel getChat()
        {
            return groupList;
        }







    }
}

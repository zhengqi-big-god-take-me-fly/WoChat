using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoChat.Models
{
    /**
     * Model of a group
     */
    public class GroupModel
    {

        /**
         * gid , gname , ginfo are the similar meaning of user
         */
        private string gid;
        private string gname;
        private InfoModel ginfo;

        /**
         * The String List of Group Member IDs.
         */
        private List<string> members;

        /**
         * The ID of corresponding Chat
         */
        private string groupChatID;

        /**
         * Judge if is admin(Just Like WeChat)
         * We served the FirstComer as Group Administrator
         */
        private string admin;



        /**
         * Getters for the variables
         */
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


        /**
         * Transfer OwnerShip of the group
         * @param  {[type]} string uid           original admin(must)
         * @param  {[type]} string newuid        new admin(must)
         * @return {[type]} true if transfer Successfully
         */
        public bool transferAdmin(string uid , string newuid) {
            if (isAdmin(uid))
            {
                this.admin = newuid;
                return true;
            }
            return false;
        }

        /**
         * Set the Group Name
         * @param {[type]} string uid     judge if the user is admin
         * @param {[type]} string newName new Group Name(must)
         * @return         true if Change Successfully
         */
        public bool setGroupName(string uid , string newName) {
            if (isAdmin(uid)) {
                this.gname = newName;
                return true;
            }
            return false;
        }

        /**
         * Set the Group icon
         * @param {[type]} string uid     [judge if the user is admin]
         * @param {[type]} string newIcon [new Group Icon(must)]
         * @return         true if Change Successfully;
         */
        public bool setGroupIcon(string uid , string newIcon) {
            if (isAdmin(uid)) {
                ginfo.icon = newIcon;
                return true;
            }
            return false;
        }


        /**
         * Set the Group's Personal Signature
         * @param {[type]} string uid      [judge if the user is admin]
         * @param {[type]} string newStyle [new Group Style]
         * @return         true if Set Successfully
         */
        public bool setGroupStyle(string uid , string newStyle) {
            if (isAdmin(uid)) {
                ginfo.stylish = newStyle;
            }
            return false;
        }

        /**
         * Add a member to the Member ID list
         * return true if no exception occured
         */
        public bool addMember(string uid)
        {
            this.members.Add(uid);
            return true;
        }

        /**
         * Delete a member from the Member ID list
         * return true if removal successful
         */
        public bool deleteMember(string uid)
        {
            return this.members.Remove(uid);
        }

        /**
         * Judge if the group has a member of given user IDs
         * return true contains given user
         */
        public bool hasMember(string uid)
        {
            return this.members.Contains(uid);
        }

        /**
         * Judge if the given user is the group admin
         * return true if he/she is the admin.
         */
        public bool isAdmin(string uid)
        {
            return uid == this.admin;
        }

        public string getAdmin()
        {
            return this.admin;
        }

        public bool setMember(List<string> newMember)
        {
            this.members = newMember;
            return true;
        }

        public bool resetID(string newID)
        {
            this.gid = newID;
            return true;
        }


        /**
         * Constructer:
         *     uid,uname : the id and name for the Group Founder.(No founder no Group).
         *     _gname: the group name
         *     _ccid: the Group Chat id (Must provided by caller)
         *     _gicon: the Group icom
         *     _gstyle: the Group Personal Signature.
         * @type {String}
         */
        public GroupModel(string uid , string uname , string _gname , string _ccid , string _gicon = "default", string _gstyle = "None Yet!")
        {
            this.admin = uid;
            this.gid = Guid.NewGuid().ToString();
            this.gname = _gname;
            /**
             * Since it's Group , we found no emails required;
             * @type {InfoModel}
             */
            this.ginfo = new InfoModel(_gname, "NOTPERSON", _gicon, _gstyle);


            /**
             * initialize Member List and Add the member into the Member List.
             * @type {List}
             */
            this.members = new List<string>();
            members.Add(uid);
            this.groupChatID = _ccid;
        }

    }
}

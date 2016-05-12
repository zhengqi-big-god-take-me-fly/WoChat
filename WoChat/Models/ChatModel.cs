using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WoChat.Models
{
    /**
     * Model of a Chat
     */
    public class ChatModel
    {
        /**
         * Initialize Variables:
         * id of a specified chat
         */
        private string chatid;

        /**
         * chater/chatee's names and ids
         */
        private string chater, chatee;
        private string chaterId, chateeId;

        /**
         * Specified if a Corresponding group
         * (If it's a Group Chat)
         * And it's GroupName and Ids.
         */
        private string groupName;
        private string groupId;
        private Boolean isGroupChat;
        // private List<string> chaterList;

        /**
         * The Message Box of a Specified Chat
         */
        private List<MessageModel> messageList;



        /**
         * Getters for those Variables
         */
        public string getID()
        {
            return this.chatid;
        }

        public string getChaterName()
        {
            /**
             * We won't pass the chater's name when creating a GroupChat
             */
            if (!this.isGroupChat)
            {
                return this.chater;
            } else
            {
                return "It's a Group Chat!";
            }
        }

        public string getChaterID()
        {
            /**
             * We won't pass the chater's id when creating a GroupChat
             */
            if (!this.isGroupChat)
            {
                return this.chaterId;
            } else
            {
                return "It's a Group Chat!";
            }
        }

        public string getChateeName()
        {
            /**
             * We won't pass the chatee's name when creating a GroupChat
             */
            if (!isGroupChat)
            {
                return this.chatee;
            } else
            {
                return "It's a Group Chat!";
            }
        }

        public string getChateeID()
        {
            /**
             * We won't pass the chatee's id when creating a GroupChat
             */
            if (!isGroupChat)
            {
                return this.chateeId;
            } else
            {
                return "It's a Group Chat!";
            }
        }


        /**
         * Getter and Setters of a Group Name
         * Use to Upgrade Again the GroupName When Entering a Chat
         * And we also can visit the Members when using the chat.
         */
        public string getGroupName()
        {
            if (!isGroupChat)
            {
                return "It's NOT a Group Chat!";
            } else
            {
                return this.groupName;
            }
        }

        public bool setGroupName(string name)
        {
            this.groupName = name;
            return true;
        }


        /**
         * Only can get the GroupID if it's a groupChat
         */
        public string getGroupID()
        {
            if (!isGroupChat)
            {
                return "It's NOT a Group Chat!";
            } else
            {
                return this.groupId;
            }
        }

        public bool setGroupID(string gid)
        {
            this.groupId = gid;
            return true;
        }


        /**
         * The getter of the Chat Histories.
         */
        public List<MessageModel> getChat()
        {
           return messageList;
        }




        /**
         * Push a specified Message to the MessageList
         * Specified the Chater and Chatee
         * 
         */
       public Boolean pushMessage(string message , string schater , string schatee)
        {
            if (this.messageList == null)
            {
                this.messageList = new List<MessageModel>();
            }
            // Start pushing process
            MessageModel newMessage = new MessageModel(schater , schatee , message , this.isGroupChat);
            this.messageList.Add(newMessage);
            return true;
        }


        /**
         * Constructer: used for only no chater IDs
         * Needs to Be Modified! Warning!
         * @type {String}
         */
        public ChatModel(string _chater, bool _isGroupChat = false , string _chatee = "NULL")
        {
            this.chatid = Guid.NewGuid().ToString();
            this.chater = _chater;
            this.chatee = _chatee;
            this.isGroupChat = _isGroupChat;
            this.messageList = new List<MessageModel>();
        }
        

        /**
         * Regular Constructor
         * @type {[type]}
         */
        public ChatModel(string _chater, string _chaterid , string _chatee, string _chateeid , bool _isGroupChat = false)
        {
            if (!_isGroupChat)
            {
                this.chatid = Guid.NewGuid().ToString();
                this.chater = _chater;
                this.chatee = _chatee;
                this.chaterId = _chaterid;
                this.chateeId = _chateeid;
                this.isGroupChat = _isGroupChat;
                this.messageList = new List<MessageModel>();
            } else
            {
                this.chatid = Guid.NewGuid().ToString();
                this.isGroupChat = _isGroupChat;
                this.messageList = new List<MessageModel>();
                //this.chaterList = new List<string>();
                //this.chaterList.Add(_chaterid);
            }
        }
    }
}

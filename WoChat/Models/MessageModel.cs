using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoChat.Models
{
    /**
     * The model for the message
     */
    public class MessageModelOld
    {
        /**
         * Initialize Variables:
         * 
         * Sender: The name of the message sender
         * SenderID: the id of the message sender
         * Receiver: The name of the message Receiver
         * ReceiverID: The id of the message Receiver
         *
         * isGroupMessage: a flag variable that indicates the message is a group message or not.
         * (A group Message just contains sender & senderid , while other variables all set to null).
         *
         * content: The message body
         * time: The message create time;
         */
        private string sender;
        private string senderId;
        private string receiver;
        private string receiverId;
        private bool isGroupMessage;
        private string content;
        private string time;
        private string chatid;


        /**
         * Getter for sender
         */
        public string getSender()
        {
            return this.sender;
        }

        public string getSenderID()
        {
            return this.senderId;
        }

        /**
         * getter for receiver
         */
        public string getReceiver()
        {
            return this.receiver;
        }
        public string getReceiverID()
        {
            return this.receiverId;
        }

        /**
         * getter for Content
         */
        public string getContent()
        {
            return this.content;
        }


        public string getChatID()
        {
            return this.chatid;
        }
        public void setChatID(string newChatID)
        {
            this.chatid = newChatID;
        }



        /**
         * getter for time
         */
        public string getTime()
        {
            return this.time;
        }
        public bool setSendingTime(string timeFromDatabase)
        {
            //DateTime times = new DateTime(long.Parse(timeFromDatabase));
            //DateTimeOffset time = new DateTimeOffset(long.Parse(timeFromDatabase) , TimeSpan.Zero);
            this.time = timeFromDatabase;
            return true;
        }

        /**
         * getter for the flag variable
         */
        public bool getFlag()
        {
            return this.isGroupMessage;
        }

        /**
         * Constructor
         *     _sender: sender's name
         *     _receiver: receiver's name
         *     message: the Content
         *     _isGroupMessage: the flag variable
         */
        public MessageModelOld(string _sender , string _receiver , string message , string chatID , bool _isGroupMessage = false)
        {
            //this.sender = _sender;
            this.senderId = _sender;
            //this.receiver = _receiver;
            this.receiverId = _receiver;
            this.content = message;
            this.isGroupMessage = _isGroupMessage;
            this.chatid = chatID;
            /**
             * Auto Generate of time
             * [time description]
             * @type {[type]}
             */
            this.time = DateTimeOffset.Now.ToLocalTime().ToString();
            //setTime();
        }



    }
}

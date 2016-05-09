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
    class MessageModel
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
        private DateTimeOffset time;


        /**
         * Getter for sender
         */
        public string getSender()
        {
            return this.sender;
        }

        /**
         * getter for receiver
         */
        public string getReceiver()
        {
            return this.receiver;
        }

        /**
         * getter for Content
         */
        public string getContent()
        {
            return this.content;
        }


        /**
         * getter for time
         */
        public DateTimeOffset getTime()
        {
            return this.time;
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
        public MessageModel(string _sender , string _receiver , string message , bool _isGroupMessage = false)
        {
            this.sender = _sender;
            this.senderId = DataModel.getFriendByName(_sender).getID();
            this.receiver = _receiver;
            this.receiverId = DataModel.getFriendByName(_receiver).getID();
            this.content = message;
            this.isGroupMessage = _isGroupMessage;
            /**
             * Auto Generate of time
             * [time description]
             * @type {[type]}
             */
            this.time = DateTimeOffset.Now;
        }
    }
}

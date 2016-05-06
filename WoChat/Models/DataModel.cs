using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoChat.Models
{
    class DataModel
    {
        public DataModel() { }
        public static string lookUpForId(string name , string type)
        {
            string result;
            switch (type) {
                case "user":
                    // Look up in userList;
                    break;
                case "group":
                    // Look up in groupList;
                    break;
                case "message":
                    // Look up in messageList;
                    break;
                case "fuck":
                    // Don't know what to append;
                    break;
                default:
                    break;
            }
            return result;

        }
    }
}

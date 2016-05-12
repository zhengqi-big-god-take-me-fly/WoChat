using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoChat.Models
{
    /**
     * Models of all kinds of infos
     */
    public class InfoModel
    {
        /**
         * Nickname
         */
        public string nickname { get; set; }
        /**
         * Signatures
         */
        public string stylish { get; set; }
        /**
         * User Icons(Serialized)
         */
        public string icon { get; set; }
        /**
         * Emails
         */
        public string email { get; set; }


        /**
         * Constructer for Info
         * @type {String}
         */
        public InfoModel(string _nick , string _email , string _icon = "default" , string _style = "None Yet!")
        {
            this.nickname = _nick;
            this.email = _email;
            this.icon = _icon;
            this.stylish = _style;
        }

    }
}

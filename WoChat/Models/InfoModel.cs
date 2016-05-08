using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WoChat.Models
{
    class InfoModel
    {
        //用户呢称
        public string nickname { get; set; }
        //个人签名
        public string stylish { get; set; }
        //用户头像（序列化）
        public string icon { get; set; }
        //邮箱
        public string email { get; set; }

        public InfoModel(string _nick , string _email , string _icon = "default" , string _style = "None Yet!")
        {
            this.nickname = _nick;
            this.email = _email;
            this.icon = _icon;
            this.stylish = _style;
        }

    }
}

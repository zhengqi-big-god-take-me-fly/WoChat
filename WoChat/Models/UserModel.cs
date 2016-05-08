using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace WoChat.Models
{
    class UserModel
    {
        //模型定义： uid：用户名,password：用户密码
        private string uid;
        private string uname;
        private string upassword;
        //Info包括头像、Email、个人简介、呢称（以及其他，可拓展）；
        private InfoModel uinfo;
        //朋友的ID列表
        private List<string> friends;
        //讨论群的ID列表
        private List<string> groups;
        //所有的聊天列表
        private List<string> chats;



        public string getID()
        {
            return uid;
        }
        public string getName()
        {
            return uname;
        }
        public string getPassword()
        {
            return upassword;
        }
        public InfoModel getInfo()
        {
            return uinfo;
        }

        //这也是要跟服务器处理的 这里只是模拟
        private string addChatModel(string participantName , string participantID,  bool isGroup = false) 
        {
            return DataModel.createChatForUser(this.uname, this.uid, participantName, participantID);
        }

        // Add a friend.
        // if type == 0 then is passing a name , or else it's passing a friend's ID
        public Boolean addFriend(string friend , int type)
        {
            string id = friend;
            if (type == 0)
            {
                id = DataModel.lookUpForId(friend, "user");
                this.friends.Add(id);
                this.chats.Add(addChatModel(friend , id));
            } else
            {
                id = DataModel.lookUpForName(friend, "user");
                this.friends.Add(friend);
                this.chats.Add(addChatModel(friend, id));
            }
            return true;
        }




        //针对传入的string encrypt生成对应的密文 ， 加密方法为SHA512
        private string encryptCreator(string encrypt)
        {
            string sha = HashAlgorithmNames.Sha512;
            HashAlgorithmProvider provider = HashAlgorithmProvider.OpenAlgorithm(sha);
            CryptographicHash hashme = provider.CreateHash();
            IBuffer origin = CryptographicBuffer.ConvertStringToBinary(encrypt, BinaryStringEncoding.Utf16BE);
            hashme.Append(origin);
            IBuffer result = hashme.GetValueAndReset();
            string hashcode = CryptographicBuffer.EncodeToBase64String(result);
            return hashcode;
        }
        //构造函数
        public UserModel(string name, string password , string _nick , string _email , string _icon = "default" , string _style = "None Yet!")
        {
            //生成基本信息
            this.uid = Guid.NewGuid().ToString();
            this.uname = name;
            this.upassword = encryptCreator(password);
            this.uinfo = new InfoModel(_nick, _email, _icon, _style);
            //初始化好友和群列表
            this.groups = new List<string>();
            this.friends = new List<string>();
        }
    }
}

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
        private string unickname;
        private string upassword;
        private List<string> friends;
        private List<string> groups;

        // Add a friend.
        // if type == 0 then is passing a name , or else it's passing a friend's ID
        public Boolean addFriend(string friend , int type)
        {
            //return true;
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
        public UserModel(string name, string password)
        {
            this.uid = Guid.NewGuid().ToString(); //生成id
            this.uname = name;
            this.upassword = encryptCreator(password);;
        }
    }
}

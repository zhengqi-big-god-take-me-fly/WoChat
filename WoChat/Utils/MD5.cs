using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;

namespace WoChat.Utils {
    public class MD5 {
        private static CryptographicHash ReusableHash = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Md5).CreateHash();
        public static string Hash(string origin) {
            ReusableHash.Append(CryptographicBuffer.ConvertStringToBinary(origin, BinaryStringEncoding.Utf8));
            return CryptographicBuffer.EncodeToHexString(ReusableHash.GetValueAndReset());
        }
    }
}

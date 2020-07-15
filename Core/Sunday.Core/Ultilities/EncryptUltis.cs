using System;
using System.Security.Cryptography;
using System.Text;

namespace Sunday.Core.Ultilities
{
    public class EncryptUltis
    {
        public static string Sha256Encrypt(string phrase, string salt)
        {
            var saltAndPwd = string.Concat(phrase, salt);
            var encoder = new UTF8Encoding();
            using var sha256Hasher = new SHA256Managed();
            var hashedDataBytes = sha256Hasher.ComputeHash(encoder.GetBytes(saltAndPwd));
            var hashedPwd = string.Concat(ByteArrayToString(hashedDataBytes), salt);
            return hashedPwd;
        }
        public static string ByteArrayToString(byte[] inputArray)
        {
            var output = new StringBuilder("");
            foreach (var t in inputArray)
            {
                output.Append(t.ToString("X2"));
            }
            return output.ToString();
        }
    }
}

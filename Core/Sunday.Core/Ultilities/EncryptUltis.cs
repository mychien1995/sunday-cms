using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Sunday.Core.Ultilities
{
    public class EncryptUltis
    {
        public static string SHA256Encrypt(string phrase, string salt)
        {
            string saltAndPwd = String.Concat(phrase, salt);
            UTF8Encoding encoder = new UTF8Encoding();
            string hashedPwd = string.Empty;
            using (SHA256Managed sha256hasher = new SHA256Managed())
            {
                byte[] hashedDataBytes = sha256hasher.ComputeHash(encoder.GetBytes(saltAndPwd));
                hashedPwd = String.Concat(byteArrayToString(hashedDataBytes), salt);
            }
            return hashedPwd;
        }
        public static string byteArrayToString(byte[] inputArray)
        {
            StringBuilder output = new StringBuilder("");
            for (int i = 0; i < inputArray.Length; i++)
            {
                output.Append(inputArray[i].ToString("X2"));
            }
            return output.ToString();
        }
    }
}

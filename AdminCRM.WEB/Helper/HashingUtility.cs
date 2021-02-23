using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace UPTax.Helper
{
    public static class HashingUtility
    {
        private static byte[] _GetSha1Hash(string inputString)
        {
            HashAlgorithm algorithm = SHA1.Create();  
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }

        public static string GetSha1HashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in _GetSha1Hash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }

        public static string GetMD5HashString(string input)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString();
        }

    }
}
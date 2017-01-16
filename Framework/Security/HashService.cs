using System;
using System.Security.Cryptography;
using System.Text;

namespace Framework.Security
{
    public interface IHashService
    {
        string ComputeHashValue(string content);
    }
    public class HashService : IHashService
    {
        static IHashService s_instance = new HashService();

        private Encoding _encoding = Encoding.ASCII;
        private HashAlgorithm _hashAlgorithm = SHA256.Create();

        private HashService()
        {
        }

        public static IHashService GetInstance()
        {
            return s_instance;
        }

        byte[] StringToBytes(string value)
        {
            return this._encoding.GetBytes(value);
        }

        byte[] ComputeHashByte(byte[] contentByte)
        {
            byte[] hashedByte = this._hashAlgorithm.ComputeHash(contentByte);

            return hashedByte;
        }

        public string ComputeHashValue(string content)
        {
            byte[] contentByte = this.StringToBytes(content);
            byte[] hashByte = this.ComputeHashByte(contentByte);

            string hashValue = Convert.ToBase64String(hashByte);

            return hashValue;
        }
    }
}

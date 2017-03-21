using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AesJsonEncryptor
{
    public class AesJsonEncryptor
    {
        private byte[] _key;
        private byte[] _iv;

        public byte[] Key
        {
            get
            {
                if (_key == null)
                {
                    _key = GenerateRandomBytes(256);
                }

                return _key;
            }
        }

        public byte[] IV {
            get
            {
                if (_iv == null)
                {
                    _iv = GenerateRandomBytes(128);
                }

                return _iv;
            }
        }

        public AesJsonEncryptor(byte[] secretKey = null, byte[] initalizationVector = null)
        {
            _key = secretKey;
            _iv = initalizationVector;
        }

        private static byte[] GenerateRandomBytes(int bits)
        {
            var result = new byte[bits / 8];
            RandomNumberGenerator.Create().GetBytes(result);
            return result;
        }

        public string Encrypt(string stringifiedObject)
        {
            var inputBytes = Encoding.UTF8.GetBytes(stringifiedObject);
            var outputBytes = new byte[inputBytes.Length];

            using (var aes = Aes.Create())
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, aes.CreateEncryptor(Key, IV), CryptoStreamMode.Write))
                using (var inputStream = new MemoryStream(inputBytes))
                {
                    inputStream.CopyTo(cs);
                }

                outputBytes = ms.ToArray();
            }

            return Convert.ToBase64String(outputBytes);
        }

        public string Decrypt(string base64string)
        {
            var cryptBytes = Convert.FromBase64String(base64string);
            byte[] clearBytes = null;

            using (var aes = Aes.Create())
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, aes.CreateDecryptor(Key, IV), CryptoStreamMode.Write))
                {
                    cs.Write(cryptBytes, 0, cryptBytes.Length);
                }

                clearBytes = ms.ToArray();
            }

            return Encoding.UTF8.GetString(clearBytes);
        }
    }
}
